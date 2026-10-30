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
    int? editingId;            // null = nothing selected (or the "add only" pop-up)
    MaterialInput? original;   // the form as loaded, to list what changed before saving
    bool filling;              // true while the list reloads, so the grid's own selection changes are ignored
    readonly bool addOnly;     // this copy is the Add Material pop-up (RecordForm)
    readonly LatestOnly loads = new();

    /// <summary>Raised after any change, so the page can refresh the low-stock count on the tab.</summary>
    public event EventHandler? Changed;

    /// <summary>The Add Material pop-up reports the new material's id here.</summary>
    public event Action<int>? Added;

    public MaterialsTab() : this(false) { }

    public MaterialsTab(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        dgvMaterials.AutoGenerateColumns = false;
        colPrice.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        cboMaterialCategoryFilter.Items.Add("All");
        cboMaterialCategoryFilter.Items.AddRange(InventoryLists.MaterialCategories);
        cboMaterialCategoryFilter.SelectedIndex = 0;
        cboMaterialCategory.Items.AddRange(InventoryLists.MaterialCategories);
        cboMaterialUnit.Items.AddRange(InventoryLists.Units);
        if (addOnly)
        {
            RecordForm.ShowFormOnly(this, lblFormMode, lblCurrentStock);
            StartNew();
            return;
        }
        Responsive.FitGrid(this, dgvMaterials);
        Responsive.OnTypingPause(txtMaterialSearch, () => LoadAsync());
        ShowNothingSelected();
    }

    async void MaterialsTab_Load(object sender, EventArgs e) { if (!addOnly) await LoadAsync(); }

    async Task LoadAsync(int? selectId = null)
    {
        if (addOnly) return;   // the Add pop-up has no list (its hidden filters still fire on start-up)
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        string? category = cboMaterialCategoryFilter.SelectedIndex > 0 ? (string)cboMaterialCategoryFilter.SelectedItem! : null;
        var rows = await MaterialService.ListAsync(txtMaterialSearch.Text, category, chkLowStockOnly.Checked);
        var low = await MaterialService.LowStockCountAsync();
        if (!loads.IsLatest(ticket)) return;

        selectId ??= editingId;   // searching keeps the material being edited selected
        filling = true;
        dgvMaterials.DataSource = rows;
        dgvMaterials.ClearSelection();   // never pick the first row by itself (RecordForm)
        if (selectId is { } id)
            Responsive.SelectRow(dgvMaterials, (MaterialRow row) => row.MaterialId == id);
        filling = false;
        lblLowStockCount.Text = low == 0 ? "" : $"{low} item{(low == 1 ? " is" : "s are")} below minimum stock";

        if (Selected is not { } selected) ShowNothingSelected();
        else if (selected.MaterialId != editingId) StartEdit(selected);
        btnDeleteMaterial.Enabled = Selected != null;
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
        if (filling) return;
        btnDeleteMaterial.Enabled = Selected != null;
        if (Selected is { } m) StartEdit(m); else ShowNothingSelected();
    }

    void ShowNothingSelected()
    {
        StartNew();
        lblFormMode.Text = "Click a material in the list to see or change it. To add one, click Add Material.";
        RecordForm.SetEnabled(this, lblFormMode, false);
    }

    void StartNew()
    {
        editingId = null;
        original = null;
        lblFormMode.Text = "New material";
        txtMaterialName.Clear();
        // SelectedIndex, not Text = "": clearing only the text keeps the old choice selected underneath,
        // and picking that same choice again then doesn't register.
        cboMaterialCategory.SelectedIndex = -1;
        cboMaterialUnit.SelectedIndex = -1;
        numMaterialPrice.Value = 0;
        numStartingStock.Value = 0;
        numStartingStock.Enabled = lblStartingStock.Enabled = true;
        lblCurrentStock.Text = "Current stock: -";
        numMinimumStock.Value = 0;
        lblMaterialMsg.Text = "";
    }

    void StartEdit(MaterialRow m)
    {
        RecordForm.SetEnabled(this, lblFormMode, true);
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
        original = ReadForm();
    }

    MaterialInput ReadForm() => new(txtMaterialName.Text, cboMaterialCategory.Text, cboMaterialUnit.Text,
        numMaterialPrice.Value, numStartingStock.Value, numMinimumStock.Value);

    async void btnAddMaterial_Click(object sender, EventArgs e)
    {
        var page = new MaterialsTab(addOnly: true);
        if (RecordForm.AddDialog(this, "Add Material", page, a => page.Added += a) is not { } newId) return;
        editingId = null;
        await LoadAsync(newId);
        lblMaterialMsg.Text = "Material added.";
        Changed?.Invoke(this, EventArgs.Empty);
    }

    void btnCancelMaterial_Click(object sender, EventArgs e)
    {
        if (addOnly) { FindForm()?.Close(); return; }
        if (Selected is { } m) StartEdit(m);
    }

    async void btnSaveMaterial_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (MaterialService.CheckInput(input) is { } problem) { lblMaterialMsg.Text = problem; return; }
        if (!addOnly && editingId == null) return;   // the form is locked when nothing is selected
        if (editingId != null && original != null)
        {
            var answer = RecordForm.ConfirmSave(this, original.Name, original, input);
            if (answer == SaveAnswer.NothingChanged) { lblMaterialMsg.Text = "Nothing changed."; return; }
            if (answer == SaveAnswer.No) { lblMaterialMsg.Text = "Not saved. Click Cancel to undo your changes."; return; }
        }
        btnSaveMaterial.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await MaterialService.UpdateAsync(id, input);
                if (error != null) { lblMaterialMsg.Text = error; return; }
                editingId = null;   // reload the form from the saved row
                await LoadAsync(id);
                lblMaterialMsg.Text = "Saved.";
                Changed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                var (newId, error) = await MaterialService.CreateAsync(input);
                if (newId is { } added) Added?.Invoke(added); else lblMaterialMsg.Text = error;
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
        editingId = null;
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
