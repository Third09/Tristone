using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Forms;

/// <summary>
/// The Website Admin app's main window: who is signed in, Sign Out, and the six website tabs
/// (Page Content, Announcements, Website Projects, Document Vault, Inquiries, Website Traffic), filled in one by one.
/// Full screen with no title bar, like the office app.
/// </summary>
public partial class MainForm : Form
{
    /// <summary>True when the person clicked Sign Out (back to Sign In); false when the window was closed.</summary>
    public bool SignedOut { get; private set; }
    bool loggedOut;   // sign-out already written to website_login_logs

    public MainForm()
    {
        InitializeComponent();
        // Always full screen, stopping at the taskbar (Windows reads this position relative to the monitor).
        var screen = Screen.FromPoint(Cursor.Position);
        MaximizedBounds = new Rectangle(screen.WorkingArea.X - screen.Bounds.X, screen.WorkingArea.Y - screen.Bounds.Y,
                                        screen.WorkingArea.Width, screen.WorkingArea.Height);
        lblUserName.Text = WebsiteAuth.Current is { } a ? $"Signed in: {a.FullName} ({a.Username})" : "";
    }

    async void btnSignOut_Click(object sender, EventArgs e)
    {
        SignedOut = true;
        await WebsiteAuth.SignOutAsync();
        loggedOut = true;
        Close();
    }

    // Alt+F4: write the sign-out before the window closes, or the app could exit before the row is saved.
    async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (loggedOut) return;
        e.Cancel = true;
        loggedOut = true;
        await WebsiteAuth.SignOutAsync();
        Close();
    }
}
