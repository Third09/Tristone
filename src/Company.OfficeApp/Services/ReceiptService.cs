using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>
/// One line in the Receipt Vault or Needs Review. Material deliveries from Inventory appear too, marked as
/// stock purchases (<see cref="DeliveryId"/> set, ReceiptId 0); they are read only here.
/// </summary>
public record ReceiptRow(int ReceiptId, int? DeliveryId, string? ReceiptNo, DateOnly Date, string Merchant, decimal Amount, string Category,
                         string ProjectOrOverhead, string Copy, string Status, string UploadedBy, string? FilePath, string? NoReceiptReason)
{
    public bool IsStockPurchase => DeliveryId != null;
    public string StockPurchase => IsStockPurchase ? "Yes" : "";
}

/// <summary>Vault filters. <see cref="ProjectId"/> -1 = Company Overhead only, null = all.</summary>
public record ReceiptFilter(int? ProjectId, DateOnly From, DateOnly To, string? Category, string? Status, string Search)
{
    public const int OverheadOnly = -1;
}

public record ReceiptInput(string? ReceiptNo, DateOnly PurchaseDate, string Merchant, decimal Amount, string Category, string ExpenseType,
                           int? ProjectId, string? NewScanPath, bool PhysicalOnly, string? FiledLocation, bool NoReceipt, string? NoReceiptReason);

/// <summary>Everything shown in the Receipt Viewer and printed on the summary sheet.</summary>
public record ReceiptView(string? ReceiptNo, DateOnly Date, string Merchant, decimal Amount, string Category, string ProjectOrOverhead,
                          string UploadedBy, DateTime UploadedAt, string Status, string? DecidedBy, string Copy, string? FiledLocation,
                          string? NoReceiptReason, string? FilePath, string? OriginalName, bool StockPurchase);

/// <summary>
/// Receipts > Receipt Vault and Needs Review (Project Outline, Part 5). Each receipt is a Project expense or
/// Company overhead; the Receipts Employee uploads, the Receipts Supervisor (or an Admin) approves, and only
/// approved ones count. No receipt at all = "Needs Review" with a reason until approved; attaching a receipt
/// later clears the flag. Delivery receipts show as "Stock purchase" and are not counted again
/// (materials count when used on a project).
/// </summary>
public static class ReceiptService
{
    const string Dept = Departments.Receipts;
    public const string StockPurchaseLabel = "Stock purchase";
    public const string OverheadLabel = "Company Overhead";

    static string CopyOf(bool noReceipt, bool physicalOnly) => noReceipt ? "None" : physicalOnly ? "Paper" : "Scan";

    public static async Task<List<ReceiptRow>> ListAsync(ReceiptFilter f)
    {
        using var db = Db.Open();
        var q = from r in db.Receipts
                join p in db.Projects on r.ProjectId equals p.ProjectId into ps
                from p in ps.DefaultIfEmpty()
                join u in db.Users on r.UploadedBy equals u.UserId into us
                from u in us.DefaultIfEmpty()
                where !r.NoReceipt && r.PurchaseDate >= f.From && r.PurchaseDate <= f.To
                select new { r, Project = p == null ? null : p.ProjectName, Who = u == null ? "" : u.FullName };
        if (f.ProjectId == ReceiptFilter.OverheadOnly) q = q.Where(x => x.r.ExpenseType == ExpenseTypes.Overhead);
        else if (f.ProjectId is { } pid) q = q.Where(x => x.r.ProjectId == pid);
        if (f.Category != null) q = q.Where(x => x.r.Category == f.Category);
        if (f.Status != null) q = q.Where(x => x.r.Status == f.Status);
        if (!string.IsNullOrWhiteSpace(f.Search))
        {
            var s = f.Search.Trim().ToLower();
            q = q.Where(x => (x.r.ReceiptNo != null && x.r.ReceiptNo.ToLower().Contains(s)) || x.r.Merchant.ToLower().Contains(s) || x.r.Category.ToLower().Contains(s));
        }
        var rows = (await q.ToListAsync()).Select(x => new ReceiptRow(x.r.ReceiptId, null, x.r.ReceiptNo, x.r.PurchaseDate, x.r.Merchant, x.r.Amount,
            x.r.Category, x.Project ?? OverheadLabel, CopyOf(false, x.r.PhysicalOnly), x.r.Status, x.Who, x.r.FilePath, null)).ToList();

        // Stock purchases from Inventory: only when not filtering to one project / overhead.
        if (f.ProjectId == null && (f.Category == null || f.Category == "Materials") && (f.Status == null || f.Status == ApprovalStatuses.Approved))
        {
            var d = from x in db.Deliveries
                    join v in db.Vendors on x.VendorId equals v.VendorId
                    join u in db.Users on x.RecordedBy equals u.UserId into us
                    from u in us.DefaultIfEmpty()
                    where x.DeliveredOn >= f.From && x.DeliveredOn <= f.To
                    select new { x, Vendor = v.Name, Who = u == null ? "" : u.FullName };
            if (!string.IsNullOrWhiteSpace(f.Search))
            {
                var s = f.Search.Trim().ToLower();
                d = d.Where(y => y.x.ReceiptNo.ToLower().Contains(s) || y.Vendor.ToLower().Contains(s));
            }
            rows.AddRange((await d.ToListAsync()).Select(y => new ReceiptRow(0, y.x.DeliveryId, y.x.ReceiptNo, y.x.DeliveredOn, y.Vendor,
                Math.Round(y.x.Quantity * y.x.UnitPrice, 2), "Materials", StockPurchaseLabel, CopyOf(false, y.x.PhysicalOnly),
                ApprovalStatuses.Approved, y.Who, y.x.ReceiptFilePath, null)));
        }
        return rows.OrderByDescending(r => r.Date).ThenByDescending(r => r.ReceiptId).ToList();
    }

    /// <summary>Expenses with no receipt: waiting ones first, then the decided ones.</summary>
    public static async Task<List<ReceiptRow>> NeedsReviewAsync()
    {
        using var db = Db.Open();
        var list = await (from r in db.Receipts
                          join p in db.Projects on r.ProjectId equals p.ProjectId into ps
                          from p in ps.DefaultIfEmpty()
                          join u in db.Users on r.UploadedBy equals u.UserId into us
                          from u in us.DefaultIfEmpty()
                          where r.NoReceipt
                          select new { r, Project = p == null ? null : p.ProjectName, Who = u == null ? "" : u.FullName }).ToListAsync();
        return list.OrderBy(x => x.r.Status != ApprovalStatuses.Waiting).ThenByDescending(x => x.r.PurchaseDate)
            .Select(x => new ReceiptRow(x.r.ReceiptId, null, null, x.r.PurchaseDate, x.r.Merchant, x.r.Amount, x.r.Category,
                x.Project ?? OverheadLabel, "None", x.r.Status, x.Who, null, x.r.NoReceiptReason)).ToList();
    }

    public static async Task<int> NeedsReviewCountAsync()
    {
        using var db = Db.Open();
        return await db.Receipts.CountAsync(r => r.NoReceipt && r.Status == ApprovalStatuses.Waiting);
    }

    public static async Task<Receipt?> GetAsync(int receiptId)
    {
        using var db = Db.Open();
        return await db.Receipts.AsNoTracking().FirstOrDefaultAsync(r => r.ReceiptId == receiptId);
    }

    public static string? CheckInput(ReceiptInput i, bool hasStoredScan)
    {
        if (string.IsNullOrWhiteSpace(i.Merchant)) return "Type the merchant / store name.";
        if (i.Amount <= 0) return "Type the amount.";
        if (!ReceiptLists.Categories.Contains(i.Category)) return "Choose the category.";
        if (i.ExpenseType == ExpenseTypes.Project && i.ProjectId is not > 0) return "Choose the project.";
        if (i.ExpenseType is not (ExpenseTypes.Project or ExpenseTypes.Overhead)) return "Choose Project or Company overhead.";
        if (i.PurchaseDate > DateOnly.FromDateTime(DateTime.Today)) return "The date can't be in the future.";
        if (i.NoReceipt)
            return string.IsNullOrWhiteSpace(i.NoReceiptReason) ? "Type why there is no receipt (e.g. tricycle fare, no receipt given)." : null;
        if (string.IsNullOrWhiteSpace(i.ReceiptNo)) return "Type the receipt number.";
        if (i.PhysicalOnly) return string.IsNullOrWhiteSpace(i.FiledLocation) ? "Type where the paper copy is filed." : null;
        if (i.NewScanPath != null) return FileStore.Check(i.NewScanPath);
        return hasStoredScan ? null : "Attach the scan, or tick \"Physical copy only\", or \"No receipt\".";
    }

    public static async Task<(int? Id, string? Error)> CreateAsync(ReceiptInput i)
    {
        if (Access.RequirePage(AppPage.Receipts) is { } denied) return (null, denied);
        if (CheckInput(i, false) is { } problem) return (null, problem);
        using var db = Db.Open();
        if (await ProjectProblemAsync(db, i) is { } bad) return (null, bad);
        var r = new Receipt { UploadedBy = Session.CurrentUser!.UserId };
        await ApplyAsync(r, i);
        db.Receipts.Add(r);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "receipts", r.ReceiptId, Name(r), AuditActions.Added, null, Snapshot(r));
        await db.SaveChangesAsync();
        return (r.ReceiptId, null);
    }

    /// <summary>Only while waiting or rejected; saving sends it back for approval.</summary>
    public static async Task<string?> UpdateAsync(int receiptId, ReceiptInput i)
    {
        if (Access.RequirePage(AppPage.Receipts) is { } denied) return denied;
        using var db = Db.Open();
        var r = await db.Receipts.FirstAsync(x => x.ReceiptId == receiptId);
        if (r.Status == ApprovalStatuses.Approved) return "This receipt is already approved, so it can't be changed.";
        if (CheckInput(i, r.FilePath != null) is { } problem) return problem;
        if (await ProjectProblemAsync(db, i) is { } bad) return bad;
        var before = Snapshot(r);
        await ApplyAsync(r, i);
        r.Status = ApprovalStatuses.Waiting;
        r.DecidedBy = null;
        r.DecidedAt = null;
        AuditService.Add(db, Dept, "receipts", r.ReceiptId, Name(r), AuditActions.Edited, before, Snapshot(r));
        await db.SaveChangesAsync();
        return null;
    }

    static async Task<string?> ProjectProblemAsync(AppDbContext db, ReceiptInput i)
    {
        if (i.ExpenseType != ExpenseTypes.Project) return null;
        var p = await db.Projects.FirstOrDefaultAsync(x => x.ProjectId == i.ProjectId);
        return p == null ? "Choose the project." : p.Status == ProjectStatuses.Cancelled ? "That project is Cancelled." : null;
    }

    static async Task ApplyAsync(Receipt r, ReceiptInput i)
    {
        r.PurchaseDate = i.PurchaseDate; r.Merchant = i.Merchant.Trim(); r.Amount = i.Amount; r.Category = i.Category;
        r.ExpenseType = i.ExpenseType; r.ProjectId = i.ExpenseType == ExpenseTypes.Project ? i.ProjectId : null;
        r.NoReceipt = i.NoReceipt;
        r.NoReceiptReason = i.NoReceipt ? i.NoReceiptReason!.Trim() : null;
        r.ReceiptNo = i.NoReceipt || string.IsNullOrWhiteSpace(i.ReceiptNo) ? null : i.ReceiptNo.Trim();
        r.PhysicalOnly = !i.NoReceipt && i.PhysicalOnly;
        r.FiledLocation = r.PhysicalOnly ? i.FiledLocation!.Trim() : null;
        if (i.NoReceipt || r.PhysicalOnly) { r.FilePath = null; r.OriginalName = null; }
        else if (i.NewScanPath != null) { r.FilePath = await FileStore.SaveAsync(i.NewScanPath, "receipts"); r.OriginalName = Path.GetFileName(i.NewScanPath); }
        r.UpdatedAt = DateTime.Now;
    }

    /// <summary>A receipt that turned up later for a no-receipt expense: clears the Needs Review flag.</summary>
    public static async Task<string?> AttachLateReceiptAsync(int receiptId, string receiptNo, string? scanPath, bool physicalOnly, string? filedLocation)
    {
        if (Access.RequirePage(AppPage.Receipts) is { } denied) return denied;
        using var db = Db.Open();
        var r = await db.Receipts.FirstAsync(x => x.ReceiptId == receiptId);
        if (!r.NoReceipt) return "This expense already has a receipt.";
        if (r.Status == ApprovalStatuses.Rejected) return "This expense was rejected.";
        var i = new ReceiptInput(receiptNo, r.PurchaseDate, r.Merchant, r.Amount, r.Category, r.ExpenseType, r.ProjectId, scanPath, physicalOnly, filedLocation, false, null);
        if (CheckInput(i, false) is { } problem) return problem;
        var before = Snapshot(r);
        await ApplyAsync(r, i);   // the approval stays as it was
        AuditService.Add(db, Dept, "receipts", r.ReceiptId, Name(r), AuditActions.Edited, before, Snapshot(r));
        await db.SaveChangesAsync();
        return null;
    }

    public static Task<string?> ApproveAsync(IReadOnlyCollection<int> receiptIds) => DecideAsync(receiptIds, ApprovalStatuses.Approved);

    public static Task<string?> RejectAsync(IReadOnlyCollection<int> receiptIds) => DecideAsync(receiptIds, ApprovalStatuses.Rejected);

    static async Task<string?> DecideAsync(IReadOnlyCollection<int> ids, string decision)
    {
        if (Access.RequireApprover(Dept) is { } denied) return denied;
        if (ids.Count == 0) return "Select the receipts first.";
        using var db = Db.Open();
        var list = await db.Receipts.Where(r => ids.Contains(r.ReceiptId)).ToListAsync();
        if (list.Any(r => r.Status != ApprovalStatuses.Waiting))
            return list.Count == 1 ? $"This receipt is already {list[0].Status}." : "Some of the selected receipts are already decided. Select only waiting ones.";
        foreach (var r in list)
        {
            r.Status = decision;
            r.DecidedBy = Session.CurrentUser!.UserId;
            r.DecidedAt = DateTime.Now;
            AuditService.Add(db, Dept, "receipts", r.ReceiptId, Name(r), AuditActions.Edited, new { status = ApprovalStatuses.Waiting }, new { status = decision });
        }
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<ReceiptView> ViewAsync(ReceiptRow row)
    {
        using var db = Db.Open();
        if (row.DeliveryId is { } did)
        {
            var d = await db.Deliveries.AsNoTracking().FirstAsync(x => x.DeliveryId == did);
            return new ReceiptView(d.ReceiptNo, d.DeliveredOn, row.Merchant, row.Amount, row.Category, StockPurchaseLabel, row.UploadedBy, d.CreatedAt,
                row.Status, null, row.Copy, d.FiledLocation, null, d.ReceiptFilePath, d.ReceiptFilePath == null ? null : Path.GetFileName(d.ReceiptFilePath), true);
        }
        var r = await db.Receipts.AsNoTracking().FirstAsync(x => x.ReceiptId == row.ReceiptId);
        var decidedBy = r.DecidedBy == null ? null : await db.Users.Where(u => u.UserId == r.DecidedBy).Select(u => u.FullName).FirstOrDefaultAsync();
        return new ReceiptView(r.ReceiptNo, r.PurchaseDate, r.Merchant, r.Amount, r.Category, row.ProjectOrOverhead, row.UploadedBy, r.UploadedAt,
            r.Status, decidedBy, CopyOf(r.NoReceipt, r.PhysicalOnly), r.FiledLocation, r.NoReceiptReason, r.FilePath, r.OriginalName, false);
    }

    /// <summary>Approved project receipts per project (project expenses).</summary>
    internal static async Task<Dictionary<int, decimal>> ByProjectAsync(AppDbContext db, List<int> projectIds) =>
        await db.Receipts.Where(r => r.Status == ApprovalStatuses.Approved && r.ProjectId != null && projectIds.Contains(r.ProjectId.Value))
            .GroupBy(r => r.ProjectId!.Value).Select(g => new { g.Key, Sum = g.Sum(r => r.Amount) }).ToDictionaryAsync(x => x.Key, x => x.Sum);

    /// <summary>Approved company-overhead receipts in a date range (for the Dashboard).</summary>
    public static async Task<decimal> OverheadTotalAsync(DateOnly from, DateOnly to)
    {
        using var db = Db.Open();
        return await db.Receipts.Where(r => r.Status == ApprovalStatuses.Approved && r.ExpenseType == ExpenseTypes.Overhead
                                            && r.PurchaseDate >= from && r.PurchaseDate <= to).SumAsync(r => (decimal?)r.Amount) ?? 0;
    }

    public static async Task<string> UserNameAsync(int? userId)
    {
        using var db = Db.Open();
        return await db.Users.Where(u => u.UserId == userId).Select(u => u.FullName).FirstOrDefaultAsync() ?? "";
    }

    static string Name(Receipt r) => r.ReceiptNo != null ? $"{r.ReceiptNo} {r.Merchant}" : $"(no receipt) {r.Merchant}";

    static object Snapshot(Receipt r) => new
    {
        receipt_no = r.ReceiptNo, purchase_date = r.PurchaseDate, merchant = r.Merchant, amount = r.Amount, category = r.Category,
        expense_type = r.ExpenseType, project_id = r.ProjectId, copy = CopyOf(r.NoReceipt, r.PhysicalOnly), file = r.OriginalName,
        filed_location = r.FiledLocation, no_receipt_reason = r.NoReceiptReason, status = r.Status,
    };
}
