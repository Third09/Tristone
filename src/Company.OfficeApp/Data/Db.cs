using Company.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Data;

/// <summary>Opens a database context using this PC's saved settings. Use one per task: <c>using var db = Db.Open();</c></summary>
public static class Db
{
    public static DbSettings Settings { get; set; } = DbSettings.Load();

    public static AppDbContext Open() => AppDbContext.Create(Settings.ConnectionString);

    /// <summary>
    /// Entity Framework sets itself up on the first query (about 1-2 seconds). Doing one small query in the
    /// background while the Sign In window is open makes the real sign-in feel instant.
    /// </summary>
    public static void WarmUpInBackground() => Task.Run(async () =>
    {
        try
        {
            using var db = Open();
            await db.Users.AsNoTracking().AnyAsync();
        }
        catch { /* only a speed-up; a real problem shows up when signing in */ }
    });
}
