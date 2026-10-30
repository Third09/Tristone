using Company.WebsiteAdmin.Pages;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Forms;

/// <summary>
/// The Website Admin app's main window, laid out like the office app: the sidebar on the left (the open page's
/// button in bold), the page title and who is signed in at the top, and the chosen page in the middle.
/// Full screen with no title bar. Accounts is shown to main accounts only.
/// </summary>
public partial class MainForm : Form
{
    /// <summary>True when the person clicked Sign Out (back to Sign In); false when the window was closed.</summary>
    public bool SignedOut { get; private set; }
    bool loggedOut;   // sign-out already written to website_login_logs

    enum WebPage { PageContent, Announcements, WebsiteProjects, DocumentVault, Inquiries, WebsiteTraffic, Accounts }

    readonly Dictionary<WebPage, Button> navButtons;

    public MainForm()
    {
        InitializeComponent();
        // Always full screen, stopping at the taskbar (Windows reads this position relative to the monitor).
        var screen = Screen.FromPoint(Cursor.Position);
        MaximizedBounds = new Rectangle(screen.WorkingArea.X - screen.Bounds.X, screen.WorkingArea.Y - screen.Bounds.Y,
                                        screen.WorkingArea.Width, screen.WorkingArea.Height);
        lblUserName.Text = WebsiteAuth.Current is { } a ? $"Signed in: {a.FullName} ({a.Username})" : "";

        navButtons = new()
        {
            [WebPage.PageContent] = btnNavPageContent,
            [WebPage.Announcements] = btnNavAnnouncements,
            [WebPage.WebsiteProjects] = btnNavWebsiteProjects,
            [WebPage.DocumentVault] = btnNavDocumentVault,
            [WebPage.Inquiries] = btnNavInquiries,
            [WebPage.WebsiteTraffic] = btnNavWebsiteTraffic,
            [WebPage.Accounts] = btnNavAccounts,
        };
        foreach (var (page, button) in navButtons) button.Click += (_, _) => ShowPage(page);
        // Only a main account manages the Website Admin accounts (the service checks again on every action).
        btnNavAccounts.Visible = WebsiteAuth.Current is { IsMain: true };
    }

    async void MainForm_Load(object sender, EventArgs e)
    {
        ShowPage(WebPage.Inquiries);   // the first finished page
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
        if (page == WebPage.Accounts && !btnNavAccounts.Visible) return;

        UserControl view;
        switch (page)
        {
            case WebPage.Inquiries:
                var inquiries = new InquiriesTab();
                inquiries.Changed += async (_, _) => await ShowNewInquiriesAsync();
                view = inquiries;
                break;
            case WebPage.Accounts:
                view = new AccountsTab();
                break;
            default:
                view = new UserControl();
                view.Controls.Add(new Label { AutoSize = true, Location = new Point(12, 12), Text = $"{Title(page)}: coming next." });
                break;
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
