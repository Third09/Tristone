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
    [Column("sort_order")] public int SortOrder { get; set; }
    [Column("is_visible")] public bool IsVisible { get; set; } = true;
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
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

/// <summary>The website_content keys the editors use.</summary>
public static class WebsiteKeys
{
    public const string HeroImage = "home.hero_image", Headline = "home.headline", Subheadline = "home.subheadline";
    public static string AboutImage(int section) => $"about.{section}.image";
    public static string AboutHeading(int section) => $"about.{section}.heading";
    public const string About1Text = "about.1.text", About2Text = "about.2.text", Vision = "about.3.vision", Mission = "about.3.mission";
    public const string Address = "contact.address", Phone = "contact.phone", Email = "contact.email", Copyright = "contact.copyright";
    public const string RecipientEmail = "inquiry.recipient_email";
}
