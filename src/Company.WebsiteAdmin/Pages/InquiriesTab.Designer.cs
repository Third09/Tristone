namespace Company.WebsiteAdmin.Pages
{
    partial class InquiriesTab
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
            lblNewCount = new Label();
            lblContactedCount = new Label();
            lblClosedCount = new Label();
            lblStatusFilter = new Label();
            cboInquiryStatusFilter = new ComboBox();
            lblSearch = new Label();
            txtInquirySearch = new TextBox();
            lblInquiryFrom = new Label();
            dtpInquiryFrom = new DateTimePicker();
            lblInquiryTo = new Label();
            dtpInquiryTo = new DateTimePicker();
            btnRefreshInquiries = new Button();
            dgvInquiries = new DataGridView();
            btnExportInquiries = new Button();
            lblInqTitle = new Label();
            lblInqName = new Label();
            lblInqContact = new Label();
            lblInqEmail = new Label();
            btnCopyEmail = new Button();
            lblInqLocation = new Label();
            lblInqService = new Label();
            lblInqReceived = new Label();
            lblInqMessage = new Label();
            txtInqMessage = new TextBox();
            lblInquiryStatus = new Label();
            cboInquiryStatus = new ComboBox();
            lblInquiryNotes = new Label();
            txtInquiryNotes = new TextBox();
            lblInquiryMsg = new Label();
            btnSaveInquiry = new Button();
            btnCancelInquiry = new Button();
            colReceived = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colService = new DataGridViewTextBoxColumn();
            colLocation = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvInquiries).BeginInit();
            SuspendLayout();
            // 
            // lblNewCount
            // 
            lblNewCount.AutoSize = true;
            lblNewCount.Location = new Point(12, 12);
            lblNewCount.Name = "lblNewCount";
            lblNewCount.Text = "New: 0";
            // 
            // lblContactedCount
            // 
            lblContactedCount.AutoSize = true;
            lblContactedCount.Location = new Point(140, 12);
            lblContactedCount.Name = "lblContactedCount";
            lblContactedCount.Text = "Contacted: 0";
            // 
            // lblClosedCount
            // 
            lblClosedCount.AutoSize = true;
            lblClosedCount.Location = new Point(290, 12);
            lblClosedCount.Name = "lblClosedCount";
            lblClosedCount.Text = "Closed: 0";
            // 
            // lblStatusFilter
            // 
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(12, 43);
            lblStatusFilter.Name = "lblStatusFilter";
            lblStatusFilter.Text = "Status";
            // 
            // cboInquiryStatusFilter
            // 
            cboInquiryStatusFilter.Location = new Point(60, 40);
            cboInquiryStatusFilter.Name = "cboInquiryStatusFilter";
            cboInquiryStatusFilter.Size = new Size(110, 23);
            cboInquiryStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboInquiryStatusFilter.SelectedIndexChanged += Filter_Changed;
            cboInquiryStatusFilter.TabIndex = 0;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(184, 43);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search";
            // 
            // txtInquirySearch
            // 
            txtInquirySearch.Location = new Point(234, 40);
            txtInquirySearch.Name = "txtInquirySearch";
            txtInquirySearch.Size = new Size(180, 23);
            txtInquirySearch.TabIndex = 1;
            // 
            // lblInquiryFrom
            // 
            lblInquiryFrom.AutoSize = true;
            lblInquiryFrom.Location = new Point(428, 43);
            lblInquiryFrom.Name = "lblInquiryFrom";
            lblInquiryFrom.Text = "From";
            // 
            // dtpInquiryFrom
            // 
            dtpInquiryFrom.Location = new Point(468, 40);
            dtpInquiryFrom.Name = "dtpInquiryFrom";
            dtpInquiryFrom.Size = new Size(120, 23);
            dtpInquiryFrom.Format = DateTimePickerFormat.Short;
            dtpInquiryFrom.ShowCheckBox = true;
            dtpInquiryFrom.Checked = false;
            dtpInquiryFrom.ValueChanged += Filter_Changed;
            dtpInquiryFrom.TabIndex = 2;
            // 
            // lblInquiryTo
            // 
            lblInquiryTo.AutoSize = true;
            lblInquiryTo.Location = new Point(602, 43);
            lblInquiryTo.Name = "lblInquiryTo";
            lblInquiryTo.Text = "To";
            // 
            // dtpInquiryTo
            // 
            dtpInquiryTo.Location = new Point(628, 40);
            dtpInquiryTo.Name = "dtpInquiryTo";
            dtpInquiryTo.Size = new Size(120, 23);
            dtpInquiryTo.Format = DateTimePickerFormat.Short;
            dtpInquiryTo.ShowCheckBox = true;
            dtpInquiryTo.Checked = false;
            dtpInquiryTo.ValueChanged += Filter_Changed;
            dtpInquiryTo.TabIndex = 3;
            // 
            // btnRefreshInquiries
            // 
            btnRefreshInquiries.Location = new Point(762, 39);
            btnRefreshInquiries.Name = "btnRefreshInquiries";
            btnRefreshInquiries.Size = new Size(80, 25);
            btnRefreshInquiries.Text = "Refresh";
            btnRefreshInquiries.Click += btnRefreshInquiries_Click;
            btnRefreshInquiries.TabIndex = 4;
            // 
            // dgvInquiries
            // 
            dgvInquiries.Location = new Point(12, 72);
            dgvInquiries.Name = "dgvInquiries";
            dgvInquiries.Size = new Size(830, 200);
            dgvInquiries.AllowUserToAddRows = false;
            dgvInquiries.AllowUserToDeleteRows = false;
            dgvInquiries.ReadOnly = true;
            dgvInquiries.MultiSelect = false;
            dgvInquiries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInquiries.RowHeadersVisible = false;
            dgvInquiries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInquiries.Columns.AddRange(new DataGridViewColumn[] { colReceived, colName, colService, colLocation, colStatus });
            dgvInquiries.SelectionChanged += dgvInquiries_SelectionChanged;
            dgvInquiries.TabIndex = 5;
            // 
            // btnExportInquiries
            // 
            btnExportInquiries.Location = new Point(12, 280);
            btnExportInquiries.Name = "btnExportInquiries";
            btnExportInquiries.Size = new Size(90, 28);
            btnExportInquiries.Text = "Export";
            btnExportInquiries.Click += btnExportInquiries_Click;
            btnExportInquiries.TabIndex = 6;
            // 
            // lblInqTitle
            // 
            lblInqTitle.AutoSize = true;
            lblInqTitle.Location = new Point(12, 322);
            lblInqTitle.Name = "lblInqTitle";
            lblInqTitle.Text = "Click an inquiry in the list to read it and follow it up.";
            // 
            // lblInqName
            // 
            lblInqName.Location = new Point(12, 346);
            lblInqName.Name = "lblInqName";
            lblInqName.Size = new Size(500, 18);
            lblInqName.Text = "Name: -";
            // 
            // lblInqContact
            // 
            lblInqContact.Location = new Point(12, 368);
            lblInqContact.Name = "lblInqContact";
            lblInqContact.Size = new Size(500, 18);
            lblInqContact.Text = "Contact number: -";
            // 
            // lblInqEmail
            // 
            lblInqEmail.Location = new Point(12, 390);
            lblInqEmail.Name = "lblInqEmail";
            lblInqEmail.Size = new Size(400, 18);
            lblInqEmail.Text = "Email: -";
            // 
            // btnCopyEmail
            // 
            btnCopyEmail.Location = new Point(420, 386);
            btnCopyEmail.Name = "btnCopyEmail";
            btnCopyEmail.Size = new Size(90, 25);
            btnCopyEmail.Text = "Copy Email";
            btnCopyEmail.Click += btnCopyEmail_Click;
            btnCopyEmail.TabIndex = 7;
            // 
            // lblInqLocation
            // 
            lblInqLocation.Location = new Point(12, 412);
            lblInqLocation.Name = "lblInqLocation";
            lblInqLocation.Size = new Size(500, 18);
            lblInqLocation.Text = "Project location: -";
            // 
            // lblInqService
            // 
            lblInqService.Location = new Point(12, 434);
            lblInqService.Name = "lblInqService";
            lblInqService.Size = new Size(500, 18);
            lblInqService.Text = "Service required: -";
            // 
            // lblInqReceived
            // 
            lblInqReceived.Location = new Point(12, 456);
            lblInqReceived.Name = "lblInqReceived";
            lblInqReceived.Size = new Size(500, 18);
            lblInqReceived.Text = "Received: -";
            // 
            // lblInqMessage
            // 
            lblInqMessage.AutoSize = true;
            lblInqMessage.Location = new Point(12, 482);
            lblInqMessage.Name = "lblInqMessage";
            lblInqMessage.Text = "Message";
            // 
            // txtInqMessage
            // 
            txtInqMessage.Location = new Point(12, 500);
            txtInqMessage.Name = "txtInqMessage";
            txtInqMessage.Size = new Size(500, 100);
            txtInqMessage.Multiline = true;
            txtInqMessage.ReadOnly = true;
            txtInqMessage.ScrollBars = ScrollBars.Vertical;
            txtInqMessage.TabIndex = 8;
            // 
            // lblInquiryStatus
            // 
            lblInquiryStatus.AutoSize = true;
            lblInquiryStatus.Location = new Point(540, 346);
            lblInquiryStatus.Name = "lblInquiryStatus";
            lblInquiryStatus.Text = "Status";
            // 
            // cboInquiryStatus
            // 
            cboInquiryStatus.Location = new Point(540, 364);
            cboInquiryStatus.Name = "cboInquiryStatus";
            cboInquiryStatus.Size = new Size(140, 23);
            cboInquiryStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboInquiryStatus.TabIndex = 9;
            // 
            // lblInquiryNotes
            // 
            lblInquiryNotes.AutoSize = true;
            lblInquiryNotes.Location = new Point(540, 394);
            lblInquiryNotes.Name = "lblInquiryNotes";
            lblInquiryNotes.Text = "Notes (internal, never shown on the website)";
            // 
            // txtInquiryNotes
            // 
            txtInquiryNotes.Location = new Point(540, 412);
            txtInquiryNotes.Name = "txtInquiryNotes";
            txtInquiryNotes.Size = new Size(300, 188);
            txtInquiryNotes.Multiline = true;
            txtInquiryNotes.ScrollBars = ScrollBars.Vertical;
            txtInquiryNotes.TabIndex = 10;
            // 
            // lblInquiryMsg
            // 
            lblInquiryMsg.Location = new Point(12, 612);
            lblInquiryMsg.Name = "lblInquiryMsg";
            lblInquiryMsg.Size = new Size(700, 18);
            lblInquiryMsg.Text = "";
            // 
            // btnSaveInquiry
            // 
            btnSaveInquiry.Location = new Point(12, 634);
            btnSaveInquiry.Name = "btnSaveInquiry";
            btnSaveInquiry.Size = new Size(90, 28);
            btnSaveInquiry.Text = "Save";
            btnSaveInquiry.Click += btnSaveInquiry_Click;
            btnSaveInquiry.TabIndex = 11;
            // 
            // btnCancelInquiry
            // 
            btnCancelInquiry.Location = new Point(108, 634);
            btnCancelInquiry.Name = "btnCancelInquiry";
            btnCancelInquiry.Size = new Size(90, 28);
            btnCancelInquiry.Text = "Cancel";
            btnCancelInquiry.Click += btnCancelInquiry_Click;
            btnCancelInquiry.TabIndex = 12;
            // 
            // colReceived
            // 
            colReceived.DataPropertyName = "ReceivedAt";
            colReceived.HeaderText = "Date";
            colReceived.Name = "colReceived";
            colReceived.ReadOnly = true;
            colReceived.DefaultCellStyle.Format = "MMM d, yyyy h:mm tt";
            colReceived.FillWeight = 110F;
            // 
            // colName
            // 
            colName.DataPropertyName = "FullName";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.FillWeight = 120F;
            // 
            // colService
            // 
            colService.DataPropertyName = "ServiceRequired";
            colService.HeaderText = "Service";
            colService.Name = "colService";
            colService.ReadOnly = true;
            colService.FillWeight = 100F;
            // 
            // colLocation
            // 
            colLocation.DataPropertyName = "ProjectLocation";
            colLocation.HeaderText = "Location";
            colLocation.Name = "colLocation";
            colLocation.ReadOnly = true;
            colLocation.FillWeight = 140F;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.FillWeight = 60F;
            // 
            // InquiriesTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Size = new Size(860, 680);
            Load += InquiriesTab_Load;
            Controls.Add(lblNewCount);
            Controls.Add(lblContactedCount);
            Controls.Add(lblClosedCount);
            Controls.Add(lblStatusFilter);
            Controls.Add(cboInquiryStatusFilter);
            Controls.Add(lblSearch);
            Controls.Add(txtInquirySearch);
            Controls.Add(lblInquiryFrom);
            Controls.Add(dtpInquiryFrom);
            Controls.Add(lblInquiryTo);
            Controls.Add(dtpInquiryTo);
            Controls.Add(btnRefreshInquiries);
            Controls.Add(dgvInquiries);
            Controls.Add(btnExportInquiries);
            Controls.Add(lblInqTitle);
            Controls.Add(lblInqName);
            Controls.Add(lblInqContact);
            Controls.Add(lblInqEmail);
            Controls.Add(btnCopyEmail);
            Controls.Add(lblInqLocation);
            Controls.Add(lblInqService);
            Controls.Add(lblInqReceived);
            Controls.Add(lblInqMessage);
            Controls.Add(txtInqMessage);
            Controls.Add(lblInquiryStatus);
            Controls.Add(cboInquiryStatus);
            Controls.Add(lblInquiryNotes);
            Controls.Add(txtInquiryNotes);
            Controls.Add(lblInquiryMsg);
            Controls.Add(btnSaveInquiry);
            Controls.Add(btnCancelInquiry);
            Name = "InquiriesTab";
            ((System.ComponentModel.ISupportInitialize)dgvInquiries).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNewCount;
        private Label lblContactedCount;
        private Label lblClosedCount;
        private Label lblStatusFilter;
        private ComboBox cboInquiryStatusFilter;
        private Label lblSearch;
        private TextBox txtInquirySearch;
        private Label lblInquiryFrom;
        private DateTimePicker dtpInquiryFrom;
        private Label lblInquiryTo;
        private DateTimePicker dtpInquiryTo;
        private Button btnRefreshInquiries;
        private DataGridView dgvInquiries;
        private Button btnExportInquiries;
        private Label lblInqTitle;
        private Label lblInqName;
        private Label lblInqContact;
        private Label lblInqEmail;
        private Button btnCopyEmail;
        private Label lblInqLocation;
        private Label lblInqService;
        private Label lblInqReceived;
        private Label lblInqMessage;
        private TextBox txtInqMessage;
        private Label lblInquiryStatus;
        private ComboBox cboInquiryStatus;
        private Label lblInquiryNotes;
        private TextBox txtInquiryNotes;
        private Label lblInquiryMsg;
        private Button btnSaveInquiry;
        private Button btnCancelInquiry;
        private DataGridViewTextBoxColumn colReceived;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colService;
        private DataGridViewTextBoxColumn colLocation;
        private DataGridViewTextBoxColumn colStatus;
    }
}
