namespace Company.WebsiteAdmin.Pages
{
    partial class ContactUsPage
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
            lblContactHint = new Label();
            btnRefreshContact = new Button();
            lblTotalInquiries = new Label();
            lblPendingInquiries = new Label();
            lblWebsiteVisits = new Label();
            lblRecipientEmail = new Label();
            txtRecipientEmail = new TextBox();
            btnSaveRecipientEmail = new Button();
            lblCompanyDetails = new Label();
            lblCompanyAddress = new Label();
            txtCompanyAddress = new TextBox();
            lblCompanyPhone = new Label();
            txtCompanyPhone = new TextBox();
            lblCompanyEmail = new Label();
            txtCompanyEmail = new TextBox();
            lblCopyright = new Label();
            txtCopyright = new TextBox();
            btnSaveCompanyDetails = new Button();
            lblContactSettingsMsg = new Label();
            lblClientInquiries = new Label();
            txtInquirySearch = new TextBox();
            cboInquiryStatusFilter = new ComboBox();
            lblInquiryFrom = new Label();
            dtpInquiryFrom = new DateTimePicker();
            lblInquiryTo = new Label();
            dtpInquiryTo = new DateTimePicker();
            btnResetInquiryFilter = new Button();
            btnExportInquiries = new Button();
            dgvInquiries = new DataGridView();
            lblInquiryCount = new Label();
            btnOpenInquiry = new Button();
            colDate = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colService = new DataGridViewTextBoxColumn();
            colLocation = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvInquiries).BeginInit();
            SuspendLayout();
            // 
            // lblContactHint
            // 
            lblContactHint.AutoSize = true;
            lblContactHint.Location = new Point(12, 12);
            lblContactHint.Name = "lblContactHint";
            lblContactHint.Text = "View and manage inquiries submitted through the website.";
            // 
            // btnRefreshContact
            // 
            btnRefreshContact.Location = new Point(852, 8);
            btnRefreshContact.Name = "btnRefreshContact";
            btnRefreshContact.Size = new Size(90, 28);
            btnRefreshContact.Text = "Refresh";
            btnRefreshContact.TabIndex = 0;
            btnRefreshContact.Click += btnRefreshContact_Click;
            // 
            // lblTotalInquiries
            // 
            lblTotalInquiries.AutoSize = true;
            lblTotalInquiries.Location = new Point(12, 42);
            lblTotalInquiries.Name = "lblTotalInquiries";
            lblTotalInquiries.Text = "Total inquiries: -";
            // 
            // lblPendingInquiries
            // 
            lblPendingInquiries.AutoSize = true;
            lblPendingInquiries.Location = new Point(200, 42);
            lblPendingInquiries.Name = "lblPendingInquiries";
            lblPendingInquiries.Text = "Pending (New): -";
            // 
            // lblWebsiteVisits
            // 
            lblWebsiteVisits.AutoSize = true;
            lblWebsiteVisits.Location = new Point(380, 42);
            lblWebsiteVisits.Name = "lblWebsiteVisits";
            lblWebsiteVisits.Text = "Website visits this month: -";
            // 
            // lblRecipientEmail
            // 
            lblRecipientEmail.AutoSize = true;
            lblRecipientEmail.Location = new Point(12, 74);
            lblRecipientEmail.Name = "lblRecipientEmail";
            lblRecipientEmail.Text = "Inquiries are forwarded to";
            // 
            // txtRecipientEmail
            // 
            txtRecipientEmail.Location = new Point(170, 71);
            txtRecipientEmail.Name = "txtRecipientEmail";
            txtRecipientEmail.Size = new Size(260, 23);
            txtRecipientEmail.TabIndex = 1;
            // 
            // btnSaveRecipientEmail
            // 
            btnSaveRecipientEmail.Location = new Point(436, 69);
            btnSaveRecipientEmail.Name = "btnSaveRecipientEmail";
            btnSaveRecipientEmail.Size = new Size(110, 27);
            btnSaveRecipientEmail.Text = "Save Email";
            btnSaveRecipientEmail.TabIndex = 2;
            btnSaveRecipientEmail.Click += btnSaveRecipientEmail_Click;
            // 
            // lblCompanyDetails
            // 
            lblCompanyDetails.AutoSize = true;
            lblCompanyDetails.Location = new Point(12, 106);
            lblCompanyDetails.Name = "lblCompanyDetails";
            lblCompanyDetails.Text = "Company details on the website (address, phone, email, footer)";
            // 
            // lblCompanyAddress
            // 
            lblCompanyAddress.AutoSize = true;
            lblCompanyAddress.Location = new Point(12, 130);
            lblCompanyAddress.Name = "lblCompanyAddress";
            lblCompanyAddress.Text = "Address";
            // 
            // txtCompanyAddress
            // 
            txtCompanyAddress.Location = new Point(100, 127);
            txtCompanyAddress.Name = "txtCompanyAddress";
            txtCompanyAddress.Size = new Size(446, 23);
            txtCompanyAddress.TabIndex = 3;
            // 
            // lblCompanyPhone
            // 
            lblCompanyPhone.AutoSize = true;
            lblCompanyPhone.Location = new Point(12, 160);
            lblCompanyPhone.Name = "lblCompanyPhone";
            lblCompanyPhone.Text = "Phone";
            // 
            // txtCompanyPhone
            // 
            txtCompanyPhone.Location = new Point(100, 157);
            txtCompanyPhone.Name = "txtCompanyPhone";
            txtCompanyPhone.Size = new Size(180, 23);
            txtCompanyPhone.TabIndex = 4;
            // 
            // lblCompanyEmail
            // 
            lblCompanyEmail.AutoSize = true;
            lblCompanyEmail.Location = new Point(292, 160);
            lblCompanyEmail.Name = "lblCompanyEmail";
            lblCompanyEmail.Text = "Email";
            // 
            // txtCompanyEmail
            // 
            txtCompanyEmail.Location = new Point(336, 157);
            txtCompanyEmail.Name = "txtCompanyEmail";
            txtCompanyEmail.Size = new Size(210, 23);
            txtCompanyEmail.TabIndex = 5;
            // 
            // lblCopyright
            // 
            lblCopyright.AutoSize = true;
            lblCopyright.Location = new Point(12, 190);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Text = "Footer";
            // 
            // txtCopyright
            // 
            txtCopyright.Location = new Point(100, 187);
            txtCopyright.Name = "txtCopyright";
            txtCopyright.Size = new Size(446, 23);
            txtCopyright.TabIndex = 6;
            // 
            // btnSaveCompanyDetails
            // 
            btnSaveCompanyDetails.Location = new Point(560, 185);
            btnSaveCompanyDetails.Name = "btnSaveCompanyDetails";
            btnSaveCompanyDetails.Size = new Size(120, 27);
            btnSaveCompanyDetails.Text = "Save Details";
            btnSaveCompanyDetails.TabIndex = 7;
            btnSaveCompanyDetails.Click += btnSaveCompanyDetails_Click;
            // 
            // lblContactSettingsMsg
            // 
            lblContactSettingsMsg.AutoSize = true;
            lblContactSettingsMsg.Location = new Point(560, 130);
            lblContactSettingsMsg.Name = "lblContactSettingsMsg";
            lblContactSettingsMsg.MaximumSize = new Size(380, 0);
            // 
            // lblClientInquiries
            // 
            lblClientInquiries.AutoSize = true;
            lblClientInquiries.Location = new Point(12, 228);
            lblClientInquiries.Name = "lblClientInquiries";
            lblClientInquiries.Text = "Client inquiries (double-click one to open it)";
            // 
            // txtInquirySearch
            // 
            txtInquirySearch.Location = new Point(12, 250);
            txtInquirySearch.Name = "txtInquirySearch";
            txtInquirySearch.Size = new Size(250, 23);
            txtInquirySearch.PlaceholderText = "Search name, email, service or location";
            txtInquirySearch.TabIndex = 8;
            // 
            // cboInquiryStatusFilter
            // 
            cboInquiryStatusFilter.Location = new Point(270, 250);
            cboInquiryStatusFilter.Name = "cboInquiryStatusFilter";
            cboInquiryStatusFilter.Size = new Size(110, 23);
            cboInquiryStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboInquiryStatusFilter.TabIndex = 9;
            cboInquiryStatusFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblInquiryFrom
            // 
            lblInquiryFrom.AutoSize = true;
            lblInquiryFrom.Location = new Point(390, 253);
            lblInquiryFrom.Name = "lblInquiryFrom";
            lblInquiryFrom.Text = "From";
            // 
            // dtpInquiryFrom
            // 
            dtpInquiryFrom.Location = new Point(428, 250);
            dtpInquiryFrom.Name = "dtpInquiryFrom";
            dtpInquiryFrom.Size = new Size(120, 23);
            dtpInquiryFrom.Format = DateTimePickerFormat.Short;
            dtpInquiryFrom.ShowCheckBox = true;
            dtpInquiryFrom.Checked = false;
            dtpInquiryFrom.TabIndex = 10;
            dtpInquiryFrom.ValueChanged += Filter_Changed;
            // 
            // lblInquiryTo
            // 
            lblInquiryTo.AutoSize = true;
            lblInquiryTo.Location = new Point(556, 253);
            lblInquiryTo.Name = "lblInquiryTo";
            lblInquiryTo.Text = "To";
            // 
            // dtpInquiryTo
            // 
            dtpInquiryTo.Location = new Point(580, 250);
            dtpInquiryTo.Name = "dtpInquiryTo";
            dtpInquiryTo.Size = new Size(120, 23);
            dtpInquiryTo.Format = DateTimePickerFormat.Short;
            dtpInquiryTo.ShowCheckBox = true;
            dtpInquiryTo.Checked = false;
            dtpInquiryTo.TabIndex = 11;
            dtpInquiryTo.ValueChanged += Filter_Changed;
            // 
            // btnResetInquiryFilter
            // 
            btnResetInquiryFilter.Location = new Point(708, 248);
            btnResetInquiryFilter.Name = "btnResetInquiryFilter";
            btnResetInquiryFilter.Size = new Size(100, 27);
            btnResetInquiryFilter.Text = "Reset Filter";
            btnResetInquiryFilter.TabIndex = 12;
            btnResetInquiryFilter.Click += btnResetInquiryFilter_Click;
            // 
            // btnExportInquiries
            // 
            btnExportInquiries.Location = new Point(822, 248);
            btnExportInquiries.Name = "btnExportInquiries";
            btnExportInquiries.Size = new Size(120, 27);
            btnExportInquiries.Text = "Export to Excel";
            btnExportInquiries.TabIndex = 13;
            btnExportInquiries.Click += btnExportInquiries_Click;
            // 
            // dgvInquiries
            // 
            dgvInquiries.AllowUserToAddRows = false;
            dgvInquiries.AllowUserToDeleteRows = false;
            dgvInquiries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInquiries.Columns.AddRange(new DataGridViewColumn[] { colDate, colName, colService, colLocation, colStatus });
            dgvInquiries.MultiSelect = false;
            dgvInquiries.ReadOnly = true;
            dgvInquiries.RowHeadersVisible = false;
            dgvInquiries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInquiries.Location = new Point(12, 282);
            dgvInquiries.Name = "dgvInquiries";
            dgvInquiries.Size = new Size(930, 330);
            dgvInquiries.TabIndex = 14;
            dgvInquiries.CellDoubleClick += dgvInquiries_CellDoubleClick;
            // 
            // lblInquiryCount
            // 
            lblInquiryCount.AutoSize = true;
            lblInquiryCount.Location = new Point(12, 622);
            lblInquiryCount.Name = "lblInquiryCount";
            lblInquiryCount.Text = "";
            // 
            // btnOpenInquiry
            // 
            btnOpenInquiry.Location = new Point(822, 618);
            btnOpenInquiry.Name = "btnOpenInquiry";
            btnOpenInquiry.Size = new Size(120, 28);
            btnOpenInquiry.Text = "Open Inquiry";
            btnOpenInquiry.TabIndex = 15;
            btnOpenInquiry.Click += btnOpenInquiry_Click;
            // 
            // colDate
            // 
            colDate.DataPropertyName = "ReceivedAt";
            colDate.HeaderText = "Date";
            colDate.Name = "colDate";
            colDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" };
            colDate.FillWeight = 50;
            // 
            // colName
            // 
            colName.DataPropertyName = "FullName";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            // 
            // colService
            // 
            colService.DataPropertyName = "ServiceRequired";
            colService.HeaderText = "Service";
            colService.Name = "colService";
            // 
            // colLocation
            // 
            colLocation.DataPropertyName = "ProjectLocation";
            colLocation.HeaderText = "Location";
            colLocation.Name = "colLocation";
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.FillWeight = 50;
            // 
            // ContactUsPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblContactHint);
            Controls.Add(btnRefreshContact);
            Controls.Add(lblTotalInquiries);
            Controls.Add(lblPendingInquiries);
            Controls.Add(lblWebsiteVisits);
            Controls.Add(lblRecipientEmail);
            Controls.Add(txtRecipientEmail);
            Controls.Add(btnSaveRecipientEmail);
            Controls.Add(lblCompanyDetails);
            Controls.Add(lblCompanyAddress);
            Controls.Add(txtCompanyAddress);
            Controls.Add(lblCompanyPhone);
            Controls.Add(txtCompanyPhone);
            Controls.Add(lblCompanyEmail);
            Controls.Add(txtCompanyEmail);
            Controls.Add(lblCopyright);
            Controls.Add(txtCopyright);
            Controls.Add(btnSaveCompanyDetails);
            Controls.Add(lblContactSettingsMsg);
            Controls.Add(lblClientInquiries);
            Controls.Add(txtInquirySearch);
            Controls.Add(cboInquiryStatusFilter);
            Controls.Add(lblInquiryFrom);
            Controls.Add(dtpInquiryFrom);
            Controls.Add(lblInquiryTo);
            Controls.Add(dtpInquiryTo);
            Controls.Add(btnResetInquiryFilter);
            Controls.Add(btnExportInquiries);
            Controls.Add(dgvInquiries);
            Controls.Add(lblInquiryCount);
            Controls.Add(btnOpenInquiry);
            Name = "ContactUsPage";
            Size = new Size(960, 700);
            Load += ContactUsPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvInquiries).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblContactHint;
        private Button btnRefreshContact;
        private Label lblTotalInquiries;
        private Label lblPendingInquiries;
        private Label lblWebsiteVisits;
        private Label lblRecipientEmail;
        private TextBox txtRecipientEmail;
        private Button btnSaveRecipientEmail;
        private Label lblCompanyDetails;
        private Label lblCompanyAddress;
        private TextBox txtCompanyAddress;
        private Label lblCompanyPhone;
        private TextBox txtCompanyPhone;
        private Label lblCompanyEmail;
        private TextBox txtCompanyEmail;
        private Label lblCopyright;
        private TextBox txtCopyright;
        private Button btnSaveCompanyDetails;
        private Label lblContactSettingsMsg;
        private Label lblClientInquiries;
        private TextBox txtInquirySearch;
        private ComboBox cboInquiryStatusFilter;
        private Label lblInquiryFrom;
        private DateTimePicker dtpInquiryFrom;
        private Label lblInquiryTo;
        private DateTimePicker dtpInquiryTo;
        private Button btnResetInquiryFilter;
        private Button btnExportInquiries;
        private DataGridView dgvInquiries;
        private Label lblInquiryCount;
        private Button btnOpenInquiry;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colService;
        private DataGridViewTextBoxColumn colLocation;
        private DataGridViewTextBoxColumn colStatus;
    }
}
