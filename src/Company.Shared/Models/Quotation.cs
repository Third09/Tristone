using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>A price offer to a client before a project starts (database/023_quotations.sql). Numbered QT-0001...</summary>
[Table("quotations")]
public class Quotation
{
    [Column("quotation_id")] public int QuotationId { get; set; }
    [Column("quote_no")] public string QuoteNo { get; set; } = "";
    [Column("client_name")] public string ClientName { get; set; } = "";
    [Column("client_contact")] public string? ClientContact { get; set; }
    [Column("client_email")] public string? ClientEmail { get; set; }
    [Column("project_title")] public string ProjectTitle { get; set; } = "";
    [Column("site_location")] public string? SiteLocation { get; set; }
    /// <summary>One of <see cref="ProjectSectors"/>.</summary>
    [Column("sector")] public string Sector { get; set; } = ProjectSectors.Residential;
    [Column("quote_date")] public DateOnly QuoteDate { get; set; }
    [Column("valid_until")] public DateOnly ValidUntil { get; set; }
    [Column("planned_start")] public DateOnly? PlannedStart { get; set; }
    [Column("discount")] public decimal Discount { get; set; }
    [Column("vat_applies")] public bool VatApplies { get; set; }
    [Column("terms")] public string? Terms { get; set; }
    /// <summary>Internal; never printed.</summary>
    [Column("notes")] public string? Notes { get; set; }
    /// <summary>One of <see cref="QuotationStatuses"/>.</summary>
    [Column("status")] public string Status { get; set; } = QuotationStatuses.Draft;
    [Column("inquiry_id")] public int? InquiryId { get; set; }
    /// <summary>The project made from it when the client accepted.</summary>
    [Column("project_id")] public int? ProjectId { get; set; }
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime? UpdatedAt { get; set; }
    [Column("decided_by")] public int? DecidedBy { get; set; }
    [Column("decided_at")] public DateTime? DecidedAt { get; set; }
}

/// <summary>One line of a quotation: item, quantity, unit, unit price.</summary>
[Table("quotation_items")]
public class QuotationItem
{
    [Column("item_id")] public int ItemId { get; set; }
    [Column("quotation_id")] public int QuotationId { get; set; }
    [Column("line_no")] public int LineNo { get; set; }
    [Column("description")] public string Description { get; set; } = "";
    [Column("quantity")] public decimal Quantity { get; set; }
    [Column("unit")] public string Unit { get; set; } = "";
    [Column("unit_price")] public decimal UnitPrice { get; set; }
}

/// <summary>
/// Draft (being prepared) -> Waiting (sent to the Receipts Supervisor) -> Approved (print and send it) or Rejected
/// (fix it, it goes back to Draft) -> Accepted (the client agreed: a project was made) or Declined.
/// </summary>
public static class QuotationStatuses
{
    public const string Draft = "Draft", Waiting = "Waiting", Approved = "Approved", Rejected = "Rejected",
                        Accepted = "Accepted", Declined = "Declined";
    public static readonly string[] All = [Draft, Waiting, Approved, Rejected, Accepted, Declined];
}
