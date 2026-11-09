using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>A price offer to a client before a project starts (database/023_quotations.sql). Numbered QT-0001...</summary>
[Table("quotations")]
public class Quotation
{
    [Column("quotation_id")] public int QuotationId { get; set; }
    [Column("quote_no")] public string QuoteNo { get; set; } = "";
    [Column("client_name")] public string ClientName { get; set; } = "";
    [Column("client_address")] public string? ClientAddress { get; set; }
    /// <summary>The person the quotation is addressed to ("ATTENTION:").</summary>
    [Column("attention")] public string? Attention { get; set; }
    [Column("client_contact")] public string? ClientContact { get; set; }
    [Column("client_email")] public string? ClientEmail { get; set; }
    [Column("project_title")] public string ProjectTitle { get; set; } = "";
    [Column("site_location")] public string? SiteLocation { get; set; }
    /// <summary>One of <see cref="ProjectSectors"/>.</summary>
    [Column("sector")] public string Sector { get; set; } = ProjectSectors.Residential;
    [Column("quote_date")] public DateOnly QuoteDate { get; set; }
    [Column("valid_until")] public DateOnly ValidUntil { get; set; }
    [Column("planned_start")] public DateOnly? PlannedStart { get; set; }
    /// <summary>The bullets under "Notes:" on the printout, one per line.</summary>
    [Column("printed_notes")] public string? PrintedNotes { get; set; }
    /// <summary>Internal; never printed.</summary>
    [Column("internal_notes")] public string? InternalNotes { get; set; }
    [Column("work_duration")] public string? WorkDuration { get; set; }
    [Column("payment_terms")] public string? PaymentTerms { get; set; }
    /// <summary>"Cash Payable to:" name and number.</summary>
    [Column("payable_to")] public string? PayableTo { get; set; }
    [Column("payable_number")] public string? PayableNumber { get; set; }
    /// <summary>"Very truly yours," name and title (e.g. Contractor).</summary>
    [Column("signatory_name")] public string? SignatoryName { get; set; }
    [Column("signatory_title")] public string? SignatoryTitle { get; set; }
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

/// <summary>One cost line of a quotation (e.g. "Labor Cost" 25,000). The total is their sum.</summary>
[Table("quotation_items")]
public class QuotationItem
{
    [Column("item_id")] public int ItemId { get; set; }
    [Column("quotation_id")] public int QuotationId { get; set; }
    [Column("line_no")] public int LineNo { get; set; }
    [Column("description")] public string Description { get; set; } = "";
    [Column("amount")] public decimal Amount { get; set; }
}

/// <summary>One "Scope of Works" line (printed A, B, C...): the work and its size (database/024).</summary>
[Table("quotation_scope")]
public class QuotationScopeItem
{
    [Column("scope_id")] public int ScopeId { get; set; }
    [Column("quotation_id")] public int QuotationId { get; set; }
    [Column("line_no")] public int LineNo { get; set; }
    [Column("description")] public string Description { get; set; } = "";
    [Column("details")] public string? Details { get; set; }
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
