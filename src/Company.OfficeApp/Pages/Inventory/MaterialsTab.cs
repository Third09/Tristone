using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Inventory;

/// <summary>
/// Inventory > Materials: the material master list with low-stock alerts. Stock is set once when a material is
/// added; after that it only changes through approved usage and deliveries.
/// Plain layout for now; control names match the UI Elements Guide (Part 3: Tab: Materials).
/// </summary>
public partial class MaterialsTab : UserControl
{
    int? editingId;
    readonly LatestOnly loads = new();

    /// <summary>Raised after any change, so the page can refresh the low-stock count on the tab.</summary>
    public event EventHandler? Changed;

    public MaterialsTab()
    {
        InitializeComponent();
        dgvMaterials.AutoGenerateColumns = false;
        colPrice.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        cboMaterialCategoryFilter.Items.Add("All");
        cboMaterialCategoryFilter.Items.AddRange(InventoryLists.MaterialCategories);
        cboMaterialCategoryFilter.SelectedIndex = 0;
        cboMaterialCategory.Items.AddRange(InventoryLists.MaterialCategories);
        cboMaterialUnit.Items.AddRange(InventoryLists.Units);
        Responsive.FitGrid(this, dgvMaterials);
        Responsive.OnTypingPause(txtMaterialSearch, () => LoadAsync());
        StartNew();
    }

    async void MaterialsTab_Load(object sender, EventArgs e) => await LoadAsync();

    async Task LoadAsync(int? selectId = null)
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        string? category = cboMaterialCategoryFilter.SelectedIndex > 0 ? (string)cboMaterialCategoryFilter.SelectedItem! : null;
        var rows = await MaterialService.ListAsync(txtMaterialSearch.Text, category, chkLowStockOnly.Checked);
        var low = await MaterialService.LowStockCountAsync();
        if (!loads.IsLatest(ticket)) return;
        dgvMaterials.DataSource = rows;
        lblLowStockCount.Text = low == 0 ? "" : $"{low} item{(low == 1 ? " is" : "s are")} below minimum stock";
        if (selectId is { } id)
            Responsive.SelectRow(dgvMaterials, (MaterialRow row) => row.MaterialId == id);
    }

    MaterialRow? Selected => dgvMaterials.SelectedRows.Count > 0 ? dgvMaterials.SelectedRows[0].DataBoundItem as MaterialRow : null;

    async void Filter_Changed(object sender, EventArgs e) => await LoadAsync();

    // Low-stock alert: low rows light red, out-of-stock rows stronger red (a function, not styling).
    void dgvMaterials_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvMaterials.Rows[e.RowIndex].DataBoundItem is not MaterialRow m || e.CellStyle == null) return;
        if (m.IsOutOfStock) e.CellStyle.BackColor = Color.LightCoral;
        else if (m.IsLowStock) e.CellStyle.BackColor = Color.MistyRose;
    }

    void dgvMaterials_SelectionChanged(object sender, EventArgs e)
    {
        btnDeleteMaterial.Enabled = Selected != null;
        if (Selected is { } m) StartEdit(m);
    }

    void StartNew()
    {
        editingId = null;
        lblFormMode.Text = "New material";
        txtMaterialName.Clear();
        cboMaterialCategory.Text = "";
        cboMaterialUnit.Text = "";
        numMaterialPrice.Value = 0;
        numStartingStock.Value = 0;
        numStartingStock.Enabled = lblStartingStock.Enabled = true;
        lblCurrentStock.Text = "Current stock: -";
        numMinimumStock.Value = 0;
        lblMaterialMsg.Text = "";
    }

    void StartEdit(MaterialRow m)
    {
        editingId = m.MaterialId;
        lblFormMode.Text = $"Editing: {m.Name}";
        txtMaterialName.Text = m.Name;
        cboMaterialCategory.Text = m.Category;
        cboMaterialUnit.Text = m.Unit;
        numMaterialPrice.Value = m.UnitPrice;
        numStartingStock.Value = 0;
        numStartingStock.Enabled = lblStartingStock.Enabled = false;   // stock changes only through usage and deliveries
        lblCurrentStock.Text = $"Current stock: {m.StockQty:#,##0.##} {m.Unit}";
        numMinimumStock.Value = m.MinimumStock;
        lblMaterialMsg.Text = "";
    }

    MaterialInput ReadForm() => new(txtMaterialName.Text, cboMaterialCategory.Text, cboMaterialUnit.Text,
        numMaterialPrice.Value, numStartingStock.Value, numMinimumStock.Value);

    void btnAddMaterial_Click(object sender, EventArgs e) { dgvMaterials.ClearSelection(); StartNew(); txtMaterialName.Focus(); }

    void btnCancelMaterial_Click(object sender, EventArgs e) { if (Selected is { } m) StartEdit(m); else StartNew(); }

    async void btnSaveMaterial_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (MaterialService.CheckInput(input) is { } problem) { lblMaterialMsg.Text = problem; return; }
        btnSaveMaterial.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await MaterialService.UpdateAsync(id, input);
                lblMaterialMsg.Text = error ?? "Saved.";
                if (error == null) { await LoadAsync(id); Changed?.Invoke(this, EventArgs.Empty); }
            }
            else
            {
                var (newId, error) = await MaterialService.CreateAsync(input);
                lblMaterialMsg.Text = error ?? "Material added.";
                if (newId != null) { await LoadAsync(newId); Changed?.Invoke(this, EventArgs.Empty); }
            }
        }
        catch (Exception ex) { lblMaterialMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveMaterial.Enabled = true; }
    }

    async void btnDeleteMaterial_Click(object sender, EventArgs e)
    {
        if (Selected is not { } m) return;
        if (MessageBox.Show(this, $"Delete \"{m.Name}\"?", "Delete material", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        var error = await MaterialService.DeleteAsync(m.MaterialId);
        if (error != null) { MessageBox.Show(this, error, "Not deleted", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        StartNew();
        await LoadAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    void btnExportMaterials_Click(object sender, EventArgs e)
    {
        if (dgvMaterials.DataSource is not List<MaterialRow> rows) return;
        if (ExcelExport.AskPath(this, "Materials") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Materials", rows,
                new("Name", r => r.Name), new("Category", r => r.Category), new("Unit", r => r.Unit),
                new("Unit Price", r => r.UnitPrice, "#,##0.00"), new("Stock", r => r.StockQty, "#,##0.##"),
                new("Minimum", r => r.MinimumStock, "#,##0.##"), new("Low Stock", r => r.IsLowStock));
            MessageBox.Show(this, $"Saved {rows.Count} materials to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
