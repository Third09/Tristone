using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

public record CashAdvanceRow(int AdvanceId, int EmployeeId, string Name, DateOnly GivenOn, decimal Amount, decimal PerPayday, decimal Balance,
                             string Status, string? Reason);

public record CashAdvanceInput(int EmployeeId, decimal Amount, decimal PerPayday, DateOnly GivenOn, string? Reason);

public record DeductionRow(DateOnly PayDate, string Period, decimal Deducted, decimal BalanceAfter);

/// <summary>An employee in the cash advance dropdown.</summary>
public record PersonOption(int EmployeeId, string Name)
{
    public override string ToString() => Name;
}

/// <summary>
/// HR & Payroll > Cash Advances (vale), Project Outline Part 2: the HR Employee records the amount, date, reason and
/// how much to deduct per payday; the HR Supervisor (or an Admin) approves. Payroll then takes that amount every
/// payday until it is paid off; when a payslip is too small the rest carries over, and pay never goes negative.
/// </summary>
public static class CashAdvanceService
{
    const string Dept = Departments.HrPayroll;
    public const string FilterAll = "All", FilterWaiting = "Waiting Approval";
    public static readonly string[] Filters = [FilterAll, FilterWaiting, CashAdvanceStatuses.Active, CashAdvanceStatuses.PaidOff, CashAdvanceStatuses.Rejected];

    public static async Task<List<PersonOption>> EmployeeOptionsAsync()
    {
        using var db = Db.Open();
        return await db.Employees.AsNoTracking().Where(e => e.Status == EmployeeStatuses.Active).OrderBy(e => e.FullName)
            .Select(e => new PersonOption(e.EmployeeId, e.FullName)).ToListAsync();
    }

    public static async Task<List<CashAdvanceRow>> ListAsync(string filter)
    {
        var status = filter == FilterWaiting ? CashAdvanceStatuses.Waiting : filter == FilterAll ? null : filter;
        using var db = Db.Open();
        return await (from c in db.CashAdvances
                      join e in db.Employees on c.EmployeeId equals e.EmployeeId
                      where status == null || c.Status == status
                      orderby c.Status == CashAdvanceStatuses.Waiting descending, c.GivenOn descending, c.AdvanceId descending
                      select new CashAdvanceRow(c.AdvanceId, c.EmployeeId, e.FullName, c.GivenOn, c.Amount, c.PerPayday, c.Balance, c.Status, c.Reason))
                     .ToListAsync();
    }

    public static string? CheckInput(CashAdvanceInput i)
    {
        if (i.EmployeeId <= 0) return "Choose the employee.";
        if (i.Amount <= 0) return "Type the amount given.";
        if (i.PerPayday <= 0) return "Type how much to deduct each payday.";
        if (i.PerPayday > i.Amount) return "The deduction per payday can't be more than the amount.";
        if (string.IsNullOrWhiteSpace(i.Reason)) return "Type the reason.";
        return null;
    }

    /// <summary>Saved as Waiting until the HR Supervisor approves.</summary>
    public static async Task<(int? Id, string? Error)> CreateAsync(CashAdvanceInput i)
    {
        if (Access.RequirePage(AppPage.HrPayroll) is { } denied) return (null, denied);
        if (CheckInput(i) is { } problem) return (null, problem);
        using var db = Db.Open();
        var e = await db.Employees.FirstOrDefaultAsync(x => x.EmployeeId == i.EmployeeId);
        if (e == null || e.Status != EmployeeStatuses.Active) return (null, "Choose an active employee.");
        var c = new CashAdvance
        {
            EmployeeId = i.EmployeeId, Amount = i.Amount, PerPayday = i.PerPayday, Balance = i.Amount, GivenOn = i.GivenOn,
            Reason = i.Reason!.Trim(), CreatedBy = Session.CurrentUser!.UserId,
        };
        db.CashAdvances.Add(c);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "cash_advances", c.AdvanceId, $"{e.FullName} {c.GivenOn:MMM d, yyyy}", AuditActions.Added, null,
            new { amount = c.Amount, per_payday = c.PerPayday, given_on = c.GivenOn, reason = c.Reason, status = c.Status });
        await db.SaveChangesAsync();
        return (c.AdvanceId, null);
    }

    public static Task<string?> ApproveAsync(int advanceId) => DecideAsync(advanceId, CashAdvanceStatuses.Active);

    public static Task<string?> RejectAsync(int advanceId) => DecideAsync(advanceId, CashAdvanceStatuses.Rejected);

    static async Task<string?> DecideAsync(int advanceId, string decision)
    {
        if (Access.RequireApprover(Dept) is { } denied) return denied;
        using var db = Db.Open();
        var c = await db.CashAdvances.FirstAsync(x => x.AdvanceId == advanceId);
        if (c.Status != CashAdvanceStatuses.Waiting) return $"This cash advance is already {c.Status}.";
        c.Status = decision;
        c.DecidedBy = Session.CurrentUser!.UserId;
        c.DecidedAt = DateTime.Now;
        var name = await db.Employees.Where(e => e.EmployeeId == c.EmployeeId).Select(e => e.FullName).FirstAsync();
        AuditService.Add(db, Dept, "cash_advances", c.AdvanceId, $"{name} {c.GivenOn:MMM d, yyyy}", AuditActions.Edited,
            new { status = CashAdvanceStatuses.Waiting }, new { status = decision });
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Every payday that took something from this advance.</summary>
    public static async Task<List<DeductionRow>> DeductionsAsync(int advanceId)
    {
        using var db = Db.Open();
        var rows = await (from d in db.CashAdvanceDeductions
                          join p in db.Payslips on d.PayslipId equals p.PayslipId
                          join r in db.PayrollRuns on p.RunId equals r.RunId
                          where d.AdvanceId == advanceId
                          orderby r.PayDate
                          select new { r.PayDate, r.PeriodFrom, r.PeriodTo, d.Amount, d.BalanceAfter }).ToListAsync();
        return rows.Select(x => new DeductionRow(x.PayDate, $"{x.PeriodFrom:MMM d} - {x.PeriodTo:MMM d}", x.Amount, x.BalanceAfter)).ToList();
    }
}
