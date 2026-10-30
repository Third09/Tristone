using Company.OfficeApp.Forms;
using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
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
    int? editingUserId;      // null = nothing selected (or the "add only" pop-up)
    UserInput? original;     // the form as loaded, to list what changed before saving
    bool filling;            // true while the list reloads, so the grid's own selection changes are ignored
    readonly bool addOnly;   // this copy is the Create Account pop-up (RecordForm)
    readonly LatestOnly loads = new();

    /// <summary>The Create Account pop-up reports the new account's id here.</summary>
    public event Action<int>? Added;

    public UserAccountsPage() : this(false) { }

    public UserAccountsPage(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        dgvUsers.AutoGenerateColumns = false;
        RecordForm.NoAutoSelect(dgvUsers, on => filling = on, RestoreSelection);
        cboUserStatusFilter.Items.AddRange(["All", UserStatuses.Active, UserStatuses.Locked, UserStatuses.Disabled]);
        cboUserStatusFilter.SelectedIndex = 0;
        cboUserLevel.Items.AddRange(UserAdminService.LevelList);
        if (addOnly) { RecordForm.ShowFormOnly(this, lblFormMode); return; }
        Responsive.FitGrid(this, dgvUsers);
        Responsive.OnTypingPause(txtUserSearch, () => LoadUsersAsync());
    }

    async void UserAccountsPage_Load(object sender, EventArgs e)
    {
        if (addOnly) { await StartNewAsync(); return; }
        ShowNothingSelected();
        await LoadUsersAsync();
    }

    async Task LoadUsersAsync(int? selectUserId = null)
    {
        if (addOnly) return;   // the Add pop-up has no list (its hidden filters still fire on start-up)
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var status = cboUserStatusFilter.SelectedIndex > 0 ? (string)cboUserStatusFilter.SelectedItem! : null;
        var rows = await UserAdminService.ListAsync(txtUserSearch.Text, status);
        if (!loads.IsLatest(ticket)) return;   // a newer search already started

        selectUserId ??= editingUserId;   // searching keeps the account being edited selected
        filling = true;
        dgvUsers.DataSource = rows;
        dgvUsers.ClearSelection();         // never pick the first row by itself (RecordForm)
        if (selectUserId is { } id)
            Responsive.SelectRow(dgvUsers, (UserRow row) => row.UserId == id);
        filling = false;

        if (Selected is not { } selected) ShowNothingSelected();
        else if (selected.UserId != editingUserId) await StartEditAsync(selected);
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
        if (filling) return;
        UpdateButtons();
        if (Selected is { } u) await StartEditAsync(u); else ShowNothingSelected();
    }

    /// <summary>After the grid picked a row by itself on first show (RecordForm.NoAutoSelect): only the record being shown stays selected.</summary>
    void RestoreSelection()
    {
        filling = true;
        dgvUsers.ClearSelection();
        if (editingUserId is { } id) Responsive.SelectRow(dgvUsers, (UserRow r) => r.UserId == id);
        filling = false;
        if (Selected == null) ShowNothingSelected();
        UpdateButtons();
    }

    void ShowNothingSelected()
    {
        editingUserId = null;
        original = null;
        lblFormMode.Text = "Click an account in the list to see or change it. To make one, click Create Account.";
        cboUserEmployee.Items.Clear();
        txtNewUsername.Clear();
        txtNewUserPassword.Clear();
        cboUserLevel.SelectedIndex = -1;
        cboUserDepartment.Items.Clear();
        lblUserFormMsg.Text = "";
        RecordForm.SetEnabled(this, lblFormMode, false);
    }

    // ----- form -----

    // An unlinked old account (made before employees existed) shows this entry so it can stay unlinked.
    static readonly EmployeeOption NotLinked = new(null, "(not linked to an employee)", "");

    async Task StartNewAsync()
    {
        editingUserId = null;
        original = null;
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
        RecordForm.SetEnabled(this, lblFormMode, true);
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
        original = ReadForm();
    }

    UserInput ReadForm() => new((cboUserEmployee.SelectedItem as EmployeeOption)?.EmployeeId, txtNewUsername.Text,
                                (string?)cboUserLevel.SelectedItem ?? "");

    /// <summary>Department follows the employee (none for Admins). Shown only; it can't be picked.</summary>
    void ShowDepartment(string? unlinkedDepartment = null)
    {
        var level = (string?)cboUserLevel.SelectedItem ?? "";
        string? dept = cboUserEmployee.SelectedItem switch
        {
            EmployeeOption { EmployeeId: not null } o => UserAdminService.DepartmentFor(level, o.Department),
            _ => UserLevels.HasNoDepartment(level) ? null : unlinkedDepartment,
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
        var page = new UserAccountsPage(addOnly: true);
        if (RecordForm.AddDialog(this, "Create Account", page, a => page.Added += a) is not { } newId) return;
        editingUserId = null;
        await LoadUsersAsync(newId);
        lblUserFormMsg.Text = "Account created. Give the username and password to the person.";
    }

    async void btnCancelUser_Click(object sender, EventArgs e)
    {
        if (addOnly) { FindForm()?.Close(); return; }
        if (editingUserId != null && Selected is { } u) await StartEditAsync(u);
    }

    async void btnSaveUser_Click(object sender, EventArgs e)
    {
        var employee = cboUserEmployee.SelectedItem as EmployeeOption;
        var input = ReadForm();

        if (employee == null) { lblUserFormMsg.Text = "Choose the employee this login is for."; return; }
        if (UserAdminService.CheckInput(input) is { } problem) { lblUserFormMsg.Text = problem; return; }
        if (!addOnly && editingUserId == null) return;   // the form is locked when nothing is selected
        if (editingUserId != null && original != null)
        {
            var answer = RecordForm.ConfirmSave(this, Selected?.FullName ?? input.Username, original, input,
                new Dictionary<string, string> { ["EmployeeId"] = "Employee" });
            if (answer == SaveAnswer.NothingChanged) { lblUserFormMsg.Text = "Nothing changed."; return; }
            if (answer == SaveAnswer.No) { lblUserFormMsg.Text = "Not saved. Click Cancel to undo your changes."; return; }
        }

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
                if (error != null) { lblUserFormMsg.Text = error; return; }
                editingUserId = null;   // reload the form from the saved account
                await LoadUsersAsync(id);
                lblUserFormMsg.Text = "Saved.";
            }
            else
            {
                var (newId, error) = await UserAdminService.CreateAsync(input, txtNewUserPassword.Text);
                if (newId is { } created) Added?.Invoke(created); else lblUserFormMsg.Text = error;
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
