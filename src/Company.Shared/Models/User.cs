using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>One login for an office staff member. Table: users (database/001_login_security.sql).</summary>
[Table("users")]
public class User
{
    [Column("user_id")] public int UserId { get; set; }
    [Column("employee_id")] public int? EmployeeId { get; set; }
    [Column("username")] public string Username { get; set; } = "";
    [Column("password_hash")] public string PasswordHash { get; set; } = "";
    [Column("full_name")] public string FullName { get; set; } = "";

    /// <summary>One of <see cref="UserLevels"/>.</summary>
    [Column("level")] public string Level { get; set; } = UserLevels.Employee;

    /// <summary>One of <see cref="Departments"/>; null for Admins.</summary>
    [Column("department")] public string? Department { get; set; }

    /// <summary>One of <see cref="UserStatuses"/>.</summary>
    [Column("status")] public string Status { get; set; } = UserStatuses.Active;

    [Column("failed_login_count")] public int FailedLoginCount { get; set; }
    [Column("security_question")] public string? SecurityQuestion { get; set; }
    [Column("security_answer_hash")] public string? SecurityAnswerHash { get; set; }
    [Column("last_login_at")] public DateTime? LastLoginAt { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [NotMapped] public bool IsAdmin => Level == UserLevels.Admin;

    /// <summary>A Website Admin login opens the Website Admin app, not the office app (script 011).</summary>
    [NotMapped] public bool IsWebsiteAdmin => Level == UserLevels.WebsiteAdmin;

    /// <summary>May open the Website Admin app: Website Admins and office Admins.</summary>
    [NotMapped] public bool CanOpenWebsiteAdmin => IsWebsiteAdmin || IsAdmin;
}

public static class UserLevels
{
    public const string Admin = "Admin";
    public const string Supervisor = "Supervisor";
    public const string Employee = "Employee";
    public const string WebsiteAdmin = "Website Admin";

    /// <summary>Admins and Website Admins have no department (script 011).</summary>
    public static bool HasNoDepartment(string level) => level is Admin or WebsiteAdmin;
}

public static class UserStatuses
{
    public const string Active = "Active";
    public const string Locked = "Locked";
    public const string Disabled = "Disabled";
}

public static class Departments
{
    public const string HrPayroll = "HR & Payroll";
    public const string Inventory = "Inventory";
    public const string Subcontractors = "Subcontractors";
    public const string Receipts = "Receipts";
    public static readonly string[] All = [HrPayroll, Inventory, Subcontractors, Receipts];
}
