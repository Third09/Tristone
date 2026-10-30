using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>
/// A message from the website Contact form (database/010_inquiries.sql). Followed up in the Website Admin app:
/// New -> Contacted -> Closed, with internal notes.
/// </summary>
[Table("inquiries")]
public class Inquiry
{
    [Column("inquiry_id")] public int InquiryId { get; set; }
    [Column("received_at")] public DateTime ReceivedAt { get; set; } = DateTime.Now;
    [Column("full_name")] public string FullName { get; set; } = "";
    [Column("email")] public string? Email { get; set; }
    [Column("phone")] public string? Phone { get; set; }
    [Column("project_location")] public string? ProjectLocation { get; set; }
    [Column("service_required")] public string? ServiceRequired { get; set; }
    [Column("message")] public string Message { get; set; } = "";

    /// <summary>One of <see cref="InquiryStatuses"/>.</summary>
    [Column("status")] public string Status { get; set; } = InquiryStatuses.New;

    /// <summary>Internal follow-up notes; never shown on the website.</summary>
    [Column("notes")] public string? Notes { get; set; }

    /// <summary>The Website Admin account that last changed the status or notes.</summary>
    [Column("updated_by")] public int? UpdatedBy { get; set; }
    [Column("updated_at")] public DateTime? UpdatedAt { get; set; }
}

public static class InquiryStatuses
{
    public const string New = "New";
    public const string Contacted = "Contacted";
    public const string Closed = "Closed";
    public static readonly string[] All = [New, Contacted, Closed];
}
