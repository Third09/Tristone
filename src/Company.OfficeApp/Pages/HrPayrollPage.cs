using Company.OfficeApp.Pages.Hr;
using Company.OfficeApp.Pages.Shared;
using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages;

/// <summary>
/// HR & Payroll (Admin and HR department). 7 tabs; each tab's content is its own UserControl in Pages/Hr.
/// Settings and Edit History are for Admins and the HR Supervisor only.
/// Plain layout for now; control names match the UI Elements Guide (Part 2).
/// </summary>
public partial class HrPayrollPage : UserControl
{
    public HrPayrollPage()
    {
        InitializeComponent();

        tabEmployees.Controls.Add(new EmployeesTab { Dock = DockStyle.Fill });
        tabAttendance.Controls.Add(new AttendanceTab { Dock = DockStyle.Fill });
        tabHrEditHistory.Controls.Add(new EditHistoryTab(Departments.HrPayroll) { Dock = DockStyle.Fill });

        var payslips = new PayslipHistoryTab { Dock = DockStyle.Fill };
        tabPayroll.Controls.Add(new PayrollTab { Dock = DockStyle.Fill });
        tabPayslipHistory.Controls.Add(payslips);
        tabCashAdvances.Controls.Add(new CashAdvancesTab { Dock = DockStyle.Fill });
        tabHrSettings.Controls.Add(new HrSettingsTab { Dock = DockStyle.Fill });
        // A payroll finalized on the Payroll tab shows up in Payslip History when that tab is opened.
        tabHrPayroll.SelectedIndexChanged += async (_, _) => { if (tabHrPayroll.SelectedTab == tabPayslipHistory && payslips.IsHandleCreated) await payslips.ReloadAsync(); };

        var me = Session.CurrentUser!;
        if (!me.IsAdmin && me.Level != UserLevels.Supervisor)
        {
            tabHrPayroll.TabPages.Remove(tabHrSettings);
            tabHrPayroll.TabPages.Remove(tabHrEditHistory);
        }
    }
}
