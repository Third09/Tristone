using Company.OfficeApp.Data;

namespace Company.OfficeApp.Forms;

/// <summary>
/// Shown when the app can't reach the database (first run on a new PC, or the server address changed).
/// Saves to this PC only. Plain layout for now; styled later from the Figma design.
/// </summary>
public partial class DatabaseSettingsForm : Form
{
    public DatabaseSettingsForm(string? problem = null)
    {
        InitializeComponent();

        var s = Db.Settings;
        txtHost.Text = s.Host;
        numPort.Value = s.Port;
        txtDatabase.Text = s.Database;
        txtDbUser.Text = s.Username;
        txtDbPassword.Text = s.Password;

        if (problem != null) lblStatus.Text = problem;
    }

    DbSettings ReadForm()
    {
        var s = new DbSettings
        {
            Host = txtHost.Text.Trim(),
            Port = (int)numPort.Value,
            Database = txtDatabase.Text.Trim(),
            Username = txtDbUser.Text.Trim(),
        };
        s.Password = txtDbPassword.Text;
        return s;
    }

    async Task<bool> TestAsync(DbSettings s)
    {
        UseWaitCursor = true;
        btnTest.Enabled = btnSave.Enabled = false;
        lblStatus.Text = "Connecting...";
        try
        {
            var problem = await s.TestConnectionAsync();
            lblStatus.Text = problem ?? "Connected.";
            return problem == null;
        }
        finally
        {
            UseWaitCursor = false;
            btnTest.Enabled = btnSave.Enabled = true;
        }
    }

    async void btnTest_Click(object sender, EventArgs e) => await TestAsync(ReadForm());

    async void btnSave_Click(object sender, EventArgs e)
    {
        var s = ReadForm();
        if (!await TestAsync(s)) return;   // only save settings that work
        s.Save();
        Db.Settings = s;
        DialogResult = DialogResult.OK;
    }
}
