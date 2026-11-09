using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

public record QuotationRow(int QuotationId, string QuoteNo, DateOnly QuoteDate, string ClientName, string ProjectTitle,
                           decimal Total, DateOnly ValidUntil, string Status);

/// <summary>One line as typed in the form.</summary>
public record QuotationLine(string Description, decimal Quantity, string Unit, decimal UnitPrice)
{
    public decimal Amount => Math.Round(Quantity * UnitPrice, 2);
}

public record QuotationInput(string ClientName, string? ClientContact, string? ClientEmail, string ProjectTitle, string? SiteLocation,
                             string Sector, DateOnly QuoteDate, DateOnly ValidUntil, DateOnly? PlannedStart, decimal Discount,
                             bool VatApplies, string? Terms, string? Notes, int? InquiryId, IReadOnlyList<QuotationLine> Lines);

/// <summary>Subtotal of the lines, minus the discount, plus 12% VAT when ticked.</summary>
public record QuotationTotals(decimal Subtotal, decimal Discount, decimal Taxable, decimal Vat, decimal Total);

/// <summary>An inquiry a quotation can start from (its name, contact, location and service fill the form).</summary>
public record InquiryChoice(int InquiryId, string Text, string FullName, string? Phone, string? Email, string? Location, string? Service);

/// <summary>
/// Quotations (owner request 2026-11-09): a price offer to a client before a project starts, numbered QT-0001...
/// The Receipts Employee prepares it (Draft) and sends it for approval (Waiting); the Receipts Supervisor approves or
/// rejects it (a rejected one is fixed and sent again). An approved one is printed and given to the client. When the
/// client accepts, the Receipts Supervisor turns it into a project (contract price = the total); or marks it Declined.
/// Admins can look but not change (Access).
/// </summary>
public static class QuotationService
{
    const string Dept = Departments.Receipts;
    public const decimal VatRate = 0.12m;
    public const string DefaultTerms =
        "1. Prices are in Philippine pesos and valid until the date above.\n" +
        "2. 50% down payment upon signing, 40% progress billing, 10% upon completion and turnover.\n" +
        "3. Any work not listed above is quoted separately through a change order.\n" +
        "4. Permits and inspection fees are for the client's account unless listed above.";

    public static QuotationTotals Totals(IEnumerable<QuotationLine> lines, decimal discount, bool vat)
    {
        var subtotal = lines.Sum(l => l.Amount);
        var taxable = Math.Max(0, subtotal - discount);
        var tax = vat ? Math.Round(taxable * VatRate, 2) : 0;
        return new QuotationTotals(subtotal, discount, taxable, tax, taxable + tax);
    }

    public static async Task<List<QuotationRow>> ListAsync(string? status, string search)
    {
        if (Access.RequirePage(AppPage.Quotations) != null) return [];
        using var db = Db.Open();
        var q = db.Quotations.AsNoTracking();
        if (status != null) q = q.Where(x => x.Status == status);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(x => x.QuoteNo.ToLower().Contains(s) || x.ClientName.ToLower().Contains(s) || x.ProjectTitle.ToLower().Contains(s));
        }
        var rows = await q.OrderByDescending(x => x.QuotationId).ToListAsync();
        var ids = rows.Select(r => r.QuotationId).ToList();
        var items = await db.QuotationItems.AsNoTracking().Where(i => ids.Contains(i.QuotationId)).ToListAsync();
        return rows.Select(r => new QuotationRow(r.QuotationId, r.QuoteNo, r.QuoteDate, r.ClientName, r.ProjectTitle,
            Totals(items.Where(i => i.QuotationId == r.QuotationId).Select(ToLine), r.Discount, r.VatApplies).Total,
            r.ValidUntil, r.Status)).ToList();
    }

    public static async Task<(Quotation Quotation, List<QuotationLine> Lines)?> GetAsync(int id)
    {
        using var db = Db.Open();
        var q = await db.Quotations.AsNoTracking().FirstOrDefaultAsync(x => x.QuotationId == id);
        if (q == null) return null;
        var lines = await db.QuotationItems.AsNoTracking().Where(i => i.QuotationId == id).OrderBy(i => i.LineNo).Select(i => ToLine(i)).ToListAsync();
        return (q, lines);
    }

    static QuotationLine ToLine(QuotationItem i) => new(i.Description, i.Quantity, i.Unit, i.UnitPrice);

    public static async Task<string> NextNumberAsync()
    {
        using var db = Db.Open();
        return await NextNumberAsync(db);
    }

    static async Task<string> NextNumberAsync(AppDbContext db)
    {
        var numbers = await db.Quotations.Select(q => q.QuoteNo).ToListAsync();
        var last = numbers.Select(n => int.TryParse(n.Replace("QT-", ""), out var x) ? x : 0).DefaultIfEmpty(0).Max();
        return $"QT-{last + 1:0000}";
    }

    /// <summary>Website inquiries a quotation can start from (newest first, not closed).</summary>
    public static async Task<List<InquiryChoice>> InquiryChoicesAsync()
    {
        using var db = Db.Open();
        var rows = await db.Inquiries.AsNoTracking().Where(i => i.Status != InquiryStatuses.Closed)
            .OrderByDescending(i => i.ReceivedAt).Take(100).ToListAsync();
        return rows.Select(i => new InquiryChoice(i.InquiryId, $"{i.ReceivedAt:MMM d} - {i.FullName}{(i.ServiceRequired == null ? "" : " (" + i.ServiceRequired + ")")}",
                                                  i.FullName, i.Phone, i.Email, i.ProjectLocation, i.ServiceRequired)).ToList();
    }

    public static string? CheckInput(QuotationInput i)
    {
        if (string.IsNullOrWhiteSpace(i.ClientName)) return "Type the client's name.";
        if (string.IsNullOrWhiteSpace(i.ProjectTitle)) return "Type what the quotation is for (e.g. Two-storey house).";
        if (!ProjectSectors.All.Contains(i.Sector)) return "Choose a sector.";
        if (i.ValidUntil < i.QuoteDate) return "\"Valid until\" can't be before the quotation date.";
        if (i.Discount < 0) return "The discount can't be negative.";
        if (i.Lines.Count == 0) return "Add at least one line (item, quantity, unit, unit price).";
        for (int n = 0; n < i.Lines.Count; n++)
        {
            var l = i.Lines[n];
            if (string.IsNullOrWhiteSpace(l.Description)) return $"Line {n + 1}: type the item.";
            if (l.Quantity <= 0) return $"Line {n + 1}: the quantity must be more than zero.";
            if (string.IsNullOrWhiteSpace(l.Unit)) return $"Line {n + 1}: type the unit (e.g. lot, sq.m, pc).";
            if (l.UnitPrice < 0) return $"Line {n + 1}: the unit price can't be negative.";
        }
        if (i.Discount > Totals(i.Lines, 0, false).Subtotal) return "The discount can't be more than the subtotal.";
        return null;
    }

    public static async Task<(int? Id, string? Error)> CreateAsync(QuotationInput i)
    {
        if (Access.RequireEdit(AppPage.Quotations) is { } denied) return (null, denied);
        if (CheckInput(i) is { } problem) return (null, problem);
        using var db = Db.Open();
        var q = new Quotation { QuoteNo = await NextNumberAsync(db), CreatedBy = Session.CurrentUser!.UserId, CreatedAt = DateTime.Now };
        Apply(q, i);
        db.Quotations.Add(q);
        await db.SaveChangesAsync();
        AddLines(db, q.QuotationId, i.Lines);
        AuditService.Add(db, Dept, "quotations", q.QuotationId, $"{q.QuoteNo} {q.ClientName}", AuditActions.Added, null, Snapshot(q, i.Lines));
        await db.SaveChangesAsync();
        return (q.QuotationId, null);
    }

    /// <summary>Only a Draft or a Rejected one can change; a Rejected one goes back to Draft.</summary>
    public static async Task<string?> UpdateAsync(int id, QuotationInput i)
    {
        if (Access.RequireEdit(AppPage.Quotations) is { } denied) return denied;
        if (CheckInput(i) is { } problem) return problem;
        using var db = Db.Open();
        var q = await db.Quotations.FirstAsync(x => x.QuotationId == id);
        if (q.Status is not (QuotationStatuses.Draft or QuotationStatuses.Rejected)) return $"This quotation is {q.Status}, so it can't be changed.";
        var oldLines = await db.QuotationItems.Where(x => x.QuotationId == id).OrderBy(x => x.LineNo).ToListAsync();
        var before = Snapshot(q, oldLines.Select(ToLine).ToList());
        Apply(q, i);
        q.Status = QuotationStatuses.Draft;
        q.UpdatedAt = DateTime.Now;
        db.QuotationItems.RemoveRange(oldLines);
        AddLines(db, id, i.Lines);
        AuditService.Add(db, Dept, "quotations", q.QuotationId, $"{q.QuoteNo} {q.ClientName}", AuditActions.Edited, before, Snapshot(q, i.Lines));
        await db.SaveChangesAsync();
        return null;
    }

    static void Apply(Quotation q, QuotationInput i)
    {
        static string? Clean(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        q.ClientName = i.ClientName.Trim(); q.ClientContact = Clean(i.ClientContact); q.ClientEmail = Clean(i.ClientEmail);
        q.ProjectTitle = i.ProjectTitle.Trim(); q.SiteLocation = Clean(i.SiteLocation); q.Sector = i.Sector;
        q.QuoteDate = i.QuoteDate; q.ValidUntil = i.ValidUntil; q.PlannedStart = i.PlannedStart;
        q.Discount = i.Discount; q.VatApplies = i.VatApplies; q.Terms = Clean(i.Terms); q.Notes = Clean(i.Notes);
        q.InquiryId = i.InquiryId ?? q.InquiryId;
    }

    static void AddLines(AppDbContext db, int quotationId, IReadOnlyList<QuotationLine> lines)
    {
        for (int n = 0; n < lines.Count; n++)
            db.QuotationItems.Add(new QuotationItem
            {
                QuotationId = quotationId, LineNo = n + 1, Description = lines[n].Description.Trim(),
                Quantity = lines[n].Quantity, Unit = lines[n].Unit.Trim(), UnitPrice = lines[n].UnitPrice,
            });
    }

    static object Snapshot(Quotation q, IReadOnlyList<QuotationLine> lines) => new
    {
        client = q.ClientName, project = q.ProjectTitle, site = q.SiteLocation, sector = q.Sector,
        quote_date = q.QuoteDate, valid_until = q.ValidUntil, discount = q.Discount, vat = q.VatApplies,
        lines = lines.Count, total = Totals(lines, q.Discount, q.VatApplies).Total, status = q.Status,
    };

    async static Task<string?> MoveAsync(int id, string from, string to, Func<string?> check, bool decide)
    {
        if (check() is { } denied) return denied;
        using var db = Db.Open();
        var q = await db.Quotations.FirstAsync(x => x.QuotationId == id);
        var allowed = from.Split('|');
        if (!allowed.Contains(q.Status)) return $"This quotation is {q.Status}.";
        var old = q.Status;
        q.Status = to;
        q.UpdatedAt = DateTime.Now;
        if (decide) { q.DecidedBy = Session.CurrentUser!.UserId; q.DecidedAt = DateTime.Now; }
        AuditService.Add(db, Dept, "quotations", q.QuotationId, $"{q.QuoteNo} {q.ClientName}", AuditActions.Edited, new { status = old }, new { status = to });
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>The Employee sends a Draft to the Receipts Supervisor.</summary>
    public static Task<string?> SubmitAsync(int id) =>
        MoveAsync(id, QuotationStatuses.Draft, QuotationStatuses.Waiting, () => Access.RequireEdit(AppPage.Quotations), false);

    public static Task<string?> ApproveAsync(int id) =>
        MoveAsync(id, QuotationStatuses.Waiting, QuotationStatuses.Approved, () => Access.RequireApprover(Dept), true);

    public static Task<string?> RejectAsync(int id) =>
        MoveAsync(id, QuotationStatuses.Waiting, QuotationStatuses.Rejected, () => Access.RequireApprover(Dept), true);

    /// <summary>The client said no.</summary>
    public static Task<string?> DeclineAsync(int id) =>
        MoveAsync(id, QuotationStatuses.Approved, QuotationStatuses.Declined, () => Access.RequireApprover(Dept), true);

    /// <summary>
    /// The client accepted: the Receipts Supervisor makes the project (Pending, contract price = the total, start =
    /// the planned start or today) and the quotation is marked Accepted with a link to it. Returns the project's id.
    /// </summary>
    public static async Task<(int? ProjectId, string? Error)> AcceptAsync(int id)
    {
        if (Access.RequireApprover(Dept) is { } denied) return (null, denied);
        if (await GetAsync(id) is not { } found) return (null, "This quotation no longer exists.");
        var (q, lines) = found;
        if (q.Status != QuotationStatuses.Approved) return (null, $"Only an Approved quotation can be accepted (this one is {q.Status}).");
        var total = Totals(lines, q.Discount, q.VatApplies).Total;
        var start = q.PlannedStart ?? DateOnly.FromDateTime(DateTime.Today);
        var (projectId, error) = await ProjectService.CreateAsync(new ProjectInput(q.ProjectTitle, q.SiteLocation ?? "(to follow)",
            q.ClientName, q.Sector, total, start, null, ProjectStatuses.Pending, false));
        if (projectId == null) return (null, error);
        using var db = Db.Open();
        var row = await db.Quotations.FirstAsync(x => x.QuotationId == id);
        row.Status = QuotationStatuses.Accepted;
        row.ProjectId = projectId;
        row.DecidedBy = Session.CurrentUser!.UserId;
        row.DecidedAt = DateTime.Now;
        row.UpdatedAt = DateTime.Now;
        AuditService.Add(db, Dept, "quotations", row.QuotationId, $"{row.QuoteNo} {row.ClientName}", AuditActions.Edited,
            new { status = QuotationStatuses.Approved }, new { status = row.Status, project = q.ProjectTitle });
        await db.SaveChangesAsync();
        return (projectId, null);
    }

    /// <summary>Only a Draft or Rejected one, by the Receipts Supervisor (sent quotations stay on record).</summary>
    public static async Task<string?> DeleteAsync(int id)
    {
        if (Access.RequireDelete(AppPage.Quotations) is { } denied) return denied;
        using var db = Db.Open();
        var q = await db.Quotations.FirstOrDefaultAsync(x => x.QuotationId == id);
        if (q == null) return null;
        if (q.Status is not (QuotationStatuses.Draft or QuotationStatuses.Rejected)) return "Only a Draft or Rejected quotation can be deleted. Mark it Declined instead.";
        var lines = await db.QuotationItems.Where(x => x.QuotationId == id).ToListAsync();
        AuditService.Add(db, Dept, "quotations", q.QuotationId, $"{q.QuoteNo} {q.ClientName}", AuditActions.Deleted, Snapshot(q, lines.Select(ToLine).ToList()), null);
        db.QuotationItems.RemoveRange(lines);
        db.Quotations.Remove(q);
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Everything the printed quotation needs, with the company details from Website Admin > Contact Us.</summary>
    public static async Task<QuotationPrintData> PrintDataAsync(int id)
    {
        var (q, lines) = (await GetAsync(id))!.Value;
        using var db = Db.Open();
        var keys = new[] { "contact.address", "contact.phone", "contact.email" };
        var company = await db.WebsiteContent.AsNoTracking().Where(c => keys.Contains(c.ContentKey)).ToDictionaryAsync(c => c.ContentKey, c => c.Value);
        string? C(string k) => company.TryGetValue(k, out var v) ? v : null;
        var by = await db.Users.Where(u => u.UserId == q.CreatedBy).Select(u => u.FullName).FirstOrDefaultAsync() ?? "";
        var approvedBy = q.Status is QuotationStatuses.Approved or QuotationStatuses.Accepted or QuotationStatuses.Declined
            ? await db.Users.Where(u => u.UserId == q.DecidedBy).Select(u => u.FullName).FirstOrDefaultAsync() : null;
        return new QuotationPrintData(q, lines, Totals(lines, q.Discount, q.VatApplies), C("contact.address"), C("contact.phone"), C("contact.email"), by, approvedBy);
    }
}

public record QuotationPrintData(Quotation Quotation, List<QuotationLine> Lines, QuotationTotals Totals,
                                 string? CompanyAddress, string? CompanyPhone, string? CompanyEmail, string PreparedBy, string? ApprovedBy);
