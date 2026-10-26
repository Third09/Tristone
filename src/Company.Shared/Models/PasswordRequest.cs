using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>A Forgot Password request from an Employee or Supervisor. Table: password_requests.</summary>
[Table("password_requests")]
public class PasswordRequest
{
    [Column("request_id")] public int RequestId { get; set; }
    [Column("user_id")] public int UserId { get; set; }
    [Column("requested_at")] public DateTime RequestedAt { get; set; } = DateTime.Now;

    /// <summary>Waiting, Done or Dismissed. Waiting requests drive the Admin badge.</summary>
    [Column("status")] public string Status { get; set; } = "Waiting";

    /// <summary>The Admin who handled it.</summary>
    [Column("handled_by")] public int? HandledBy { get; set; }
    [Column("handled_at")] public DateTime? HandledAt { get; set; }
}
