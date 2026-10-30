using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Hr;

/// <summary>
/// HR & Payroll > Cash Advances: the list (waiting ones first), New Cash Advance (pop-up), Approve / Reject
/// (HR Supervisor and Admins), and the deductions each payday took from the selected advance.
/// Plain layout for now; control names match the UI Elements Guide (Part 2: Tab: Cash Advances).
/// </summary>
public partial class CashAdvancesTab : UserControl
{
    readonly bool approver;
    readonly LatestOnly loads = new();
    bool filling;
    int? selectedId;

    public CashAdvancesTab()
    {
        InitializeComponent();
        dgvCashAdvances.AutoGenerateColumns = false;
        dgvCashAdvanceDeductions.AutoGenerateColumns = false;
        foreach (var c in new[] { colCaAmount, colCaPerPayday, colCaBalance, colDedAmount, colDedBalance }) c.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        cboCashAdvanceFilter.Items.AddRange(CashAdvanceService.Filters);
        cboCashAdvanceFilter.SelectedIndex = 0;
        approver = Access.IsApprover(Session.CurrentUser!, Departments.HrPayroll);
        btnApproveCashAdvance.Visible = btnRejectCashAdvance.Visible = approver;
        lblApproverNote.Visible = !approver;
        Responsive.FitGrid(this, dgvCashAdvances);
        RecordForm.NoAutoSelect(dgvCashAdvances, on => filling = on, RestoreSelection);
        UpdateButtons();
    }

    async void CashAdvancesTab_Load(object sender, EventArgs e) => await LoadAsync();

    async Task LoadAsync(int? selectId = null)
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await CashAdvanceService.ListAsync((string)cboCashAdvanceFilter.SelectedItem!);
        if (!loads.IsLatest(ticket)) return;
        filling = true;
        dgvCashAdvances.DataSource = rows;
        dgvCashAdvances.ClearSelection();
        selectedId = selectId;
        if (selectId is { } id) Responsive.SelectRow(dgvCashAdvances, (CashAdvanceRow r) => r.AdvanceId == id);
        filling = false;
        await ShowDeductionsAsync();
    }

    void RestoreSelection()
    {
        filling = true;
        dgvCashAdvances.ClearSelection();
        if (selectedId is { } id) Responsive.SelectRow(dgvCashAdvances, (CashAdvanceRow r) => r.AdvanceId == id);
        filling = false;
        UpdateButtons();
    }

    CashAdvanceRow? Selected => dgvCashAdvances.SelectedRows.Count > 0 ? dgvCashAdvances.SelectedRows[0].DataBoundItem as CashAdvanceRow : null;

    void UpdateButtons() => btnApproveCashAdvance.Enabled = btnRejectCashAdvance.Enabled = Selected is { Status: CashAdvanceStatuses.Waiting };

    async Task ShowDeductionsAsync()
    {
        UpdateButtons();
        dgvCashAdvanceDeductions.DataSource = Selected is { } r ? await CashAdvanceService.DeductionsAsync(r.AdvanceId) : null;
    }

    async void Filter_Changed(object sender, EventArgs e) => await LoadAsync();

    async void dgvCashAdvances_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        selectedId = Selected?.AdvanceId;
        await ShowDeductionsAsync();
    }

    async void btnNewCashAdvance_Click(object sender, EventArgs e)
    {
        using var f = new CashAdvanceEntryForm();
        if (f.ShowDialog(this) != DialogResult.OK) return;
        lblCaMsg.Text = "Saved. It waits for the HR Supervisor's approval.";
        await LoadAsync();
    }

    async Task DecideAsync(bool approve)
    {
        if (Selected is not { } r) return;
        var error = approve ? await CashAdvanceService.ApproveAsync(r.AdvanceId) : await CashAdvanceService.RejectAsync(r.AdvanceId);
        lblCaMsg.Text = error ?? (approve ? $"{r.Name}'s cash advance approved. It is deducted every payday from now on." : $"{r.Name}'s cash advance rejected.");
        if (error == null) await LoadAsync(r.AdvanceId);
    }

    async void btnApproveCashAdvance_Click(object sender, EventArgs e) => await DecideAsync(true);

    async void btnRejectCashAdvance_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(this, "Reject this cash advance?", "Reject", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            await DecideAsync(false);
    }
}
