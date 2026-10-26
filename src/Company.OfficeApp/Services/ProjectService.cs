using System.Globalization;
using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>One row of the Projects table.</summary>
public record ProjectRow(int ProjectId, string ProjectName, string SiteLocation, string? ClientName, string Sector,
                         decimal ContractPrice, DateOnly StartDate, DateOnly? TargetEndDate, string Status, bool ShowOnWebsite);

/// <summary>What the Admin types in the project form.</summary>
public record ProjectInput(string ProjectName, string SiteLocation, string? ClientName, string Sector,
                           decimal ContractPrice, DateOnly StartDate, DateOnly? TargetEndDate, string Status, bool ShowOnWebsite);

/// <summary>Numbers the app works out by itself for one project.</summary>
public record ProjectSummary(string DaysText, decimal Revenue, decimal Expenses, decimal Profit);

/// <summary>
/// Projects page (Admin only, Project Outline Part 1b). The one shared project list: other departments only pick
/// from it. Every change goes to audit_logs.
/// </summary>
public static class ProjectService
{
    public static readonly CultureInfo Peso = CultureInfo.GetCultureInfo("en-PH");

    public static string Money(decimal amount) => amount.ToString("C2", Peso);

    public static async Task<List<ProjectRow>> ListAsync(string search, string? status, string? sector)
    {
        using var db = Db.Open();
        var q = db.Projects.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(p => p.ProjectName.ToLower().Contains(s) || p.SiteLocation.ToLower().Contains(s) || (p.ClientName ?? "").ToLower().Contains(s));
        }
        if (status != null) q = q.Where(p => p.Status == status);
        if (sector != null) q = q.Where(p => p.Sector == sector);
        return await q.OrderByDescending(p => p.StartDate).ThenBy(p => p.ProjectName)
            .Select(p => new ProjectRow(p.ProjectId, p.ProjectName, p.SiteLocation, p.ClientName, p.Sector,
                                        p.ContractPrice, p.StartDate, p.TargetEndDate, p.Status, p.ShowOnWebsite))
            .ToListAsync();
    }

    public static async Task<Project?> GetAsync(int projectId)
    {
        using var db = Db.Open();
        return await db.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.ProjectId == projectId);
    }

    /// <summary>Null when the input is acceptable, otherwise the reason.</summary>
    public static string? CheckInput(ProjectInput p)
    {
        if (string.IsNullOrWhiteSpace(p.ProjectName)) return "Type the project name.";
        if (p.ProjectName.Trim().Length > 150) return "The project name is too long (150 characters at most).";
        if (string.IsNullOrWhiteSpace(p.SiteLocation)) return "Type the site / location.";
        if (p.SiteLocation.Trim().Length > 255) return "The site / location is too long (255 characters at most).";
        if ((p.ClientName?.Trim().Length ?? 0) > 150) return "The client name is too long (150 characters at most).";
        if (!ProjectSectors.All.Contains(p.Sector)) return "Choose a sector.";
        if (!ProjectStatuses.All.Contains(p.Status)) return "Choose a status.";
        if (p.ContractPrice < 0) return "The contract price can't be negative.";
        if (p.TargetEndDate is { } end && end < p.StartDate) return "The target end date can't be before the start date.";
        return null;
    }

    /// <summary>Returns (new id, null) or (null, error).</summary>
    public static async Task<(int? ProjectId, string? Error)> CreateAsync(ProjectInput input)
    {
        if (RequireAdmin() is { } notAdmin) return (null, notAdmin);
        if (CheckInput(input) is { } problem) return (null, problem);

        using var db = Db.Open();
        var p = new Project { CreatedBy = Session.CurrentUser!.UserId, CreatedAt = DateTime.Now };
        Apply(p, input);
        db.Projects.Add(p);
        await db.SaveChangesAsync();

        AuditService.Add(db, null, "projects", p.ProjectId, p.ProjectName, AuditActions.Added, null, Snapshot(p));
        await db.SaveChangesAsync();
        return (p.ProjectId, null);
    }

    public static async Task<string?> UpdateAsync(int projectId, ProjectInput input)
    {
        if (RequireAdmin() is { } notAdmin) return notAdmin;
        if (CheckInput(input) is { } problem) return problem;

        using var db = Db.Open();
        var p = await db.Projects.FirstAsync(x => x.ProjectId == projectId);
        var before = Snapshot(p);
        Apply(p, input);
        AuditService.Add(db, null, "projects", p.ProjectId, p.ProjectName, AuditActions.Edited, before, Snapshot(p));
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>
    /// Deletes a project that nothing uses yet. Once receipts, materials or subcontractor jobs point to it,
    /// deleting is refused; set it to Cancelled instead so its history stays.
    /// </summary>
    public static async Task<string?> DeleteAsync(int projectId)
    {
        if (RequireAdmin() is { } notAdmin) return notAdmin;
        using var db = Db.Open();
        var p = await db.Projects.FirstAsync(x => x.ProjectId == projectId);

        if (await CountLinkedRecordsAsync(projectId) > 0)
            return "This project already has records (receipts, materials or subcontractor jobs). Set its status to Cancelled instead.";

        AuditService.Add(db, null, "projects", p.ProjectId, p.ProjectName, AuditActions.Deleted, Snapshot(p), null);
        db.Projects.Remove(p);
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>
    /// Records in other parts that use this project (material use and subcontractor jobs so far; receipts and
    /// change orders join when built).
    /// </summary>
    public static async Task<int> CountLinkedRecordsAsync(int projectId)
    {
        using var db = Db.Open();
        return await db.MaterialUsages.CountAsync(u => u.ProjectId == projectId)
             + await db.SubJobs.CountAsync(j => j.ProjectId == projectId);
    }

    /// <summary>
    /// Days spent, revenue, expenses to date and profit to date, from <see cref="ProjectFinance"/>
    /// (the same numbers the Dashboard uses).
    /// </summary>
    public static async Task<ProjectSummary> SummarizeAsync(Project p, DateOnly today)
    {
        var money = await ProjectFinance.ForProjectAsync(p.ProjectId);
        return new ProjectSummary(DaysText(p.StartDate, p.CompletedDate, p.Status, today), money.Revenue, money.Expenses, money.Profit);
    }

    public static string DaysText(DateOnly start, DateOnly? completed, string status, DateOnly today)
    {
        if (status == ProjectStatuses.Cancelled) return "Cancelled";
        if (completed is { } done) return $"Finished in {done.DayNumber - start.DayNumber + 1} days";
        if (start > today) return $"Starts in {start.DayNumber - today.DayNumber} days";
        return $"Day {today.DayNumber - start.DayNumber + 1}";
    }

    static void Apply(Project p, ProjectInput i)
    {
        p.ProjectName = i.ProjectName.Trim();
        p.SiteLocation = i.SiteLocation.Trim();
        p.ClientName = string.IsNullOrWhiteSpace(i.ClientName) ? null : i.ClientName.Trim();
        p.Sector = i.Sector;
        p.ContractPrice = Math.Round(i.ContractPrice, 2);
        p.StartDate = i.StartDate;
        p.TargetEndDate = i.TargetEndDate;
        p.ShowOnWebsite = i.ShowOnWebsite;

        // Completion date is kept by the app: set the day a project becomes Done, cleared if it's reopened.
        if (i.Status == ProjectStatuses.Done && p.Status != ProjectStatuses.Done)
            p.CompletedDate = DateOnly.FromDateTime(DateTime.Today) < i.StartDate ? i.StartDate : DateOnly.FromDateTime(DateTime.Today);
        else if (i.Status != ProjectStatuses.Done)
            p.CompletedDate = null;
        p.Status = i.Status;

        p.UpdatedAt = DateTime.Now;
    }

    static object Snapshot(Project p) => new
    {
        project_name = p.ProjectName,
        site_location = p.SiteLocation,
        client_name = p.ClientName,
        sector = p.Sector,
        contract_price = p.ContractPrice,
        start_date = p.StartDate,
        target_end_date = p.TargetEndDate,
        completed_date = p.CompletedDate,
        status = p.Status,
        show_on_website = p.ShowOnWebsite,
    };

    static string? RequireAdmin() =>
        Session.CurrentUser is { IsAdmin: true } ? null : "Only Admins can change projects.";
}
