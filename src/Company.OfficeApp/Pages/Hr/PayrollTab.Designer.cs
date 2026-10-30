namespace Company.OfficeApp.Pages.Hr
{
    partial class PayrollTab
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
            lblPayGroup = new Label();
            cboPayGroup = new ComboBox();
            lblPayPeriod = new Label();
            cboPayPeriod = new ComboBox();
            lblPayrollStatus = new Label();
            lblPayrollWarnings = new Label();
            btnComputePayroll = new Button();
            btnFinalizePayroll = new Button();
            dgvPayroll = new DataGridView();
            lblPayrollTotal = new Label();
            btnPrintPayslips = new Button();
            btnSavePayslipsPdf = new Button();
            btnExportPayroll = new Button();
            lblApproverNote = new Label();
            lblPayrollMsg = new Label();
            colPayName = new DataGridViewTextBoxColumn();
            colPayDays = new DataGridViewTextBoxColumn();
            colPayGross = new DataGridViewTextBoxColumn();
            colPayLate = new DataGridViewTextBoxColumn();
            colPayOt = new DataGridViewTextBoxColumn();
            colPayCa = new DataGridViewTextBoxColumn();
            colPayGov = new DataGridViewTextBoxColumn();
            colPayNet = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvPayroll).BeginInit();
            SuspendLayout();
            // 
            // lblPayGroup
            // 
            lblPayGroup.AutoSize = true;
            lblPayGroup.Location = new Point(12, 15);
            lblPayGroup.Name = "lblPayGroup";
            lblPayGroup.Text = "Pay group";
            // 
            // cboPayGroup
            // 
            cboPayGroup.Location = new Point(90, 12);
            cboPayGroup.Name = "cboPayGroup";
            cboPayGroup.Size = new Size(150, 23);
            cboPayGroup.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPayGroup.TabIndex = 0;
            cboPayGroup.SelectedIndexChanged += cboPayGroup_SelectedIndexChanged;
            // 
            // lblPayPeriod
            // 
            lblPayPeriod.AutoSize = true;
            lblPayPeriod.Location = new Point(252, 15);
            lblPayPeriod.Name = "lblPayPeriod";
            lblPayPeriod.Text = "Pay period";
            // 
            // cboPayPeriod
            // 
            cboPayPeriod.Location = new Point(330, 12);
            cboPayPeriod.Name = "cboPayPeriod";
            cboPayPeriod.Size = new Size(330, 23);
            cboPayPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPayPeriod.TabIndex = 1;
            cboPayPeriod.SelectedIndexChanged += cboPayPeriod_SelectedIndexChanged;
            // 
            // lblPayrollStatus
            // 
            lblPayrollStatus.AutoSize = true;
            lblPayrollStatus.Location = new Point(672, 15);
            lblPayrollStatus.Name = "lblPayrollStatus";
            lblPayrollStatus.Text = "";
            // 
            // lblPayrollWarnings
            // 
            lblPayrollWarnings.AutoSize = true;
            lblPayrollWarnings.Location = new Point(12, 44);
            lblPayrollWarnings.Name = "lblPayrollWarnings";
            lblPayrollWarnings.Text = "";
            lblPayrollWarnings.MaximumSize = new Size(930, 0);
            // 
            // btnComputePayroll
            // 
            btnComputePayroll.Location = new Point(12, 100);
            btnComputePayroll.Name = "btnComputePayroll";
            btnComputePayroll.Size = new Size(130, 28);
            btnComputePayroll.Text = "Compute Payroll";
            btnComputePayroll.TabIndex = 2;
            btnComputePayroll.Click += btnComputePayroll_Click;
            // 
            // btnFinalizePayroll
            // 
            btnFinalizePayroll.Location = new Point(148, 100);
            btnFinalizePayroll.Name = "btnFinalizePayroll";
            btnFinalizePayroll.Size = new Size(130, 28);
            btnFinalizePayroll.Text = "Finalize Payroll";
            btnFinalizePayroll.TabIndex = 3;
            btnFinalizePayroll.Click += btnFinalizePayroll_Click;
            // 
            // dgvPayroll
            // 
            dgvPayroll.AllowUserToAddRows = false;
            dgvPayroll.AllowUserToDeleteRows = false;
            dgvPayroll.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayroll.Columns.AddRange(new DataGridViewColumn[] { colPayName, colPayDays, colPayGross, colPayLate, colPayOt, colPayCa, colPayGov, colPayNet });
            dgvPayroll.MultiSelect = false;
            dgvPayroll.ReadOnly = true;
            dgvPayroll.RowHeadersVisible = false;
            dgvPayroll.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayroll.Location = new Point(12, 136);
            dgvPayroll.Name = "dgvPayroll";
            dgvPayroll.Size = new Size(930, 330);
            dgvPayroll.TabIndex = 4;
            dgvPayroll.CellDoubleClick += dgvPayroll_CellDoubleClick;
            // 
            // lblPayrollTotal
            // 
            lblPayrollTotal.AutoSize = true;
            lblPayrollTotal.Location = new Point(12, 474);
            lblPayrollTotal.Name = "lblPayrollTotal";
            lblPayrollTotal.Text = "";
            // 
            // btnPrintPayslips
            // 
            btnPrintPayslips.Location = new Point(12, 498);
            btnPrintPayslips.Name = "btnPrintPayslips";
            btnPrintPayslips.Size = new Size(120, 28);
            btnPrintPayslips.Text = "Print Payslips";
            btnPrintPayslips.TabIndex = 5;
            btnPrintPayslips.Click += btnPrintPayslips_Click;
            // 
            // btnSavePayslipsPdf
            // 
            btnSavePayslipsPdf.Location = new Point(138, 498);
            btnSavePayslipsPdf.Name = "btnSavePayslipsPdf";
            btnSavePayslipsPdf.Size = new Size(150, 28);
            btnSavePayslipsPdf.Text = "Save Payslips as PDF";
            btnSavePayslipsPdf.TabIndex = 6;
            btnSavePayslipsPdf.Click += btnSavePayslipsPdf_Click;
            // 
            // btnExportPayroll
            // 
            btnExportPayroll.Location = new Point(294, 498);
            btnExportPayroll.Name = "btnExportPayroll";
            btnExportPayroll.Size = new Size(80, 28);
            btnExportPayroll.Text = "Export";
            btnExportPayroll.TabIndex = 7;
            btnExportPayroll.Click += btnExportPayroll_Click;
            // 
            // lblApproverNote
            // 
            lblApproverNote.AutoSize = true;
            lblApproverNote.Location = new Point(12, 534);
            lblApproverNote.Name = "lblApproverNote";
            lblApproverNote.Text = "The HR Supervisor or an Admin finalizes payroll.";
            // 
            // lblPayrollMsg
            // 
            lblPayrollMsg.AutoSize = true;
            lblPayrollMsg.Location = new Point(12, 558);
            lblPayrollMsg.Name = "lblPayrollMsg";
            lblPayrollMsg.MaximumSize = new Size(930, 0);
            // 
            // colPayName
            // 
            colPayName.DataPropertyName = "Name";
            colPayName.HeaderText = "Name";
            colPayName.Name = "colPayName";
            // 
            // colPayDays
            // 
            colPayDays.DataPropertyName = "DaysPaid";
            colPayDays.HeaderText = "Days Paid";
            colPayDays.Name = "colPayDays";
            colPayDays.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "0.#" };
            // 
            // colPayGross
            // 
            colPayGross.DataPropertyName = "GrossPay";
            colPayGross.HeaderText = "Gross Pay";
            colPayGross.Name = "colPayGross";
            colPayGross.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colPayLate
            // 
            colPayLate.DataPropertyName = "LateDeduction";
            colPayLate.HeaderText = "Late Deduction";
            colPayLate.Name = "colPayLate";
            colPayLate.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colPayOt
            // 
            colPayOt.DataPropertyName = "OtPay";
            colPayOt.HeaderText = "Overtime Pay";
            colPayOt.Name = "colPayOt";
            colPayOt.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colPayCa
            // 
            colPayCa.DataPropertyName = "CashAdvance";
            colPayCa.HeaderText = "Cash Advance";
            colPayCa.Name = "colPayCa";
            colPayCa.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colPayGov
            // 
            colPayGov.DataPropertyName = "GovDeductions";
            colPayGov.HeaderText = "Gov. Deductions";
            colPayGov.Name = "colPayGov";
            colPayGov.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colPayNet
            // 
            colPayNet.DataPropertyName = "NetPay";
            colPayNet.HeaderText = "Net Pay";
            colPayNet.Name = "colPayNet";
            colPayNet.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // PayrollTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblPayGroup);
            Controls.Add(cboPayGroup);
            Controls.Add(lblPayPeriod);
            Controls.Add(cboPayPeriod);
            Controls.Add(lblPayrollStatus);
            Controls.Add(lblPayrollWarnings);
            Controls.Add(btnComputePayroll);
            Controls.Add(btnFinalizePayroll);
            Controls.Add(dgvPayroll);
            Controls.Add(lblPayrollTotal);
            Controls.Add(btnPrintPayslips);
            Controls.Add(btnSavePayslipsPdf);
            Controls.Add(btnExportPayroll);
            Controls.Add(lblApproverNote);
            Controls.Add(lblPayrollMsg);
            Name = "PayrollTab";
            Size = new Size(960, 600);
            Load += PayrollTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPayroll).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPayGroup;
        private ComboBox cboPayGroup;
        private Label lblPayPeriod;
        private ComboBox cboPayPeriod;
        private Label lblPayrollStatus;
        private Label lblPayrollWarnings;
        private Button btnComputePayroll;
        private Button btnFinalizePayroll;
        private DataGridView dgvPayroll;
        private Label lblPayrollTotal;
        private Button btnPrintPayslips;
        private Button btnSavePayslipsPdf;
        private Button btnExportPayroll;
        private Label lblApproverNote;
        private Label lblPayrollMsg;
        private DataGridViewTextBoxColumn colPayName;
        private DataGridViewTextBoxColumn colPayDays;
        private DataGridViewTextBoxColumn colPayGross;
        private DataGridViewTextBoxColumn colPayLate;
        private DataGridViewTextBoxColumn colPayOt;
        private DataGridViewTextBoxColumn colPayCa;
        private DataGridViewTextBoxColumn colPayGov;
        private DataGridViewTextBoxColumn colPayNet;
    }
}
