using Company.OfficeApp.Data;
using Company.OfficeApp.Forms;
using Company.WebsiteAdmin.Forms;

namespace Company.WebsiteAdmin;

internal static class Program
{
    /// <summary>
    /// Start-up, like the office app: make sure the database can be reached, then Sign In -> Main window,
    /// back to Sign In after Sign Out, until the user exits.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        if (!EnsureDatabase()) return;
        Db.WarmUpInBackground();

        while (true)
        {
            using (var login = new LoginForm())
            {
                if (login.ShowDialog() != DialogResult.OK) return;   // Exit on the Sign In window
            }

            using var main = new MainForm();
            Application.Run(main);
            if (!main.SignedOut) return;
        }
    }

    /// <summary>If the database can't be reached (or this PC was never set up), show Database Settings.</summary>
    static bool EnsureDatabase()
    {
        string? problem = Db.Settings.Exists
            ? Db.Settings.TestConnectionAsync().GetAwaiter().GetResult()
            : "First time on this computer: enter where the database is.";

        if (problem == null) return true;

        using var settings = new DatabaseSettingsForm(problem);
        return settings.ShowDialog() == DialogResult.OK;
    }
}
