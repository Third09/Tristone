using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

public record MaterialRow(int MaterialId, string Name, string Category, string Unit, decimal UnitPrice,
                          decimal StockQty, decimal MinimumStock)
{
    public bool IsOutOfStock => StockQty <= 0;
    public bool IsLowStock => StockQty < MinimumStock;
    public override string ToString() => $"{Name} ({Unit})";
}

public record MaterialInput(string Name, string Category, string Unit, decimal UnitPrice, decimal StartingStock, decimal MinimumStock);

public record UsageRow(int UsageId, DateOnly UsedOn, string Project, string Material, decimal Quantity, string Unit,
                       decimal Cost, string RequestedBy, string Status, string? Notes);

public record UsageInput(int ProjectId, int MaterialId, decimal Quantity, DateOnly UsedOn, string? Notes);

/// <summary>A project in a dropdown.</summary>
public record ProjectOption(int ProjectId, string Name)
{
    public override string ToString() => Name;
}

/// <summary>
/// Inventory > Materials and Material Usage (Project Outline, Part 3).
/// Stock only changes through approved usage (down) and deliveries (up), never by typing it in.
/// Usage: the Inventory Employee records it; the Inventory Supervisor (or an Admin) approves; then the stock
/// goes down and quantity x the material's current price becomes that project's material expense.
/// </summary>
public static class MaterialService
{
    const string Dept = Departments.Inventory;

    // ----- materials -----

    public static async Task<List<MaterialRow>> ListAsync(string search = "", string? category = null, bool lowOnly = false)
    {
        using var db = Db.Open();
        var q = db.Materials.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) { var s = search.Trim().ToLower(); q = q.Where(m => m.Name.ToLower().Contains(s)); }
        if (category != null) q = q.Where(m => m.Category == category);
        if (lowOnly) q = q.Where(m => m.StockQty < m.MinimumStock);
        return await q.OrderBy(m => m.Name)
            .Select(m => new MaterialRow(m.MaterialId, m.Name, m.Category, m.Unit, m.UnitPrice, m.StockQty, m.MinimumStock))
            .ToListAsync();
    }

    public static async Task<int> LowStockCountAsync()
    {
        using var db = Db.Open();
        return await db.Materials.CountAsync(m => m.StockQty < m.MinimumStock);
    }

    public static string? CheckInput(MaterialInput i)
    {
        if (string.IsNullOrWhiteSpace(i.Name)) return "Type the material name.";
        if (i.Name.Trim().Length > 120) return "The name is too long (120 characters at most).";
        if (string.IsNullOrWhiteSpace(i.Category)) return "Choose a category.";
        if (string.IsNullOrWhiteSpace(i.Unit)) return "Choose a unit.";
        if (i.UnitPrice < 0 || i.StartingStock < 0 || i.MinimumStock < 0) return "Price and quantities can't be negative.";
        return null;
    }

    public static async Task<(int? Id, string? Error)> CreateAsync(MaterialInput i)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return (null, denied);
        if (CheckInput(i) is { } problem) return (null, problem);
        using var db = Db.Open();
        if (await db.Materials.AnyAsync(m => m.Name.ToLower() == i.Name.Trim().ToLower())) return (null, "A material with that name already exists.");

        var m = new Material
        {
            Name = i.Name.Trim(), Category = i.Category.Trim(), Unit = i.Unit.Trim(), UnitPrice = i.UnitPrice,
            StockQty = i.StartingStock, MinimumStock = i.MinimumStock, CreatedBy = Session.CurrentUser!.UserId,
        };
        db.Materials.Add(m);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "materials", m.MaterialId, m.Name, AuditActions.Added, null, Snapshot(m));
        await db.SaveChangesAsync();
        return (m.MaterialId, null);
    }

    /// <summary>Edits name, category, unit, price and minimum. The stock itself is not edited here.</summary>
    public static async Task<string?> UpdateAsync(int materialId, MaterialInput i)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return denied;
        if (CheckInput(i) is { } problem) return problem;
        using var db = Db.Open();
        var m = await db.Materials.FirstAsync(x => x.MaterialId == materialId);
        if (await db.Materials.AnyAsync(x => x.Name.ToLower() == i.Name.Trim().ToLower() && x.MaterialId != materialId))
            return "A material with that name already exists.";

        var before = Snapshot(m);
        m.Name = i.Name.Trim(); m.Category = i.Category.Trim(); m.Unit = i.Unit.Trim();
        m.UnitPrice = i.UnitPrice; m.MinimumStock = i.MinimumStock; m.UpdatedAt = DateTime.Now;
        AuditService.Add(db, Dept, "materials", m.MaterialId, m.Name, AuditActions.Edited, before, Snapshot(m));
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> DeleteAsync(int materialId)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return denied;
        using var db = Db.Open();
        var m = await db.Materials.FirstAsync(x => x.MaterialId == materialId);
        if (await db.MaterialUsages.AnyAsync(u => u.MaterialId == materialId) || await db.Quotes.AnyAsync(q => q.MaterialId == materialId)
            || await db.Deliveries.AnyAsync(d => d.MaterialId == materialId))
            return "This material already has usage, quotes or deliveries recorded, so it can't be deleted.";
        AuditService.Add(db, Dept, "materials", m.MaterialId, m.Name, AuditActions.Deleted, Snapshot(m), null);
        db.Materials.Remove(m);
        await db.SaveChangesAsync();
        return null;
    }

    static object Snapshot(Material m) => new
    {
        name = m.Name, category = m.Category, unit = m.Unit, unit_price = m.UnitPrice,
        stock_qty = m.StockQty, minimum_stock = m.MinimumStock,
    };

    // ----- usage -----

    /// <summary>Projects materials can be used on (not Cancelled).</summary>
    public static async Task<List<ProjectOption>> ProjectOptionsAsync()
    {
        using var db = Db.Open();
        return await db.Projects.AsNoTracking().Where(p => p.Status != ProjectStatuses.Cancelled)
            .OrderBy(p => p.ProjectName).Select(p => new ProjectOption(p.ProjectId, p.ProjectName)).ToListAsync();
    }

    public static async Task<(int? Id, string? Error)> SubmitUsageAsync(UsageInput i)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return (null, denied);
        if (i.Quantity <= 0) return (null, "Type how many were used.");
        using var db = Db.Open();
        var m = await db.Materials.FirstOrDefaultAsync(x => x.MaterialId == i.MaterialId);
        var p = await db.Projects.FirstOrDefaultAsync(x => x.ProjectId == i.ProjectId);
        if (m == null || p == null) return (null, "Choose the project and the material.");
        if (p.Status == ProjectStatuses.Cancelled) return (null, "That project is Cancelled.");
        if (i.Quantity > m.StockQty) return (null, $"Only {m.StockQty:0.##} {m.Unit} in stock.");

        var u = new MaterialUsage
        {
            MaterialId = i.MaterialId, ProjectId = i.ProjectId, Quantity = i.Quantity, UsedOn = i.UsedOn,
            Notes = string.IsNullOrWhiteSpace(i.Notes) ? null : i.Notes.Trim(), RequestedBy = Session.CurrentUser!.UserId,
        };
        db.MaterialUsages.Add(u);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "material_usages", u.UsageId, $"{m.Name} -> {p.ProjectName}", AuditActions.Added, null,
            new { project = p.ProjectName, material = m.Name, quantity = i.Quantity, used_on = i.UsedOn, status = u.Status });
        await db.SaveChangesAsync();
        return (u.UsageId, null);
    }

    public static async Task<List<UsageRow>> ListUsageAsync(string? status, int? projectId = null)
    {
        using var db = Db.Open();
        var q = from u in db.MaterialUsages
                join m in db.Materials on u.MaterialId equals m.MaterialId
                join p in db.Projects on u.ProjectId equals p.ProjectId
                join r in db.Users on u.RequestedBy equals r.UserId into rs
                from r in rs.DefaultIfEmpty()
                where (status == null || u.Status == status) && (projectId == null || u.ProjectId == projectId)
                orderby u.UsedOn descending, u.UsageId descending
                select new UsageRow(u.UsageId, u.UsedOn, p.ProjectName, m.Name, u.Quantity, m.Unit,
                    u.Cost ?? u.Quantity * m.UnitPrice, r == null ? "" : r.FullName, u.Status, u.Notes);
        return await q.ToListAsync();
    }

    /// <summary>Approves: stock goes down, the cost is fixed at today's price and counts for the project.</summary>
    public static async Task<string?> ApproveUsageAsync(int usageId)
    {
        if (Access.RequireApprover(Dept) is { } denied) return denied;
        using var db = Db.Open();
        var u = await db.MaterialUsages.FirstAsync(x => x.UsageId == usageId);
        if (u.Status != ApprovalStatuses.Waiting) return $"This request is already {u.Status}.";
        var m = await db.Materials.FirstAsync(x => x.MaterialId == u.MaterialId);
        if (u.Quantity > m.StockQty) return $"Not enough stock: {m.StockQty:0.##} {m.Unit} left, {u.Quantity:0.##} requested.";

        m.StockQty -= u.Quantity;
        m.UpdatedAt = DateTime.Now;
        u.Cost = Math.Round(u.Quantity * m.UnitPrice, 2);
        u.Status = ApprovalStatuses.Approved;
        u.DecidedBy = Session.CurrentUser!.UserId;
        u.DecidedAt = DateTime.Now;
        AuditService.Add(db, Dept, "material_usages", u.UsageId, m.Name, AuditActions.Edited,
            new { status = ApprovalStatuses.Waiting }, new { status = u.Status, cost = u.Cost, stock_after = m.StockQty });
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> RejectUsageAsync(int usageId)
    {
        if (Access.RequireApprover(Dept) is { } denied) return denied;
        using var db = Db.Open();
        var u = await db.MaterialUsages.FirstAsync(x => x.UsageId == usageId);
        if (u.Status != ApprovalStatuses.Waiting) return $"This request is already {u.Status}.";
        u.Status = ApprovalStatuses.Rejected;
        u.DecidedBy = Session.CurrentUser!.UserId;
        u.DecidedAt = DateTime.Now;
        var name = await db.Materials.Where(m => m.MaterialId == u.MaterialId).Select(m => m.Name).FirstAsync();
        AuditService.Add(db, Dept, "material_usages", u.UsageId, name, AuditActions.Edited,
            new { status = ApprovalStatuses.Waiting }, new { status = u.Status });
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Approved material use on one project, and the total (that project's material expense).</summary>
    public static async Task<(List<UsageRow> Rows, decimal Total)> ProjectHistoryAsync(int projectId)
    {
        var rows = await ListUsageAsync(ApprovalStatuses.Approved, projectId);
        return (rows, rows.Sum(r => r.Cost));
    }
}
