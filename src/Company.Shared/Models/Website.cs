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

/// <summary>The original 3 services (owner decision 2026-11-01). Since 2026-11-03 any name can be typed; see <see cref="Suggestions"/>.</summary>
public static class WebsiteServiceNames
{
    public const string Residential = "Residential Construction";
    public const string Commercial = "Commercial Construction";
    public const string Repairs = "Repairs and Home Extension";
    public static readonly string[] All = [Residential, Commercial, Repairs];

    /// <summary>
    /// Services offered in the name list (owner request 2026-11-03: common construction and specialty services), each with
    /// its usual icon (<see cref="WebsiteServiceIcons"/>). Picking or typing one sets that icon.
    /// </summary>
    public static readonly (string Name, string Icon)[] Suggestions =
    [
        (Residential, "ruler"), (Commercial, "helmet"), (Repairs, "tools"),
        ("Industrial Construction", "building-factory-2"),
        ("Design and Build", "ruler-measure"),
        ("Home Renovation and Remodeling", "home-edit"),
        ("Kitchen and Bathroom Renovation", "bath"),
        ("Home Extension", "home-plus"),
        ("Repairs and Maintenance", "tool"),
        ("Interior Fit-out", "armchair"),
        ("Roofing and Waterproofing", "umbrella"),
        ("Electrical Works", "bolt"),
        ("Plumbing and Sanitary Works", "droplet"),
        ("Painting and Finishing", "paint"),
        ("Tiling and Flooring", "layout-grid"),
        ("Ceiling and Drywall", "layout-board-split"),
        ("Steel Works and Welding", "flame"),
        ("Masonry and Concrete Works", "wall"),
        ("Glass and Aluminum Works", "window"),
        ("Carpentry and Cabinetry", "hammer"),
        ("Site Development and Earthworks", "shovel"),
        ("Demolition", "wrecking-ball"),
        ("Fencing and Perimeter Walls", "fence"),
        ("Landscaping", "plant-2"),
        ("Swimming Pools", "pool"),
        ("Driveways and Paving", "road"),
        ("Project Management", "clipboard-check"),
        ("Cost Estimation and Quotation", "calculator"),
        ("Permits and Plans Assistance", "file-certificate"),
    ];
}

/// <summary>One service icon: its key (website_services.icon), its name in the list, the website picture a card without
/// a picture shows, and its address on the website.</summary>
public record ServiceIcon(string Key, string Label, string Picture, string Url);

/// <summary>
/// The service icons (owner requests 2026-11-03). The first 3 are the website's own; the rest are Tabler Icons (MIT),
/// kept in the website at Updated/images/service-icons. Each also brings the picture a card without one shows: one of
/// the website's 3 service pictures (Residential.jpg, commercial.jpg, repairs.jpg), the closest match.
/// </summary>
public static class WebsiteServiceIcons
{
    public const string Ruler = "ruler", Helmet = "helmet", Tools = "tools";
    const string House = "Residential.jpg", Building = "commercial.jpg", Fixing = "repairs.jpg";

    static ServiceIcon Own(string key, string label, string picture) => new(key, label, picture, $"/Updated/images/icon-{key}.svg");
    static ServiceIcon Tabler(string key, string label, string picture) => new(key, label, picture, $"/Updated/images/service-icons/{key}.svg");

    public static readonly ServiceIcon[] List =
    [
        Own(Ruler, "Ruler and pencil", House), Own(Helmet, "Hard hat and tools", Building), Own(Tools, "Wrench and screwdriver", Fixing),
        Tabler("building-factory-2", "Factory", Building), Tabler("ruler-measure", "Measuring tape", House),
        Tabler("home-edit", "House with pencil", Fixing), Tabler("bath", "Bathtub", Fixing), Tabler("home-plus", "House with plus", House),
        Tabler("tool", "Wrench", Fixing), Tabler("armchair", "Armchair", Building), Tabler("umbrella", "Umbrella", Fixing),
        Tabler("bolt", "Lightning bolt", Fixing), Tabler("droplet", "Water drop", Fixing), Tabler("paint", "Paint roller", Fixing),
        Tabler("layout-grid", "Tiles", Fixing), Tabler("layout-board-split", "Panels", Fixing), Tabler("flame", "Flame", Building),
        Tabler("wall", "Brick wall", House), Tabler("window", "Window", House), Tabler("hammer", "Hammer", Fixing),
        Tabler("shovel", "Shovel", Building), Tabler("wrecking-ball", "Wrecking ball", Building), Tabler("fence", "Fence", House),
        Tabler("plant-2", "Plant", House), Tabler("pool", "Pool", House), Tabler("road", "Road", Building),
        Tabler("clipboard-check", "Checklist", Building), Tabler("calculator", "Calculator", Building),
        Tabler("file-certificate", "Certificate", Building),
    ];

    public static readonly string[] All = [.. List.Select(i => i.Key)];

    public static ServiceIcon Get(string key) => List.FirstOrDefault(i => i.Key == key) ?? List[0];

    /// <summary>The usual icon for a service name (any case); Ruler for a name not in the list.</summary>
    public static string For(string serviceName) =>
        WebsiteServiceNames.Suggestions.FirstOrDefault(s => s.Name.Equals(serviceName.Trim(), StringComparison.OrdinalIgnoreCase)).Icon ?? Ruler;

    /// <summary>Shown in the icon list.</summary>
    public static string Label(string key) => Get(key).Label;
}

/// <summary>The website_content keys the editors use.</summary>
public static class WebsiteKeys
{
    public const string HeroImage = "home.hero_image", Headline = "home.headline", Subheadline = "home.subheadline";
    public const string Address = "contact.address", Phone = "contact.phone", Email = "contact.email", Copyright = "contact.copyright";
    public const string RecipientEmail = "inquiry.recipient_email";
}
