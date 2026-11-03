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

    /// <summary>Longest side of a website picture after Shrink (owner request 2026-11-03): sharp on big screens, quick to load.</summary>
    public const int MaxSide = 1600;

    /// <summary>
    /// Copies the picture into the files folder; returns the relative path to store. Big pictures are shrunk first
    /// (<see cref="Shrink"/>), so the website loads faster; the original file is never changed.
    /// </summary>
    public static async Task<string> StoreAsync(string path)
    {
        if (Check(path) is { } problem) throw new InvalidOperationException(problem);
        var smaller = Shrink(path);
        try { return await FileStore.SaveAsync(smaller ?? path, "website"); }
        finally { if (smaller != null) Directory.Delete(Path.GetDirectoryName(smaller)!, recursive: true); }
    }

    /// <summary>
    /// A smaller copy (in a temporary folder, same file name) when the picture is wider or taller than
    /// <see cref="MaxSide"/> or over 1 MB; null when it is already fine. JPEG stays JPEG (quality 85); PNG stays PNG.
    /// </summary>
    public static string? Shrink(string path)
    {
        try
        {
            using var original = Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
            var big = Math.Max(original.Width, original.Height);
            if (big <= MaxSide && new FileInfo(path).Length <= 1024 * 1024) return null;
            var scale = Math.Min(1.0, (double)MaxSide / big);
            int w = Math.Max(1, (int)Math.Round(original.Width * scale)), h = Math.Max(1, (int)Math.Round(original.Height * scale));
            using var resized = new Bitmap(w, h);
            using (var g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, 0, 0, w, h);
            }
            var folder = Directory.CreateTempSubdirectory("tristone-pic").FullName;
            var target = Path.Combine(folder, Path.GetFileName(path));
            if (Path.GetExtension(path).Equals(".png", StringComparison.OrdinalIgnoreCase)) resized.Save(target, System.Drawing.Imaging.ImageFormat.Png);
            else
            {
                var jpeg = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid);
                using var quality = new System.Drawing.Imaging.EncoderParameters(1);
                quality.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
                resized.Save(target, jpeg, quality);
            }
            // Keep the original when "shrinking" didn't make it smaller (e.g. an already small PNG over 1 MB at full size).
            if (new FileInfo(target).Length >= new FileInfo(path).Length && scale >= 1.0) { Directory.Delete(folder, recursive: true); return null; }
            return target;
        }
        catch { return null; }   // can't read it as a picture here: store it as it is
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
