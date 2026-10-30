using Company.OfficeApp.Services;
using Company.OfficeApp.UI;

namespace Company.OfficeApp.Pages.Hr;

/// <summary>
/// HR & Payroll > Payslip History: every payslip of a finalized payroll, searchable by name and payroll;
/// view, print or save one as PDF.
/// Plain layout for now; control names match the UI Elements Guide (Part 2: Tab: Payslip History).
/// </summary>
public partial class PayslipHistoryTab : UserControl
{
    readonly LatestOnly loads = new();
    bool filling, ready;

    public PayslipHistoryTab()
    {
        InitializeComponent();
        dgvPayslipHistory.AutoGenerateColumns = false;
        foreach (var c in new[] { colPsGross, colPsNet }) c.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        Responsive.FitGrid(this, dgvPayslipHistory);
        Responsive.OnTypingPause(txtPayslipSearch, LoadAsync);
        RecordForm.NoAutoSelect(dgvPayslipHistory, on => filling = on, () => { filling = true; dgvPayslipHistory.ClearSelection(); filling = false; UpdateButtons(); });
        UpdateButtons();
    }

    /// <summary>A finalized payroll in the filter.</summary>
    record RunOption(int RunId, string Label) { public override string ToString() => Label; }

    async void PayslipHistoryTab_Load(object sender, EventArgs e) => await ReloadAsync();

    /// <summary>Refills the payroll filter and the list (after a payroll is finalized).</summary>
    public async Task ReloadAsync()
    {
        var keep = (cboPayslipPeriod.SelectedItem as RunOption)?.RunId;
        var runs = (await PayrollService.FinalRunsAsync()).Select(r => new RunOption(r.RunId, r.Label)).ToList();
        filling = true;
        cboPayslipPeriod.Items.Clear();
        cboPayslipPeriod.Items.Add("All");
        cboPayslipPeriod.Items.AddRange([.. runs]);
        cboPayslipPeriod.SelectedItem = (object?)runs.FirstOrDefault(r => r.RunId == keep) ?? "All";
        filling = false;
        ready = true;
        await LoadAsync();
    }

    async Task LoadAsync()
    {
        if (!ready) return;
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await PayrollService.HistoryAsync((cboPayslipPeriod.SelectedItem as RunOption)?.RunId, txtPayslipSearch.Text);
        if (!loads.IsLatest(ticket)) return;
        filling = true;
        dgvPayslipHistory.DataSource = rows;
        dgvPayslipHistory.ClearSelection();
        filling = false;
        lblPayslipMsg.Text = rows.Count == 0 ? "No finalized payslips yet." : "";
        UpdateButtons();
    }

    PayrollRow? Selected => dgvPayslipHistory.SelectedRows.Count > 0 ? dgvPayslipHistory.SelectedRows[0].DataBoundItem as PayrollRow : null;

    void UpdateButtons() => btnViewPayslip.Enabled = btnPrintPayslip.Enabled = btnSavePayslipPdf.Enabled = Selected != null;

    async void Filter_Changed(object sender, EventArgs e) { if (!filling) await LoadAsync(); }

    void dgvPayslipHistory_SelectionChanged(object sender, EventArgs e) { if (!filling) UpdateButtons(); }

    void dgvPayslipHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0) btnViewPayslip_Click(this, EventArgs.Empty); }

    async Task<PayslipPrint?> SlipAsync() => Selected is { } r ? new PayslipPrint(await PayrollService.PayslipsAsync([r.PayslipId])) : null;

    async void btnViewPayslip_Click(object sender, EventArgs e)
    {
        using var slip = await SlipAsync();
        if (slip == null) return;
        using var preview = new PrintPreviewDialog { Document = slip.Document, Width = 900, Height = 700 };
        preview.ShowDialog(this);
    }

    async void btnPrintPayslip_Click(object sender, EventArgs e)
    {
        using var slip = await SlipAsync();
        if (slip == null) return;
        using var dlg = new PrintDialog { Document = slip.Document, UseEXDialog = true };
        if (dlg.ShowDialog(this) == DialogResult.OK) slip.Document.Print();
    }

    async void btnSavePayslipPdf_Click(object sender, EventArgs e)
    {
        if (Selected is not { } r) return;
        using var dlg = new SaveFileDialog { Filter = "PDF file (*.pdf)|*.pdf", FileName = $"Payslip {r.Name} {r.PayDate:yyyy-MM-dd}.pdf" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            using var slip = await SlipAsync();
            slip?.SaveAsPdf(dlg.FileName);
            lblPayslipMsg.Text = $"Saved: {dlg.FileName}";
        }
        catch (Exception ex) { lblPayslipMsg.Text = "Couldn't save the PDF (is \"Microsoft Print to PDF\" installed?). " + ex.Message; }
    }
}
