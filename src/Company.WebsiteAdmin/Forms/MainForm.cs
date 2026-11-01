using Company.OfficeApp.Forms;
using Company.OfficeApp.Services;
using Company.WebsiteAdmin.Pages;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Forms;

/// <summary>
/// The Website Admin app's main window, laid out like the office app: the sidebar on the left (Home, Projects,
/// Services, About Us, Contact Us, Log out, following the Figma design; the open page's button in bold), the page
/// title, who is signed in and Publish Website at the top, and the chosen page in the middle.
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

    enum WebPage { Home, Projects, Services, AboutUs, ContactUs }

    readonly Dictionary<WebPage, Button> navButtons;
    WebPage? current;

    public MainForm()
    {
        InitializeComponent();
        // Always full screen, stopping at the taskbar (Windows reads this position relative to the monitor).
        var screen = Screen.FromPoint(Cursor.Position);
        MaximizedBounds = new Rectangle(screen.WorkingArea.X - screen.Bounds.X, screen.WorkingArea.Y - screen.Bounds.Y,
                                        screen.WorkingArea.Width, screen.WorkingArea.Height);
        lblUserName.Text = Session.CurrentUser is { } u ? $"Signed in: {u.FullName} ({u.Username}) - {u.Level}" : "";

        // The sidebar follows the Figma design (owner decisions 2026-11-01).
        navButtons = new()
        {
            [WebPage.Home] = btnNavHome,
            [WebPage.Projects] = btnNavProjects,
            [WebPage.Services] = btnNavServices,
            [WebPage.AboutUs] = btnNavAboutUs,
            [WebPage.ContactUs] = btnNavContactUs,
        };
        foreach (var (page, button) in navButtons) button.Click += (_, _) => ShowPage(page);
    }

    async void MainForm_Load(object sender, EventArgs e)
    {
        ShowPage(WebPage.Home);
        IdleMonitor.Instance.Start();
        tmrIdle.Start();
        await ShowNewInquiriesAsync();
        await ShowLastPublishedAsync();
    }

    static string Title(WebPage page) => page switch
    {
        WebPage.Home => "Homepage Editor",
        WebPage.Projects => "Projects Page Editor",
        WebPage.Services => "Services Page Editor",
        WebPage.AboutUs => "About Us Page Editor",
        _ => "Contact Us",
    };

    IWebsiteEditor? OpenEditor => pnlContent.Controls.Count > 0 ? pnlContent.Controls[0] as IWebsiteEditor : null;

    /// <summary>Asks before leaving a page with unsaved changes. True = go ahead.</summary>
    bool OkToLeave() =>
        OpenEditor is not { HasUnsavedChanges: true } ||
        MessageBox.Show(this, "This page has unsaved changes. Leave it without saving?", "Unsaved changes",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

    /// <summary>Loads a page into the main area.</summary>
    void ShowPage(WebPage page)
    {
        if (page == current || !OkToLeave()) return;
        UserControl view = page switch
        {
            WebPage.Home => new HomeEditorPage(),
            WebPage.Projects => new ProjectsEditorPage(),
            WebPage.Services => new ServicesEditorPage(),
            WebPage.AboutUs => new AboutEditorPage(),
            _ => ContactUs(),
        };

        foreach (Control old in pnlContent.Controls) old.Dispose();
        pnlContent.Controls.Clear();
        view.Dock = DockStyle.Fill;
        pnlContent.Controls.Add(view);
        current = page;

        lblPageTitle.Text = Title(page);
        foreach (var (p, b) in navButtons)
            b.Font = p == page ? NavFontOpen : Font;   // bold = the page that's open
    }

    ContactUsPage ContactUs()
    {
        var page = new ContactUsPage();
        page.Changed += async (_, _) => await ShowNewInquiriesAsync();
        return page;
    }

    Font? openFont;
    Font NavFontOpen => openFont ??= new Font(Font, FontStyle.Bold);

    /// <summary>The Contact Us button says how many inquiries are waiting, e.g. "Contact Us (3 new)".</summary>
    async Task ShowNewInquiriesAsync()
    {
        try
        {
            var counts = await WebsiteInquiries.CountsAsync();
            btnNavContactUs.Text = counts.New == 0 ? "Contact Us" : $"Contact Us ({counts.New} new)";
        }
        catch { /* only a hint; the page itself reports database problems */ }
    }

    async Task ShowLastPublishedAsync()
    {
        try
        {
            lblLastPublished.Text = await PublishService.LastAsync() is { } p
                ? $"Last published: {p.At:MMM d, yyyy h:mm tt} by {p.By}" : "Not published from the app yet";
        }
        catch { /* only a hint */ }
    }

    /// <summary>Publish Website: everything saved on every page goes live (unsaved edits on the open page are not included).</summary>
    async void btnPublishWebsite_Click(object sender, EventArgs e)
    {
        string unsaved = OpenEditor is { HasUnsavedChanges: true }
            ? "\n\nThis page has unsaved changes. They are NOT included: click Save Changes first if you want them." : "";
        if (MessageBox.Show(this, "Put every saved change (Home, Projects, Services, About Us and the company details) on the live website?" + unsaved,
                "Publish Website", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        btnPublishWebsite.Enabled = false;
        try
        {
            var error = await PublishService.PublishAsync();
            if (error != null) { MessageBox.Show(this, error, "Publish Website"); return; }
            await ShowLastPublishedAsync();
            MessageBox.Show(this, "Published. The live website shows these changes once the website is connected (a minute or two).",
                            "Publish Website", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { MessageBox.Show(this, "Couldn't publish. Please try again.\n" + ex.Message, "Publish Website"); }
        finally { btnPublishWebsite.Enabled = true; }
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

    async void btnSignOut_Click(object sender, EventArgs e)
    {
        if (OkToLeave()) await SignOutAsync(idle: false);
    }

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
