using System.Globalization;
using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>A finished report: what <see cref="TableReport"/> prints and Excel export saves.</summary>
public record ReportData(string Title, string Subtitle, List<ReportColumn> Columns, List<string[]> Rows, decimal Total, int Count)
{
    public string TotalLine => $"Total: {ProjectService.Money(Total)} ({Count} expense{(Count == 1 ? "" : "s")})";
}

/// <summary>
/// Receipts > Reports: Expense & Receipt Report (every approved expense, by project or overhead), and Weekly /
/// Monthly Expense Summaries. Only approved receipts count; stock purchases are left out (materials are
/// counted when used on a project).
/// </summary>
public static class ReceiptReports
{
    public const string Detailed = "Expense & Receipt Report";
    public const string Weekly = "Weekly Expense Summary";
    public const string Monthly = "Monthly Expense Summary";
    public static readonly string[] Types = [Detailed, Weekly, Monthly];

    /// <param name="projectId">null = all, <see cref="ReceiptFilter.OverheadOnly"/> = company overhead, else one project.</param>
    public static async Task<ReportData> BuildAsync(string type, int? projectId, DateOnly fromDate, DateOnly toDate)
    {
        using var db = Db.Open();
        var q = from r in db.Receipts
                join p in db.Projects on r.ProjectId equals p.ProjectId into ps
                from p in ps.DefaultIfEmpty()
                where r.Status == ApprovalStatuses.Approved && r.PurchaseDate >= fromDate && r.PurchaseDate <= toDate
                select new { r, Project = p == null ? ReceiptService.OverheadLabel : p.ProjectName };
        if (projectId == ReceiptFilter.OverheadOnly) q = q.Where(x => x.r.ExpenseType == ExpenseTypes.Overhead);
        else if (projectId is { } pid) q = q.Where(x => x.r.ProjectId == pid);
        var list = await q.OrderBy(x => x.r.PurchaseDate).ThenBy(x => x.r.ReceiptId).ToListAsync();

        var scope = projectId == null ? "All projects and company overhead" : list.FirstOrDefault()?.Project
                    ?? (projectId == ReceiptFilter.OverheadOnly ? ReceiptService.OverheadLabel : await db.Projects.Where(p => p.ProjectId == projectId).Select(p => p.ProjectName).FirstOrDefaultAsync() ?? "");
        var subtitle = $"{scope}  |  {fromDate:MMM d, yyyy} to {toDate:MMM d, yyyy}  |  approved expenses only";
        var total = list.Sum(x => x.r.Amount);
        string M(decimal v) => ProjectService.Money(v);

        if (type == Detailed)
        {
            var rows = list.Select(x => new[] { x.r.PurchaseDate.ToString("MMM d, yyyy"), x.r.ReceiptNo ?? "(none)", x.r.Merchant, x.r.Category, x.Project, M(x.r.Amount) }).ToList();
            return new ReportData(type, subtitle,
                [new("Date", 12), new("Receipt No.", 12), new("Merchant", 22), new("Category", 14), new("Project / Overhead", 24), new("Amount", 16, true)],
                rows, total, list.Count);
        }

        // Summaries: one row per week (starting Monday) or month, split into project expenses and overhead.
        Func<DateOnly, DateOnly> start = type == Weekly
            ? d => d.AddDays(-(((int)d.DayOfWeek + 6) % 7))
            : d => new DateOnly(d.Year, d.Month, 1);
        var groups = list.GroupBy(x => start(x.r.PurchaseDate)).OrderBy(g => g.Key).Select(g => new[]
        {
            type == Weekly ? $"Week of {g.Key:MMM d, yyyy}" : g.Key.ToString("MMMM yyyy", CultureInfo.InvariantCulture),
            g.Count().ToString(),
            M(g.Where(x => x.r.ExpenseType == ExpenseTypes.Project).Sum(x => x.r.Amount)),
            M(g.Where(x => x.r.ExpenseType == ExpenseTypes.Overhead).Sum(x => x.r.Amount)),
            M(g.Sum(x => x.r.Amount)),
        }).ToList();
        return new ReportData(type, subtitle,
            [new(type == Weekly ? "Week" : "Month", 26), new("Receipts", 10, true), new("Project Expenses", 22, true), new("Overhead", 20, true), new("Total", 22, true)],
            groups, total, list.Count);
    }
}
