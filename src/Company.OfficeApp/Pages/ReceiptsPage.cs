using Company.OfficeApp.Pages.Receipts;
using Company.OfficeApp.Pages.Shared;
using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages;

/// <summary>
/// Receipts & change orders (Admin and the Receipts department). 5 tabs; each tab's content is its own
/// UserControl in Pages/Receipts. Edit History is for Admins and the Receipts Supervisor only.
/// The Needs Review tab title shows how many no-receipt expenses are waiting: "Needs Review (2)".
/// </summary>
public partial class ReceiptsPage : UserControl
{
    readonly ReceiptVaultTab vault = new() { Dock = DockStyle.Fill };
    readonly NeedsReviewTab review = new() { Dock = DockStyle.Fill };
    readonly ChangeOrdersTab changeOrders = new() { Dock = DockStyle.Fill };
    readonly ReceiptReportsTab reports = new() { Dock = DockStyle.Fill };

    public ReceiptsPage()
    {
        InitializeComponent();
        tabReceiptVault.Controls.Add(vault);
        tabNeedsReview.Controls.Add(review);
        tabChangeOrders.Controls.Add(changeOrders);
        tabReceiptReports.Controls.Add(reports);
        tabRecEditHistory.Controls.Add(new EditHistoryTab(Departments.Receipts) { Dock = DockStyle.Fill });

        if (!Access.IsApprover(Session.CurrentUser!, Departments.Receipts))
            tabReceipts.TabPages.Remove(tabRecEditHistory);

        vault.Changed += async (_, _) => { await review.LoadAsync(); await RefreshBadgeAsync(); };
        review.Changed += async (_, _) => { await vault.LoadAsync(); await RefreshBadgeAsync(); };
    }

    async void ReceiptsPage_Load(object sender, EventArgs e) => await RefreshBadgeAsync();

    async Task RefreshBadgeAsync()
    {
        var waiting = await ReceiptService.NeedsReviewCountAsync();
        tabNeedsReview.Text = waiting == 0 ? "Needs Review" : $"Needs Review ({waiting})";
    }
}
