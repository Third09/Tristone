namespace Company.OfficeApp.Pages.Hr
{
    partial class HrSettingsTab
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
            lblWorkSchedule = new Label();
            lblWorkStart = new Label();
            dtpWorkStart = new DateTimePicker();
            lblWorkEnd = new Label();
            dtpWorkEnd = new DateTimePicker();
            lblBreakStart = new Label();
            dtpBreakStart = new DateTimePicker();
            lblBreakEnd = new Label();
            dtpBreakEnd = new DateTimePicker();
            lblGraceMinutes = new Label();
            numGraceMinutes = new NumericUpDown();
            lblWorkDays = new Label();
            clbWorkDays = new CheckedListBox();
            lblOvertimeRate = new Label();
            numOvertimeRate = new NumericUpDown();
            lblOtMinimum = new Label();
            numOtMinimum = new NumericUpDown();
            lblGovTitle = new Label();
            lblWorkDaysPerMonth = new Label();
            numWorkDaysPerMonth = new NumericUpDown();
            lblSssRate = new Label();
            numSssRate = new NumericUpDown();
            lblSssMin = new Label();
            numSssMin = new NumericUpDown();
            lblSssMax = new Label();
            numSssMax = new NumericUpDown();
            lblPhilHealthRate = new Label();
            numPhilHealthRate = new NumericUpDown();
            lblPhilHealthFloor = new Label();
            numPhilHealthFloor = new NumericUpDown();
            lblPhilHealthCeiling = new Label();
            numPhilHealthCeiling = new NumericUpDown();
            lblPagIbigRate = new Label();
            numPagIbigRate = new NumericUpDown();
            lblPagIbigCap = new Label();
            numPagIbigCap = new NumericUpDown();
            chkWithholdingTax = new CheckBox();
            lblGovNote = new Label();
            lblSettingsMsg = new Label();
            btnSaveHrSettings = new Button();
            lblHolidays = new Label();
            dgvHolidays = new DataGridView();
            lblHolidayDate = new Label();
            dtpHolidayDate = new DateTimePicker();
            lblHolidayName = new Label();
            txtHolidayName = new TextBox();
            lblHolidayKind = new Label();
            cboHolidayKind = new ComboBox();
            btnAddHoliday = new Button();
            btnDeleteHoliday = new Button();
            lblHolidayMsg = new Label();
            colHolidayDate = new DataGridViewTextBoxColumn();
            colHolidayName = new DataGridViewTextBoxColumn();
            colHolidayKind = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)numGraceMinutes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numOvertimeRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numOtMinimum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numWorkDaysPerMonth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSssRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSssMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSssMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPhilHealthRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPhilHealthFloor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPhilHealthCeiling).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPagIbigRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPagIbigCap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvHolidays).BeginInit();
            SuspendLayout();
            // 
            // lblWorkSchedule
            // 
            lblWorkSchedule.AutoSize = true;
            lblWorkSchedule.Location = new Point(12, 12);
            lblWorkSchedule.Name = "lblWorkSchedule";
            lblWorkSchedule.Text = "Work schedule";
            // 
            // lblWorkStart
            // 
            lblWorkStart.AutoSize = true;
            lblWorkStart.Location = new Point(12, 40);
            lblWorkStart.Name = "lblWorkStart";
            lblWorkStart.Text = "Work start";
            // 
            // dtpWorkStart
            // 
            dtpWorkStart.Location = new Point(140, 36);
            dtpWorkStart.Name = "dtpWorkStart";
            dtpWorkStart.Size = new Size(110, 23);
            dtpWorkStart.Format = DateTimePickerFormat.Custom;
            dtpWorkStart.CustomFormat = "h:mm tt";
            dtpWorkStart.ShowUpDown = true;
            dtpWorkStart.TabIndex = 0;
            // 
            // lblWorkEnd
            // 
            lblWorkEnd.AutoSize = true;
            lblWorkEnd.Location = new Point(12, 70);
            lblWorkEnd.Name = "lblWorkEnd";
            lblWorkEnd.Text = "Work end";
            // 
            // dtpWorkEnd
            // 
            dtpWorkEnd.Location = new Point(140, 66);
            dtpWorkEnd.Name = "dtpWorkEnd";
            dtpWorkEnd.Size = new Size(110, 23);
            dtpWorkEnd.Format = DateTimePickerFormat.Custom;
            dtpWorkEnd.CustomFormat = "h:mm tt";
            dtpWorkEnd.ShowUpDown = true;
            dtpWorkEnd.TabIndex = 1;
            // 
            // lblBreakStart
            // 
            lblBreakStart.AutoSize = true;
            lblBreakStart.Location = new Point(12, 100);
            lblBreakStart.Name = "lblBreakStart";
            lblBreakStart.Text = "Break start";
            // 
            // dtpBreakStart
            // 
            dtpBreakStart.Location = new Point(140, 96);
            dtpBreakStart.Name = "dtpBreakStart";
            dtpBreakStart.Size = new Size(110, 23);
            dtpBreakStart.Format = DateTimePickerFormat.Custom;
            dtpBreakStart.CustomFormat = "h:mm tt";
            dtpBreakStart.ShowUpDown = true;
            dtpBreakStart.TabIndex = 2;
            // 
            // lblBreakEnd
            // 
            lblBreakEnd.AutoSize = true;
            lblBreakEnd.Location = new Point(12, 130);
            lblBreakEnd.Name = "lblBreakEnd";
            lblBreakEnd.Text = "Break end";
            // 
            // dtpBreakEnd
            // 
            dtpBreakEnd.Location = new Point(140, 126);
            dtpBreakEnd.Name = "dtpBreakEnd";
            dtpBreakEnd.Size = new Size(110, 23);
            dtpBreakEnd.Format = DateTimePickerFormat.Custom;
            dtpBreakEnd.CustomFormat = "h:mm tt";
            dtpBreakEnd.ShowUpDown = true;
            dtpBreakEnd.TabIndex = 3;
            // 
            // lblGraceMinutes
            // 
            lblGraceMinutes.AutoSize = true;
            lblGraceMinutes.Location = new Point(12, 160);
            lblGraceMinutes.Name = "lblGraceMinutes";
            lblGraceMinutes.Text = "Grace minutes";
            // 
            // numGraceMinutes
            // 
            numGraceMinutes.Location = new Point(140, 156);
            numGraceMinutes.Name = "numGraceMinutes";
            numGraceMinutes.Size = new Size(70, 23);
            numGraceMinutes.Maximum = 120m;
            numGraceMinutes.TabIndex = 4;
            // 
            // lblWorkDays
            // 
            lblWorkDays.AutoSize = true;
            lblWorkDays.Location = new Point(12, 190);
            lblWorkDays.Name = "lblWorkDays";
            lblWorkDays.Text = "Work days";
            // 
            // clbWorkDays
            // 
            clbWorkDays.Location = new Point(140, 186);
            clbWorkDays.Name = "clbWorkDays";
            clbWorkDays.Size = new Size(140, 130);
            clbWorkDays.CheckOnClick = true;
            clbWorkDays.TabIndex = 5;
            // 
            // lblOvertimeRate
            // 
            lblOvertimeRate.AutoSize = true;
            lblOvertimeRate.Location = new Point(12, 326);
            lblOvertimeRate.Name = "lblOvertimeRate";
            lblOvertimeRate.Text = "Overtime per hour";
            // 
            // numOvertimeRate
            // 
            numOvertimeRate.Location = new Point(140, 322);
            numOvertimeRate.Name = "numOvertimeRate";
            numOvertimeRate.Size = new Size(100, 23);
            numOvertimeRate.DecimalPlaces = 2;
            numOvertimeRate.ThousandsSeparator = true;
            numOvertimeRate.Maximum = 99999999m;
            numOvertimeRate.TabIndex = 6;
            // 
            // lblOtMinimum
            // 
            lblOtMinimum.AutoSize = true;
            lblOtMinimum.Location = new Point(12, 356);
            lblOtMinimum.Name = "lblOtMinimum";
            lblOtMinimum.Text = "Overtime from (min)";
            // 
            // numOtMinimum
            // 
            numOtMinimum.Location = new Point(140, 352);
            numOtMinimum.Name = "numOtMinimum";
            numOtMinimum.Size = new Size(70, 23);
            numOtMinimum.Maximum = 240m;
            numOtMinimum.TabIndex = 7;
            // 
            // lblGovTitle
            // 
            lblGovTitle.AutoSize = true;
            lblGovTitle.Location = new Point(320, 12);
            lblGovTitle.Name = "lblGovTitle";
            lblGovTitle.Text = "Government deductions (only for employees with the switch on)";
            // 
            // lblWorkDaysPerMonth
            // 
            lblWorkDaysPerMonth.AutoSize = true;
            lblWorkDaysPerMonth.Location = new Point(320, 40);
            lblWorkDaysPerMonth.Name = "lblWorkDaysPerMonth";
            lblWorkDaysPerMonth.Text = "Work days per month";
            // 
            // numWorkDaysPerMonth
            // 
            numWorkDaysPerMonth.Location = new Point(520, 36);
            numWorkDaysPerMonth.Name = "numWorkDaysPerMonth";
            numWorkDaysPerMonth.Size = new Size(70, 23);
            numWorkDaysPerMonth.Minimum = 1m;
            numWorkDaysPerMonth.Maximum = 31m;
            numWorkDaysPerMonth.Value = 26m;
            numWorkDaysPerMonth.TabIndex = 8;
            // 
            // lblSssRate
            // 
            lblSssRate.AutoSize = true;
            lblSssRate.Location = new Point(320, 70);
            lblSssRate.Name = "lblSssRate";
            lblSssRate.Text = "SSS employee share %";
            // 
            // numSssRate
            // 
            numSssRate.Location = new Point(520, 66);
            numSssRate.Name = "numSssRate";
            numSssRate.Size = new Size(70, 23);
            numSssRate.DecimalPlaces = 2;
            numSssRate.Maximum = 50m;
            numSssRate.TabIndex = 9;
            // 
            // lblSssMin
            // 
            lblSssMin.AutoSize = true;
            lblSssMin.Location = new Point(320, 100);
            lblSssMin.Name = "lblSssMin";
            lblSssMin.Text = "SSS salary credit, minimum";
            // 
            // numSssMin
            // 
            numSssMin.Location = new Point(520, 96);
            numSssMin.Name = "numSssMin";
            numSssMin.Size = new Size(100, 23);
            numSssMin.DecimalPlaces = 2;
            numSssMin.ThousandsSeparator = true;
            numSssMin.Maximum = 99999999m;
            numSssMin.TabIndex = 10;
            // 
            // lblSssMax
            // 
            lblSssMax.AutoSize = true;
            lblSssMax.Location = new Point(320, 130);
            lblSssMax.Name = "lblSssMax";
            lblSssMax.Text = "SSS salary credit, maximum";
            // 
            // numSssMax
            // 
            numSssMax.Location = new Point(520, 126);
            numSssMax.Name = "numSssMax";
            numSssMax.Size = new Size(100, 23);
            numSssMax.DecimalPlaces = 2;
            numSssMax.ThousandsSeparator = true;
            numSssMax.Maximum = 99999999m;
            numSssMax.TabIndex = 11;
            // 
            // lblPhilHealthRate
            // 
            lblPhilHealthRate.AutoSize = true;
            lblPhilHealthRate.Location = new Point(320, 160);
            lblPhilHealthRate.Name = "lblPhilHealthRate";
            lblPhilHealthRate.Text = "PhilHealth employee share %";
            // 
            // numPhilHealthRate
            // 
            numPhilHealthRate.Location = new Point(520, 156);
            numPhilHealthRate.Name = "numPhilHealthRate";
            numPhilHealthRate.Size = new Size(70, 23);
            numPhilHealthRate.DecimalPlaces = 2;
            numPhilHealthRate.Maximum = 50m;
            numPhilHealthRate.TabIndex = 12;
            // 
            // lblPhilHealthFloor
            // 
            lblPhilHealthFloor.AutoSize = true;
            lblPhilHealthFloor.Location = new Point(320, 190);
            lblPhilHealthFloor.Name = "lblPhilHealthFloor";
            lblPhilHealthFloor.Text = "PhilHealth salary floor";
            // 
            // numPhilHealthFloor
            // 
            numPhilHealthFloor.Location = new Point(520, 186);
            numPhilHealthFloor.Name = "numPhilHealthFloor";
            numPhilHealthFloor.Size = new Size(100, 23);
            numPhilHealthFloor.DecimalPlaces = 2;
            numPhilHealthFloor.ThousandsSeparator = true;
            numPhilHealthFloor.Maximum = 99999999m;
            numPhilHealthFloor.TabIndex = 13;
            // 
            // lblPhilHealthCeiling
            // 
            lblPhilHealthCeiling.AutoSize = true;
            lblPhilHealthCeiling.Location = new Point(320, 220);
            lblPhilHealthCeiling.Name = "lblPhilHealthCeiling";
            lblPhilHealthCeiling.Text = "PhilHealth salary ceiling";
            // 
            // numPhilHealthCeiling
            // 
            numPhilHealthCeiling.Location = new Point(520, 216);
            numPhilHealthCeiling.Name = "numPhilHealthCeiling";
            numPhilHealthCeiling.Size = new Size(100, 23);
            numPhilHealthCeiling.DecimalPlaces = 2;
            numPhilHealthCeiling.ThousandsSeparator = true;
            numPhilHealthCeiling.Maximum = 99999999m;
            numPhilHealthCeiling.TabIndex = 14;
            // 
            // lblPagIbigRate
            // 
            lblPagIbigRate.AutoSize = true;
            lblPagIbigRate.Location = new Point(320, 250);
            lblPagIbigRate.Name = "lblPagIbigRate";
            lblPagIbigRate.Text = "Pag-IBIG employee share %";
            // 
            // numPagIbigRate
            // 
            numPagIbigRate.Location = new Point(520, 246);
            numPagIbigRate.Name = "numPagIbigRate";
            numPagIbigRate.Size = new Size(70, 23);
            numPagIbigRate.DecimalPlaces = 2;
            numPagIbigRate.Maximum = 50m;
            numPagIbigRate.TabIndex = 15;
            // 
            // lblPagIbigCap
            // 
            lblPagIbigCap.AutoSize = true;
            lblPagIbigCap.Location = new Point(320, 280);
            lblPagIbigCap.Name = "lblPagIbigCap";
            lblPagIbigCap.Text = "Pag-IBIG salary cap";
            // 
            // numPagIbigCap
            // 
            numPagIbigCap.Location = new Point(520, 276);
            numPagIbigCap.Name = "numPagIbigCap";
            numPagIbigCap.Size = new Size(100, 23);
            numPagIbigCap.DecimalPlaces = 2;
            numPagIbigCap.ThousandsSeparator = true;
            numPagIbigCap.Maximum = 99999999m;
            numPagIbigCap.TabIndex = 16;
            // 
            // chkWithholdingTax
            // 
            chkWithholdingTax.AutoSize = true;
            chkWithholdingTax.Location = new Point(320, 310);
            chkWithholdingTax.Name = "chkWithholdingTax";
            chkWithholdingTax.Text = "Withhold income tax (TRAIN table)";
            chkWithholdingTax.TabIndex = 17;
            // 
            // lblGovNote
            // 
            lblGovNote.AutoSize = true;
            lblGovNote.Location = new Point(320, 338);
            lblGovNote.Name = "lblGovNote";
            lblGovNote.Text = "Defaults are the 2025 employee shares. Check them with your accountant and change them here when the government updates them.";
            lblGovNote.MaximumSize = new Size(400, 0);
            // 
            // lblSettingsMsg
            // 
            lblSettingsMsg.AutoSize = true;
            lblSettingsMsg.Location = new Point(12, 392);
            lblSettingsMsg.Name = "lblSettingsMsg";
            lblSettingsMsg.MaximumSize = new Size(900, 0);
            // 
            // btnSaveHrSettings
            // 
            btnSaveHrSettings.Location = new Point(12, 414);
            btnSaveHrSettings.Name = "btnSaveHrSettings";
            btnSaveHrSettings.Size = new Size(120, 28);
            btnSaveHrSettings.Text = "Save Settings";
            btnSaveHrSettings.TabIndex = 18;
            btnSaveHrSettings.Click += btnSaveHrSettings_Click;
            // 
            // lblHolidays
            // 
            lblHolidays.AutoSize = true;
            lblHolidays.Location = new Point(12, 460);
            lblHolidays.Name = "lblHolidays";
            lblHolidays.Text = "Holidays (no one is marked absent; paydays move to the day before)";
            // 
            // dgvHolidays
            // 
            dgvHolidays.AllowUserToAddRows = false;
            dgvHolidays.AllowUserToDeleteRows = false;
            dgvHolidays.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHolidays.Columns.AddRange(new DataGridViewColumn[] { colHolidayDate, colHolidayName, colHolidayKind });
            dgvHolidays.MultiSelect = false;
            dgvHolidays.ReadOnly = true;
            dgvHolidays.RowHeadersVisible = false;
            dgvHolidays.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHolidays.Location = new Point(12, 482);
            dgvHolidays.Name = "dgvHolidays";
            dgvHolidays.Size = new Size(420, 140);
            dgvHolidays.TabIndex = 19;
            dgvHolidays.SelectionChanged += dgvHolidays_SelectionChanged;
            // 
            // lblHolidayDate
            // 
            lblHolidayDate.AutoSize = true;
            lblHolidayDate.Location = new Point(450, 482);
            lblHolidayDate.Name = "lblHolidayDate";
            lblHolidayDate.Text = "Date";
            // 
            // dtpHolidayDate
            // 
            dtpHolidayDate.Location = new Point(450, 500);
            dtpHolidayDate.Name = "dtpHolidayDate";
            dtpHolidayDate.Size = new Size(120, 23);
            dtpHolidayDate.Format = DateTimePickerFormat.Short;
            dtpHolidayDate.TabIndex = 20;
            // 
            // lblHolidayName
            // 
            lblHolidayName.AutoSize = true;
            lblHolidayName.Location = new Point(450, 530);
            lblHolidayName.Name = "lblHolidayName";
            lblHolidayName.Text = "Name";
            // 
            // txtHolidayName
            // 
            txtHolidayName.Location = new Point(450, 548);
            txtHolidayName.Name = "txtHolidayName";
            txtHolidayName.Size = new Size(200, 23);
            txtHolidayName.TabIndex = 21;
            // 
            // lblHolidayKind
            // 
            lblHolidayKind.AutoSize = true;
            lblHolidayKind.Location = new Point(450, 578);
            lblHolidayKind.Name = "lblHolidayKind";
            lblHolidayKind.Text = "Kind";
            // 
            // cboHolidayKind
            // 
            cboHolidayKind.Location = new Point(450, 596);
            cboHolidayKind.Name = "cboHolidayKind";
            cboHolidayKind.Size = new Size(120, 23);
            cboHolidayKind.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHolidayKind.TabIndex = 22;
            // 
            // btnAddHoliday
            // 
            btnAddHoliday.Location = new Point(670, 546);
            btnAddHoliday.Name = "btnAddHoliday";
            btnAddHoliday.Size = new Size(110, 28);
            btnAddHoliday.Text = "Add Holiday";
            btnAddHoliday.TabIndex = 23;
            btnAddHoliday.Click += btnAddHoliday_Click;
            // 
            // btnDeleteHoliday
            // 
            btnDeleteHoliday.Location = new Point(670, 580);
            btnDeleteHoliday.Name = "btnDeleteHoliday";
            btnDeleteHoliday.Size = new Size(110, 28);
            btnDeleteHoliday.Text = "Delete Holiday";
            btnDeleteHoliday.TabIndex = 24;
            btnDeleteHoliday.Click += btnDeleteHoliday_Click;
            // 
            // lblHolidayMsg
            // 
            lblHolidayMsg.AutoSize = true;
            lblHolidayMsg.Location = new Point(12, 630);
            lblHolidayMsg.Name = "lblHolidayMsg";
            // 
            // colHolidayDate
            // 
            colHolidayDate.DataPropertyName = "HolidayDate";
            colHolidayDate.HeaderText = "Date";
            colHolidayDate.Name = "colHolidayDate";
            colHolidayDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "ddd, MMM d, yyyy" };
            // 
            // colHolidayName
            // 
            colHolidayName.DataPropertyName = "Name";
            colHolidayName.HeaderText = "Name";
            colHolidayName.Name = "colHolidayName";
            // 
            // colHolidayKind
            // 
            colHolidayKind.DataPropertyName = "Kind";
            colHolidayKind.HeaderText = "Kind";
            colHolidayKind.Name = "colHolidayKind";
            // 
            // HrSettingsTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblWorkSchedule);
            Controls.Add(lblWorkStart);
            Controls.Add(dtpWorkStart);
            Controls.Add(lblWorkEnd);
            Controls.Add(dtpWorkEnd);
            Controls.Add(lblBreakStart);
            Controls.Add(dtpBreakStart);
            Controls.Add(lblBreakEnd);
            Controls.Add(dtpBreakEnd);
            Controls.Add(lblGraceMinutes);
            Controls.Add(numGraceMinutes);
            Controls.Add(lblWorkDays);
            Controls.Add(clbWorkDays);
            Controls.Add(lblOvertimeRate);
            Controls.Add(numOvertimeRate);
            Controls.Add(lblOtMinimum);
            Controls.Add(numOtMinimum);
            Controls.Add(lblGovTitle);
            Controls.Add(lblWorkDaysPerMonth);
            Controls.Add(numWorkDaysPerMonth);
            Controls.Add(lblSssRate);
            Controls.Add(numSssRate);
            Controls.Add(lblSssMin);
            Controls.Add(numSssMin);
            Controls.Add(lblSssMax);
            Controls.Add(numSssMax);
            Controls.Add(lblPhilHealthRate);
            Controls.Add(numPhilHealthRate);
            Controls.Add(lblPhilHealthFloor);
            Controls.Add(numPhilHealthFloor);
            Controls.Add(lblPhilHealthCeiling);
            Controls.Add(numPhilHealthCeiling);
            Controls.Add(lblPagIbigRate);
            Controls.Add(numPagIbigRate);
            Controls.Add(lblPagIbigCap);
            Controls.Add(numPagIbigCap);
            Controls.Add(chkWithholdingTax);
            Controls.Add(lblGovNote);
            Controls.Add(lblSettingsMsg);
            Controls.Add(btnSaveHrSettings);
            Controls.Add(lblHolidays);
            Controls.Add(dgvHolidays);
            Controls.Add(lblHolidayDate);
            Controls.Add(dtpHolidayDate);
            Controls.Add(lblHolidayName);
            Controls.Add(txtHolidayName);
            Controls.Add(lblHolidayKind);
            Controls.Add(cboHolidayKind);
            Controls.Add(btnAddHoliday);
            Controls.Add(btnDeleteHoliday);
            Controls.Add(lblHolidayMsg);
            Name = "HrSettingsTab";
            Size = new Size(960, 660);
            Load += HrSettingsTab_Load;
            ((System.ComponentModel.ISupportInitialize)numGraceMinutes).EndInit();
            ((System.ComponentModel.ISupportInitialize)numOvertimeRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)numOtMinimum).EndInit();
            ((System.ComponentModel.ISupportInitialize)numWorkDaysPerMonth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSssRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSssMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSssMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPhilHealthRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPhilHealthFloor).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPhilHealthCeiling).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPagIbigRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPagIbigCap).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvHolidays).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblWorkSchedule;
        private Label lblWorkStart;
        private DateTimePicker dtpWorkStart;
        private Label lblWorkEnd;
        private DateTimePicker dtpWorkEnd;
        private Label lblBreakStart;
        private DateTimePicker dtpBreakStart;
        private Label lblBreakEnd;
        private DateTimePicker dtpBreakEnd;
        private Label lblGraceMinutes;
        private NumericUpDown numGraceMinutes;
        private Label lblWorkDays;
        private CheckedListBox clbWorkDays;
        private Label lblOvertimeRate;
        private NumericUpDown numOvertimeRate;
        private Label lblOtMinimum;
        private NumericUpDown numOtMinimum;
        private Label lblGovTitle;
        private Label lblWorkDaysPerMonth;
        private NumericUpDown numWorkDaysPerMonth;
        private Label lblSssRate;
        private NumericUpDown numSssRate;
        private Label lblSssMin;
        private NumericUpDown numSssMin;
        private Label lblSssMax;
        private NumericUpDown numSssMax;
        private Label lblPhilHealthRate;
        private NumericUpDown numPhilHealthRate;
        private Label lblPhilHealthFloor;
        private NumericUpDown numPhilHealthFloor;
        private Label lblPhilHealthCeiling;
        private NumericUpDown numPhilHealthCeiling;
        private Label lblPagIbigRate;
        private NumericUpDown numPagIbigRate;
        private Label lblPagIbigCap;
        private NumericUpDown numPagIbigCap;
        private CheckBox chkWithholdingTax;
        private Label lblGovNote;
        private Label lblSettingsMsg;
        private Button btnSaveHrSettings;
        private Label lblHolidays;
        private DataGridView dgvHolidays;
        private Label lblHolidayDate;
        private DateTimePicker dtpHolidayDate;
        private Label lblHolidayName;
        private TextBox txtHolidayName;
        private Label lblHolidayKind;
        private ComboBox cboHolidayKind;
        private Button btnAddHoliday;
        private Button btnDeleteHoliday;
        private Label lblHolidayMsg;
        private DataGridViewTextBoxColumn colHolidayDate;
        private DataGridViewTextBoxColumn colHolidayName;
        private DataGridViewTextBoxColumn colHolidayKind;
    }
}
