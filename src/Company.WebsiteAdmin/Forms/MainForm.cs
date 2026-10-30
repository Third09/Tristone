using Company.OfficeApp.Forms;
using Company.OfficeApp.Services;
using Company.WebsiteAdmin.Pages;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Forms;

/// <summary>
/// The Website Admin app's main window, laid out like the office app: the sidebar on the left (the open page's
/// button in bold), the page title and who is signed in at the top, and the chosen page in the middle.
/// Full screen with no title bar. Opened by the Tri-Stone launcher for Website Admin logins and Admins who choose it;
/// accounts are managed in the office app's User Accounts (one account list, script 011).
/// Same "Still there?" idle check as the office app.
/// </summary>
public partial class MainForm : Form
{
    /// <summary>True when the person signed out (back to Sign In); false when the window was closed.</summary>
    public bool SignedOut { get; private set; }
    /// <summary>True when the sign-out was the automatic one after 5 minutes without activity.</summary>
    public bool SignedOutIdle { get; private set; }
    bool loggedOut;   // sign-out already written to login_logs

    enum WebPage { PageContent, Announcements, WebsiteProjects, DocumentVault, Inquiries, WebsiteTraffic }

    readonly Dictionary<WebPage, Button> navButtons;

    public MainForm()
    {
        InitializeComponent();
        // Always full screen, stopping at the taskbar (Windows reads this position relative to the monitor).
        var screen = Screen.FromPoint(Cursor.Position);
        MaximizedBounds = new Rectangle(screen.WorkingArea.X - screen.Bounds.X, screen.WorkingArea.Y - screen.Bounds.Y,
                                        screen.WorkingArea.Width, screen.WorkingArea.Height);
        lblUserName.Text = Session.CurrentUser is { } u ? $"Signed in: {u.FullName} ({u.Username}) - {u.Level}" : "";

        navButtons = new()
        {
            [WebPage.PageContent] = btnNavPageContent,
            [WebPage.Announcements] = btnNavAnnouncements,
            [WebPage.WebsiteProjects] = btnNavWebsiteProjects,
            [WebPage.DocumentVault] = btnNavDocumentVault,
            [WebPage.Inquiries] = btnNavInquiries,
            [WebPage.WebsiteTraffic] = btnNavWebsiteTraffic,
        };
        foreach (var (page, button) in navButtons) button.Click += (_, _) => ShowPage(page);
    }

    async void MainForm_Load(object sender, EventArgs e)
    {
        ShowPage(WebPage.Inquiries);   // the first finished page
        IdleMonitor.Instance.Start();
        tmrIdle.Start();
        await ShowNewInquiriesAsync();
    }

    static string Title(WebPage page) => page switch
    {
        WebPage.PageContent => "Page Content",
        WebPage.WebsiteProjects => "Website Projects",
        WebPage.DocumentVault => "Document Vault",
        WebPage.WebsiteTraffic => "Website Traffic",
        _ => page.ToString(),
    };

    /// <summary>Loads a page into the main area. Pages not built yet say so.</summary>
    void ShowPage(WebPage page)
    {
        UserControl view;
        if (page == WebPage.Inquiries)
        {
            var inquiries = new InquiriesTab();
            inquiries.Changed += async (_, _) => await ShowNewInquiriesAsync();
            view = inquiries;
        }
        else
        {
            view = new UserControl();
            view.Controls.Add(new Label { AutoSize = true, Location = new Point(12, 12), Text = $"{Title(page)}: coming next." });
        }

        foreach (Control old in pnlContent.Controls) old.Dispose();
        pnlContent.Controls.Clear();
        view.Dock = DockStyle.Fill;
        pnlContent.Controls.Add(view);

        lblPageTitle.Text = Title(page);
        foreach (var (p, b) in navButtons)
            b.Font = p == page ? NavFontOpen : Font;   // bold = the page that's open
    }

    Font? openFont;
    Font NavFontOpen => openFont ??= new Font(Font, FontStyle.Bold);

    /// <summary>The Inquiries button says how many are waiting, e.g. "Inquiries (3 new)".</summary>
    async Task ShowNewInquiriesAsync()
    {
        try
        {
            var counts = await WebsiteInquiries.CountsAsync();
            btnNavInquiries.Text = counts.New == 0 ? "Inquiries" : $"Inquiries ({counts.New} new)";
        }
        catch { /* only a hint; the page itself reports database problems */ }
    }

    // Checked every second; the "Still there?" popup opens after IdleMonitor.Timeout (5 minutes) without activity.
    async void tmrIdle_Tick(object? sender, EventArgs e)
    {
        if (IdleMonitor.Instance.IdleFor < IdleMonitor.Timeout) return;

        tmrIdle.Stop();
        DialogResult answer;
        using (var popup = new IdleWarningForm()) answer = popup.ShowDialog(this);

        if (answer == DialogResult.OK)
        {
            IdleMonitor.Instance.Reset();
            tmrIdle.Start();
            return;
        }
        await SignOutAsync(idle: answer == DialogResult.Abort);   // Abort = nobody answered
    }

    async void btnSignOut_Click(object sender, EventArgs e) => await SignOutAsync(idle: false);

    async Task SignOutAsync(bool idle)
    {
        SignedOut = true;
        SignedOutIdle = idle;
        tmrIdle.Stop();
        await AuthService.SignOutAsync(idle);
        loggedOut = true;
        Close();
    }

    // Alt+F4: write the sign-out before the window closes, or the app could exit before the row is saved.
    async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        IdleMonitor.Instance.Stop();
        if (loggedOut) return;
        e.Cancel = true;
        tmrIdle.Stop();
        await AuthService.SignOutAsync();
        loggedOut = true;
        Close();
    }
}
