using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Inventory;

/// <summary>
/// Inventory > Price Comparison: quotes from several vendors for the same material, the cheapest highlighted;
/// the Supervisor approves one, which becomes the approved purchase and adds the stock (no Deliveries tab
/// since 2026-11-09). Vendors that list the material on their "Materials supplied" show above the quotes,
/// and picking a vendor fills in their listed price.
/// Plain layout for now; control names match the UI Elements Guide (Part 3: Tab: Price Comparison).
/// </summary>
public partial class PriceComparisonTab : UserControl
{
    /// <summary>Raised after a purchase is approved (stock changed, so Materials and Usage refresh).</summary>
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
        await FillListedPriceAsync();
    }

    async void cboQuoteVendor_SelectionChangeCommitted(object? sender, EventArgs e) => await FillListedPriceAsync();

    List<(int VendorId, string Vendor, decimal Price)> suppliers = [];

    /// <summary>When the chosen vendor lists the chosen material, its price goes into Unit price.</summary>
    async Task FillListedPriceAsync()
    {
        if (cboCompareMaterial.SelectedItem is not MaterialRow m || cboQuoteVendor.SelectedItem is not VendorRow v) return;
        if (suppliers.Count == 0) suppliers = await PurchasingService.SuppliersOfAsync(m.MaterialId);
        var listed = suppliers.FirstOrDefault(s => s.VendorId == v.VendorId);
        if (listed.Vendor == null) return;
        numQuoteUnitPrice.Value = Math.Min(listed.Price, numQuoteUnitPrice.Maximum);
        lblQuoteMsg.Text = $"Unit price filled in from {v.Name}'s list.";
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
        suppliers = await PurchasingService.SuppliersOfAsync(m.MaterialId);
        dgvQuotes.DataSource = quotes;
        var cheapest = quotes.Where(q => q.Status == "Open").MinBy(q => q.UnitPrice);
        var text = cheapest == null ? "No open quotes for this material yet."
            : $"Cheapest open quote: {cheapest.Vendor}, {ProjectService.Money(cheapest.UnitPrice)} per {m.Unit}";
        if (suppliers.Count > 0)
            text += "   |   Listed by: " + string.Join(", ", suppliers.Take(4).Select(s => $"{s.Vendor} {ProjectService.Money(s.Price)}"))
                  + (suppliers.Count > 4 ? $" (+{suppliers.Count - 4} more)" : "");
        lblCheapestQuote.Text = text;
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
        var ok = MessageBox.Show(this, $"Approve the quote from {q.Vendor} ({q.Quantity:#,##0.##} x {ProjectService.Money(q.UnitPrice)} = {ProjectService.Money(q.Total)})?\n\nThe stock goes up by {q.Quantity:#,##0.##} now, and the other open quotes for this material will be closed.",
            "Approve purchase", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (ok != DialogResult.Yes) return;
        var (_, error) = await PurchasingService.ApproveQuoteAsync(q.QuoteId);
        lblQuoteMsg.Text = error ?? $"Approved. {q.Quantity:#,##0.##} added to the stock.";
        await ReloadListsAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
