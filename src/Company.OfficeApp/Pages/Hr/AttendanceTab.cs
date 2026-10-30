using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Hr;

/// <summary>
/// HR & Payroll > Attendance: upload the scanner's file, see each employee's days (late, absent, missing scans,
/// overtime), fix a missing scan with a reason, and (HR Supervisor / Admin) approve or reject overtime.
/// Plain layout for now; control names match the UI Elements Guide (Part 2: Tab: Attendance).
/// </summary>
public partial class AttendanceTab : UserControl
{
    readonly bool approver;
    readonly LatestOnly loads = new();
    List<string> lastProblems = [];
    bool filling, ready;
    int? selectedDayId;

    public AttendanceTab()
    {
        InitializeComponent();
        dgvAttendance.AutoGenerateColumns = false;
        cboAttendanceFilter.Items.AddRange(AttendanceService.Filters);
        cboAttendanceFilter.SelectedIndex = 0;
        var today = DateTime.Today;
        dtpAttendanceFrom.Value = new DateTime(today.Year, today.Month, 1);
        dtpAttendanceTo.Value = today;
        approver = Access.IsApprover(Session.CurrentUser!, Departments.HrPayroll);
        btnApproveOvertime.Visible = btnRejectOvertime.Visible = approver;
        lblApproverNote.Visible = !approver;
        btnViewImportErrors.Enabled = false;
        Responsive.FitGrid(this, dgvAttendance);
        Responsive.OnTypingPause(txtAttendanceSearch, () => LoadAsync());
        RecordForm.NoAutoSelect(dgvAttendance, on => filling = on, RestoreSelection);
        UpdateButtons();
    }

    async void AttendanceTab_Load(object sender, EventArgs e)
    {
        if (await AttendanceService.LastImportAsync() is { } last) ShowImport(last.Summary, last.Problems);
        ready = true;
        await LoadAsync();
    }

    void ShowImport(string summary, List<string> problems)
    {
        lblImportSummary.Text = summary;
        lastProblems = problems;
        btnViewImportErrors.Enabled = problems.Count > 0;
    }

    async Task LoadAsync(int? selectId = null)
    {
        if (!ready) return;
        var from = DateOnly.FromDateTime(dtpAttendanceFrom.Value);
        var to = DateOnly.FromDateTime(dtpAttendanceTo.Value);
        if (to < from) { lblAttendanceMsg.Text = "The To date can't be before the From date."; return; }
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await AttendanceService.ListAsync(from, to, (string)cboAttendanceFilter.SelectedItem!, txtAttendanceSearch.Text);
        if (!loads.IsLatest(ticket)) return;
        filling = true;
        dgvAttendance.DataSource = rows;
        dgvAttendance.ClearSelection();
        selectedDayId = selectId;
        if (selectId is { } id) Responsive.SelectRow(dgvAttendance, (AttendanceRow r) => r.DayId == id);
        filling = false;
        int problems = rows.Count(r => r.IsProblem);
        lblAttendanceCount.Text = $"{rows.Count} day{(rows.Count == 1 ? "" : "s")} shown" + (problems > 0 ? $", {problems} need HR's attention (highlighted)" : "");
        UpdateButtons();
    }

    /// <summary>After the grid picked a row by itself on first show (RecordForm.NoAutoSelect): only the intended row stays selected.</summary>
    void RestoreSelection()
    {
        filling = true;
        dgvAttendance.ClearSelection();
        if (selectedDayId is { } id) Responsive.SelectRow(dgvAttendance, (AttendanceRow r) => r.DayId == id);
        filling = false;
        UpdateButtons();
    }

    AttendanceRow? Selected => dgvAttendance.SelectedRows.Count > 0 ? dgvAttendance.SelectedRows[0].DataBoundItem as AttendanceRow : null;

    void UpdateButtons()
    {
        btnFixScan.Enabled = Selected != null;
        btnApproveOvertime.Enabled = btnRejectOvertime.Enabled = Selected is { OtStatus: OtStatuses.Waiting };
    }

    async void Filter_Changed(object sender, EventArgs e) => await LoadAsync();

    void dgvAttendance_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        selectedDayId = Selected?.DayId;
        UpdateButtons();
    }

    // Days that need HR: absent / missing scan light red, overtime waiting light yellow (a function, not styling).
    void dgvAttendance_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.CellStyle == null || dgvAttendance.Rows[e.RowIndex].DataBoundItem is not AttendanceRow r) return;
        if (r.Status is AttendanceStatuses.Absent or AttendanceStatuses.MissingScan) e.CellStyle.BackColor = Color.MistyRose;
        else if (r.OtStatus == OtStatuses.Waiting) e.CellStyle.BackColor = Color.LightYellow;
    }

    void dgvAttendance_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnFixScan_Click(this, EventArgs.Empty);
    }

    async void btnUploadBiometrics_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Choose the file exported from the scanner (All Report)",
            Filter = "Scanner export (*.xls;*.xlsx;*.csv)|*.xls;*.xlsx;*.csv",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        btnUploadBiometrics.Enabled = false;
        try
        {
            using var busy = Responsive.Busy(this);
            var result = await AttendanceService.ImportAsync(dlg.FileName);
            ShowImport(result.Summary, result.Problems);
            lblAttendanceMsg.Text = "Upload done.";
            // Show the uploaded period.
            filling = true;
            dtpAttendanceFrom.Value = result.PeriodFrom.ToDateTime(TimeOnly.MinValue);
            dtpAttendanceTo.Value = result.PeriodTo.ToDateTime(TimeOnly.MinValue);
            filling = false;
            await LoadAsync();
            if (result.Problems.Count > 0) btnViewImportErrors_Click(this, EventArgs.Empty);
        }
        catch (FormatException ex) { MessageBox.Show(this, ex.Message, "Not a scanner file", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        catch (IOException ex) { MessageBox.Show(this, "The file couldn't be opened. Is it open in Excel?\n" + ex.Message, "Upload"); }
        catch (Exception ex) { lblAttendanceMsg.Text = "The upload failed. " + ex.Message; }
        finally { btnUploadBiometrics.Enabled = true; }
    }

    void btnViewImportErrors_Click(object sender, EventArgs e)
    {
        if (lastProblems.Count == 0) return;
        MessageBox.Show(this, string.Join("\n\n", lastProblems), "Rejected rows", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Makes the scanner's "Employee Form" from the employee list (ID = Biometric ID), to load on the scanner by USB.
    async void btnExportToScanner_Click(object sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog
        {
            Title = "Save the employee file for the scanner (put it on the USB stick)",
            Filter = "Scanner employee file (*.xls)|*.xls",
            FileName = "Employee Form.xls",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            var result = await ScannerEmployeeFile.SaveAsync(dlg.FileName);
            var text = $"Saved {result.Employees} employee{(result.Employees == 1 ? "" : "s")} to:\n{dlg.FileName}\n\n"
                + "Copy it to the USB stick and load it on the scanner (its USB menu, upload user / employee information). "
                + "Fingerprints are still registered on the scanner itself.";
            if (result.Skipped.Count > 0) text += "\n\nNot included:\n" + string.Join("\n", result.Skipped);
            MessageBox.Show(this, text, "Scanner employee file", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Scanner employee file"); }
    }

    async void btnFixScan_Click(object sender, EventArgs e)
    {
        if (Selected is not { } r) return;
        using var f = new FixScanForm(r);
        if (f.ShowDialog(this) != DialogResult.OK) return;
        lblAttendanceMsg.Text = $"{r.Name}'s {r.Date:MMM d} was fixed.";
        await LoadAsync(r.DayId);
    }

    async Task DecideAsync(bool approve)
    {
        if (Selected is not { } r) return;
        var error = approve ? await AttendanceService.ApproveOvertimeAsync(r.DayId) : await AttendanceService.RejectOvertimeAsync(r.DayId);
        lblAttendanceMsg.Text = error ?? $"{r.Name}'s overtime on {r.Date:MMM d} {(approve ? "approved" : "rejected")}.";
        if (error == null) await LoadAsync(r.DayId);
    }

    async void btnApproveOvertime_Click(object sender, EventArgs e) => await DecideAsync(true);

    async void btnRejectOvertime_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(this, "Reject this overtime? It won't be paid.", "Reject overtime", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            await DecideAsync(false);
    }

    void btnExportAttendance_Click(object sender, EventArgs e)
    {
        if (dgvAttendance.DataSource is not List<AttendanceRow> rows) return;
        if (ExcelExport.AskPath(this, "Attendance") is not { } path) return;
        string T(TimeOnly? t) => t?.ToString("h:mm tt") ?? "";
        try
        {
            ExcelExport.Save(path, "Attendance", rows, new("Date", r => r.Date, "mmm d, yyyy"), new("Day", r => r.Day), new("Name", r => r.Name),
                new("Time In", r => T(r.TimeIn)), new("Break Out", r => T(r.BreakOut)), new("Break In", r => T(r.BreakIn)), new("Time Out", r => T(r.TimeOut)),
                new("Late (min)", r => r.LateMinutes), new("Early (min)", r => r.EarlyMinutes), new("Overtime (h)", r => r.OtHours, "0.00"),
                new("Overtime Status", r => r.OtMinutes == 0 ? "" : r.OtStatus), new("Status", r => r.Status), new("Fix Reason", r => r.FixReason));
            MessageBox.Show(this, $"Saved {rows.Count} days to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
