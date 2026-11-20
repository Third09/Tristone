using System.Text.Json;

namespace Company.OfficeApp.Data;

/// <summary>
/// Which version of Tri-Stone this copy is (owner decision 2026-11-04), read from edition.json next to TriStone.exe.
/// The Setup files put that file there (installer\build-installer.cmd); without it (Visual Studio) nothing changes:
/// - Development: no file. Database Settings as they are saved on this PC.
/// - Office: for the office PCs. Server address, port, database and login are fixed in the file; only the password is
///   typed, once per PC (never inside Setup.exe). Staff don't see database settings.
/// - Demo: stand-alone. Its own private database on this PC with sample data; nothing reaches the live website.
/// - Standalone (owner request 2026-11-18): the same private database, but a copy of the home PC's real data with
///   its own logins and passwords; called just "Tri-Stone", no "Test as...", no reset. Nothing reaches the live website.
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

    /// <summary>Runs its own private database on this PC and never touches the live website (Demo and Standalone).</summary>
    public static bool IsDemo => file?.Edition is "Demo" or "Standalone";
    /// <summary>The real-style stand-alone copy: real logins, called "Tri-Stone".</summary>
    public static bool IsStandalone => file?.Edition == "Standalone";
    /// <summary>"Test as..." and the sample-data reset exist only in the Demo / Test version.</summary>
    public static bool IsSampleDemo => file?.Edition == "Demo";
    /// <summary>The program's name on its windows.</summary>
    public static string Title => IsSampleDemo ? "Tri-Stone Demo" : "Tri-Stone";
    /// <summary>Its folder under %LOCALAPPDATA% (separate, so a Demo and a Standalone copy can sit on one PC).</summary>
    public static string LocalFolderName => IsStandalone ? "TriStone Local" : "TriStone Demo";
    public static bool IsOffice => file?.Edition == "Office";

    public static string Host => file?.Host ?? "";
    public static int Port => file?.Port ?? 5432;
    public static string Database => file?.Database ?? "construction_db";
    public static string Username => file?.Username ?? "tristone_app";
}
