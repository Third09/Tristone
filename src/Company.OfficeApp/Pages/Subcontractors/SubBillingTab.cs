using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Subcontractors;

/// <summary>
/// Subcontractors > Billing & Payouts: for one job, mark stages reached, prepare vouchers (Employee), approve or
/// reject them (Supervisor / Admin), print them, record payouts, and release the retention once the job is done.
/// Plain layout for now; control names match the UI Elements Guide (Part 4: Tab: Billing & Payouts).
/// </summary>
public partial class SubBillingTab : UserControl
{
    readonly bool approver;
    readonly LatestOnly loads = new();
    JobBilling? billing;

    /// <summary>Raised after a payout, so the Jobs tab can refresh the job status.</summary>
    public event EventHandler? Changed;

    public SubBillingTab()
    {
        InitializeComponent();
        dgvBillingStages.AutoGenerateColumns = false;
        dgvPayouts.AutoGenerateColumns = false;
        foreach (var c in new[] { colAmount, colRetention, colPayable, colPayAmount }) c.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        cboPayoutMethod.Items.AddRange(SubLists.PayoutMethods);
        cboPayoutMethod.SelectedIndex = 0;
        approver = Access.IsApprover(Session.CurrentUser!, Departments.Subcontractors);
        btnApproveVoucher.Visible = btnRejectVoucher.Visible = approver;
        lblApproverNote.Visible = !approver;
        Responsive.FitGrid(this, dgvBillingStages);
        Responsive.FitGrid(this, dgvPayouts);
        ShowBilling(null);
    }

    async void SubBillingTab_Load(object sender, EventArgs e) => await ReloadJobsAsync();

    /// <summary>Refills the job list (after the Jobs tab changes), keeping the chosen job.</summary>
    public async Task ReloadJobsAsync()
    {
        var keep = (cboBillingJob.SelectedItem as JobRow)?.JobId;
        var jobs = await SubJobService.ListAsync();
        cboBillingJob.Items.Clear();
        cboBillingJob.Items.AddRange([.. jobs]);
        cboBillingJob.SelectedItem = jobs.FirstOrDefault(j => j.JobId == keep);
        if (cboBillingJob.SelectedItem == null) ShowBilling(null);
    }

    int? JobId => (cboBillingJob.SelectedItem as JobRow)?.JobId;

    async void cboBillingJob_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblBillMsg.Text = "";
        await LoadAsync();
    }

    async Task LoadAsync(int? selectStageId = null)
    {
        if (JobId is not { } id) { ShowBilling(null); return; }
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var b = await SubBillingService.GetAsync(id);
        if (!loads.IsLatest(ticket)) return;
        ShowBilling(b);
        if (selectStageId is { } sid)
            foreach (DataGridViewRow r in dgvBillingStages.Rows)
                if (((StageBillRow)r.DataBoundItem!).StageId == sid) { r.Selected = true; dgvBillingStages.CurrentCell = r.Cells[0]; }
    }

    void ShowBilling(JobBilling? b)
    {
        billing = b;
        string M(decimal? v) => v is { } d ? ProjectService.Money(d) : "-";
        lblBillContract.Text = $"Contract: {M(b?.ContractAmount)}";
        lblBillPaid.Text = $"Paid so far: {M(b?.Paid)}";
        lblBillRetention.Text = $"Retention held: {M(b?.RetentionHeld)}";
        lblBillRemaining.Text = $"Remaining: {M(b?.Remaining)}";
        lblBillProgress.Text = b == null ? "Done: -" : $"Done: {b.PercentDone:0.##}%";
        var rows = b == null ? [] : b.Stages.ToList();
        if (b?.RetentionRow is { } r) rows.Add(r);
        dgvBillingStages.DataSource = rows;
        dgvPayouts.DataSource = b?.Payouts;
        btnReleaseRetention.Enabled = b != null;
        dgvBillingStages_SelectionChanged(this, EventArgs.Empty);
    }

    StageBillRow? Selected => dgvBillingStages.SelectedRows.Count > 0 ? dgvBillingStages.SelectedRows[0].DataBoundItem as StageBillRow : null;

    void dgvBillingStages_SelectionChanged(object? sender, EventArgs e)
    {
        var s = Selected;
        btnMarkStageReached.Enabled = s is { IsRetention: false, Status: SubBillingService.NotReached };
        btnPrepareVoucher.Enabled = s is { IsRetention: false, Status: SubBillingService.Reached };
        btnApproveVoucher.Enabled = btnRejectVoucher.Enabled = s is { Status: SubBillingService.VoucherWaiting };
        btnPrintVoucher.Enabled = s?.VoucherId != null;
        btnRecordPayout.Enabled = s is { Status: VoucherStatuses.Approved };
        if (s is { Status: VoucherStatuses.Approved }) numPayoutAmount.Value = s.PayableNow;
    }

    async Task RunAsync(Func<Task<string?>> action, string done, int? reselect)
    {
        try
        {
            var error = await action();
            lblBillMsg.Text = error ?? done;
            if (error == null) { await LoadAsync(reselect); Changed?.Invoke(this, EventArgs.Empty); }
        }
        catch (Exception ex) { lblBillMsg.Text = "Can't reach the database. " + ex.Message; }
    }

    async void btnMarkStageReached_Click(object sender, EventArgs e)
    {
        if (Selected is not { } s) return;
        await RunAsync(() => SubBillingService.MarkStageReachedAsync(s.StageId, DateOnly.FromDateTime(DateTime.Today)), $"{s.Name} marked as reached.", s.StageId);
    }

    async void btnPrepareVoucher_Click(object sender, EventArgs e)
    {
        if (Selected is not { } s) return;
        await RunAsync(async () => (await SubBillingService.PrepareVoucherAsync(s.StageId)).Error, "Voucher prepared. It waits for the Supervisor's approval.", s.StageId);
    }

    async void btnApproveVoucher_Click(object sender, EventArgs e)
    {
        if (Selected is not { VoucherId: { } vid } s) return;
        await RunAsync(() => SubBillingService.ApproveVoucherAsync(vid), $"{s.VoucherNo} approved.", s.StageId);
    }

    async void btnRejectVoucher_Click(object sender, EventArgs e)
    {
        if (Selected is not { VoucherId: { } vid } s) return;
        if (MessageBox.Show(this, $"Reject {s.VoucherNo}? The Employee can prepare a new one.", "Reject voucher", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        await RunAsync(() => SubBillingService.RejectVoucherAsync(vid), $"{s.VoucherNo} rejected.", s.StageId);
    }

    async void btnReleaseRetention_Click(object sender, EventArgs e)
    {
        if (JobId is not { } id) return;
        await RunAsync(async () => (await SubBillingService.PrepareRetentionVoucherAsync(id)).Error, "Retention voucher prepared. It waits for the Supervisor's approval.", 0);
    }

    async void btnRecordPayout_Click(object sender, EventArgs e)
    {
        if (Selected is not { VoucherId: { } vid } s) return;
        await RunAsync(() => SubBillingService.RecordPayoutAsync(vid, DateOnly.FromDateTime(dtpPayoutDate.Value), numPayoutAmount.Value,
            txtPayoutReference.Text, (string)cboPayoutMethod.SelectedItem!), "Payout recorded.", s.StageId);
        if (lblBillMsg.Text == "Payout recorded.") txtPayoutReference.Clear();
    }

    async void btnPrintVoucher_Click(object sender, EventArgs e)
    {
        if (Selected is not { VoucherId: { } vid }) return;
        using var voucher = new VoucherPrint(await SubBillingService.PrintDataAsync(vid));
        using var preview = new PrintPreviewDialog { Document = voucher.Document, Width = 900, Height = 700 };
        preview.ShowDialog(this);
    }
}
