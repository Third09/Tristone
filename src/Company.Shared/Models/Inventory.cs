using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

// Part 3: Inventory & Vendors. Tables: database/005_inventory.sql.

/// <summary>A setting every PC shares (one row per key). Table: app_settings.</summary>
[Table("app_settings")]
public class AppSetting
{
    [Column("key")] public string Key { get; set; } = "";
    [Column("value")] public string Value { get; set; } = "";
    [Column("updated_by")] public int? UpdatedBy { get; set; }
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

[Table("materials")]
public class Material
{
    [Column("material_id")] public int MaterialId { get; set; }
    [Column("name")] public string Name { get; set; } = "";
    [Column("category")] public string Category { get; set; } = "";
    [Column("unit")] public string Unit { get; set; } = "";
    [Column("unit_price")] public decimal UnitPrice { get; set; }
    [Column("stock_qty")] public decimal StockQty { get; set; }
    [Column("minimum_stock")] public decimal MinimumStock { get; set; }
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [NotMapped] public bool IsOutOfStock => StockQty <= 0;
    [NotMapped] public bool IsLowStock => StockQty < MinimumStock;
}

[Table("material_usages")]
public class MaterialUsage
{
    [Column("usage_id")] public int UsageId { get; set; }
    [Column("material_id")] public int MaterialId { get; set; }
    [Column("project_id")] public int ProjectId { get; set; }
    [Column("quantity")] public decimal Quantity { get; set; }
    [Column("used_on")] public DateOnly UsedOn { get; set; }
    [Column("notes")] public string? Notes { get; set; }
    [Column("status")] public string Status { get; set; } = ApprovalStatuses.Waiting;
    [Column("cost")] public decimal? Cost { get; set; }
    [Column("requested_by")] public int? RequestedBy { get; set; }
    [Column("decided_by")] public int? DecidedBy { get; set; }
    [Column("decided_at")] public DateTime? DecidedAt { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("vendors")]
public class Vendor
{
    [Column("vendor_id")] public int VendorId { get; set; }
    [Column("name")] public string Name { get; set; } = "";
    [Column("contact_person")] public string? ContactPerson { get; set; }
    [Column("phone")] public string? Phone { get; set; }
    [Column("email")] public string? Email { get; set; }
    [Column("address")] public string? Address { get; set; }
    [Column("bank_name")] public string? BankName { get; set; }
    [Column("account_name")] public string? AccountName { get; set; }
    [Column("account_number")] public string? AccountNumber { get; set; }
    [Column("vat_status")] public string VatStatus { get; set; } = "VAT";
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

[Table("vendor_documents")]
public class VendorDocument
{
    [Column("document_id")] public int DocumentId { get; set; }
    [Column("vendor_id")] public int VendorId { get; set; }
    [Column("doc_type")] public string DocType { get; set; } = "Other";
    [Column("file_path")] public string FilePath { get; set; } = "";
    [Column("original_name")] public string OriginalName { get; set; } = "";
    [Column("uploaded_by")] public int? UploadedBy { get; set; }
    [Column("uploaded_at")] public DateTime UploadedAt { get; set; } = DateTime.Now;
}

[Table("quotes")]
public class Quote
{
    [Column("quote_id")] public int QuoteId { get; set; }
    [Column("material_id")] public int MaterialId { get; set; }
    [Column("vendor_id")] public int VendorId { get; set; }
    [Column("quoted_on")] public DateOnly QuotedOn { get; set; }
    [Column("product_name")] public string ProductName { get; set; } = "";
    [Column("description")] public string? Description { get; set; }
    [Column("category")] public string? Category { get; set; }
    [Column("unit_price")] public decimal UnitPrice { get; set; }
    [Column("quantity")] public decimal Quantity { get; set; }
    [Column("status")] public string Status { get; set; } = "Open";
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("purchases")]
public class Purchase
{
    [Column("purchase_id")] public int PurchaseId { get; set; }
    [Column("quote_id")] public int QuoteId { get; set; }
    [Column("material_id")] public int MaterialId { get; set; }
    [Column("vendor_id")] public int VendorId { get; set; }
    [Column("quantity")] public decimal Quantity { get; set; }
    [Column("unit_price")] public decimal UnitPrice { get; set; }
    [Column("approved_by")] public int? ApprovedBy { get; set; }
    [Column("approved_at")] public DateTime ApprovedAt { get; set; } = DateTime.Now;
}

[Table("deliveries")]
public class Delivery
{
    [Column("delivery_id")] public int DeliveryId { get; set; }
    [Column("purchase_id")] public int? PurchaseId { get; set; }
    [Column("vendor_id")] public int VendorId { get; set; }
    [Column("material_id")] public int MaterialId { get; set; }
    [Column("quantity")] public decimal Quantity { get; set; }
    [Column("unit_price")] public decimal UnitPrice { get; set; }
    [Column("delivered_on")] public DateOnly DeliveredOn { get; set; }
    [Column("receipt_no")] public string ReceiptNo { get; set; } = "";
    [Column("receipt_file_path")] public string? ReceiptFilePath { get; set; }
    [Column("physical_only")] public bool PhysicalOnly { get; set; }
    [Column("filed_location")] public string? FiledLocation { get; set; }
    [Column("recorded_by")] public int? RecordedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("equipment")]
public class Equipment
{
    [Column("equipment_id")] public int EquipmentId { get; set; }
    [Column("name")] public string Name { get; set; } = "";
    [Column("type")] public string Type { get; set; } = "Tool";
    [Column("ownership")] public string Ownership { get; set; } = "Owned";
    [Column("rental_company")] public string? RentalCompany { get; set; }
    [Column("rental_rate")] public decimal? RentalRate { get; set; }
    [Column("rental_rate_unit")] public string? RentalRateUnit { get; set; }
    [Column("return_date")] public DateOnly? ReturnDate { get; set; }
    [Column("status")] public string Status { get; set; } = "Available";
    [Column("next_maintenance")] public DateOnly? NextMaintenance { get; set; }
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

[Table("maintenance_logs")]
public class MaintenanceLog
{
    [Column("log_id")] public int LogId { get; set; }
    [Column("equipment_id")] public int EquipmentId { get; set; }
    [Column("done_on")] public DateOnly DoneOn { get; set; }
    [Column("kind")] public string Kind { get; set; } = "Preventive";
    [Column("work_done")] public string WorkDone { get; set; } = "";
    [Column("cost")] public decimal Cost { get; set; }
    [Column("done_by")] public string? DoneBy { get; set; }
    [Column("recorded_by")] public int? RecordedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Waiting / Approved / Rejected: used by every "Employee records, Supervisor approves" record.</summary>
public static class ApprovalStatuses
{
    public const string Waiting = "Waiting";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
}

public static class InventoryLists
{
    public static readonly string[] MaterialCategories = ["Cement", "Aggregates", "Steel", "Wood", "Electrical", "Plumbing", "Paint", "Hardware", "Other"];
    public static readonly string[] Units = ["Bag", "Piece", "Meter", "Kilo", "Box", "Liter", "Sheet", "Cubic Meter", "Set", "Roll"];
    public static readonly string[] EquipmentTypes = ["Tool", "Heavy Equipment", "Vehicle"];
    public static readonly string[] EquipmentStatuses = ["Available", "Under Repair", "Retired"];
    public static readonly string[] RateUnits = ["Day", "Week", "Month"];
    public static readonly string[] MaintenanceKinds = ["Preventive", "Repair"];
    public static readonly string[] VendorDocTypes = ["Company Profile", "Bank Details", "Certificate of Registration", "Other"];
    public static readonly string[] VatStatuses = ["VAT", "Non-VAT"];
}
