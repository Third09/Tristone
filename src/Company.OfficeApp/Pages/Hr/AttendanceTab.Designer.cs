namespace Company.OfficeApp.Pages.Hr
{
    partial class AttendanceTab
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
            btnUploadBiometrics = new Button();
            lblImportSummary = new Label();
            btnViewImportErrors = new Button();
            btnExportToScanner = new Button();
            lblAttendanceFrom = new Label();
            dtpAttendanceFrom = new DateTimePicker();
            lblAttendanceTo = new Label();
            dtpAttendanceTo = new DateTimePicker();
            lblAttendanceFilter = new Label();
            cboAttendanceFilter = new ComboBox();
            lblAttendanceSearch = new Label();
            txtAttendanceSearch = new TextBox();
            dgvAttendance = new DataGridView();
            lblAttendanceCount = new Label();
            btnFixScan = new Button();
            btnApproveOvertime = new Button();
            btnRejectOvertime = new Button();
            btnExportAttendance = new Button();
            lblApproverNote = new Label();
            lblAttendanceMsg = new Label();
            colDate = new DataGridViewTextBoxColumn();
            colDay = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colTimeIn = new DataGridViewTextBoxColumn();
            colBreakOut = new DataGridViewTextBoxColumn();
            colBreakIn = new DataGridViewTextBoxColumn();
            colTimeOut = new DataGridViewTextBoxColumn();
            colLate = new DataGridViewTextBoxColumn();
            colEarly = new DataGridViewTextBoxColumn();
            colOvertime = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colFixed = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).BeginInit();
            SuspendLayout();
            // 
            // btnUploadBiometrics
            // 
            btnUploadBiometrics.Location = new Point(12, 10);
            btnUploadBiometrics.Name = "btnUploadBiometrics";
            btnUploadBiometrics.Size = new Size(160, 28);
            btnUploadBiometrics.Text = "Upload Scanner File";
            btnUploadBiometrics.TabIndex = 0;
            btnUploadBiometrics.Click += btnUploadBiometrics_Click;
            // 
            // lblImportSummary
            // 
            lblImportSummary.AutoSize = true;
            lblImportSummary.Location = new Point(180, 16);
            lblImportSummary.Name = "lblImportSummary";
            lblImportSummary.Text = "No scanner file uploaded yet.";
            // 
            // btnViewImportErrors
            // 
            btnViewImportErrors.Location = new Point(12, 42);
            btnViewImportErrors.Name = "btnViewImportErrors";
            btnViewImportErrors.Size = new Size(160, 28);
            btnViewImportErrors.Text = "View Rejected Rows";
            btnViewImportErrors.TabIndex = 1;
            btnViewImportErrors.Click += btnViewImportErrors_Click;
            // 
            // btnExportToScanner
            // 
            btnExportToScanner.Location = new Point(178, 42);
            btnExportToScanner.Name = "btnExportToScanner";
            btnExportToScanner.Size = new Size(200, 28);
            btnExportToScanner.Text = "Make Scanner Employee File";
            btnExportToScanner.TabIndex = 2;
            btnExportToScanner.Click += btnExportToScanner_Click;
            // 
            // lblAttendanceFrom
            // 
            lblAttendanceFrom.AutoSize = true;
            lblAttendanceFrom.Location = new Point(12, 83);
            lblAttendanceFrom.Name = "lblAttendanceFrom";
            lblAttendanceFrom.Text = "From";
            // 
            // dtpAttendanceFrom
            // 
            dtpAttendanceFrom.Location = new Point(52, 80);
            dtpAttendanceFrom.Name = "dtpAttendanceFrom";
            dtpAttendanceFrom.Size = new Size(115, 23);
            dtpAttendanceFrom.Format = DateTimePickerFormat.Short;
            dtpAttendanceFrom.TabIndex = 3;
            dtpAttendanceFrom.ValueChanged += Filter_Changed;
            // 
            // lblAttendanceTo
            // 
            lblAttendanceTo.AutoSize = true;
            lblAttendanceTo.Location = new Point(177, 83);
            lblAttendanceTo.Name = "lblAttendanceTo";
            lblAttendanceTo.Text = "To";
            // 
            // dtpAttendanceTo
            // 
            dtpAttendanceTo.Location = new Point(203, 80);
            dtpAttendanceTo.Name = "dtpAttendanceTo";
            dtpAttendanceTo.Size = new Size(115, 23);
            dtpAttendanceTo.Format = DateTimePickerFormat.Short;
            dtpAttendanceTo.TabIndex = 4;
            dtpAttendanceTo.ValueChanged += Filter_Changed;
            // 
            // lblAttendanceFilter
            // 
            lblAttendanceFilter.AutoSize = true;
            lblAttendanceFilter.Location = new Point(330, 83);
            lblAttendanceFilter.Name = "lblAttendanceFilter";
            lblAttendanceFilter.Text = "Show";
            // 
            // cboAttendanceFilter
            // 
            cboAttendanceFilter.Location = new Point(372, 80);
            cboAttendanceFilter.Name = "cboAttendanceFilter";
            cboAttendanceFilter.Size = new Size(200, 23);
            cboAttendanceFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAttendanceFilter.TabIndex = 5;
            cboAttendanceFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblAttendanceSearch
            // 
            lblAttendanceSearch.AutoSize = true;
            lblAttendanceSearch.Location = new Point(584, 83);
            lblAttendanceSearch.Name = "lblAttendanceSearch";
            lblAttendanceSearch.Text = "Search";
            // 
            // txtAttendanceSearch
            // 
            txtAttendanceSearch.Location = new Point(634, 80);
            txtAttendanceSearch.Name = "txtAttendanceSearch";
            txtAttendanceSearch.Size = new Size(180, 23);
            txtAttendanceSearch.TabIndex = 6;
            // 
            // dgvAttendance
            // 
            dgvAttendance.AllowUserToAddRows = false;
            dgvAttendance.AllowUserToDeleteRows = false;
            dgvAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAttendance.Columns.AddRange(new DataGridViewColumn[] { colDate, colDay, colName, colTimeIn, colBreakOut, colBreakIn, colTimeOut, colLate, colEarly, colOvertime, colStatus, colFixed });
            dgvAttendance.MultiSelect = false;
            dgvAttendance.ReadOnly = true;
            dgvAttendance.RowHeadersVisible = false;
            dgvAttendance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAttendance.Location = new Point(12, 112);
            dgvAttendance.Name = "dgvAttendance";
            dgvAttendance.Size = new Size(930, 380);
            dgvAttendance.TabIndex = 7;
            dgvAttendance.SelectionChanged += dgvAttendance_SelectionChanged;
            dgvAttendance.CellFormatting += dgvAttendance_CellFormatting;
            dgvAttendance.CellDoubleClick += dgvAttendance_CellDoubleClick;
            // 
            // lblAttendanceCount
            // 
            lblAttendanceCount.AutoSize = true;
            lblAttendanceCount.Location = new Point(12, 500);
            lblAttendanceCount.Name = "lblAttendanceCount";
            lblAttendanceCount.Text = "";
            // 
            // btnFixScan
            // 
            btnFixScan.Location = new Point(12, 522);
            btnFixScan.Name = "btnFixScan";
            btnFixScan.Size = new Size(130, 28);
            btnFixScan.Text = "Fix Missing Scan";
            btnFixScan.TabIndex = 8;
            btnFixScan.Click += btnFixScan_Click;
            // 
            // btnApproveOvertime
            // 
            btnApproveOvertime.Location = new Point(148, 522);
            btnApproveOvertime.Name = "btnApproveOvertime";
            btnApproveOvertime.Size = new Size(130, 28);
            btnApproveOvertime.Text = "Approve Overtime";
            btnApproveOvertime.TabIndex = 9;
            btnApproveOvertime.Click += btnApproveOvertime_Click;
            // 
            // btnRejectOvertime
            // 
            btnRejectOvertime.Location = new Point(284, 522);
            btnRejectOvertime.Name = "btnRejectOvertime";
            btnRejectOvertime.Size = new Size(120, 28);
            btnRejectOvertime.Text = "Reject Overtime";
            btnRejectOvertime.TabIndex = 10;
            btnRejectOvertime.Click += btnRejectOvertime_Click;
            // 
            // btnExportAttendance
            // 
            btnExportAttendance.Location = new Point(410, 522);
            btnExportAttendance.Name = "btnExportAttendance";
            btnExportAttendance.Size = new Size(80, 28);
            btnExportAttendance.Text = "Export";
            btnExportAttendance.TabIndex = 11;
            btnExportAttendance.Click += btnExportAttendance_Click;
            // 
            // lblApproverNote
            // 
            lblApproverNote.AutoSize = true;
            lblApproverNote.Location = new Point(12, 558);
            lblApproverNote.Name = "lblApproverNote";
            lblApproverNote.Text = "The HR Supervisor or an Admin approves overtime.";
            // 
            // lblAttendanceMsg
            // 
            lblAttendanceMsg.AutoSize = true;
            lblAttendanceMsg.Location = new Point(12, 582);
            lblAttendanceMsg.Name = "lblAttendanceMsg";
            // 
            // colDate
            // 
            colDate.DataPropertyName = "Date";
            colDate.HeaderText = "Date";
            colDate.Name = "colDate";
            colDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colDay
            // 
            colDay.DataPropertyName = "Day";
            colDay.HeaderText = "Day";
            colDay.Name = "colDay";
            // 
            // colName
            // 
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            // 
            // colTimeIn
            // 
            colTimeIn.DataPropertyName = "TimeIn";
            colTimeIn.HeaderText = "Time In";
            colTimeIn.Name = "colTimeIn";
            colTimeIn.DefaultCellStyle = new DataGridViewCellStyle { Format = "h:mm tt", NullValue = "" };
            // 
            // colBreakOut
            // 
            colBreakOut.DataPropertyName = "BreakOut";
            colBreakOut.HeaderText = "Break Out";
            colBreakOut.Name = "colBreakOut";
            colBreakOut.DefaultCellStyle = new DataGridViewCellStyle { Format = "h:mm tt", NullValue = "" };
            // 
            // colBreakIn
            // 
            colBreakIn.DataPropertyName = "BreakIn";
            colBreakIn.HeaderText = "Break In";
            colBreakIn.Name = "colBreakIn";
            colBreakIn.DefaultCellStyle = new DataGridViewCellStyle { Format = "h:mm tt", NullValue = "" };
            // 
            // colTimeOut
            // 
            colTimeOut.DataPropertyName = "TimeOut";
            colTimeOut.HeaderText = "Time Out";
            colTimeOut.Name = "colTimeOut";
            colTimeOut.DefaultCellStyle = new DataGridViewCellStyle { Format = "h:mm tt", NullValue = "" };
            // 
            // colLate
            // 
            colLate.DataPropertyName = "LateMinutes";
            colLate.HeaderText = "Late (min)";
            colLate.Name = "colLate";
            colLate.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#", NullValue = "" };
            // 
            // colEarly
            // 
            colEarly.DataPropertyName = "EarlyMinutes";
            colEarly.HeaderText = "Early (min)";
            colEarly.Name = "colEarly";
            colEarly.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#", NullValue = "" };
            // 
            // colOvertime
            // 
            colOvertime.DataPropertyName = "Overtime";
            colOvertime.HeaderText = "Overtime";
            colOvertime.Name = "colOvertime";
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            // 
            // colFixed
            // 
            colFixed.DataPropertyName = "Fixed";
            colFixed.HeaderText = "Fixed";
            colFixed.Name = "colFixed";
            // 
            // AttendanceTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(btnUploadBiometrics);
            Controls.Add(lblImportSummary);
            Controls.Add(btnViewImportErrors);
            Controls.Add(btnExportToScanner);
            Controls.Add(lblAttendanceFrom);
            Controls.Add(dtpAttendanceFrom);
            Controls.Add(lblAttendanceTo);
            Controls.Add(dtpAttendanceTo);
            Controls.Add(lblAttendanceFilter);
            Controls.Add(cboAttendanceFilter);
            Controls.Add(lblAttendanceSearch);
            Controls.Add(txtAttendanceSearch);
            Controls.Add(dgvAttendance);
            Controls.Add(lblAttendanceCount);
            Controls.Add(btnFixScan);
            Controls.Add(btnApproveOvertime);
            Controls.Add(btnRejectOvertime);
            Controls.Add(btnExportAttendance);
            Controls.Add(lblApproverNote);
            Controls.Add(lblAttendanceMsg);
            Name = "AttendanceTab";
            Size = new Size(960, 610);
            Load += AttendanceTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnUploadBiometrics;
        private Label lblImportSummary;
        private Button btnViewImportErrors;
        private Button btnExportToScanner;
        private Label lblAttendanceFrom;
        private DateTimePicker dtpAttendanceFrom;
        private Label lblAttendanceTo;
        private DateTimePicker dtpAttendanceTo;
        private Label lblAttendanceFilter;
        private ComboBox cboAttendanceFilter;
        private Label lblAttendanceSearch;
        private TextBox txtAttendanceSearch;
        private DataGridView dgvAttendance;
        private Label lblAttendanceCount;
        private Button btnFixScan;
        private Button btnApproveOvertime;
        private Button btnRejectOvertime;
        private Button btnExportAttendance;
        private Label lblApproverNote;
        private Label lblAttendanceMsg;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colDay;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colTimeIn;
        private DataGridViewTextBoxColumn colBreakOut;
        private DataGridViewTextBoxColumn colBreakIn;
        private DataGridViewTextBoxColumn colTimeOut;
        private DataGridViewTextBoxColumn colLate;
        private DataGridViewTextBoxColumn colEarly;
        private DataGridViewTextBoxColumn colOvertime;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colFixed;
    }
}
