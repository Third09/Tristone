using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

// Part 4: Subcontractors. Tables: database/006_subcontractors.sql.

[Table("subcontractors")]
public class Subcontractor
{
    [Column("subcontractor_id")] public int SubcontractorId { get; set; }
    [Column("business_name")] public string BusinessName { get; set; } = "";
    [Column("trade_name")] public string? TradeName { get; set; }
    [Column("dti_sec_no")] public string? DtiSecNo { get; set; }
    [Column("tin")] public string? Tin { get; set; }
    [Column("primary_name")] public string? PrimaryName { get; set; }
    [Column("primary_role")] public string? PrimaryRole { get; set; }
    [Column("primary_phone")] public string? PrimaryPhone { get; set; }
    [Column("primary_mobile")] public string? PrimaryMobile { get; set; }
    [Column("primary_email")] public string? PrimaryEmail { get; set; }
    [Column("secondary_name")] public string? SecondaryName { get; set; }
    [Column("secondary_role")] public string? SecondaryRole { get; set; }
    [Column("secondary_phone")] public string? SecondaryPhone { get; set; }
    [Column("secondary_mobile")] public string? SecondaryMobile { get; set; }
    [Column("secondary_email")] public string? SecondaryEmail { get; set; }
    [Column("office_address")] public string? OfficeAddress { get; set; }
    [Column("yard_address")] public string? YardAddress { get; set; }
    [Column("bank_name")] public string? BankName { get; set; }
    [Column("account_name")] public string? AccountName { get; set; }
    [Column("account_number")] public string? AccountNumber { get; set; }
    [Column("pcab_license_no")] public string? PcabLicenseNo { get; set; }
    [Column("pcab_classification")] public string? PcabClassification { get; set; }
    [Column("pcab_expiry")] public DateOnly? PcabExpiry { get; set; }
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

[Table("subcontractor_trades")]
public class SubcontractorTrade
{
    [Column("subcontractor_id")] public int SubcontractorId { get; set; }
    [Column("trade")] public string Trade { get; set; } = "";
}

[Table("sub_jobs")]
public class SubJob
{
    [Column("job_id")] public int JobId { get; set; }
    [Column("subcontractor_id")] public int SubcontractorId { get; set; }
    [Column("project_id")] public int ProjectId { get; set; }
    [Column("scope")] public string Scope { get; set; } = "";
    [Column("contract_amount")] public decimal ContractAmount { get; set; }
    [Column("start_date")] public DateOnly StartDate { get; set; }
    [Column("end_date")] public DateOnly EndDate { get; set; }
    [Column("retention_pct")] public decimal RetentionPct { get; set; } = 10;
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

[Table("sub_job_stages")]
public class SubJobStage
{
    [Column("stage_id")] public int StageId { get; set; }
    [Column("job_id")] public int JobId { get; set; }
    [Column("stage_no")] public int StageNo { get; set; }
    [Column("name")] public string Name { get; set; } = "";
    [Column("percent")] public decimal Percent { get; set; }
    [Column("reached_on")] public DateOnly? ReachedOn { get; set; }
    [Column("reached_by")] public int? ReachedBy { get; set; }
}

[Table("sub_job_documents")]
public class SubJobDocument
{
    [Column("document_id")] public int DocumentId { get; set; }
    [Column("job_id")] public int JobId { get; set; }
    [Column("file_path")] public string FilePath { get; set; } = "";
    [Column("original_name")] public string OriginalName { get; set; } = "";
    [Column("uploaded_by")] public int? UploadedBy { get; set; }
    [Column("uploaded_at")] public DateTime UploadedAt { get; set; } = DateTime.Now;
}

[Table("sub_progress")]
public class SubProgress
{
    [Column("progress_id")] public int ProgressId { get; set; }
    [Column("job_id")] public int JobId { get; set; }
    [Column("percent")] public decimal Percent { get; set; }
    [Column("progress_date")] public DateOnly ProgressDate { get; set; }
    [Column("notes")] public string? Notes { get; set; }
    [Column("file_path")] public string? FilePath { get; set; }
    [Column("original_name")] public string? OriginalName { get; set; }
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("sub_vouchers")]
public class SubVoucher
{
    [Column("voucher_id")] public int VoucherId { get; set; }
    [Column("voucher_no")] public string VoucherNo { get; set; } = "";
    [Column("job_id")] public int JobId { get; set; }
    [Column("stage_id")] public int? StageId { get; set; }
    [Column("kind")] public string Kind { get; set; } = VoucherKinds.Stage;
    [Column("gross_amount")] public decimal GrossAmount { get; set; }
    [Column("retention_amount")] public decimal RetentionAmount { get; set; }
    [Column("payable_amount")] public decimal PayableAmount { get; set; }
    [Column("status")] public string Status { get; set; } = VoucherStatuses.Waiting;
    [Column("prepared_by")] public int? PreparedBy { get; set; }
    [Column("prepared_at")] public DateTime PreparedAt { get; set; } = DateTime.Now;
    [Column("decided_by")] public int? DecidedBy { get; set; }
    [Column("decided_at")] public DateTime? DecidedAt { get; set; }
}

[Table("sub_payouts")]
public class SubPayout
{
    [Column("payout_id")] public int PayoutId { get; set; }
    [Column("voucher_id")] public int VoucherId { get; set; }
    [Column("job_id")] public int JobId { get; set; }
    [Column("paid_on")] public DateOnly PaidOn { get; set; }
    [Column("amount")] public decimal Amount { get; set; }
    [Column("reference_no")] public string? ReferenceNo { get; set; }
    [Column("method")] public string Method { get; set; } = "Bank Transfer";
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public static class VoucherKinds
{
    public const string Stage = "Stage";
    public const string Retention = "Retention";
}

public static class VoucherStatuses
{
    public const string Waiting = "Waiting";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Paid = "Paid";
}

public static class SubLists
{
    public static readonly string[] Trades =
    [
        "Structural Steel", "Electrical Rough-ins & Wiring", "Plumbing & Sanitary", "HVAC", "Waterproofing",
        "Masonry & Tiling", "Roofing", "Heavy Equipment Rental", "Carpentry", "Painting", "Other",
    ];

    public static readonly string[] PayoutMethods = ["Bank Transfer", "Check", "Cash"];

    /// <summary>A PCAB license this many days or less from expiry shows the reminder.</summary>
    public const int PcabWarningDays = 30;
}
