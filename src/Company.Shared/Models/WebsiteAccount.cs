using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>
/// One account for the Website Admin app (database/008_website_accounts.sql). Separate from the office app's
/// <see cref="User"/> logins (owner decision 2026-10-30). Same statuses and lock-out rules.
/// </summary>
[Table("website_accounts")]
public class WebsiteAccount
{
    [Column("account_id")] public int AccountId { get; set; }
    [Column("username")] public string Username { get; set; } = "";
    [Column("password_hash")] public string PasswordHash { get; set; } = "";
    [Column("full_name")] public string FullName { get; set; } = "";

    /// <summary>One of <see cref="UserStatuses"/> (Active / Locked / Disabled).</summary>
    [Column("status")] public string Status { get; set; } = UserStatuses.Active;

    /// <summary>A main account can manage the other Website Admin accounts (script 009).</summary>
    [Column("is_main")] public bool IsMain { get; set; }

    [Column("failed_login_count")] public int FailedLoginCount { get; set; }
    [Column("last_login_at")] public DateTime? LastLoginAt { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>A sign in / sign out / lock event in the Website Admin app. Table: website_login_logs.</summary>
[Table("website_login_logs")]
public class WebsiteLoginLog
{
    [Column("log_id")] public int LogId { get; set; }

    /// <summary>Null when the typed username doesn't exist.</summary>
    [Column("account_id")] public int? AccountId { get; set; }

    [Column("username_tried")] public string? UsernameTried { get; set; }

    /// <summary>One of <see cref="LoginActions"/> (Login, Logout, Failed Login, Account Locked, Account Unlocked, Password Changed).</summary>
    [Column("action")] public string Action { get; set; } = "";

    [Column("details")] public string? Details { get; set; }
    [Column("computer_name")] public string? ComputerName { get; set; }
    [Column("logged_at")] public DateTime LoggedAt { get; set; } = DateTime.Now;
}
