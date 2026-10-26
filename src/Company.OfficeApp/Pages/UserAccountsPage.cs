using Company.OfficeApp.Forms;
using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages;

/// <summary>
/// User Accounts (Admin only): list, create, edit, set password, unlock, disable, enable.
/// Each login is for one office employee, picked from HR & Payroll's Employee list; the name and department
/// come from the employee.
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
    }

    async void UserAccountsPage_Load(object sender, EventArgs e)
    {
        await StartNewAsync();
        await LoadUsersAsync();
    }

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

    // SelectedRows, not CurrentRow: after ClearSelection (Create Account) the grid still has a current row.
    UserRow? Selected => dgvUsers.SelectedRows.Count > 0 ? dgvUsers.SelectedRows[0].DataBoundItem as UserRow : null;

    void UpdateButtons()
    {
        var u = Selected;
        btnSetUserPassword.Enabled = u != null;
        btnUnlockUser.Enabled = u?.Status == UserStatuses.Locked;
        btnDisableUser.Enabled = u != null && u.Status != UserStatuses.Disabled;
        btnEnableUser.Enabled = u?.Status == UserStatuses.Disabled;
    }

    async void Filter_Changed(object sender, EventArgs e) => await LoadUsersAsync();

    async void dgvUsers_SelectionChanged(object sender, EventArgs e)
    {
        UpdateButtons();
        if (Selected is { } u) await StartEditAsync(u);
    }

    // ----- form -----

    // An unlinked old account (made before employees existed) shows this entry so it can stay unlinked.
    static readonly EmployeeOption NotLinked = new(null, "(not linked to an employee)", "");

    async Task StartNewAsync()
    {
        editingUserId = null;
        lblFormMode.Text = "New account";
        cboUserEmployee.Items.Clear();
        cboUserEmployee.Items.AddRange([.. await UserAdminService.LinkableEmployeesAsync(null)]);
        cboUserEmployee.SelectedIndex = -1;
        txtNewUsername.Clear();
        txtNewUserPassword.Clear();
        txtNewUserPassword.Enabled = lblNewUserPassword.Enabled = true;
        cboUserLevel.SelectedItem = UserLevels.Employee;
        lblUserFormMsg.Text = cboUserEmployee.Items.Count == 0
            ? "No office staff without a login. Add the person in HR & Payroll > Employees first."
            : "";
        ShowDepartment();
    }

    async Task StartEditAsync(UserRow u)
    {
        editingUserId = u.UserId;
        lblFormMode.Text = $"Editing: {u.FullName}";
        var options = await UserAdminService.LinkableEmployeesAsync(u.UserId);
        cboUserEmployee.Items.Clear();
        if (u.EmployeeId == null) cboUserEmployee.Items.Add(NotLinked);
        cboUserEmployee.Items.AddRange([.. options]);
        cboUserEmployee.SelectedItem = u.EmployeeId == null ? NotLinked : options.FirstOrDefault(o => o.EmployeeId == u.EmployeeId);
        txtNewUsername.Text = u.Username;
        txtNewUserPassword.Clear();
        txtNewUserPassword.Enabled = lblNewUserPassword.Enabled = false;   // use Set New Password instead
        cboUserLevel.SelectedItem = u.Level;
        lblUserFormMsg.Text = "";
        ShowDepartment(u.Department);
    }

    /// <summary>Department follows the employee (none for Admins). Shown only; it can't be picked.</summary>
    void ShowDepartment(string? unlinkedDepartment = null)
    {
        var level = (string?)cboUserLevel.SelectedItem ?? "";
        string? dept = cboUserEmployee.SelectedItem switch
        {
            EmployeeOption { EmployeeId: not null } o => UserAdminService.DepartmentFor(level, o.Department),
            _ => level == UserLevels.Admin ? null : unlinkedDepartment,
        };
        cboUserDepartment.Items.Clear();
        if (dept != null) { cboUserDepartment.Items.Add(dept); cboUserDepartment.SelectedIndex = 0; }
    }

    void cboUserLevel_SelectedIndexChanged(object sender, EventArgs e) => ShowDepartment(Selected?.Department);

    void cboUserEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowDepartment(Selected?.Department);
        // Suggest a username for new accounts, e.g. "Juan Dela Cruz" -> "jdelacruz".
        if (editingUserId == null && txtNewUsername.Text == "" && cboUserEmployee.SelectedItem is EmployeeOption { EmployeeId: not null } o)
            txtNewUsername.Text = SuggestUsername(o.FullName);
    }

    // Words that belong to a surname: "Juan Dela Cruz" -> "jdelacruz", "Ana de los Santos" -> "adelossantos".
    static readonly HashSet<string> SurnameParticles = ["de", "del", "dela", "la", "los", "las", "delos", "san", "santa", "sta", "sto", "van", "von", "di"];

    /// <summary>First letter of the first name + the surname, e.g. "Shem John Adrian P. Morilla" -> "smorilla".</summary>
    public static string SuggestUsername(string fullName)
    {
        var parts = fullName.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => new string(p.Where(char.IsLetterOrDigit).ToArray())).Where(p => p != "").ToList();
        if (parts.Count == 0) return "";
        if (parts.Count == 1) return parts[0].Length > 50 ? parts[0][..50] : parts[0];

        int start = parts.Count - 1;
        while (start > 1 && SurnameParticles.Contains(parts[start - 1])) start--;
        var name = parts[0][0] + string.Concat(parts.Skip(start));
        return name.Length > 50 ? name[..50] : name;
    }

    async void btnCreateAccount_Click(object sender, EventArgs e)
    {
        dgvUsers.ClearSelection();
        await StartNewAsync();
        cboUserEmployee.Focus();
    }

    async void btnCancelUser_Click(object sender, EventArgs e)
    {
        if (editingUserId != null && Selected is { } u) await StartEditAsync(u); else await StartNewAsync();
    }

    async void btnSaveUser_Click(object sender, EventArgs e)
    {
        var employee = cboUserEmployee.SelectedItem as EmployeeOption;
        var input = new UserInput(employee?.EmployeeId, txtNewUsername.Text, (string?)cboUserLevel.SelectedItem ?? "");

        if (employee == null) { lblUserFormMsg.Text = "Choose the employee this login is for."; return; }
        if (UserAdminService.CheckInput(input) is { } problem) { lblUserFormMsg.Text = problem; return; }

        // The plan is 1 Employee and 1 Supervisor per department: ask before adding a second.
        var department = cboUserDepartment.SelectedItem as string;
        var others = await UserAdminService.OthersInSameRoleAsync(input.Level, department, editingUserId);
        if (others.Count > 0)
        {
            var ok = MessageBox.Show(this,
                $"{department} already has an active {input.Level}: {string.Join(", ", others)}.\n\nSave anyway?",
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
