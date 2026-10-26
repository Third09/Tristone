using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Inventory;

/// <summary>
/// Inventory > Vendors: supplier details, bank details, VAT status and their papers (company profile, bank
/// details, certificate of registration). Plain layout for now; control names match the UI Elements Guide.
/// </summary>
public partial class VendorsTab : UserControl
{
    int? editingId;
    readonly LatestOnly loads = new();

    /// <summary>Raised after a vendor is added, changed or deleted (other tabs list vendors).</summary>
    public event EventHandler? Changed;

    public VendorsTab()
    {
        InitializeComponent();
        dgvVendors.AutoGenerateColumns = false;
        dgvVendorDocs.AutoGenerateColumns = false;
        cboVendorVat.Items.AddRange(InventoryLists.VatStatuses);
        cboVendorDocType.Items.AddRange(InventoryLists.VendorDocTypes);
        cboVendorDocType.SelectedIndex = 0;
        Responsive.FitGrid(this, dgvVendors);
        Responsive.OnTypingPause(txtVendorSearch, () => LoadAsync());
        StartNew();
    }

    async void VendorsTab_Load(object sender, EventArgs e) => await LoadAsync();

    async Task LoadAsync(int? selectId = null)
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await PurchasingService.ListVendorsAsync(txtVendorSearch.Text);
        if (!loads.IsLatest(ticket)) return;
        dgvVendors.DataSource = rows;
        if (selectId is { } id)
            foreach (DataGridViewRow r in dgvVendors.Rows)
                if (((VendorRow)r.DataBoundItem!).VendorId == id) { r.Selected = true; dgvVendors.CurrentCell = r.Cells[0]; }
    }

    VendorRow? Selected => dgvVendors.SelectedRows.Count > 0 ? dgvVendors.SelectedRows[0].DataBoundItem as VendorRow : null;
    VendorDocRow? SelectedDoc => dgvVendorDocs.SelectedRows.Count > 0 ? dgvVendorDocs.SelectedRows[0].DataBoundItem as VendorDocRow : null;

    async void dgvVendors_SelectionChanged(object sender, EventArgs e)
    {
        btnDeleteVendor.Enabled = Selected != null;
        if (Selected is { } row && await PurchasingService.GetVendorAsync(row.VendorId) is { } v) await StartEditAsync(v);
    }

    void StartNew()
    {
        editingId = null;
        lblFormMode.Text = "New vendor";
        foreach (var t in new[] { txtVendorName, txtVendorContact, txtVendorPhone, txtVendorEmail, txtVendorAddress,
                                  txtVendorBank, txtVendorAccountName, txtVendorAccountNo })
            t.Clear();
        cboVendorVat.SelectedItem = "VAT";
        dgvVendorDocs.DataSource = null;
        btnUploadVendorDoc.Enabled = btnViewVendorDoc.Enabled = btnDeleteVendorDoc.Enabled = false;
        lblVendorMsg.Text = "";
    }

    async Task StartEditAsync(Vendor v)
    {
        editingId = v.VendorId;
        lblFormMode.Text = $"Editing: {v.Name}";
        txtVendorName.Text = v.Name;
        txtVendorContact.Text = v.ContactPerson ?? "";
        txtVendorPhone.Text = v.Phone ?? "";
        txtVendorEmail.Text = v.Email ?? "";
        txtVendorAddress.Text = v.Address ?? "";
        txtVendorBank.Text = v.BankName ?? "";
        txtVendorAccountName.Text = v.AccountName ?? "";
        txtVendorAccountNo.Text = v.AccountNumber ?? "";
        cboVendorVat.SelectedItem = v.VatStatus;
        lblVendorMsg.Text = "";
        await LoadDocsAsync();
    }

    async Task LoadDocsAsync()
    {
        dgvVendorDocs.DataSource = editingId is { } id ? await PurchasingService.ListDocumentsAsync(id) : null;
        btnUploadVendorDoc.Enabled = editingId != null;
        btnViewVendorDoc.Enabled = btnDeleteVendorDoc.Enabled = dgvVendorDocs.Rows.Count > 0;
    }

    VendorInput ReadForm() => new(txtVendorName.Text, txtVendorContact.Text, txtVendorPhone.Text, txtVendorEmail.Text,
        txtVendorAddress.Text, txtVendorBank.Text, txtVendorAccountName.Text, txtVendorAccountNo.Text, (string?)cboVendorVat.SelectedItem ?? "");

    void btnAddVendor_Click(object sender, EventArgs e) { dgvVendors.ClearSelection(); StartNew(); txtVendorName.Focus(); }

    async void btnCancelVendor_Click(object sender, EventArgs e)
    {
        if (editingId is { } id && await PurchasingService.GetVendorAsync(id) is { } v) await StartEditAsync(v); else StartNew();
    }

    async void btnSaveVendor_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (PurchasingService.CheckVendor(input) is { } problem) { lblVendorMsg.Text = problem; return; }
        btnSaveVendor.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await PurchasingService.UpdateVendorAsync(id, input);
                lblVendorMsg.Text = error ?? "Saved.";
                if (error == null) { await LoadAsync(id); Changed?.Invoke(this, EventArgs.Empty); }
            }
            else
            {
                var (newId, error) = await PurchasingService.CreateVendorAsync(input);
                lblVendorMsg.Text = error ?? "Vendor added. You can upload their papers now.";
                if (newId != null) { await LoadAsync(newId); Changed?.Invoke(this, EventArgs.Empty); }
            }
        }
        catch (Exception ex) { lblVendorMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveVendor.Enabled = true; }
    }

    async void btnDeleteVendor_Click(object sender, EventArgs e)
    {
        if (Selected is not { } v) return;
        if (MessageBox.Show(this, $"Delete \"{v.Name}\"?", "Delete vendor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        var error = await PurchasingService.DeleteVendorAsync(v.VendorId);
        if (error != null) { MessageBox.Show(this, error, "Not deleted", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        StartNew();
        await LoadAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    async void btnUploadVendorDoc_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id) return;
        using var dlg = new OpenFileDialog { Filter = FileStore.ScanFilter, Title = "Choose the vendor paper" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        var error = await PurchasingService.UploadDocumentAsync(id, (string)cboVendorDocType.SelectedItem!, dlg.FileName);
        lblVendorMsg.Text = error ?? "Paper uploaded.";
        await LoadDocsAsync();
    }

    async void btnViewVendorDoc_Click(object sender, EventArgs e)
    {
        if (SelectedDoc is { } d && await FileStore.OpenAsync(d.FilePath) is { } error)
            MessageBox.Show(this, error, "Can't open", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    async void btnDeleteVendorDoc_Click(object sender, EventArgs e)
    {
        if (SelectedDoc is not { } d) return;
        if (MessageBox.Show(this, $"Remove \"{d.OriginalName}\" from this vendor?", "Delete paper", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        lblVendorMsg.Text = await PurchasingService.DeleteDocumentAsync(d.DocumentId) ?? "Paper removed.";
        await LoadDocsAsync();
    }

    void btnExportVendors_Click(object sender, EventArgs e)
    {
        if (dgvVendors.DataSource is not List<VendorRow> rows) return;
        if (ExcelExport.AskPath(this, "Vendors") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Vendors", rows, new("Name", r => r.Name), new("Contact Person", r => r.ContactPerson),
                new("Phone", r => r.Phone), new("VAT Status", r => r.VatStatus));
            MessageBox.Show(this, $"Saved {rows.Count} vendors to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
