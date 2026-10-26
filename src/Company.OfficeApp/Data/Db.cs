using Company.Shared.Data;

namespace Company.OfficeApp.Data;

/// <summary>Opens a database context using this PC's saved settings. Use one per task: <c>using var db = Db.Open();</c></summary>
public static class Db
{
    public static DbSettings Settings { get; set; } = DbSettings.Load();

    public static AppDbContext Open() => AppDbContext.Create(Settings.ConnectionString);
}
