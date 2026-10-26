using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

// Part 5: Receipts & change orders. Tables: database/007_receipts.sql.

[Table("receipts")]
public class Receipt
{
    [Column("receipt_id")] public int ReceiptId { get; set; }
    [Column("receipt_no")] public string? ReceiptNo { get; set; }
    [Column("purchase_date")] public DateOnly PurchaseDate { get; set; }
    [Column("merchant")] public string Merchant { get; set; } = "";
    [Column("amount")] public decimal Amount { get; set; }
    [Column("category")] public string Category { get; set; } = "";

    /// <summary>One of <see cref="ExpenseTypes"/>.</summary>
    [Column("expense_type")] public string ExpenseType { get; set; } = ExpenseTypes.Project;
    [Column("project_id")] public int? ProjectId { get; set; }
    [Column("file_path")] public string? FilePath { get; set; }
    [Column("original_name")] public string? OriginalName { get; set; }
    [Column("physical_only")] public bool PhysicalOnly { get; set; }
    [Column("filed_location")] public string? FiledLocation { get; set; }
    [Column("no_receipt")] public bool NoReceipt { get; set; }
    [Column("no_receipt_reason")] public string? NoReceiptReason { get; set; }
    [Column("status")] public string Status { get; set; } = ApprovalStatuses.Waiting;
    [Column("uploaded_by")] public int? UploadedBy { get; set; }
    [Column("uploaded_at")] public DateTime UploadedAt { get; set; } = DateTime.Now;
    [Column("decided_by")] public int? DecidedBy { get; set; }
    [Column("decided_at")] public DateTime? DecidedAt { get; set; }
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

[Table("change_orders")]
public class ChangeOrder
{
    [Column("change_order_id")] public int ChangeOrderId { get; set; }
    [Column("co_no")] public string CoNo { get; set; } = "";
    [Column("project_id")] public int ProjectId { get; set; }
    [Column("requested_on")] public DateOnly RequestedOn { get; set; }
    [Column("description")] public string Description { get; set; } = "";
    [Column("cost_change")] public decimal CostChange { get; set; }
    [Column("days_change")] public int DaysChange { get; set; }
    [Column("file_path")] public string? FilePath { get; set; }
    [Column("original_name")] public string? OriginalName { get; set; }
    [Column("status")] public string Status { get; set; } = ApprovalStatuses.Waiting;
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("decided_by")] public int? DecidedBy { get; set; }
    [Column("decided_at")] public DateTime? DecidedAt { get; set; }
}

public static class ExpenseTypes
{
    public const string Project = "Project";
    public const string Overhead = "Overhead";
}

public static class ReceiptLists
{
    public static readonly string[] Categories =
        ["Tools", "Hardware", "Meals", "Permits", "Materials", "Transport", "Utilities", "Rent", "Office Supplies", "Equipment Repairs", "Other"];
}
