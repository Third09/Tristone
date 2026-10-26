using Company.OfficeApp.Services;

namespace Company.OfficeApp.Pages.Receipts;

/// <summary>
/// Receipts > Reports: Expense & Receipt Report and Weekly / Monthly Expense Summaries for all, overhead or one
/// project in a date range; preview, print, save as PDF or export to Excel.
/// Plain layout for now; control names match the UI Elements Guide (Part 5: Tab: Reports).
/// </summary>
public partial class ReceiptReportsTab : UserControl
{
    public ReceiptReportsTab()
    {
        InitializeComponent();
        cboReportType.Items.AddRange(ReceiptReports.Types);
        cboReportType.SelectedIndex = 0;
        dtpReportFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpReportTo.Value = DateTime.Today;
    }

    async void ReceiptReportsTab_Load(object sender, EventArgs e) => await ReloadProjectsAsync();

    public async Task ReloadProjectsAsync()
    {
        var keep = cboReportProject.SelectedItem;
        var projects = await MaterialService.ProjectOptionsAsync();
        cboReportProject.Items.Clear();
        cboReportProject.Items.AddRange(["All", ReceiptService.OverheadLabel, .. projects]);
        cboReportProject.SelectedItem = keep is ProjectOption p ? projects.FirstOrDefault(x => x.ProjectId == p.ProjectId) : keep;
        if (cboReportProject.SelectedIndex < 0) cboReportProject.SelectedIndex = 0;
    }

    async Task<ReportData?> BuildAsync()
    {
        var from = DateOnly.FromDateTime(dtpReportFrom.Value);
        var to = DateOnly.FromDateTime(dtpReportTo.Value);
        if (to < from) { lblReportMsg.Text = "The To date can't be before the From date."; return null; }
        int? project = cboReportProject.SelectedItem switch { ProjectOption p => p.ProjectId, ReceiptService.OverheadLabel => ReceiptFilter.OverheadOnly, _ => null };
        lblReportMsg.Text = "";
        try { return await ReceiptReports.BuildAsync((string)cboReportType.SelectedItem!, project, from, to); }
        catch (Exception ex) { lblReportMsg.Text = "Can't reach the database. " + ex.Message; return null; }
    }

    static TableReport Printable(ReportData d) => new(d.Title, d.Subtitle, d.Columns, d.Rows, d.TotalLine);

    async void btnPreviewReport_Click(object sender, EventArgs e)
    {
        if (await BuildAsync() is not { } data) return;
        using var report = Printable(data);
        using var preview = new PrintPreviewDialog { Document = report.Document, Width = 900, Height = 700 };
        preview.ShowDialog(this);
    }

    async void btnPrintReport_Click(object sender, EventArgs e)
    {
        if (await BuildAsync() is not { } data) return;
        using var report = Printable(data);
        using var dlg = new PrintDialog { Document = report.Document, UseEXDialog = true };
        if (dlg.ShowDialog(this) == DialogResult.OK) report.Document.Print();
    }

    async void btnReportPdf_Click(object sender, EventArgs e)
    {
        if (await BuildAsync() is not { } data) return;
        using var dlg = new SaveFileDialog { Filter = "PDF file (*.pdf)|*.pdf", FileName = $"{data.Title} {DateTime.Now:yyyy-MM-dd}.pdf" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            using var report = Printable(data);
            report.SaveAsPdf(dlg.FileName);
            lblReportMsg.Text = $"Saved: {dlg.FileName}";
        }
        catch (Exception ex) { lblReportMsg.Text = "Couldn't save the PDF (is \"Microsoft Print to PDF\" installed?). " + ex.Message; }
    }

    async void btnReportExcel_Click(object sender, EventArgs e)
    {
        if (await BuildAsync() is not { } data) return;
        if (ExcelExport.AskPath(this, data.Title) is not { } path) return;
        try
        {
            var columns = data.Columns.Select((c, i) => new ExcelExport.Column<string[]>(c.Header, r => r[i])).ToList();
            ExcelExport.Save(path, "Report", data.Rows.Append(data.Columns.Select((_, i) => i == 0 ? data.TotalLine : "").ToArray()), [.. columns]);
            lblReportMsg.Text = $"Saved: {path}";
        }
        catch (IOException ex) { lblReportMsg.Text = "Couldn't save the file. Is it open in Excel? " + ex.Message; }
    }
}
