using Company.OfficeApp.Data;
using Company.OfficeApp.Services;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.WebsiteAdmin.Services;

/// <summary>One row of the Inquiries list.</summary>
public record InquiryRow(int InquiryId, DateTime ReceivedAt, string FullName, string? ServiceRequired, string? ProjectLocation,
                         string Status, string? Email, string? Phone);

/// <summary>How many inquiries are New, Contacted and Closed (all of them, not just the filtered list).</summary>
public record InquiryCounts(int New, int Contacted, int Closed);

/// <summary>
/// Website Admin > Inquiries: messages from the website Contact form. Website Admins and office Admins can follow
/// them up (status + internal notes). Inquiries are never deleted here, so nothing a client sent is lost.
/// </summary>
public static class WebsiteInquiries
{
    public const int MaxNotesLength = 2000;

    /// <summary>Website Admins and office Admins may use the Website Admin app (re-checked here, not just on screen).</summary>
    static string? RequireAccess() => Session.CurrentUser is { CanOpenWebsiteAdmin: true } ? null : "Only Website Admins and Admins can do this.";

    /// <summary>Newest first. <paramref name="status"/> null = all; dates are inclusive; search looks in name, email, phone and location.</summary>
    public static async Task<List<InquiryRow>> ListAsync(string? status, string search, DateOnly? from, DateOnly? to)
    {
        if (RequireAccess() != null) return [];
        using var db = Db.Open();
        var q = db.Inquiries.AsNoTracking();
        if (status != null) q = q.Where(i => i.Status == status);
        if (from is { } f) { var start = f.ToDateTime(TimeOnly.MinValue); q = q.Where(i => i.ReceivedAt >= start); }
        if (to is { } t) { var end = t.AddDays(1).ToDateTime(TimeOnly.MinValue); q = q.Where(i => i.ReceivedAt < end); }
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(i => i.FullName.ToLower().Contains(s) || (i.Email ?? "").ToLower().Contains(s)
                          || (i.Phone ?? "").Contains(s) || (i.ProjectLocation ?? "").ToLower().Contains(s));
        }
        return await q.OrderByDescending(i => i.ReceivedAt)
            .Select(i => new InquiryRow(i.InquiryId, i.ReceivedAt, i.FullName, i.ServiceRequired, i.ProjectLocation, i.Status, i.Email, i.Phone))
            .ToListAsync();
    }

    public static async Task<InquiryCounts> CountsAsync()
    {
        using var db = Db.Open();
        var byStatus = await db.Inquiries.GroupBy(i => i.Status).Select(g => new { g.Key, Count = g.Count() }).ToListAsync();
        int Of(string s) => byStatus.FirstOrDefault(x => x.Key == s)?.Count ?? 0;
        return new InquiryCounts(Of(InquiryStatuses.New), Of(InquiryStatuses.Contacted), Of(InquiryStatuses.Closed));
    }

    public static async Task<Inquiry?> GetAsync(int inquiryId)
    {
        using var db = Db.Open();
        return await db.Inquiries.AsNoTracking().FirstOrDefaultAsync(i => i.InquiryId == inquiryId);
    }

    /// <summary>Saves the follow-up: status and internal notes. Null when saved, otherwise the reason.</summary>
    public static async Task<string?> UpdateAsync(int inquiryId, string status, string? notes)
    {
        if (RequireAccess() is { } denied) return denied;
        if (!InquiryStatuses.All.Contains(status)) return "Choose a status.";
        notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        if (notes?.Length > MaxNotesLength) return $"The notes are too long ({MaxNotesLength} characters at most).";
        using var db = Db.Open();
        var i = await db.Inquiries.FirstOrDefaultAsync(x => x.InquiryId == inquiryId);
        if (i == null) return "This inquiry no longer exists.";
        i.Status = status;
        i.Notes = notes;
        i.UpdatedBy = Session.CurrentUser!.UserId;
        i.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        return null;
    }
}
