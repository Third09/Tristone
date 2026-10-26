using Company.OfficeApp.Pages.Inventory;
using Company.OfficeApp.Pages.Shared;
using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages;

/// <summary>
/// Inventory & Vendors (Admin and the Inventory department). 7 tabs; each tab's content is its own UserControl
/// in Pages/Inventory. Edit History is for Admins and the Inventory Supervisor only.
/// Tab titles carry the alerts: "Materials (2 low)", "Equipment (1 due)".
/// </summary>
public partial class InventoryPage : UserControl
{
    readonly MaterialsTab materials = new() { Dock = DockStyle.Fill };
    readonly UsageTab usage = new() { Dock = DockStyle.Fill };
    readonly DeliveriesTab deliveries = new() { Dock = DockStyle.Fill };
    readonly EquipmentTab equipment = new() { Dock = DockStyle.Fill };
    readonly VendorsTab vendors = new() { Dock = DockStyle.Fill };
    readonly PriceComparisonTab prices = new() { Dock = DockStyle.Fill };

    public InventoryPage()
    {
        InitializeComponent();
        tabMaterials.Controls.Add(materials);
        tabMaterialUsage.Controls.Add(usage);
        tabDeliveries.Controls.Add(deliveries);
        tabEquipment.Controls.Add(equipment);
        tabVendors.Controls.Add(vendors);
        tabPriceComparison.Controls.Add(prices);
        tabInvEditHistory.Controls.Add(new EditHistoryTab(Departments.Inventory) { Dock = DockStyle.Fill });

        if (!Access.IsApprover(Session.CurrentUser!, Departments.Inventory))
            tabInventory.TabPages.Remove(tabInvEditHistory);

        // A change in one tab refreshes the tabs that show the same data.
        materials.Changed += async (_, _) => { await usage.ReloadListsAsync(); await prices.ReloadListsAsync(); await deliveries.ReloadListsAsync(); await RefreshAlertsAsync(); };
        usage.Changed += async (_, _) => await RefreshAlertsAsync();
        deliveries.Changed += async (_, _) => { await usage.ReloadListsAsync(); await prices.ReloadListsAsync(); await RefreshAlertsAsync(); };
        vendors.Changed += async (_, _) => { await prices.ReloadListsAsync(); await deliveries.ReloadListsAsync(); };
        prices.Changed += async (_, _) => await deliveries.ReloadListsAsync();
        equipment.Changed += async (_, _) => await RefreshAlertsAsync();
    }

    async void InventoryPage_Load(object sender, EventArgs e) => await RefreshAlertsAsync();

    async Task RefreshAlertsAsync()
    {
        var low = await MaterialService.LowStockCountAsync();
        var due = await EquipmentService.MaintenanceDueCountAsync();
        tabMaterials.Text = low == 0 ? "Materials" : $"Materials ({low} low)";
        tabEquipment.Text = due == 0 ? "Equipment" : $"Equipment ({due} due)";
    }
}
