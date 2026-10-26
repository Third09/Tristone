namespace Company.OfficeApp.Pages
{
    partial class HrPayrollPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            tabHrPayroll = new TabControl();
            tabEmployees = new TabPage();
            tabAttendance = new TabPage();
            tabPayroll = new TabPage();
            tabPayslipHistory = new TabPage();
            tabCashAdvances = new TabPage();
            tabHrSettings = new TabPage();
            tabHrEditHistory = new TabPage();
            tabHrPayroll.SuspendLayout();
            SuspendLayout();
            //
            // tabHrPayroll
            //
            tabHrPayroll.Controls.Add(tabEmployees);
            tabHrPayroll.Controls.Add(tabAttendance);
            tabHrPayroll.Controls.Add(tabPayroll);
            tabHrPayroll.Controls.Add(tabPayslipHistory);
            tabHrPayroll.Controls.Add(tabCashAdvances);
            tabHrPayroll.Controls.Add(tabHrSettings);
            tabHrPayroll.Controls.Add(tabHrEditHistory);
            tabHrPayroll.Dock = DockStyle.Fill;
            tabHrPayroll.Name = "tabHrPayroll";
            tabHrPayroll.SelectedIndex = 0;
            tabHrPayroll.TabIndex = 0;
            //
            // tabEmployees
            //
            tabEmployees.Name = "tabEmployees";
            tabEmployees.Text = "Employees";
            //
            // tabAttendance
            //
            tabAttendance.Name = "tabAttendance";
            tabAttendance.Text = "Attendance";
            //
            // tabPayroll
            //
            tabPayroll.Name = "tabPayroll";
            tabPayroll.Text = "Payroll";
            //
            // tabPayslipHistory
            //
            tabPayslipHistory.Name = "tabPayslipHistory";
            tabPayslipHistory.Text = "Payslip History";
            //
            // tabCashAdvances
            //
            tabCashAdvances.Name = "tabCashAdvances";
            tabCashAdvances.Text = "Cash Advances";
            //
            // tabHrSettings
            //
            tabHrSettings.Name = "tabHrSettings";
            tabHrSettings.Text = "Settings";
            //
            // tabHrEditHistory
            //
            tabHrEditHistory.Name = "tabHrEditHistory";
            tabHrEditHistory.Text = "Edit History";
            //
            // HrPayrollPage
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabHrPayroll);
            Name = "HrPayrollPage";
            Size = new Size(980, 760);
            tabHrPayroll.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabHrPayroll;
        private TabPage tabEmployees;
        private TabPage tabAttendance;
        private TabPage tabPayroll;
        private TabPage tabPayslipHistory;
        private TabPage tabCashAdvances;
        private TabPage tabHrSettings;
        private TabPage tabHrEditHistory;
    }
}
