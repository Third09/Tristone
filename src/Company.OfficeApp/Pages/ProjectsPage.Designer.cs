namespace Company.OfficeApp.Pages
{
    partial class ProjectsPage
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
            txtProjectSearch = new TextBox();
            lblStatusFilter = new Label();
            cboStatusFilter = new ComboBox();
            lblSectorFilter = new Label();
            cboSectorFilter = new ComboBox();
            dgvProjects = new DataGridView();
            colProjectName = new DataGridViewTextBoxColumn();
            colSite = new DataGridViewTextBoxColumn();
            colClient = new DataGridViewTextBoxColumn();
            colSector = new DataGridViewTextBoxColumn();
            colContractPrice = new DataGridViewTextBoxColumn();
            colStart = new DataGridViewTextBoxColumn();
            colTargetEnd = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colOnWebsite = new DataGridViewCheckBoxColumn();
            btnAddProject = new Button();
            btnDeleteProject = new Button();
            btnExportProjects = new Button();
            lblFormMode = new Label();
            lblProjectName = new Label();
            txtProjectName = new TextBox();
            lblProjectSite = new Label();
            txtProjectSite = new TextBox();
            lblClientName = new Label();
            txtClientName = new TextBox();
            lblSector = new Label();
            cboSector = new ComboBox();
            lblContractPrice = new Label();
            numContractPrice = new NumericUpDown();
            lblStartDate = new Label();
            dtpStartDate = new DateTimePicker();
            lblTargetEndDate = new Label();
            dtpTargetEndDate = new DateTimePicker();
            lblProjectStatus = new Label();
            cboProjectStatus = new ComboBox();
            chkShowOnWebsite = new CheckBox();
            lblDaysSpent = new Label();
            lblRevenue = new Label();
            lblExpensesToDate = new Label();
            lblProfitToDate = new Label();
            lblProjectFormMsg = new Label();
            btnSaveProject = new Button();
            btnCancelProject = new Button();
            btnPrintProjectReport = new Button();
            ppdProjectReport = new PrintPreviewDialog();
            ((System.ComponentModel.ISupportInitialize)dgvProjects).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numContractPrice).BeginInit();
            SuspendLayout();
            //
            // lblSearch
            //
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(12, 15);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search";
            //
            // txtProjectSearch
            //
            txtProjectSearch.Location = new Point(62, 12);
            txtProjectSearch.Name = "txtProjectSearch";
            txtProjectSearch.Size = new Size(200, 23);
            txtProjectSearch.TabIndex = 0;
            txtProjectSearch.TextChanged += Filter_Changed;
            //
            // lblStatusFilter
            //
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(276, 15);
            lblStatusFilter.Name = "lblStatusFilter";
            lblStatusFilter.Text = "Status";
            //
            // cboStatusFilter
            //
            cboStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatusFilter.Location = new Point(322, 12);
            cboStatusFilter.Name = "cboStatusFilter";
            cboStatusFilter.Size = new Size(110, 23);
            cboStatusFilter.TabIndex = 1;
            cboStatusFilter.SelectedIndexChanged += Filter_Changed;
            //
            // lblSectorFilter
            //
            lblSectorFilter.AutoSize = true;
            lblSectorFilter.Location = new Point(446, 15);
            lblSectorFilter.Name = "lblSectorFilter";
            lblSectorFilter.Text = "Sector";
            //
            // cboSectorFilter
            //
            cboSectorFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSectorFilter.Location = new Point(494, 12);
            cboSectorFilter.Name = "cboSectorFilter";
            cboSectorFilter.Size = new Size(160, 23);
            cboSectorFilter.TabIndex = 2;
            cboSectorFilter.SelectedIndexChanged += Filter_Changed;
            //
            // dgvProjects
            //
            dgvProjects.AllowUserToAddRows = false;
            dgvProjects.AllowUserToDeleteRows = false;
            dgvProjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProjects.Columns.AddRange(new DataGridViewColumn[] { colProjectName, colSite, colClient, colSector, colContractPrice, colStart, colTargetEnd, colStatus, colOnWebsite });
            dgvProjects.Location = new Point(12, 44);
            dgvProjects.MultiSelect = false;
            dgvProjects.Name = "dgvProjects";
            dgvProjects.ReadOnly = true;
            dgvProjects.RowHeadersVisible = false;
            dgvProjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProjects.Size = new Size(900, 220);
            dgvProjects.TabIndex = 3;
            dgvProjects.SelectionChanged += dgvProjects_SelectionChanged;
            //
            // colProjectName
            //
            colProjectName.DataPropertyName = "ProjectName";
            colProjectName.HeaderText = "Project Name";
            colProjectName.Name = "colProjectName";
            //
            // colSite
            //
            colSite.DataPropertyName = "SiteLocation";
            colSite.HeaderText = "Site";
            colSite.Name = "colSite";
            //
            // colClient
            //
            colClient.DataPropertyName = "ClientName";
            colClient.HeaderText = "Client";
            colClient.Name = "colClient";
            //
            // colSector
            //
            colSector.DataPropertyName = "Sector";
            colSector.HeaderText = "Sector";
            colSector.Name = "colSector";
            //
            // colContractPrice
            //
            colContractPrice.DataPropertyName = "ContractPrice";
            colContractPrice.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            colContractPrice.HeaderText = "Contract Price";
            colContractPrice.Name = "colContractPrice";
            //
            // colStart
            //
            colStart.DataPropertyName = "StartDate";
            colStart.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            colStart.HeaderText = "Start";
            colStart.Name = "colStart";
            //
            // colTargetEnd
            //
            colTargetEnd.DataPropertyName = "TargetEndDate";
            colTargetEnd.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy", NullValue = "-" };
            colTargetEnd.HeaderText = "Target End";
            colTargetEnd.Name = "colTargetEnd";
            //
            // colStatus
            //
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            //
            // colOnWebsite
            //
            colOnWebsite.DataPropertyName = "ShowOnWebsite";
            colOnWebsite.HeaderText = "On Website";
            colOnWebsite.Name = "colOnWebsite";
            //
            // btnAddProject
            //
            btnAddProject.Location = new Point(12, 272);
            btnAddProject.Name = "btnAddProject";
            btnAddProject.Size = new Size(100, 28);
            btnAddProject.TabIndex = 4;
            btnAddProject.Text = "Add Project";
            btnAddProject.Click += btnAddProject_Click;
            //
            // btnDeleteProject
            //
            btnDeleteProject.Location = new Point(118, 272);
            btnDeleteProject.Name = "btnDeleteProject";
            btnDeleteProject.Size = new Size(90, 28);
            btnDeleteProject.TabIndex = 5;
            btnDeleteProject.Text = "Delete";
            btnDeleteProject.Click += btnDeleteProject_Click;
            //
            // btnExportProjects
            //
            btnExportProjects.Location = new Point(214, 272);
            btnExportProjects.Name = "btnExportProjects";
            btnExportProjects.Size = new Size(90, 28);
            btnExportProjects.TabIndex = 6;
            btnExportProjects.Text = "Export";
            btnExportProjects.Click += btnExportProjects_Click;
            //
            // lblFormMode
            //
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(12, 316);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New project";
            //
            // lblProjectName
            //
            lblProjectName.AutoSize = true;
            lblProjectName.Location = new Point(12, 342);
            lblProjectName.Name = "lblProjectName";
            lblProjectName.Text = "Project name";
            //
            // txtProjectName
            //
            txtProjectName.Location = new Point(12, 360);
            txtProjectName.Name = "txtProjectName";
            txtProjectName.Size = new Size(300, 23);
            txtProjectName.TabIndex = 7;
            //
            // lblProjectSite
            //
            lblProjectSite.AutoSize = true;
            lblProjectSite.Location = new Point(12, 390);
            lblProjectSite.Name = "lblProjectSite";
            lblProjectSite.Text = "Site / location";
            //
            // txtProjectSite
            //
            txtProjectSite.Location = new Point(12, 408);
            txtProjectSite.Name = "txtProjectSite";
            txtProjectSite.Size = new Size(300, 23);
            txtProjectSite.TabIndex = 8;
            //
            // lblClientName
            //
            lblClientName.AutoSize = true;
            lblClientName.Location = new Point(12, 438);
            lblClientName.Name = "lblClientName";
            lblClientName.Text = "Client (optional)";
            //
            // txtClientName
            //
            txtClientName.Location = new Point(12, 456);
            txtClientName.Name = "txtClientName";
            txtClientName.Size = new Size(300, 23);
            txtClientName.TabIndex = 9;
            //
            // lblSector
            //
            lblSector.AutoSize = true;
            lblSector.Location = new Point(12, 486);
            lblSector.Name = "lblSector";
            lblSector.Text = "Sector";
            //
            // cboSector
            //
            cboSector.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSector.Location = new Point(12, 504);
            cboSector.Name = "cboSector";
            cboSector.Size = new Size(200, 23);
            cboSector.TabIndex = 10;
            //
            // lblContractPrice
            //
            lblContractPrice.AutoSize = true;
            lblContractPrice.Location = new Point(12, 534);
            lblContractPrice.Name = "lblContractPrice";
            lblContractPrice.Text = "Contract price (pesos)";
            //
            // numContractPrice
            //
            numContractPrice.DecimalPlaces = 2;
            numContractPrice.Location = new Point(12, 552);
            numContractPrice.Maximum = new decimal(new int[] { -727379969, 232, 0, 131072 });
            numContractPrice.Name = "numContractPrice";
            numContractPrice.Size = new Size(200, 23);
            numContractPrice.TabIndex = 11;
            numContractPrice.ThousandsSeparator = true;
            //
            // lblStartDate
            //
            lblStartDate.AutoSize = true;
            lblStartDate.Location = new Point(12, 582);
            lblStartDate.Name = "lblStartDate";
            lblStartDate.Text = "Start date";
            //
            // dtpStartDate
            //
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Location = new Point(12, 600);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(140, 23);
            dtpStartDate.TabIndex = 12;
            //
            // lblTargetEndDate
            //
            lblTargetEndDate.AutoSize = true;
            lblTargetEndDate.Location = new Point(12, 630);
            lblTargetEndDate.Name = "lblTargetEndDate";
            lblTargetEndDate.Text = "Target end date (tick to set)";
            //
            // dtpTargetEndDate
            //
            dtpTargetEndDate.Format = DateTimePickerFormat.Short;
            dtpTargetEndDate.Location = new Point(12, 648);
            dtpTargetEndDate.Name = "dtpTargetEndDate";
            dtpTargetEndDate.ShowCheckBox = true;
            dtpTargetEndDate.Size = new Size(140, 23);
            dtpTargetEndDate.TabIndex = 13;
            //
            // lblProjectStatus
            //
            lblProjectStatus.AutoSize = true;
            lblProjectStatus.Location = new Point(12, 678);
            lblProjectStatus.Name = "lblProjectStatus";
            lblProjectStatus.Text = "Status";
            //
            // cboProjectStatus
            //
            cboProjectStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProjectStatus.Location = new Point(12, 696);
            cboProjectStatus.Name = "cboProjectStatus";
            cboProjectStatus.Size = new Size(140, 23);
            cboProjectStatus.TabIndex = 14;
            //
            // chkShowOnWebsite
            //
            chkShowOnWebsite.AutoSize = true;
            chkShowOnWebsite.Location = new Point(12, 728);
            chkShowOnWebsite.Name = "chkShowOnWebsite";
            chkShowOnWebsite.TabIndex = 15;
            chkShowOnWebsite.Text = "Show on website (sends it to Website Admin)";
            //
            // lblDaysSpent
            //
            lblDaysSpent.AutoSize = true;
            lblDaysSpent.Location = new Point(360, 342);
            lblDaysSpent.Name = "lblDaysSpent";
            lblDaysSpent.Text = "Days: -";
            //
            // lblRevenue
            //
            lblRevenue.AutoSize = true;
            lblRevenue.Location = new Point(360, 364);
            lblRevenue.Name = "lblRevenue";
            lblRevenue.Text = "Revenue: -";
            //
            // lblExpensesToDate
            //
            lblExpensesToDate.AutoSize = true;
            lblExpensesToDate.Location = new Point(360, 386);
            lblExpensesToDate.Name = "lblExpensesToDate";
            lblExpensesToDate.Text = "Expenses to date: -";
            //
            // lblProfitToDate
            //
            lblProfitToDate.AutoSize = true;
            lblProfitToDate.Location = new Point(360, 408);
            lblProfitToDate.Name = "lblProfitToDate";
            lblProfitToDate.Text = "Profit to date: -";
            //
            // lblProjectFormMsg
            //
            lblProjectFormMsg.AutoSize = true;
            lblProjectFormMsg.Location = new Point(12, 756);
            lblProjectFormMsg.Name = "lblProjectFormMsg";
            //
            // btnSaveProject
            //
            btnSaveProject.Location = new Point(12, 778);
            btnSaveProject.Name = "btnSaveProject";
            btnSaveProject.Size = new Size(90, 28);
            btnSaveProject.TabIndex = 16;
            btnSaveProject.Text = "Save";
            btnSaveProject.Click += btnSaveProject_Click;
            //
            // btnCancelProject
            //
            btnCancelProject.Location = new Point(108, 778);
            btnCancelProject.Name = "btnCancelProject";
            btnCancelProject.Size = new Size(90, 28);
            btnCancelProject.TabIndex = 17;
            btnCancelProject.Text = "Cancel";
            btnCancelProject.Click += btnCancelProject_Click;
            //
            // btnPrintProjectReport
            //
            btnPrintProjectReport.Location = new Point(204, 778);
            btnPrintProjectReport.Name = "btnPrintProjectReport";
            btnPrintProjectReport.Size = new Size(110, 28);
            btnPrintProjectReport.TabIndex = 18;
            btnPrintProjectReport.Text = "Print Report";
            btnPrintProjectReport.Click += btnPrintProjectReport_Click;
            //
            // ppdProjectReport
            //
            ppdProjectReport.Name = "ppdProjectReport";
            ppdProjectReport.Text = "Project Report";
            ppdProjectReport.UseAntiAlias = true;
            //
            // ProjectsPage
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblSearch);
            Controls.Add(txtProjectSearch);
            Controls.Add(lblStatusFilter);
            Controls.Add(cboStatusFilter);
            Controls.Add(lblSectorFilter);
            Controls.Add(cboSectorFilter);
            Controls.Add(dgvProjects);
            Controls.Add(btnAddProject);
            Controls.Add(btnDeleteProject);
            Controls.Add(btnExportProjects);
            Controls.Add(lblFormMode);
            Controls.Add(lblProjectName);
            Controls.Add(txtProjectName);
            Controls.Add(lblProjectSite);
            Controls.Add(txtProjectSite);
            Controls.Add(lblClientName);
            Controls.Add(txtClientName);
            Controls.Add(lblSector);
            Controls.Add(cboSector);
            Controls.Add(lblContractPrice);
            Controls.Add(numContractPrice);
            Controls.Add(lblStartDate);
            Controls.Add(dtpStartDate);
            Controls.Add(lblTargetEndDate);
            Controls.Add(dtpTargetEndDate);
            Controls.Add(lblProjectStatus);
            Controls.Add(cboProjectStatus);
            Controls.Add(chkShowOnWebsite);
            Controls.Add(lblDaysSpent);
            Controls.Add(lblRevenue);
            Controls.Add(lblExpensesToDate);
            Controls.Add(lblProfitToDate);
            Controls.Add(lblProjectFormMsg);
            Controls.Add(btnSaveProject);
            Controls.Add(btnCancelProject);
            Controls.Add(btnPrintProjectReport);
            Name = "ProjectsPage";
            Size = new Size(930, 820);
            Load += ProjectsPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProjects).EndInit();
            ((System.ComponentModel.ISupportInitialize)numContractPrice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSearch;
        private TextBox txtProjectSearch;
        private Label lblStatusFilter;
        private ComboBox cboStatusFilter;
        private Label lblSectorFilter;
        private ComboBox cboSectorFilter;
        private DataGridView dgvProjects;
        private DataGridViewTextBoxColumn colProjectName;
        private DataGridViewTextBoxColumn colSite;
        private DataGridViewTextBoxColumn colClient;
        private DataGridViewTextBoxColumn colSector;
        private DataGridViewTextBoxColumn colContractPrice;
        private DataGridViewTextBoxColumn colStart;
        private DataGridViewTextBoxColumn colTargetEnd;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewCheckBoxColumn colOnWebsite;
        private Button btnAddProject;
        private Button btnDeleteProject;
        private Button btnExportProjects;
        private Label lblFormMode;
        private Label lblProjectName;
        private TextBox txtProjectName;
        private Label lblProjectSite;
        private TextBox txtProjectSite;
        private Label lblClientName;
        private TextBox txtClientName;
        private Label lblSector;
        private ComboBox cboSector;
        private Label lblContractPrice;
        private NumericUpDown numContractPrice;
        private Label lblStartDate;
        private DateTimePicker dtpStartDate;
        private Label lblTargetEndDate;
        private DateTimePicker dtpTargetEndDate;
        private Label lblProjectStatus;
        private ComboBox cboProjectStatus;
        private CheckBox chkShowOnWebsite;
        private Label lblDaysSpent;
        private Label lblRevenue;
        private Label lblExpensesToDate;
        private Label lblProfitToDate;
        private Label lblProjectFormMsg;
        private Button btnSaveProject;
        private Button btnCancelProject;
        private Button btnPrintProjectReport;
        private PrintPreviewDialog ppdProjectReport;
    }
}
