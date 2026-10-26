using Company.OfficeApp.Services;
using Company.OfficeApp.UI;

namespace Company.OfficeApp.Pages.Inventory;

/// <summary>
/// Inventory > Deliveries: what arrived from a vendor, with its receipt (a scan, or "physical copy only" with
/// where the paper is filed). No approval needed; the stock goes up and the material takes the delivered price.
/// Plain layout for now; control names match the UI Elements Guide (Part 3: Tab: Deliveries).
/// </summary>
public partial class DeliveriesTab : UserControl
{
    // "No purchase" entry: deliveries that didn't go through Price Comparison.
    static readonly string NoPurchase = "(no approved purchase)";
    string? scanPath;

    /// <summary>Raised after a delivery is saved (stock changed).</summary>
    public event EventHandler? Changed;

    public DeliveriesTab()
    {
        InitializeComponent();
        dgvDeliveries.AutoGenerateColumns = false;
        colTotal.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        dtpDeliveryDate.Value = DateTime.Today;
        dtpDeliveryDate.MaxDate = DateTime.Today;
        Responsive.FitGrid(this, dgvDeliveries);
        Clear();
    }

    async void DeliveriesTab_Load(object sender, EventArgs e) => await ReloadListsAsync();

    /// <summary>Refreshes the purchase, vendor and material dropdowns and the list.</summary>
    public async Task ReloadListsAsync()
    {
        var purchases = await PurchasingService.ListPurchasesAsync(undeliveredOnly: true);
        cboDeliveryPurchase.Items.Clear();
        cboDeliveryPurchase.Items.Add(NoPurchase);
        cboDeliveryPurchase.Items.AddRange([.. purchases]);
        cboDeliveryPurchase.SelectedIndex = 0;

        cboDeliveryVendor.Items.Clear();
        cboDeliveryVendor.Items.AddRange([.. await PurchasingService.ListVendorsAsync()]);
        cboDeliveryMaterial.Items.Clear();
        cboDeliveryMaterial.Items.AddRange([.. await MaterialService.ListAsync()]);

        dgvDeliveries.DataSource = await PurchasingService.ListDeliveriesAsync();
        btnOpenDeliveryReceipt.Enabled = SelectedDelivery?.ReceiptFilePath != null;
    }

    void cboDeliveryPurchase_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool fromPurchase = cboDeliveryPurchase.SelectedItem is PurchaseRow;
        cboDeliveryVendor.Enabled = cboDeliveryMaterial.Enabled = !fromPurchase;
        if (cboDeliveryPurchase.SelectedItem is PurchaseRow p)
        {
            cboDeliveryVendor.SelectedItem = cboDeliveryVendor.Items.Cast<VendorRow>().FirstOrDefault(v => v.VendorId == p.VendorId);
            cboDeliveryMaterial.SelectedItem = cboDeliveryMaterial.Items.Cast<MaterialRow>().FirstOrDefault(m => m.MaterialId == p.MaterialId);
            numDeliveryQty.Value = Math.Max(0, p.Quantity - p.DeliveredQty);
            numDeliveryPrice.Value = p.UnitPrice;
        }
    }

    void btnAttachDeliveryReceipt_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Filter = FileStore.ScanFilter, Title = "Choose the receipt scan or photo" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        if (FileStore.Check(dlg.FileName) is { } problem) { lblDeliveryMsg.Text = problem; return; }
        scanPath = dlg.FileName;
        lblDeliveryReceiptFile.Text = Path.GetFileName(scanPath);
        chkDeliveryPhysicalOnly.Checked = false;
    }

    void chkDeliveryPhysicalOnly_CheckedChanged(object sender, EventArgs e)
    {
        txtDeliveryFiledAt.Enabled = lblDeliveryFiledAt.Enabled = chkDeliveryPhysicalOnly.Checked;
        btnAttachDeliveryReceipt.Enabled = !chkDeliveryPhysicalOnly.Checked;
        if (chkDeliveryPhysicalOnly.Checked) { scanPath = null; lblDeliveryReceiptFile.Text = "No file attached"; }
    }

    void Clear()
    {
        if (cboDeliveryPurchase.Items.Count > 0) cboDeliveryPurchase.SelectedIndex = 0;
        cboDeliveryVendor.SelectedIndex = cboDeliveryMaterial.SelectedIndex = -1;
        numDeliveryQty.Value = numDeliveryPrice.Value = 0;
        dtpDeliveryDate.Value = DateTime.Today;
        txtDeliveryReceiptNo.Clear();
        txtDeliveryFiledAt.Clear();
        scanPath = null;
        lblDeliveryReceiptFile.Text = "No file attached";
        chkDeliveryPhysicalOnly.Checked = false;
        txtDeliveryFiledAt.Enabled = lblDeliveryFiledAt.Enabled = false;
        lblDeliveryMsg.Text = "";
    }

    void btnCancelDelivery_Click(object sender, EventArgs e) => Clear();

    async void btnSaveDelivery_Click(object sender, EventArgs e)
    {
        var input = new DeliveryInput((cboDeliveryPurchase.SelectedItem as PurchaseRow)?.PurchaseId,
            (cboDeliveryVendor.SelectedItem as VendorRow)?.VendorId ?? 0, (cboDeliveryMaterial.SelectedItem as MaterialRow)?.MaterialId ?? 0,
            numDeliveryQty.Value, numDeliveryPrice.Value, DateOnly.FromDateTime(dtpDeliveryDate.Value),
            txtDeliveryReceiptNo.Text, scanPath, chkDeliveryPhysicalOnly.Checked, txtDeliveryFiledAt.Text);
        if (PurchasingService.CheckDelivery(input) is { } problem) { lblDeliveryMsg.Text = problem; return; }
        btnSaveDelivery.Enabled = false;
        try
        {
            var (_, error) = await PurchasingService.RecordDeliveryAsync(input);
            if (error != null) { lblDeliveryMsg.Text = error; return; }
            Clear();
            lblDeliveryMsg.Text = "Delivery saved. Stock updated.";
            await ReloadListsAsync();
            Changed?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex) { lblDeliveryMsg.Text = "Couldn't save: " + ex.Message; }
        finally { btnSaveDelivery.Enabled = true; }
    }

    DeliveryRow? SelectedDelivery => dgvDeliveries.SelectedRows.Count > 0 ? dgvDeliveries.SelectedRows[0].DataBoundItem as DeliveryRow : null;

    void dgvDeliveries_SelectionChanged(object sender, EventArgs e) => btnOpenDeliveryReceipt.Enabled = SelectedDelivery?.ReceiptFilePath != null;

    async void btnOpenDeliveryReceipt_Click(object sender, EventArgs e)
    {
        if (SelectedDelivery?.ReceiptFilePath is { } path && await FileStore.OpenAsync(path) is { } error)
            MessageBox.Show(this, error, "Can't open", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
