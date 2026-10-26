using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

public record VendorRow(int VendorId, string Name, string? ContactPerson, string? Phone, string VatStatus)
{
    public override string ToString() => Name;
}

public record VendorInput(string Name, string? ContactPerson, string? Phone, string? Email, string? Address,
                          string? BankName, string? AccountName, string? AccountNumber, string VatStatus);

public record VendorDocRow(int DocumentId, string DocType, string OriginalName, DateTime UploadedAt, string FilePath);

public record QuoteRow(int QuoteId, string Vendor, DateOnly QuotedOn, string ProductName, string? Description,
                       decimal UnitPrice, decimal Quantity, string Status)
{
    public decimal Total => UnitPrice * Quantity;
}

public record QuoteInput(int MaterialId, int VendorId, DateOnly QuotedOn, string ProductName, string? Description,
                         string? Category, decimal UnitPrice, decimal Quantity);

public record PurchaseRow(int PurchaseId, DateTime ApprovedAt, int VendorId, string Vendor, int MaterialId, string Material,
                          decimal Quantity, decimal UnitPrice, decimal DeliveredQty)
{
    public decimal Total => Quantity * UnitPrice;
    public bool Delivered => DeliveredQty >= Quantity;
    public override string ToString() => $"#{PurchaseId} {Material} from {Vendor} ({Quantity:0.##} @ {ProjectService.Money(UnitPrice)})";
}

public record DeliveryInput(int? PurchaseId, int VendorId, int MaterialId, decimal Quantity, decimal UnitPrice, DateOnly DeliveredOn,
                            string ReceiptNo, string? ScanSourcePath, bool PhysicalOnly, string? FiledLocation);

public record DeliveryRow(int DeliveryId, DateOnly DeliveredOn, string Vendor, string Material, decimal Quantity, decimal Total,
                          string ReceiptNo, bool PhysicalOnly, string? ReceiptFilePath)
{
    public string ReceiptCopy => PhysicalOnly ? "Paper" : "Scan";
}

/// <summary>
/// Inventory > Vendors, Price Comparison and Deliveries (Project Outline, Part 3).
/// Price comparison: the Employee enters quotes from several vendors for the same material; the Supervisor
/// (or an Admin) picks one, which becomes the approved purchase. Deliveries need no approval: stock goes up,
/// the material's price becomes the delivered price, and the receipt (scan or paper) is kept with it.
/// </summary>
public static class PurchasingService
{
    const string Dept = Departments.Inventory;

    // ----- vendors -----

    public static async Task<List<VendorRow>> ListVendorsAsync(string search = "")
    {
        using var db = Db.Open();
        var q = db.Vendors.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) { var s = search.Trim().ToLower(); q = q.Where(v => v.Name.ToLower().Contains(s)); }
        return await q.OrderBy(v => v.Name).Select(v => new VendorRow(v.VendorId, v.Name, v.ContactPerson, v.Phone, v.VatStatus)).ToListAsync();
    }

    public static async Task<Vendor?> GetVendorAsync(int vendorId)
    {
        using var db = Db.Open();
        return await db.Vendors.AsNoTracking().FirstOrDefaultAsync(v => v.VendorId == vendorId);
    }

    public static string? CheckVendor(VendorInput i)
    {
        if (string.IsNullOrWhiteSpace(i.Name)) return "Type the vendor name.";
        if (i.Name.Trim().Length > 150) return "The name is too long (150 characters at most).";
        if (!InventoryLists.VatStatuses.Contains(i.VatStatus)) return "Choose VAT or Non-VAT.";
        if (!string.IsNullOrWhiteSpace(i.Email) && !i.Email.Contains('@')) return "That email doesn't look right.";
        return null;
    }

    public static async Task<(int? Id, string? Error)> CreateVendorAsync(VendorInput i)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return (null, denied);
        if (CheckVendor(i) is { } problem) return (null, problem);
        using var db = Db.Open();
        if (await db.Vendors.AnyAsync(v => v.Name.ToLower() == i.Name.Trim().ToLower())) return (null, "A vendor with that name already exists.");
        var v = new Vendor { CreatedBy = Session.CurrentUser!.UserId };
        Apply(v, i);
        db.Vendors.Add(v);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "vendors", v.VendorId, v.Name, AuditActions.Added, null, Snapshot(v));
        await db.SaveChangesAsync();
        return (v.VendorId, null);
    }

    public static async Task<string?> UpdateVendorAsync(int vendorId, VendorInput i)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return denied;
        if (CheckVendor(i) is { } problem) return problem;
        using var db = Db.Open();
        var v = await db.Vendors.FirstAsync(x => x.VendorId == vendorId);
        if (await db.Vendors.AnyAsync(x => x.Name.ToLower() == i.Name.Trim().ToLower() && x.VendorId != vendorId)) return "A vendor with that name already exists.";
        var before = Snapshot(v);
        Apply(v, i);
        AuditService.Add(db, Dept, "vendors", v.VendorId, v.Name, AuditActions.Edited, before, Snapshot(v));
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> DeleteVendorAsync(int vendorId)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return denied;
        using var db = Db.Open();
        var v = await db.Vendors.FirstAsync(x => x.VendorId == vendorId);
        if (await db.Quotes.AnyAsync(q => q.VendorId == vendorId) || await db.Deliveries.AnyAsync(d => d.VendorId == vendorId))
            return "This vendor already has quotes or deliveries, so it can't be deleted.";
        db.VendorDocuments.RemoveRange(db.VendorDocuments.Where(d => d.VendorId == vendorId));
        AuditService.Add(db, Dept, "vendors", v.VendorId, v.Name, AuditActions.Deleted, Snapshot(v), null);
        db.Vendors.Remove(v);
        await db.SaveChangesAsync();
        return null;
    }

    static string? Clean(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    static void Apply(Vendor v, VendorInput i)
    {
        v.Name = i.Name.Trim(); v.ContactPerson = Clean(i.ContactPerson); v.Phone = Clean(i.Phone); v.Email = Clean(i.Email);
        v.Address = Clean(i.Address); v.BankName = Clean(i.BankName); v.AccountName = Clean(i.AccountName);
        v.AccountNumber = Clean(i.AccountNumber); v.VatStatus = i.VatStatus; v.UpdatedAt = DateTime.Now;
    }

    // Bank account numbers are kept out of the readable history: only whether one is on file.
    static object Snapshot(Vendor v) => new
    {
        name = v.Name, contact_person = v.ContactPerson, phone = v.Phone, email = v.Email, address = v.Address,
        bank_name = v.BankName, account_name = v.AccountName, account_number_on_file = v.AccountNumber != null, vat_status = v.VatStatus,
    };

    // ----- vendor papers -----

    public static async Task<List<VendorDocRow>> ListDocumentsAsync(int vendorId)
    {
        using var db = Db.Open();
        return await db.VendorDocuments.AsNoTracking().Where(d => d.VendorId == vendorId).OrderBy(d => d.DocType)
            .Select(d => new VendorDocRow(d.DocumentId, d.DocType, d.OriginalName, d.UploadedAt, d.FilePath)).ToListAsync();
    }

    public static async Task<string?> UploadDocumentAsync(int vendorId, string docType, string sourcePath)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return denied;
        if (!InventoryLists.VendorDocTypes.Contains(docType)) return "Choose the paper type.";
        if (FileStore.Check(sourcePath) is { } problem) return problem;
        var relative = await FileStore.SaveAsync(sourcePath, "vendors");
        using var db = Db.Open();
        var vendorName = await db.Vendors.Where(v => v.VendorId == vendorId).Select(v => v.Name).FirstAsync();
        var d = new VendorDocument
        {
            VendorId = vendorId, DocType = docType, FilePath = relative, OriginalName = Path.GetFileName(sourcePath),
            UploadedBy = Session.CurrentUser!.UserId,
        };
        db.VendorDocuments.Add(d);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "vendor_documents", d.DocumentId, vendorName, AuditActions.Added, null, new { doc_type = docType, file = d.OriginalName });
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> DeleteDocumentAsync(int documentId)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return denied;
        using var db = Db.Open();
        var d = await db.VendorDocuments.FirstAsync(x => x.DocumentId == documentId);
        var vendorName = await db.Vendors.Where(v => v.VendorId == d.VendorId).Select(v => v.Name).FirstAsync();
        AuditService.Add(db, Dept, "vendor_documents", d.DocumentId, vendorName, AuditActions.Deleted, new { doc_type = d.DocType, file = d.OriginalName }, null);
        db.VendorDocuments.Remove(d);
        await db.SaveChangesAsync();
        return null;   // the stored file itself is kept on disk
    }

    // ----- price comparison -----

    public static async Task<List<QuoteRow>> ListQuotesAsync(int materialId)
    {
        using var db = Db.Open();
        return await (from q in db.Quotes
                      join v in db.Vendors on q.VendorId equals v.VendorId
                      where q.MaterialId == materialId
                      orderby q.Status == "Open" descending, q.UnitPrice
                      select new QuoteRow(q.QuoteId, v.Name, q.QuotedOn, q.ProductName, q.Description, q.UnitPrice, q.Quantity, q.Status))
                     .ToListAsync();
    }

    public static string? CheckQuote(QuoteInput i)
    {
        if (i.MaterialId <= 0) return "Choose the material.";
        if (i.VendorId <= 0) return "Choose the vendor.";
        if (string.IsNullOrWhiteSpace(i.ProductName)) return "Type the vendor's product name.";
        if (i.UnitPrice <= 0) return "Type the quoted price per unit.";
        if (i.Quantity <= 0) return "Type how many to buy.";
        return null;
    }

    public static async Task<(int? Id, string? Error)> AddQuoteAsync(QuoteInput i)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return (null, denied);
        if (CheckQuote(i) is { } problem) return (null, problem);
        using var db = Db.Open();
        var q = new Quote
        {
            MaterialId = i.MaterialId, VendorId = i.VendorId, QuotedOn = i.QuotedOn, ProductName = i.ProductName.Trim(),
            Description = Clean(i.Description), Category = Clean(i.Category), UnitPrice = i.UnitPrice, Quantity = i.Quantity,
            CreatedBy = Session.CurrentUser!.UserId,
        };
        db.Quotes.Add(q);
        await db.SaveChangesAsync();
        var vendor = await db.Vendors.Where(v => v.VendorId == i.VendorId).Select(v => v.Name).FirstAsync();
        AuditService.Add(db, Dept, "quotes", q.QuoteId, $"{q.ProductName} from {vendor}", AuditActions.Added, null,
            new { unit_price = q.UnitPrice, quantity = q.Quantity, quoted_on = q.QuotedOn });
        await db.SaveChangesAsync();
        return (q.QuoteId, null);
    }

    public static async Task<string?> DeleteQuoteAsync(int quoteId)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return denied;
        using var db = Db.Open();
        var q = await db.Quotes.FirstAsync(x => x.QuoteId == quoteId);
        if (q.Status != "Open") return "Only open quotes can be deleted.";
        AuditService.Add(db, Dept, "quotes", q.QuoteId, q.ProductName, AuditActions.Deleted, new { unit_price = q.UnitPrice, quantity = q.Quantity }, null);
        db.Quotes.Remove(q);
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>The Supervisor picks a quote: it becomes the approved purchase; the other open quotes for that material close.</summary>
    public static async Task<(int? PurchaseId, string? Error)> ApproveQuoteAsync(int quoteId)
    {
        if (Access.RequireApprover(Dept) is { } denied) return (null, denied);
        using var db = Db.Open();
        var q = await db.Quotes.FirstAsync(x => x.QuoteId == quoteId);
        if (q.Status != "Open") return (null, $"This quote is already {q.Status}.");

        q.Status = "Selected";
        foreach (var other in await db.Quotes.Where(x => x.MaterialId == q.MaterialId && x.Status == "Open" && x.QuoteId != quoteId).ToListAsync())
            other.Status = "Not Selected";
        var p = new Purchase
        {
            QuoteId = q.QuoteId, MaterialId = q.MaterialId, VendorId = q.VendorId, Quantity = q.Quantity, UnitPrice = q.UnitPrice,
            ApprovedBy = Session.CurrentUser!.UserId,
        };
        db.Purchases.Add(p);
        await db.SaveChangesAsync();
        var vendor = await db.Vendors.Where(v => v.VendorId == q.VendorId).Select(v => v.Name).FirstAsync();
        AuditService.Add(db, Dept, "purchases", p.PurchaseId, $"{q.ProductName} from {vendor}", AuditActions.Added, null,
            new { quantity = p.Quantity, unit_price = p.UnitPrice, total = p.Quantity * p.UnitPrice });
        await db.SaveChangesAsync();
        return (p.PurchaseId, null);
    }

    public static async Task<List<PurchaseRow>> ListPurchasesAsync(bool undeliveredOnly = false)
    {
        using var db = Db.Open();
        var rows = await (from p in db.Purchases
                          join v in db.Vendors on p.VendorId equals v.VendorId
                          join m in db.Materials on p.MaterialId equals m.MaterialId
                          orderby p.ApprovedAt descending
                          select new PurchaseRow(p.PurchaseId, p.ApprovedAt, v.VendorId, v.Name, m.MaterialId, m.Name, p.Quantity, p.UnitPrice,
                              db.Deliveries.Where(d => d.PurchaseId == p.PurchaseId).Sum(d => (decimal?)d.Quantity) ?? 0))
                         .ToListAsync();
        return undeliveredOnly ? rows.Where(r => !r.Delivered).ToList() : rows;
    }

    // ----- deliveries -----

    public static string? CheckDelivery(DeliveryInput i)
    {
        if (i.VendorId <= 0) return "Choose the vendor.";
        if (i.MaterialId <= 0) return "Choose the material.";
        if (i.Quantity <= 0) return "Type how many arrived.";
        if (i.UnitPrice < 0) return "The price can't be negative.";
        if (string.IsNullOrWhiteSpace(i.ReceiptNo)) return "Type the receipt number.";
        if (i.PhysicalOnly)
        {
            if (string.IsNullOrWhiteSpace(i.FiledLocation)) return "Type where the paper receipt is filed.";
        }
        else if (string.IsNullOrWhiteSpace(i.ScanSourcePath)) return "Attach the receipt scan, or tick \"Physical copy only\".";
        else if (FileStore.Check(i.ScanSourcePath) is { } fileProblem) return fileProblem;
        return null;
    }

    /// <summary>Saves the delivery and its receipt, adds the stock, and updates the material's price.</summary>
    public static async Task<(int? Id, string? Error)> RecordDeliveryAsync(DeliveryInput i)
    {
        if (Access.RequirePage(AppPage.Inventory) is { } denied) return (null, denied);
        if (CheckDelivery(i) is { } problem) return (null, problem);

        string? stored = i.PhysicalOnly ? null : await FileStore.SaveAsync(i.ScanSourcePath!, "receipts");
        using var db = Db.Open();
        var m = await db.Materials.FirstAsync(x => x.MaterialId == i.MaterialId);
        var d = new Delivery
        {
            PurchaseId = i.PurchaseId, VendorId = i.VendorId, MaterialId = i.MaterialId, Quantity = i.Quantity, UnitPrice = i.UnitPrice,
            DeliveredOn = i.DeliveredOn, ReceiptNo = i.ReceiptNo.Trim(), ReceiptFilePath = stored, PhysicalOnly = i.PhysicalOnly,
            FiledLocation = i.PhysicalOnly ? i.FiledLocation!.Trim() : null, RecordedBy = Session.CurrentUser!.UserId,
        };
        db.Deliveries.Add(d);
        var stockBefore = m.StockQty;
        m.StockQty += i.Quantity;
        m.UnitPrice = i.UnitPrice;
        m.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        var vendor = await db.Vendors.Where(v => v.VendorId == i.VendorId).Select(v => v.Name).FirstAsync();
        AuditService.Add(db, Dept, "deliveries", d.DeliveryId, $"{m.Name} from {vendor}", AuditActions.Added, new { stock_qty = stockBefore },
            new { quantity = d.Quantity, unit_price = d.UnitPrice, receipt_no = d.ReceiptNo, receipt_copy = d.PhysicalOnly ? "Paper" : "Scan", stock_qty = m.StockQty });
        await db.SaveChangesAsync();
        return (d.DeliveryId, null);
    }

    public static async Task<List<DeliveryRow>> ListDeliveriesAsync()
    {
        using var db = Db.Open();
        return await (from d in db.Deliveries
                      join v in db.Vendors on d.VendorId equals v.VendorId
                      join m in db.Materials on d.MaterialId equals m.MaterialId
                      orderby d.DeliveredOn descending, d.DeliveryId descending
                      select new DeliveryRow(d.DeliveryId, d.DeliveredOn, v.Name, m.Name, d.Quantity, d.Quantity * d.UnitPrice,
                          d.ReceiptNo, d.PhysicalOnly, d.ReceiptFilePath))
                     .ToListAsync();
    }
}
