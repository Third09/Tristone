using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

public record QuotationRow(int QuotationId, string QuoteNo, DateOnly QuoteDate, string ClientName, string ProjectTitle,
                           decimal Total, DateOnly ValidUntil, string Status);

/// <summary>One "Scope of Works" line: the work and its size or measurement (printed A, B, C...).</summary>
public record QuotationScopeLine(string Work, string? Details);

/// <summary>One cost line (e.g. "Labor Cost", 25,000).</summary>
public record QuotationLine(string Description, decimal Amount);

public record QuotationInput(string ClientName, string? ClientAddress, string? Attention, string? ClientContact, string? ClientEmail,
                             string ProjectTitle, string? SiteLocation, string Sector, DateOnly QuoteDate, DateOnly ValidUntil,
                             DateOnly? PlannedStart, string? PrintedNotes, string? InternalNotes, string? WorkDuration,
                             string? PaymentTerms, string? PayableTo, string? PayableNumber, string? SignatoryName,
                             string? SignatoryTitle, int? InquiryId, IReadOnlyList<QuotationScopeLine> Scope,
                             IReadOnlyList<QuotationLine> Lines);

/// <summary>What a new quotation starts with: the last quotation's payment and signature details, or the company's usual ones.</summary>
public record QuotationDefaults(string PrintedNotes, string PaymentTerms, string PayableTo, string PayableNumber,
                                string SignatoryName, string SignatoryTitle);

/// <summary>An inquiry a quotation can start from (its name, contact, location and service fill the form).</summary>
public record InquiryChoice(int InquiryId, string Text, string FullName, string? Phone, string? Email, string? Location, string? Service);

/// <summary>
/// Quotations (owner request 2026-11-09): a price offer to a client before a project starts, numbered QT-0001...
/// It follows the company's own quotation paper (script 024): Customer / Address / Attention / Date, Scope of Works,
/// cost lines and Total Cost, Notes, Work Duration, Terms of Payment, Cash Payable to, Conforme / Very truly yours.
/// The Receipts Employee prepares it (Draft) and sends it for approval (Waiting); the Receipts Supervisor approves or
/// rejects it (a rejected one is fixed and sent again). An approved one is printed and given to the client. When the
/// client accepts, the Receipts Supervisor turns it into a project (contract price = the total); or marks it Declined.
/// Admins can look but not change (Access).
/// </summary>
public static class QuotationService
{
    const string Dept = Departments.Receipts;

    /// <summary>The usual notes and details from the company's quotation paper, used until a quotation has been saved.</summary>
    public static readonly QuotationDefaults CompanyDefaults = new(
        "Heavy equipment, Hauling equipment, Disposal Waste Materials shouldered by the customer.\n" +
        "Owner must provide designated place for temporary shelter of the construction personnel.\n" +
        "Any permits, supply of electricity and water at the job site shall be shouldered by the customer.",
        "Weekly Payroll", "ADRIAN F. MORILLA", "09691806929", "ADRIAN F. MORILLA", "Contractor");

    public static decimal Total(IEnumerable<QuotationLine> lines) => lines.Sum(l => l.Amount);

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
        var totals = await db.QuotationItems.AsNoTracking().Where(i => ids.Contains(i.QuotationId))
            .GroupBy(i => i.QuotationId).Select(g => new { g.Key, Sum = g.Sum(i => i.Amount) }).ToDictionaryAsync(x => x.Key, x => x.Sum);
        return rows.Select(r => new QuotationRow(r.QuotationId, r.QuoteNo, r.QuoteDate, r.ClientName, r.ProjectTitle,
            totals.GetValueOrDefault(r.QuotationId), r.ValidUntil, r.Status)).ToList();
    }

    public static async Task<(Quotation Quotation, List<QuotationScopeLine> Scope, List<QuotationLine> Lines)?> GetAsync(int id)
    {
        using var db = Db.Open();
        var q = await db.Quotations.AsNoTracking().FirstOrDefaultAsync(x => x.QuotationId == id);
        if (q == null) return null;
        var scope = await db.QuotationScope.AsNoTracking().Where(i => i.QuotationId == id).OrderBy(i => i.LineNo)
            .Select(i => new QuotationScopeLine(i.Description, i.Details)).ToListAsync();
        var lines = await db.QuotationItems.AsNoTracking().Where(i => i.QuotationId == id).OrderBy(i => i.LineNo)
            .Select(i => new QuotationLine(i.Description, i.Amount)).ToListAsync();
        return (q, scope, lines);
    }

    /// <summary>The newest quotation's payment and signature details (they rarely change), else the company's usual ones.</summary>
    public static async Task<QuotationDefaults> DefaultsAsync()
    {
        using var db = Db.Open();
        var last = await db.Quotations.AsNoTracking().OrderByDescending(q => q.QuotationId).FirstOrDefaultAsync();
        var d = CompanyDefaults;
        if (last == null) return d;
        return new QuotationDefaults(last.PrintedNotes ?? d.PrintedNotes, last.PaymentTerms ?? d.PaymentTerms, last.PayableTo ?? d.PayableTo,
            last.PayableNumber ?? d.PayableNumber, last.SignatoryName ?? d.SignatoryName, last.SignatoryTitle ?? d.SignatoryTitle);
    }

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
        if (string.IsNullOrWhiteSpace(i.ClientName)) return "Type the customer's name.";
        if (string.IsNullOrWhiteSpace(i.ProjectTitle)) return "Type the project name (it becomes the project's name if the client accepts).";
        if (!ProjectSectors.All.Contains(i.Sector)) return "Choose a sector.";
        if (i.ValidUntil < i.QuoteDate) return "\"Valid until\" can't be before the quotation date.";
        if (i.Scope.Count == 0) return "Add at least one line to the Scope of Works.";
        for (int n = 0; n < i.Scope.Count; n++)
            if (string.IsNullOrWhiteSpace(i.Scope[n].Work)) return $"Scope of Works line {(char)('A' + n)}: type the work.";
        if (i.Lines.Count == 0) return "Add at least one cost line (e.g. Labor Cost and its amount).";
        for (int n = 0; n < i.Lines.Count; n++)
        {
            if (string.IsNullOrWhiteSpace(i.Lines[n].Description)) return $"Cost line {n + 1}: type what the cost is for.";
            if (i.Lines[n].Amount < 0) return $"Cost line {n + 1}: the amount can't be negative.";
        }
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
        AddLines(db, q.QuotationId, i);
        AuditService.Add(db, Dept, "quotations", q.QuotationId, $"{q.QuoteNo} {q.ClientName}", AuditActions.Added, null, Snapshot(q, i.Scope, i.Lines));
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
        var oldScope = await db.QuotationScope.Where(x => x.QuotationId == id).OrderBy(x => x.LineNo).ToListAsync();
        var before = Snapshot(q, oldScope.Select(s => new QuotationScopeLine(s.Description, s.Details)).ToList(),
                              oldLines.Select(l => new QuotationLine(l.Description, l.Amount)).ToList());
        Apply(q, i);
        q.Status = QuotationStatuses.Draft;
        q.UpdatedAt = DateTime.Now;
        db.QuotationItems.RemoveRange(oldLines);
        db.QuotationScope.RemoveRange(oldScope);
        AddLines(db, id, i);
        AuditService.Add(db, Dept, "quotations", q.QuotationId, $"{q.QuoteNo} {q.ClientName}", AuditActions.Edited, before, Snapshot(q, i.Scope, i.Lines));
        await db.SaveChangesAsync();
        return null;
    }

    static void Apply(Quotation q, QuotationInput i)
    {
        static string? Clean(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        q.ClientName = i.ClientName.Trim(); q.ClientAddress = Clean(i.ClientAddress); q.Attention = Clean(i.Attention);
        q.ClientContact = Clean(i.ClientContact); q.ClientEmail = Clean(i.ClientEmail);
        q.ProjectTitle = i.ProjectTitle.Trim(); q.SiteLocation = Clean(i.SiteLocation); q.Sector = i.Sector;
        q.QuoteDate = i.QuoteDate; q.ValidUntil = i.ValidUntil; q.PlannedStart = i.PlannedStart;
        q.PrintedNotes = Clean(i.PrintedNotes); q.InternalNotes = Clean(i.InternalNotes);
        q.WorkDuration = Clean(i.WorkDuration); q.PaymentTerms = Clean(i.PaymentTerms);
        q.PayableTo = Clean(i.PayableTo); q.PayableNumber = Clean(i.PayableNumber);
        q.SignatoryName = Clean(i.SignatoryName); q.SignatoryTitle = Clean(i.SignatoryTitle);
        q.InquiryId = i.InquiryId ?? q.InquiryId;
    }

    static void AddLines(AppDbContext db, int quotationId, QuotationInput i)
    {
        for (int n = 0; n < i.Scope.Count; n++)
            db.QuotationScope.Add(new QuotationScopeItem
            {
                QuotationId = quotationId, LineNo = n + 1, Description = i.Scope[n].Work.Trim(),
                Details = string.IsNullOrWhiteSpace(i.Scope[n].Details) ? null : i.Scope[n].Details!.Trim(),
            });
        for (int n = 0; n < i.Lines.Count; n++)
            db.QuotationItems.Add(new QuotationItem
            {
                QuotationId = quotationId, LineNo = n + 1, Description = i.Lines[n].Description.Trim(), Amount = i.Lines[n].Amount,
            });
    }

    static object Snapshot(Quotation q, IReadOnlyList<QuotationScopeLine> scope, IReadOnlyList<QuotationLine> lines) => new
    {
        client = q.ClientName, address = q.ClientAddress, attention = q.Attention, project = q.ProjectTitle, site = q.SiteLocation,
        sector = q.Sector, quote_date = q.QuoteDate, valid_until = q.ValidUntil, work_duration = q.WorkDuration,
        payment_terms = q.PaymentTerms, payable_to = q.PayableTo, signatory = q.SignatoryName,
        scope = scope.Count, cost_lines = lines.Count, total = Total(lines), status = q.Status,
    };

    async static Task<string?> MoveAsync(int id, string from, string to, Func<string?> check, bool decide)
    {
        if (check() is { } denied) return denied;
        using var db = Db.Open();
        var q = await db.Quotations.FirstAsync(x => x.QuotationId == id);
        if (q.Status != from) return $"This quotation is {q.Status}.";
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
        var (q, _, lines) = found;
        if (q.Status != QuotationStatuses.Approved) return (null, $"Only an Approved quotation can be accepted (this one is {q.Status}).");
        var start = q.PlannedStart ?? DateOnly.FromDateTime(DateTime.Today);
        var (projectId, error) = await ProjectService.CreateAsync(new ProjectInput(q.ProjectTitle, q.SiteLocation ?? q.ClientAddress ?? "(to follow)",
            q.ClientName, q.Sector, Total(lines), start, null, ProjectStatuses.Pending, false));
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
        if (await GetAsync(id) is not { } found) return null;
        var (q, scope, lines) = found;
        if (q.Status is not (QuotationStatuses.Draft or QuotationStatuses.Rejected)) return "Only a Draft or Rejected quotation can be deleted. Mark it Declined instead.";
        using var db = Db.Open();
        AuditService.Add(db, Dept, "quotations", q.QuotationId, $"{q.QuoteNo} {q.ClientName}", AuditActions.Deleted, Snapshot(q, scope, lines), null);
        db.Quotations.Remove(await db.Quotations.FirstAsync(x => x.QuotationId == id));   // scope and cost lines go with it (ON DELETE CASCADE)
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<QuotationPrintData> PrintDataAsync(int id)
    {
        var (q, scope, lines) = (await GetAsync(id))!.Value;
        return new QuotationPrintData(q, scope, lines, Total(lines));
    }
}

public record QuotationPrintData(Quotation Quotation, List<QuotationScopeLine> Scope, List<QuotationLine> Lines, decimal Total);
