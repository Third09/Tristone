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
            cardInquiries = new Company.OfficeApp.UI.Card();
            pnlGap2 = new Panel();
            cardCompany = new Company.OfficeApp.UI.Card();
            pnlGap1 = new Panel();
            pnlKpis = new Panel();
            cardEmail = new Company.OfficeApp.UI.Card();
            pnlKpiGap3 = new Panel();
            cardVisits = new Company.OfficeApp.UI.Card();
            pnlKpiGap2 = new Panel();
            cardPending = new Company.OfficeApp.UI.Card();
            pnlKpiGap1 = new Panel();
            cardTotal = new Company.OfficeApp.UI.Card();
            lblTotalTitle = new Label();
            lblTotalInquiries = new Label();
            lblTotalNote = new Label();
            lblPendingTitle = new Label();
            lblPendingInquiries = new Label();
            lblPendingNote = new Label();
            lblVisitsTitle = new Label();
            lblWebsiteVisits = new Label();
            lblVisitsNote = new Label();
            lblEmailSettingsTitle = new Label();
            lblRecipientEmail = new Label();
            frmRecipient = new Company.OfficeApp.UI.InputFrame();
            txtRecipientEmail = new TextBox();
            btnSaveRecipientEmail = new Button();
            lblRecipientNote = new Label();
            lblCompanyDetails = new Label();
            lblCompanyAddress = new Label();
            frmCompanyAddress = new Company.OfficeApp.UI.InputFrame();
            txtCompanyAddress = new TextBox();
            lblCompanyPhone = new Label();
            frmCompanyPhone = new Company.OfficeApp.UI.InputFrame();
            txtCompanyPhone = new TextBox();
            lblCompanyEmail = new Label();
            frmCompanyEmail = new Company.OfficeApp.UI.InputFrame();
            txtCompanyEmail = new TextBox();
            lblCopyright = new Label();
            frmCopyright = new Company.OfficeApp.UI.InputFrame();
            txtCopyright = new TextBox();
            lblContactSettingsMsg = new Label();
            btnSaveCompanyDetails = new Button();
            lblClientInquiries = new Label();
            lblInquiriesSub = new Label();
            btnRefreshContact = new Button();
            btnExportInquiries = new Button();
            frmInquirySearch = new Company.OfficeApp.UI.InputFrame();
            txtInquirySearch = new TextBox();
            cboInquiryStatusFilter = new ComboBox();
            lblInquiryFrom = new Label();
            dtpInquiryFrom = new DateTimePicker();
            lblInquiryTo = new Label();
            dtpInquiryTo = new DateTimePicker();
            btnResetInquiryFilter = new Button();
            dgvInquiries = new DataGridView();
            lblInquiryCount = new Label();
            btnOpenInquiry = new Button();
            pnlKpis.Controls.Add(cardEmail);
            pnlKpis.Controls.Add(pnlKpiGap3);
            pnlKpis.Controls.Add(cardVisits);
            pnlKpis.Controls.Add(pnlKpiGap2);
            pnlKpis.Controls.Add(cardPending);
            pnlKpis.Controls.Add(pnlKpiGap1);
            pnlKpis.Controls.Add(cardTotal);
            cardTotal.Controls.Add(lblTotalTitle);
            cardTotal.Controls.Add(lblTotalInquiries);
            cardTotal.Controls.Add(lblTotalNote);
            cardPending.Controls.Add(lblPendingTitle);
            cardPending.Controls.Add(lblPendingInquiries);
            cardPending.Controls.Add(lblPendingNote);
            cardVisits.Controls.Add(lblVisitsTitle);
            cardVisits.Controls.Add(lblWebsiteVisits);
            cardVisits.Controls.Add(lblVisitsNote);
            cardEmail.Controls.Add(lblEmailSettingsTitle);
            cardEmail.Controls.Add(lblRecipientEmail);
            cardEmail.Controls.Add(frmRecipient);
            frmRecipient.Controls.Add(txtRecipientEmail);
            cardEmail.Controls.Add(btnSaveRecipientEmail);
            cardEmail.Controls.Add(lblRecipientNote);
            cardCompany.Controls.Add(lblCompanyDetails);
            cardCompany.Controls.Add(lblCompanyAddress);
            cardCompany.Controls.Add(frmCompanyAddress);
            frmCompanyAddress.Controls.Add(txtCompanyAddress);
            cardCompany.Controls.Add(lblCompanyPhone);
            cardCompany.Controls.Add(frmCompanyPhone);
            frmCompanyPhone.Controls.Add(txtCompanyPhone);
            cardCompany.Controls.Add(lblCompanyEmail);
            cardCompany.Controls.Add(frmCompanyEmail);
            frmCompanyEmail.Controls.Add(txtCompanyEmail);
            cardCompany.Controls.Add(lblCopyright);
            cardCompany.Controls.Add(frmCopyright);
            frmCopyright.Controls.Add(txtCopyright);
            cardCompany.Controls.Add(lblContactSettingsMsg);
            cardCompany.Controls.Add(btnSaveCompanyDetails);
            cardInquiries.Controls.Add(lblClientInquiries);
            cardInquiries.Controls.Add(lblInquiriesSub);
            cardInquiries.Controls.Add(btnRefreshContact);
            cardInquiries.Controls.Add(btnExportInquiries);
            cardInquiries.Controls.Add(frmInquirySearch);
            frmInquirySearch.Controls.Add(txtInquirySearch);
            cardInquiries.Controls.Add(cboInquiryStatusFilter);
            cardInquiries.Controls.Add(lblInquiryFrom);
            cardInquiries.Controls.Add(dtpInquiryFrom);
            cardInquiries.Controls.Add(lblInquiryTo);
            cardInquiries.Controls.Add(dtpInquiryTo);
            cardInquiries.Controls.Add(btnResetInquiryFilter);
            cardInquiries.Controls.Add(dgvInquiries);
            cardInquiries.Controls.Add(lblInquiryCount);
            cardInquiries.Controls.Add(btnOpenInquiry);
            colDate = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colService = new DataGridViewTextBoxColumn();
            colLocation = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvInquiries).BeginInit();
            SuspendLayout();
            // 
            // cardInquiries
            // 
            cardInquiries.Location = new Point(24, 426);
            cardInquiries.Name = "cardInquiries";
            cardInquiries.Size = new Size(1192, 492);
            cardInquiries.Dock = DockStyle.Fill;
            cardInquiries.Padding = new Padding(24);
            cardInquiries.TabIndex = 0;
            // 
            // pnlGap2
            // 
            pnlGap2.Location = new Point(24, 406);
            pnlGap2.Name = "pnlGap2";
            pnlGap2.Size = new Size(1192, 20);
            pnlGap2.Dock = DockStyle.Top;
            pnlGap2.TabIndex = 1;
            // 
            // cardCompany
            // 
            cardCompany.Location = new Point(24, 188);
            cardCompany.Name = "cardCompany";
            cardCompany.Size = new Size(1192, 218);
            cardCompany.Dock = DockStyle.Top;
            cardCompany.Padding = new Padding(24);
            cardCompany.TabIndex = 2;
            // 
            // pnlGap1
            // 
            pnlGap1.Location = new Point(24, 168);
            pnlGap1.Name = "pnlGap1";
            pnlGap1.Size = new Size(1192, 20);
            pnlGap1.Dock = DockStyle.Top;
            pnlGap1.TabIndex = 3;
            // 
            // pnlKpis
            // 
            pnlKpis.Location = new Point(24, 8);
            pnlKpis.Name = "pnlKpis";
            pnlKpis.Size = new Size(1192, 160);
            pnlKpis.Dock = DockStyle.Top;
            pnlKpis.TabIndex = 4;
            // 
            // cardEmail
            // 
            cardEmail.Location = new Point(750, 0);
            cardEmail.Name = "cardEmail";
            cardEmail.Size = new Size(442, 160);
            cardEmail.Dock = DockStyle.Fill;
            cardEmail.TabIndex = 5;
            // 
            // pnlKpiGap3
            // 
            pnlKpiGap3.Location = new Point(730, 0);
            pnlKpiGap3.Name = "pnlKpiGap3";
            pnlKpiGap3.Size = new Size(20, 160);
            pnlKpiGap3.Dock = DockStyle.Left;
            pnlKpiGap3.TabIndex = 6;
            // 
            // cardVisits
            // 
            cardVisits.Location = new Point(500, 0);
            cardVisits.Name = "cardVisits";
            cardVisits.Size = new Size(230, 160);
            cardVisits.Dock = DockStyle.Left;
            cardVisits.TabIndex = 7;
            // 
            // pnlKpiGap2
            // 
            pnlKpiGap2.Location = new Point(480, 0);
            pnlKpiGap2.Name = "pnlKpiGap2";
            pnlKpiGap2.Size = new Size(20, 160);
            pnlKpiGap2.Dock = DockStyle.Left;
            pnlKpiGap2.TabIndex = 8;
            // 
            // cardPending
            // 
            cardPending.Location = new Point(250, 0);
            cardPending.Name = "cardPending";
            cardPending.Size = new Size(230, 160);
            cardPending.Dock = DockStyle.Left;
            cardPending.TabIndex = 9;
            // 
            // pnlKpiGap1
            // 
            pnlKpiGap1.Location = new Point(230, 0);
            pnlKpiGap1.Name = "pnlKpiGap1";
            pnlKpiGap1.Size = new Size(20, 160);
            pnlKpiGap1.Dock = DockStyle.Left;
            pnlKpiGap1.TabIndex = 10;
            // 
            // cardTotal
            // 
            cardTotal.Location = new Point(0, 0);
            cardTotal.Name = "cardTotal";
            cardTotal.Size = new Size(230, 160);
            cardTotal.Dock = DockStyle.Left;
            cardTotal.TabIndex = 11;
            // 
            // lblTotalTitle
            // 
            lblTotalTitle.AutoSize = true;
            lblTotalTitle.Location = new Point(20, 18);
            lblTotalTitle.Name = "lblTotalTitle";
            lblTotalTitle.Font = new Font("Segoe UI Semibold", 11F);
            lblTotalTitle.Text = "Total Inquiries";
            // 
            // lblTotalInquiries
            // 
            lblTotalInquiries.AutoSize = true;
            lblTotalInquiries.Location = new Point(18, 50);
            lblTotalInquiries.Name = "lblTotalInquiries";
            lblTotalInquiries.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotalInquiries.Text = "-";
            // 
            // lblTotalNote
            // 
            lblTotalNote.AutoSize = true;
            lblTotalNote.Location = new Point(20, 118);
            lblTotalNote.Name = "lblTotalNote";
            lblTotalNote.Text = "All received inquiries";
            // 
            // lblPendingTitle
            // 
            lblPendingTitle.AutoSize = true;
            lblPendingTitle.Location = new Point(20, 18);
            lblPendingTitle.Name = "lblPendingTitle";
            lblPendingTitle.Font = new Font("Segoe UI Semibold", 11F);
            lblPendingTitle.Text = "Pending Inquiries";
            // 
            // lblPendingInquiries
            // 
            lblPendingInquiries.AutoSize = true;
            lblPendingInquiries.Location = new Point(18, 50);
            lblPendingInquiries.Name = "lblPendingInquiries";
            lblPendingInquiries.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblPendingInquiries.Text = "-";
            // 
            // lblPendingNote
            // 
            lblPendingNote.AutoSize = true;
            lblPendingNote.Location = new Point(20, 118);
            lblPendingNote.Name = "lblPendingNote";
            lblPendingNote.Text = "Awaiting response (New)";
            // 
            // lblVisitsTitle
            // 
            lblVisitsTitle.AutoSize = true;
            lblVisitsTitle.Location = new Point(20, 18);
            lblVisitsTitle.Name = "lblVisitsTitle";
            lblVisitsTitle.Font = new Font("Segoe UI Semibold", 11F);
            lblVisitsTitle.Text = "Website Visits";
            // 
            // lblWebsiteVisits
            // 
            lblWebsiteVisits.AutoSize = true;
            lblWebsiteVisits.Location = new Point(18, 50);
            lblWebsiteVisits.Name = "lblWebsiteVisits";
            lblWebsiteVisits.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblWebsiteVisits.Text = "-";
            // 
            // lblVisitsNote
            // 
            lblVisitsNote.AutoSize = true;
            lblVisitsNote.Location = new Point(20, 118);
            lblVisitsNote.Name = "lblVisitsNote";
            lblVisitsNote.Text = "This month";
            // 
            // lblEmailSettingsTitle
            // 
            lblEmailSettingsTitle.AutoSize = true;
            lblEmailSettingsTitle.Location = new Point(20, 18);
            lblEmailSettingsTitle.Name = "lblEmailSettingsTitle";
            lblEmailSettingsTitle.Font = new Font("Segoe UI Semibold", 11F);
            lblEmailSettingsTitle.Text = "Inquiry Email Settings";
            // 
            // lblRecipientEmail
            // 
            lblRecipientEmail.AutoSize = true;
            lblRecipientEmail.Location = new Point(20, 50);
            lblRecipientEmail.Name = "lblRecipientEmail";
            lblRecipientEmail.Font = new Font("Segoe UI Semibold", 10F);
            lblRecipientEmail.Text = "Recipient Email Address";
            // 
            // frmRecipient
            // 
            frmRecipient.Location = new Point(20, 72);
            frmRecipient.Name = "frmRecipient";
            frmRecipient.Size = new Size(272, 40);
            frmRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            frmRecipient.TabIndex = 12;
            // 
            // txtRecipientEmail
            // 
            txtRecipientEmail.Location = new Point(12, 9);
            txtRecipientEmail.Name = "txtRecipientEmail";
            txtRecipientEmail.Size = new Size(248, 23);
            txtRecipientEmail.BorderStyle = BorderStyle.None;
            txtRecipientEmail.Font = new Font("Segoe UI", 10.5F);
            txtRecipientEmail.TabIndex = 13;
            // 
            // btnSaveRecipientEmail
            // 
            btnSaveRecipientEmail.Location = new Point(302, 72);
            btnSaveRecipientEmail.Name = "btnSaveRecipientEmail";
            btnSaveRecipientEmail.Size = new Size(120, 40);
            btnSaveRecipientEmail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveRecipientEmail.Font = new Font("Segoe UI", 10F);
            btnSaveRecipientEmail.Text = "Save Email";
            btnSaveRecipientEmail.TabIndex = 14;
            btnSaveRecipientEmail.Click += btnSaveRecipientEmail_Click;
            // 
            // lblRecipientNote
            // 
            lblRecipientNote.Location = new Point(20, 124);
            lblRecipientNote.Name = "lblRecipientNote";
            lblRecipientNote.Size = new Size(402, 30);
            lblRecipientNote.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblRecipientNote.Text = "Inquiries from the website's Contact form are forwarded to this address.";
            // 
            // lblCompanyDetails
            // 
            lblCompanyDetails.AutoSize = true;
            lblCompanyDetails.Location = new Point(24, 18);
            lblCompanyDetails.Name = "lblCompanyDetails";
            lblCompanyDetails.Font = new Font("Segoe UI", 14F);
            lblCompanyDetails.Text = "Company Details on the Website";
            // 
            // lblCompanyAddress
            // 
            lblCompanyAddress.AutoSize = true;
            lblCompanyAddress.Location = new Point(24, 58);
            lblCompanyAddress.Name = "lblCompanyAddress";
            lblCompanyAddress.Font = new Font("Segoe UI Semibold", 10F);
            lblCompanyAddress.Text = "Address";
            // 
            // frmCompanyAddress
            // 
            frmCompanyAddress.Location = new Point(24, 80);
            frmCompanyAddress.Name = "frmCompanyAddress";
            frmCompanyAddress.Size = new Size(560, 40);
            frmCompanyAddress.TabIndex = 15;
            // 
            // txtCompanyAddress
            // 
            txtCompanyAddress.Location = new Point(12, 9);
            txtCompanyAddress.Name = "txtCompanyAddress";
            txtCompanyAddress.Size = new Size(536, 23);
            txtCompanyAddress.BorderStyle = BorderStyle.None;
            txtCompanyAddress.Font = new Font("Segoe UI", 10.5F);
            txtCompanyAddress.TabIndex = 16;
            // 
            // lblCompanyPhone
            // 
            lblCompanyPhone.AutoSize = true;
            lblCompanyPhone.Location = new Point(608, 58);
            lblCompanyPhone.Name = "lblCompanyPhone";
            lblCompanyPhone.Font = new Font("Segoe UI Semibold", 10F);
            lblCompanyPhone.Text = "Phone";
            // 
            // frmCompanyPhone
            // 
            frmCompanyPhone.Location = new Point(608, 80);
            frmCompanyPhone.Name = "frmCompanyPhone";
            frmCompanyPhone.Size = new Size(240, 40);
            frmCompanyPhone.TabIndex = 17;
            // 
            // txtCompanyPhone
            // 
            txtCompanyPhone.Location = new Point(12, 9);
            txtCompanyPhone.Name = "txtCompanyPhone";
            txtCompanyPhone.Size = new Size(216, 23);
            txtCompanyPhone.BorderStyle = BorderStyle.None;
            txtCompanyPhone.Font = new Font("Segoe UI", 10.5F);
            txtCompanyPhone.TabIndex = 18;
            // 
            // lblCompanyEmail
            // 
            lblCompanyEmail.AutoSize = true;
            lblCompanyEmail.Location = new Point(872, 58);
            lblCompanyEmail.Name = "lblCompanyEmail";
            lblCompanyEmail.Font = new Font("Segoe UI Semibold", 10F);
            lblCompanyEmail.Text = "Email";
            // 
            // frmCompanyEmail
            // 
            frmCompanyEmail.Location = new Point(872, 80);
            frmCompanyEmail.Name = "frmCompanyEmail";
            frmCompanyEmail.Size = new Size(296, 40);
            frmCompanyEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            frmCompanyEmail.TabIndex = 19;
            // 
            // txtCompanyEmail
            // 
            txtCompanyEmail.Location = new Point(12, 9);
            txtCompanyEmail.Name = "txtCompanyEmail";
            txtCompanyEmail.Size = new Size(272, 23);
            txtCompanyEmail.BorderStyle = BorderStyle.None;
            txtCompanyEmail.Font = new Font("Segoe UI", 10.5F);
            txtCompanyEmail.TabIndex = 20;
            // 
            // lblCopyright
            // 
            lblCopyright.AutoSize = true;
            lblCopyright.Location = new Point(24, 132);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Font = new Font("Segoe UI Semibold", 10F);
            lblCopyright.Text = "Footer";
            // 
            // frmCopyright
            // 
            frmCopyright.Location = new Point(24, 154);
            frmCopyright.Name = "frmCopyright";
            frmCopyright.Size = new Size(560, 40);
            frmCopyright.TabIndex = 21;
            // 
            // txtCopyright
            // 
            txtCopyright.Location = new Point(12, 9);
            txtCopyright.Name = "txtCopyright";
            txtCopyright.Size = new Size(536, 23);
            txtCopyright.BorderStyle = BorderStyle.None;
            txtCopyright.Font = new Font("Segoe UI", 10.5F);
            txtCopyright.TabIndex = 22;
            // 
            // lblContactSettingsMsg
            // 
            lblContactSettingsMsg.Location = new Point(608, 154);
            lblContactSettingsMsg.Name = "lblContactSettingsMsg";
            lblContactSettingsMsg.Size = new Size(400, 40);
            lblContactSettingsMsg.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnSaveCompanyDetails
            // 
            btnSaveCompanyDetails.Location = new Point(1018, 154);
            btnSaveCompanyDetails.Name = "btnSaveCompanyDetails";
            btnSaveCompanyDetails.Size = new Size(150, 40);
            btnSaveCompanyDetails.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveCompanyDetails.Font = new Font("Segoe UI", 10F);
            btnSaveCompanyDetails.Text = "Save Details";
            btnSaveCompanyDetails.TabIndex = 23;
            btnSaveCompanyDetails.Click += btnSaveCompanyDetails_Click;
            // 
            // lblClientInquiries
            // 
            lblClientInquiries.AutoSize = true;
            lblClientInquiries.Location = new Point(24, 18);
            lblClientInquiries.Name = "lblClientInquiries";
            lblClientInquiries.Font = new Font("Segoe UI", 14F);
            lblClientInquiries.Text = "Client Inquiries";
            // 
            // lblInquiriesSub
            // 
            lblInquiriesSub.AutoSize = true;
            lblInquiriesSub.Location = new Point(24, 48);
            lblInquiriesSub.Name = "lblInquiriesSub";
            lblInquiriesSub.Text = "Website contact form submissions. Double-click one to open it.";
            // 
            // btnRefreshContact
            // 
            btnRefreshContact.Location = new Point(858, 18);
            btnRefreshContact.Name = "btnRefreshContact";
            btnRefreshContact.Size = new Size(140, 40);
            btnRefreshContact.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefreshContact.Font = new Font("Segoe UI", 10F);
            btnRefreshContact.Text = "Refresh";
            btnRefreshContact.TabIndex = 24;
            btnRefreshContact.Click += btnRefreshContact_Click;
            // 
            // btnExportInquiries
            // 
            btnExportInquiries.Location = new Point(1008, 18);
            btnExportInquiries.Name = "btnExportInquiries";
            btnExportInquiries.Size = new Size(160, 40);
            btnExportInquiries.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportInquiries.Font = new Font("Segoe UI", 10F);
            btnExportInquiries.Text = "Export to Excel";
            btnExportInquiries.TabIndex = 25;
            btnExportInquiries.Click += btnExportInquiries_Click;
            // 
            // frmInquirySearch
            // 
            frmInquirySearch.Location = new Point(24, 82);
            frmInquirySearch.Name = "frmInquirySearch";
            frmInquirySearch.Size = new Size(380, 40);
            frmInquirySearch.TabIndex = 26;
            // 
            // txtInquirySearch
            // 
            txtInquirySearch.Location = new Point(12, 9);
            txtInquirySearch.Name = "txtInquirySearch";
            txtInquirySearch.Size = new Size(356, 23);
            txtInquirySearch.BorderStyle = BorderStyle.None;
            txtInquirySearch.Font = new Font("Segoe UI", 10.5F);
            txtInquirySearch.PlaceholderText = "Search name, email, service or location...";
            txtInquirySearch.TabIndex = 27;
            // 
            // cboInquiryStatusFilter
            // 
            cboInquiryStatusFilter.Location = new Point(420, 88);
            cboInquiryStatusFilter.Name = "cboInquiryStatusFilter";
            cboInquiryStatusFilter.Size = new Size(150, 27);
            cboInquiryStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboInquiryStatusFilter.Font = new Font("Segoe UI", 10.5F);
            cboInquiryStatusFilter.TabIndex = 28;
            cboInquiryStatusFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblInquiryFrom
            // 
            lblInquiryFrom.AutoSize = true;
            lblInquiryFrom.Location = new Point(588, 92);
            lblInquiryFrom.Name = "lblInquiryFrom";
            lblInquiryFrom.Text = "From";
            // 
            // dtpInquiryFrom
            // 
            dtpInquiryFrom.Location = new Point(628, 88);
            dtpInquiryFrom.Name = "dtpInquiryFrom";
            dtpInquiryFrom.Size = new Size(150, 27);
            dtpInquiryFrom.Format = DateTimePickerFormat.Short;
            dtpInquiryFrom.ShowCheckBox = true;
            dtpInquiryFrom.Checked = false;
            dtpInquiryFrom.Font = new Font("Segoe UI", 10.5F);
            dtpInquiryFrom.TabIndex = 29;
            dtpInquiryFrom.ValueChanged += Filter_Changed;
            // 
            // lblInquiryTo
            // 
            lblInquiryTo.AutoSize = true;
            lblInquiryTo.Location = new Point(796, 92);
            lblInquiryTo.Name = "lblInquiryTo";
            lblInquiryTo.Text = "To";
            // 
            // dtpInquiryTo
            // 
            dtpInquiryTo.Location = new Point(820, 88);
            dtpInquiryTo.Name = "dtpInquiryTo";
            dtpInquiryTo.Size = new Size(150, 27);
            dtpInquiryTo.Format = DateTimePickerFormat.Short;
            dtpInquiryTo.ShowCheckBox = true;
            dtpInquiryTo.Checked = false;
            dtpInquiryTo.Font = new Font("Segoe UI", 10.5F);
            dtpInquiryTo.TabIndex = 30;
            dtpInquiryTo.ValueChanged += Filter_Changed;
            // 
            // btnResetInquiryFilter
            // 
            btnResetInquiryFilter.Location = new Point(1028, 82);
            btnResetInquiryFilter.Name = "btnResetInquiryFilter";
            btnResetInquiryFilter.Size = new Size(140, 40);
            btnResetInquiryFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnResetInquiryFilter.Text = "Reset Filter";
            btnResetInquiryFilter.TabIndex = 31;
            btnResetInquiryFilter.Click += btnResetInquiryFilter_Click;
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
            dgvInquiries.Location = new Point(24, 138);
            dgvInquiries.Name = "dgvInquiries";
            dgvInquiries.Size = new Size(1144, 280);
            dgvInquiries.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvInquiries.TabIndex = 32;
            dgvInquiries.CellDoubleClick += dgvInquiries_CellDoubleClick;
            // 
            // lblInquiryCount
            // 
            lblInquiryCount.AutoSize = true;
            lblInquiryCount.Location = new Point(24, 440);
            lblInquiryCount.Name = "lblInquiryCount";
            lblInquiryCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblInquiryCount.Text = "";
            // 
            // btnOpenInquiry
            // 
            btnOpenInquiry.Location = new Point(1018, 432);
            btnOpenInquiry.Name = "btnOpenInquiry";
            btnOpenInquiry.Size = new Size(150, 36);
            btnOpenInquiry.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOpenInquiry.Text = "Open Inquiry";
            btnOpenInquiry.TabIndex = 33;
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
            Controls.Add(cardInquiries);
            Controls.Add(pnlGap2);
            Controls.Add(cardCompany);
            Controls.Add(pnlGap1);
            Controls.Add(pnlKpis);
            Name = "ContactUsPage";
            Size = new Size(1240, 940);
            Load += ContactUsPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvInquiries).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Company.OfficeApp.UI.Card cardInquiries;
        private Panel pnlGap2;
        private Company.OfficeApp.UI.Card cardCompany;
        private Panel pnlGap1;
        private Panel pnlKpis;
        private Company.OfficeApp.UI.Card cardEmail;
        private Panel pnlKpiGap3;
        private Company.OfficeApp.UI.Card cardVisits;
        private Panel pnlKpiGap2;
        private Company.OfficeApp.UI.Card cardPending;
        private Panel pnlKpiGap1;
        private Company.OfficeApp.UI.Card cardTotal;
        private Label lblTotalTitle;
        private Label lblTotalInquiries;
        private Label lblTotalNote;
        private Label lblPendingTitle;
        private Label lblPendingInquiries;
        private Label lblPendingNote;
        private Label lblVisitsTitle;
        private Label lblWebsiteVisits;
        private Label lblVisitsNote;
        private Label lblEmailSettingsTitle;
        private Label lblRecipientEmail;
        private Company.OfficeApp.UI.InputFrame frmRecipient;
        private TextBox txtRecipientEmail;
        private Button btnSaveRecipientEmail;
        private Label lblRecipientNote;
        private Label lblCompanyDetails;
        private Label lblCompanyAddress;
        private Company.OfficeApp.UI.InputFrame frmCompanyAddress;
        private TextBox txtCompanyAddress;
        private Label lblCompanyPhone;
        private Company.OfficeApp.UI.InputFrame frmCompanyPhone;
        private TextBox txtCompanyPhone;
        private Label lblCompanyEmail;
        private Company.OfficeApp.UI.InputFrame frmCompanyEmail;
        private TextBox txtCompanyEmail;
        private Label lblCopyright;
        private Company.OfficeApp.UI.InputFrame frmCopyright;
        private TextBox txtCopyright;
        private Label lblContactSettingsMsg;
        private Button btnSaveCompanyDetails;
        private Label lblClientInquiries;
        private Label lblInquiriesSub;
        private Button btnRefreshContact;
        private Button btnExportInquiries;
        private Company.OfficeApp.UI.InputFrame frmInquirySearch;
        private TextBox txtInquirySearch;
        private ComboBox cboInquiryStatusFilter;
        private Label lblInquiryFrom;
        private DateTimePicker dtpInquiryFrom;
        private Label lblInquiryTo;
        private DateTimePicker dtpInquiryTo;
        private Button btnResetInquiryFilter;
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
