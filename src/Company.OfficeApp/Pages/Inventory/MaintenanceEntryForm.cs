using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Inventory;

/// <summary>
/// Adds a maintenance or repair entry for one piece of equipment. A next maintenance date moves the reminder.
/// Costs here are company-wide overhead. Plain layout for now; control names match the UI Elements Guide.
/// </summary>
public partial class MaintenanceEntryForm : Form
{
    readonly int equipmentId;

    public MaintenanceEntryForm(int equipmentId, string equipmentName)
    {
        InitializeComponent();
        this.equipmentId = equipmentId;
        Text = $"Maintenance Entry - {equipmentName}";
        cboMaintType.Items.AddRange(InventoryLists.MaintenanceKinds);
        cboMaintType.SelectedIndex = 0;
        dtpMaintDate.Value = DateTime.Today;
        dtpMaintDate.MaxDate = DateTime.Today;
        dtpMaintNextDue.Value = DateTime.Today.AddMonths(3);
        dtpMaintNextDue.Checked = false;
    }

    async void btnSaveMaintenance_Click(object sender, EventArgs e)
    {
        var input = new MaintenanceInput(DateOnly.FromDateTime(dtpMaintDate.Value), (string)cboMaintType.SelectedItem!, txtMaintWork.Text,
            numMaintCost.Value, txtMaintDoneBy.Text, dtpMaintNextDue.Checked ? DateOnly.FromDateTime(dtpMaintNextDue.Value) : null);
        if (EquipmentService.CheckLog(input) is { } problem) { lblMaintMsg.Text = problem; return; }
        btnSaveMaintenance.Enabled = false;
        var (_, error) = await EquipmentService.AddLogAsync(equipmentId, input);
        btnSaveMaintenance.Enabled = true;
        if (error != null) { lblMaintMsg.Text = error; return; }
        DialogResult = DialogResult.OK;
    }

    void btnCancelMaintenance_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;
}
