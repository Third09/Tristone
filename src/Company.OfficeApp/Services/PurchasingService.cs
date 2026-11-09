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
                          decimal Quantity, decimal UnitPrice)
{
    public decimal Total => Quantity * UnitPrice;
}

/// <summary>A material on a vendor's "Materials supplied" list.</summary>
public record VendorMaterialRow(int MaterialId, string Material, string Unit, decimal Price, DateTime UpdatedAt);

/// <summary>
/// Inventory > Vendors and Price Comparison (Project Outline, Part 3).
/// Vendors keep a "Materials supplied" list with their prices (owner request 2026-11-09).
/// Price comparison: the Employee enters quotes from several vendors for the same material; the Supervisor
/// picks one, which becomes the approved purchase: the stock goes up right away, and the material's price and
/// that vendor's listed price become the bought price (the Deliveries tab was removed 2026-11-09; older
/// deliveries stay in the table and still show in the Receipt Vault as stock purchases).
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
        if (Access.RequireEdit(AppPage.Inventory) is { } denied) return (null, denied);
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
        if (Access.RequireEdit(AppPage.Inventory) is { } denied) return denied;
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
        if (Access.RequireDelete(AppPage.Inventory) is { } denied) return denied;
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
        if (Access.RequireEdit(AppPage.Inventory) is { } denied) return denied;
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
        if (Access.RequireDelete(AppPage.Inventory) is { } denied) return denied;
        using var db = Db.Open();
        var d = await db.VendorDocuments.FirstAsync(x => x.DocumentId == documentId);
        var vendorName = await db.Vendors.Where(v => v.VendorId == d.VendorId).Select(v => v.Name).FirstAsync();
        AuditService.Add(db, Dept, "vendor_documents", d.DocumentId, vendorName, AuditActions.Deleted, new { doc_type = d.DocType, file = d.OriginalName }, null);
        db.VendorDocuments.Remove(d);
        await db.SaveChangesAsync();
        return null;   // the stored file itself is kept on disk
    }

    // ----- materials a vendor supplies -----

    public static async Task<List<VendorMaterialRow>> ListVendorMaterialsAsync(int vendorId)
    {
        using var db = Db.Open();
        return await (from vm in db.VendorMaterials
                      join m in db.Materials on vm.MaterialId equals m.MaterialId
                      where vm.VendorId == vendorId
                      orderby m.Name
                      select new VendorMaterialRow(m.MaterialId, m.Name, m.Unit, vm.Price, vm.UpdatedAt)).ToListAsync();
    }

    /// <summary>The vendors that list this material, cheapest first (for Price Comparison).</summary>
    public static async Task<List<(int VendorId, string Vendor, decimal Price)>> SuppliersOfAsync(int materialId)
    {
        using var db = Db.Open();
        var rows = await (from vm in db.VendorMaterials
                          join v in db.Vendors on vm.VendorId equals v.VendorId
                          where vm.MaterialId == materialId
                          orderby vm.Price, v.Name
                          select new { v.VendorId, v.Name, vm.Price }).ToListAsync();
        return rows.Select(r => (r.VendorId, r.Name, r.Price)).ToList();
    }

    /// <summary>Adds the material to the vendor's list, or changes its price if it is already there.</summary>
    public static async Task<string?> SetVendorMaterialAsync(int vendorId, int materialId, decimal price)
    {
        if (Access.RequireEdit(AppPage.Inventory) is { } denied) return denied;
        if (materialId <= 0) return "Choose the material.";
        if (price < 0) return "The price can't be negative.";
        using var db = Db.Open();
        var vendor = await db.Vendors.Where(v => v.VendorId == vendorId).Select(v => v.Name).FirstAsync();
        var material = await db.Materials.Where(m => m.MaterialId == materialId).Select(m => m.Name).FirstAsync();
        var row = await db.VendorMaterials.FirstOrDefaultAsync(x => x.VendorId == vendorId && x.MaterialId == materialId);
        if (row == null)
        {
            db.VendorMaterials.Add(new VendorMaterial { VendorId = vendorId, MaterialId = materialId, Price = price, UpdatedBy = Session.CurrentUser!.UserId });
            AuditService.Add(db, Dept, "vendor_materials", vendorId, $"{material} from {vendor}", AuditActions.Added, null, new { price });
        }
        else
        {
            if (row.Price == price) return null;
            AuditService.Add(db, Dept, "vendor_materials", vendorId, $"{material} from {vendor}", AuditActions.Edited, new { price = row.Price }, new { price });
            row.Price = price;
            row.UpdatedBy = Session.CurrentUser!.UserId;
            row.UpdatedAt = DateTime.Now;
        }
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> RemoveVendorMaterialAsync(int vendorId, int materialId)
    {
        if (Access.RequireEdit(AppPage.Inventory) is { } denied) return denied;
        using var db = Db.Open();
        var row = await db.VendorMaterials.FirstOrDefaultAsync(x => x.VendorId == vendorId && x.MaterialId == materialId);
        if (row == null) return null;
        var vendor = await db.Vendors.Where(v => v.VendorId == vendorId).Select(v => v.Name).FirstAsync();
        var material = await db.Materials.Where(m => m.MaterialId == materialId).Select(m => m.Name).FirstAsync();
        AuditService.Add(db, Dept, "vendor_materials", vendorId, $"{material} from {vendor}", AuditActions.Deleted, new { price = row.Price }, null);
        db.VendorMaterials.Remove(row);
        await db.SaveChangesAsync();
        return null;
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
        if (Access.RequireEdit(AppPage.Inventory) is { } denied) return (null, denied);
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
        if (Access.RequireDelete(AppPage.Inventory) is { } denied) return denied;
        using var db = Db.Open();
        var q = await db.Quotes.FirstAsync(x => x.QuoteId == quoteId);
        if (q.Status != "Open") return "Only open quotes can be deleted.";
        AuditService.Add(db, Dept, "quotes", q.QuoteId, q.ProductName, AuditActions.Deleted, new { unit_price = q.UnitPrice, quantity = q.Quantity }, null);
        db.Quotes.Remove(q);
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>
    /// The Supervisor picks a quote: it becomes the approved purchase and the other open quotes for that material
    /// close. The stock goes up by the quantity, the material's price becomes the bought price, and the vendor's
    /// "Materials supplied" price is set to it.
    /// </summary>
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
        var m = await db.Materials.FirstAsync(x => x.MaterialId == q.MaterialId);
        var stockBefore = m.StockQty;
        m.StockQty += q.Quantity;
        m.UnitPrice = q.UnitPrice;
        m.UpdatedAt = DateTime.Now;
        var listed = await db.VendorMaterials.FirstOrDefaultAsync(x => x.VendorId == q.VendorId && x.MaterialId == q.MaterialId);
        if (listed == null) db.VendorMaterials.Add(new VendorMaterial { VendorId = q.VendorId, MaterialId = q.MaterialId, Price = q.UnitPrice, UpdatedBy = Session.CurrentUser.UserId });
        else { listed.Price = q.UnitPrice; listed.UpdatedBy = Session.CurrentUser.UserId; listed.UpdatedAt = DateTime.Now; }
        await db.SaveChangesAsync();
        var vendor = await db.Vendors.Where(v => v.VendorId == q.VendorId).Select(v => v.Name).FirstAsync();
        AuditService.Add(db, Dept, "purchases", p.PurchaseId, $"{q.ProductName} from {vendor}", AuditActions.Added, new { stock_qty = stockBefore },
            new { quantity = p.Quantity, unit_price = p.UnitPrice, total = p.Quantity * p.UnitPrice, stock_qty = m.StockQty });
        await db.SaveChangesAsync();
        return (p.PurchaseId, null);
    }

    public static async Task<List<PurchaseRow>> ListPurchasesAsync()
    {
        using var db = Db.Open();
        return await (from p in db.Purchases
                      join v in db.Vendors on p.VendorId equals v.VendorId
                      join m in db.Materials on p.MaterialId equals m.MaterialId
                      orderby p.ApprovedAt descending
                      select new PurchaseRow(p.PurchaseId, p.ApprovedAt, v.VendorId, v.Name, m.MaterialId, m.Name, p.Quantity, p.UnitPrice))
                     .ToListAsync();
    }
}
