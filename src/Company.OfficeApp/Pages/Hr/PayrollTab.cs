using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Hr;

/// <summary>
/// HR & Payroll > Payroll: choose the pay group and period, see what still needs fixing, Compute (a draft that can be
/// computed again), Finalize (HR Supervisor / Admin; locks it and takes the cash advances), then print or save the
/// payslips as one PDF, or export the table.
/// Plain layout for now; control names match the UI Elements Guide (Part 2: Tab: Payroll).
/// </summary>
public partial class PayrollTab : UserControl
{
    readonly bool approver;
    readonly LatestOnly loads = new();
    PayrollView? view;
    bool filling;

    public PayrollTab()
    {
        InitializeComponent();
        dgvPayroll.AutoGenerateColumns = false;
        foreach (var c in new[] { colPayGross, colPayLate, colPayOt, colPayCa, colPayGov, colPayNet }) c.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        approver = Access.IsApprover(Session.CurrentUser!, Departments.HrPayroll);
        btnFinalizePayroll.Visible = approver;
        lblApproverNote.Visible = !approver;
        Responsive.FitGrid(this, dgvPayroll);
        RecordForm.NoAutoSelect(dgvPayroll, on => filling = on, () => { filling = true; dgvPayroll.ClearSelection(); filling = false; });
        filling = true;
        cboPayGroup.Items.AddRange([$"{PaySchedules.Office} (15th and 30th)", $"{PaySchedules.Site} (weekly, Saturday)"]);
        cboPayGroup.SelectedIndex = 0;
        filling = false;
        UpdateButtons();
    }

    async void PayrollTab_Load(object sender, EventArgs e) => await LoadPeriodsAsync();

    string Group => cboPayGroup.SelectedIndex == 1 ? PaySchedules.Site : PaySchedules.Office;

    async Task LoadPeriodsAsync()
    {
        var periods = await PayrollService.PeriodsAsync(Group);
        var today = DateOnly.FromDateTime(DateTime.Today);
        filling = true;
        cboPayPeriod.Items.Clear();
        cboPayPeriod.Items.AddRange([.. periods]);
        // Start on the most recent period that has ended (the one to pay next).
        cboPayPeriod.SelectedItem = periods.FirstOrDefault(p => p.To < today) ?? periods.FirstOrDefault();
        filling = false;
        await LoadAsync();
    }

    async void cboPayGroup_SelectedIndexChanged(object sender, EventArgs e) { if (!filling) await LoadPeriodsAsync(); }

    async void cboPayPeriod_SelectedIndexChanged(object sender, EventArgs e) { if (!filling) await LoadAsync(); }

    async Task LoadAsync()
    {
        if (cboPayPeriod.SelectedItem is not PayPeriod period) return;
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var v = await PayrollService.GetAsync(period);
        if (!loads.IsLatest(ticket)) return;
        view = v;
        filling = true;
        dgvPayroll.DataSource = v.Rows;
        dgvPayroll.ClearSelection();
        filling = false;
        lblPayrollStatus.Text = v.Run == null ? "Not computed yet" : v.Run.Status == PayrollStatuses.Final ? $"Final ({v.Run.FinalizedAt:MMM d, h:mm tt})" : $"Draft (computed {v.Run.ComputedAt:MMM d, h:mm tt})";
        lblPayrollWarnings.Text = v.Warnings.Count == 0 ? "Ready: no missing scans or overtime waiting." : string.Join("\n", v.Warnings);
        lblPayrollTotal.Text = v.Rows.Count == 0 ? "" : $"{v.Rows.Count} payslip{(v.Rows.Count == 1 ? "" : "s")}, total net pay {ProjectService.Money(v.TotalNet)}";
        UpdateButtons();
    }

    void UpdateButtons()
    {
        bool final = view?.Run?.Status == PayrollStatuses.Final;
        bool has = view?.Rows.Count > 0;
        btnComputePayroll.Enabled = view != null && !final;
        btnFinalizePayroll.Enabled = has && !final && view?.Blocked == false;
        btnPrintPayslips.Enabled = btnSavePayslipsPdf.Enabled = btnExportPayroll.Enabled = has;
    }

    async void btnComputePayroll_Click(object sender, EventArgs e)
    {
        if (view == null) return;
        btnComputePayroll.Enabled = false;
        try
        {
            using (Responsive.Busy(this)) lblPayrollMsg.Text = await PayrollService.ComputeAsync(view.Period) ?? "Computed (draft). Check it, then Finalize.";
            await LoadAsync();
        }
        catch (Exception ex) { lblPayrollMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { UpdateButtons(); }
    }

    async void btnFinalizePayroll_Click(object sender, EventArgs e)
    {
        if (view == null) return;
        if (MessageBox.Show(this, $"Finalize the {view.Period.Group.ToLower()} payroll for {view.Period}?\n\nIt is computed once more, then locked: " +
                "cash advances are deducted, the payslips become final, and these attendance days can't change any more.",
                "Finalize payroll", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        try
        {
            using (Responsive.Busy(this)) lblPayrollMsg.Text = await PayrollService.FinalizeAsync(view.Period) ?? "Finalized. The payslips are ready to print.";
            await LoadAsync();
        }
        catch (Exception ex) { lblPayrollMsg.Text = "Can't reach the database. " + ex.Message; }
    }

    async Task<PayslipPrint?> PayslipsAsync()
    {
        if (view == null || view.Rows.Count == 0) return null;
        return new PayslipPrint(await PayrollService.PayslipsAsync([.. view.Rows.Select(r => r.PayslipId)]));
    }

    async void dgvPayroll_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || dgvPayroll.Rows[e.RowIndex].DataBoundItem is not PayrollRow r) return;
        using var slip = new PayslipPrint(await PayrollService.PayslipsAsync([r.PayslipId]));
        using var preview = new PrintPreviewDialog { Document = slip.Document, Width = 900, Height = 700 };
        preview.ShowDialog(this);
    }

    async void btnPrintPayslips_Click(object sender, EventArgs e)
    {
        using var slips = await PayslipsAsync();
        if (slips == null) return;
        using var preview = new PrintPreviewDialog { Document = slips.Document, Width = 900, Height = 700 };
        preview.ShowDialog(this);   // the preview's printer button prints them
    }

    async void btnSavePayslipsPdf_Click(object sender, EventArgs e)
    {
        if (view == null) return;
        using var dlg = new SaveFileDialog { Filter = "PDF file (*.pdf)|*.pdf", FileName = $"Payslips {view.Period.Group} {view.Period.PayDate:yyyy-MM-dd}.pdf" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            using var slips = await PayslipsAsync();
            slips?.SaveAsPdf(dlg.FileName);
            lblPayrollMsg.Text = $"Saved: {dlg.FileName}";
        }
        catch (Exception ex) { lblPayrollMsg.Text = "Couldn't save the PDF (is \"Microsoft Print to PDF\" installed?). " + ex.Message; }
    }

    void btnExportPayroll_Click(object sender, EventArgs e)
    {
        if (view == null || view.Rows.Count == 0) return;
        if (ExcelExport.AskPath(this, $"Payroll {view.Period.Group} {view.Period.PayDate:yyyy-MM-dd}") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Payroll", view.Rows, new("Name", r => r.Name), new("Days Paid", r => r.DaysPaid, "0.#"),
                new("Gross Pay", r => r.GrossPay, "#,##0.00"), new("Late Deduction", r => r.LateDeduction, "#,##0.00"),
                new("Overtime Pay", r => r.OtPay, "#,##0.00"), new("Cash Advance", r => r.CashAdvance, "#,##0.00"),
                new("Gov. Deductions", r => r.GovDeductions, "#,##0.00"), new("Net Pay", r => r.NetPay, "#,##0.00"));
            lblPayrollMsg.Text = $"Saved: {path}";
        }
        catch (IOException ex) { lblPayrollMsg.Text = "Couldn't save the file. Is it open in Excel? " + ex.Message; }
    }
}
