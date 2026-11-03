using Company.Shared.Models;

namespace Company.WebsiteAdmin.Services;

/// <summary>
/// Pictures for the Services editor: the website's 3 service icons (Resources\icon-*.png, drawn from the website's
/// SVG icons) and the website's own card pictures, which a card without a picture shows (read from the Preview Website
/// copy in C:\ProgramData\TriStone\WebsitePreview when it exists; otherwise the Live Preview stays gray).
/// </summary>
public static class ServiceIconImages
{
    static readonly Dictionary<string, Image> icons = [];
    static readonly Dictionary<string, Image?> pictures = [];

    public static Image Get(string icon)
    {
        if (icons.TryGetValue(icon, out var image)) return image;
        var asm = typeof(ServiceIconImages).Assembly;
        var name = asm.GetManifestResourceNames().Single(n => n.EndsWith($".icon-{icon}.png"));
        using var stream = asm.GetManifestResourceStream(name)!;
        return icons[icon] = new Bitmap(Image.FromStream(stream));
    }

    /// <summary>The website's own picture for this icon's look, or null when no website copy is on this PC yet.</summary>
    public static Image? DefaultPicture(string icon)
    {
        if (pictures.TryGetValue(icon, out var image)) return image;
        var file = icon switch
        {
            WebsiteServiceIcons.Helmet => "commercial.jpg",
            WebsiteServiceIcons.Tools => "repairs.jpg",
            _ => "Residential.jpg",
        };
        var full = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                "TriStone", "WebsitePreview", "Updated", "images", file);
        try { image = File.Exists(full) ? new Bitmap(new MemoryStream(File.ReadAllBytes(full))) : null; }
        catch { image = null; }
        if (image != null) pictures[icon] = image;   // missing: look again next time (after a preview)
        return image;
    }
}
