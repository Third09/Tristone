using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages;

/// <summary>
/// Dashboard (Admin and Supervisors): KPI cards, project status chart, top performing projects, and (Admin only)
/// the log history. Supervisors see only their own department's expenses. Numbers come from
/// <see cref="DashboardService"/> / <see cref="ProjectFinance"/>, the same ones the Projects page uses.
/// Plain layout for now; control names match the UI Elements Guide (Part 1: Dashboard Page).
/// </summary>
public partial class DashboardPage : UserControl
{
    readonly bool admin;
    readonly LatestOnly loads = new(), logLoads = new();
    List<TopProjectRow> top = [];
    string sortBy = nameof(TopProjectRow.Profit);
    bool sortDescending = true, ready;

    public DashboardPage()
    {
        InitializeComponent();
        dgvTopProjects.AutoGenerateColumns = false;
        dgvLogHistory.AutoGenerateColumns = false;
        foreach (var c in new[] { colTopRevenue, colTopExpenses, colTopProfit }) c.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        cboPeriod.Items.AddRange(DashboardService.Periods);
        cboPeriod.SelectedItem = DashboardService.ThisYear;
        dtpFrom.Value = new DateTime(DateTime.Today.Year, 1, 1);
        dtpTo.Value = DateTime.Today;

        var user = Session.CurrentUser!;
        admin = user.IsAdmin;
        if (admin)
        {
            cboDepartment.Items.Add("All");
            cboDepartment.Items.AddRange(Departments.All);
            cboDepartment.SelectedIndex = 0;
        }
        else
        {
            cboDepartment.Items.Add(user.Department!);   // Supervisors: locked to their own department
            cboDepartment.SelectedIndex = 0;
            cboDepartment.Enabled = false;
        }
        cboLogType.Items.AddRange(DashboardService.LogTypes);
        cboLogType.SelectedIndex = 0;
        foreach (Control c in new Control[] { lblLogTitle, lblLogType, cboLogType, lblLogSearch, txtLogSearch, btnExportLogs, dgvLogHistory })
            c.Visible = admin;
        Responsive.FitGrid(this, dgvLogHistory);
        Responsive.OnTypingPause(txtLogSearch, LoadLogAsync);
        cboPeriod_SelectedIndexChanged(this, EventArgs.Empty);
    }

    async void DashboardPage_Load(object sender, EventArgs e)
    {
        ready = true;
        await RefreshAllAsync();
    }

    string? Department => cboDepartment.SelectedIndex > 0 || !admin ? (string)cboDepartment.SelectedItem! : null;

    (DateOnly From, DateOnly To) Range() => DashboardService.Range((string)cboPeriod.SelectedItem!, DateOnly.FromDateTime(DateTime.Today),
        DateOnly.FromDateTime(dtpFrom.Value), DateOnly.FromDateTime(dtpTo.Value));

    void cboPeriod_SelectedIndexChanged(object? sender, EventArgs e)
    {
        bool custom = (string?)cboPeriod.SelectedItem == DashboardService.Custom;
        dtpFrom.Enabled = dtpTo.Enabled = lblFrom.Enabled = lblTo.Enabled = custom;
        if (!custom)
        {
            var (from, to) = Range();
            dtpFrom.Value = from.ToDateTime(TimeOnly.MinValue);
            dtpTo.Value = to.ToDateTime(TimeOnly.MinValue);
        }
    }

    async void btnRefreshDashboard_Click(object sender, EventArgs e) => await RefreshAllAsync();

    async Task RefreshAllAsync()
    {
        if (!ready) return;
        var (from, to) = Range();
        if (to < from) { lblKpiBreakdown.Text = "The To date can't be before the From date."; return; }
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        try
        {
            var kpis = await DashboardService.KpisAsync(from, to, Department);
            var statuses = await DashboardService.StatusCountsAsync();
            var projects = await DashboardService.TopProjectsAsync(from, to, Department);
            if (!loads.IsLatest(ticket)) return;
            ShowKpis(kpis);
            ShowChart(statuses);
            top = projects;
            ShowTop();
        }
        catch (Exception ex) { lblKpiBreakdown.Text = "Can't reach the database. " + ex.Message; }
        await LoadLogAsync();
    }

    void ShowKpis(DashboardKpis k)
    {
        string M(decimal v) => ProjectService.Money(v);
        lblKpiCompleted.Text = k.CompletedProjects.ToString();
        lblKpiRevenue.Text = M(k.GrossRevenue);
        lblKpiExpenses.Text = M(k.OperationalExpenses);
        lblKpiNet.Text = M(k.NetRevenue);
        // A department view shows only its own expenses; revenue and net are company-wide, so they're hidden.
        pnlKpiRevenue.Visible = pnlKpiNet.Visible = k.Department == null;
        lblKpiExpensesTitle.Text = k.Department == null ? "Operational Expenses" : $"{k.Department} Expenses";
        colTopRevenue.Visible = colTopProfit.Visible = colTopMargin.Visible = k.Department == null;

        var b = k.Breakdown;
        var parts = new List<string>();
        void Add(string label, decimal v) { if (v != 0) parts.Add($"{label} {M(v)}"); }
        Add("materials used", b.ProjectMaterials); Add("project receipts", b.ProjectReceipts); Add("subcontractor payouts", b.ProjectPayouts);
        Add("overhead receipts", b.OverheadReceipts); Add("equipment repairs", b.EquipmentRepairs); Add("payroll", b.Payroll);
        lblKpiBreakdown.Text = (parts.Count == 0 ? "No expenses in this period." : "Expenses: " + string.Join(", ", parts) + ".")
            + "  Projects count when finished (Done); overhead counts by its date. Payroll joins when HR payroll is built.";
    }

    void ShowChart(List<(string Status, int Count)> statuses)
    {
        var plot = chartProjectStatus.Plot;
        plot.Clear();
        var bars = plot.Add.Bars(statuses.Select(s => (double)s.Count).ToArray());
        foreach (var bar in bars.Bars) bar.Label = bar.Value.ToString("0");
        plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
            statuses.Select((_, i) => (double)i).ToArray(), statuses.Select(s => s.Status).ToArray());
        plot.Axes.Margins(bottom: 0, top: 0.2);
        plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericAutomatic { IntegerTicksOnly = true };
        chartProjectStatus.Refresh();
    }

    // ----- top projects (sortable) -----

    void ShowTop()
    {
        IEnumerable<TopProjectRow> rows = sortBy switch
        {
            nameof(TopProjectRow.Project) => top.OrderBy(r => r.Project),
            nameof(TopProjectRow.Revenue) => top.OrderBy(r => r.Revenue),
            nameof(TopProjectRow.Expenses) => top.OrderBy(r => r.Expenses),
            nameof(TopProjectRow.MarginPercent) => top.OrderBy(r => r.MarginPercent ?? decimal.MinValue),
            _ => top.OrderBy(r => r.Profit),
        };
        dgvTopProjects.DataSource = (sortDescending ? rows.Reverse() : rows).ToList();
        foreach (DataGridViewColumn c in dgvTopProjects.Columns)
            c.HeaderCell.SortGlyphDirection = c.DataPropertyName == sortBy ? (sortDescending ? SortOrder.Descending : SortOrder.Ascending) : SortOrder.None;
    }

    void dgvTopProjects_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        var prop = dgvTopProjects.Columns[e.ColumnIndex].DataPropertyName;
        sortDescending = prop == sortBy ? !sortDescending : prop != nameof(TopProjectRow.Project);
        sortBy = prop;
        ShowTop();
    }

    void btnExportTopProjects_Click(object sender, EventArgs e)
    {
        if (dgvTopProjects.DataSource is not List<TopProjectRow> rows) return;
        if (ExcelExport.AskPath(this, "Top Projects") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Top Projects", rows, new("Project", r => r.Project), new("Status", r => r.Status),
                new("Revenue", r => r.Revenue, "#,##0.00"), new("Expenses", r => r.Expenses, "#,##0.00"),
                new("Profit", r => r.Profit, "#,##0.00"), new("Margin %", r => r.MarginPercent, "0.0"));
            MessageBox.Show(this, $"Saved {rows.Count} projects to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }

    // ----- log history (Admin) -----

    async void cboLogType_SelectedIndexChanged(object sender, EventArgs e) => await LoadLogAsync();

    async Task LoadLogAsync()
    {
        if (!admin || !ready) return;
        var (from, to) = Range();
        var ticket = logLoads.Start();
        var rows = await DashboardService.LogHistoryAsync(from, to, (string)cboLogType.SelectedItem!, txtLogSearch.Text);
        if (!logLoads.IsLatest(ticket)) return;
        dgvLogHistory.DataSource = rows;
        lblLogTitle.Text = $"Log history ({rows.Count}{(rows.Count >= 2000 ? "+, newest first" : "")})";
    }

    void btnExportLogs_Click(object sender, EventArgs e)
    {
        if (dgvLogHistory.DataSource is not List<LogRow> rows) return;
        if (ExcelExport.AskPath(this, "Log History") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Log History", rows, new("Date & Time", r => r.When, "mmm d, yyyy h:mm AM/PM"), new("Name", r => r.Name),
                new("Department", r => r.Department), new("Activity", r => r.Activity), new("Details", r => r.Details));
            MessageBox.Show(this, $"Saved {rows.Count} entries to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
