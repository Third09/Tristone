using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Hr;

/// <summary>
/// HR & Payroll > Employees: office staff and site workers, their pay details and daily rate history.
/// Plain layout for now; control names match the UI Elements Guide (Part 2: Tab: Employees).
/// </summary>
public partial class EmployeesTab : UserControl
{
    int? editingEmployeeId;   // null = adding a new employee

    public EmployeesTab()
    {
        InitializeComponent();
        dgvEmployees.AutoGenerateColumns = false;
        dgvRateHistory.AutoGenerateColumns = false;
        colCurrentRate.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        colRateAmount.DefaultCellStyle.FormatProvider = ProjectService.Peso;

        cboEmployeeStatusFilter.Items.AddRange([EmployeeStatuses.Active, EmployeeStatuses.Inactive, "All"]);
        cboEmployeeStatusFilter.SelectedIndex = 0;
        cboPayScheduleFilter.Items.AddRange(["All", PaySchedules.Office, PaySchedules.Site]);
        cboPayScheduleFilter.SelectedIndex = 0;
        cboEmployeeDepartment.Items.AddRange(EmployeeDepartments.All);
        numDailyRate.Maximum = EmployeeService.MaxDailyRate;
        StartNew();
    }

    async void EmployeesTab_Load(object sender, EventArgs e) => await LoadEmployeesAsync();

    async Task LoadEmployeesAsync(int? selectEmployeeId = null)
    {
        string? status = (string)cboEmployeeStatusFilter.SelectedItem! == "All" ? null : (string)cboEmployeeStatusFilter.SelectedItem!;
        string? schedule = cboPayScheduleFilter.SelectedIndex > 0 ? (string)cboPayScheduleFilter.SelectedItem! : null;
        dgvEmployees.DataSource = await EmployeeService.ListAsync(txtEmployeeSearch.Text, status, schedule);

        if (selectEmployeeId is { } id)
            foreach (DataGridViewRow r in dgvEmployees.Rows)
                if (((EmployeeRow)r.DataBoundItem!).EmployeeId == id) { r.Selected = true; dgvEmployees.CurrentCell = r.Cells[0]; }
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
        UpdateButtons();
        if (Selected is { } row && await EmployeeService.GetAsync(row.EmployeeId) is { } emp) await StartEditAsync(emp);
    }

    // ----- form -----

    void StartNew()
    {
        editingEmployeeId = null;
        lblFormMode.Text = "New employee";
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
    }

    async Task StartEditAsync(Employee e)
    {
        editingEmployeeId = e.EmployeeId;
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
        ShowPaySchedule();
    }

    void ShowPaySchedule()
    {
        lblPayScheduleValue.Text = cboEmployeeDepartment.SelectedItem is string dept
            ? (PaySchedules.For(dept) == PaySchedules.Site ? "Pay schedule: Site (paid every Saturday)" : "Pay schedule: Office (paid the 15th and 30th)")
            : "Pay schedule: (choose a department)";
    }

    void cboEmployeeDepartment_SelectedIndexChanged(object sender, EventArgs e) => ShowPaySchedule();

    EmployeeInput ReadForm() => new(
        txtEmployeeName.Text, txtEmployeeContact.Text, txtEmployeeAddress.Text, txtEmployeePosition.Text,
        (string?)cboEmployeeDepartment.SelectedItem ?? "",
        txtPayGrade.Text, txtBiometricId.Text, chkGovDeductions.Checked,
        txtSssNo.Text, txtPhilHealthNo.Text, txtPagIbigNo.Text, txtEmployeeTin.Text,
        numDailyRate.Value, DateOnly.FromDateTime(dtpRateEffective.Value));

    void btnAddEmployee_Click(object sender, EventArgs e)
    {
        dgvEmployees.ClearSelection();
        StartNew();
        txtEmployeeName.Focus();
    }

    async void btnCancelEmployee_Click(object sender, EventArgs e)
    {
        if (editingEmployeeId is { } id && await EmployeeService.GetAsync(id) is { } emp) await StartEditAsync(emp); else StartNew();
    }

    async void btnSaveEmployee_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (EmployeeService.CheckInput(input) is { } problem) { lblEmployeeFormMsg.Text = problem; return; }

        btnSaveEmployee.Enabled = false;
        try
        {
            if (editingEmployeeId is { } id)
            {
                var error = await EmployeeService.UpdateAsync(id, input);
                lblEmployeeFormMsg.Text = error ?? "Saved.";
                if (error == null) await LoadEmployeesAsync(id);
            }
            else
            {
                var (newId, error) = await EmployeeService.CreateAsync(input);
                lblEmployeeFormMsg.Text = error ?? "Employee added.";
                if (newId != null) await LoadEmployeesAsync(newId);
            }
        }
        catch (Exception ex) { lblEmployeeFormMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveEmployee.Enabled = true; }
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
