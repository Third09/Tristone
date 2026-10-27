using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Inventory;

/// <summary>
/// Inventory > Equipment: tools, heavy equipment and vehicles (owned or rented), with the maintenance & repair
/// log and a reminder when maintenance is due. Location is not tracked.
/// Plain layout for now; control names match the UI Elements Guide (Part 3: Tab: Equipment).
/// </summary>
public partial class EquipmentTab : UserControl
{
    int? editingId;
    readonly LatestOnly loads = new();

    /// <summary>Raised after changes, so the page can refresh the "due" count on the tab.</summary>
    public event EventHandler? Changed;

    public EquipmentTab()
    {
        InitializeComponent();
        dgvEquipment.AutoGenerateColumns = false;
        dgvMaintenanceLog.AutoGenerateColumns = false;
        colLCost.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        cboEquipmentTypeFilter.Items.Add("All");
        cboEquipmentTypeFilter.Items.AddRange(InventoryLists.EquipmentTypes);
        cboEquipmentTypeFilter.SelectedIndex = 0;
        cboEquipmentStatusFilter.Items.Add("All");
        cboEquipmentStatusFilter.Items.AddRange(InventoryLists.EquipmentStatuses);
        cboEquipmentStatusFilter.SelectedIndex = 0;
        cboEquipmentType.Items.AddRange(InventoryLists.EquipmentTypes);
        cboEquipmentStatus.Items.AddRange(InventoryLists.EquipmentStatuses);
        cboRentalRateUnit.Items.AddRange(InventoryLists.RateUnits);
        Responsive.FitGrid(this, dgvEquipment);
        Responsive.OnTypingPause(txtEquipmentSearch, () => LoadAsync());
        StartNew();
    }

    async void EquipmentTab_Load(object sender, EventArgs e) => await LoadAsync();

    async Task LoadAsync(int? selectId = null)
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        string? type = cboEquipmentTypeFilter.SelectedIndex > 0 ? (string)cboEquipmentTypeFilter.SelectedItem! : null;
        string? status = cboEquipmentStatusFilter.SelectedIndex > 0 ? (string)cboEquipmentStatusFilter.SelectedItem! : null;
        var rows = await EquipmentService.ListAsync(txtEquipmentSearch.Text, type, status);
        var due = await EquipmentService.MaintenanceDueCountAsync();
        if (!loads.IsLatest(ticket)) return;
        dgvEquipment.DataSource = rows;
        lblMaintenanceDue.Text = due == 0 ? "" : $"{due} item{(due == 1 ? " is" : "s are")} due for maintenance";
        if (selectId is { } id)
            Responsive.SelectRow(dgvEquipment, (EquipmentRow row) => row.EquipmentId == id);
    }

    EquipmentRow? Selected => dgvEquipment.SelectedRows.Count > 0 ? dgvEquipment.SelectedRows[0].DataBoundItem as EquipmentRow : null;
    MaintenanceRow? SelectedLog => dgvMaintenanceLog.SelectedRows.Count > 0 ? dgvMaintenanceLog.SelectedRows[0].DataBoundItem as MaintenanceRow : null;

    async void Filter_Changed(object sender, EventArgs e) => await LoadAsync();

    // Maintenance reminder: due rows light yellow (a function, not styling).
    void dgvEquipment_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.CellStyle != null && dgvEquipment.Rows[e.RowIndex].DataBoundItem is EquipmentRow { MaintenanceDue: true })
            e.CellStyle.BackColor = Color.LightYellow;
    }

    async void dgvEquipment_SelectionChanged(object sender, EventArgs e)
    {
        btnDeleteEquipment.Enabled = Selected != null;
        if (Selected is { } row && await EquipmentService.GetAsync(row.EquipmentId) is { } eq) await StartEditAsync(eq);
    }

    void StartNew()
    {
        editingId = null;
        lblFormMode.Text = "New equipment";
        txtEquipmentName.Clear();
        cboEquipmentType.SelectedIndex = 0;
        rdoOwned.Checked = true;
        txtRentalCompany.Clear();
        numRentalRate.Value = 0;
        cboRentalRateUnit.SelectedIndex = 0;
        dtpRentalReturn.Value = DateTime.Today;
        dtpRentalReturn.Checked = false;
        cboEquipmentStatus.SelectedIndex = 0;
        dtpNextMaintenance.Value = DateTime.Today;
        dtpNextMaintenance.Checked = false;
        dgvMaintenanceLog.DataSource = null;
        btnAddMaintenance.Enabled = btnDeleteMaintenance.Enabled = false;
        lblEquipmentMsg.Text = "";
        Ownership_Changed(this, EventArgs.Empty);
    }

    async Task StartEditAsync(Equipment eq)
    {
        editingId = eq.EquipmentId;
        lblFormMode.Text = $"Editing: {eq.Name}";
        txtEquipmentName.Text = eq.Name;
        cboEquipmentType.SelectedItem = eq.Type;
        rdoRented.Checked = eq.Ownership == "Rented";
        rdoOwned.Checked = eq.Ownership == "Owned";
        txtRentalCompany.Text = eq.RentalCompany ?? "";
        numRentalRate.Value = eq.RentalRate ?? 0;
        cboRentalRateUnit.SelectedItem = eq.RentalRateUnit ?? InventoryLists.RateUnits[0];
        dtpRentalReturn.Value = (eq.ReturnDate ?? DateOnly.FromDateTime(DateTime.Today)).ToDateTime(TimeOnly.MinValue);
        dtpRentalReturn.Checked = eq.ReturnDate != null;
        cboEquipmentStatus.SelectedItem = eq.Status;
        dtpNextMaintenance.Value = (eq.NextMaintenance ?? DateOnly.FromDateTime(DateTime.Today)).ToDateTime(TimeOnly.MinValue);
        dtpNextMaintenance.Checked = eq.NextMaintenance != null;
        lblEquipmentMsg.Text = "";
        Ownership_Changed(this, EventArgs.Empty);
        await LoadLogAsync();
    }

    async Task LoadLogAsync()
    {
        dgvMaintenanceLog.DataSource = editingId is { } id ? await EquipmentService.ListLogAsync(id) : null;
        btnAddMaintenance.Enabled = editingId != null;
        btnDeleteMaintenance.Enabled = dgvMaintenanceLog.Rows.Count > 0;
    }

    void Ownership_Changed(object? sender, EventArgs e)
    {
        bool rented = rdoRented.Checked;
        foreach (Control c in new Control[] { lblRentalCompany, txtRentalCompany, lblRentalRate, numRentalRate, cboRentalRateUnit, lblRentalReturn, dtpRentalReturn })
            c.Enabled = rented;
    }

    EquipmentInput ReadForm() => new(txtEquipmentName.Text, (string?)cboEquipmentType.SelectedItem ?? "",
        rdoRented.Checked ? "Rented" : "Owned", txtRentalCompany.Text, numRentalRate.Value, (string?)cboRentalRateUnit.SelectedItem,
        dtpRentalReturn.Checked ? DateOnly.FromDateTime(dtpRentalReturn.Value) : null, (string?)cboEquipmentStatus.SelectedItem ?? "",
        dtpNextMaintenance.Checked ? DateOnly.FromDateTime(dtpNextMaintenance.Value) : null);

    void btnAddEquipment_Click(object sender, EventArgs e) { dgvEquipment.ClearSelection(); StartNew(); txtEquipmentName.Focus(); }

    async void btnCancelEquipment_Click(object sender, EventArgs e)
    {
        if (editingId is { } id && await EquipmentService.GetAsync(id) is { } eq) await StartEditAsync(eq); else StartNew();
    }

    async void btnSaveEquipment_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (EquipmentService.CheckInput(input) is { } problem) { lblEquipmentMsg.Text = problem; return; }
        btnSaveEquipment.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await EquipmentService.UpdateAsync(id, input);
                lblEquipmentMsg.Text = error ?? "Saved.";
                if (error == null) { await LoadAsync(id); Changed?.Invoke(this, EventArgs.Empty); }
            }
            else
            {
                var (newId, error) = await EquipmentService.CreateAsync(input);
                lblEquipmentMsg.Text = error ?? "Equipment added.";
                if (newId != null) { await LoadAsync(newId); Changed?.Invoke(this, EventArgs.Empty); }
            }
        }
        catch (Exception ex) { lblEquipmentMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveEquipment.Enabled = true; }
    }

    async void btnDeleteEquipment_Click(object sender, EventArgs e)
    {
        if (Selected is not { } eq) return;
        if (MessageBox.Show(this, $"Delete \"{eq.Name}\"?", "Delete equipment", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        var error = await EquipmentService.DeleteAsync(eq.EquipmentId);
        if (error != null) { MessageBox.Show(this, error, "Not deleted", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        StartNew();
        await LoadAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    async void btnAddMaintenance_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id) return;
        using var f = new MaintenanceEntryForm(id, txtEquipmentName.Text);
        if (f.ShowDialog(this) != DialogResult.OK) return;
        await LoadAsync(id);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    async void btnDeleteMaintenance_Click(object sender, EventArgs e)
    {
        if (SelectedLog is not { } l) return;
        if (MessageBox.Show(this, $"Delete the {l.Kind.ToLower()} entry from {l.DoneOn:MMM d, yyyy}?", "Delete entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        lblEquipmentMsg.Text = await EquipmentService.DeleteLogAsync(l.LogId) ?? "Entry deleted.";
        await LoadLogAsync();
    }

    void btnExportEquipment_Click(object sender, EventArgs e)
    {
        if (dgvEquipment.DataSource is not List<EquipmentRow> rows) return;
        if (ExcelExport.AskPath(this, "Equipment") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Equipment", rows, new("Name", r => r.Name), new("Type", r => r.Type), new("Owned / Rented", r => r.Ownership),
                new("Status", r => r.Status), new("Next Maintenance", r => r.NextMaintenance, "mmm d, yyyy"), new("Maintenance Due", r => r.MaintenanceDue));
            MessageBox.Show(this, $"Saved {rows.Count} items to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
