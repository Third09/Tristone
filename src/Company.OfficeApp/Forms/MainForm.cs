using Company.OfficeApp.Pages;
using Company.OfficeApp.Services;

namespace Company.OfficeApp.Forms;

/// <summary>
/// The main window: sidebar on the left (only the pages this account may use), top bar, and the main area
/// (pnlContent) where the chosen page loads. Also runs the 5-minute "Still there?" idle check.
/// Plain layout for now; control names match the UI Elements Guide (Part A: Main Window).
/// </summary>
public partial class MainForm : Form
{
    /// <summary>True when closed by signing out (go back to the Login window) rather than closing the app.</summary>
    public bool SignedOut { get; private set; }

    /// <summary>True when the sign out came from the "Still there?" check.</summary>
    public bool SignedOutIdle { get; private set; }

    readonly Dictionary<AppPage, Button> navButtons;
    bool loggedOut;   // sign-out already written to login_logs

    public MainForm()
    {
        InitializeComponent();

        navButtons = new()
        {
            [AppPage.Dashboard] = btnNavDashboard,
            [AppPage.Projects] = btnNavProjects,
            [AppPage.HrPayroll] = btnNavHr,
            [AppPage.Inventory] = btnNavInventory,
            [AppPage.Subcontractors] = btnNavSubcontractors,
            [AppPage.Receipts] = btnNavReceipts,
            [AppPage.WebsiteAdmin] = btnNavWebsite,
            [AppPage.UserAccounts] = btnNavUsers,
            [AppPage.MyAccount] = btnNavMyAccount,
        };

        var u = Session.CurrentUser!;
        lblUserName.Text = u.FullName;
        lblUserRole.Text = u.IsAdmin ? "Admin" : $"{u.Department} - {u.Level}";
        btnNotifications.Visible = u.IsAdmin;

        foreach (var (page, button) in navButtons)
        {
            button.Visible = Access.CanSee(u, page);
            button.Click += (_, _) => ShowPage(page);
        }
    }

    async void MainForm_Load(object sender, EventArgs e)
    {
        ShowPage(Access.FirstPage(Session.CurrentUser!));
        IdleMonitor.Instance.Start();
        tmrIdle.Start();
        await RefreshNotificationCountAsync();
    }

    /// <summary>Loads a page into the main area. Pages not built yet show a placeholder.</summary>
    void ShowPage(AppPage page)
    {
        if (!Access.CanSee(Session.CurrentUser!, page)) return;

        UserControl view = page switch
        {
            AppPage.MyAccount => new MyAccountPage(),
            AppPage.UserAccounts => new UserAccountsPage(),
            AppPage.Projects => new ProjectsPage(),
            _ => new PlaceholderPage(Access.Title(page)),   // real pages replace this as they're built
        };

        foreach (Control old in pnlContent.Controls) old.Dispose();
        pnlContent.Controls.Clear();
        view.Dock = DockStyle.Fill;
        pnlContent.Controls.Add(view);

        lblPageTitle.Text = Access.Title(page);
        foreach (var (p, b) in navButtons)
            b.Font = p == page ? navFontOpen : Font;   // bold = the page that's open
    }

    Font? openFont;
    Font navFontOpen => openFont ??= new Font(Font, FontStyle.Bold);

    /// <summary>Admin only: number of waiting Forgot Password requests on the notification button.</summary>
    async Task RefreshNotificationCountAsync()
    {
        if (!btnNotifications.Visible) return;
        var count = await PasswordService.CountWaitingRequestsAsync();
        btnNotifications.Text = $"Password requests ({count})";
    }

    async void btnNotifications_Click(object sender, EventArgs e)
    {
        using (var f = new NotificationsForm()) f.ShowDialog(this);
        await RefreshNotificationCountAsync();
    }

    // Checked every second; the popup opens after IdleMonitor.Timeout (5 minutes) without activity.
    async void tmrIdle_Tick(object sender, EventArgs e)
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

    async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        IdleMonitor.Instance.Stop();
        if (loggedOut) return;

        // Closed with X: write the sign-out first, then close for real
        // (otherwise the app could exit before the log row is saved).
        e.Cancel = true;
        tmrIdle.Stop();
        await AuthService.SignOutAsync();
        loggedOut = true;
        Close();
    }
}
