using Company.OfficeApp.Data;
using Company.OfficeApp.UI;
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
        Theme.StyleMainWindow(picLogo,
        [
            (btnNavHome, ""), (btnNavProjects, ""), (btnNavServices, ""), (btnNavAboutUs, ""),
            (btnNavContactUs, ""), (btnSignOut, ""),
        ], lblUserName, lblLastPublished);
        if (AppEdition.IsDemo)   // nothing reaches the live website from the Demo
            btnPublishWebsite.Visible = btnUndoPublish.Visible = btnWebsiteSettings.Visible = false;
    }

    async void MainForm_Load(object sender, EventArgs e)
    {
        ShowPage(WebPage.Home);
        IdleMonitor.Instance.Start();
        tmrIdle.Start();
        try { await InquiryImport.PullAsync(); } catch { /* only for the count; Contact Us reports problems */ }
        await ShowNewInquiriesAsync();
        await ShowLastPublishedAsync();
        tmrWebsiteCheck.Start();
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
        {
            b.Font = p == page ? NavFontOpen : Font;   // bold = the page that's open
            Theme.SetActiveNav(b, p == page);   // and orange (UI/Theme)
        }
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
        if (AppEdition.IsDemo) { lblLastPublished.Text = "Demo version: publishing is off"; return; }
        try
        {
            lblLastPublished.Text = await PublishService.LastAsync() is { } p
                ? $"Last published: {p.At:MMM d, yyyy h:mm tt} by {p.By}" : "Not published from the app yet";
        }
        catch { /* only a hint */ }
        await ShowWaitingAsync();
    }

    /// <summary>The Publish button says how many saved edits aren't live yet, e.g. "Publish Website (3)".</summary>
    async Task ShowWaitingAsync()
    {
        try
        {
            var n = await PublishService.WaitingCountAsync();
            btnPublishWebsite.Text = n == 0 ? "Publish Website" : $"Publish Website ({n})";
        }
        catch { /* only a hint */ }
    }

    int checks;
    bool checking;

    // Every 15 s: the Publish count; every 3 minutes: new inquiries from the website, with a Windows notification.
    async void tmrWebsiteCheck_Tick(object? sender, EventArgs e)
    {
        if (checking) return;
        checking = true;
        try
        {
            await ShowWaitingAsync();
            if (++checks % 12 != 0) return;
            var pulled = await InquiryImport.PullAsync();
            if (pulled.Added == 0) return;
            await ShowNewInquiriesAsync();
            ntfInquiries.Icon = Icon ?? SystemIcons.Information;
            ntfInquiries.Visible = true;
            ntfInquiries.ShowBalloonTip(10000, "New website inquiry",
                pulled.Added == 1 ? "1 new inquiry. Click to open Contact Us." : $"{pulled.Added} new inquiries. Click to open Contact Us.", ToolTipIcon.Info);
        }
        catch { /* tried again on the next check */ }
        finally { checking = false; }
    }

    void ntfInquiries_BalloonTipClicked(object? sender, EventArgs e)
    {
        Activate();
        if (current == WebPage.ContactUs) { (pnlContent.Controls[0] as ContactUsPage)?.RefreshInquiries(); return; }
        ShowPage(WebPage.ContactUs);
    }

    /// <summary>Ctrl+S: clicks the Save button nearest to where you are typing (on Contact Us: the box you are in).</summary>
    void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (!(e.Control && e.KeyCode == Keys.S)) return;
        e.SuppressKeyPress = true;
        static IEnumerable<Button> Saves(Control c) =>
            c.Controls.Cast<Control>().SelectMany(x => x is Button b ? [b] : Saves(x))
             .Where(b => b.Name.StartsWith("btnSave") && b.Visible && b.Enabled);
        Control? at = ActiveControl;
        while (at is ContainerControl { ActiveControl: { } inner }) at = inner;
        for (var c = at; c != null && c != this; c = c.Parent)
            if (Saves(c).FirstOrDefault() is { } save) { save.PerformClick(); return; }
        Saves(pnlContent).FirstOrDefault()?.PerformClick();
    }

    /// <summary>Undo Publish: the version before the live one goes back on the website; saved edits stay as they are.</summary>
    async void btnUndoPublish_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(this, "Put the previous version of the website back on the live site?\n\nYour saved edits in Website Admin stay as they are: fix them and click Publish Website again when ready.",
                "Undo Publish", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        btnUndoPublish.Enabled = false;
        UseWaitCursor = true;
        try
        {
            var error = await PublishService.UndoAsync();
            if (error != null) { MessageBox.Show(this, error, "Undo Publish"); return; }
            await ShowLastPublishedAsync();
            MessageBox.Show(this, "Done. The live website shows the previous version in about a minute.", "Undo Publish",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { MessageBox.Show(this, "Couldn't undo. Please try again.\n" + ex.Message, "Undo Publish"); }
        finally { btnUndoPublish.Enabled = true; UseWaitCursor = false; }
    }

    /// <summary>Publish Website: everything saved on every page goes live (unsaved edits on the open page are not included).</summary>
    async void btnPublishWebsite_Click(object sender, EventArgs e)
    {
        string unsaved = OpenEditor is { HasUnsavedChanges: true }
            ? "\n\nThis page has unsaved changes. They are NOT included: click Save Changes first if you want them." : "";
        if (MessageBox.Show(this, "Put every saved change (Home, Projects, Services, About Us and the company details) on the live website?" + unsaved,
                "Publish Website", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        // First publish on this PC: ask where to send it.
        if (!PublishSettings.Load().IsSet && !ShowWebsiteSettings()) return;
        btnPublishWebsite.Enabled = false;
        UseWaitCursor = true;
        try
        {
            var error = await PublishService.PublishAsync();
            if (error != null) { MessageBox.Show(this, error, "Publish Website"); return; }
            await ShowLastPublishedAsync();
            MessageBox.Show(this, "Published. The live website shows these changes in about a minute.",
                            "Publish Website", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { MessageBox.Show(this, "Couldn't publish. Please try again.\n" + ex.Message, "Publish Website"); }
        finally { btnPublishWebsite.Enabled = true; UseWaitCursor = false; }
    }

    void btnWebsiteSettings_Click(object sender, EventArgs e) => ShowWebsiteSettings();

    /// <summary>Preview Website: the saved changes on a copy of the website; unsaved edits on the open page are not in it.</summary>
    void btnPreviewWebsite_Click(object sender, EventArgs e)
    {
        if (OpenEditor is { HasUnsavedChanges: true } &&
            MessageBox.Show(this, "This page has unsaved changes. They are NOT in the preview: click Save Changes first if you want them.\n\nPreview anyway?",
                "Preview Website", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        using var preview = new PreviewForm();
        preview.ShowDialog(this);
    }

    /// <summary>Opens Website Settings; true when they were saved.</summary>
    bool ShowWebsiteSettings()
    {
        using var form = new PublishSettingsForm();
        return form.ShowDialog(this) == DialogResult.OK;
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
        tmrWebsiteCheck.Stop();
        ntfInquiries.Visible = false;
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
