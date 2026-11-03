using System.Globalization;
using System.Text.Json.Nodes;
using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.WebsiteAdmin.Services;

/// <summary>
/// Gets new website inquiries (owner decision 2026-11-03): the website's Contact form posts to its Google Apps Script,
/// which emails Gmail and saves each message as a row in a Google Sheet. Contact Us (and the sidebar count) asks the
/// script for the rows (docs/_tools/google-script/Code.gs, doGet with the read key) and adds the ones not copied yet
/// to inquiries as New. inquiries.source_ref (script 016) keeps each message from being added twice.
/// </summary>
public static class InquiryImport
{
    /// <summary>How many new inquiries were added, or why none could be fetched (null = fine or not set up).</summary>
    public record Result(int Added, string? Problem);

    public static async Task<Result> PullAsync()
    {
        if (WebsiteAccess.Require() is { } denied) return new(0, denied);
        var settings = PublishSettings.Load();
        if (!settings.InquiriesSet) return new(0, null);   // not set up on this PC: just show what's in the database

        var (rows, problem) = await FetchAsync(settings);
        if (problem != null) return new(0, problem);

        using var db = Db.Open();
        var ids = rows!.Select(r => r.Id).ToList();
        var known = (await db.Inquiries.Where(i => i.SourceRef != null && ids.Contains(i.SourceRef)).Select(i => i.SourceRef!).ToListAsync())
            .ToHashSet();
        var added = rows!.Where(r => !known.Contains(r.Id)).DistinctBy(r => r.Id).Select(ToInquiry).ToList();
        if (added.Count == 0) return new(0, null);
        db.Inquiries.AddRange(added);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException) { return new(0, null); }   // another PC added them at the same moment
        return new(added.Count, null);
    }

    /// <summary>Checks the address and key. Null when they work, otherwise the reason.</summary>
    public static async Task<(int Count, string? Problem)> TestAsync(PublishSettings settings)
    {
        var (rows, problem) = await FetchAsync(settings);
        return (rows?.Count ?? 0, problem);
    }

    /// <summary>
    /// Tells the website's Google Script where to email new inquiries (Contact Us > Save Email). Null when done,
    /// otherwise the reason (e.g. inquiries not set up on this PC).
    /// </summary>
    public static async Task<string?> SetRecipientAsync(string email)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        var settings = PublishSettings.Load();
        if (!settings.InquiriesSet) return "Inquiries aren't set up on this PC yet (Website Settings).";
        try
        {
            // Google answers a POST with a redirect to the result, which HttpClient follows.
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            var body = new JsonObject { ["action"] = "settings", ["key"] = settings.InquiryKey, ["recipient"] = email }.ToJsonString();
            using var response = await http.PostAsync(settings.InquiryScriptUrl, new StringContent(body, System.Text.Encoding.UTF8, "text/plain"));
            var text = await response.Content.ReadAsStringAsync();
            JsonNode? json;
            try { json = JsonNode.Parse(text); }
            catch (System.Text.Json.JsonException) { return "The Google Script didn't answer like expected. Check that its new code was deployed."; }
            if (json?["ok"]?.GetValue<bool>() == true) return null;
            return (string?)json?["error"] switch
            {
                "wrong key" => "The Google Script refused the read key (Website Settings).",
                "not an email address" => "The Google Script says that isn't an email address.",
                var other => "The Google Script reported a problem: " + other,
            };
        }
        catch (HttpRequestException) { return "Can't reach the Google Script. Check this PC's internet connection."; }
        catch (TaskCanceledException) { return "The Google Script took too long to answer."; }
    }

    record SheetRow(string Id, string Received, string Name, string Email, string Phone, string Service, string Location, string Message);

    static async Task<(List<SheetRow>? Rows, string? Problem)> FetchAsync(PublishSettings settings)
    {
        if (!settings.InquiriesSet) return (null, "Inquiries aren't set up on this PC yet (Website Settings).");
        try
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };   // follows Google's redirect
            var text = await http.GetStringAsync(settings.InquiryScriptUrl + "?key=" + Uri.EscapeDataString(settings.InquiryKey));
            JsonNode? json;
            try { json = JsonNode.Parse(text); }
            catch (System.Text.Json.JsonException) { return (null, "The Google Script didn't answer like expected. Check its address and that the new code was deployed."); }
            if (json?["ok"]?.GetValue<bool>() != true)
                return (null, (string?)json?["error"] == "wrong key"
                    ? "The Google Script refused the read key. Copy it again from the script (run setup)."
                    : "The Google Script reported a problem: " + (string?)json?["error"]);
            string S(JsonNode? r, string name) => r?[name]?.ToString().Trim() ?? "";
            var rows = json["rows"]!.AsArray().Select(r => new SheetRow(S(r, "id"), S(r, "received"), S(r, "name"), S(r, "email"),
                S(r, "phone"), S(r, "service"), S(r, "location"), S(r, "message"))).Where(r => r.Id != "").ToList();
            return (rows, null);
        }
        catch (HttpRequestException) { return (null, "Can't reach the Google Script. Check this PC's internet connection."); }
        catch (TaskCanceledException) { return (null, "The Google Script took too long to answer."); }
    }

    static Inquiry ToInquiry(SheetRow r)
    {
        static string? Opt(string s, int max) => s == "" ? null : s.Length > max ? s[..max] : s;
        var received = DateTime.TryParseExact(r.Received, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var at)
            ? at : DateTime.Now;   // the script writes Philippine time, the same as the office PCs
        var email = Opt(r.Email, 150);
        var phone = Opt(r.Phone, 40);
        return new Inquiry
        {
            SourceRef = r.Id.Length > 64 ? r.Id[..64] : r.Id,
            ReceivedAt = received,
            FullName = Opt(r.Name, 100) ?? "(no name)",
            Email = email,
            Phone = phone ?? (email == null ? "(none)" : null),   // the table needs a way to reach them
            ServiceRequired = Opt(r.Service, 100),
            ProjectLocation = Opt(r.Location, 255),
            Message = Opt(r.Message, 4000) ?? "(no message)",
            Status = InquiryStatuses.New,
        };
    }
}
