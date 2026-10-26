using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Inventory;

/// <summary>
/// Inventory > Price Comparison: quotes from several vendors for the same material, the cheapest highlighted;
/// the Supervisor (or an Admin) approves one, which becomes the approved purchase. Approved purchases are
/// listed below with how much has been delivered.
/// Plain layout for now; control names match the UI Elements Guide (Part 3: Tab: Price Comparison).
/// </summary>
public partial class PriceComparisonTab : UserControl
{
    /// <summary>Raised after a purchase is approved (the Deliveries tab lists purchases).</summary>
    public event EventHandler? Changed;

    public PriceComparisonTab()
    {
        InitializeComponent();
        dgvQuotes.AutoGenerateColumns = false;
        dgvPurchases.AutoGenerateColumns = false;
        foreach (var c in new[] { colQUnitPrice, colQTotal, colPTotal }) c.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        cboQuoteCategory.Items.AddRange(InventoryLists.MaterialCategories);
        dtpQuoteDate.Value = DateTime.Today;
        bool approver = Access.IsApprover(Session.CurrentUser!, Departments.Inventory);
        btnApprovePurchase.Visible = approver;
        lblApproverNote.Visible = !approver;
        Responsive.FitGrid(this, dgvPurchases);
    }

    async void PriceComparisonTab_Load(object sender, EventArgs e) => await ReloadListsAsync();

    /// <summary>Refreshes the material and vendor dropdowns and both tables.</summary>
    public async Task ReloadListsAsync()
    {
        var keepMaterial = (cboCompareMaterial.SelectedItem as MaterialRow)?.MaterialId;
        var materials = await MaterialService.ListAsync();
        cboCompareMaterial.Items.Clear();
        cboCompareMaterial.Items.AddRange([.. materials]);
        cboCompareMaterial.SelectedItem = materials.FirstOrDefault(m => m.MaterialId == keepMaterial);

        var keepVendor = (cboQuoteVendor.SelectedItem as VendorRow)?.VendorId;
        var vendors = await PurchasingService.ListVendorsAsync();
        cboQuoteVendor.Items.Clear();
        cboQuoteVendor.Items.AddRange([.. vendors]);
        cboQuoteVendor.SelectedItem = vendors.FirstOrDefault(v => v.VendorId == keepVendor);

        await LoadQuotesAsync();
        dgvPurchases.DataSource = await PurchasingService.ListPurchasesAsync();
    }

    async void cboCompareMaterial_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboCompareMaterial.SelectedItem is MaterialRow m) { cboQuoteCategory.Text = m.Category; txtQuoteProduct.Text = m.Name; }
        await LoadQuotesAsync();
    }

    async Task LoadQuotesAsync()
    {
        if (cboCompareMaterial.SelectedItem is not MaterialRow m)
        {
            dgvQuotes.DataSource = null;
            lblCheapestQuote.Text = "Choose a material to see its quotes.";
            UpdateButtons();
            return;
        }
        var quotes = await PurchasingService.ListQuotesAsync(m.MaterialId);
        dgvQuotes.DataSource = quotes;
        var cheapest = quotes.Where(q => q.Status == "Open").MinBy(q => q.UnitPrice);
        lblCheapestQuote.Text = cheapest == null ? "No open quotes for this material yet."
            : $"Cheapest open quote: {cheapest.Vendor}, {ProjectService.Money(cheapest.UnitPrice)} per {m.Unit}";
        UpdateButtons();
    }

    QuoteRow? Selected => dgvQuotes.SelectedRows.Count > 0 ? dgvQuotes.SelectedRows[0].DataBoundItem as QuoteRow : null;

    void UpdateButtons() => btnDeleteQuote.Enabled = btnApprovePurchase.Enabled = Selected?.Status == "Open";

    void dgvQuotes_SelectionChanged(object sender, EventArgs e) => UpdateButtons();

    // The cheapest open quote is highlighted (a function, not styling).
    void dgvQuotes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.CellStyle == null || dgvQuotes.DataSource is not List<QuoteRow> all) return;
        var row = (QuoteRow)dgvQuotes.Rows[e.RowIndex].DataBoundItem!;
        var cheapest = all.Where(q => q.Status == "Open").MinBy(q => q.UnitPrice);
        if (cheapest != null && row.QuoteId == cheapest.QuoteId) e.CellStyle.BackColor = Color.Honeydew;
    }

    async void btnAddQuote_Click(object sender, EventArgs e)
    {
        var input = new QuoteInput((cboCompareMaterial.SelectedItem as MaterialRow)?.MaterialId ?? 0,
            (cboQuoteVendor.SelectedItem as VendorRow)?.VendorId ?? 0, DateOnly.FromDateTime(dtpQuoteDate.Value),
            txtQuoteProduct.Text, txtQuoteDescription.Text, cboQuoteCategory.Text, numQuoteUnitPrice.Value, numQuoteQty.Value);
        if (PurchasingService.CheckQuote(input) is { } problem) { lblQuoteMsg.Text = problem; return; }
        var (_, error) = await PurchasingService.AddQuoteAsync(input);
        lblQuoteMsg.Text = error ?? "Quote added.";
        if (error == null) { numQuoteUnitPrice.Value = 0; txtQuoteDescription.Clear(); await LoadQuotesAsync(); }
    }

    async void btnDeleteQuote_Click(object sender, EventArgs e)
    {
        if (Selected is not { } q) return;
        lblQuoteMsg.Text = await PurchasingService.DeleteQuoteAsync(q.QuoteId) ?? "Quote deleted.";
        await LoadQuotesAsync();
    }

    async void btnApprovePurchase_Click(object sender, EventArgs e)
    {
        if (Selected is not { } q) return;
        var ok = MessageBox.Show(this, $"Approve the quote from {q.Vendor} ({q.Quantity:#,##0.##} x {ProjectService.Money(q.UnitPrice)} = {ProjectService.Money(q.Total)})?\n\nThe other open quotes for this material will be closed.",
            "Approve purchase", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (ok != DialogResult.Yes) return;
        var (_, error) = await PurchasingService.ApproveQuoteAsync(q.QuoteId);
        lblQuoteMsg.Text = error ?? "Approved. Record the delivery in the Deliveries tab when it arrives.";
        await ReloadListsAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
