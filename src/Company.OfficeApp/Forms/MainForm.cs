using Company.OfficeApp.Services;

namespace Company.OfficeApp.Forms;

/// <summary>
/// The main window. For now it only shows who is signed in; the sidebar and pages come next.
/// </summary>
public partial class MainForm : Form
{
    /// <summary>True when closed with Sign Out (go back to the Login window) rather than closing the app.</summary>
    public bool SignedOut { get; private set; }

    public MainForm()
    {
        InitializeComponent();
        var u = Session.CurrentUser!;
        lblUserName.Text = $"Signed in as {u.FullName} ({u.Username})";
        lblUserRole.Text = u.IsAdmin ? "Admin" : $"{u.Department} - {u.Level}";
        btnNotifications.Visible = u.IsAdmin;
    }

    async void MainForm_Load(object sender, EventArgs e) => await RefreshNotificationCountAsync();

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

    async void btnSignOut_Click(object sender, EventArgs e)
    {
        SignedOut = true;
        await AuthService.SignOutAsync();
        Close();
    }

    async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Closing the window with X also counts as signing out.
        if (!SignedOut) await AuthService.SignOutAsync();
    }
}
