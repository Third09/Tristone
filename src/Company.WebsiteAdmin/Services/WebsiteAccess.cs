using Company.OfficeApp.Services;

namespace Company.WebsiteAdmin.Services;

/// <summary>
/// Who may change the website: Website Admin logins and office Admins (owner decision 2026-11-01: Admins can edit
/// in the Website Admin app, unlike the office pages). Every service re-checks this, not just the screens.
/// </summary>
public static class WebsiteAccess
{
    /// <summary>Edit History department for website changes (audit_logs.department).</summary>
    public const string AuditDepartment = "Website";

    public static string? Require() =>
        Session.CurrentUser is { CanOpenWebsiteAdmin: true } ? null : "Only Website Admins and Admins can do this.";
}

/// <summary>
/// Website pictures: JPEG or PNG, 8 MB at most (Figma), kept in the shared files folder under "website".
/// </summary>
public static class WebsiteImages
{
    public const long MaxBytes = 8 * 1024 * 1024;
    public const string Filter = "Pictures (JPEG, PNG)|*.jpg;*.jpeg;*.png";
    static readonly string[] Extensions = [".jpg", ".jpeg", ".png"];

    /// <summary>Null when the picture can be used, otherwise the reason.</summary>
    public static string? Check(string path)
    {
        if (!File.Exists(path)) return "That file doesn't exist anymore.";
        if (!Extensions.Contains(Path.GetExtension(path).ToLowerInvariant())) return "Only JPEG or PNG pictures can be used.";
        if (new FileInfo(path).Length > MaxBytes) return $"\"{Path.GetFileName(path)}\" is too big (8 MB at most).";
        return null;
    }

    /// <summary>Copies the picture into the files folder; returns the relative path to store.</summary>
    public static async Task<string> StoreAsync(string path)
    {
        if (Check(path) is { } problem) throw new InvalidOperationException(problem);
        return await FileStore.SaveAsync(path, "website");
    }

    /// <summary>
    /// Loads a picture for showing (a stored relative path, or a local file not saved yet). Read into memory so the
    /// file isn't kept locked. Null when it's missing or can't be read.
    /// </summary>
    public static async Task<Image?> LoadAsync(string? storedPath, string? localPath = null)
    {
        try
        {
            var full = localPath ?? (storedPath == null ? null : await FileStore.FullPathAsync(storedPath));
            if (full == null || !File.Exists(full)) return null;
            var bytes = await File.ReadAllBytesAsync(full);
            using var ms = new MemoryStream(bytes);
            using var img = Image.FromStream(ms);
            return new Bitmap(img);
        }
        catch { return null; }
    }
}
