using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Receipts;

/// <summary>
/// Receipts > Receipt Vault: every receipt with a copy (scan or paper), filterable by project, dates, category,
/// status and keyword, plus Inventory's delivery receipts marked "Stock purchase" (read only here).
/// Several rows can be selected to approve, reject or print together.
/// Plain layout for now; control names match the UI Elements Guide (Part 5: Tab: Receipt Vault).
/// </summary>
public partial class ReceiptVaultTab : UserControl
{
    readonly bool approver;
    readonly LatestOnly loads = new();
    bool ready;

    /// <summary>Raised after receipts change (the Needs Review count may change too).</summary>
    public event EventHandler? Changed;

    public ReceiptVaultTab()
    {
        InitializeComponent();
        dgvReceipts.AutoGenerateColumns = false;
        colAmount.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        dtpReceiptFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-2);
        dtpReceiptTo.Value = DateTime.Today;
        cboReceiptCategoryFilter.Items.Add("All");
        cboReceiptCategoryFilter.Items.AddRange(ReceiptLists.Categories);
        cboReceiptCategoryFilter.SelectedIndex = 0;
        cboReceiptStatusFilter.Items.AddRange(["All", ApprovalStatuses.Waiting, ApprovalStatuses.Approved, ApprovalStatuses.Rejected]);
        cboReceiptStatusFilter.SelectedIndex = 0;
        approver = Access.IsApprover(Session.CurrentUser!, Departments.Receipts);
        btnApproveReceipt.Visible = btnRejectReceipt.Visible = approver;
        lblApproverNote.Visible = !approver;
        Responsive.FitGrid(this, dgvReceipts);
        Responsive.OnTypingPause(txtReceiptSearch, () => LoadAsync());
    }

    async void ReceiptVaultTab_Load(object sender, EventArgs e)
    {
        await ReloadProjectsAsync();
        ready = true;
        await LoadAsync();
    }

    /// <summary>Refills the project filter (All, Company Overhead, each project), keeping the choice.</summary>
    public async Task ReloadProjectsAsync()
    {
        var keep = cboReceiptProject.SelectedItem;
        var projects = await MaterialService.ProjectOptionsAsync();
        cboReceiptProject.Items.Clear();
        cboReceiptProject.Items.AddRange(["All", ReceiptService.OverheadLabel, .. projects]);
        cboReceiptProject.SelectedItem = keep is ProjectOption p ? projects.FirstOrDefault(x => x.ProjectId == p.ProjectId) : keep;
        if (cboReceiptProject.SelectedIndex < 0) cboReceiptProject.SelectedIndex = 0;
    }

    ReceiptFilter CurrentFilter() => new(
        cboReceiptProject.SelectedItem switch { ProjectOption p => p.ProjectId, ReceiptService.OverheadLabel => ReceiptFilter.OverheadOnly, _ => null },
        DateOnly.FromDateTime(dtpReceiptFrom.Value), DateOnly.FromDateTime(dtpReceiptTo.Value),
        cboReceiptCategoryFilter.SelectedIndex > 0 ? (string)cboReceiptCategoryFilter.SelectedItem! : null,
        cboReceiptStatusFilter.SelectedIndex > 0 ? (string)cboReceiptStatusFilter.SelectedItem! : null,
        txtReceiptSearch.Text);

    public async Task LoadAsync()
    {
        if (!ready) return;
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await ReceiptService.ListAsync(CurrentFilter());
        if (!loads.IsLatest(ticket)) return;
        dgvReceipts.DataSource = rows;
        dgvReceipts.ClearSelection();
        var counted = rows.Where(r => !r.IsStockPurchase && r.Status == ApprovalStatuses.Approved).Sum(r => r.Amount);
        lblReceiptsTotal.Text = $"{rows.Count} shown, total {ProjectService.Money(rows.Sum(r => r.Amount))}  (approved expenses, not counting stock purchases: {ProjectService.Money(counted)})";
        dgvReceipts_SelectionChanged(this, EventArgs.Empty);
    }

    List<ReceiptRow> SelectedRows => dgvReceipts.SelectedRows.Cast<DataGridViewRow>().Select(r => (ReceiptRow)r.DataBoundItem!).ToList();

    async void Filter_Changed(object sender, EventArgs e) => await LoadAsync();

    void dgvReceipts_SelectionChanged(object? sender, EventArgs e)
    {
        var sel = SelectedRows;
        btnOpenReceipt.Enabled = sel.Count == 1;
        btnEditReceipt.Enabled = sel is [{ IsStockPurchase: false, Status: not ApprovalStatuses.Approved }];
        btnApproveReceipt.Enabled = btnRejectReceipt.Enabled = sel.Count > 0 && sel.All(r => !r.IsStockPurchase && r.Status == ApprovalStatuses.Waiting);
        btnPrintSelectedReceipts.Enabled = sel.Count > 0;
    }

    void dgvReceipts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnOpenReceipt_Click(this, EventArgs.Empty);
    }

    async void btnAddReceipt_Click(object sender, EventArgs e)
    {
        using var f = new ReceiptEntryForm();
        if (f.ShowDialog(this) != DialogResult.OK) return;
        lblVaultMsg.Text = "Receipt saved. It waits for approval.";
        await LoadAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    void btnOpenReceipt_Click(object sender, EventArgs e)
    {
        if (SelectedRows is not [var r]) return;
        using var f = new ReceiptViewerForm(r);
        f.ShowDialog(this);
    }

    async void btnEditReceipt_Click(object sender, EventArgs e)
    {
        if (SelectedRows is not [{ IsStockPurchase: false } r]) return;
        using var f = new ReceiptEntryForm(r.ReceiptId);
        if (f.ShowDialog(this) != DialogResult.OK) return;
        lblVaultMsg.Text = "Receipt saved. It waits for approval again.";
        await LoadAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    async Task DecideAsync(bool approve)
    {
        var ids = SelectedRows.Select(r => r.ReceiptId).ToList();
        var error = approve ? await ReceiptService.ApproveAsync(ids) : await ReceiptService.RejectAsync(ids);
        lblVaultMsg.Text = error ?? $"{ids.Count} receipt{(ids.Count == 1 ? "" : "s")} {(approve ? "approved" : "rejected")}.";
        if (error == null) { await LoadAsync(); Changed?.Invoke(this, EventArgs.Empty); }
    }

    async void btnApproveReceipt_Click(object sender, EventArgs e) => await DecideAsync(true);

    async void btnRejectReceipt_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(this, $"Reject {SelectedRows.Count} receipt(s)?", "Reject", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            await DecideAsync(false);
    }

    async void btnPrintSelectedReceipts_Click(object sender, EventArgs e)
    {
        var sheets = new List<(ReceiptView, string?)>();
        foreach (var r in SelectedRows.OrderBy(r => r.Date))
        {
            var v = await ReceiptService.ViewAsync(r);
            sheets.Add((v, v.FilePath == null ? null : await FileStore.FullPathAsync(v.FilePath)));
        }
        using var print = new ReceiptSheetPrint(sheets);
        using var preview = new PrintPreviewDialog { Document = print.Document, Width = 900, Height = 700 };
        preview.ShowDialog(this);
    }

    void btnExportReceipts_Click(object sender, EventArgs e)
    {
        if (dgvReceipts.DataSource is not List<ReceiptRow> rows) return;
        if (ExcelExport.AskPath(this, "Receipts") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Receipts", rows, new("Receipt No.", r => r.ReceiptNo), new("Date", r => r.Date, "mmm d, yyyy"),
                new("Merchant", r => r.Merchant), new("Amount", r => r.Amount, "#,##0.00"), new("Category", r => r.Category),
                new("Project / Overhead", r => r.ProjectOrOverhead), new("Copy", r => r.Copy), new("Stock Purchase", r => r.StockPurchase),
                new("Status", r => r.Status), new("Uploaded By", r => r.UploadedBy));
            MessageBox.Show(this, $"Saved {rows.Count} receipts to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
