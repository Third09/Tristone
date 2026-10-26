using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>A company project. Table: projects (database/002_projects.sql). Shared by every department.</summary>
[Table("projects")]
public class Project
{
    [Column("project_id")] public int ProjectId { get; set; }
    [Column("project_name")] public string ProjectName { get; set; } = "";
    [Column("site_location")] public string SiteLocation { get; set; } = "";
    [Column("client_name")] public string? ClientName { get; set; }

    /// <summary>One of <see cref="ProjectSectors"/>.</summary>
    [Column("sector")] public string Sector { get; set; } = ProjectSectors.Residential;

    /// <summary>Pesos. Revenue = this + approved change orders (once Receipts is built).</summary>
    [Column("contract_price")] public decimal ContractPrice { get; set; }

    [Column("start_date")] public DateOnly StartDate { get; set; }
    [Column("target_end_date")] public DateOnly? TargetEndDate { get; set; }

    /// <summary>Set automatically when the status becomes Done; cleared if it changes back.</summary>
    [Column("completed_date")] public DateOnly? CompletedDate { get; set; }

    /// <summary>One of <see cref="ProjectStatuses"/>.</summary>
    [Column("status")] public string Status { get; set; } = ProjectStatuses.Pending;

    [Column("show_on_website")] public bool ShowOnWebsite { get; set; }
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public static class ProjectStatuses
{
    public const string Pending = "Pending";
    public const string Ongoing = "Ongoing";
    public const string Done = "Done";
    public const string Cancelled = "Cancelled";
    public static readonly string[] All = [Pending, Ongoing, Done, Cancelled];
}

public static class ProjectSectors
{
    public const string Residential = "Residential";
    public const string Commercial = "Commercial";
    public const string Industrial = "Industrial";
    public const string Repairs = "Repairs / Renovations";
    public static readonly string[] All = [Residential, Commercial, Industrial, Repairs];
}
