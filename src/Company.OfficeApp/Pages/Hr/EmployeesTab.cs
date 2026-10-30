using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Hr;

/// <summary>
/// HR & Payroll > Employees: office staff and site workers, their pay details and daily rate history.
/// Plain layout for now; control names match the UI Elements Guide (Part 2: Tab: Employees).
/// </summary>
public partial class EmployeesTab : UserControl
{
    // The form on this tab only edits the employee clicked in the list; adding uses EmployeeEntryForm.
    Employee? editing;        // null = nobody selected, form is empty and locked
    decimal editingRate;      // the daily rate shown when the employee was loaded, to spot a rate change
    bool filling;             // true while the list reloads, so the grid's own selection changes are ignored
    readonly LatestOnly loads = new();

    public EmployeesTab()
    {
        InitializeComponent();
        dgvEmployees.AutoGenerateColumns = false;
        RecordForm.NoAutoSelect(dgvEmployees, on => filling = on, RestoreSelection);
        dgvRateHistory.AutoGenerateColumns = false;
        colCurrentRate.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        colRateAmount.DefaultCellStyle.FormatProvider = ProjectService.Peso;

        cboEmployeeStatusFilter.Items.AddRange([EmployeeStatuses.Active, EmployeeStatuses.Inactive, "All"]);
        cboEmployeeStatusFilter.SelectedIndex = 0;
        cboPayScheduleFilter.Items.AddRange(["All", PaySchedules.Office, PaySchedules.Site]);
        cboPayScheduleFilter.SelectedIndex = 0;
        cboEmployeeDepartment.Items.AddRange(EmployeeDepartments.All);
        numDailyRate.Maximum = EmployeeService.MaxDailyRate;
        Responsive.FitGrid(this, dgvEmployees);
        Responsive.OnTypingPause(txtEmployeeSearch, () => LoadEmployeesAsync());
        ShowNothingSelected();
    }

    async void EmployeesTab_Load(object sender, EventArgs e) => await LoadEmployeesAsync();

    async Task LoadEmployeesAsync(int? selectEmployeeId = null)
    {
        string? status = (string)cboEmployeeStatusFilter.SelectedItem! == "All" ? null : (string)cboEmployeeStatusFilter.SelectedItem!;
        string? schedule = cboPayScheduleFilter.SelectedIndex > 0 ? (string)cboPayScheduleFilter.SelectedItem! : null;
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await EmployeeService.ListAsync(txtEmployeeSearch.Text, status, schedule);
        if (!loads.IsLatest(ticket)) return;   // a newer search already started

        selectEmployeeId ??= editing?.EmployeeId;   // searching or filtering keeps the person being edited selected
        filling = true;
        dgvEmployees.DataSource = rows;
        // Never let the grid pick the first row by itself: the form would silently edit that person,
        // and typing a "new" employee into it would overwrite them.
        dgvEmployees.ClearSelection();
        if (selectEmployeeId is { } id)
            Responsive.SelectRow(dgvEmployees, (EmployeeRow row) => row.EmployeeId == id);
        filling = false;

        if (Selected is not { } selected) ShowNothingSelected();
        else if (selected.EmployeeId != editing?.EmployeeId && await EmployeeService.GetAsync(selected.EmployeeId) is { } emp)
            await StartEditAsync(emp);
        UpdateButtons();
    }

    // SelectedRows, not CurrentRow: after ClearSelection (Add Employee) the grid still has a current row.
    EmployeeRow? Selected => dgvEmployees.SelectedRows.Count > 0 ? dgvEmployees.SelectedRows[0].DataBoundItem as EmployeeRow : null;

    void UpdateButtons()
    {
        btnMarkInactive.Enabled = Selected?.Status == EmployeeStatuses.Active;
        btnReactivateEmployee.Enabled = Selected?.Status == EmployeeStatuses.Inactive;
    }

    async void Filter_Changed(object sender, EventArgs e) => await LoadEmployeesAsync();

    async void dgvEmployees_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        UpdateButtons();
        if (Selected is not { } row) ShowNothingSelected();
        else if (await EmployeeService.GetAsync(row.EmployeeId) is { } emp) await StartEditAsync(emp);
    }

    Control[] FormInputs => [txtEmployeeName, txtEmployeeContact, txtEmployeeAddress, txtEmployeePosition, cboEmployeeDepartment,
                             txtPayGrade, numDailyRate, dtpRateEffective, txtBiometricId, chkGovDeductions,
                             txtSssNo, txtPhilHealthNo, txtPagIbigNo, txtEmployeeTin, btnSaveEmployee, btnCancelEmployee];

    /// <summary>After the grid picked a row by itself on first show (RecordForm.NoAutoSelect): only the record being shown stays selected.</summary>
    void RestoreSelection()
    {
        filling = true;
        dgvEmployees.ClearSelection();
        if (editing?.EmployeeId is { } id) Responsive.SelectRow(dgvEmployees, (EmployeeRow r) => r.EmployeeId == id);
        filling = false;
        if (Selected == null) ShowNothingSelected();
        UpdateButtons();
    }

    void ShowNothingSelected()
    {
        editing = null;
        lblFormMode.Text = "Click an employee in the list to see or change their details. To add someone, click Add Employee.";
        foreach (var t in new[] { txtEmployeeName, txtEmployeeContact, txtEmployeeAddress, txtEmployeePosition, txtPayGrade,
                                  txtBiometricId, txtSssNo, txtPhilHealthNo, txtPagIbigNo, txtEmployeeTin })
            t.Clear();
        cboEmployeeDepartment.SelectedIndex = -1;
        numDailyRate.Value = 0;
        dtpRateEffective.Value = DateTime.Today;
        chkGovDeductions.Checked = false;
        dgvRateHistory.DataSource = null;
        lblEmployeeFormMsg.Text = "";
        ShowPaySchedule();
        foreach (var c in FormInputs) c.Enabled = false;
    }

    async Task StartEditAsync(Employee e)
    {
        editing = e;
        foreach (var c in FormInputs) c.Enabled = true;
        lblFormMode.Text = $"Editing: {e.FullName}";
        txtEmployeeName.Text = e.FullName;
        txtEmployeeContact.Text = e.ContactNumber ?? "";
        txtEmployeeAddress.Text = e.Address ?? "";
        txtEmployeePosition.Text = e.Position ?? "";
        cboEmployeeDepartment.SelectedItem = e.Department;
        txtPayGrade.Text = e.PayGrade ?? "";
        txtBiometricId.Text = e.BiometricId ?? "";
        chkGovDeductions.Checked = e.GovDeductions;
        txtSssNo.Text = e.SssNo ?? "";
        txtPhilHealthNo.Text = e.PhilHealthNo ?? "";
        txtPagIbigNo.Text = e.PagIbigNo ?? "";
        txtEmployeeTin.Text = e.Tin ?? "";
        lblEmployeeFormMsg.Text = "";

        var rates = await EmployeeService.GetRatesAsync(e.EmployeeId);
        dgvRateHistory.DataSource = rates;
        dtpRateEffective.Value = DateTime.Today;
        numDailyRate.Value = await EmployeeService.RateOnAsync(e.EmployeeId, DateOnly.FromDateTime(DateTime.Today))
                             ?? rates.LastOrDefault()?.DailyRate ?? 0;
        editingRate = numDailyRate.Value;
        ShowPaySchedule();
    }

    void ShowPaySchedule() => lblPayScheduleValue.Text = EmployeeService.PayScheduleText(cboEmployeeDepartment.SelectedItem as string);

    /// <summary>One line per changed field, for the "Save these changes?" question. Empty = nothing changed.</summary>
    List<string> DescribeChanges(Employee e, EmployeeInput i)
    {
        var lines = new List<string>();
        void Text(string label, string? before, string? after)
        {
            before = (before ?? "").Trim(); after = (after ?? "").Trim();
            if (before != after) lines.Add($"{label}: {(before == "" ? "(empty)" : before)} -> {(after == "" ? "(empty)" : after)}");
        }
        // Government ID numbers are private: say that they change, not what they are.
        void Private(string label, string? before, string? after)
        {
            if ((before ?? "").Trim() != (after ?? "").Trim()) lines.Add($"{label}: changed");
        }
        Text("Name", e.FullName, i.FullName);
        Text("Contact number", e.ContactNumber, i.ContactNumber);
        Text("Address", e.Address, i.Address);
        Text("Position", e.Position, i.Position);
        if (e.Department != i.Department)
            lines.Add($"Department: {e.Department} -> {i.Department} ({EmployeeService.PayScheduleText(i.Department).Replace("Pay schedule: ", "pay schedule ")})");
        Text("Pay grade", e.PayGrade, i.PayGrade);
        if (i.DailyRate != editingRate)
            lines.Add($"Daily rate: {ProjectService.Money(editingRate)} -> {ProjectService.Money(i.DailyRate)} from {i.RateFrom:d MMM yyyy}");
        Text("Biometric ID", e.BiometricId, i.BiometricId);
        if (e.GovDeductions != i.GovDeductions) lines.Add($"Government deductions: {(i.GovDeductions ? "turned on" : "turned off")}");
        Private("SSS number", e.SssNo, i.SssNo);
        Private("PhilHealth number", e.PhilHealthNo, i.PhilHealthNo);
        Private("Pag-IBIG number", e.PagIbigNo, i.PagIbigNo);
        Private("TIN", e.Tin, i.Tin);
        return lines;
    }

    void cboEmployeeDepartment_SelectedIndexChanged(object sender, EventArgs e) => ShowPaySchedule();

    EmployeeInput ReadForm() => new(
        txtEmployeeName.Text, txtEmployeeContact.Text, txtEmployeeAddress.Text, txtEmployeePosition.Text,
        (string?)cboEmployeeDepartment.SelectedItem ?? "",
        txtPayGrade.Text, txtBiometricId.Text, chkGovDeductions.Checked,
        txtSssNo.Text, txtPhilHealthNo.Text, txtPagIbigNo.Text, txtEmployeeTin.Text,
        numDailyRate.Value, DateOnly.FromDateTime(dtpRateEffective.Value));

    async void btnAddEmployee_Click(object sender, EventArgs e)
    {
        using var f = new EmployeeEntryForm();
        if (f.ShowDialog(this) != DialogResult.OK || f.NewEmployeeId is not { } newId) return;
        editing = null;
        if (cboEmployeeStatusFilter.SelectedIndex != 0) cboEmployeeStatusFilter.SelectedIndex = 0;   // new people are Active
        await LoadEmployeesAsync(newId);
        lblEmployeeFormMsg.Text = "Employee added.";
    }

    async void btnCancelEmployee_Click(object sender, EventArgs e)
    {
        if (editing != null && await EmployeeService.GetAsync(editing.EmployeeId) is { } emp) await StartEditAsync(emp);
    }

    async void btnSaveEmployee_Click(object sender, EventArgs e)
    {
        if (editing is not { } before) return;
        var input = ReadForm();
        if (EmployeeService.CheckInput(input) is { } problem) { lblEmployeeFormMsg.Text = problem; return; }

        var changes = DescribeChanges(before, input);
        if (changes.Count == 0) { lblEmployeeFormMsg.Text = "Nothing changed."; return; }
        var ok = MessageBox.Show(this,
            $"Save these changes to {before.FullName}?\n\n" + string.Join("\n", changes.Select(c => "- " + c)),
            "Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        if (ok != DialogResult.Yes) { lblEmployeeFormMsg.Text = "Not saved. Click Cancel to undo your changes."; return; }

        btnSaveEmployee.Enabled = false;
        try
        {
            var error = await EmployeeService.UpdateAsync(before.EmployeeId, input);
            if (error != null) { lblEmployeeFormMsg.Text = error; return; }
            if (await EmployeeService.GetAsync(before.EmployeeId) is { } fresh) await StartEditAsync(fresh);
            await LoadEmployeesAsync(before.EmployeeId);
            lblEmployeeFormMsg.Text = "Saved.";
        }
        catch (Exception ex) { lblEmployeeFormMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveEmployee.Enabled = editing != null; }
    }

    // ----- list actions -----

    async void btnMarkInactive_Click(object sender, EventArgs e)
    {
        if (Selected is not { } row) return;
        var ok = MessageBox.Show(this,
            $"Mark {row.FullName} as Inactive (left the company)?\n\nTheir records are kept. If they have a login, it is turned off.",
            "Mark Inactive", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (ok != DialogResult.Yes) return;
        await RunAsync(EmployeeService.MarkInactiveAsync, row.EmployeeId);
    }

    async void btnReactivateEmployee_Click(object sender, EventArgs e)
    {
        if (Selected is not { } row) return;
        await RunAsync(EmployeeService.ReactivateAsync, row.EmployeeId);
        MessageBox.Show(this, "Active again. If they had a login, an Admin can turn it back on in User Accounts.",
            "Reactivated", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    async Task RunAsync(Func<int, Task<string?>> action, int employeeId)
    {
        var error = await action(employeeId);
        if (error != null) MessageBox.Show(this, error, "Not done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        await LoadEmployeesAsync(employeeId);
    }

    void btnExportEmployees_Click(object sender, EventArgs e)
    {
        if (dgvEmployees.DataSource is not List<EmployeeRow> rows) return;
        if (ExcelExport.AskPath(this, "Employees") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Employees", rows,
                new("Name", r => r.FullName),
                new("Position", r => r.Position),
                new("Department", r => r.Department),
                new("Pay Schedule", r => r.PaySchedule),
                new("Daily Rate", r => r.CurrentRate, "#,##0.00"),
                new("Biometric ID", r => r.BiometricId),
                new("Status", r => r.Status));
            MessageBox.Show(this, $"Saved {rows.Count} employees to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
