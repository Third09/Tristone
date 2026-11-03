using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>One editable text or image of the website, e.g. 'home.headline' (database/014_website_content.sql).</summary>
[Table("website_content")]
public class WebsiteContent
{
    [Column("content_key")] public string ContentKey { get; set; } = "";
    /// <summary>The text, or for an image its path relative to the shared files folder.</summary>
    [Column("value")] public string? Value { get; set; }
    [Column("updated_by")] public int? UpdatedBy { get; set; }
    [Column("updated_at")] public DateTime? UpdatedAt { get; set; }
}

/// <summary>A featured project on the website: an office project (projects.show_on_website) plus website-only details.</summary>
[Table("website_projects")]
public class WebsiteProject
{
    [Column("web_project_id")] public int WebProjectId { get; set; }
    [Column("project_id")] public int ProjectId { get; set; }
    [Column("sort_order")] public int SortOrder { get; set; }
    /// <summary>False = unpublished (hidden from the website, kept here).</summary>
    [Column("is_visible")] public bool IsVisible { get; set; } = true;
    [Column("short_description")] public string? ShortDescription { get; set; }
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime? UpdatedAt { get; set; }
}

/// <summary>A photo of a featured project. Showcase photos are the ones in the website's carousel.</summary>
[Table("website_project_photos")]
public class WebsiteProjectPhoto
{
    [Column("photo_id")] public int PhotoId { get; set; }
    [Column("web_project_id")] public int WebProjectId { get; set; }
    [Column("file_path")] public string FilePath { get; set; } = "";
    [Column("file_name")] public string FileName { get; set; } = "";
    [Column("size_bytes")] public long SizeBytes { get; set; }
    [Column("sort_order")] public int SortOrder { get; set; }
    [Column("is_showcase")] public bool IsShowcase { get; set; } = true;
    [Column("uploaded_by")] public int? UploadedBy { get; set; }
    [Column("uploaded_at")] public DateTime UploadedAt { get; set; } = DateTime.Now;
}

/// <summary>A service card on the website (one of <see cref="WebsiteServiceNames"/>).</summary>
[Table("website_services")]
public class WebsiteService
{
    [Column("web_service_id")] public int WebServiceId { get; set; }
    [Column("service_name")] public string ServiceName { get; set; } = "";
    /// <summary>One of <see cref="ProjectSectors"/>.</summary>
    [Column("sector")] public string Sector { get; set; } = ProjectSectors.Residential;
    [Column("location")] public string? Location { get; set; }
    [Column("short_description")] public string? ShortDescription { get; set; }
    [Column("image_path")] public string? ImagePath { get; set; }
    /// <summary>One of <see cref="WebsiteServiceIcons"/> (script 019): the card's icon, and the website's own picture when ImagePath is empty.</summary>
    [Column("icon")] public string Icon { get; set; } = WebsiteServiceIcons.Ruler;
    [Column("sort_order")] public int SortOrder { get; set; }
    [Column("is_visible")] public bool IsVisible { get; set; } = true;
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// One About Us section (database/015_about_sections.sql): a row of the website's About Us with a picture, a heading
/// and its text, and optionally a second heading and text (like OUR VISION / OUR MISSION). Any number, in sort_order.
/// </summary>
[Table("website_about_sections")]
public class WebsiteAboutSection
{
    [Column("about_section_id")] public int AboutSectionId { get; set; }
    [Column("sort_order")] public int SortOrder { get; set; }
    [Column("heading")] public string Heading { get; set; } = "";
    [Column("body")] public string? Body { get; set; }
    [Column("heading2")] public string? Heading2 { get; set; }
    [Column("body2")] public string? Body2 { get; set; }
    /// <summary>Path relative to the shared files folder; null = the website keeps its own picture.</summary>
    [Column("image_path")] public string? ImagePath { get; set; }
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_by")] public int? UpdatedBy { get; set; }
    [Column("updated_at")] public DateTime? UpdatedAt { get; set; }
}

/// <summary>One Publish Website: a JSON snapshot of all saved content at that moment.</summary>
[Table("website_publishes")]
public class WebsitePublish
{
    [Column("publish_id")] public int PublishId { get; set; }
    [Column("published_at")] public DateTime PublishedAt { get; set; } = DateTime.Now;
    [Column("published_by")] public int? PublishedBy { get; set; }
    [Column("content", TypeName = "jsonb")] public string Content { get; set; } = "{}";
}

/// <summary>One page visit reported by the website (the free Website Traffic counter).</summary>
[Table("website_visits")]
public class WebsiteVisit
{
    [Column("visit_id")] public long VisitId { get; set; }
    [Column("visited_at")] public DateTime VisitedAt { get; set; } = DateTime.Now;
    [Column("page")] public string? Page { get; set; }
}

/// <summary>The fixed service list (owner decision 2026-11-01); the database CHECK has the same three.</summary>
public static class WebsiteServiceNames
{
    public const string Residential = "Residential Construction";
    public const string Commercial = "Commercial Construction";
    public const string Repairs = "Repairs and Home Extension";
    public static readonly string[] All = [Residential, Commercial, Repairs];
}

/// <summary>
/// The website's 3 service icons (owner request 2026-11-03). Each one also brings the picture the website shows when a
/// card has none: Ruler = the Residential Construction picture, Helmet = Commercial, Tools = Repairs.
/// </summary>
public static class WebsiteServiceIcons
{
    public const string Ruler = "ruler", Helmet = "helmet", Tools = "tools";
    public static readonly string[] All = [Ruler, Helmet, Tools];

    /// <summary>The icon an original service uses (typed names get Ruler until one is picked).</summary>
    public static string For(string serviceName) => serviceName switch
    {
        WebsiteServiceNames.Commercial => Helmet,
        WebsiteServiceNames.Repairs => Tools,
        _ => Ruler,
    };

    /// <summary>Shown in the icon list.</summary>
    public static string Label(string icon) => icon switch
    {
        Helmet => "Helmet (like Commercial Construction)",
        Tools => "Tools (like Repairs and Home Extension)",
        _ => "Ruler (like Residential Construction)",
    };
}

/// <summary>The website_content keys the editors use.</summary>
public static class WebsiteKeys
{
    public const string HeroImage = "home.hero_image", Headline = "home.headline", Subheadline = "home.subheadline";
    public const string Address = "contact.address", Phone = "contact.phone", Email = "contact.email", Copyright = "contact.copyright";
    public const string RecipientEmail = "inquiry.recipient_email";
}
