using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

public record CoRow(int ChangeOrderId, string CoNo, string Project, DateOnly RequestedOn, decimal CostChange, int DaysChange, string Status);

public record CoInput(int ProjectId, DateOnly RequestedOn, string Description, decimal CostChange, int DaysChange, string? NewDocPath);

/// <summary>Budget impact preview: the project now, and if this change order is approved.</summary>
public record CoPreview(decimal CurrentPrice, decimal RevisedPrice, DateOnly? CurrentEnd, DateOnly? RevisedEnd);

public record CoPrintData(string CoNo, string Status, DateOnly RequestedOn, string Project, string? Client, string Description,
                          decimal CostChange, int DaysChange, CoPreview Preview, string PreparedBy, string? ApprovedBy);

/// <summary>
/// Receipts > Change Orders (Project Outline, Part 5). The Receipts Employee logs a change order (cost and/or
/// days change, the client's signed paper); an Admin approves it. Then the cost change adds to the project's
/// revenue (<see cref="ProjectFinance"/>) and the days change moves the project's target end date.
/// Numbered CO-001, CO-002... across all projects.
/// </summary>
public static class ChangeOrderService
{
    const string Dept = Departments.Receipts;

    public static async Task<List<CoRow>> ListAsync(int? projectId = null)
    {
        using var db = Db.Open();
        return await (from c in db.ChangeOrders
                      join p in db.Projects on c.ProjectId equals p.ProjectId
                      where projectId == null || c.ProjectId == projectId
                      orderby c.ChangeOrderId descending
                      select new CoRow(c.ChangeOrderId, c.CoNo, p.ProjectName, c.RequestedOn, c.CostChange, c.DaysChange, c.Status)).ToListAsync();
    }

    public static async Task<ChangeOrder?> GetAsync(int id)
    {
        using var db = Db.Open();
        return await db.ChangeOrders.AsNoTracking().FirstOrDefaultAsync(c => c.ChangeOrderId == id);
    }

    /// <summary>The number the next change order will get (shown on a new form).</summary>
    public static async Task<string> NextNumberAsync()
    {
        using var db = Db.Open();
        return await NextNumberAsync(db);
    }

    static async Task<string> NextNumberAsync(AppDbContext db)
    {
        var last = await db.ChangeOrders.OrderByDescending(c => c.ChangeOrderId).Select(c => c.CoNo).FirstOrDefaultAsync();
        return $"CO-{(last == null ? 1 : int.Parse(last[3..]) + 1):000}";
    }

    public static async Task<CoPreview> PreviewAsync(int projectId, decimal costChange, int daysChange)
    {
        using var db = Db.Open();
        var p = await db.Projects.AsNoTracking().FirstAsync(x => x.ProjectId == projectId);
        var approved = await db.ChangeOrders.Where(c => c.ProjectId == projectId && c.Status == ApprovalStatuses.Approved).SumAsync(c => (decimal?)c.CostChange) ?? 0;
        var current = p.ContractPrice + approved;
        return new CoPreview(current, current + costChange, p.TargetEndDate, p.TargetEndDate?.AddDays(daysChange));
    }

    public static string? CheckInput(CoInput i)
    {
        if (i.ProjectId <= 0) return "Choose the project.";
        if (string.IsNullOrWhiteSpace(i.Description)) return "Describe what the client wants changed.";
        if (i.CostChange == 0 && i.DaysChange == 0) return "Type the cost change and/or the days change.";
        if (i.NewDocPath != null && FileStore.Check(i.NewDocPath) is { } bad) return bad;
        return null;
    }

    public static async Task<(int? Id, string? Error)> CreateAsync(CoInput i)
    {
        if (Access.RequirePage(AppPage.Receipts) is { } denied) return (null, denied);
        if (CheckInput(i) is { } problem) return (null, problem);
        using var db = Db.Open();
        var p = await db.Projects.FirstOrDefaultAsync(x => x.ProjectId == i.ProjectId);
        if (p == null || p.Status == ProjectStatuses.Cancelled) return (null, "Choose a project that isn't Cancelled.");
        if ((await PreviewAsync(i.ProjectId, i.CostChange, i.DaysChange)).RevisedPrice < 0) return (null, "The revised contract price can't be below zero.");
        var c = new ChangeOrder { CoNo = await NextNumberAsync(db), CreatedBy = Session.CurrentUser!.UserId };
        await ApplyAsync(c, i);
        db.ChangeOrders.Add(c);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "change_orders", c.ChangeOrderId, $"{c.CoNo} {p.ProjectName}", AuditActions.Added, null, Snapshot(c));
        await db.SaveChangesAsync();
        return (c.ChangeOrderId, null);
    }

    /// <summary>Only while waiting for the Admin.</summary>
    public static async Task<string?> UpdateAsync(int id, CoInput i)
    {
        if (Access.RequirePage(AppPage.Receipts) is { } denied) return denied;
        if (CheckInput(i) is { } problem) return problem;
        using var db = Db.Open();
        var c = await db.ChangeOrders.FirstAsync(x => x.ChangeOrderId == id);
        if (c.Status != ApprovalStatuses.Waiting) return $"This change order is already {c.Status}, so it can't be changed.";
        if ((await PreviewAsync(i.ProjectId, i.CostChange, i.DaysChange)).RevisedPrice < 0) return "The revised contract price can't be below zero.";
        var before = Snapshot(c);
        await ApplyAsync(c, i);
        var name = await db.Projects.Where(p => p.ProjectId == c.ProjectId).Select(p => p.ProjectName).FirstAsync();
        AuditService.Add(db, Dept, "change_orders", c.ChangeOrderId, $"{c.CoNo} {name}", AuditActions.Edited, before, Snapshot(c));
        await db.SaveChangesAsync();
        return null;
    }

    static async Task ApplyAsync(ChangeOrder c, CoInput i)
    {
        c.ProjectId = i.ProjectId; c.RequestedOn = i.RequestedOn; c.Description = i.Description.Trim();
        c.CostChange = i.CostChange; c.DaysChange = i.DaysChange;
        if (i.NewDocPath != null) { c.FilePath = await FileStore.SaveAsync(i.NewDocPath, "change-orders"); c.OriginalName = Path.GetFileName(i.NewDocPath); }
    }

    /// <summary>Admin only. Revenue goes up/down by the cost change; the target end date moves by the days.</summary>
    public static async Task<string?> ApproveAsync(int id)
    {
        if (Session.CurrentUser is not { IsAdmin: true }) return "Only an Admin can approve change orders.";
        using var db = Db.Open();
        var c = await db.ChangeOrders.FirstAsync(x => x.ChangeOrderId == id);
        if (c.Status != ApprovalStatuses.Waiting) return $"This change order is already {c.Status}.";
        var preview = await PreviewAsync(c.ProjectId, c.CostChange, c.DaysChange);
        if (preview.RevisedPrice < 0) return "The revised contract price would be below zero.";
        var p = await db.Projects.FirstAsync(x => x.ProjectId == c.ProjectId);

        c.Status = ApprovalStatuses.Approved;
        c.DecidedBy = Session.CurrentUser.UserId;
        c.DecidedAt = DateTime.Now;
        AuditService.Add(db, Dept, "change_orders", c.ChangeOrderId, $"{c.CoNo} {p.ProjectName}", AuditActions.Edited,
            new { status = ApprovalStatuses.Waiting }, new { status = c.Status, revenue_after = preview.RevisedPrice });
        if (c.DaysChange != 0 && p.TargetEndDate is { } end)
        {
            p.TargetEndDate = end.AddDays(c.DaysChange);
            AuditService.Add(db, null, "projects", p.ProjectId, p.ProjectName, AuditActions.Edited,
                new { target_end_date = end }, new { target_end_date = p.TargetEndDate, by_change_order = c.CoNo });
        }
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> RejectAsync(int id)
    {
        if (Session.CurrentUser is not { IsAdmin: true }) return "Only an Admin can reject change orders.";
        using var db = Db.Open();
        var c = await db.ChangeOrders.FirstAsync(x => x.ChangeOrderId == id);
        if (c.Status != ApprovalStatuses.Waiting) return $"This change order is already {c.Status}.";
        c.Status = ApprovalStatuses.Rejected;
        c.DecidedBy = Session.CurrentUser.UserId;
        c.DecidedAt = DateTime.Now;
        AuditService.Add(db, Dept, "change_orders", c.ChangeOrderId, c.CoNo, AuditActions.Edited, new { status = ApprovalStatuses.Waiting }, new { status = c.Status });
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<CoPrintData> PrintDataAsync(int id)
    {
        using var db = Db.Open();
        var c = await db.ChangeOrders.AsNoTracking().FirstAsync(x => x.ChangeOrderId == id);
        var p = await db.Projects.AsNoTracking().FirstAsync(x => x.ProjectId == c.ProjectId);
        var by = await db.Users.Where(u => u.UserId == c.CreatedBy).Select(u => u.FullName).FirstOrDefaultAsync() ?? "";
        var approvedBy = c.Status == ApprovalStatuses.Approved ? await db.Users.Where(u => u.UserId == c.DecidedBy).Select(u => u.FullName).FirstOrDefaultAsync() : null;
        // For an approved one, "current" is the price before it.
        var preview = await PreviewAsync(c.ProjectId, c.CostChange, c.DaysChange);
        if (c.Status == ApprovalStatuses.Approved)
            preview = new CoPreview(preview.CurrentPrice - c.CostChange, preview.CurrentPrice, preview.CurrentEnd?.AddDays(-c.DaysChange), preview.CurrentEnd);
        return new CoPrintData(c.CoNo, c.Status, c.RequestedOn, p.ProjectName, p.ClientName, c.Description, c.CostChange, c.DaysChange, preview, by, approvedBy);
    }

    /// <summary>Approved change-order totals per project (added to revenue).</summary>
    internal static async Task<Dictionary<int, decimal>> ByProjectAsync(AppDbContext db, List<int> projectIds) =>
        await db.ChangeOrders.Where(c => c.Status == ApprovalStatuses.Approved && projectIds.Contains(c.ProjectId))
            .GroupBy(c => c.ProjectId).Select(g => new { g.Key, Sum = g.Sum(c => c.CostChange) }).ToDictionaryAsync(x => x.Key, x => x.Sum);

    static object Snapshot(ChangeOrder c) => new
    {
        requested_on = c.RequestedOn, description = c.Description, cost_change = c.CostChange, days_change = c.DaysChange,
        signed_document = c.OriginalName, status = c.Status,
    };
}
