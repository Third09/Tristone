using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>A sign in / sign out / password event. Table: login_logs. Shown in Dashboard > Log History.</summary>
[Table("login_logs")]
public class LoginLog
{
    [Column("log_id")] public int LogId { get; set; }

    /// <summary>Null when the typed username doesn't exist.</summary>
    [Column("user_id")] public int? UserId { get; set; }

    [Column("username_tried")] public string? UsernameTried { get; set; }

    /// <summary>One of <see cref="LoginActions"/>.</summary>
    [Column("action")] public string Action { get; set; } = "";

    [Column("details")] public string? Details { get; set; }
    [Column("computer_name")] public string? ComputerName { get; set; }
    [Column("logged_at")] public DateTime LoggedAt { get; set; } = DateTime.Now;
}

public static class LoginActions
{
    public const string Login = "Login";
    public const string Logout = "Logout";
    public const string IdleSignOut = "Idle Sign Out";
    public const string FailedLogin = "Failed Login";
    public const string AccountLocked = "Account Locked";
    public const string AccountUnlocked = "Account Unlocked";
    public const string PasswordChanged = "Password Changed";
    public const string PasswordRequest = "Password Request";
}
