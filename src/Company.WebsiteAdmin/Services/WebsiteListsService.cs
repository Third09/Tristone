using Company.OfficeApp.Data;
using Company.OfficeApp.Services;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.WebsiteAdmin.Services;

/// <summary>One featured project in the Projects editor's list.</summary>
public record WebProjectRow(int WebProjectId, int Slot, string ProjectName, string Location, string Sector, string Shown,
                            int Photos, string? ShortDescription);

/// <summary>An office project that can be added to the website.</summary>
public record ProjectChoice(int ProjectId, string Text);

/// <summary>One photo of a featured project.</summary>
public record PhotoRow(int PhotoId, int Order, string FileName, string Size, string Showcase, string FilePath);

/// <summary>
/// Website Admin > Projects: featured projects. They are picked from the office Projects list (ticked "Show on
/// website"); name, site and sector come from there (owner decision 2026-11-01). The website adds a short
/// description and up to 10 photos. Unpublish hides a project; Delete removes it from the website only.
/// </summary>
public static class WebProjectService
{
    public const int MaxPhotos = 10;
    public const int MaxDescription = 100;

    public static async Task<List<WebProjectRow>> ListAsync()
    {
        using var db = Db.Open();
        var rows = await (from w in db.WebsiteProjects.AsNoTracking()
                          join p in db.Projects on w.ProjectId equals p.ProjectId
                          orderby w.SortOrder, w.WebProjectId
                          select new { w, p.ProjectName, p.SiteLocation, p.Sector,
                                       Photos = db.WebsiteProjectPhotos.Count(f => f.WebProjectId == w.WebProjectId) }).ToListAsync();
        return rows.Select((r, i) => new WebProjectRow(r.w.WebProjectId, i + 1, r.ProjectName, r.SiteLocation, r.Sector,
                                                        r.w.IsVisible ? "Visible" : "Hidden", r.Photos, r.w.ShortDescription)).ToList();
    }

    /// <summary>Office projects ticked "Show on website" that aren't featured yet.</summary>
    public static async Task<List<ProjectChoice>> ChoicesAsync()
    {
        using var db = Db.Open();
        return await db.Projects.AsNoTracking()
            .Where(p => p.ShowOnWebsite && !db.WebsiteProjects.Any(w => w.ProjectId == p.ProjectId))
            .OrderBy(p => p.ProjectName)
            .Select(p => new ProjectChoice(p.ProjectId, p.ProjectName + " - " + p.SiteLocation))
            .ToListAsync();
    }

    public static async Task<(int? Id, string? Error)> AddAsync(int projectId, string? description)
    {
        if (WebsiteAccess.Require() is { } denied) return (null, denied);
        description = Clean(description);
        if (description?.Length > MaxDescription) return (null, $"The short description is too long ({MaxDescription} characters at most).");
        using var db = Db.Open();
        var p = await db.Projects.FirstOrDefaultAsync(x => x.ProjectId == projectId);
        if (p == null) return (null, "That project no longer exists.");
        if (!p.ShowOnWebsite) return (null, "Tick \"Show on website\" for this project in the office app's Projects page first.");
        if (await db.WebsiteProjects.AnyAsync(w => w.ProjectId == projectId)) return (null, "This project is already on the website.");
        var w = new WebsiteProject
        {
            ProjectId = projectId, ShortDescription = description, IsVisible = true,
            SortOrder = (await db.WebsiteProjects.MaxAsync(x => (int?)x.SortOrder) ?? 0) + 1,
            CreatedBy = Session.CurrentUser!.UserId, CreatedAt = DateTime.Now,
        };
        db.WebsiteProjects.Add(w);
        await db.SaveChangesAsync();
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_projects", w.WebProjectId, p.ProjectName, AuditActions.Added,
                         null, new { description, shown = "Visible" });
        await db.SaveChangesAsync();
        return (w.WebProjectId, null);
    }

    public static async Task<string?> UpdateAsync(int webProjectId, string? description)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        description = Clean(description);
        if (description?.Length > MaxDescription) return $"The short description is too long ({MaxDescription} characters at most).";
        using var db = Db.Open();
        var w = await db.WebsiteProjects.FirstOrDefaultAsync(x => x.WebProjectId == webProjectId);
        if (w == null) return "This project is no longer on the website.";
        if (w.ShortDescription == description) return null;
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_projects", w.WebProjectId, await NameAsync(db, w.ProjectId),
                         AuditActions.Edited, new { description = w.ShortDescription }, new { description });
        w.ShortDescription = description;
        w.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Publish / Unpublish one project (hidden ones stay here).</summary>
    public static async Task<string?> SetVisibleAsync(int webProjectId, bool visible)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var w = await db.WebsiteProjects.FirstAsync(x => x.WebProjectId == webProjectId);
        if (w.IsVisible == visible) return null;
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_projects", w.WebProjectId, await NameAsync(db, w.ProjectId),
                         AuditActions.Edited, new { shown = w.IsVisible ? "Visible" : "Hidden" }, new { shown = visible ? "Visible" : "Hidden" });
        w.IsVisible = visible;
        w.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Moves a project up (-1) or down (+1) the list; the website shows them in this order.</summary>
    public static async Task<string?> MoveAsync(int webProjectId, int direction)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var all = await db.WebsiteProjects.OrderBy(x => x.SortOrder).ThenBy(x => x.WebProjectId).ToListAsync();
        Ordering.Move(all, all.FindIndex(x => x.WebProjectId == webProjectId), direction, (x, n) => x.SortOrder = n);
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Removes the project from the website (the office project and its records stay).</summary>
    public static async Task<string?> DeleteAsync(int webProjectId)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var w = await db.WebsiteProjects.FirstOrDefaultAsync(x => x.WebProjectId == webProjectId);
        if (w == null) return null;
        var photos = await db.WebsiteProjectPhotos.CountAsync(f => f.WebProjectId == webProjectId);
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_projects", w.WebProjectId, await NameAsync(db, w.ProjectId),
                         AuditActions.Deleted, new { description = w.ShortDescription, photos }, null);
        db.WebsiteProjects.Remove(w);   // its photos go with it (ON DELETE CASCADE)
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<List<PhotoRow>> PhotosAsync(int webProjectId)
    {
        using var db = Db.Open();
        var photos = await db.WebsiteProjectPhotos.AsNoTracking().Where(f => f.WebProjectId == webProjectId)
            .OrderBy(f => f.SortOrder).ThenBy(f => f.PhotoId).ToListAsync();
        return photos.Select((f, i) => new PhotoRow(f.PhotoId, i + 1, f.FileName, $"{f.SizeBytes / 1024.0 / 1024.0:0.0} MB",
                                                     f.IsShowcase ? "Yes" : "No", f.FilePath)).ToList();
    }

    /// <summary>
    /// Uploads pictures one by one. Returns how many were added and the problems with the others (wrong type,
    /// over 8 MB, or over the 10-photo limit), so one bad file doesn't stop the rest.
    /// </summary>
    public static async Task<(int Added, List<string> Problems)> AddPhotosAsync(int webProjectId, IEnumerable<string> paths,
                                                                               IProgress<int>? progress = null)
    {
        var problems = new List<string>();
        if (WebsiteAccess.Require() is { } denied) return (0, [denied]);
        using var db = Db.Open();
        var w = await db.WebsiteProjects.FirstOrDefaultAsync(x => x.WebProjectId == webProjectId);
        if (w == null) return (0, ["This project is no longer on the website."]);
        int count = await db.WebsiteProjectPhotos.CountAsync(f => f.WebProjectId == webProjectId);
        int order = await db.WebsiteProjectPhotos.Where(f => f.WebProjectId == webProjectId).MaxAsync(f => (int?)f.SortOrder) ?? 0;
        int added = 0, done = 0;
        var names = new List<string>();
        foreach (var path in paths)
        {
            progress?.Report(done++);
            var name = Path.GetFileName(path);
            if (count >= MaxPhotos) { problems.Add($"{name}: a project can have {MaxPhotos} photos at most."); continue; }
            if (WebsiteImages.Check(path) is { } problem) { problems.Add($"{name}: {problem}"); continue; }
            try
            {
                var stored = await WebsiteImages.StoreAsync(path);
                db.WebsiteProjectPhotos.Add(new WebsiteProjectPhoto
                {
                    WebProjectId = webProjectId, FilePath = stored, FileName = name, SizeBytes = new FileInfo(path).Length,
                    SortOrder = ++order, IsShowcase = true, UploadedBy = Session.CurrentUser!.UserId, UploadedAt = DateTime.Now,
                });
                count++; added++; names.Add(name);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { problems.Add($"{name}: upload failed ({ex.Message}). Please retry."); }
        }
        progress?.Report(done);
        if (added > 0)
        {
            AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_project_photos", webProjectId, await NameAsync(db, w.ProjectId),
                             AuditActions.Added, null, new { photos = string.Join(", ", names) });
            await db.SaveChangesAsync();
        }
        return (added, problems);
    }

    public static async Task<string?> RemovePhotoAsync(int photoId)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var f = await db.WebsiteProjectPhotos.FirstOrDefaultAsync(x => x.PhotoId == photoId);
        if (f == null) return null;
        var w = await db.WebsiteProjects.FirstAsync(x => x.WebProjectId == f.WebProjectId);
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_project_photos", f.WebProjectId, await NameAsync(db, w.ProjectId),
                         AuditActions.Deleted, new { photo = f.FileName }, null);
        db.WebsiteProjectPhotos.Remove(f);
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Showcase photos are the ones in the website's carousel, in this order.</summary>
    public static async Task<string?> SetShowcaseAsync(int photoId, bool showcase)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var f = await db.WebsiteProjectPhotos.FirstAsync(x => x.PhotoId == photoId);
        f.IsShowcase = showcase;
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> MovePhotoAsync(int photoId, int direction)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var f = await db.WebsiteProjectPhotos.FirstAsync(x => x.PhotoId == photoId);
        var all = await db.WebsiteProjectPhotos.Where(x => x.WebProjectId == f.WebProjectId).OrderBy(x => x.SortOrder).ThenBy(x => x.PhotoId).ToListAsync();
        Ordering.Move(all, all.FindIndex(x => x.PhotoId == photoId), direction, (x, n) => x.SortOrder = n);
        await db.SaveChangesAsync();
        return null;
    }

    static async Task<string> NameAsync(AppDbContext db, int projectId) =>
        await db.Projects.Where(p => p.ProjectId == projectId).Select(p => p.ProjectName).FirstOrDefaultAsync() ?? "project";

    static string? Clean(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}

/// <summary>One service card in the Services editor's list.</summary>
public record WebServiceRow(int WebServiceId, int Slot, string ServiceName, string Sector, string? Location, string Shown, string? ShortDescription);

/// <summary>What the Services form edits.</summary>
public record WebServiceInput(string ServiceName, string Sector, string? Location, string? ShortDescription,
                              string Icon = WebsiteServiceIcons.Ruler);

/// <summary>
/// Website Admin > Services: the service cards: a typed name (the 3 original services are offered; owner request
/// 2026-11-03), a sector (the office sectors), a typed location, a short description, one of the 3 website icons and an
/// optional picture (none = the website's own picture for that icon, like the original 3).
/// </summary>
public static class WebServiceService
{
    public const int MaxDescription = 100;
    public const int MaxLocation = 100;
    public const int MaxName = 60;

    public static async Task<List<WebServiceRow>> ListAsync()
    {
        using var db = Db.Open();
        var rows = await db.WebsiteServices.AsNoTracking().OrderBy(s => s.SortOrder).ThenBy(s => s.WebServiceId).ToListAsync();
        return rows.Select((s, i) => new WebServiceRow(s.WebServiceId, i + 1, s.ServiceName, s.Sector, s.Location,
                                                        s.IsVisible ? "Visible" : "Hidden", s.ShortDescription)).ToList();
    }

    public static async Task<WebsiteService?> GetAsync(int id)
    {
        using var db = Db.Open();
        return await db.WebsiteServices.AsNoTracking().FirstOrDefaultAsync(s => s.WebServiceId == id);
    }

    static string? Check(WebServiceInput i, string? newImage)
    {
        // Any service can be typed (owner request 2026-11-03); the 3 original ones are offered in the list.
        if (i.ServiceName == "") return "Please type or choose the name of the service.";
        if (i.ServiceName.Length > MaxName) return $"The name is too long ({MaxName} characters at most).";
        // No picture needed: the website then shows its own picture for the chosen icon (like the original 3).
        if (!WebsiteServiceIcons.All.Contains(i.Icon)) return "Choose an icon.";
        if (!ProjectSectors.All.Contains(i.Sector)) return "Choose a category.";
        if (i.Location?.Length > MaxLocation) return $"The location is too long ({MaxLocation} characters at most).";
        if (i.ShortDescription?.Length > MaxDescription) return $"The short description is too long ({MaxDescription} characters at most).";
        if (newImage != null && WebsiteImages.Check(newImage) is { } problem) return problem;
        return null;
    }

    static WebServiceInput Clean(WebServiceInput i) => i with
    {
        ServiceName = WebsiteServiceNames.All.FirstOrDefault(n => n.Equals(i.ServiceName.Trim(), StringComparison.OrdinalIgnoreCase))
                      ?? string.Join(' ', i.ServiceName.Split(' ', StringSplitOptions.RemoveEmptyEntries)),
        Location = string.IsNullOrWhiteSpace(i.Location) ? null : i.Location.Trim(),
        ShortDescription = string.IsNullOrWhiteSpace(i.ShortDescription) ? null : i.ShortDescription.Trim(),
    };

    static object Snapshot(WebsiteService s) => new
    {
        service = s.ServiceName, category = s.Sector, location = s.Location, description = s.ShortDescription, icon = s.Icon,
        picture = s.ImagePath == null ? "(none)" : Path.GetFileName(s.ImagePath), shown = s.IsVisible ? "Visible" : "Hidden",
    };

    /// <summary><paramref name="newImage"/> = a local picture to upload (null = none yet).</summary>
    public static async Task<(int? Id, string? Error)> CreateAsync(WebServiceInput input, string? newImage)
    {
        if (WebsiteAccess.Require() is { } denied) return (null, denied);
        input = Clean(input);
        if (Check(input, newImage) is { } problem) return (null, problem);
        using var db = Db.Open();
        var s = new WebsiteService
        {
            ServiceName = input.ServiceName, Sector = input.Sector, Location = input.Location, ShortDescription = input.ShortDescription,
            Icon = input.Icon,
            ImagePath = newImage == null ? null : await WebsiteImages.StoreAsync(newImage), IsVisible = true,
            SortOrder = (await db.WebsiteServices.MaxAsync(x => (int?)x.SortOrder) ?? 0) + 1,
            CreatedBy = Session.CurrentUser!.UserId, CreatedAt = DateTime.Now,
        };
        db.WebsiteServices.Add(s);
        await db.SaveChangesAsync();
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_services", s.WebServiceId, s.ServiceName, AuditActions.Added, null, Snapshot(s));
        await db.SaveChangesAsync();
        return (s.WebServiceId, null);
    }

    /// <summary><paramref name="newImage"/> = a new local picture to upload; null keeps the current one.</summary>
    public static async Task<string?> UpdateAsync(int id, WebServiceInput input, string? newImage)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        input = Clean(input);
        using var db = Db.Open();
        var s = await db.WebsiteServices.FirstOrDefaultAsync(x => x.WebServiceId == id);
        if (s == null) return "This service no longer exists.";
        if (Check(input, newImage) is { } problem) return problem;
        var before = Snapshot(s);
        s.ServiceName = input.ServiceName;
        s.Sector = input.Sector;
        s.Location = input.Location;
        s.ShortDescription = input.ShortDescription;
        s.Icon = input.Icon;
        if (newImage != null) s.ImagePath = await WebsiteImages.StoreAsync(newImage);
        s.UpdatedAt = DateTime.Now;
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_services", s.WebServiceId, s.ServiceName, AuditActions.Edited, before, Snapshot(s));
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> SetVisibleAsync(int id, bool visible)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var s = await db.WebsiteServices.FirstAsync(x => x.WebServiceId == id);
        if (s.IsVisible == visible) return null;
        var before = Snapshot(s);
        s.IsVisible = visible;
        s.UpdatedAt = DateTime.Now;
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_services", s.WebServiceId, s.ServiceName, AuditActions.Edited, before, Snapshot(s));
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> MoveAsync(int id, int direction)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var all = await db.WebsiteServices.OrderBy(x => x.SortOrder).ThenBy(x => x.WebServiceId).ToListAsync();
        Ordering.Move(all, all.FindIndex(x => x.WebServiceId == id), direction, (x, n) => x.SortOrder = n);
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> DeleteAsync(int id)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var s = await db.WebsiteServices.FirstOrDefaultAsync(x => x.WebServiceId == id);
        if (s == null) return null;
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_services", s.WebServiceId, s.ServiceName, AuditActions.Deleted, Snapshot(s), null);
        db.WebsiteServices.Remove(s);
        await db.SaveChangesAsync();
        return null;
    }
}

/// <summary>What the About Us form edits. <see cref="Heading2"/> and <see cref="Body2"/> are the optional second part.</summary>
public record AboutSectionInput(string Heading, string? Body, string? Heading2, string? Body2);

/// <summary>
/// Website Admin > About Us: the sections (website_about_sections, script 015; owner request 2026-11-03). Any number,
/// shown on the website in this order; any one can be deleted as long as one stays. Each section has a picture, a
/// heading and its text, and optionally a second heading and text (like Vision and Mission).
/// </summary>
public static class WebAboutService
{
    public const int MaxHeading = 100;
    public const int MaxBody = 2000;

    public static async Task<List<WebsiteAboutSection>> ListAsync()
    {
        using var db = Db.Open();
        return await db.WebsiteAboutSections.AsNoTracking().OrderBy(s => s.SortOrder).ThenBy(s => s.AboutSectionId).ToListAsync();
    }

    public static async Task<SavedStamp?> LastSavedAsync(int id)
    {
        using var db = Db.Open();
        var s = await db.WebsiteAboutSections.AsNoTracking().FirstOrDefaultAsync(x => x.AboutSectionId == id);
        if (s?.UpdatedAt == null) return null;
        var who = await db.Users.Where(u => u.UserId == s.UpdatedBy).Select(u => u.FullName).FirstOrDefaultAsync();
        return new SavedStamp(s.UpdatedAt.Value, who ?? "-");
    }

    static AboutSectionInput Clean(AboutSectionInput i)
    {
        static string? C(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        return new(i.Heading.Trim(), C(i.Body), C(i.Heading2), C(i.Body2));
    }

    static string? Check(AboutSectionInput i, string? newImage)
    {
        if (i.Heading.Length == 0) return "Please fill in the section heading.";
        if (i.Heading.Length > MaxHeading || i.Heading2?.Length > MaxHeading) return $"A heading is too long ({MaxHeading} characters at most).";
        if (i.Body?.Length > MaxBody || i.Body2?.Length > MaxBody) return $"A text is too long ({MaxBody} characters at most).";
        if (i.Heading2 == null && i.Body2 != null) return "Please fill in the second heading, or clear the second text.";
        if (newImage != null && WebsiteImages.Check(newImage) is { } problem) return problem;
        return null;
    }

    static object Snapshot(WebsiteAboutSection s) => new
    {
        heading = s.Heading, text = s.Body, second_heading = s.Heading2, second_text = s.Body2,
        picture = s.ImagePath == null ? "(none)" : Path.GetFileName(s.ImagePath),
    };

    /// <summary>Adds a section at the end. <paramref name="newImage"/> = a local picture to upload (null = none yet).</summary>
    public static async Task<(int? Id, string? Error)> CreateAsync(AboutSectionInput input, string? newImage)
    {
        if (WebsiteAccess.Require() is { } denied) return (null, denied);
        input = Clean(input);
        if (Check(input, newImage) is { } problem) return (null, problem);
        using var db = Db.Open();
        var s = new WebsiteAboutSection
        {
            Heading = input.Heading, Body = input.Body, Heading2 = input.Heading2, Body2 = input.Body2,
            ImagePath = newImage == null ? null : await WebsiteImages.StoreAsync(newImage),
            SortOrder = (await db.WebsiteAboutSections.MaxAsync(x => (int?)x.SortOrder) ?? 0) + 1,
            CreatedBy = Session.CurrentUser!.UserId, CreatedAt = DateTime.Now,
            UpdatedBy = Session.CurrentUser!.UserId, UpdatedAt = DateTime.Now,
        };
        db.WebsiteAboutSections.Add(s);
        await db.SaveChangesAsync();
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_about_sections", s.AboutSectionId, "About Us: " + s.Heading,
                         AuditActions.Added, null, Snapshot(s));
        await db.SaveChangesAsync();
        return (s.AboutSectionId, null);
    }

    /// <summary><paramref name="newImage"/> = a new local picture; <paramref name="removeImage"/> clears it; neither keeps it.</summary>
    public static async Task<string?> UpdateAsync(int id, AboutSectionInput input, string? newImage, bool removeImage)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        input = Clean(input);
        if (Check(input, newImage) is { } problem) return problem;
        using var db = Db.Open();
        var s = await db.WebsiteAboutSections.FirstOrDefaultAsync(x => x.AboutSectionId == id);
        if (s == null) return "This section no longer exists.";
        var before = Snapshot(s);
        s.Heading = input.Heading;
        s.Body = input.Body;
        s.Heading2 = input.Heading2;
        s.Body2 = input.Body2;
        if (newImage != null) s.ImagePath = await WebsiteImages.StoreAsync(newImage);
        else if (removeImage) s.ImagePath = null;
        s.UpdatedBy = Session.CurrentUser!.UserId;
        s.UpdatedAt = DateTime.Now;
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_about_sections", s.AboutSectionId, "About Us: " + s.Heading,
                         AuditActions.Edited, before, Snapshot(s));
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> MoveAsync(int id, int direction)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var all = await db.WebsiteAboutSections.OrderBy(x => x.SortOrder).ThenBy(x => x.AboutSectionId).ToListAsync();
        Ordering.Move(all, all.FindIndex(x => x.AboutSectionId == id), direction, (x, n) => x.SortOrder = n);
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Removes a section from the website. The last one can't be deleted (About Us would be empty).</summary>
    public static async Task<string?> DeleteAsync(int id)
    {
        if (WebsiteAccess.Require() is { } denied) return denied;
        using var db = Db.Open();
        var s = await db.WebsiteAboutSections.FirstOrDefaultAsync(x => x.AboutSectionId == id);
        if (s == null) return null;
        if (await db.WebsiteAboutSections.CountAsync() <= 1) return "About Us needs at least one section. Add another one first.";
        AuditService.Add(db, WebsiteAccess.AuditDepartment, "website_about_sections", s.AboutSectionId, "About Us: " + s.Heading,
                         AuditActions.Deleted, Snapshot(s), null);
        db.WebsiteAboutSections.Remove(s);
        await db.SaveChangesAsync();
        var rest = await db.WebsiteAboutSections.OrderBy(x => x.SortOrder).ThenBy(x => x.AboutSectionId).ToListAsync();
        Ordering.Move(rest, -1, 0, (x, n) => x.SortOrder = n);   // renumber 1, 2, 3...
        await db.SaveChangesAsync();
        return null;
    }
}

/// <summary>Moving an item up or down a list stored with a sort_order column.</summary>
static class Ordering
{
    /// <summary>Swaps item <paramref name="index"/> with its neighbour and renumbers the whole list 1, 2, 3...</summary>
    public static void Move<T>(List<T> items, int index, int direction, Action<T, int> setOrder)
    {
        int other = index + direction;
        if (index >= 0 && other >= 0 && other < items.Count) (items[index], items[other]) = (items[other], items[index]);
        for (int i = 0; i < items.Count; i++) setOrder(items[i], i + 1);
    }
}
