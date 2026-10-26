using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>A Forgot Password request from an Employee or Supervisor. Table: password_requests.</summary>
[Table("password_requests")]
public class PasswordRequest
{
    [Column("request_id")] public int RequestId { get; set; }
    [Column("user_id")] public int UserId { get; set; }
    [Column("requested_at")] public DateTime RequestedAt { get; set; } = DateTime.Now;

    /// <summary>One of <see cref="PasswordRequestStatuses"/>. Waiting requests drive the Admin badge.</summary>
    [Column("status")] public string Status { get; set; } = PasswordRequestStatuses.Waiting;

    /// <summary>The Admin who handled it.</summary>
    [Column("handled_by")] public int? HandledBy { get; set; }
    [Column("handled_at")] public DateTime? HandledAt { get; set; }
}

public static class PasswordRequestStatuses
{
    public const string Waiting = "Waiting";
    public const string Done = "Done";
    public const string Dismissed = "Dismissed";
}
