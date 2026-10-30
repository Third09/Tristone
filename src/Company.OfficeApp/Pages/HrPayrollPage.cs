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

        // Tabs not built yet show a note.
        foreach (var tab in new[] { tabPayroll, tabPayslipHistory, tabCashAdvances, tabHrSettings })
            tab.Controls.Add(new Label { Text = $"{tab.Text}: not built yet.", AutoSize = true, Location = new Point(12, 12) });

        var me = Session.CurrentUser!;
        if (!me.IsAdmin && me.Level != UserLevels.Supervisor)
        {
            tabHrPayroll.TabPages.Remove(tabHrSettings);
            tabHrPayroll.TabPages.Remove(tabHrEditHistory);
        }
    }
}
