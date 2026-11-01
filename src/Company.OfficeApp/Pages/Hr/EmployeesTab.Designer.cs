namespace Company.OfficeApp.Pages.Hr
{
    partial class EmployeesTab
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
            lblSearch = new Label();
            txtEmployeeSearch = new TextBox();
            lblStatusFilter = new Label();
            cboEmployeeStatusFilter = new ComboBox();
            lblScheduleFilter = new Label();
            cboPayScheduleFilter = new ComboBox();
            dgvEmployees = new DataGridView();
            btnAddEmployee = new Button();
            btnMarkInactive = new Button();
            btnReactivateEmployee = new Button();
            btnExportEmployees = new Button();
            lblFormMode = new Label();
            lblEmployeeName = new Label();
            txtEmployeeName = new TextBox();
            lblEmployeeContact = new Label();
            txtEmployeeContact = new TextBox();
            lblEmployeeAddress = new Label();
            txtEmployeeAddress = new TextBox();
            lblEmployeePosition = new Label();
            txtEmployeePosition = new TextBox();
            lblEmployeeDepartment = new Label();
            cboEmployeeDepartment = new ComboBox();
            lblPayScheduleValue = new Label();
            lblPayGrade = new Label();
            txtPayGrade = new TextBox();
            lblDailyRate = new Label();
            numDailyRate = new NumericUpDown();
            lblRateEffective = new Label();
            dtpRateEffective = new DateTimePicker();
            lblBiometricId = new Label();
            txtBiometricId = new TextBox();
            lblRateHistory = new Label();
            dgvRateHistory = new DataGridView();
            chkGovDeductions = new CheckBox();
            lblSssNo = new Label();
            txtSssNo = new TextBox();
            lblPhilHealthNo = new Label();
            txtPhilHealthNo = new TextBox();
            lblPagIbigNo = new Label();
            txtPagIbigNo = new TextBox();
            lblEmployeeTin = new Label();
            txtEmployeeTin = new TextBox();
            lblEmployeeFormMsg = new Label();
            btnSaveEmployee = new Button();
            btnCancelEmployee = new Button();
            colName = new DataGridViewTextBoxColumn();
            colPosition = new DataGridViewTextBoxColumn();
            colDepartment = new DataGridViewTextBoxColumn();
            colPaySchedule = new DataGridViewTextBoxColumn();
            colCurrentRate = new DataGridViewTextBoxColumn();
            colBiometricId = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colRateFrom = new DataGridViewTextBoxColumn();
            colRateAmount = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDailyRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvRateHistory).BeginInit();
            SuspendLayout();
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(38, 36);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search";
            // 
            // txtEmployeeSearch
            // 
            txtEmployeeSearch.Location = new Point(95, 32);
            txtEmployeeSearch.Name = "txtEmployeeSearch";
            txtEmployeeSearch.Size = new Size(207, 23);
            txtEmployeeSearch.TabIndex = 0;
            txtEmployeeSearch.TextChanged += Filter_Changed;
            // 
            // lblStatusFilter
            // 
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(318, 36);
            lblStatusFilter.Name = "lblStatusFilter";
            lblStatusFilter.Text = "Status";
            // 
            // cboEmployeeStatusFilter
            // 
            cboEmployeeStatusFilter.Location = new Point(371, 32);
            cboEmployeeStatusFilter.Name = "cboEmployeeStatusFilter";
            cboEmployeeStatusFilter.Size = new Size(115, 23);
            cboEmployeeStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEmployeeStatusFilter.TabIndex = 1;
            cboEmployeeStatusFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblScheduleFilter
            // 
            lblScheduleFilter.AutoSize = true;
            lblScheduleFilter.Location = new Point(502, 36);
            lblScheduleFilter.Name = "lblScheduleFilter";
            lblScheduleFilter.Text = "Pay schedule";
            // 
            // cboPayScheduleFilter
            // 
            cboPayScheduleFilter.Location = new Point(599, 32);
            cboPayScheduleFilter.Name = "cboPayScheduleFilter";
            cboPayScheduleFilter.Size = new Size(115, 23);
            cboPayScheduleFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPayScheduleFilter.TabIndex = 2;
            cboPayScheduleFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // dgvEmployees
            // 
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.AllowUserToDeleteRows = false;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.Columns.AddRange(new DataGridViewColumn[] { colName, colPosition, colDepartment, colPaySchedule, colCurrentRate, colBiometricId, colStatus });
            dgvEmployees.MultiSelect = false;
            dgvEmployees.ReadOnly = true;
            dgvEmployees.RowHeadersVisible = false;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.Location = new Point(38, 74);
            dgvEmployees.Name = "dgvEmployees";
            dgvEmployees.Size = new Size(1070, 264);
            dgvEmployees.TabIndex = 3;
            dgvEmployees.SelectionChanged += dgvEmployees_SelectionChanged;
            // 
            // btnAddEmployee
            // 
            btnAddEmployee.Location = new Point(38, 349);
            btnAddEmployee.Name = "btnAddEmployee";
            btnAddEmployee.Size = new Size(126, 36);
            btnAddEmployee.Text = "Add Employee";
            btnAddEmployee.TabIndex = 4;
            btnAddEmployee.Click += btnAddEmployee_Click;
            // 
            // btnMarkInactive
            // 
            btnMarkInactive.Location = new Point(171, 349);
            btnMarkInactive.Name = "btnMarkInactive";
            btnMarkInactive.Size = new Size(126, 36);
            btnMarkInactive.Text = "Mark Inactive";
            btnMarkInactive.TabIndex = 5;
            btnMarkInactive.Click += btnMarkInactive_Click;
            // 
            // btnReactivateEmployee
            // 
            btnReactivateEmployee.Location = new Point(305, 349);
            btnReactivateEmployee.Name = "btnReactivateEmployee";
            btnReactivateEmployee.Size = new Size(115, 36);
            btnReactivateEmployee.Text = "Reactivate";
            btnReactivateEmployee.TabIndex = 6;
            btnReactivateEmployee.Click += btnReactivateEmployee_Click;
            // 
            // btnExportEmployees
            // 
            btnExportEmployees.Location = new Point(426, 349);
            btnExportEmployees.Name = "btnExportEmployees";
            btnExportEmployees.Size = new Size(103, 36);
            btnExportEmployees.Text = "Export";
            btnExportEmployees.TabIndex = 7;
            btnExportEmployees.Click += btnExportEmployees_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(38, 407);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New employee";
            lblFormMode.Font = new Font("Segoe UI Semibold", 11.5F);
            // 
            // lblEmployeeName
            // 
            lblEmployeeName.AutoSize = true;
            lblEmployeeName.Location = new Point(38, 438);
            lblEmployeeName.Name = "lblEmployeeName";
            lblEmployeeName.Text = "Full name";
            // 
            // txtEmployeeName
            // 
            txtEmployeeName.Location = new Point(38, 462);
            txtEmployeeName.Name = "txtEmployeeName";
            txtEmployeeName.Size = new Size(345, 23);
            txtEmployeeName.TabIndex = 8;
            // 
            // lblEmployeeContact
            // 
            lblEmployeeContact.AutoSize = true;
            lblEmployeeContact.Location = new Point(38, 502);
            lblEmployeeContact.Name = "lblEmployeeContact";
            lblEmployeeContact.Text = "Contact number";
            // 
            // txtEmployeeContact
            // 
            txtEmployeeContact.Location = new Point(38, 526);
            txtEmployeeContact.Name = "txtEmployeeContact";
            txtEmployeeContact.Size = new Size(345, 23);
            txtEmployeeContact.TabIndex = 9;
            // 
            // lblEmployeeAddress
            // 
            lblEmployeeAddress.AutoSize = true;
            lblEmployeeAddress.Location = new Point(38, 565);
            lblEmployeeAddress.Name = "lblEmployeeAddress";
            lblEmployeeAddress.Text = "Address";
            // 
            // txtEmployeeAddress
            // 
            txtEmployeeAddress.Location = new Point(38, 589);
            txtEmployeeAddress.Name = "txtEmployeeAddress";
            txtEmployeeAddress.Size = new Size(345, 61);
            txtEmployeeAddress.Multiline = true;
            txtEmployeeAddress.TabIndex = 10;
            // 
            // lblEmployeePosition
            // 
            lblEmployeePosition.AutoSize = true;
            lblEmployeePosition.Location = new Point(38, 660);
            lblEmployeePosition.Name = "lblEmployeePosition";
            lblEmployeePosition.Text = "Position (e.g. Mason, HR Staff)";
            // 
            // txtEmployeePosition
            // 
            txtEmployeePosition.Location = new Point(38, 684);
            txtEmployeePosition.Name = "txtEmployeePosition";
            txtEmployeePosition.Size = new Size(345, 23);
            txtEmployeePosition.TabIndex = 11;
            // 
            // lblEmployeeDepartment
            // 
            lblEmployeeDepartment.AutoSize = true;
            lblEmployeeDepartment.Location = new Point(38, 724);
            lblEmployeeDepartment.Name = "lblEmployeeDepartment";
            lblEmployeeDepartment.Text = "Department (Site for site workers)";
            // 
            // cboEmployeeDepartment
            // 
            cboEmployeeDepartment.Location = new Point(38, 747);
            cboEmployeeDepartment.Name = "cboEmployeeDepartment";
            cboEmployeeDepartment.Size = new Size(230, 23);
            cboEmployeeDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEmployeeDepartment.TabIndex = 12;
            cboEmployeeDepartment.SelectedIndexChanged += cboEmployeeDepartment_SelectedIndexChanged;
            // 
            // lblPayScheduleValue
            // 
            lblPayScheduleValue.AutoSize = true;
            lblPayScheduleValue.Location = new Point(38, 787);
            lblPayScheduleValue.Name = "lblPayScheduleValue";
            lblPayScheduleValue.Text = "Pay schedule: (choose a department)";
            // 
            // lblPayGrade
            // 
            lblPayGrade.AutoSize = true;
            lblPayGrade.Location = new Point(415, 438);
            lblPayGrade.Name = "lblPayGrade";
            lblPayGrade.Text = "Pay grade (optional)";
            // 
            // txtPayGrade
            // 
            txtPayGrade.Location = new Point(415, 462);
            txtPayGrade.Name = "txtPayGrade";
            txtPayGrade.Size = new Size(230, 23);
            txtPayGrade.TabIndex = 13;
            // 
            // lblDailyRate
            // 
            lblDailyRate.AutoSize = true;
            lblDailyRate.Location = new Point(415, 502);
            lblDailyRate.Name = "lblDailyRate";
            lblDailyRate.Text = "Daily rate (pesos)";
            // 
            // numDailyRate
            // 
            numDailyRate.Location = new Point(415, 526);
            numDailyRate.Name = "numDailyRate";
            numDailyRate.Size = new Size(161, 23);
            numDailyRate.DecimalPlaces = 2;
            numDailyRate.ThousandsSeparator = true;
            numDailyRate.TabIndex = 14;
            // 
            // lblRateEffective
            // 
            lblRateEffective.AutoSize = true;
            lblRateEffective.Location = new Point(415, 565);
            lblRateEffective.Name = "lblRateEffective";
            lblRateEffective.Text = "Rate applies from";
            // 
            // dtpRateEffective
            // 
            dtpRateEffective.Location = new Point(415, 589);
            dtpRateEffective.Name = "dtpRateEffective";
            dtpRateEffective.Size = new Size(161, 23);
            dtpRateEffective.Format = DateTimePickerFormat.Short;
            dtpRateEffective.TabIndex = 15;
            // 
            // lblBiometricId
            // 
            lblBiometricId.AutoSize = true;
            lblBiometricId.Location = new Point(415, 628);
            lblBiometricId.Name = "lblBiometricId";
            lblBiometricId.Text = "Biometric ID (from the scanner)";
            // 
            // txtBiometricId
            // 
            txtBiometricId.Location = new Point(415, 652);
            txtBiometricId.Name = "txtBiometricId";
            txtBiometricId.Size = new Size(161, 23);
            txtBiometricId.TabIndex = 16;
            // 
            // lblRateHistory
            // 
            lblRateHistory.AutoSize = true;
            lblRateHistory.Location = new Point(415, 692);
            lblRateHistory.Name = "lblRateHistory";
            lblRateHistory.Text = "Daily rate history";
            // 
            // dgvRateHistory
            // 
            dgvRateHistory.AllowUserToAddRows = false;
            dgvRateHistory.AllowUserToDeleteRows = false;
            dgvRateHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRateHistory.Columns.AddRange(new DataGridViewColumn[] { colRateFrom, colRateAmount });
            dgvRateHistory.MultiSelect = false;
            dgvRateHistory.ReadOnly = true;
            dgvRateHistory.RowHeadersVisible = false;
            dgvRateHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRateHistory.Location = new Point(415, 716);
            dgvRateHistory.Name = "dgvRateHistory";
            dgvRateHistory.Size = new Size(322, 119);
            dgvRateHistory.TabIndex = 17;
            // 
            // chkGovDeductions
            // 
            chkGovDeductions.AutoSize = true;
            chkGovDeductions.Location = new Point(771, 438);
            chkGovDeductions.Name = "chkGovDeductions";
            chkGovDeductions.Text = "Government deductions (tax, SSS, PhilHealth, Pag-IBIG)";
            chkGovDeductions.TabIndex = 18;
            // 
            // lblSssNo
            // 
            lblSssNo.AutoSize = true;
            lblSssNo.Location = new Point(771, 475);
            lblSssNo.Name = "lblSssNo";
            lblSssNo.Text = "SSS number (optional)";
            // 
            // txtSssNo
            // 
            txtSssNo.Location = new Point(771, 499);
            txtSssNo.Name = "txtSssNo";
            txtSssNo.Size = new Size(230, 23);
            txtSssNo.TabIndex = 19;
            // 
            // lblPhilHealthNo
            // 
            lblPhilHealthNo.AutoSize = true;
            lblPhilHealthNo.Location = new Point(771, 539);
            lblPhilHealthNo.Name = "lblPhilHealthNo";
            lblPhilHealthNo.Text = "PhilHealth number (optional)";
            // 
            // txtPhilHealthNo
            // 
            txtPhilHealthNo.Location = new Point(771, 562);
            txtPhilHealthNo.Name = "txtPhilHealthNo";
            txtPhilHealthNo.Size = new Size(230, 23);
            txtPhilHealthNo.TabIndex = 20;
            // 
            // lblPagIbigNo
            // 
            lblPagIbigNo.AutoSize = true;
            lblPagIbigNo.Location = new Point(771, 602);
            lblPagIbigNo.Name = "lblPagIbigNo";
            lblPagIbigNo.Text = "Pag-IBIG number (optional)";
            // 
            // txtPagIbigNo
            // 
            txtPagIbigNo.Location = new Point(771, 626);
            txtPagIbigNo.Name = "txtPagIbigNo";
            txtPagIbigNo.Size = new Size(230, 23);
            txtPagIbigNo.TabIndex = 21;
            // 
            // lblEmployeeTin
            // 
            lblEmployeeTin.AutoSize = true;
            lblEmployeeTin.Location = new Point(771, 665);
            lblEmployeeTin.Name = "lblEmployeeTin";
            lblEmployeeTin.Text = "TIN (optional)";
            // 
            // txtEmployeeTin
            // 
            txtEmployeeTin.Location = new Point(771, 689);
            txtEmployeeTin.Name = "txtEmployeeTin";
            txtEmployeeTin.Size = new Size(230, 23);
            txtEmployeeTin.TabIndex = 22;
            // 
            // lblEmployeeFormMsg
            // 
            lblEmployeeFormMsg.AutoSize = true;
            lblEmployeeFormMsg.Location = new Point(38, 853);
            lblEmployeeFormMsg.Name = "lblEmployeeFormMsg";
            // 
            // btnSaveEmployee
            // 
            btnSaveEmployee.Location = new Point(38, 882);
            btnSaveEmployee.Name = "btnSaveEmployee";
            btnSaveEmployee.Size = new Size(103, 36);
            btnSaveEmployee.Text = "Save";
            btnSaveEmployee.TabIndex = 23;
            btnSaveEmployee.Click += btnSaveEmployee_Click;
            // 
            // btnCancelEmployee
            // 
            btnCancelEmployee.Location = new Point(148, 882);
            btnCancelEmployee.Name = "btnCancelEmployee";
            btnCancelEmployee.Size = new Size(103, 36);
            btnCancelEmployee.Text = "Cancel";
            btnCancelEmployee.TabIndex = 24;
            btnCancelEmployee.Click += btnCancelEmployee_Click;
            // 
            // colName
            // 
            colName.DataPropertyName = "FullName";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            // 
            // colPosition
            // 
            colPosition.DataPropertyName = "Position";
            colPosition.HeaderText = "Position";
            colPosition.Name = "colPosition";
            // 
            // colDepartment
            // 
            colDepartment.DataPropertyName = "Department";
            colDepartment.HeaderText = "Department";
            colDepartment.Name = "colDepartment";
            // 
            // colPaySchedule
            // 
            colPaySchedule.DataPropertyName = "PaySchedule";
            colPaySchedule.HeaderText = "Pay Schedule";
            colPaySchedule.Name = "colPaySchedule";
            // 
            // colCurrentRate
            // 
            colCurrentRate.DataPropertyName = "CurrentRate";
            colCurrentRate.HeaderText = "Daily Rate";
            colCurrentRate.Name = "colCurrentRate";
            colCurrentRate.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2", NullValue = "-" };
            // 
            // colBiometricId
            // 
            colBiometricId.DataPropertyName = "BiometricId";
            colBiometricId.HeaderText = "Biometric ID";
            colBiometricId.Name = "colBiometricId";
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            // 
            // colRateFrom
            // 
            colRateFrom.DataPropertyName = "EffectiveFrom";
            colRateFrom.HeaderText = "Applies From";
            colRateFrom.Name = "colRateFrom";
            colRateFrom.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colRateAmount
            // 
            colRateAmount.DataPropertyName = "DailyRate";
            colRateAmount.HeaderText = "Daily Rate";
            colRateAmount.Name = "colRateAmount";
            colRateAmount.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2", NullValue = "-" };
            // 
            // EmployeesTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblSearch);
            Controls.Add(txtEmployeeSearch);
            Controls.Add(lblStatusFilter);
            Controls.Add(cboEmployeeStatusFilter);
            Controls.Add(lblScheduleFilter);
            Controls.Add(cboPayScheduleFilter);
            Controls.Add(dgvEmployees);
            Controls.Add(btnAddEmployee);
            Controls.Add(btnMarkInactive);
            Controls.Add(btnReactivateEmployee);
            Controls.Add(btnExportEmployees);
            Controls.Add(lblFormMode);
            Controls.Add(lblEmployeeName);
            Controls.Add(txtEmployeeName);
            Controls.Add(lblEmployeeContact);
            Controls.Add(txtEmployeeContact);
            Controls.Add(lblEmployeeAddress);
            Controls.Add(txtEmployeeAddress);
            Controls.Add(lblEmployeePosition);
            Controls.Add(txtEmployeePosition);
            Controls.Add(lblEmployeeDepartment);
            Controls.Add(cboEmployeeDepartment);
            Controls.Add(lblPayScheduleValue);
            Controls.Add(lblPayGrade);
            Controls.Add(txtPayGrade);
            Controls.Add(lblDailyRate);
            Controls.Add(numDailyRate);
            Controls.Add(lblRateEffective);
            Controls.Add(dtpRateEffective);
            Controls.Add(lblBiometricId);
            Controls.Add(txtBiometricId);
            Controls.Add(lblRateHistory);
            Controls.Add(dgvRateHistory);
            Controls.Add(chkGovDeductions);
            Controls.Add(lblSssNo);
            Controls.Add(txtSssNo);
            Controls.Add(lblPhilHealthNo);
            Controls.Add(txtPhilHealthNo);
            Controls.Add(lblPagIbigNo);
            Controls.Add(txtPagIbigNo);
            Controls.Add(lblEmployeeTin);
            Controls.Add(txtEmployeeTin);
            Controls.Add(lblEmployeeFormMsg);
            Controls.Add(btnSaveEmployee);
            Controls.Add(btnCancelEmployee);
            Name = "EmployeesTab";
            Size = new Size(1152, 956);
            Load += EmployeesTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDailyRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvRateHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSearch;
        private TextBox txtEmployeeSearch;
        private Label lblStatusFilter;
        private ComboBox cboEmployeeStatusFilter;
        private Label lblScheduleFilter;
        private ComboBox cboPayScheduleFilter;
        private DataGridView dgvEmployees;
        private Button btnAddEmployee;
        private Button btnMarkInactive;
        private Button btnReactivateEmployee;
        private Button btnExportEmployees;
        private Label lblFormMode;
        private Label lblEmployeeName;
        private TextBox txtEmployeeName;
        private Label lblEmployeeContact;
        private TextBox txtEmployeeContact;
        private Label lblEmployeeAddress;
        private TextBox txtEmployeeAddress;
        private Label lblEmployeePosition;
        private TextBox txtEmployeePosition;
        private Label lblEmployeeDepartment;
        private ComboBox cboEmployeeDepartment;
        private Label lblPayScheduleValue;
        private Label lblPayGrade;
        private TextBox txtPayGrade;
        private Label lblDailyRate;
        private NumericUpDown numDailyRate;
        private Label lblRateEffective;
        private DateTimePicker dtpRateEffective;
        private Label lblBiometricId;
        private TextBox txtBiometricId;
        private Label lblRateHistory;
        private DataGridView dgvRateHistory;
        private CheckBox chkGovDeductions;
        private Label lblSssNo;
        private TextBox txtSssNo;
        private Label lblPhilHealthNo;
        private TextBox txtPhilHealthNo;
        private Label lblPagIbigNo;
        private TextBox txtPagIbigNo;
        private Label lblEmployeeTin;
        private TextBox txtEmployeeTin;
        private Label lblEmployeeFormMsg;
        private Button btnSaveEmployee;
        private Button btnCancelEmployee;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colPosition;
        private DataGridViewTextBoxColumn colDepartment;
        private DataGridViewTextBoxColumn colPaySchedule;
        private DataGridViewTextBoxColumn colCurrentRate;
        private DataGridViewTextBoxColumn colBiometricId;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colRateFrom;
        private DataGridViewTextBoxColumn colRateAmount;
    }
}
