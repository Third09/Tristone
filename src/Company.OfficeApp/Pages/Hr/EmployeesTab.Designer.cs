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
            colName = new DataGridViewTextBoxColumn();
            colPosition = new DataGridViewTextBoxColumn();
            colDepartment = new DataGridViewTextBoxColumn();
            colPaySchedule = new DataGridViewTextBoxColumn();
            colCurrentRate = new DataGridViewTextBoxColumn();
            colBiometricId = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
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
            colRateFrom = new DataGridViewTextBoxColumn();
            colRateAmount = new DataGridViewTextBoxColumn();
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
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDailyRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvRateHistory).BeginInit();
            SuspendLayout();
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(12, 15);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(42, 15);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Search";
            // 
            // txtEmployeeSearch
            // 
            txtEmployeeSearch.Location = new Point(62, 12);
            txtEmployeeSearch.Name = "txtEmployeeSearch";
            txtEmployeeSearch.Size = new Size(180, 23);
            txtEmployeeSearch.TabIndex = 0;
            // 
            // lblStatusFilter
            // 
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(256, 15);
            lblStatusFilter.Name = "lblStatusFilter";
            lblStatusFilter.Size = new Size(39, 15);
            lblStatusFilter.TabIndex = 1;
            lblStatusFilter.Text = "Status";
            // 
            // cboEmployeeStatusFilter
            // 
            cboEmployeeStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEmployeeStatusFilter.Location = new Point(302, 12);
            cboEmployeeStatusFilter.Name = "cboEmployeeStatusFilter";
            cboEmployeeStatusFilter.Size = new Size(100, 23);
            cboEmployeeStatusFilter.TabIndex = 1;
            cboEmployeeStatusFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblScheduleFilter
            // 
            lblScheduleFilter.AutoSize = true;
            lblScheduleFilter.Location = new Point(416, 15);
            lblScheduleFilter.Name = "lblScheduleFilter";
            lblScheduleFilter.Size = new Size(76, 15);
            lblScheduleFilter.TabIndex = 2;
            lblScheduleFilter.Text = "Pay schedule";
            // 
            // cboPayScheduleFilter
            // 
            cboPayScheduleFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPayScheduleFilter.Location = new Point(500, 12);
            cboPayScheduleFilter.Name = "cboPayScheduleFilter";
            cboPayScheduleFilter.Size = new Size(100, 23);
            cboPayScheduleFilter.TabIndex = 2;
            cboPayScheduleFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // dgvEmployees
            // 
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.AllowUserToDeleteRows = false;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.Columns.AddRange(new DataGridViewColumn[] { colName, colPosition, colDepartment, colPaySchedule, colCurrentRate, colBiometricId, colStatus });
            dgvEmployees.Location = new Point(12, 44);
            dgvEmployees.MultiSelect = false;
            dgvEmployees.Name = "dgvEmployees";
            dgvEmployees.ReadOnly = true;
            dgvEmployees.RowHeadersVisible = false;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.Size = new Size(930, 200);
            dgvEmployees.TabIndex = 3;
            dgvEmployees.SelectionChanged += dgvEmployees_SelectionChanged;
            // 
            // colName
            // 
            colName.DataPropertyName = "FullName";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            colName.ReadOnly = true;
            // 
            // colPosition
            // 
            colPosition.DataPropertyName = "Position";
            colPosition.HeaderText = "Position";
            colPosition.Name = "colPosition";
            colPosition.ReadOnly = true;
            // 
            // colDepartment
            // 
            colDepartment.DataPropertyName = "Department";
            colDepartment.HeaderText = "Department";
            colDepartment.Name = "colDepartment";
            colDepartment.ReadOnly = true;
            // 
            // colPaySchedule
            // 
            colPaySchedule.DataPropertyName = "PaySchedule";
            colPaySchedule.HeaderText = "Pay Schedule";
            colPaySchedule.Name = "colPaySchedule";
            colPaySchedule.ReadOnly = true;
            // 
            // colCurrentRate
            // 
            colCurrentRate.DataPropertyName = "CurrentRate";
            colCurrentRate.HeaderText = "Daily Rate";
            colCurrentRate.Name = "colCurrentRate";
            colCurrentRate.ReadOnly = true;
            // 
            // colBiometricId
            // 
            colBiometricId.DataPropertyName = "BiometricId";
            colBiometricId.HeaderText = "Biometric ID";
            colBiometricId.Name = "colBiometricId";
            colBiometricId.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // btnAddEmployee
            // 
            btnAddEmployee.Location = new Point(1005, 726);
            btnAddEmployee.Name = "btnAddEmployee";
            btnAddEmployee.Size = new Size(110, 28);
            btnAddEmployee.TabIndex = 4;
            btnAddEmployee.Text = "Add Employee";
            btnAddEmployee.Click += btnAddEmployee_Click;
            // 
            // btnMarkInactive
            // 
            btnMarkInactive.Location = new Point(9, 690);
            btnMarkInactive.Name = "btnMarkInactive";
            btnMarkInactive.Size = new Size(110, 28);
            btnMarkInactive.TabIndex = 5;
            btnMarkInactive.Text = "Mark Inactive";
            btnMarkInactive.Click += btnMarkInactive_Click;
            // 
            // btnReactivateEmployee
            // 
            btnReactivateEmployee.Location = new Point(120, 690);
            btnReactivateEmployee.Name = "btnReactivateEmployee";
            btnReactivateEmployee.Size = new Size(110, 28);
            btnReactivateEmployee.TabIndex = 6;
            btnReactivateEmployee.Text = "Reactivate";
            btnReactivateEmployee.Click += btnReactivateEmployee_Click;
            // 
            // btnExportEmployees
            // 
            btnExportEmployees.Location = new Point(1116, 726);
            btnExportEmployees.Name = "btnExportEmployees";
            btnExportEmployees.Size = new Size(110, 28);
            btnExportEmployees.TabIndex = 7;
            btnExportEmployees.Text = "Export";
            btnExportEmployees.Click += btnExportEmployees_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(12, 296);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Size = new Size(106, 15);
            lblFormMode.TabIndex = 8;
            lblFormMode.Text = "Edit Employee Info";
            lblFormMode.Click += lblFormMode_Click;
            // 
            // lblEmployeeName
            // 
            lblEmployeeName.AutoSize = true;
            lblEmployeeName.Location = new Point(12, 320);
            lblEmployeeName.Name = "lblEmployeeName";
            lblEmployeeName.Size = new Size(59, 15);
            lblEmployeeName.TabIndex = 9;
            lblEmployeeName.Text = "Full name";
            // 
            // txtEmployeeName
            // 
            txtEmployeeName.Location = new Point(12, 338);
            txtEmployeeName.Name = "txtEmployeeName";
            txtEmployeeName.Size = new Size(300, 23);
            txtEmployeeName.TabIndex = 8;
            // 
            // lblEmployeeContact
            // 
            lblEmployeeContact.AutoSize = true;
            lblEmployeeContact.Location = new Point(12, 368);
            lblEmployeeContact.Name = "lblEmployeeContact";
            lblEmployeeContact.Size = new Size(94, 15);
            lblEmployeeContact.TabIndex = 10;
            lblEmployeeContact.Text = "Contact number";
            // 
            // txtEmployeeContact
            // 
            txtEmployeeContact.Location = new Point(12, 386);
            txtEmployeeContact.Name = "txtEmployeeContact";
            txtEmployeeContact.Size = new Size(300, 23);
            txtEmployeeContact.TabIndex = 9;
            // 
            // lblEmployeeAddress
            // 
            lblEmployeeAddress.AutoSize = true;
            lblEmployeeAddress.Location = new Point(12, 416);
            lblEmployeeAddress.Name = "lblEmployeeAddress";
            lblEmployeeAddress.Size = new Size(49, 15);
            lblEmployeeAddress.TabIndex = 11;
            lblEmployeeAddress.Text = "Address";
            // 
            // txtEmployeeAddress
            // 
            txtEmployeeAddress.Location = new Point(12, 434);
            txtEmployeeAddress.Multiline = true;
            txtEmployeeAddress.Name = "txtEmployeeAddress";
            txtEmployeeAddress.Size = new Size(300, 46);
            txtEmployeeAddress.TabIndex = 10;
            // 
            // lblEmployeePosition
            // 
            lblEmployeePosition.AutoSize = true;
            lblEmployeePosition.Location = new Point(12, 488);
            lblEmployeePosition.Name = "lblEmployeePosition";
            lblEmployeePosition.Size = new Size(168, 15);
            lblEmployeePosition.TabIndex = 12;
            lblEmployeePosition.Text = "Position (e.g. Mason, HR Staff)";
            // 
            // txtEmployeePosition
            // 
            txtEmployeePosition.Location = new Point(12, 506);
            txtEmployeePosition.Name = "txtEmployeePosition";
            txtEmployeePosition.Size = new Size(300, 23);
            txtEmployeePosition.TabIndex = 11;
            // 
            // lblEmployeeDepartment
            // 
            lblEmployeeDepartment.AutoSize = true;
            lblEmployeeDepartment.Location = new Point(12, 536);
            lblEmployeeDepartment.Name = "lblEmployeeDepartment";
            lblEmployeeDepartment.Size = new Size(183, 15);
            lblEmployeeDepartment.TabIndex = 13;
            lblEmployeeDepartment.Text = "Department (Site for site workers)";
            // 
            // cboEmployeeDepartment
            // 
            cboEmployeeDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEmployeeDepartment.Location = new Point(12, 554);
            cboEmployeeDepartment.Name = "cboEmployeeDepartment";
            cboEmployeeDepartment.Size = new Size(200, 23);
            cboEmployeeDepartment.TabIndex = 12;
            cboEmployeeDepartment.SelectedIndexChanged += cboEmployeeDepartment_SelectedIndexChanged;
            // 
            // lblPayScheduleValue
            // 
            lblPayScheduleValue.AutoSize = true;
            lblPayScheduleValue.Location = new Point(12, 584);
            lblPayScheduleValue.Name = "lblPayScheduleValue";
            lblPayScheduleValue.Size = new Size(202, 15);
            lblPayScheduleValue.TabIndex = 14;
            lblPayScheduleValue.Text = "Pay schedule: (choose a department)";
            // 
            // lblPayGrade
            // 
            lblPayGrade.AutoSize = true;
            lblPayGrade.Location = new Point(340, 320);
            lblPayGrade.Name = "lblPayGrade";
            lblPayGrade.Size = new Size(114, 15);
            lblPayGrade.TabIndex = 15;
            lblPayGrade.Text = "Pay grade (optional)";
            // 
            // txtPayGrade
            // 
            txtPayGrade.Location = new Point(340, 338);
            txtPayGrade.Name = "txtPayGrade";
            txtPayGrade.Size = new Size(200, 23);
            txtPayGrade.TabIndex = 13;
            // 
            // lblDailyRate
            // 
            lblDailyRate.AutoSize = true;
            lblDailyRate.Location = new Point(340, 368);
            lblDailyRate.Name = "lblDailyRate";
            lblDailyRate.Size = new Size(97, 15);
            lblDailyRate.TabIndex = 16;
            lblDailyRate.Text = "Daily rate (pesos)";
            // 
            // numDailyRate
            // 
            numDailyRate.DecimalPlaces = 2;
            numDailyRate.Location = new Point(340, 386);
            numDailyRate.Name = "numDailyRate";
            numDailyRate.Size = new Size(140, 23);
            numDailyRate.TabIndex = 14;
            numDailyRate.ThousandsSeparator = true;
            // 
            // lblRateEffective
            // 
            lblRateEffective.AutoSize = true;
            lblRateEffective.Location = new Point(340, 416);
            lblRateEffective.Name = "lblRateEffective";
            lblRateEffective.Size = new Size(99, 15);
            lblRateEffective.TabIndex = 17;
            lblRateEffective.Text = "Rate applies from";
            // 
            // dtpRateEffective
            // 
            dtpRateEffective.Format = DateTimePickerFormat.Short;
            dtpRateEffective.Location = new Point(340, 434);
            dtpRateEffective.Name = "dtpRateEffective";
            dtpRateEffective.Size = new Size(140, 23);
            dtpRateEffective.TabIndex = 15;
            // 
            // lblBiometricId
            // 
            lblBiometricId.AutoSize = true;
            lblBiometricId.Location = new Point(340, 464);
            lblBiometricId.Name = "lblBiometricId";
            lblBiometricId.Size = new Size(173, 15);
            lblBiometricId.TabIndex = 18;
            lblBiometricId.Text = "Biometric ID (from the scanner)";
            // 
            // txtBiometricId
            // 
            txtBiometricId.Location = new Point(340, 482);
            txtBiometricId.Name = "txtBiometricId";
            txtBiometricId.Size = new Size(140, 23);
            txtBiometricId.TabIndex = 16;
            // 
            // lblRateHistory
            // 
            lblRateHistory.AutoSize = true;
            lblRateHistory.Location = new Point(340, 512);
            lblRateHistory.Name = "lblRateHistory";
            lblRateHistory.Size = new Size(95, 15);
            lblRateHistory.TabIndex = 19;
            lblRateHistory.Text = "Daily rate history";
            // 
            // dgvRateHistory
            // 
            dgvRateHistory.AllowUserToAddRows = false;
            dgvRateHistory.AllowUserToDeleteRows = false;
            dgvRateHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRateHistory.Columns.AddRange(new DataGridViewColumn[] { colRateFrom, colRateAmount });
            dgvRateHistory.Location = new Point(340, 530);
            dgvRateHistory.MultiSelect = false;
            dgvRateHistory.Name = "dgvRateHistory";
            dgvRateHistory.ReadOnly = true;
            dgvRateHistory.RowHeadersVisible = false;
            dgvRateHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRateHistory.Size = new Size(280, 90);
            dgvRateHistory.TabIndex = 17;
            // 
            // colRateFrom
            // 
            colRateFrom.DataPropertyName = "EffectiveFrom";
            colRateFrom.HeaderText = "Applies From";
            colRateFrom.Name = "colRateFrom";
            colRateFrom.ReadOnly = true;
            // 
            // colRateAmount
            // 
            colRateAmount.DataPropertyName = "DailyRate";
            colRateAmount.HeaderText = "Daily Rate";
            colRateAmount.Name = "colRateAmount";
            colRateAmount.ReadOnly = true;
            // 
            // chkGovDeductions
            // 
            chkGovDeductions.AutoSize = true;
            chkGovDeductions.Location = new Point(650, 320);
            chkGovDeductions.Name = "chkGovDeductions";
            chkGovDeductions.Size = new Size(318, 19);
            chkGovDeductions.TabIndex = 18;
            chkGovDeductions.Text = "Government deductions (tax, SSS, PhilHealth, Pag-IBIG)";
            // 
            // lblSssNo
            // 
            lblSssNo.AutoSize = true;
            lblSssNo.Location = new Point(650, 348);
            lblSssNo.Name = "lblSssNo";
            lblSssNo.Size = new Size(125, 15);
            lblSssNo.TabIndex = 20;
            lblSssNo.Text = "SSS number (optional)";
            // 
            // txtSssNo
            // 
            txtSssNo.Location = new Point(650, 366);
            txtSssNo.Name = "txtSssNo";
            txtSssNo.Size = new Size(200, 23);
            txtSssNo.TabIndex = 19;
            // 
            // lblPhilHealthNo
            // 
            lblPhilHealthNo.AutoSize = true;
            lblPhilHealthNo.Location = new Point(650, 396);
            lblPhilHealthNo.Name = "lblPhilHealthNo";
            lblPhilHealthNo.Size = new Size(162, 15);
            lblPhilHealthNo.TabIndex = 21;
            lblPhilHealthNo.Text = "PhilHealth number (optional)";
            // 
            // txtPhilHealthNo
            // 
            txtPhilHealthNo.Location = new Point(650, 414);
            txtPhilHealthNo.Name = "txtPhilHealthNo";
            txtPhilHealthNo.Size = new Size(200, 23);
            txtPhilHealthNo.TabIndex = 20;
            // 
            // lblPagIbigNo
            // 
            lblPagIbigNo.AutoSize = true;
            lblPagIbigNo.Location = new Point(650, 444);
            lblPagIbigNo.Name = "lblPagIbigNo";
            lblPagIbigNo.Size = new Size(153, 15);
            lblPagIbigNo.TabIndex = 22;
            lblPagIbigNo.Text = "Pag-IBIG number (optional)";
            // 
            // txtPagIbigNo
            // 
            txtPagIbigNo.Location = new Point(650, 462);
            txtPagIbigNo.Name = "txtPagIbigNo";
            txtPagIbigNo.Size = new Size(200, 23);
            txtPagIbigNo.TabIndex = 21;
            // 
            // lblEmployeeTin
            // 
            lblEmployeeTin.AutoSize = true;
            lblEmployeeTin.Location = new Point(650, 492);
            lblEmployeeTin.Name = "lblEmployeeTin";
            lblEmployeeTin.Size = new Size(80, 15);
            lblEmployeeTin.TabIndex = 23;
            lblEmployeeTin.Text = "TIN (optional)";
            // 
            // txtEmployeeTin
            // 
            txtEmployeeTin.Location = new Point(650, 510);
            txtEmployeeTin.Name = "txtEmployeeTin";
            txtEmployeeTin.Size = new Size(200, 23);
            txtEmployeeTin.TabIndex = 22;
            // 
            // lblEmployeeFormMsg
            // 
            lblEmployeeFormMsg.AutoSize = true;
            lblEmployeeFormMsg.Location = new Point(12, 634);
            lblEmployeeFormMsg.Name = "lblEmployeeFormMsg";
            lblEmployeeFormMsg.Size = new Size(0, 15);
            lblEmployeeFormMsg.TabIndex = 24;
            // 
            // btnSaveEmployee
            // 
            btnSaveEmployee.Location = new Point(9, 656);
            btnSaveEmployee.Name = "btnSaveEmployee";
            btnSaveEmployee.Size = new Size(109, 28);
            btnSaveEmployee.TabIndex = 23;
            btnSaveEmployee.Text = "Save Edits";
            btnSaveEmployee.Click += btnSaveEmployee_Click;
            // 
            // btnCancelEmployee
            // 
            btnCancelEmployee.Location = new Point(120, 656);
            btnCancelEmployee.Name = "btnCancelEmployee";
            btnCancelEmployee.Size = new Size(110, 28);
            btnCancelEmployee.TabIndex = 24;
            btnCancelEmployee.Text = "Cancel";
            btnCancelEmployee.Click += btnCancelEmployee_Click;
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
            Size = new Size(1258, 795);
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
