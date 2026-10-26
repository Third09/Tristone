using Company.OfficeApp.Data;
using Company.OfficeApp.Forms;

namespace Company.OfficeApp
{
    internal static class Program
    {
        /// <summary>
        ///  Start-up: make sure the database can be reached, then Login -> Main window,
        ///  back to Login after Sign Out, until the user exits.
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
                    if (login.ShowDialog() != DialogResult.OK) return;   // Exit on the Login window
                }

                using var main = new MainForm();
                Application.Run(main);
                if (!main.SignedOut) return;                             // window closed with X

                notice = main.SignedOutIdle
                    ? "You were signed out after 5 minutes without activity."
                    : null;
            }
        }

        /// <summary>
        /// Flowchart step "Connect to database": if it fails (or this PC has never been set up),
        /// show Database Settings until it works or the user cancels.
        /// </summary>
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
}
