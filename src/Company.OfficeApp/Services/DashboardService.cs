using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>The four KPI cards. A department view (<see cref="Department"/> set) shows only that department's expenses.</summary>
public record DashboardKpis(int CompletedProjects, decimal GrossRevenue, decimal OperationalExpenses, decimal NetRevenue,
                            ExpenseBreakdown Breakdown, string? Department);

/// <summary>Where the operational expenses come from (shown under the cards).</summary>
public record ExpenseBreakdown(decimal ProjectMaterials, decimal ProjectReceipts, decimal ProjectPayouts,
                               decimal OverheadReceipts, decimal EquipmentRepairs, decimal Payroll)
{
    public decimal Total => ProjectMaterials + ProjectReceipts + ProjectPayouts + OverheadReceipts + EquipmentRepairs + Payroll;
}

public record TopProjectRow(int ProjectId, string Project, string Status, decimal Revenue, decimal Expenses, decimal Profit, decimal? MarginPercent);

public record LogRow(DateTime When, string Name, string Department, string Activity, string Details);

/// <summary>
/// Dashboard numbers (Project Outline, Part 1). Uses the completed-contract method, the usual one for
/// construction: a project's revenue and its expenses count in the period it is finished (status Done,
/// by its completed date). Company overhead (overhead receipts, equipment repairs, payroll) counts by its own
/// date. Net Revenue = Gross Revenue - Operational Expenses.
/// A department view (Supervisors, or an Admin's department filter) shows only that department's expenses:
/// Inventory = material use + equipment repairs, Subcontractors = payouts, Receipts = receipts,
/// HR & Payroll = payroll.
/// </summary>
public static class DashboardService
{
    public const string ThisMonth = "This Month", ThisQuarter = "This Quarter", ThisYear = "This Year", AllTime = "All Time", Custom = "Custom";
    public static readonly string[] Periods = [ThisMonth, ThisQuarter, ThisYear, AllTime, Custom];

    /// <summary>The dates a period covers. All Time = from year 2000 to today.</summary>
    public static (DateOnly From, DateOnly To) Range(string period, DateOnly today, DateOnly customFrom, DateOnly customTo) => period switch
    {
        ThisMonth => (new DateOnly(today.Year, today.Month, 1), today),
        ThisQuarter => (new DateOnly(today.Year, (today.Month - 1) / 3 * 3 + 1, 1), today),
        ThisYear => (new DateOnly(today.Year, 1, 1), today),
        AllTime => (new DateOnly(2000, 1, 1), today),
        _ => (customFrom, customTo),
    };

    /// <summary>The department a user's Dashboard is locked to (null = Admin, all departments).</summary>
    public static string? LockedDepartment(User u) => u.IsAdmin ? null : u.Department;

    public static async Task<DashboardKpis> KpisAsync(DateOnly from, DateOnly to, string? department)
    {
        if (Session.CurrentUser is { } u && LockedDepartment(u) is { } locked) department = locked;   // re-checked here, not just in the UI
        using var db = Db.Open();
        var done = await db.Projects.AsNoTracking()
            .Where(p => p.Status == ProjectStatuses.Done && p.CompletedDate >= from && p.CompletedDate <= to)
            .Select(p => p.ProjectId).ToListAsync();
        var money = done.Count == 0 ? [] : await ProjectFinance.ForProjectsAsync(done);

        var overheadReceipts = await ReceiptService.OverheadTotalAsync(from, to);
        var repairs = await EquipmentService.MaintenanceCostAsync(from, to);
        const decimal payroll = 0;   // HR & Payroll's payroll runs join here when built

        var all = new ExpenseBreakdown(money.Values.Sum(m => m.Materials), money.Values.Sum(m => m.Receipts), money.Values.Sum(m => m.Payouts),
                                       overheadReceipts, repairs, payroll);
        var shown = department switch
        {
            null => all,
            Departments.Inventory => all with { ProjectReceipts = 0, ProjectPayouts = 0, OverheadReceipts = 0, Payroll = 0 },
            Departments.Subcontractors => all with { ProjectMaterials = 0, ProjectReceipts = 0, OverheadReceipts = 0, EquipmentRepairs = 0, Payroll = 0 },
            Departments.Receipts => all with { ProjectMaterials = 0, ProjectPayouts = 0, EquipmentRepairs = 0, Payroll = 0 },
            _ => all with { ProjectMaterials = 0, ProjectReceipts = 0, ProjectPayouts = 0, OverheadReceipts = 0, EquipmentRepairs = 0 },   // HR & Payroll
        };
        var revenue = money.Values.Sum(m => m.Revenue);
        return new DashboardKpis(done.Count, revenue, shown.Total, revenue - shown.Total, shown, department);
    }

    /// <summary>How many projects are in each status right now (Done, Ongoing, Cancelled, Pending).</summary>
    public static async Task<List<(string Status, int Count)>> StatusCountsAsync()
    {
        using var db = Db.Open();
        var counts = await db.Projects.GroupBy(p => p.Status).Select(g => new { g.Key, Count = g.Count() }).ToDictionaryAsync(x => x.Key, x => x.Count);
        string[] order = [ProjectStatuses.Done, ProjectStatuses.Ongoing, ProjectStatuses.Cancelled, ProjectStatuses.Pending];
        return order.Select(s => (s, counts.GetValueOrDefault(s))).ToList();
    }

    /// <summary>
    /// Projects active during the period (not Cancelled; started by its end and not finished before its start),
    /// with revenue, expenses and profit to date, highest profit first. In a department view Expenses is only
    /// that department's share.
    /// </summary>
    public static async Task<List<TopProjectRow>> TopProjectsAsync(DateOnly from, DateOnly to, string? department)
    {
        if (Session.CurrentUser is { } u && LockedDepartment(u) is { } locked) department = locked;
        using var db = Db.Open();
        var projects = await db.Projects.AsNoTracking()
            .Where(p => p.Status != ProjectStatuses.Cancelled && p.StartDate <= to && (p.CompletedDate == null || p.CompletedDate >= from))
            .Select(p => new { p.ProjectId, p.ProjectName, p.Status }).ToListAsync();
        if (projects.Count == 0) return [];
        var money = await ProjectFinance.ForProjectsAsync(projects.Select(p => p.ProjectId).ToList());
        return projects.Select(p =>
        {
            var m = money[p.ProjectId];
            var expenses = department switch
            {
                null => m.Expenses,
                Departments.Inventory => m.Materials,
                Departments.Subcontractors => m.Payouts,
                Departments.Receipts => m.Receipts,
                _ => 0m,
            };
            var profit = m.Revenue - expenses;
            return new TopProjectRow(p.ProjectId, p.ProjectName, p.Status, m.Revenue, expenses, profit,
                m.Revenue == 0 ? null : Math.Round(profit / m.Revenue * 100, 1));
        }).OrderByDescending(r => r.Profit).ToList();
    }

    // ----- Log History (Admin only) -----

    public const string LogAll = "All", LogLogin = "Login / Logout", LogFailed = "Failed Login", LogPassword = "Password Change", LogRequest = "Password Request";
    public static readonly string[] LogTypes = [LogAll, LogLogin, LogFailed, LogPassword, LogRequest];

    static string[] ActionsFor(string type) => type switch
    {
        LogLogin => ["Login", "Logout", "Idle Sign Out"],
        LogFailed => ["Failed Login", "Account Locked", "Account Unlocked"],
        LogPassword => ["Password Changed"],
        LogRequest => ["Password Request"],
        _ => [],
    };

    /// <summary>Sign-ins, sign-outs, failed attempts, locks, password changes and requests (newest first).</summary>
    public static async Task<List<LogRow>> LogHistoryAsync(DateOnly from, DateOnly to, string type, string search, int max = 2000)
    {
        if (Session.CurrentUser is not { IsAdmin: true }) return [];
        using var db = Db.Open();
        var start = from.ToDateTime(TimeOnly.MinValue);
        var end = to.AddDays(1).ToDateTime(TimeOnly.MinValue);
        var actions = ActionsFor(type);
        var q = from l in db.LoginLogs
                join u in db.Users on l.UserId equals u.UserId into us
                from u in us.DefaultIfEmpty()
                where l.LoggedAt >= start && l.LoggedAt < end && (actions.Length == 0 || actions.Contains(l.Action))
                select new { l, Name = u == null ? null : u.FullName, Username = u == null ? l.UsernameTried : u.Username, Dept = u == null ? null : u.Department, Level = u == null ? null : u.Level };
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(x => (x.Name != null && x.Name.ToLower().Contains(s)) || (x.Username != null && x.Username.ToLower().Contains(s)));
        }
        var rows = await q.OrderByDescending(x => x.l.LoggedAt).Take(max).ToListAsync();
        return rows.Select(x => new LogRow(x.l.LoggedAt, x.Name ?? $"(unknown: {x.Username})",
            x.Level == UserLevels.Admin ? "Admin" : x.Dept ?? "", x.l.Action,
            string.Join("  ", new[] { x.l.Details, x.l.ComputerName == null ? null : $"PC: {x.l.ComputerName}" }.Where(d => !string.IsNullOrEmpty(d))))).ToList();
    }
}
