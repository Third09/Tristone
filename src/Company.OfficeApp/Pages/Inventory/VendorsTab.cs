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
    int? editingId;          // null = nothing selected (or the "add only" pop-up)
    VendorInput? original;   // the form as loaded, to list what changed before saving
    bool filling;            // true while the list reloads, so the grid's own selection changes are ignored
    readonly bool addOnly;   // this copy is the Add Vendor pop-up (RecordForm)
    readonly LatestOnly loads = new();

    /// <summary>Raised after a vendor is added, changed or deleted (other tabs list vendors).</summary>
    public event EventHandler? Changed;

    /// <summary>The Add Vendor pop-up reports the new vendor's id here.</summary>
    public event Action<int>? Added;

    public VendorsTab() : this(false) { }

    public VendorsTab(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        dgvVendors.AutoGenerateColumns = false;
        dgvVendorDocs.AutoGenerateColumns = false;
        cboVendorVat.Items.AddRange(InventoryLists.VatStatuses);
        cboVendorDocType.Items.AddRange(InventoryLists.VendorDocTypes);
        cboVendorDocType.SelectedIndex = 0;
        if (addOnly)
        {
            // Papers are uploaded after the vendor exists, on the Vendors tab.
            RecordForm.ShowFormOnly(this, lblFormMode, lblPapers, dgvVendorDocs, cboVendorDocType, btnUploadVendorDoc, btnViewVendorDoc, btnDeleteVendorDoc);
            StartNew();
            return;
        }
        Responsive.FitGrid(this, dgvVendors);
        Responsive.OnTypingPause(txtVendorSearch, () => LoadAsync());
        ShowNothingSelected();
    }

    async void VendorsTab_Load(object sender, EventArgs e) { if (!addOnly) await LoadAsync(); }

    async Task LoadAsync(int? selectId = null)
    {
        if (addOnly) return;   // the Add pop-up has no list (its hidden filters still fire on start-up)
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await PurchasingService.ListVendorsAsync(txtVendorSearch.Text);
        if (!loads.IsLatest(ticket)) return;

        selectId ??= editingId;   // searching keeps the vendor being edited selected
        filling = true;
        dgvVendors.DataSource = rows;
        dgvVendors.ClearSelection();   // never pick the first row by itself (RecordForm)
        if (selectId is { } id)
            Responsive.SelectRow(dgvVendors, (VendorRow row) => row.VendorId == id);
        filling = false;

        if (Selected is not { } selected) ShowNothingSelected();
        else if (selected.VendorId != editingId && await PurchasingService.GetVendorAsync(selected.VendorId) is { } v) await StartEditAsync(v);
        btnDeleteVendor.Enabled = Selected != null;
    }

    VendorRow? Selected => dgvVendors.SelectedRows.Count > 0 ? dgvVendors.SelectedRows[0].DataBoundItem as VendorRow : null;
    VendorDocRow? SelectedDoc => dgvVendorDocs.SelectedRows.Count > 0 ? dgvVendorDocs.SelectedRows[0].DataBoundItem as VendorDocRow : null;

    async void dgvVendors_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        btnDeleteVendor.Enabled = Selected != null;
        if (Selected is not { } row) ShowNothingSelected();
        else if (await PurchasingService.GetVendorAsync(row.VendorId) is { } v) await StartEditAsync(v);
    }

    void ShowNothingSelected()
    {
        StartNew();
        lblFormMode.Text = "Click a vendor in the list to see or change it. To add one, click Add Vendor.";
        RecordForm.SetEnabled(this, lblFormMode, false);
    }

    void StartNew()
    {
        editingId = null;
        original = null;
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
        RecordForm.SetEnabled(this, lblFormMode, true);
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
        original = ReadForm();
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

    async void btnAddVendor_Click(object sender, EventArgs e)
    {
        var page = new VendorsTab(addOnly: true);
        if (RecordForm.AddDialog(this, "Add Vendor", page, a => page.Added += a) is not { } newId) return;
        editingId = null;
        await LoadAsync(newId);
        lblVendorMsg.Text = "Vendor added. You can upload their papers now.";
        Changed?.Invoke(this, EventArgs.Empty);
    }

    async void btnCancelVendor_Click(object sender, EventArgs e)
    {
        if (addOnly) { FindForm()?.Close(); return; }
        if (editingId is { } id && await PurchasingService.GetVendorAsync(id) is { } v) await StartEditAsync(v);
    }

    async void btnSaveVendor_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (PurchasingService.CheckVendor(input) is { } problem) { lblVendorMsg.Text = problem; return; }
        if (!addOnly && editingId == null) return;   // the form is locked when nothing is selected
        if (editingId != null && original != null)
        {
            var answer = RecordForm.ConfirmSave(this, original.Name, original, input,
                new Dictionary<string, string> { ["VatStatus"] = "VAT status" }, nameof(VendorInput.AccountNumber));
            if (answer == SaveAnswer.NothingChanged) { lblVendorMsg.Text = "Nothing changed."; return; }
            if (answer == SaveAnswer.No) { lblVendorMsg.Text = "Not saved. Click Cancel to undo your changes."; return; }
        }
        btnSaveVendor.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await PurchasingService.UpdateVendorAsync(id, input);
                if (error != null) { lblVendorMsg.Text = error; return; }
                editingId = null;   // reload the form from the saved record
                await LoadAsync(id);
                lblVendorMsg.Text = "Saved.";
                Changed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                var (newId, error) = await PurchasingService.CreateVendorAsync(input);
                if (newId is { } added) Added?.Invoke(added); else lblVendorMsg.Text = error;
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
        editingId = null;
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
