namespace Company.OfficeApp.Pages.Receipts
{
    partial class ReceiptVaultTab
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
            lblReceiptProject = new Label();
            cboReceiptProject = new ComboBox();
            lblReceiptFrom = new Label();
            dtpReceiptFrom = new DateTimePicker();
            lblReceiptTo = new Label();
            dtpReceiptTo = new DateTimePicker();
            lblReceiptCategory = new Label();
            cboReceiptCategoryFilter = new ComboBox();
            lblReceiptStatus = new Label();
            cboReceiptStatusFilter = new ComboBox();
            lblReceiptSearch = new Label();
            txtReceiptSearch = new TextBox();
            dgvReceipts = new DataGridView();
            lblReceiptsTotal = new Label();
            btnAddReceipt = new Button();
            btnOpenReceipt = new Button();
            btnEditReceipt = new Button();
            btnApproveReceipt = new Button();
            btnRejectReceipt = new Button();
            btnPrintSelectedReceipts = new Button();
            btnExportReceipts = new Button();
            lblApproverNote = new Label();
            lblVaultMsg = new Label();
            colReceiptNo = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();
            colMerchant = new DataGridViewTextBoxColumn();
            colAmount = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colProject = new DataGridViewTextBoxColumn();
            colCopy = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colUploadedBy = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvReceipts).BeginInit();
            SuspendLayout();
            // 
            // lblReceiptProject
            // 
            lblReceiptProject.AutoSize = true;
            lblReceiptProject.Location = new Point(12, 15);
            lblReceiptProject.Name = "lblReceiptProject";
            lblReceiptProject.Text = "Project";
            // 
            // cboReceiptProject
            // 
            cboReceiptProject.Location = new Point(80, 12);
            cboReceiptProject.Name = "cboReceiptProject";
            cboReceiptProject.Size = new Size(220, 23);
            cboReceiptProject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReceiptProject.TabIndex = 0;
            cboReceiptProject.SelectedIndexChanged += Filter_Changed;
            // 
            // lblReceiptFrom
            // 
            lblReceiptFrom.AutoSize = true;
            lblReceiptFrom.Location = new Point(312, 15);
            lblReceiptFrom.Name = "lblReceiptFrom";
            lblReceiptFrom.Text = "From";
            // 
            // dtpReceiptFrom
            // 
            dtpReceiptFrom.Location = new Point(352, 12);
            dtpReceiptFrom.Name = "dtpReceiptFrom";
            dtpReceiptFrom.Size = new Size(115, 23);
            dtpReceiptFrom.Format = DateTimePickerFormat.Short;
            dtpReceiptFrom.TabIndex = 1;
            dtpReceiptFrom.ValueChanged += Filter_Changed;
            // 
            // lblReceiptTo
            // 
            lblReceiptTo.AutoSize = true;
            lblReceiptTo.Location = new Point(477, 15);
            lblReceiptTo.Name = "lblReceiptTo";
            lblReceiptTo.Text = "To";
            // 
            // dtpReceiptTo
            // 
            dtpReceiptTo.Location = new Point(503, 12);
            dtpReceiptTo.Name = "dtpReceiptTo";
            dtpReceiptTo.Size = new Size(115, 23);
            dtpReceiptTo.Format = DateTimePickerFormat.Short;
            dtpReceiptTo.TabIndex = 2;
            dtpReceiptTo.ValueChanged += Filter_Changed;
            // 
            // lblReceiptCategory
            // 
            lblReceiptCategory.AutoSize = true;
            lblReceiptCategory.Location = new Point(12, 45);
            lblReceiptCategory.Name = "lblReceiptCategory";
            lblReceiptCategory.Text = "Category";
            // 
            // cboReceiptCategoryFilter
            // 
            cboReceiptCategoryFilter.Location = new Point(80, 42);
            cboReceiptCategoryFilter.Name = "cboReceiptCategoryFilter";
            cboReceiptCategoryFilter.Size = new Size(150, 23);
            cboReceiptCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReceiptCategoryFilter.TabIndex = 3;
            cboReceiptCategoryFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblReceiptStatus
            // 
            lblReceiptStatus.AutoSize = true;
            lblReceiptStatus.Location = new Point(242, 45);
            lblReceiptStatus.Name = "lblReceiptStatus";
            lblReceiptStatus.Text = "Status";
            // 
            // cboReceiptStatusFilter
            // 
            cboReceiptStatusFilter.Location = new Point(290, 42);
            cboReceiptStatusFilter.Name = "cboReceiptStatusFilter";
            cboReceiptStatusFilter.Size = new Size(140, 23);
            cboReceiptStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReceiptStatusFilter.TabIndex = 4;
            cboReceiptStatusFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblReceiptSearch
            // 
            lblReceiptSearch.AutoSize = true;
            lblReceiptSearch.Location = new Point(442, 45);
            lblReceiptSearch.Name = "lblReceiptSearch";
            lblReceiptSearch.Text = "Search";
            // 
            // txtReceiptSearch
            // 
            txtReceiptSearch.Location = new Point(492, 42);
            txtReceiptSearch.Name = "txtReceiptSearch";
            txtReceiptSearch.Size = new Size(200, 23);
            txtReceiptSearch.TabIndex = 5;
            // 
            // dgvReceipts
            // 
            dgvReceipts.AllowUserToAddRows = false;
            dgvReceipts.AllowUserToDeleteRows = false;
            dgvReceipts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReceipts.Columns.AddRange(new DataGridViewColumn[] { colReceiptNo, colDate, colMerchant, colAmount, colCategory, colProject, colCopy, colStock, colStatus, colUploadedBy });
            dgvReceipts.MultiSelect = false;
            dgvReceipts.ReadOnly = true;
            dgvReceipts.RowHeadersVisible = false;
            dgvReceipts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReceipts.Location = new Point(12, 74);
            dgvReceipts.Name = "dgvReceipts";
            dgvReceipts.Size = new Size(930, 320);
            dgvReceipts.MultiSelect = true;
            dgvReceipts.TabIndex = 6;
            dgvReceipts.SelectionChanged += dgvReceipts_SelectionChanged;
            dgvReceipts.CellDoubleClick += dgvReceipts_CellDoubleClick;
            // 
            // lblReceiptsTotal
            // 
            lblReceiptsTotal.AutoSize = true;
            lblReceiptsTotal.Location = new Point(12, 402);
            lblReceiptsTotal.Name = "lblReceiptsTotal";
            lblReceiptsTotal.Text = "";
            // 
            // btnAddReceipt
            // 
            btnAddReceipt.Location = new Point(12, 426);
            btnAddReceipt.Name = "btnAddReceipt";
            btnAddReceipt.Size = new Size(100, 28);
            btnAddReceipt.Text = "Add Receipt";
            btnAddReceipt.TabIndex = 7;
            btnAddReceipt.Click += btnAddReceipt_Click;
            // 
            // btnOpenReceipt
            // 
            btnOpenReceipt.Location = new Point(118, 426);
            btnOpenReceipt.Name = "btnOpenReceipt";
            btnOpenReceipt.Size = new Size(70, 28);
            btnOpenReceipt.Text = "Open";
            btnOpenReceipt.TabIndex = 8;
            btnOpenReceipt.Click += btnOpenReceipt_Click;
            // 
            // btnEditReceipt
            // 
            btnEditReceipt.Location = new Point(194, 426);
            btnEditReceipt.Name = "btnEditReceipt";
            btnEditReceipt.Size = new Size(70, 28);
            btnEditReceipt.Text = "Edit";
            btnEditReceipt.TabIndex = 9;
            btnEditReceipt.Click += btnEditReceipt_Click;
            // 
            // btnApproveReceipt
            // 
            btnApproveReceipt.Location = new Point(270, 426);
            btnApproveReceipt.Name = "btnApproveReceipt";
            btnApproveReceipt.Size = new Size(80, 28);
            btnApproveReceipt.Text = "Approve";
            btnApproveReceipt.TabIndex = 10;
            btnApproveReceipt.Click += btnApproveReceipt_Click;
            // 
            // btnRejectReceipt
            // 
            btnRejectReceipt.Location = new Point(356, 426);
            btnRejectReceipt.Name = "btnRejectReceipt";
            btnRejectReceipt.Size = new Size(70, 28);
            btnRejectReceipt.Text = "Reject";
            btnRejectReceipt.TabIndex = 11;
            btnRejectReceipt.Click += btnRejectReceipt_Click;
            // 
            // btnPrintSelectedReceipts
            // 
            btnPrintSelectedReceipts.Location = new Point(432, 426);
            btnPrintSelectedReceipts.Name = "btnPrintSelectedReceipts";
            btnPrintSelectedReceipts.Size = new Size(110, 28);
            btnPrintSelectedReceipts.Text = "Print Selected";
            btnPrintSelectedReceipts.TabIndex = 12;
            btnPrintSelectedReceipts.Click += btnPrintSelectedReceipts_Click;
            // 
            // btnExportReceipts
            // 
            btnExportReceipts.Location = new Point(548, 426);
            btnExportReceipts.Name = "btnExportReceipts";
            btnExportReceipts.Size = new Size(80, 28);
            btnExportReceipts.Text = "Export";
            btnExportReceipts.TabIndex = 13;
            btnExportReceipts.Click += btnExportReceipts_Click;
            // 
            // lblApproverNote
            // 
            lblApproverNote.AutoSize = true;
            lblApproverNote.Location = new Point(12, 462);
            lblApproverNote.Name = "lblApproverNote";
            lblApproverNote.Text = "The Receipts Supervisor or an Admin approves receipts.";
            // 
            // lblVaultMsg
            // 
            lblVaultMsg.AutoSize = true;
            lblVaultMsg.Location = new Point(12, 486);
            lblVaultMsg.Name = "lblVaultMsg";
            // 
            // colReceiptNo
            // 
            colReceiptNo.DataPropertyName = "ReceiptNo";
            colReceiptNo.HeaderText = "Receipt No.";
            colReceiptNo.Name = "colReceiptNo";
            // 
            // colDate
            // 
            colDate.DataPropertyName = "Date";
            colDate.HeaderText = "Date";
            colDate.Name = "colDate";
            colDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colMerchant
            // 
            colMerchant.DataPropertyName = "Merchant";
            colMerchant.HeaderText = "Merchant";
            colMerchant.Name = "colMerchant";
            // 
            // colAmount
            // 
            colAmount.DataPropertyName = "Amount";
            colAmount.HeaderText = "Amount";
            colAmount.Name = "colAmount";
            colAmount.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colCategory
            // 
            colCategory.DataPropertyName = "Category";
            colCategory.HeaderText = "Category";
            colCategory.Name = "colCategory";
            // 
            // colProject
            // 
            colProject.DataPropertyName = "ProjectOrOverhead";
            colProject.HeaderText = "Project / Overhead";
            colProject.Name = "colProject";
            // 
            // colCopy
            // 
            colCopy.DataPropertyName = "Copy";
            colCopy.HeaderText = "Copy";
            colCopy.Name = "colCopy";
            // 
            // colStock
            // 
            colStock.DataPropertyName = "StockPurchase";
            colStock.HeaderText = "Stock Purchase";
            colStock.Name = "colStock";
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            // 
            // colUploadedBy
            // 
            colUploadedBy.DataPropertyName = "UploadedBy";
            colUploadedBy.HeaderText = "Uploaded By";
            colUploadedBy.Name = "colUploadedBy";
            // 
            // ReceiptVaultTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblReceiptProject);
            Controls.Add(cboReceiptProject);
            Controls.Add(lblReceiptFrom);
            Controls.Add(dtpReceiptFrom);
            Controls.Add(lblReceiptTo);
            Controls.Add(dtpReceiptTo);
            Controls.Add(lblReceiptCategory);
            Controls.Add(cboReceiptCategoryFilter);
            Controls.Add(lblReceiptStatus);
            Controls.Add(cboReceiptStatusFilter);
            Controls.Add(lblReceiptSearch);
            Controls.Add(txtReceiptSearch);
            Controls.Add(dgvReceipts);
            Controls.Add(lblReceiptsTotal);
            Controls.Add(btnAddReceipt);
            Controls.Add(btnOpenReceipt);
            Controls.Add(btnEditReceipt);
            Controls.Add(btnApproveReceipt);
            Controls.Add(btnRejectReceipt);
            Controls.Add(btnPrintSelectedReceipts);
            Controls.Add(btnExportReceipts);
            Controls.Add(lblApproverNote);
            Controls.Add(lblVaultMsg);
            Name = "ReceiptVaultTab";
            Size = new Size(960, 540);
            Load += ReceiptVaultTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvReceipts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblReceiptProject;
        private ComboBox cboReceiptProject;
        private Label lblReceiptFrom;
        private DateTimePicker dtpReceiptFrom;
        private Label lblReceiptTo;
        private DateTimePicker dtpReceiptTo;
        private Label lblReceiptCategory;
        private ComboBox cboReceiptCategoryFilter;
        private Label lblReceiptStatus;
        private ComboBox cboReceiptStatusFilter;
        private Label lblReceiptSearch;
        private TextBox txtReceiptSearch;
        private DataGridView dgvReceipts;
        private Label lblReceiptsTotal;
        private Button btnAddReceipt;
        private Button btnOpenReceipt;
        private Button btnEditReceipt;
        private Button btnApproveReceipt;
        private Button btnRejectReceipt;
        private Button btnPrintSelectedReceipts;
        private Button btnExportReceipts;
        private Label lblApproverNote;
        private Label lblVaultMsg;
        private DataGridViewTextBoxColumn colReceiptNo;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colMerchant;
        private DataGridViewTextBoxColumn colAmount;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colProject;
        private DataGridViewTextBoxColumn colCopy;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colUploadedBy;
    }
}
