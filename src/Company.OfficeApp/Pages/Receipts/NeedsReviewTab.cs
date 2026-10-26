using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Receipts;

/// <summary>
/// Receipts > Needs Review: expenses with no receipt (they need a reason). The Receipts Supervisor (or an Admin)
/// approves or rejects them; a receipt that turns up later can be attached, which clears the flag.
/// Plain layout for now; control names match the UI Elements Guide (Part 5: Tab: Needs Review).
/// </summary>
public partial class NeedsReviewTab : UserControl
{
    readonly bool approver;
    readonly LatestOnly loads = new();

    public event EventHandler? Changed;

    public NeedsReviewTab()
    {
        InitializeComponent();
        dgvNeedsReview.AutoGenerateColumns = false;
        colAmount.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        approver = Access.IsApprover(Session.CurrentUser!, Departments.Receipts);
        btnApproveNoReceipt.Visible = btnRejectNoReceipt.Visible = approver;
        lblApproverNote.Visible = !approver;
        Responsive.FitGrid(this, dgvNeedsReview);
    }

    async void NeedsReviewTab_Load(object sender, EventArgs e) => await LoadAsync();

    public async Task LoadAsync()
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await ReceiptService.NeedsReviewAsync();
        if (!loads.IsLatest(ticket)) return;
        dgvNeedsReview.DataSource = rows;
        dgvNeedsReview.ClearSelection();
        dgvNeedsReview_SelectionChanged(this, EventArgs.Empty);
    }

    ReceiptRow? Selected => dgvNeedsReview.SelectedRows.Count > 0 ? dgvNeedsReview.SelectedRows[0].DataBoundItem as ReceiptRow : null;

    void dgvNeedsReview_SelectionChanged(object? sender, EventArgs e)
    {
        btnApproveNoReceipt.Enabled = btnRejectNoReceipt.Enabled = Selected is { Status: ApprovalStatuses.Waiting };
        btnAttachLateReceipt.Enabled = Selected is { Status: not ApprovalStatuses.Rejected };
    }

    async Task DecideAsync(bool approve)
    {
        if (Selected is not { } r) return;
        var error = approve ? await ReceiptService.ApproveAsync([r.ReceiptId]) : await ReceiptService.RejectAsync([r.ReceiptId]);
        lblReviewMsg.Text = error ?? (approve ? "Approved. It now counts as an expense." : "Rejected.");
        if (error == null) { await LoadAsync(); Changed?.Invoke(this, EventArgs.Empty); }
    }

    async void btnApproveNoReceipt_Click(object sender, EventArgs e) => await DecideAsync(true);

    async void btnRejectNoReceipt_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(this, "Reject this expense?", "Reject", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            await DecideAsync(false);
    }

    async void btnAttachLateReceipt_Click(object sender, EventArgs e)
    {
        if (Selected is not { } r) return;
        using var f = new ReceiptEntryForm(r.ReceiptId, lateReceipt: true);
        if (f.ShowDialog(this) != DialogResult.OK) return;
        lblReviewMsg.Text = "Receipt attached. The expense moved to the Receipt Vault.";
        await LoadAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
