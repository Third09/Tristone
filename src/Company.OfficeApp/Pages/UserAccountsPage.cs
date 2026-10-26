using Company.OfficeApp.Forms;
using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages;

/// <summary>
/// User Accounts (Admin only): list, create, edit, set password, unlock, disable, enable.
/// For now the full name is typed; it will be picked from the Employee list once HR & Payroll is built.
/// Plain layout for now; control names match the UI Elements Guide (Part 2: User Accounts Page).
/// </summary>
public partial class UserAccountsPage : UserControl
{
    int? editingUserId;   // null = creating a new account

    public UserAccountsPage()
    {
        InitializeComponent();
        dgvUsers.AutoGenerateColumns = false;
        cboUserStatusFilter.Items.AddRange(["All", UserStatuses.Active, UserStatuses.Locked, UserStatuses.Disabled]);
        cboUserStatusFilter.SelectedIndex = 0;
        cboUserLevel.Items.AddRange(UserAdminService.LevelList);
        cboUserDepartment.Items.AddRange(UserAdminService.DepartmentList);
        StartNew();
    }

    async void UserAccountsPage_Load(object sender, EventArgs e) => await LoadUsersAsync();

    async Task LoadUsersAsync(int? selectUserId = null)
    {
        var status = cboUserStatusFilter.SelectedIndex > 0 ? (string)cboUserStatusFilter.SelectedItem! : null;
        var rows = await UserAdminService.ListAsync(txtUserSearch.Text, status);
        dgvUsers.DataSource = rows;

        if (selectUserId is { } id)
            foreach (DataGridViewRow r in dgvUsers.Rows)
                if (((UserRow)r.DataBoundItem!).UserId == id) { r.Selected = true; dgvUsers.CurrentCell = r.Cells[0]; }
        UpdateButtons();
    }

    UserRow? Selected => dgvUsers.CurrentRow?.DataBoundItem as UserRow;

    void UpdateButtons()
    {
        var u = Selected;
        btnSetUserPassword.Enabled = u != null;
        btnUnlockUser.Enabled = u?.Status == UserStatuses.Locked;
        btnDisableUser.Enabled = u != null && u.Status != UserStatuses.Disabled;
        btnEnableUser.Enabled = u?.Status == UserStatuses.Disabled;
    }

    async void Filter_Changed(object sender, EventArgs e) => await LoadUsersAsync();

    void dgvUsers_SelectionChanged(object sender, EventArgs e)
    {
        UpdateButtons();
        if (Selected is { } u) StartEdit(u);
    }

    // ----- form -----

    void StartNew()
    {
        editingUserId = null;
        lblFormMode.Text = "New account";
        txtUserFullName.Clear();
        txtNewUsername.Clear();
        txtNewUserPassword.Clear();
        txtNewUserPassword.Enabled = lblNewUserPassword.Enabled = true;
        cboUserLevel.SelectedItem = UserLevels.Employee;
        cboUserDepartment.SelectedIndex = -1;
        lblUserFormMsg.Text = "";
    }

    void StartEdit(UserRow u)
    {
        editingUserId = u.UserId;
        lblFormMode.Text = $"Editing: {u.FullName}";
        txtUserFullName.Text = u.FullName;
        txtNewUsername.Text = u.Username;
        txtNewUserPassword.Clear();
        txtNewUserPassword.Enabled = lblNewUserPassword.Enabled = false;   // use Set New Password instead
        cboUserLevel.SelectedItem = u.Level;
        cboUserDepartment.SelectedItem = u.Department;
        lblUserFormMsg.Text = "";
    }

    void cboUserLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        var isAdmin = (string?)cboUserLevel.SelectedItem == UserLevels.Admin;
        cboUserDepartment.Enabled = !isAdmin;
        if (isAdmin) cboUserDepartment.SelectedIndex = -1;
    }

    void btnCreateAccount_Click(object sender, EventArgs e)
    {
        dgvUsers.ClearSelection();
        StartNew();
        txtUserFullName.Focus();
    }

    void btnCancelUser_Click(object sender, EventArgs e)
    {
        if (editingUserId != null && Selected is { } u) StartEdit(u); else StartNew();
    }

    async void btnSaveUser_Click(object sender, EventArgs e)
    {
        var input = new UserInput(txtUserFullName.Text, txtNewUsername.Text,
            (string?)cboUserLevel.SelectedItem ?? "", (string?)cboUserDepartment.SelectedItem);

        if (UserAdminService.CheckInput(input) is { } problem) { lblUserFormMsg.Text = problem; return; }

        // The plan is 1 Employee and 1 Supervisor per department: ask before adding a second.
        var others = await UserAdminService.OthersInSameRoleAsync(input, editingUserId);
        if (others.Count > 0)
        {
            var ok = MessageBox.Show(this,
                $"{input.Department} already has an active {input.Level}: {string.Join(", ", others)}.\n\nSave anyway?",
                "Same role already taken", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ok != DialogResult.Yes) return;
        }

        btnSaveUser.Enabled = false;
        try
        {
            if (editingUserId is { } id)
            {
                var error = await UserAdminService.UpdateAsync(id, input);
                lblUserFormMsg.Text = error ?? "Saved.";
                if (error == null) await LoadUsersAsync(id);
            }
            else
            {
                var (newId, error) = await UserAdminService.CreateAsync(input, txtNewUserPassword.Text);
                lblUserFormMsg.Text = error ?? "Account created. Give the username and password to the person.";
                if (newId != null) await LoadUsersAsync(newId);
            }
        }
        catch (Exception ex) { lblUserFormMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveUser.Enabled = true; }
    }

    void btnGenerateUserPassword_Click(object sender, EventArgs e) => txtNewUserPassword.Text = PasswordService.GeneratePassword();

    // ----- list actions -----

    async void btnSetUserPassword_Click(object sender, EventArgs e)
    {
        if (Selected is not { } u) return;
        using var f = new SetNewPasswordForm(u.UserId, $"{u.FullName} ({u.Username})");
        if (f.ShowDialog(this) == DialogResult.OK) await LoadUsersAsync(u.UserId);
    }

    async void btnUnlockUser_Click(object sender, EventArgs e) => await RunAsync(UserAdminService.UnlockAsync, null);

    async void btnDisableUser_Click(object sender, EventArgs e) =>
        await RunAsync(UserAdminService.DisableAsync, "Turn off this account? The person won't be able to sign in. Their records are kept.");

    async void btnEnableUser_Click(object sender, EventArgs e) => await RunAsync(UserAdminService.EnableAsync, null);

    async Task RunAsync(Func<int, Task<string?>> action, string? confirm)
    {
        if (Selected is not { } u) return;
        if (confirm != null && MessageBox.Show(this, $"{u.FullName}: {confirm}", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        var error = await action(u.UserId);
        if (error != null) MessageBox.Show(this, error, "Not done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        await LoadUsersAsync(u.UserId);
    }
}
