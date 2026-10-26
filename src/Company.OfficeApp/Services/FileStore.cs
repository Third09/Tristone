using System.Diagnostics;
using Company.OfficeApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>
/// Uploaded scans, photos and documents. Files are copied into one shared folder; the database only keeps
/// the path relative to that folder, so it never fills up with files.
/// The folder is the "file_storage_path" app setting (C:\TriStoneFiles on the development PC; a shared
/// network folder on the office server in Phase 6), so every PC uses the same one.
/// </summary>
public static class FileStore
{
    public const string SettingKey = "file_storage_path";
    const long MaxBytes = 25 * 1024 * 1024;   // 25 MB per file

    public static readonly string[] ScanExtensions = [".pdf", ".jpg", ".jpeg", ".png", ".bmp", ".tif", ".tiff", ".doc", ".docx"];
    public const string ScanFilter = "Scans and documents|*.pdf;*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.doc;*.docx";

    static string? cachedRoot;

    public static async Task<string> RootAsync()
    {
        if (cachedRoot != null) return cachedRoot;
        using var db = Db.Open();
        cachedRoot = await db.AppSettings.Where(s => s.Key == SettingKey).Select(s => s.Value).FirstOrDefaultAsync()
                     ?? @"C:\TriStoneFiles";
        return cachedRoot;
    }

    /// <summary>Null when the file can be stored, otherwise the reason.</summary>
    public static string? Check(string sourcePath)
    {
        if (!File.Exists(sourcePath)) return "That file doesn't exist anymore.";
        if (!ScanExtensions.Contains(Path.GetExtension(sourcePath).ToLowerInvariant()))
            return "Only PDF, image (JPG, PNG, BMP, TIF) or Word files can be attached.";
        if (new FileInfo(sourcePath).Length > MaxBytes) return "The file is too big (25 MB at most).";
        return null;
    }

    /// <summary>
    /// Copies a file into storage under <paramref name="folder"/>\yyyy\MM and returns the relative path to save.
    /// A random prefix keeps two files with the same name apart.
    /// </summary>
    public static async Task<string> SaveAsync(string sourcePath, string folder)
    {
        if (Check(sourcePath) is { } problem) throw new InvalidOperationException(problem);
        var safeName = string.Concat(Path.GetFileName(sourcePath).Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
        var relative = Path.Combine(folder, DateTime.Today.ToString("yyyy"), DateTime.Today.ToString("MM"),
                                    $"{Guid.NewGuid():N}"[..8] + "_" + safeName);
        var full = Path.Combine(await RootAsync(), relative);
        Directory.CreateDirectory(Path.GetDirectoryName(full)!);
        File.Copy(sourcePath, full);
        return relative;
    }

    public static async Task<string> FullPathAsync(string relativePath) => Path.Combine(await RootAsync(), relativePath);

    /// <summary>Opens a stored file in its usual program (PDF viewer, Photos, Word).</summary>
    public static async Task<string?> OpenAsync(string relativePath)
    {
        var full = await FullPathAsync(relativePath);
        if (!File.Exists(full)) return $"The file is missing from storage:\n{full}";
        Process.Start(new ProcessStartInfo(full) { UseShellExecute = true });
        return null;
    }

    /// <summary>For tests and when the setting changes.</summary>
    public static void ForgetCachedRoot() => cachedRoot = null;
}
