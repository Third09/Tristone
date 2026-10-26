using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>One payment stage on the Billing & Payouts tab.</summary>
public record StageBillRow(int StageId, int StageNo, string Name, decimal Percent, decimal Amount, decimal Retention, decimal PayableNow,
                           string Status, int? VoucherId, string? VoucherNo)
{
    public bool IsRetention => StageId == 0;
    public string PercentText => IsRetention ? "" : $"{Percent:0.##}%";
}

public record PayoutRow(int PayoutId, DateOnly PaidOn, string For, decimal Amount, string Method, string? ReferenceNo);

/// <summary>
/// A job's billing at a glance: totals, each stage, the retention release (a row with StageId 0, shown once
/// retention is held or released) and the payouts.
/// </summary>
public record JobBilling(decimal ContractAmount, decimal RetentionPct, decimal Paid, decimal RetentionHeld, decimal Remaining,
                         decimal PercentDone, List<StageBillRow> Stages, StageBillRow? RetentionRow, List<PayoutRow> Payouts)
{
    public int? RetentionVoucherId => RetentionRow?.VoucherId;
}

/// <summary>Everything printed on a billing voucher.</summary>
public record VoucherPrintData(string VoucherNo, string Status, DateTime PreparedAt, string PreparedBy, string? ApprovedBy,
                               string Subcontractor, string? BankName, string? AccountName, string? AccountNumber,
                               string Project, string Scope, string For, decimal Gross, decimal Retention, decimal Payable);

/// <summary>
/// Subcontractors > Billing & Payouts (Project Outline, Part 4).
/// Each stage pays (contract x stage %) minus the retention %. The Employee marks a stage reached and prepares
/// the voucher; the Supervisor (or an Admin) approves it; then the payout is recorded (the voucher's exact amount).
/// The retention held is released by its own voucher once the job is 100% done and every stage is paid.
/// Payouts are project expenses (<see cref="ProjectFinance"/>).
/// </summary>
public static class SubBillingService
{
    const string Dept = Departments.Subcontractors;
    public const string NotReached = "Not Reached", Reached = "Reached", VoucherWaiting = "Voucher Waiting";

    /// <summary>
    /// Peso amount of each stage. The last stage takes any rounding centavos, so the stages always add up to the
    /// contract amount exactly.
    /// </summary>
    public static decimal[] StageAmounts(decimal contract, IReadOnlyList<decimal> percents)
    {
        var amounts = percents.Select(p => Math.Round(contract * p / 100, 2)).ToArray();
        if (amounts.Length > 0) amounts[^1] = contract - amounts[..^1].Sum();
        return amounts;
    }

    public static decimal RetentionOf(decimal amount, decimal retentionPct) => Math.Round(amount * retentionPct / 100, 2);

    public static async Task<JobBilling> GetAsync(int jobId)
    {
        using var db = Db.Open();
        var j = await db.SubJobs.AsNoTracking().FirstAsync(x => x.JobId == jobId);
        var stages = await db.SubJobStages.AsNoTracking().Where(s => s.JobId == jobId).OrderBy(s => s.StageNo).ToListAsync();
        var vouchers = await db.SubVouchers.AsNoTracking().Where(v => v.JobId == jobId && v.Status != VoucherStatuses.Rejected).ToListAsync();
        var amounts = StageAmounts(j.ContractAmount, stages.Select(s => s.Percent).ToList());

        var rows = stages.Select((s, n) =>
        {
            var v = vouchers.FirstOrDefault(x => x.StageId == s.StageId);
            var retention = v?.RetentionAmount ?? RetentionOf(amounts[n], j.RetentionPct);
            var status = v?.Status switch
            {
                VoucherStatuses.Waiting => VoucherWaiting,
                VoucherStatuses.Approved => VoucherStatuses.Approved,
                VoucherStatuses.Paid => VoucherStatuses.Paid,
                _ => s.ReachedOn == null ? NotReached : Reached,
            };
            return new StageBillRow(s.StageId, s.StageNo, s.Name, s.Percent, v?.GrossAmount ?? amounts[n], retention,
                (v?.GrossAmount ?? amounts[n]) - retention, status, v?.VoucherId, v?.VoucherNo);
        }).ToList();

        var payouts = await (from p in db.SubPayouts
                             join v in db.SubVouchers on p.VoucherId equals v.VoucherId
                             join s in db.SubJobStages on v.StageId equals s.StageId into ss
                             from s in ss.DefaultIfEmpty()
                             where p.JobId == jobId
                             orderby p.PaidOn, p.PayoutId
                             select new PayoutRow(p.PayoutId, p.PaidOn, s == null ? "Retention release" : s.Name, p.Amount, p.Method, p.ReferenceNo))
                            .ToListAsync();

        var retentionVoucher = vouchers.FirstOrDefault(v => v.Kind == VoucherKinds.Retention);
        var held = vouchers.Where(v => v.Kind == VoucherKinds.Stage && v.Status == VoucherStatuses.Paid).Sum(v => v.RetentionAmount)
                   - (retentionVoucher?.Status == VoucherStatuses.Paid ? retentionVoucher.PayableAmount : 0);
        var paid = payouts.Sum(p => p.Amount);
        var percent = await SubJobService.LatestPercentAsync(jobId);

        StageBillRow? retentionRow = null;
        if (retentionVoucher != null)
            retentionRow = new StageBillRow(0, 0, "Retention release", 0, retentionVoucher.GrossAmount, 0, retentionVoucher.PayableAmount,
                retentionVoucher.Status == VoucherStatuses.Waiting ? VoucherWaiting : retentionVoucher.Status, retentionVoucher.VoucherId, retentionVoucher.VoucherNo);
        else if (held > 0)
            retentionRow = new StageBillRow(0, 0, "Retention release", 0, held, 0, held, "Held", null, null);

        return new JobBilling(j.ContractAmount, j.RetentionPct, paid, held, j.ContractAmount - paid, percent, rows, retentionRow, payouts);
    }

    public static async Task<string?> MarkStageReachedAsync(int stageId, DateOnly reachedOn)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return denied;
        using var db = Db.Open();
        var s = await db.SubJobStages.FirstAsync(x => x.StageId == stageId);
        if (s.ReachedOn != null) return "This stage is already marked as reached.";
        s.ReachedOn = reachedOn;
        s.ReachedBy = Session.CurrentUser!.UserId;
        var j = await db.SubJobs.FirstAsync(x => x.JobId == s.JobId);
        AuditService.Add(db, Dept, "sub_job_stages", s.StageId, $"{await SubJobService.JobNameAsync(db, j)}: {s.Name}", AuditActions.Edited,
            new { reached_on = (DateOnly?)null }, new { reached_on = reachedOn });
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>The Employee prepares the voucher for a reached stage; it waits for the Supervisor.</summary>
    public static async Task<(int? Id, string? Error)> PrepareVoucherAsync(int stageId)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return (null, denied);
        using var db = Db.Open();
        var s = await db.SubJobStages.FirstAsync(x => x.StageId == stageId);
        if (s.ReachedOn == null) return (null, "Mark the stage as reached first.");
        if (await db.SubVouchers.AnyAsync(v => v.StageId == stageId && v.Status != VoucherStatuses.Rejected))
            return (null, "This stage already has a voucher.");
        var j = await db.SubJobs.FirstAsync(x => x.JobId == s.JobId);
        var stages = await db.SubJobStages.Where(x => x.JobId == j.JobId).OrderBy(x => x.StageNo).ToListAsync();
        var gross = StageAmounts(j.ContractAmount, stages.Select(x => x.Percent).ToList())[stages.FindIndex(x => x.StageId == stageId)];
        var retention = RetentionOf(gross, j.RetentionPct);
        return await AddVoucherAsync(db, j, stageId, VoucherKinds.Stage, gross, retention, s.Name);
    }

    /// <summary>
    /// Releases the retention held, right after the job is finished and checked: the job must be 100% done and
    /// every stage paid. Its voucher goes through the same approval.
    /// </summary>
    public static async Task<(int? Id, string? Error)> PrepareRetentionVoucherAsync(int jobId)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return (null, denied);
        var billing = await GetAsync(jobId);
        if (billing.PercentDone < 100) return (null, "The job isn't 100% done yet. Add a progress update first.");
        if (billing.Stages.Any(s => s.Status != VoucherStatuses.Paid)) return (null, "Every stage must be paid before the retention is released.");
        if (billing.RetentionVoucherId != null) return (null, "The retention already has a voucher.");
        if (billing.RetentionHeld <= 0) return (null, "No retention is held on this job.");
        using var db = Db.Open();
        var j = await db.SubJobs.FirstAsync(x => x.JobId == jobId);
        return await AddVoucherAsync(db, j, null, VoucherKinds.Retention, billing.RetentionHeld, 0, "Retention release");
    }

    static async Task<(int? Id, string? Error)> AddVoucherAsync(AppDbContext db, SubJob j, int? stageId, string kind, decimal gross, decimal retention, string label)
    {
        var last = await db.SubVouchers.OrderByDescending(v => v.VoucherId).Select(v => v.VoucherNo).FirstOrDefaultAsync();
        var next = last == null ? 1 : int.Parse(last[3..]) + 1;
        var v = new SubVoucher
        {
            VoucherNo = $"SV-{next:0000}", JobId = j.JobId, StageId = stageId, Kind = kind, GrossAmount = gross,
            RetentionAmount = retention, PayableAmount = gross - retention, PreparedBy = Session.CurrentUser!.UserId,
        };
        db.SubVouchers.Add(v);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "sub_vouchers", v.VoucherId, $"{v.VoucherNo} {await SubJobService.JobNameAsync(db, j)}", AuditActions.Added, null,
            new { @for = label, gross, retention, payable = v.PayableAmount, status = v.Status });
        await db.SaveChangesAsync();
        return (v.VoucherId, null);
    }

    public static Task<string?> ApproveVoucherAsync(int voucherId) => DecideAsync(voucherId, VoucherStatuses.Approved);

    public static Task<string?> RejectVoucherAsync(int voucherId) => DecideAsync(voucherId, VoucherStatuses.Rejected);

    static async Task<string?> DecideAsync(int voucherId, string decision)
    {
        if (Access.RequireApprover(Dept) is { } denied) return denied;
        using var db = Db.Open();
        var v = await db.SubVouchers.FirstAsync(x => x.VoucherId == voucherId);
        if (v.Status != VoucherStatuses.Waiting) return $"This voucher is already {v.Status}.";
        v.Status = decision;
        v.DecidedBy = Session.CurrentUser!.UserId;
        v.DecidedAt = DateTime.Now;
        AuditService.Add(db, Dept, "sub_vouchers", v.VoucherId, v.VoucherNo, AuditActions.Edited,
            new { status = VoucherStatuses.Waiting }, new { status = decision });
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Records the payment of an approved voucher. It counts as the project's expense.</summary>
    public static async Task<string?> RecordPayoutAsync(int voucherId, DateOnly paidOn, decimal amount, string? referenceNo, string method)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return denied;
        if (!SubLists.PayoutMethods.Contains(method)) return "Choose how it was paid.";
        if (method != "Cash" && string.IsNullOrWhiteSpace(referenceNo)) return "Type the bank or check reference number.";
        using var db = Db.Open();
        var v = await db.SubVouchers.FirstAsync(x => x.VoucherId == voucherId);
        if (v.Status != VoucherStatuses.Approved) return v.Status == VoucherStatuses.Paid ? "This voucher is already paid." : "The voucher must be approved first.";
        if (amount != v.PayableAmount) return $"The approved voucher is for {ProjectService.Money(v.PayableAmount)}.";

        var p = new SubPayout
        {
            VoucherId = voucherId, JobId = v.JobId, PaidOn = paidOn, Amount = amount, Method = method,
            ReferenceNo = string.IsNullOrWhiteSpace(referenceNo) ? null : referenceNo.Trim(), CreatedBy = Session.CurrentUser!.UserId,
        };
        db.SubPayouts.Add(p);
        v.Status = VoucherStatuses.Paid;
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "sub_payouts", p.PayoutId, v.VoucherNo, AuditActions.Added, null,
            new { paid_on = paidOn, amount, method, reference_no = p.ReferenceNo });
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<VoucherPrintData> PrintDataAsync(int voucherId)
    {
        using var db = Db.Open();
        var v = await db.SubVouchers.AsNoTracking().FirstAsync(x => x.VoucherId == voucherId);
        var j = await db.SubJobs.AsNoTracking().FirstAsync(x => x.JobId == v.JobId);
        var s = await db.Subcontractors.AsNoTracking().FirstAsync(x => x.SubcontractorId == j.SubcontractorId);
        var project = await db.Projects.Where(p => p.ProjectId == j.ProjectId).Select(p => p.ProjectName).FirstAsync();
        var stage = v.StageId == null ? null : await db.SubJobStages.FirstAsync(x => x.StageId == v.StageId);
        var preparedBy = await db.Users.Where(u => u.UserId == v.PreparedBy).Select(u => u.FullName).FirstOrDefaultAsync() ?? "";
        var approvedBy = v.Status is VoucherStatuses.Approved or VoucherStatuses.Paid
            ? await db.Users.Where(u => u.UserId == v.DecidedBy).Select(u => u.FullName).FirstOrDefaultAsync() : null;
        return new VoucherPrintData(v.VoucherNo, v.Status, v.PreparedAt, preparedBy, approvedBy, s.BusinessName, s.BankName, s.AccountName,
            s.AccountNumber, project, j.Scope, stage == null ? "Retention release" : $"{stage.Name} ({stage.Percent:0.##}%)",
            v.GrossAmount, v.RetentionAmount, v.PayableAmount);
    }

    /// <summary>Subcontractor payouts per project (project expenses).</summary>
    internal static async Task<Dictionary<int, decimal>> PayoutsByProjectAsync(AppDbContext db, List<int> projectIds) =>
        await (from p in db.SubPayouts
               join j in db.SubJobs on p.JobId equals j.JobId
               where projectIds.Contains(j.ProjectId)
               group p by j.ProjectId into g
               select new { g.Key, Sum = g.Sum(x => x.Amount) }).ToDictionaryAsync(x => x.Key, x => x.Sum);
}
