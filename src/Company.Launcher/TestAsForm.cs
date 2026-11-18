using Company.OfficeApp.Data;
using Company.OfficeApp.Services;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Launcher;

/// <summary>
/// Demo / Test version only (owner request 2026-11-17): after signing in as demo.admin, pick any active login and
/// Tri-Stone opens as that person, without their password, to try every role from one sign-in. Sign Out in the
/// app comes back here. The Office and Standalone versions never show this (Program checks <see cref="AppEdition.IsSampleDemo"/>).
/// </summary>
public partial class TestAsForm : Form
{
    /// <summary>The login that signs in to the picker.</summary>
    public const string PickerLogin = "demo.admin";

    public record TestUserRow(int UserId, string FullName, string Username, string Level, string Department, string Opens);

    List<TestUserRow> all = [];

    /// <summary>The login picked; null = Sign Out.</summary>
    public User? Chosen { get; private set; }

    public TestAsForm()
    {
        InitializeComponent();
        Text = "Tri-Stone Demo - Test as...";
        StartPosition = FormStartPosition.CenterScreen;
        dgvTestUsers.AutoGenerateColumns = false;
        dgvTestUsers.MultiSelect = false;
        AcceptButton = btnOpenAsUser;
    }

    /// <summary>Shows the picker; returns the login to open as, or null for Sign Out.</summary>
    public static User? Pick()
    {
        using var f = new TestAsForm();
        f.ShowDialog();
        return f.Chosen;
    }

    async void TestAsForm_Load(object sender, EventArgs e)
    {
        using var db = Db.Open();
        var users = await db.Users.AsNoTracking().Where(u => u.Status == "Active").ToListAsync();
        static int Order(string level) => level switch
        {
            UserLevels.Admin => 0, UserLevels.Supervisor => 2, UserLevels.Employee => 3, UserLevels.WebsiteAdmin => 4, _ => 5,
        };
        all = users.OrderBy(u => u.Department ?? "").ThenBy(u => Order(u.Level)).ThenBy(u => u.FullName)
            .Select(u => new TestUserRow(u.UserId, u.FullName, u.Username, u.Level, u.Department ?? "-",
                u.IsWebsiteAdmin ? "Website Admin" : u.IsAdmin ? "Office or Website" : "Office App"))
            .ToList();
        ShowRows();
        txtTestAsSearch.Focus();
    }

    void ShowRows()
    {
        var s = txtTestAsSearch.Text.Trim().ToLower();
        dgvTestUsers.DataSource = s == "" ? all
            : all.Where(r => r.FullName.ToLower().Contains(s) || r.Username.ToLower().Contains(s) || r.Department.ToLower().Contains(s)
                          || r.Level.ToLower().Contains(s)).ToList();
        dgvTestUsers.ClearSelection();
        btnOpenAsUser.Enabled = dgvTestUsers.Rows.Count > 0;
    }

    void txtTestAsSearch_TextChanged(object? sender, EventArgs e) => ShowRows();

    TestUserRow? Selected => dgvTestUsers.SelectedRows.Count > 0 ? dgvTestUsers.SelectedRows[0].DataBoundItem as TestUserRow
                           : dgvTestUsers.Rows.Count == 1 ? dgvTestUsers.Rows[0].DataBoundItem as TestUserRow : null;

    async void btnOpenAsUser_Click(object? sender, EventArgs e)
    {
        if (Selected is not { } row) { MessageBox.Show(this, "Click a login in the list first.", Text); return; }
        using var db = Db.Open();
        Chosen = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == row.UserId && u.Status == "Active");
        if (Chosen == null) { MessageBox.Show(this, "That login is no longer active.", Text); return; }
        Close();
    }

    void dgvTestUsers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnOpenAsUser_Click(sender, e);
    }

    void btnCancelTestAs_Click(object? sender, EventArgs e) { Chosen = null; Close(); }
}
