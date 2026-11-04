using System.Text.Json;

namespace Company.OfficeApp.Data;

/// <summary>
/// Which version of Tri-Stone this copy is (owner decision 2026-11-04), read from edition.json next to TriStone.exe.
/// The Setup files put that file there (installer\build-installer.cmd); without it (Visual Studio) nothing changes:
/// - Development: no file. Database Settings as they are saved on this PC.
/// - Office: for the office PCs. Server address, port, database and login are fixed in the file; only the password is
///   typed, once per PC (never inside Setup.exe). Staff don't see database settings.
/// - Demo: stand-alone. Its own private database on this PC with sample data; nothing reaches the live website.
/// </summary>
public static class AppEdition
{
    /// <summary>The Demo's private database listens here, on this PC only (not the usual 5432).</summary>
    public const int DemoPort = 54329;
    public const string DemoDatabaseName = "construction_db";

    class EditionFile
    {
        public string Edition { get; set; } = "";
        public string Host { get; set; } = "";
        public int Port { get; set; } = 5432;
        public string Database { get; set; } = "construction_db";
        public string Username { get; set; } = "tristone_app";
    }

    static readonly EditionFile? file = Load();

    static EditionFile? Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "edition.json");
        if (!File.Exists(path)) return null;
        try { return JsonSerializer.Deserialize<EditionFile>(File.ReadAllText(path)); }
        catch (JsonException) { return null; }
    }

    public static bool IsDemo => file?.Edition == "Demo";
    public static bool IsOffice => file?.Edition == "Office";

    public static string Host => file?.Host ?? "";
    public static int Port => file?.Port ?? 5432;
    public static string Database => file?.Database ?? "construction_db";
    public static string Username => file?.Username ?? "tristone_app";
}
