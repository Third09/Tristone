using Company.OfficeApp.Services;
using Company.OfficeApp.UI;

namespace Company.OfficeApp.Pages.Shared;

/// <summary>
/// The "Edit History" tab every department page ends with: who added, changed or deleted what, and when,
/// one row per changed field. Read only. Used by HR & Payroll, Inventory, Subcontractors and Receipts.
/// Plain layout for now; control names match the UI Elements Guide (Tab: Edit History).
/// </summary>
public partial class EditHistoryTab : UserControl
{
    readonly string department;
    readonly LatestOnly loads = new();

    public EditHistoryTab(string department)
    {
        InitializeComponent();
        this.department = department;
        dgvEditHistory.AutoGenerateColumns = false;
        dtpHistFrom.Value = DateTime.Today.AddDays(-30);
        dtpHistTo.Value = DateTime.Today;
        Responsive.FitGrid(this, dgvEditHistory);
        Responsive.OnTypingPause(txtHistSearch, () => LoadAsync());
    }

    async void EditHistoryTab_Load(object sender, EventArgs e)
    {
        cboHistUser.Items.Add("Everyone");
        foreach (var (id, name) in await AuditService.PeopleAsync(department)) cboHistUser.Items.Add(new PersonItem(id, name));
        cboHistUser.SelectedIndex = 0;   // triggers the first load
    }

    record PersonItem(int UserId, string Name) { public override string ToString() => Name; }

    async void Filter_Changed(object sender, EventArgs e) => await LoadAsync();

    async Task LoadAsync()
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await AuditService.ListAsync(department,
            DateOnly.FromDateTime(dtpHistFrom.Value), DateOnly.FromDateTime(dtpHistTo.Value),
            (cboHistUser.SelectedItem as PersonItem)?.UserId, txtHistSearch.Text);
        if (!loads.IsLatest(ticket)) return;
        dgvEditHistory.DataSource = rows;
        lblHistCount.Text = $"{rows.Count} change{(rows.Count == 1 ? "" : "s")}";
    }

    void btnExportHistory_Click(object sender, EventArgs e)
    {
        if (dgvEditHistory.DataSource is not List<HistoryRow> rows) return;
        if (ExcelExport.AskPath(this, $"{department} Edit History") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Edit History", rows,
                new("Date & Time", r => r.ChangedAt, "mmm d, yyyy h:mm AM/PM"),
                new("Who", r => r.Who),
                new("Action", r => r.Action),
                new("Record", r => r.Record),
                new("Field", r => r.Field),
                new("Old Value", r => r.OldValue),
                new("New Value", r => r.NewValue));
            MessageBox.Show(this, $"Saved {rows.Count} rows to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
