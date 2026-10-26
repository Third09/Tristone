using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>
/// One project's money, from every part that records it.
/// Revenue = contract price + approved change orders. Expenses = approved material use + approved project
/// receipts (not stock purchases, which are counted when the material is used) + subcontractor payouts.
/// Payroll and equipment repairs are company-wide overhead and are NOT here.
/// </summary>
public record ProjectMoney(decimal ContractPrice, decimal ChangeOrders, decimal Materials, decimal Receipts, decimal Payouts)
{
    public decimal Revenue => ContractPrice + ChangeOrders;
    public decimal Expenses => Materials + Receipts + Payouts;
    public decimal Profit => Revenue - Expenses;
    public decimal? MarginPercent => Revenue == 0 ? null : Math.Round(Profit / Revenue * 100, 1);
}

/// <summary>
/// The one place project revenue and expenses are added up, so the Projects page, the project report and the
/// Dashboard always agree. Each part adds its own totals here as it is built.
/// </summary>
public static class ProjectFinance
{
    /// <summary>Money for the given projects (all projects when null).</summary>
    public static async Task<Dictionary<int, ProjectMoney>> ForProjectsAsync(IReadOnlyCollection<int>? projectIds = null)
    {
        using var db = Db.Open();
        var projects = db.Projects.AsNoTracking();
        if (projectIds != null) projects = projects.Where(p => projectIds.Contains(p.ProjectId));
        var prices = await projects.Select(p => new { p.ProjectId, p.ContractPrice }).ToListAsync();
        var ids = prices.Select(p => p.ProjectId).ToList();

        var materials = await db.MaterialUsages
            .Where(u => u.Status == ApprovalStatuses.Approved && ids.Contains(u.ProjectId))
            .GroupBy(u => u.ProjectId).Select(g => new { Id = g.Key, Sum = g.Sum(u => u.Cost ?? 0) })
            .ToDictionaryAsync(x => x.Id, x => x.Sum);

        var payouts = await SubBillingService.PayoutsByProjectAsync(db, ids);

        // Receipts and change orders (Part 5) join here when built.
        return prices.ToDictionary(p => p.ProjectId, p => new ProjectMoney(
            p.ContractPrice,
            ChangeOrders: 0,
            Materials: materials.GetValueOrDefault(p.ProjectId),
            Receipts: 0,
            Payouts: payouts.GetValueOrDefault(p.ProjectId)));
    }

    public static async Task<ProjectMoney> ForProjectAsync(int projectId) =>
        (await ForProjectsAsync([projectId])).GetValueOrDefault(projectId) ?? new ProjectMoney(0, 0, 0, 0, 0);
}
