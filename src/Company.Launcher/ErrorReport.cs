using System.Text;
using Company.OfficeApp.Data;
using Company.OfficeApp.Services;

namespace Company.Launcher;

/// <summary>
/// Instead of the red .NET "Unhandled exception" window (owner request 2026-11-20, before outside testing): a plain
/// message, the person keeps working, and the details are written to an error log the developer can read
/// (%LOCALAPPDATA%\TriStone\errors.log: time, version, who was signed in, which window, the full error).
/// </summary>
internal static class ErrorReport
{
    public static string LogPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TriStone", "errors.log");

    public static void Install()
    {
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => Show(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) => { if (e.ExceptionObject is Exception ex) Write(ex, "background"); };
        TaskScheduler.UnobservedTaskException += (_, e) => { Write(e.Exception, "background task"); e.SetObserved(); };
    }

    static DateTime lastShown;

    static void Show(Exception ex)
    {
        Write(ex, "screen");
        if (DateTime.Now - lastShown < TimeSpan.FromSeconds(3)) return;   // the same problem twice in a row: one message
        lastShown = DateTime.Now;
        MessageBox.Show(
            "Something went wrong on this screen, but your saved work is safe.\n\n" +
            "You can keep working. If it happens again, tell the developer what you clicked just before.\n\n" +
            $"The details were saved in:\n{LogPath}\n\n({(ex.InnerException ?? ex).Message})",
            AppEdition.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    static void Write(Exception ex, string where)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
            var who = Session.CurrentUser is { } u ? $"{u.Username} ({u.Level}{(u.Department is { } d ? ", " + d : "")})" : "nobody signed in";
            var window = Form.ActiveForm?.Text ?? "-";
            var sb = new StringBuilder()
                .AppendLine($"===== {DateTime.Now:yyyy-MM-dd HH:mm:ss}  Tri-Stone {Application.ProductVersion}  ({AppEdition.Title}, {where})")
                .AppendLine($"Signed in: {who}   Window: {window}")
                .AppendLine(ex.ToString())
                .AppendLine();
            File.AppendAllText(LogPath, sb.ToString());
        }
        catch { /* never let the error report itself fail */ }
    }
}
