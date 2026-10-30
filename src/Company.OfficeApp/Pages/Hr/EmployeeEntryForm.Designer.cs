namespace Company.OfficeApp.Pages.Hr
{
    partial class EmployeeEntryForm
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
            lblFormTitle = new Label();
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
            ((System.ComponentModel.ISupportInitialize)numDailyRate).BeginInit();
            SuspendLayout();
            // 
            // lblFormTitle
            // 
            lblFormTitle.AutoSize = true;
            lblFormTitle.Location = new Point(12, 12);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.Text = "New employee";
            // 
            // lblEmployeeName
            // 
            lblEmployeeName.AutoSize = true;
            lblEmployeeName.Location = new Point(12, 40);
            lblEmployeeName.Name = "lblEmployeeName";
            lblEmployeeName.Text = "Full name";
            // 
            // txtEmployeeName
            // 
            txtEmployeeName.Location = new Point(12, 58);
            txtEmployeeName.Name = "txtEmployeeName";
            txtEmployeeName.Size = new Size(300, 23);
            txtEmployeeName.TabIndex = 0;
            // 
            // lblEmployeeContact
            // 
            lblEmployeeContact.AutoSize = true;
            lblEmployeeContact.Location = new Point(12, 88);
            lblEmployeeContact.Name = "lblEmployeeContact";
            lblEmployeeContact.Text = "Contact number";
            // 
            // txtEmployeeContact
            // 
            txtEmployeeContact.Location = new Point(12, 106);
            txtEmployeeContact.Name = "txtEmployeeContact";
            txtEmployeeContact.Size = new Size(300, 23);
            txtEmployeeContact.TabIndex = 1;
            // 
            // lblEmployeeAddress
            // 
            lblEmployeeAddress.AutoSize = true;
            lblEmployeeAddress.Location = new Point(12, 136);
            lblEmployeeAddress.Name = "lblEmployeeAddress";
            lblEmployeeAddress.Text = "Address";
            // 
            // txtEmployeeAddress
            // 
            txtEmployeeAddress.Location = new Point(12, 154);
            txtEmployeeAddress.Name = "txtEmployeeAddress";
            txtEmployeeAddress.Size = new Size(300, 46);
            txtEmployeeAddress.Multiline = true;
            txtEmployeeAddress.TabIndex = 2;
            // 
            // lblEmployeePosition
            // 
            lblEmployeePosition.AutoSize = true;
            lblEmployeePosition.Location = new Point(12, 208);
            lblEmployeePosition.Name = "lblEmployeePosition";
            lblEmployeePosition.Text = "Position (e.g. Mason, HR Staff)";
            // 
            // txtEmployeePosition
            // 
            txtEmployeePosition.Location = new Point(12, 226);
            txtEmployeePosition.Name = "txtEmployeePosition";
            txtEmployeePosition.Size = new Size(300, 23);
            txtEmployeePosition.TabIndex = 3;
            // 
            // lblEmployeeDepartment
            // 
            lblEmployeeDepartment.AutoSize = true;
            lblEmployeeDepartment.Location = new Point(12, 256);
            lblEmployeeDepartment.Name = "lblEmployeeDepartment";
            lblEmployeeDepartment.Text = "Department (Site for site workers)";
            // 
            // cboEmployeeDepartment
            // 
            cboEmployeeDepartment.Location = new Point(12, 274);
            cboEmployeeDepartment.Name = "cboEmployeeDepartment";
            cboEmployeeDepartment.Size = new Size(200, 23);
            cboEmployeeDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEmployeeDepartment.SelectedIndexChanged += cboEmployeeDepartment_SelectedIndexChanged;
            cboEmployeeDepartment.TabIndex = 4;
            // 
            // lblPayScheduleValue
            // 
            lblPayScheduleValue.AutoSize = true;
            lblPayScheduleValue.Location = new Point(12, 304);
            lblPayScheduleValue.Name = "lblPayScheduleValue";
            lblPayScheduleValue.Text = "Pay schedule: (choose a department)";
            // 
            // lblPayGrade
            // 
            lblPayGrade.AutoSize = true;
            lblPayGrade.Location = new Point(340, 40);
            lblPayGrade.Name = "lblPayGrade";
            lblPayGrade.Text = "Pay grade (optional)";
            // 
            // txtPayGrade
            // 
            txtPayGrade.Location = new Point(340, 58);
            txtPayGrade.Name = "txtPayGrade";
            txtPayGrade.Size = new Size(200, 23);
            txtPayGrade.TabIndex = 5;
            // 
            // lblDailyRate
            // 
            lblDailyRate.AutoSize = true;
            lblDailyRate.Location = new Point(340, 88);
            lblDailyRate.Name = "lblDailyRate";
            lblDailyRate.Text = "Daily rate (pesos)";
            // 
            // numDailyRate
            // 
            numDailyRate.Location = new Point(340, 106);
            numDailyRate.Name = "numDailyRate";
            numDailyRate.Size = new Size(140, 23);
            numDailyRate.DecimalPlaces = 2;
            numDailyRate.ThousandsSeparator = true;
            numDailyRate.TabIndex = 6;
            // 
            // lblRateEffective
            // 
            lblRateEffective.AutoSize = true;
            lblRateEffective.Location = new Point(340, 136);
            lblRateEffective.Name = "lblRateEffective";
            lblRateEffective.Text = "Rate applies from";
            // 
            // dtpRateEffective
            // 
            dtpRateEffective.Location = new Point(340, 154);
            dtpRateEffective.Name = "dtpRateEffective";
            dtpRateEffective.Size = new Size(140, 23);
            dtpRateEffective.Format = DateTimePickerFormat.Short;
            dtpRateEffective.TabIndex = 7;
            // 
            // lblBiometricId
            // 
            lblBiometricId.AutoSize = true;
            lblBiometricId.Location = new Point(340, 184);
            lblBiometricId.Name = "lblBiometricId";
            lblBiometricId.Text = "Biometric ID (from the scanner)";
            // 
            // txtBiometricId
            // 
            txtBiometricId.Location = new Point(340, 202);
            txtBiometricId.Name = "txtBiometricId";
            txtBiometricId.Size = new Size(140, 23);
            txtBiometricId.TabIndex = 8;
            // 
            // chkGovDeductions
            // 
            chkGovDeductions.AutoSize = true;
            chkGovDeductions.Location = new Point(650, 40);
            chkGovDeductions.Name = "chkGovDeductions";
            chkGovDeductions.Text = "Government deductions (tax, SSS, PhilHealth, Pag-IBIG)";
            chkGovDeductions.TabIndex = 9;
            // 
            // lblSssNo
            // 
            lblSssNo.AutoSize = true;
            lblSssNo.Location = new Point(650, 68);
            lblSssNo.Name = "lblSssNo";
            lblSssNo.Text = "SSS number (optional)";
            // 
            // txtSssNo
            // 
            txtSssNo.Location = new Point(650, 86);
            txtSssNo.Name = "txtSssNo";
            txtSssNo.Size = new Size(200, 23);
            txtSssNo.TabIndex = 10;
            // 
            // lblPhilHealthNo
            // 
            lblPhilHealthNo.AutoSize = true;
            lblPhilHealthNo.Location = new Point(650, 116);
            lblPhilHealthNo.Name = "lblPhilHealthNo";
            lblPhilHealthNo.Text = "PhilHealth number (optional)";
            // 
            // txtPhilHealthNo
            // 
            txtPhilHealthNo.Location = new Point(650, 134);
            txtPhilHealthNo.Name = "txtPhilHealthNo";
            txtPhilHealthNo.Size = new Size(200, 23);
            txtPhilHealthNo.TabIndex = 11;
            // 
            // lblPagIbigNo
            // 
            lblPagIbigNo.AutoSize = true;
            lblPagIbigNo.Location = new Point(650, 164);
            lblPagIbigNo.Name = "lblPagIbigNo";
            lblPagIbigNo.Text = "Pag-IBIG number (optional)";
            // 
            // txtPagIbigNo
            // 
            txtPagIbigNo.Location = new Point(650, 182);
            txtPagIbigNo.Name = "txtPagIbigNo";
            txtPagIbigNo.Size = new Size(200, 23);
            txtPagIbigNo.TabIndex = 12;
            // 
            // lblEmployeeTin
            // 
            lblEmployeeTin.AutoSize = true;
            lblEmployeeTin.Location = new Point(650, 212);
            lblEmployeeTin.Name = "lblEmployeeTin";
            lblEmployeeTin.Text = "TIN (optional)";
            // 
            // txtEmployeeTin
            // 
            txtEmployeeTin.Location = new Point(650, 230);
            txtEmployeeTin.Name = "txtEmployeeTin";
            txtEmployeeTin.Size = new Size(200, 23);
            txtEmployeeTin.TabIndex = 13;
            // 
            // lblEmployeeFormMsg
            // 
            lblEmployeeFormMsg.AutoSize = true;
            lblEmployeeFormMsg.Location = new Point(12, 334);
            lblEmployeeFormMsg.Name = "lblEmployeeFormMsg";
            lblEmployeeFormMsg.Text = "";
            // 
            // btnSaveEmployee
            // 
            btnSaveEmployee.Location = new Point(12, 356);
            btnSaveEmployee.Name = "btnSaveEmployee";
            btnSaveEmployee.Size = new Size(90, 28);
            btnSaveEmployee.Text = "Save";
            btnSaveEmployee.Click += btnSaveEmployee_Click;
            btnSaveEmployee.TabIndex = 14;
            // 
            // btnCancelEmployee
            // 
            btnCancelEmployee.Location = new Point(108, 356);
            btnCancelEmployee.Name = "btnCancelEmployee";
            btnCancelEmployee.Size = new Size(90, 28);
            btnCancelEmployee.Text = "Cancel";
            btnCancelEmployee.DialogResult = DialogResult.Cancel;
            btnCancelEmployee.TabIndex = 15;
            // 
            // EmployeeEntryForm
            // 
            AcceptButton = btnSaveEmployee;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelEmployee;
            ClientSize = new Size(880, 396);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(lblFormTitle);
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
            Name = "EmployeeEntryForm";
            Text = "Add Employee";
            ((System.ComponentModel.ISupportInitialize)numDailyRate).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFormTitle;
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
    }
}
