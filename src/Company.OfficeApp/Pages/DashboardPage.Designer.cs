namespace Company.OfficeApp.Pages
{
    partial class DashboardPage
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
            lblPeriod = new Label();
            cboPeriod = new ComboBox();
            lblFrom = new Label();
            dtpFrom = new DateTimePicker();
            lblTo = new Label();
            dtpTo = new DateTimePicker();
            lblDepartment = new Label();
            cboDepartment = new ComboBox();
            btnRefreshDashboard = new Button();
            pnlKpiCompleted = new Panel();
            lblKpiCompletedTitle = new Label();
            lblKpiCompleted = new Label();
            pnlKpiRevenue = new Panel();
            lblKpiRevenueTitle = new Label();
            lblKpiRevenue = new Label();
            pnlKpiExpenses = new Panel();
            lblKpiExpensesTitle = new Label();
            lblKpiExpenses = new Label();
            pnlKpiNet = new Panel();
            lblKpiNetTitle = new Label();
            lblKpiNet = new Label();
            lblKpiBreakdown = new Label();
            lblChartTitle = new Label();
            chartProjectStatus = new ScottPlot.WinForms.FormsPlot();
            lblTopProjects = new Label();
            dgvTopProjects = new DataGridView();
            btnExportTopProjects = new Button();
            lblLogTitle = new Label();
            lblLogType = new Label();
            cboLogType = new ComboBox();
            lblLogSearch = new Label();
            txtLogSearch = new TextBox();
            btnExportLogs = new Button();
            dgvLogHistory = new DataGridView();
            pnlKpiCompleted.Controls.Add(lblKpiCompletedTitle);
            pnlKpiCompleted.Controls.Add(lblKpiCompleted);
            pnlKpiRevenue.Controls.Add(lblKpiRevenueTitle);
            pnlKpiRevenue.Controls.Add(lblKpiRevenue);
            pnlKpiExpenses.Controls.Add(lblKpiExpensesTitle);
            pnlKpiExpenses.Controls.Add(lblKpiExpenses);
            pnlKpiNet.Controls.Add(lblKpiNetTitle);
            pnlKpiNet.Controls.Add(lblKpiNet);
            colTopProject = new DataGridViewTextBoxColumn();
            colTopRevenue = new DataGridViewTextBoxColumn();
            colTopExpenses = new DataGridViewTextBoxColumn();
            colTopProfit = new DataGridViewTextBoxColumn();
            colTopMargin = new DataGridViewTextBoxColumn();
            colLogWhen = new DataGridViewTextBoxColumn();
            colLogName = new DataGridViewTextBoxColumn();
            colLogDept = new DataGridViewTextBoxColumn();
            colLogActivity = new DataGridViewTextBoxColumn();
            colLogDetails = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvTopProjects).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLogHistory).BeginInit();
            SuspendLayout();
            // 
            // lblPeriod
            // 
            lblPeriod.AutoSize = true;
            lblPeriod.Location = new Point(12, 15);
            lblPeriod.Name = "lblPeriod";
            lblPeriod.Text = "Period";
            // 
            // cboPeriod
            // 
            cboPeriod.Location = new Point(60, 12);
            cboPeriod.Name = "cboPeriod";
            cboPeriod.Size = new Size(130, 23);
            cboPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPeriod.TabIndex = 0;
            cboPeriod.SelectedIndexChanged += cboPeriod_SelectedIndexChanged;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Location = new Point(202, 15);
            lblFrom.Name = "lblFrom";
            lblFrom.Text = "From";
            // 
            // dtpFrom
            // 
            dtpFrom.Location = new Point(240, 12);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(115, 23);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.TabIndex = 1;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Location = new Point(365, 15);
            lblTo.Name = "lblTo";
            lblTo.Text = "To";
            // 
            // dtpTo
            // 
            dtpTo.Location = new Point(390, 12);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(115, 23);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.TabIndex = 2;
            // 
            // lblDepartment
            // 
            lblDepartment.AutoSize = true;
            lblDepartment.Location = new Point(517, 15);
            lblDepartment.Name = "lblDepartment";
            lblDepartment.Text = "Department";
            // 
            // cboDepartment
            // 
            cboDepartment.Location = new Point(600, 12);
            cboDepartment.Name = "cboDepartment";
            cboDepartment.Size = new Size(170, 23);
            cboDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDepartment.TabIndex = 3;
            // 
            // btnRefreshDashboard
            // 
            btnRefreshDashboard.Location = new Point(782, 10);
            btnRefreshDashboard.Name = "btnRefreshDashboard";
            btnRefreshDashboard.Size = new Size(90, 28);
            btnRefreshDashboard.Text = "Refresh";
            btnRefreshDashboard.TabIndex = 4;
            btnRefreshDashboard.Click += btnRefreshDashboard_Click;
            // 
            // pnlKpiCompleted
            // 
            pnlKpiCompleted.Location = new Point(12, 48);
            pnlKpiCompleted.Name = "pnlKpiCompleted";
            pnlKpiCompleted.Size = new Size(225, 64);
            pnlKpiCompleted.BorderStyle = BorderStyle.FixedSingle;
            pnlKpiCompleted.TabIndex = 5;
            // 
            // lblKpiCompletedTitle
            // 
            lblKpiCompletedTitle.AutoSize = true;
            lblKpiCompletedTitle.Location = new Point(8, 8);
            lblKpiCompletedTitle.Name = "lblKpiCompletedTitle";
            lblKpiCompletedTitle.Text = "Completed Projects";
            // 
            // lblKpiCompleted
            // 
            lblKpiCompleted.AutoSize = true;
            lblKpiCompleted.Location = new Point(8, 32);
            lblKpiCompleted.Name = "lblKpiCompleted";
            lblKpiCompleted.Text = "-";
            // 
            // pnlKpiRevenue
            // 
            pnlKpiRevenue.Location = new Point(245, 48);
            pnlKpiRevenue.Name = "pnlKpiRevenue";
            pnlKpiRevenue.Size = new Size(225, 64);
            pnlKpiRevenue.BorderStyle = BorderStyle.FixedSingle;
            pnlKpiRevenue.TabIndex = 6;
            // 
            // lblKpiRevenueTitle
            // 
            lblKpiRevenueTitle.AutoSize = true;
            lblKpiRevenueTitle.Location = new Point(8, 8);
            lblKpiRevenueTitle.Name = "lblKpiRevenueTitle";
            lblKpiRevenueTitle.Text = "Gross Revenue";
            // 
            // lblKpiRevenue
            // 
            lblKpiRevenue.AutoSize = true;
            lblKpiRevenue.Location = new Point(8, 32);
            lblKpiRevenue.Name = "lblKpiRevenue";
            lblKpiRevenue.Text = "-";
            // 
            // pnlKpiExpenses
            // 
            pnlKpiExpenses.Location = new Point(478, 48);
            pnlKpiExpenses.Name = "pnlKpiExpenses";
            pnlKpiExpenses.Size = new Size(225, 64);
            pnlKpiExpenses.BorderStyle = BorderStyle.FixedSingle;
            pnlKpiExpenses.TabIndex = 7;
            // 
            // lblKpiExpensesTitle
            // 
            lblKpiExpensesTitle.AutoSize = true;
            lblKpiExpensesTitle.Location = new Point(8, 8);
            lblKpiExpensesTitle.Name = "lblKpiExpensesTitle";
            lblKpiExpensesTitle.Text = "Operational Expenses";
            // 
            // lblKpiExpenses
            // 
            lblKpiExpenses.AutoSize = true;
            lblKpiExpenses.Location = new Point(8, 32);
            lblKpiExpenses.Name = "lblKpiExpenses";
            lblKpiExpenses.Text = "-";
            // 
            // pnlKpiNet
            // 
            pnlKpiNet.Location = new Point(711, 48);
            pnlKpiNet.Name = "pnlKpiNet";
            pnlKpiNet.Size = new Size(225, 64);
            pnlKpiNet.BorderStyle = BorderStyle.FixedSingle;
            pnlKpiNet.TabIndex = 8;
            // 
            // lblKpiNetTitle
            // 
            lblKpiNetTitle.AutoSize = true;
            lblKpiNetTitle.Location = new Point(8, 8);
            lblKpiNetTitle.Name = "lblKpiNetTitle";
            lblKpiNetTitle.Text = "Net Revenue";
            // 
            // lblKpiNet
            // 
            lblKpiNet.AutoSize = true;
            lblKpiNet.Location = new Point(8, 32);
            lblKpiNet.Name = "lblKpiNet";
            lblKpiNet.Text = "-";
            // 
            // lblKpiBreakdown
            // 
            lblKpiBreakdown.AutoSize = true;
            lblKpiBreakdown.Location = new Point(12, 120);
            lblKpiBreakdown.Name = "lblKpiBreakdown";
            lblKpiBreakdown.Text = "";
            lblKpiBreakdown.MaximumSize = new Size(930, 0);
            // 
            // lblChartTitle
            // 
            lblChartTitle.AutoSize = true;
            lblChartTitle.Location = new Point(12, 168);
            lblChartTitle.Name = "lblChartTitle";
            lblChartTitle.Text = "Project status (all projects, today)";
            // 
            // chartProjectStatus
            // 
            chartProjectStatus.Location = new Point(12, 188);
            chartProjectStatus.Name = "chartProjectStatus";
            chartProjectStatus.Size = new Size(440, 260);
            chartProjectStatus.TabIndex = 9;
            // 
            // lblTopProjects
            // 
            lblTopProjects.AutoSize = true;
            lblTopProjects.Location = new Point(470, 168);
            lblTopProjects.Name = "lblTopProjects";
            lblTopProjects.Text = "Top performing projects (active in the period; click a column title to sort)";
            // 
            // dgvTopProjects
            // 
            dgvTopProjects.AllowUserToAddRows = false;
            dgvTopProjects.AllowUserToDeleteRows = false;
            dgvTopProjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTopProjects.Columns.AddRange(new DataGridViewColumn[] { colTopProject, colTopRevenue, colTopExpenses, colTopProfit, colTopMargin });
            dgvTopProjects.MultiSelect = false;
            dgvTopProjects.ReadOnly = true;
            dgvTopProjects.RowHeadersVisible = false;
            dgvTopProjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTopProjects.Location = new Point(470, 188);
            dgvTopProjects.Name = "dgvTopProjects";
            dgvTopProjects.Size = new Size(472, 260);
            dgvTopProjects.TabIndex = 10;
            dgvTopProjects.ColumnHeaderMouseClick += dgvTopProjects_ColumnHeaderMouseClick;
            // 
            // btnExportTopProjects
            // 
            btnExportTopProjects.Location = new Point(470, 454);
            btnExportTopProjects.Name = "btnExportTopProjects";
            btnExportTopProjects.Size = new Size(90, 28);
            btnExportTopProjects.Text = "Export";
            btnExportTopProjects.TabIndex = 11;
            btnExportTopProjects.Click += btnExportTopProjects_Click;
            // 
            // lblLogTitle
            // 
            lblLogTitle.AutoSize = true;
            lblLogTitle.Location = new Point(12, 494);
            lblLogTitle.Name = "lblLogTitle";
            lblLogTitle.Text = "Log history";
            // 
            // lblLogType
            // 
            lblLogType.AutoSize = true;
            lblLogType.Location = new Point(12, 520);
            lblLogType.Name = "lblLogType";
            lblLogType.Text = "Activity";
            // 
            // cboLogType
            // 
            cboLogType.Location = new Point(70, 517);
            cboLogType.Name = "cboLogType";
            cboLogType.Size = new Size(170, 23);
            cboLogType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLogType.TabIndex = 12;
            cboLogType.SelectedIndexChanged += cboLogType_SelectedIndexChanged;
            // 
            // lblLogSearch
            // 
            lblLogSearch.AutoSize = true;
            lblLogSearch.Location = new Point(252, 520);
            lblLogSearch.Name = "lblLogSearch";
            lblLogSearch.Text = "Search";
            // 
            // txtLogSearch
            // 
            txtLogSearch.Location = new Point(302, 517);
            txtLogSearch.Name = "txtLogSearch";
            txtLogSearch.Size = new Size(200, 23);
            txtLogSearch.TabIndex = 13;
            // 
            // btnExportLogs
            // 
            btnExportLogs.Location = new Point(514, 515);
            btnExportLogs.Name = "btnExportLogs";
            btnExportLogs.Size = new Size(90, 28);
            btnExportLogs.Text = "Export";
            btnExportLogs.TabIndex = 14;
            btnExportLogs.Click += btnExportLogs_Click;
            // 
            // dgvLogHistory
            // 
            dgvLogHistory.AllowUserToAddRows = false;
            dgvLogHistory.AllowUserToDeleteRows = false;
            dgvLogHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLogHistory.Columns.AddRange(new DataGridViewColumn[] { colLogWhen, colLogName, colLogDept, colLogActivity, colLogDetails });
            dgvLogHistory.MultiSelect = false;
            dgvLogHistory.ReadOnly = true;
            dgvLogHistory.RowHeadersVisible = false;
            dgvLogHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogHistory.Location = new Point(12, 548);
            dgvLogHistory.Name = "dgvLogHistory";
            dgvLogHistory.Size = new Size(930, 190);
            dgvLogHistory.TabIndex = 15;
            // 
            // colTopProject
            // 
            colTopProject.DataPropertyName = "Project";
            colTopProject.HeaderText = "Project";
            colTopProject.Name = "colTopProject";
            colTopProject.SortMode = DataGridViewColumnSortMode.Programmatic;
            // 
            // colTopRevenue
            // 
            colTopRevenue.DataPropertyName = "Revenue";
            colTopRevenue.HeaderText = "Revenue";
            colTopRevenue.Name = "colTopRevenue";
            colTopRevenue.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            colTopRevenue.SortMode = DataGridViewColumnSortMode.Programmatic;
            // 
            // colTopExpenses
            // 
            colTopExpenses.DataPropertyName = "Expenses";
            colTopExpenses.HeaderText = "Expenses";
            colTopExpenses.Name = "colTopExpenses";
            colTopExpenses.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            colTopExpenses.SortMode = DataGridViewColumnSortMode.Programmatic;
            // 
            // colTopProfit
            // 
            colTopProfit.DataPropertyName = "Profit";
            colTopProfit.HeaderText = "Profit";
            colTopProfit.Name = "colTopProfit";
            colTopProfit.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            colTopProfit.SortMode = DataGridViewColumnSortMode.Programmatic;
            // 
            // colTopMargin
            // 
            colTopMargin.DataPropertyName = "MarginPercent";
            colTopMargin.HeaderText = "Margin %";
            colTopMargin.Name = "colTopMargin";
            colTopMargin.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "0.0'%'", NullValue = "-" };
            colTopMargin.SortMode = DataGridViewColumnSortMode.Programmatic;
            // 
            // colLogWhen
            // 
            colLogWhen.DataPropertyName = "When";
            colLogWhen.HeaderText = "Date & Time";
            colLogWhen.Name = "colLogWhen";
            colLogWhen.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy h:mm tt" };
            // 
            // colLogName
            // 
            colLogName.DataPropertyName = "Name";
            colLogName.HeaderText = "Name";
            colLogName.Name = "colLogName";
            // 
            // colLogDept
            // 
            colLogDept.DataPropertyName = "Department";
            colLogDept.HeaderText = "Department";
            colLogDept.Name = "colLogDept";
            // 
            // colLogActivity
            // 
            colLogActivity.DataPropertyName = "Activity";
            colLogActivity.HeaderText = "Activity";
            colLogActivity.Name = "colLogActivity";
            // 
            // colLogDetails
            // 
            colLogDetails.DataPropertyName = "Details";
            colLogDetails.HeaderText = "Details";
            colLogDetails.Name = "colLogDetails";
            // 
            // DashboardPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblPeriod);
            Controls.Add(cboPeriod);
            Controls.Add(lblFrom);
            Controls.Add(dtpFrom);
            Controls.Add(lblTo);
            Controls.Add(dtpTo);
            Controls.Add(lblDepartment);
            Controls.Add(cboDepartment);
            Controls.Add(btnRefreshDashboard);
            Controls.Add(pnlKpiCompleted);
            Controls.Add(pnlKpiRevenue);
            Controls.Add(pnlKpiExpenses);
            Controls.Add(pnlKpiNet);
            Controls.Add(lblKpiBreakdown);
            Controls.Add(lblChartTitle);
            Controls.Add(chartProjectStatus);
            Controls.Add(lblTopProjects);
            Controls.Add(dgvTopProjects);
            Controls.Add(btnExportTopProjects);
            Controls.Add(lblLogTitle);
            Controls.Add(lblLogType);
            Controls.Add(cboLogType);
            Controls.Add(lblLogSearch);
            Controls.Add(txtLogSearch);
            Controls.Add(btnExportLogs);
            Controls.Add(dgvLogHistory);
            Name = "DashboardPage";
            Size = new Size(960, 756);
            Load += DashboardPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTopProjects).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLogHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPeriod;
        private ComboBox cboPeriod;
        private Label lblFrom;
        private DateTimePicker dtpFrom;
        private Label lblTo;
        private DateTimePicker dtpTo;
        private Label lblDepartment;
        private ComboBox cboDepartment;
        private Button btnRefreshDashboard;
        private Panel pnlKpiCompleted;
        private Label lblKpiCompletedTitle;
        private Label lblKpiCompleted;
        private Panel pnlKpiRevenue;
        private Label lblKpiRevenueTitle;
        private Label lblKpiRevenue;
        private Panel pnlKpiExpenses;
        private Label lblKpiExpensesTitle;
        private Label lblKpiExpenses;
        private Panel pnlKpiNet;
        private Label lblKpiNetTitle;
        private Label lblKpiNet;
        private Label lblKpiBreakdown;
        private Label lblChartTitle;
        private ScottPlot.WinForms.FormsPlot chartProjectStatus;
        private Label lblTopProjects;
        private DataGridView dgvTopProjects;
        private Button btnExportTopProjects;
        private Label lblLogTitle;
        private Label lblLogType;
        private ComboBox cboLogType;
        private Label lblLogSearch;
        private TextBox txtLogSearch;
        private Button btnExportLogs;
        private DataGridView dgvLogHistory;
        private DataGridViewTextBoxColumn colTopProject;
        private DataGridViewTextBoxColumn colTopRevenue;
        private DataGridViewTextBoxColumn colTopExpenses;
        private DataGridViewTextBoxColumn colTopProfit;
        private DataGridViewTextBoxColumn colTopMargin;
        private DataGridViewTextBoxColumn colLogWhen;
        private DataGridViewTextBoxColumn colLogName;
        private DataGridViewTextBoxColumn colLogDept;
        private DataGridViewTextBoxColumn colLogActivity;
        private DataGridViewTextBoxColumn colLogDetails;
    }
}
