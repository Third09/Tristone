using Company.OfficeApp.Services;

namespace Company.OfficeApp.Pages.Hr;

/// <summary>
/// New cash advance (Cash Advances > New Cash Advance). Saved as Waiting until the HR Supervisor approves.
/// Plain layout for now; control names match the UI Elements Guide (Part 2: Tab: Cash Advances).
/// </summary>
public partial class CashAdvanceEntryForm : Form
{
    public CashAdvanceEntryForm()
    {
        InitializeComponent();
        Text = "New Cash Advance";
        AcceptButton = btnSaveCashAdvance;
        CancelButton = btnCancelCashAdvance;
        dtpCaDate.Value = DateTime.Today;
    }

    async void CashAdvanceEntryForm_Load(object sender, EventArgs e) =>
        cboCaEmployee.Items.AddRange([.. await CashAdvanceService.EmployeeOptionsAsync()]);

    async void btnSaveCashAdvance_Click(object sender, EventArgs e)
    {
        var input = new CashAdvanceInput((cboCaEmployee.SelectedItem as PersonOption)?.EmployeeId ?? 0, numCaAmount.Value, numCaPerPayday.Value,
            DateOnly.FromDateTime(dtpCaDate.Value), txtCaReason.Text);
        if (CashAdvanceService.CheckInput(input) is { } problem) { lblCaEntryMsg.Text = problem; return; }
        btnSaveCashAdvance.Enabled = false;
        try
        {
            var (_, error) = await CashAdvanceService.CreateAsync(input);
            if (error != null) { lblCaEntryMsg.Text = error; return; }
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { lblCaEntryMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveCashAdvance.Enabled = true; }
    }
}
