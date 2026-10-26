using Company.OfficeApp.Services;

namespace Company.OfficeApp.Forms;

/// <summary>
/// Admin only: waiting Forgot Password requests. Opened from the notification button on the main window.
/// Plain layout for now; control names match the UI Elements Guide (Part A: Notifications Window).
/// </summary>
public partial class NotificationsForm : Form
{
    public NotificationsForm()
    {
        InitializeComponent();
        dgvPasswordRequests.AutoGenerateColumns = false;
    }

    async void NotificationsForm_Load(object sender, EventArgs e) => await LoadRequestsAsync();

    async Task LoadRequestsAsync()
    {
        var rows = await PasswordService.GetWaitingRequestsAsync();
        dgvPasswordRequests.DataSource = rows;
        lblEmpty.Visible = rows.Count == 0;
        btnSetNewPassword.Enabled = btnDismissRequest.Enabled = rows.Count > 0;
    }

    PasswordRequestRow? Selected => dgvPasswordRequests.CurrentRow?.DataBoundItem as PasswordRequestRow;

    async void btnSetNewPassword_Click(object sender, EventArgs e)
    {
        if (Selected is not { } row) return;
        using var f = new SetNewPasswordForm(row.UserId, $"{row.FullName} ({row.Username})");
        if (f.ShowDialog(this) == DialogResult.OK) await LoadRequestsAsync();
    }

    async void btnDismissRequest_Click(object sender, EventArgs e)
    {
        if (Selected is not { } row) return;
        var answer = MessageBox.Show(this, $"Dismiss the request from {row.FullName} without changing the password?",
            "Dismiss request", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (answer != DialogResult.Yes) return;
        await PasswordService.DismissRequestAsync(row.RequestId);
        await LoadRequestsAsync();
    }

    void btnCloseNotifications_Click(object sender, EventArgs e) => Close();
}
