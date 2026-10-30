using Company.OfficeApp.Data;
using Company.OfficeApp.Forms;
using Company.OfficeApp.Services;

namespace Company.Launcher;

internal static class Program
{
    /// <summary>
    /// Tri-Stone start-up: make sure the database can be reached, then one Sign In window for everyone.
    /// The login decides which app opens: Website Admin -> Website Admin app; Admin -> asks (office or website);
    /// everyone else -> office app. After Sign Out it goes back to Sign In, until the person clicks Exit.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        if (!EnsureDatabase()) return;
        Db.WarmUpInBackground();

        string? notice = null;
        while (true)
        {
            using (var login = new LoginForm(notice))
            {
                if (login.ShowDialog() != DialogResult.OK) return;   // Exit on the Sign In window
            }
            notice = null;

            var user = Session.CurrentUser!;
            var app = user.IsWebsiteAdmin ? ChosenApp.Website
                    : user.IsAdmin ? AppChoiceForm.Ask()
                    : ChosenApp.Office;

            if (app == ChosenApp.None)   // an Admin chose Sign Out instead of an app
            {
                AuthService.SignOutAsync().GetAwaiter().GetResult();
                continue;
            }

            bool signedOut, idle;
            if (app == ChosenApp.Website)
            {
                using var main = new WebsiteAdmin.Forms.MainForm();
                Application.Run(main);
                (signedOut, idle) = (main.SignedOut, main.SignedOutIdle);
            }
            else
            {
                using var main = new MainForm();
                Application.Run(main);
                (signedOut, idle) = (main.SignedOut, main.SignedOutIdle);
            }
            if (!signedOut) return;   // window closed (Alt+F4)
            if (idle) notice = "You were signed out after 5 minutes without activity.";
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
