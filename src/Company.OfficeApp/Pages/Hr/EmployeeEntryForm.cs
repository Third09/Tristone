using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Hr;

/// <summary>
/// Pop-up for adding a new employee (HR & Payroll > Employees > Add Employee).
/// Adding has its own window so it can never be mixed up with editing someone who is selected in the list.
/// </summary>
public partial class EmployeeEntryForm : Form
{
    /// <summary>Set when the employee was saved; the Employees tab selects them.</summary>
    public int? NewEmployeeId { get; private set; }

    public EmployeeEntryForm()
    {
        InitializeComponent();
        cboEmployeeDepartment.Items.AddRange(EmployeeDepartments.All);
        numDailyRate.Maximum = EmployeeService.MaxDailyRate;
        dtpRateEffective.Value = DateTime.Today;
        lblPayScheduleValue.Text = EmployeeService.PayScheduleText(null);
    }

    void cboEmployeeDepartment_SelectedIndexChanged(object sender, EventArgs e) =>
        lblPayScheduleValue.Text = EmployeeService.PayScheduleText(cboEmployeeDepartment.SelectedItem as string);

    EmployeeInput ReadForm() => new(
        txtEmployeeName.Text, txtEmployeeContact.Text, txtEmployeeAddress.Text, txtEmployeePosition.Text,
        (string?)cboEmployeeDepartment.SelectedItem ?? "",
        txtPayGrade.Text, txtBiometricId.Text, chkGovDeductions.Checked,
        txtSssNo.Text, txtPhilHealthNo.Text, txtPagIbigNo.Text, txtEmployeeTin.Text,
        numDailyRate.Value, DateOnly.FromDateTime(dtpRateEffective.Value));

    async void btnSaveEmployee_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (EmployeeService.CheckInput(input) is { } problem) { lblEmployeeFormMsg.Text = problem; return; }

        btnSaveEmployee.Enabled = false;
        try
        {
            var (newId, error) = await EmployeeService.CreateAsync(input);
            if (error != null) { lblEmployeeFormMsg.Text = error; return; }
            NewEmployeeId = newId;
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { lblEmployeeFormMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveEmployee.Enabled = true; }
    }
}
