using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

public record EquipmentRow(int EquipmentId, string Name, string Type, string Ownership, string Status, DateOnly? NextMaintenance)
{
    public bool MaintenanceDue => Status != "Retired" && NextMaintenance is { } d && d <= DateOnly.FromDateTime(DateTime.Today);
}

public record EquipmentInput(string Name, string Type, string Ownership, string? RentalCompany, decimal? RentalRate,
                             string? RentalRateUnit, DateOnly? ReturnDate, string Status, DateOnly? NextMaintenance);

public record MaintenanceRow(int LogId, DateOnly DoneOn, string Kind, string WorkDone, decimal Cost, string? DoneBy);

public record MaintenanceInput(DateOnly DoneOn, string Kind, string WorkDone, decimal Cost, string? DoneBy, DateOnly? NextDue);

/// <summary>
/// Inventory > Equipment and its maintenance & repair log (Project Outline, Part 3). Location is not tracked.
/// A reminder shows when the next maintenance date has come. Repair and maintenance costs are company-wide
/// overhead, not a project expense.
/// </summary>
public static class EquipmentService
{
    const string Dept = Departments.Inventory;

    public static async Task<List<EquipmentRow>> ListAsync(string search = "", string? type = null, string? status = null)
    {
        using var db = Db.Open();
        var q = db.Equipment.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) { var s = search.Trim().ToLower(); q = q.Where(e => e.Name.ToLower().Contains(s)); }
        if (type != null) q = q.Where(e => e.Type == type);
        if (status != null) q = q.Where(e => e.Status == status);
        return await q.OrderBy(e => e.Name)
            .Select(e => new EquipmentRow(e.EquipmentId, e.Name, e.Type, e.Ownership, e.Status, e.NextMaintenance)).ToListAsync();
    }

    public static async Task<Equipment?> GetAsync(int equipmentId)
    {
        using var db = Db.Open();
        return await db.Equipment.AsNoTracking().FirstOrDefaultAsync(e => e.EquipmentId == equipmentId);
    }

    public static async Task<int> MaintenanceDueCountAsync()
    {
        using var db = Db.Open();
        var today = DateOnly.FromDateTime(DateTime.Today);
        return await db.Equipment.CountAsync(e => e.Status != "Retired" && e.NextMaintenance != null && e.NextMaintenance <= today);
    }

    public static string? CheckInput(EquipmentInput i)
    {
        if (string.IsNullOrWhiteSpace(i.Name)) return "Type the equipment name.";
        if (!InventoryLists.EquipmentTypes.Contains(i.Type)) return "Choose the type.";
        if (!InventoryLists.EquipmentStatuses.Contains(i.Status)) return "Choose the status.";
        if (i.Ownership == "Rented")
        {
            if (string.IsNullOrWhiteSpace(i.RentalCompany)) return "Type the rental company.";
            if (i.RentalRate is not > 0) return "Type the rental rate.";
            if (!InventoryLists.RateUnits.Contains(i.RentalRateUnit)) return "Choose per day, week or month.";
        }
        else if (i.Ownership != "Owned") return "Choose Owned or Rented.";
        return null;
    }

    public static async Task<(int? Id, string? Error)> CreateAsync(EquipmentInput i)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return (null, denied);
        if (CheckInput(i) is { } problem) return (null, problem);
        using var db = Db.Open();
        var e = new Equipment { CreatedBy = Session.CurrentUser!.UserId };
        Apply(e, i);
        db.Equipment.Add(e);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "equipment", e.EquipmentId, e.Name, AuditActions.Added, null, Snapshot(e));
        await db.SaveChangesAsync();
        return (e.EquipmentId, null);
    }

    public static async Task<string?> UpdateAsync(int equipmentId, EquipmentInput i)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return denied;
        if (CheckInput(i) is { } problem) return problem;
        using var db = Db.Open();
        var e = await db.Equipment.FirstAsync(x => x.EquipmentId == equipmentId);
        var before = Snapshot(e);
        Apply(e, i);
        AuditService.Add(db, Dept, "equipment", e.EquipmentId, e.Name, AuditActions.Edited, before, Snapshot(e));
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Only equipment with no maintenance history; otherwise set it to Retired.</summary>
    public static async Task<string?> DeleteAsync(int equipmentId)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return denied;
        using var db = Db.Open();
        var e = await db.Equipment.FirstAsync(x => x.EquipmentId == equipmentId);
        if (await db.MaintenanceLogs.AnyAsync(l => l.EquipmentId == equipmentId))
            return "This equipment has maintenance history. Set its status to Retired instead, to keep the history.";
        AuditService.Add(db, Dept, "equipment", e.EquipmentId, e.Name, AuditActions.Deleted, Snapshot(e), null);
        db.Equipment.Remove(e);
        await db.SaveChangesAsync();
        return null;
    }

    static void Apply(Equipment e, EquipmentInput i)
    {
        e.Name = i.Name.Trim(); e.Type = i.Type; e.Ownership = i.Ownership; e.Status = i.Status; e.NextMaintenance = i.NextMaintenance;
        bool rented = i.Ownership == "Rented";
        e.RentalCompany = rented ? i.RentalCompany!.Trim() : null;
        e.RentalRate = rented ? i.RentalRate : null;
        e.RentalRateUnit = rented ? i.RentalRateUnit : null;
        e.ReturnDate = rented ? i.ReturnDate : null;
        e.UpdatedAt = DateTime.Now;
    }

    static object Snapshot(Equipment e) => new
    {
        name = e.Name, type = e.Type, ownership = e.Ownership, rental_company = e.RentalCompany, rental_rate = e.RentalRate,
        rental_rate_unit = e.RentalRateUnit, return_date = e.ReturnDate, status = e.Status, next_maintenance = e.NextMaintenance,
    };

    // ----- maintenance log -----

    public static async Task<List<MaintenanceRow>> ListLogAsync(int equipmentId)
    {
        using var db = Db.Open();
        return await db.MaintenanceLogs.AsNoTracking().Where(l => l.EquipmentId == equipmentId)
            .OrderByDescending(l => l.DoneOn).ThenByDescending(l => l.LogId)
            .Select(l => new MaintenanceRow(l.LogId, l.DoneOn, l.Kind, l.WorkDone, l.Cost, l.DoneBy)).ToListAsync();
    }

    public static string? CheckLog(MaintenanceInput i)
    {
        if (!InventoryLists.MaintenanceKinds.Contains(i.Kind)) return "Choose Preventive or Repair.";
        if (string.IsNullOrWhiteSpace(i.WorkDone)) return "Type what was done.";
        if (i.WorkDone.Trim().Length > 500) return "The description is too long (500 characters at most).";
        if (i.Cost < 0) return "The cost can't be negative.";
        if (i.NextDue is { } next && next < i.DoneOn) return "The next maintenance date can't be before this work.";
        return null;
    }

    /// <summary>Adds an entry; if a next maintenance date is given, the equipment's reminder moves to it.</summary>
    public static async Task<(int? Id, string? Error)> AddLogAsync(int equipmentId, MaintenanceInput i)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return (null, denied);
        if (CheckLog(i) is { } problem) return (null, problem);
        using var db = Db.Open();
        var e = await db.Equipment.FirstAsync(x => x.EquipmentId == equipmentId);
        var l = new MaintenanceLog
        {
            EquipmentId = equipmentId, DoneOn = i.DoneOn, Kind = i.Kind, WorkDone = i.WorkDone.Trim(), Cost = i.Cost,
            DoneBy = string.IsNullOrWhiteSpace(i.DoneBy) ? null : i.DoneBy.Trim(), RecordedBy = Session.CurrentUser!.UserId,
        };
        db.MaintenanceLogs.Add(l);
        var oldNext = e.NextMaintenance;
        if (i.NextDue != null) { e.NextMaintenance = i.NextDue; e.UpdatedAt = DateTime.Now; }
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "maintenance_logs", l.LogId, e.Name, AuditActions.Added, new { next_maintenance = oldNext },
            new { kind = l.Kind, work_done = l.WorkDone, cost = l.Cost, done_on = l.DoneOn, next_maintenance = e.NextMaintenance });
        await db.SaveChangesAsync();
        return (l.LogId, null);
    }

    public static async Task<string?> DeleteLogAsync(int logId)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return denied;
        using var db = Db.Open();
        var l = await db.MaintenanceLogs.FirstAsync(x => x.LogId == logId);
        var name = await db.Equipment.Where(e => e.EquipmentId == l.EquipmentId).Select(e => e.Name).FirstAsync();
        AuditService.Add(db, Dept, "maintenance_logs", l.LogId, name, AuditActions.Deleted,
            new { kind = l.Kind, work_done = l.WorkDone, cost = l.Cost, done_on = l.DoneOn }, null);
        db.MaintenanceLogs.Remove(l);
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>All maintenance and repair costs in a period (company-wide overhead, for the Dashboard).</summary>
    public static async Task<decimal> MaintenanceCostAsync(DateOnly from, DateOnly to)
    {
        using var db = Db.Open();
        return await db.MaintenanceLogs.Where(l => l.DoneOn >= from && l.DoneOn <= to).SumAsync(l => (decimal?)l.Cost) ?? 0;
    }
}
