using System.Net.Mail;
using System.Text;
using System.Text.Json;
using Company.OfficeApp.Data;
using Company.OfficeApp.Services;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.WebsiteAdmin.Services;

/// <summary>When a group of website items was last saved, and by whom.</summary>
public record SavedStamp(DateTime At, string By);

/// <summary>
/// The website's single texts and images (website_content): Home hero, Contact details and
/// the inquiry recipient email. Save Changes writes here (not live); Publish Website makes it live
/// (<see cref="PublishService"/>).
/// </summary>
public static class WebsiteContentService
{
    /// <summary>Longest text allowed per item (Figma limits where it gives one).</summary>
    public static int MaxLength(string key) => key switch
    {
        WebsiteKeys.Headline => 150,
        WebsiteKeys.Subheadline => 300,
        WebsiteKeys.Copyright => 200,
        WebsiteKeys.Email or WebsiteKeys.RecipientEmail => 150,
        _ when key.EndsWith(".heading") => 100,
        _ => 255,
    };

    static bool IsImage(string key) => key.EndsWith("image");

    /// <summary>Items that may not be left empty.</summary>
    static string? Required(string key) => key switch
    {
        WebsiteKeys.Headline => "the main headline",
        WebsiteKeys.RecipientEmail => "the recipient email",
        _ when key.EndsWith(".heading") => "the section heading",
        _ => null,
    };

    public static async Task<Dictionary<string, string?>> LoadAsync(params string[] keys)
    {
        using var db = Db.Open();
        var rows = await db.WebsiteContent.AsNoTracking().Where(c => keys.Contains(c.ContentKey)).ToListAsync();
        return keys.ToDictionary(k => k, k => rows.FirstOrDefault(r => r.ContentKey == k)?.Value);
    }

    /// <summary>The latest save among these items, or null when none was ever saved in the app.</summary>
    public static async Task<SavedStamp?> LastSavedAsync(params string[] keys)
    {
        using var db = Db.Open();
        var last = await db.WebsiteContent.AsNoTracking().Where(c => keys.Contains(c.ContentKey) && c.UpdatedAt != null)
            .OrderByDescending(c => c.UpdatedAt).FirstOrDefaultAsync();
        if (last == null) return null;
        var who = await db.Users.Where(u => u.UserId == last.UpdatedBy).Select(u => u.FullName).FirstOrDefaultAsync();
        return new SavedStamp(last.UpdatedAt!.Value, who ?? "-");
    }

    /// <summary>
    /// Saves texts (<paramref name="texts"/>: key -> text) and new pictures (<paramref name="newImages"/>: key -> a
    /// local file picked by the user; null = remove the picture). Only changed items are written and audited.
    /// <paramref name="what"/> names the group in Edit History, e.g. "Home".
    /// </summary>
    public static async Task<string?> SaveAsync(string what, Dictionary<string, string?> texts, Dictionary<string, string?>? newImages = null)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        newImages ??= [];
        var clean = texts.ToDictionary(t => t.Key, t => string.IsNullOrWhiteSpace(t.Value) ? null : t.Value.Trim());
        foreach (var (key, value) in clean)
        {
            if (value == null && Required(key) is { } name) return $"Please fill in {name}.";
            if (value?.Length > MaxLength(key)) return $"Too long: {MaxLength(key)} characters at most ({value.Length} now).";
            if (value != null && (key == WebsiteKeys.Email || key == WebsiteKeys.RecipientEmail) && !IsEmail(value))
                return $"\"{value}\" isn't a valid email address.";
        }
        foreach (var path in newImages.Values)
            if (path != null && WebsiteImages.Check(path) is { } problem) return problem;

        using var db = Db.Open();
        var keys = clean.Keys.Concat(newImages.Keys).ToList();
        var rows = await db.WebsiteContent.Where(c => keys.Contains(c.ContentKey)).ToDictionaryAsync(c => c.ContentKey);
        var before = new Dictionary<string, string?>();
        var after = new Dictionary<string, string?>();

        WebsiteContent Row(string key)
        {
            if (rows.TryGetValue(key, out var r)) return r;
            r = new WebsiteContent { ContentKey = key };
            db.WebsiteContent.Add(r);
            rows[key] = r;
            return r;
        }
        void Set(string key, string? value, string? shown = null)
        {
            var r = Row(key);
            if (r.Value == value) return;
            before[key] = IsImage(key) ? (r.Value == null ? "(none)" : Path.GetFileName(r.Value)) : r.Value;
            after[key] = IsImage(key) ? shown ?? "(none)" : value;
            r.Value = value;
            r.UpdatedBy = Session.CurrentUser!.UserId;
            r.UpdatedAt = DateTime.Now;
        }

        foreach (var (key, value) in clean) Set(key, value);
        foreach (var (key, path) in newImages)
            Set(key, path == null ? null : await WebsiteImages.StoreAsync(path), path == null ? null : Path.GetFileName(path));

        if (after.Count == 0) return null;
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_content", 0, what, AuditActions.Edited, before, after);
        await db.SaveChangesAsync();
        return null;
    }

    static bool IsEmail(string s)
    {
        try { return new MailAddress(s).Address == s && s.Contains('.'); }
        catch (FormatException) { return false; }
    }
}

/// <summary>
/// Publish Website (owner decision 2026-11-03): writes everything saved (texts, pictures, visible projects with their
/// showcase photos, visible services, About Us sections) as Updated/content/site.json + the pictures into the
/// website's GitHub repository (<see cref="GitHubPublisher"/>); Netlify updates the live site from it in about a
/// minute and the site's js/content.js fills the pages from site.json. A copy of site.json is kept in website_publishes.
/// </summary>
public static class PublishService
{
    /// <summary>The website repository folder Publish owns (nothing else should be put there).</summary>
    public const string SiteFolder = "Updated/content";
    const string WebRoot = "/" + SiteFolder + "/";

    /// <summary>site.json and its pictures (repository path -> bytes), or the reason they can't be made.</summary>
    public record SiteFiles(string Json, Dictionary<string, byte[]> Files);

    public static async Task<string?> PublishAsync()
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        var settings = PublishSettings.Load();
        if (!settings.IsSet) return "Publishing isn't set up on this PC yet. Click Website Settings first.";

        var (site, problem) = await BuildAsync();
        if (problem != null) return problem;
        var who = Session.CurrentUser!;
        var error = await GitHubPublisher.PushAsync(settings, site!.Files, SiteFolder, $"Website Admin: published by {who.FullName}");
        if (error != null) return error;

        using var db = Db.Open();
        db.WebsitePublishes.Add(new WebsitePublish { PublishedBy = who.UserId, PublishedAt = DateTime.Now, Content = site.Json });
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Makes site.json (what the website reads) and collects the pictures it points to.</summary>
    public static async Task<(SiteFiles? Site, string? Problem)> BuildAsync()
    {
        using var db = Db.Open();
        var content = await db.WebsiteContent.AsNoTracking().ToDictionaryAsync(c => c.ContentKey, c => c.Value);
        string? Text(string key) => content.GetValueOrDefault(key);
        var projects = await (from w in db.WebsiteProjects.AsNoTracking()
                              join p in db.Projects on w.ProjectId equals p.ProjectId
                              where w.IsVisible
                              orderby w.SortOrder
                              select new { w.WebProjectId, p.ProjectName, p.SiteLocation, p.Sector, w.ShortDescription }).ToListAsync();
        var ids = projects.Select(p => p.WebProjectId).ToList();
        var photos = await db.WebsiteProjectPhotos.AsNoTracking().Where(f => ids.Contains(f.WebProjectId) && f.IsShowcase)
            .OrderBy(f => f.SortOrder).ToListAsync();
        var services = await db.WebsiteServices.AsNoTracking().Where(s => s.IsVisible).OrderBy(s => s.SortOrder).ToListAsync();
        var about = await db.WebsiteAboutSections.AsNoTracking().OrderBy(s => s.SortOrder).ThenBy(s => s.AboutSectionId).ToListAsync();

        // Pictures: stored path in the files folder -> address on the website (a web-safe copy of its file name).
        var files = new Dictionary<string, byte[]>();
        var missing = new List<string>();
        async Task<string?> Picture(string? stored)
        {
            if (string.IsNullOrEmpty(stored)) return null;   // none: the website keeps its own picture
            var name = string.Concat(Path.GetFileName(stored).Select(c => char.IsAsciiLetterOrDigit(c) || c is '.' or '-' or '_' ? c : '-'));
            var full = await FileStore.FullPathAsync(stored);
            if (!File.Exists(full)) { missing.Add(full); return null; }
            files[SiteFolder + "/images/" + name] = await File.ReadAllBytesAsync(full);
            return WebRoot + "images/" + name;
        }

        var site = new
        {
            published_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            home = new
            {
                headline = Text(WebsiteKeys.Headline),
                subheadline = Text(WebsiteKeys.Subheadline),
                image = await Picture(Text(WebsiteKeys.HeroImage)),
            },
            contact = new   // the inquiry recipient email is not part of the public site
            {
                address = Text(WebsiteKeys.Address),
                phone = Text(WebsiteKeys.Phone),
                email = Text(WebsiteKeys.Email),
                copyright = Text(WebsiteKeys.Copyright),
            },
            services = await OneByOne(services, async s => new
            {
                name = s.ServiceName, category = s.Sector, location = s.Location, description = s.ShortDescription,
                icon = s.Icon,
                icon_url = WebsiteServiceIcons.Get(s.Icon).Url,
                // no picture of its own: the website's picture for its icon (like the original 3 cards)
                image = await Picture(s.ImagePath) ?? "/Updated/images/" + WebsiteServiceIcons.Get(s.Icon).Picture,
            }),
            projects = await OneByOne(projects, async p => new
            {
                title = p.ProjectName, location = p.SiteLocation, category = p.Sector, description = p.ShortDescription,
                photos = (await OneByOne(photos.Where(f => f.WebProjectId == p.WebProjectId), f => Picture(f.FilePath)))
                         .OfType<string>().ToList(),
            }),
            // About Us rows in order: picture, heading + text, optional second heading + text
            about = await OneByOne(about, async s => new
            {
                heading = s.Heading, text = s.Body, second_heading = s.Heading2, second_text = s.Body2,
                image = await Picture(s.ImagePath),
            }),
        };
        if (missing.Count > 0)
            return (null, "These pictures are missing from the files folder, so the website can't show them:\n" +
                          string.Join("\n", missing.Distinct()) + "\nChoose them again on their page, save, then publish.");

        var json = JsonSerializer.Serialize(site, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,   // keep ₱, ñ readable
        });
        files[SiteFolder + "/site.json"] = Encoding.UTF8.GetBytes(json);
        return (new SiteFiles(json, files), null);
    }

    /// <summary>Runs <paramref name="make"/> for each item in turn (pictures are read one at a time).</summary>
    static async Task<List<TOut>> OneByOne<TIn, TOut>(IEnumerable<TIn> items, Func<TIn, Task<TOut>> make)
    {
        var list = new List<TOut>();
        foreach (var item in items) list.Add(await make(item));
        return list;
    }

    public static async Task<SavedStamp?> LastAsync()
    {
        using var db = Db.Open();
        var last = await db.WebsitePublishes.AsNoTracking().OrderByDescending(p => p.PublishedAt)
            .Select(p => new { p.PublishedAt, p.PublishedBy }).FirstOrDefaultAsync();
        if (last == null) return null;
        var who = await db.Users.Where(u => u.UserId == last.PublishedBy).Select(u => u.FullName).FirstOrDefaultAsync();
        return new SavedStamp(last.PublishedAt, who ?? "-");
    }
}
