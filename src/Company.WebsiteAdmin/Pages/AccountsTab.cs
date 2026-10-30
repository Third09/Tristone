using Company.OfficeApp.UI;
using Company.Shared;
using Company.Shared.Models;
using Company.WebsiteAdmin.Forms;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > Accounts (main accounts only): list, add, change, set password, unlock, turn off / on.
/// Same safe pattern as the office app's list pages (RecordForm): the list never selects a row by itself,
/// Create Account opens a pop-up, and saving a change asks Yes/No listing the changes.
/// </summary>
public partial class AccountsTab : UserControl
{
    int? editingId;          // null = nothing selected (or the "add only" pop-up)
    AccountInput? original;  // the form as loaded, to list what changed before saving
    bool filling;            // true while the list reloads, so the grid's own selection changes are ignored
    readonly bool addOnly;   // this copy is the Create Account pop-up

    /// <summary>The Create Account pop-up reports the new account's id here.</summary>
    public event Action<int>? Added;

    public AccountsTab() : this(false) { }

    public AccountsTab(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        dgvAccounts.AutoGenerateColumns = false;
        RecordForm.NoAutoSelect(dgvAccounts, on => filling = on, RestoreSelection);
        if (addOnly) { RecordForm.ShowFormOnly(this, lblFormMode); StartNew(); return; }
        Responsive.FitGrid(this, dgvAccounts);
        ShowNothingSelected();
    }

    async void AccountsTab_Load(object sender, EventArgs e) { if (!addOnly) await LoadAsync(); }

    async Task LoadAsync(int? selectId = null)
    {
        if (addOnly) return;
        using var busy = Responsive.Busy(this);
        var rows = await WebsiteAccounts.ListAsync();
        selectId ??= editingId;
        filling = true;
        dgvAccounts.DataSource = rows;
        dgvAccounts.ClearSelection();   // never pick the first row by itself
        if (selectId is { } id) Responsive.SelectRow(dgvAccounts, (AccountRow r) => r.AccountId == id);
        filling = false;

        if (Selected is not { } selected) ShowNothingSelected();
        else if (selected.AccountId != editingId) StartEdit(selected);
        UpdateButtons();
    }

    AccountRow? Selected => dgvAccounts.SelectedRows.Count > 0 ? dgvAccounts.SelectedRows[0].DataBoundItem as AccountRow : null;

    void UpdateButtons()
    {
        var a = Selected;
        btnSetAccountPassword.Enabled = a != null;
        btnUnlockAccount.Enabled = a?.Status == UserStatuses.Locked;
        btnDisableAccount.Enabled = a != null && a.Status != UserStatuses.Disabled;
        btnEnableAccount.Enabled = a?.Status == UserStatuses.Disabled;
    }

    void dgvAccounts_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        UpdateButtons();
        if (Selected is { } a) StartEdit(a); else ShowNothingSelected();
    }

    /// <summary>After the grid picked a row by itself on first show (RecordForm.NoAutoSelect): only the record being shown stays selected.</summary>
    void RestoreSelection()
    {
        filling = true;
        dgvAccounts.ClearSelection();
        if (editingId is { } id) Responsive.SelectRow(dgvAccounts, (AccountRow r) => r.AccountId == id);
        filling = false;
        if (Selected == null) ShowNothingSelected();
        UpdateButtons();
    }

    void ShowNothingSelected()
    {
        StartNew();
        lblFormMode.Text = "Click an account in the list to see or change it. To add one, click Create Account.";
        RecordForm.SetEnabled(this, lblFormMode, false);
    }

    void StartNew()
    {
        editingId = null;
        original = null;
        lblFormMode.Text = "New Website Admin account";
        txtFullName.Clear();
        txtUsername.Clear();
        txtPassword.Clear();
        txtPassword.Enabled = lblPassword.Enabled = btnGeneratePassword.Enabled = true;
        chkIsMain.Checked = false;
        lblAccountMsg.Text = "";
    }

    void StartEdit(AccountRow a)
    {
        RecordForm.SetEnabled(this, lblFormMode, true);
        editingId = a.AccountId;
        lblFormMode.Text = $"Editing: {a.FullName}";
        txtFullName.Text = a.FullName;
        txtUsername.Text = a.Username;
        txtPassword.Clear();
        txtPassword.Enabled = lblPassword.Enabled = btnGeneratePassword.Enabled = false;   // use Set New Password instead
        chkIsMain.Checked = a.IsMain;
        lblAccountMsg.Text = "";
        original = ReadForm();
    }

    AccountInput ReadForm() => new(txtFullName.Text, txtUsername.Text, chkIsMain.Checked);

    void btnGeneratePassword_Click(object sender, EventArgs e) => txtPassword.Text = AccountRules.GeneratePassword();

    async void btnCreateAccount_Click(object sender, EventArgs e)
    {
        var page = new AccountsTab(addOnly: true);
        if (RecordForm.AddDialog(this, "Create Account", page, a => page.Added += a) is not { } newId) return;
        editingId = null;
        await LoadAsync(newId);
        lblAccountMsg.Text = "Account created. Give the username and password to the person in person.";
    }

    void btnCancelAccount_Click(object sender, EventArgs e)
    {
        if (addOnly) { FindForm()?.Close(); return; }
        if (Selected is { } a) StartEdit(a);
    }

    async void btnSaveAccount_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (WebsiteAccounts.CheckInput(input) is { } problem) { lblAccountMsg.Text = problem; return; }
        if (!addOnly && editingId == null) return;   // the form is locked when nothing is selected
        if (editingId != null && original != null)
        {
            var answer = RecordForm.ConfirmSave(this, original.FullName, original, input,
                new Dictionary<string, string> { ["IsMain"] = "Main account" });
            if (answer == SaveAnswer.NothingChanged) { lblAccountMsg.Text = "Nothing changed."; return; }
            if (answer == SaveAnswer.No) { lblAccountMsg.Text = "Not saved. Click Cancel to undo your changes."; return; }
        }

        btnSaveAccount.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await WebsiteAccounts.UpdateAsync(id, input);
                if (error != null) { lblAccountMsg.Text = error; return; }
                editingId = null;   // reload the form from the saved account
                await LoadAsync(id);
                lblAccountMsg.Text = "Saved.";
            }
            else
            {
                var (newId, error) = await WebsiteAccounts.CreateAsync(input, txtPassword.Text);
                if (newId is { } created) Added?.Invoke(created); else lblAccountMsg.Text = error;
            }
        }
        catch (Exception ex) { lblAccountMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveAccount.Enabled = true; }
    }

    // ----- list actions -----

    async void btnSetAccountPassword_Click(object sender, EventArgs e)
    {
        if (Selected is not { } a) return;
        using var f = new SetAccountPasswordForm(a.AccountId, $"{a.FullName} ({a.Username})");
        if (f.ShowDialog(this) == DialogResult.OK) { editingId = null; await LoadAsync(a.AccountId); lblAccountMsg.Text = "New password saved. Give it to the person in person."; }
    }

    async void btnUnlockAccount_Click(object sender, EventArgs e) => await RunAsync(WebsiteAccounts.UnlockAsync, null, "Unlocked.");

    async void btnDisableAccount_Click(object sender, EventArgs e) =>
        await RunAsync(WebsiteAccounts.DisableAsync, "Turn off this account? The person won't be able to sign in to Website Admin.", "Turned off.");

    async void btnEnableAccount_Click(object sender, EventArgs e) => await RunAsync(WebsiteAccounts.EnableAsync, null, "Turned on.");

    async Task RunAsync(Func<int, Task<string?>> action, string? confirm, string done)
    {
        if (Selected is not { } a) return;
        if (confirm != null && MessageBox.Show(this, $"{a.FullName}: {confirm}", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        var error = await action(a.AccountId);
        if (error != null) { MessageBox.Show(this, error, "Not done", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        editingId = null;
        await LoadAsync(a.AccountId);
        lblAccountMsg.Text = done;
    }
}
