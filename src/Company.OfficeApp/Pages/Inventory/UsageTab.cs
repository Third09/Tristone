using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Inventory;

/// <summary>
/// Inventory > Material Usage: the Employee records what was used on which project; the Supervisor (or an
/// Admin) approves or rejects. Approved use lowers the stock and becomes that project's material expense.
/// Plain layout for now; control names match the UI Elements Guide (Part 3: Tab: Material Usage).
/// </summary>
public partial class UsageTab : UserControl
{
    readonly LatestOnly loads = new();
    List<MaterialRow> materials = [];

    /// <summary>Raised after an approval changes stock, so the page can refresh other tabs.</summary>
    public event EventHandler? Changed;

    public UsageTab()
    {
        InitializeComponent();
        dgvUsageRequests.AutoGenerateColumns = false;
        dgvProjectMaterials.AutoGenerateColumns = false;
        colCost.DefaultCellStyle.FormatProvider = colHCost.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        cboUsageStatusFilter.Items.AddRange(["Waiting Approval", "Approved", "Rejected", "All"]);
        dtpUsageDate.Value = DateTime.Today;
        dtpUsageDate.MaxDate = DateTime.Today;

        bool approver = Access.IsApprover(Session.CurrentUser!, Departments.Inventory);
        btnApproveUsage.Visible = btnRejectUsage.Visible = approver;
        lblApproverNote.Visible = !approver;
        Responsive.FitGrid(this, dgvProjectMaterials);
    }

    async void UsageTab_Load(object sender, EventArgs e) => await ReloadListsAsync();

    /// <summary>Refreshes the project and material dropdowns (materials change when stock changes).</summary>
    public async Task ReloadListsAsync()
    {
        var projects = await MaterialService.ProjectOptionsAsync();
        materials = await MaterialService.ListAsync();

        var keepProject = (cboUsageProject.SelectedItem as ProjectOption)?.ProjectId;
        cboUsageProject.Items.Clear();
        cboUsageProject.Items.AddRange([.. projects]);
        cboUsageProject.SelectedItem = projects.FirstOrDefault(p => p.ProjectId == keepProject);

        var keepMaterial = (cboUsageMaterial.SelectedItem as MaterialRow)?.MaterialId;
        cboUsageMaterial.Items.Clear();
        cboUsageMaterial.Items.AddRange([.. materials]);
        cboUsageMaterial.SelectedItem = materials.FirstOrDefault(m => m.MaterialId == keepMaterial);

        var keepHistory = (cboHistoryProject.SelectedItem as ProjectOption)?.ProjectId;
        cboHistoryProject.Items.Clear();
        cboHistoryProject.Items.AddRange([.. projects]);
        cboHistoryProject.SelectedItem = projects.FirstOrDefault(p => p.ProjectId == keepHistory);

        if (cboUsageStatusFilter.SelectedIndex < 0) cboUsageStatusFilter.SelectedIndex = 0;   // loads the requests
        else await LoadRequestsAsync();
    }

    void cboUsageMaterial_SelectedIndexChanged(object sender, EventArgs e) =>
        lblAvailableStock.Text = cboUsageMaterial.SelectedItem is MaterialRow m ? $"In stock: {m.StockQty:#,##0.##} {m.Unit}" : "In stock: -";

    async void btnSubmitUsage_Click(object sender, EventArgs e)
    {
        if (cboUsageProject.SelectedItem is not ProjectOption p || cboUsageMaterial.SelectedItem is not MaterialRow m)
        { lblUsageMsg.Text = "Choose the project and the material."; return; }
        btnSubmitUsage.Enabled = false;
        try
        {
            var (_, error) = await MaterialService.SubmitUsageAsync(new UsageInput(p.ProjectId, m.MaterialId, numUsageQty.Value,
                DateOnly.FromDateTime(dtpUsageDate.Value), txtUsageNotes.Text));
            lblUsageMsg.Text = error ?? "Sent for approval.";
            if (error == null) { numUsageQty.Value = 0; txtUsageNotes.Clear(); await LoadRequestsAsync(); }
        }
        catch (Exception ex) { lblUsageMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSubmitUsage.Enabled = true; }
    }

    // ----- requests -----

    string? StatusFilter => cboUsageStatusFilter.SelectedIndex switch
    {
        0 => ApprovalStatuses.Waiting, 1 => ApprovalStatuses.Approved, 2 => ApprovalStatuses.Rejected, _ => null,
    };

    async void cboUsageStatusFilter_SelectedIndexChanged(object sender, EventArgs e) => await LoadRequestsAsync();

    async Task LoadRequestsAsync()
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await MaterialService.ListUsageAsync(StatusFilter);
        if (!loads.IsLatest(ticket)) return;
        dgvUsageRequests.DataSource = rows;
        UpdateButtons();
    }

    UsageRow? Selected => dgvUsageRequests.SelectedRows.Count > 0 ? dgvUsageRequests.SelectedRows[0].DataBoundItem as UsageRow : null;

    void UpdateButtons() => btnApproveUsage.Enabled = btnRejectUsage.Enabled = Selected?.Status == ApprovalStatuses.Waiting;

    void dgvUsageRequests_SelectionChanged(object sender, EventArgs e) => UpdateButtons();

    async void btnApproveUsage_Click(object sender, EventArgs e) => await DecideAsync(MaterialService.ApproveUsageAsync);

    async void btnRejectUsage_Click(object sender, EventArgs e) => await DecideAsync(MaterialService.RejectUsageAsync);

    async Task DecideAsync(Func<int, Task<string?>> decide)
    {
        if (Selected is not { } u) return;
        var error = await decide(u.UsageId);
        if (error != null) MessageBox.Show(this, error, "Not done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        await ReloadListsAsync();
        await LoadHistoryAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    // ----- project history -----

    async void cboHistoryProject_SelectedIndexChanged(object sender, EventArgs e) => await LoadHistoryAsync();

    async Task LoadHistoryAsync()
    {
        if (cboHistoryProject.SelectedItem is not ProjectOption p) { dgvProjectMaterials.DataSource = null; lblProjectMaterialTotal.Text = "Total: -"; return; }
        var (rows, total) = await MaterialService.ProjectHistoryAsync(p.ProjectId);
        dgvProjectMaterials.DataSource = rows;
        lblProjectMaterialTotal.Text = $"Total material cost: {ProjectService.Money(total)}";
    }

    void btnExportProjectMaterials_Click(object sender, EventArgs e)
    {
        if (dgvProjectMaterials.DataSource is not List<UsageRow> rows || cboHistoryProject.SelectedItem is not ProjectOption p) return;
        if (ExcelExport.AskPath(this, $"{p.Name} materials") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Materials Used", rows,
                new("Date", r => r.UsedOn, "mmm d, yyyy"), new("Material", r => r.Material), new("Qty", r => r.Quantity, "#,##0.##"),
                new("Unit", r => r.Unit), new("Cost", r => r.Cost, "#,##0.00"));
            MessageBox.Show(this, $"Saved to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
