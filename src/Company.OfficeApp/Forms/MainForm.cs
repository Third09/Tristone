using Company.OfficeApp.UI;
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
        // Always full screen with no title bar, so it can't be resized, moved or minimized by accident.
        // Maximizing stops at the taskbar so the clock and other apps stay reachable.
        // (Windows reads this position relative to the monitor, not the whole desktop.)
        var screen = Screen.FromPoint(Cursor.Position);
        MaximizedBounds = new Rectangle(screen.WorkingArea.X - screen.Bounds.X, screen.WorkingArea.Y - screen.Bounds.Y,
                                        screen.WorkingArea.Width, screen.WorkingArea.Height);

        navButtons = new()
        {
            [AppPage.Dashboard] = btnNavDashboard,
            [AppPage.Projects] = btnNavProjects,
            [AppPage.HrPayroll] = btnNavHr,
            [AppPage.Inventory] = btnNavInventory,
            [AppPage.Subcontractors] = btnNavSubcontractors,
            [AppPage.Receipts] = btnNavReceipts,
            [AppPage.Quotations] = btnNavQuotations,

            [AppPage.UserAccounts] = btnNavUsers,
            [AppPage.MyAccount] = btnNavMyAccount,
        };

        var u = Session.CurrentUser!;
        lblUserName.Text = u.FullName;
        lblUserRole.Text = u.IsAdmin ? "Admin" : $"{u.Department} - {u.Level}";
        btnNotifications.Visible = u.IsAdmin;
        Theme.StyleMainWindow(picLogo,
        [
            (btnNavDashboard, ""), (btnNavProjects, ""), (btnNavHr, ""), (btnNavInventory, ""),
            (btnNavSubcontractors, ""), (btnNavReceipts, ""), (btnNavUsers, ""), (btnNavMyAccount, ""),
            (btnSignOut, ""),
        ], lblUserName, lblUserRole);

        Theme.SetIcon(btnNavQuotations, "");   // a document
        lblPageTitle.UseMnemonic = lblUserRole.UseMnemonic = false;   // show "HR & Payroll", not "HR Payroll"
        pnlSidebar.AutoScroll = true;   // HR's 7 sub-items can run past a short screen
        foreach (var (page, button) in navButtons)
        {
            button.Visible = Access.CanSee(u, page);
            button.Click += (_, _) => ShowPage(page);
        }
    }

    async void MainForm_Load(object sender, EventArgs e)
    {
        // Employees and Supervisors (one department): their department page is made once and kept, and its parts
        // stay listed in the sidebar the whole time, so going to My Account and back doesn't rebuild it
        // (owner request 2026-11-18: it was slow on low-end PCs).
        var u = Session.CurrentUser!;
        if (!u.IsAdmin)
            keptPage = new[] { AppPage.HrPayroll, AppPage.Inventory, AppPage.Subcontractors, AppPage.Receipts }
                .FirstOrDefault(p => Access.CanSee(u, p), AppPage.MyAccount);
        if (keptPage is { } dept && dept != AppPage.MyAccount)
        {
            var view = MakePage(dept);
            AddToContent(view, visible: false);
            kept[dept] = view;
            BuildSubPages(dept, view.Controls.OfType<TabControl>().FirstOrDefault());
        }
        else keptPage = null;

        ShowPage(Access.FirstPage(u));
        IdleMonitor.Instance.Start();
        tmrIdle.Start();
        await RefreshNotificationCountAsync();
    }

    AppPage? keptPage;                                     // Employee / Supervisor: their department page
    readonly Dictionary<AppPage, UserControl> kept = [];   // pages made once and kept (shown / hidden, never rebuilt)
    AppPage? openPage;

    static UserControl MakePage(AppPage page) => page switch
    {
        AppPage.Dashboard => new DashboardPage(),
        AppPage.MyAccount => new MyAccountPage(),
        AppPage.UserAccounts => new UserAccountsPage(),
        AppPage.Projects => new ProjectsPage(),
        AppPage.HrPayroll => new HrPayrollPage(),
        AppPage.Inventory => new InventoryPage(),
        AppPage.Subcontractors => new SubcontractorsPage(),
        AppPage.Receipts => new ReceiptsPage(),
        AppPage.Quotations => new QuotationsPage(),
        _ => new PlaceholderPage(Access.Title(page)),   // real pages replace this as they're built
    };

    void AddToContent(UserControl view, bool visible)
    {
        var tabs = view.Controls.OfType<TabControl>().FirstOrDefault();
        if (tabs != null) HiddenTabs.Hide(tabs);   // its tabs are listed in the sidebar instead
        view.Dock = DockStyle.Fill;
        view.Visible = visible;
        pnlContent.Controls.Add(view);
    }

    /// <summary>Loads a page into the main area (a kept page is only shown again, not rebuilt).</summary>
    void ShowPage(AppPage page)
    {
        if (!Access.CanSee(Session.CurrentUser!, page)) return;
        if (page == openPage && kept.ContainsKey(page)) return;

        pnlContent.SuspendLayout();
        foreach (var old in pnlContent.Controls.OfType<Control>().ToList())
        {
            if (old is UserControl uc && kept.ContainsValue(uc)) { old.Visible = false; continue; }
            pnlContent.Controls.Remove(old);
            old.Dispose();
        }
        if (kept.TryGetValue(page, out var view)) view.Visible = true;
        else { view = MakePage(page); AddToContent(view, visible: true); }
        pnlContent.ResumeLayout();
        openPage = page;

        lblPageTitle.Text = Access.Title(page);
        var tabs = view.Controls.OfType<TabControl>().FirstOrDefault();
        if (keptPage == null) BuildSubPages(page, tabs);   // Admins: the open page's parts only
        else MarkSubPage(page == keptPage ? tabs : null);  // Employees / Supervisors: always listed, marked when open
        foreach (var (p, b) in navButtons)
        {
            b.Font = p == page ? navFontOpen : Font;   // bold = the page that's open
            Theme.SetActiveNav(b, p == page);   // and orange (UI/Theme)
        }
    }

    readonly List<Button> subButtons = [];
    TabControl? subTabs;

    /// <summary>
    /// Pages with tabs (HR & Payroll, Inventory, Subcontractors, Receipts) list their tabs under their sidebar
    /// button instead of on the page (owner request 2026-11-03). Admins see the open page's parts; Employees and
    /// Supervisors always see their department's parts (2026-11-18). The tabs a user may not see were already removed
    /// by the page. Tab titles with alerts ("Materials (2 low)") update here too.
    /// </summary>
    void BuildSubPages(AppPage page, TabControl? tabs)
    {
        pnlSidebar.SuspendLayout();
        foreach (var old in subButtons) { pnlSidebar.Controls.Remove(old); old.Dispose(); }
        subButtons.Clear();
        subTabs = tabs;
        foreach (var nav in navButtons.Values) nav.Margin = nav.Margin with { Bottom = 6 };
        if (tabs != null)
        {
            var parent = navButtons[page];
            int at = pnlSidebar.Controls.GetChildIndex(parent);
            foreach (TabPage tab in tabs.TabPages)
            {
                var b = new Button
                {
                    Name = "btnNavTab" + tab.Name[3..],
                    Text = tab.Text.Replace("&", "&&"),   // a single & would underline the next letter
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = Font,
                    Size = new Size(parent.Width - 18, Theme.Y(36)),
                    Margin = new Padding(18, 0, 0, 4),
                };
                b.Click += (_, _) =>
                {
                    if (openPage != page) ShowPage(page);   // from My Account etc.: back to the department page
                    tabs.SelectedTab = tab;
                };
                tab.TextChanged += (_, _) => { if (!b.IsDisposed) b.Text = tab.Text.Replace("&", "&&"); };
                pnlSidebar.Controls.Add(b);
                pnlSidebar.Controls.SetChildIndex(b, ++at);
                subButtons.Add(b);
            }
            parent.Margin = parent.Margin with { Bottom = 4 };
            tabs.SelectedIndexChanged += (_, _) => { if (openPage == page) MarkSubPage(tabs); };
        }
        pnlSidebar.ResumeLayout();
        MarkSubPage(tabs);
    }

    /// <summary>Marks the open part light orange; null = none marked (another page is open).</summary>
    void MarkSubPage(TabControl? tabs)
    {
        if (tabs != null && tabs.IsDisposed) return;
        for (int i = 0; i < subButtons.Count; i++) Theme.SetActiveSubNav(subButtons[i], tabs != null && i == tabs.SelectedIndex);
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
