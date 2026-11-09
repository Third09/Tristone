namespace Company.OfficeApp.Pages
{
    partial class QuotationsPage
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
            txtQuotationSearch = new TextBox();
            lblStatusFilter = new Label();
            cboQuotationStatusFilter = new ComboBox();
            dgvQuotations = new DataGridView();
            btnNewQuotation = new Button();
            btnDeleteQuotation = new Button();
            btnPrintQuotation = new Button();
            btnSaveQuotationPdf = new Button();
            lblFormMode = new Label();
            lblClientName = new Label();
            txtClientName = new TextBox();
            lblClientAddress = new Label();
            txtClientAddress = new TextBox();
            lblAttention = new Label();
            txtAttention = new TextBox();
            lblClientContact = new Label();
            txtClientContact = new TextBox();
            lblClientEmail = new Label();
            txtClientEmail = new TextBox();
            lblFromInquiry = new Label();
            cboFromInquiry = new ComboBox();
            lblQuoteDate = new Label();
            dtpQuoteDate = new DateTimePicker();
            lblValidUntil = new Label();
            dtpValidUntil = new DateTimePicker();
            lblPlannedStart = new Label();
            dtpPlannedStart = new DateTimePicker();
            lblProjectTitle = new Label();
            txtProjectTitle = new TextBox();
            lblSiteLocation = new Label();
            txtSiteLocation = new TextBox();
            lblSector = new Label();
            cboSector = new ComboBox();
            lblScope = new Label();
            dgvScope = new DataGridView();
            btnRemoveScope = new Button();
            lblCosts = new Label();
            dgvLines = new DataGridView();
            btnRemoveLine = new Button();
            lblTotal = new Label();
            lblPrintedNotes = new Label();
            txtPrintedNotes = new TextBox();
            lblWorkDuration = new Label();
            txtWorkDuration = new TextBox();
            lblPaymentTerms = new Label();
            cboPaymentTerms = new ComboBox();
            lblPayableTo = new Label();
            txtPayableTo = new TextBox();
            lblPayableNumber = new Label();
            txtPayableNumber = new TextBox();
            lblSignatoryName = new Label();
            txtSignatoryName = new TextBox();
            lblSignatoryTitle = new Label();
            txtSignatoryTitle = new TextBox();
            lblInternalNotes = new Label();
            txtInternalNotes = new TextBox();
            lblQuotationMsg = new Label();
            btnSaveQuotation = new Button();
            btnCancelQuotation = new Button();
            btnSubmitQuotation = new Button();
            btnApproveQuotation = new Button();
            btnRejectQuotation = new Button();
            btnAcceptQuotation = new Button();
            btnDeclineQuotation = new Button();
            colQuoteNo = new DataGridViewTextBoxColumn();
            colQuoteDate = new DataGridViewTextBoxColumn();
            colClient = new DataGridViewTextBoxColumn();
            colProject = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colValidUntil = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colWork = new DataGridViewTextBoxColumn();
            colDetails = new DataGridViewTextBoxColumn();
            colPrice = new DataGridViewTextBoxColumn();
            colItem = new DataGridViewTextBoxColumn();
            colAmount = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvQuotations).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvScope).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            SuspendLayout();
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(38, 36);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search";
            // 
            // txtQuotationSearch
            // 
            txtQuotationSearch.Location = new Point(95, 32);
            txtQuotationSearch.Name = "txtQuotationSearch";
            txtQuotationSearch.Size = new Size(207, 23);
            txtQuotationSearch.PlaceholderText = "No., client or project";
            txtQuotationSearch.TabIndex = 0;
            // 
            // lblStatusFilter
            // 
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(318, 36);
            lblStatusFilter.Name = "lblStatusFilter";
            lblStatusFilter.Text = "Status";
            // 
            // cboQuotationStatusFilter
            // 
            cboQuotationStatusFilter.Location = new Point(376, 32);
            cboQuotationStatusFilter.Name = "cboQuotationStatusFilter";
            cboQuotationStatusFilter.Size = new Size(150, 23);
            cboQuotationStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuotationStatusFilter.TabIndex = 1;
            cboQuotationStatusFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // dgvQuotations
            // 
            dgvQuotations.AllowUserToAddRows = false;
            dgvQuotations.AllowUserToDeleteRows = false;
            dgvQuotations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvQuotations.Columns.AddRange(new DataGridViewColumn[] { colQuoteNo, colQuoteDate, colClient, colProject, colTotal, colValidUntil, colStatus });
            dgvQuotations.MultiSelect = false;
            dgvQuotations.ReadOnly = true;
            dgvQuotations.RowHeadersVisible = false;
            dgvQuotations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvQuotations.Location = new Point(38, 74);
            dgvQuotations.Name = "dgvQuotations";
            dgvQuotations.Size = new Size(1070, 224);
            dgvQuotations.TabIndex = 2;
            dgvQuotations.SelectionChanged += dgvQuotations_SelectionChanged;
            // 
            // btnNewQuotation
            // 
            btnNewQuotation.Location = new Point(38, 309);
            btnNewQuotation.Name = "btnNewQuotation";
            btnNewQuotation.Size = new Size(138, 36);
            btnNewQuotation.Text = "New Quotation";
            btnNewQuotation.TabIndex = 3;
            btnNewQuotation.Click += btnNewQuotation_Click;
            // 
            // btnDeleteQuotation
            // 
            btnDeleteQuotation.Location = new Point(183, 309);
            btnDeleteQuotation.Name = "btnDeleteQuotation";
            btnDeleteQuotation.Size = new Size(103, 36);
            btnDeleteQuotation.Text = "Delete";
            btnDeleteQuotation.TabIndex = 4;
            btnDeleteQuotation.Click += btnDeleteQuotation_Click;
            // 
            // btnPrintQuotation
            // 
            btnPrintQuotation.Location = new Point(293, 309);
            btnPrintQuotation.Name = "btnPrintQuotation";
            btnPrintQuotation.Size = new Size(103, 36);
            btnPrintQuotation.Text = "Print";
            btnPrintQuotation.TabIndex = 5;
            btnPrintQuotation.Click += btnPrintQuotation_Click;
            // 
            // btnSaveQuotationPdf
            // 
            btnSaveQuotationPdf.Location = new Point(403, 309);
            btnSaveQuotationPdf.Name = "btnSaveQuotationPdf";
            btnSaveQuotationPdf.Size = new Size(138, 36);
            btnSaveQuotationPdf.Text = "Save as PDF";
            btnSaveQuotationPdf.TabIndex = 6;
            btnSaveQuotationPdf.Click += btnSaveQuotationPdf_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(38, 367);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New quotation";
            lblFormMode.Font = new Font("Segoe UI Semibold", 11.5F);
            // 
            // lblClientName
            // 
            lblClientName.AutoSize = true;
            lblClientName.Location = new Point(38, 399);
            lblClientName.Name = "lblClientName";
            lblClientName.Text = "Customer";
            // 
            // txtClientName
            // 
            txtClientName.Location = new Point(38, 423);
            txtClientName.Name = "txtClientName";
            txtClientName.Size = new Size(322, 23);
            txtClientName.MaxLength = 150;
            txtClientName.TabIndex = 7;
            // 
            // lblClientAddress
            // 
            lblClientAddress.AutoSize = true;
            lblClientAddress.Location = new Point(38, 462);
            lblClientAddress.Name = "lblClientAddress";
            lblClientAddress.Text = "Address";
            // 
            // txtClientAddress
            // 
            txtClientAddress.Location = new Point(38, 486);
            txtClientAddress.Name = "txtClientAddress";
            txtClientAddress.Size = new Size(322, 23);
            txtClientAddress.MaxLength = 255;
            txtClientAddress.TabIndex = 8;
            // 
            // lblAttention
            // 
            lblAttention.AutoSize = true;
            lblAttention.Location = new Point(38, 526);
            lblAttention.Name = "lblAttention";
            lblAttention.Text = "Attention (person it is addressed to)";
            // 
            // txtAttention
            // 
            txtAttention.Location = new Point(38, 549);
            txtAttention.Name = "txtAttention";
            txtAttention.Size = new Size(322, 23);
            txtAttention.MaxLength = 150;
            txtAttention.TabIndex = 9;
            // 
            // lblClientContact
            // 
            lblClientContact.AutoSize = true;
            lblClientContact.Location = new Point(380, 399);
            lblClientContact.Name = "lblClientContact";
            lblClientContact.Text = "Contact number (not printed)";
            // 
            // txtClientContact
            // 
            txtClientContact.Location = new Point(380, 423);
            txtClientContact.Name = "txtClientContact";
            txtClientContact.Size = new Size(322, 23);
            txtClientContact.MaxLength = 60;
            txtClientContact.TabIndex = 10;
            // 
            // lblClientEmail
            // 
            lblClientEmail.AutoSize = true;
            lblClientEmail.Location = new Point(380, 462);
            lblClientEmail.Name = "lblClientEmail";
            lblClientEmail.Text = "Email (not printed)";
            // 
            // txtClientEmail
            // 
            txtClientEmail.Location = new Point(380, 486);
            txtClientEmail.Name = "txtClientEmail";
            txtClientEmail.Size = new Size(322, 23);
            txtClientEmail.MaxLength = 150;
            txtClientEmail.TabIndex = 11;
            // 
            // lblFromInquiry
            // 
            lblFromInquiry.AutoSize = true;
            lblFromInquiry.Location = new Point(380, 526);
            lblFromInquiry.Name = "lblFromInquiry";
            lblFromInquiry.Text = "From a website inquiry";
            // 
            // cboFromInquiry
            // 
            cboFromInquiry.Location = new Point(380, 549);
            cboFromInquiry.Name = "cboFromInquiry";
            cboFromInquiry.Size = new Size(322, 23);
            cboFromInquiry.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFromInquiry.TabIndex = 12;
            cboFromInquiry.SelectionChangeCommitted += cboFromInquiry_SelectionChangeCommitted;
            // 
            // lblQuoteDate
            // 
            lblQuoteDate.AutoSize = true;
            lblQuoteDate.Location = new Point(723, 399);
            lblQuoteDate.Name = "lblQuoteDate";
            lblQuoteDate.Text = "Date";
            // 
            // dtpQuoteDate
            // 
            dtpQuoteDate.Location = new Point(723, 423);
            dtpQuoteDate.Name = "dtpQuoteDate";
            dtpQuoteDate.Size = new Size(161, 23);
            dtpQuoteDate.Format = DateTimePickerFormat.Short;
            dtpQuoteDate.TabIndex = 13;
            // 
            // lblValidUntil
            // 
            lblValidUntil.AutoSize = true;
            lblValidUntil.Location = new Point(723, 462);
            lblValidUntil.Name = "lblValidUntil";
            lblValidUntil.Text = "Valid until (not printed)";
            // 
            // dtpValidUntil
            // 
            dtpValidUntil.Location = new Point(723, 486);
            dtpValidUntil.Name = "dtpValidUntil";
            dtpValidUntil.Size = new Size(161, 23);
            dtpValidUntil.Format = DateTimePickerFormat.Short;
            dtpValidUntil.TabIndex = 14;
            // 
            // lblPlannedStart
            // 
            lblPlannedStart.AutoSize = true;
            lblPlannedStart.Location = new Point(723, 526);
            lblPlannedStart.Name = "lblPlannedStart";
            lblPlannedStart.Text = "Planned start (tick to set)";
            // 
            // dtpPlannedStart
            // 
            dtpPlannedStart.Location = new Point(723, 549);
            dtpPlannedStart.Name = "dtpPlannedStart";
            dtpPlannedStart.Size = new Size(161, 23);
            dtpPlannedStart.Format = DateTimePickerFormat.Short;
            dtpPlannedStart.ShowCheckBox = true;
            dtpPlannedStart.Checked = false;
            dtpPlannedStart.TabIndex = 15;
            // 
            // lblProjectTitle
            // 
            lblProjectTitle.AutoSize = true;
            lblProjectTitle.Location = new Point(900, 399);
            lblProjectTitle.Name = "lblProjectTitle";
            lblProjectTitle.Text = "Project name";
            lblProjectTitle.Font = new Font("Segoe UI", 9F);
            // 
            // txtProjectTitle
            // 
            txtProjectTitle.Location = new Point(900, 423);
            txtProjectTitle.Name = "txtProjectTitle";
            txtProjectTitle.Size = new Size(207, 23);
            txtProjectTitle.MaxLength = 150;
            txtProjectTitle.TabIndex = 16;
            // 
            // lblSiteLocation
            // 
            lblSiteLocation.AutoSize = true;
            lblSiteLocation.Location = new Point(900, 462);
            lblSiteLocation.Name = "lblSiteLocation";
            lblSiteLocation.Text = "Site location";
            // 
            // txtSiteLocation
            // 
            txtSiteLocation.Location = new Point(900, 486);
            txtSiteLocation.Name = "txtSiteLocation";
            txtSiteLocation.Size = new Size(207, 23);
            txtSiteLocation.MaxLength = 255;
            txtSiteLocation.TabIndex = 17;
            // 
            // lblSector
            // 
            lblSector.AutoSize = true;
            lblSector.Location = new Point(900, 526);
            lblSector.Name = "lblSector";
            lblSector.Text = "Sector";
            // 
            // cboSector
            // 
            cboSector.Location = new Point(900, 549);
            cboSector.Name = "cboSector";
            cboSector.Size = new Size(207, 23);
            cboSector.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSector.TabIndex = 18;
            // 
            // lblScope
            // 
            lblScope.AutoSize = true;
            lblScope.Location = new Point(38, 597);
            lblScope.Name = "lblScope";
            lblScope.Text = "Scope of Works (printed A, B, C...; price optional)";
            // 
            // dgvScope
            // 
            dgvScope.AllowUserToAddRows = false;
            dgvScope.AllowUserToDeleteRows = false;
            dgvScope.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvScope.Columns.AddRange(new DataGridViewColumn[] { colWork, colDetails, colPrice });
            dgvScope.MultiSelect = false;
            dgvScope.ReadOnly = true;
            dgvScope.RowHeadersVisible = false;
            dgvScope.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvScope.Location = new Point(38, 621);
            dgvScope.Name = "dgvScope";
            dgvScope.Size = new Size(690, 172);
            dgvScope.TabIndex = 19;
            dgvScope.CellEndEdit += Lines_Changed;
            dgvScope.RowsRemoved += Lines_RowsRemoved;
            // 
            // btnRemoveScope
            // 
            btnRemoveScope.Location = new Point(38, 800);
            btnRemoveScope.Name = "btnRemoveScope";
            btnRemoveScope.Size = new Size(126, 36);
            btnRemoveScope.Text = "Remove Line";
            btnRemoveScope.TabIndex = 20;
            btnRemoveScope.Click += btnRemoveScope_Click;
            // 
            // lblCosts
            // 
            lblCosts.AutoSize = true;
            lblCosts.Location = new Point(748, 597);
            lblCosts.Name = "lblCosts";
            lblCosts.Text = "Other costs (e.g. Labor Cost)";
            // 
            // dgvLines
            // 
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.Columns.AddRange(new DataGridViewColumn[] { colItem, colAmount });
            dgvLines.MultiSelect = false;
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Location = new Point(748, 621);
            dgvLines.Name = "dgvLines";
            dgvLines.Size = new Size(359, 172);
            dgvLines.TabIndex = 21;
            dgvLines.CellEndEdit += Lines_Changed;
            dgvLines.RowsRemoved += Lines_RowsRemoved;
            // 
            // btnRemoveLine
            // 
            btnRemoveLine.Location = new Point(748, 800);
            btnRemoveLine.Name = "btnRemoveLine";
            btnRemoveLine.Size = new Size(126, 36);
            btnRemoveLine.Text = "Remove Cost";
            btnRemoveLine.TabIndex = 22;
            btnRemoveLine.Click += btnRemoveLine_Click;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(886, 805);
            lblTotal.Name = "lblTotal";
            lblTotal.Text = "Total Cost: -";
            lblTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            // 
            // lblPrintedNotes
            // 
            lblPrintedNotes.AutoSize = true;
            lblPrintedNotes.Location = new Point(38, 850);
            lblPrintedNotes.Name = "lblPrintedNotes";
            lblPrintedNotes.Text = "Notes (printed; one per line)";
            // 
            // txtPrintedNotes
            // 
            txtPrintedNotes.Location = new Point(38, 874);
            txtPrintedNotes.Name = "txtPrintedNotes";
            txtPrintedNotes.Size = new Size(529, 92);
            txtPrintedNotes.Multiline = true;
            txtPrintedNotes.ScrollBars = ScrollBars.Vertical;
            txtPrintedNotes.MaxLength = 2000;
            txtPrintedNotes.TabIndex = 23;
            // 
            // lblWorkDuration
            // 
            lblWorkDuration.AutoSize = true;
            lblWorkDuration.Location = new Point(588, 850);
            lblWorkDuration.Name = "lblWorkDuration";
            lblWorkDuration.Text = "Work duration";
            // 
            // txtWorkDuration
            // 
            txtWorkDuration.Location = new Point(588, 874);
            txtWorkDuration.Name = "txtWorkDuration";
            txtWorkDuration.Size = new Size(161, 23);
            txtWorkDuration.MaxLength = 100;
            txtWorkDuration.PlaceholderText = "e.g. 30 days";
            txtWorkDuration.TabIndex = 24;
            // 
            // lblPaymentTerms
            // 
            lblPaymentTerms.AutoSize = true;
            lblPaymentTerms.Location = new Point(762, 850);
            lblPaymentTerms.Name = "lblPaymentTerms";
            lblPaymentTerms.Text = "Terms of payment";
            // 
            // cboPaymentTerms
            // 
            cboPaymentTerms.Location = new Point(762, 874);
            cboPaymentTerms.Name = "cboPaymentTerms";
            cboPaymentTerms.Size = new Size(161, 23);
            cboPaymentTerms.DropDownStyle = ComboBoxStyle.DropDown;
            cboPaymentTerms.MaxLength = 150;
            cboPaymentTerms.TabIndex = 25;
            // 
            // lblPayableTo
            // 
            lblPayableTo.AutoSize = true;
            lblPayableTo.Location = new Point(937, 850);
            lblPayableTo.Name = "lblPayableTo";
            lblPayableTo.Text = "Cash payable to";
            // 
            // txtPayableTo
            // 
            txtPayableTo.Location = new Point(937, 874);
            txtPayableTo.Name = "txtPayableTo";
            txtPayableTo.Size = new Size(170, 23);
            txtPayableTo.MaxLength = 150;
            txtPayableTo.TabIndex = 26;
            // 
            // lblPayableNumber
            // 
            lblPayableNumber.AutoSize = true;
            lblPayableNumber.Location = new Point(588, 914);
            lblPayableNumber.Name = "lblPayableNumber";
            lblPayableNumber.Text = "Payable number (#)";
            // 
            // txtPayableNumber
            // 
            txtPayableNumber.Location = new Point(588, 937);
            txtPayableNumber.Name = "txtPayableNumber";
            txtPayableNumber.Size = new Size(161, 23);
            txtPayableNumber.MaxLength = 60;
            txtPayableNumber.TabIndex = 27;
            // 
            // lblSignatoryName
            // 
            lblSignatoryName.AutoSize = true;
            lblSignatoryName.Location = new Point(762, 914);
            lblSignatoryName.Name = "lblSignatoryName";
            lblSignatoryName.Text = "Very truly yours (name)";
            // 
            // txtSignatoryName
            // 
            txtSignatoryName.Location = new Point(762, 937);
            txtSignatoryName.Name = "txtSignatoryName";
            txtSignatoryName.Size = new Size(161, 23);
            txtSignatoryName.MaxLength = 150;
            txtSignatoryName.TabIndex = 28;
            // 
            // lblSignatoryTitle
            // 
            lblSignatoryTitle.AutoSize = true;
            lblSignatoryTitle.Location = new Point(937, 914);
            lblSignatoryTitle.Name = "lblSignatoryTitle";
            lblSignatoryTitle.Text = "Position";
            lblSignatoryTitle.Font = new Font("Segoe UI", 9F);
            // 
            // txtSignatoryTitle
            // 
            txtSignatoryTitle.Location = new Point(937, 937);
            txtSignatoryTitle.Name = "txtSignatoryTitle";
            txtSignatoryTitle.Size = new Size(170, 23);
            txtSignatoryTitle.MaxLength = 100;
            txtSignatoryTitle.TabIndex = 29;
            // 
            // lblInternalNotes
            // 
            lblInternalNotes.AutoSize = true;
            lblInternalNotes.Location = new Point(38, 977);
            lblInternalNotes.Name = "lblInternalNotes";
            lblInternalNotes.Text = "Internal notes (not printed)";
            // 
            // txtInternalNotes
            // 
            txtInternalNotes.Location = new Point(38, 1001);
            txtInternalNotes.Name = "txtInternalNotes";
            txtInternalNotes.Size = new Size(1070, 53);
            txtInternalNotes.Multiline = true;
            txtInternalNotes.ScrollBars = ScrollBars.Vertical;
            txtInternalNotes.MaxLength = 1000;
            txtInternalNotes.TabIndex = 30;
            // 
            // lblQuotationMsg
            // 
            lblQuotationMsg.AutoSize = true;
            lblQuotationMsg.Location = new Point(38, 1064);
            lblQuotationMsg.Name = "lblQuotationMsg";
            lblQuotationMsg.MaximumSize = new Size(930, 0);
            // 
            // btnSaveQuotation
            // 
            btnSaveQuotation.Location = new Point(38, 1096);
            btnSaveQuotation.Name = "btnSaveQuotation";
            btnSaveQuotation.Size = new Size(103, 36);
            btnSaveQuotation.Text = "Save";
            btnSaveQuotation.TabIndex = 31;
            btnSaveQuotation.Click += btnSaveQuotation_Click;
            // 
            // btnCancelQuotation
            // 
            btnCancelQuotation.Location = new Point(148, 1096);
            btnCancelQuotation.Name = "btnCancelQuotation";
            btnCancelQuotation.Size = new Size(103, 36);
            btnCancelQuotation.Text = "Cancel";
            btnCancelQuotation.TabIndex = 32;
            btnCancelQuotation.Click += btnCancelQuotation_Click;
            // 
            // btnSubmitQuotation
            // 
            btnSubmitQuotation.Location = new Point(259, 1096);
            btnSubmitQuotation.Name = "btnSubmitQuotation";
            btnSubmitQuotation.Size = new Size(172, 36);
            btnSubmitQuotation.Text = "Send for Approval";
            btnSubmitQuotation.TabIndex = 33;
            btnSubmitQuotation.Click += btnSubmitQuotation_Click;
            // 
            // btnApproveQuotation
            // 
            btnApproveQuotation.Location = new Point(438, 1096);
            btnApproveQuotation.Name = "btnApproveQuotation";
            btnApproveQuotation.Size = new Size(103, 36);
            btnApproveQuotation.Text = "Approve";
            btnApproveQuotation.TabIndex = 34;
            btnApproveQuotation.Click += btnApproveQuotation_Click;
            // 
            // btnRejectQuotation
            // 
            btnRejectQuotation.Location = new Point(548, 1096);
            btnRejectQuotation.Name = "btnRejectQuotation";
            btnRejectQuotation.Size = new Size(103, 36);
            btnRejectQuotation.Text = "Reject";
            btnRejectQuotation.TabIndex = 35;
            btnRejectQuotation.Click += btnRejectQuotation_Click;
            // 
            // btnAcceptQuotation
            // 
            btnAcceptQuotation.Location = new Point(659, 1096);
            btnAcceptQuotation.Name = "btnAcceptQuotation";
            btnAcceptQuotation.Size = new Size(207, 36);
            btnAcceptQuotation.Text = "Client Accepted: Make Project";
            btnAcceptQuotation.TabIndex = 36;
            btnAcceptQuotation.Click += btnAcceptQuotation_Click;
            // 
            // btnDeclineQuotation
            // 
            btnDeclineQuotation.Location = new Point(873, 1096);
            btnDeclineQuotation.Name = "btnDeclineQuotation";
            btnDeclineQuotation.Size = new Size(138, 36);
            btnDeclineQuotation.Text = "Client Declined";
            btnDeclineQuotation.TabIndex = 37;
            btnDeclineQuotation.Click += btnDeclineQuotation_Click;
            // 
            // colQuoteNo
            // 
            colQuoteNo.DataPropertyName = "QuoteNo";
            colQuoteNo.HeaderText = "No.";
            colQuoteNo.Name = "colQuoteNo";
            colQuoteNo.FillWeight = 50;
            // 
            // colQuoteDate
            // 
            colQuoteDate.DataPropertyName = "QuoteDate";
            colQuoteDate.HeaderText = "Date";
            colQuoteDate.Name = "colQuoteDate";
            colQuoteDate.FillWeight = 55;
            colQuoteDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colClient
            // 
            colClient.DataPropertyName = "ClientName";
            colClient.HeaderText = "Client";
            colClient.Name = "colClient";
            // 
            // colProject
            // 
            colProject.DataPropertyName = "ProjectTitle";
            colProject.HeaderText = "Project";
            colProject.Name = "colProject";
            // 
            // colTotal
            // 
            colTotal.DataPropertyName = "Total";
            colTotal.HeaderText = "Total";
            colTotal.Name = "colTotal";
            colTotal.FillWeight = 60;
            colTotal.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" };
            // 
            // colValidUntil
            // 
            colValidUntil.DataPropertyName = "ValidUntil";
            colValidUntil.HeaderText = "Valid Until";
            colValidUntil.Name = "colValidUntil";
            colValidUntil.FillWeight = 55;
            colValidUntil.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.FillWeight = 50;
            // 
            // colWork
            // 
            colWork.DataPropertyName = "Work";
            colWork.HeaderText = "Work";
            colWork.Name = "colWork";
            colWork.FillWeight = 170;
            // 
            // colDetails
            // 
            colDetails.DataPropertyName = "Details";
            colDetails.HeaderText = "Size / Details";
            colDetails.Name = "colDetails";
            colDetails.FillWeight = 110;
            // 
            // colPrice
            // 
            colPrice.DataPropertyName = "Price";
            colPrice.HeaderText = "Price";
            colPrice.Name = "colPrice";
            colPrice.FillWeight = 70;
            colPrice.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" };
            // 
            // colItem
            // 
            colItem.DataPropertyName = "Description";
            colItem.HeaderText = "Description";
            colItem.Name = "colItem";
            colItem.FillWeight = 150;
            // 
            // colAmount
            // 
            colAmount.DataPropertyName = "Amount";
            colAmount.HeaderText = "Amount";
            colAmount.Name = "colAmount";
            colAmount.FillWeight = 90;
            colAmount.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" };
            // 
            // QuotationsPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblSearch);
            Controls.Add(txtQuotationSearch);
            Controls.Add(lblStatusFilter);
            Controls.Add(cboQuotationStatusFilter);
            Controls.Add(dgvQuotations);
            Controls.Add(btnNewQuotation);
            Controls.Add(btnDeleteQuotation);
            Controls.Add(btnPrintQuotation);
            Controls.Add(btnSaveQuotationPdf);
            Controls.Add(lblFormMode);
            Controls.Add(lblClientName);
            Controls.Add(txtClientName);
            Controls.Add(lblClientAddress);
            Controls.Add(txtClientAddress);
            Controls.Add(lblAttention);
            Controls.Add(txtAttention);
            Controls.Add(lblClientContact);
            Controls.Add(txtClientContact);
            Controls.Add(lblClientEmail);
            Controls.Add(txtClientEmail);
            Controls.Add(lblFromInquiry);
            Controls.Add(cboFromInquiry);
            Controls.Add(lblQuoteDate);
            Controls.Add(dtpQuoteDate);
            Controls.Add(lblValidUntil);
            Controls.Add(dtpValidUntil);
            Controls.Add(lblPlannedStart);
            Controls.Add(dtpPlannedStart);
            Controls.Add(lblProjectTitle);
            Controls.Add(txtProjectTitle);
            Controls.Add(lblSiteLocation);
            Controls.Add(txtSiteLocation);
            Controls.Add(lblSector);
            Controls.Add(cboSector);
            Controls.Add(lblScope);
            Controls.Add(dgvScope);
            Controls.Add(btnRemoveScope);
            Controls.Add(lblCosts);
            Controls.Add(dgvLines);
            Controls.Add(btnRemoveLine);
            Controls.Add(lblTotal);
            Controls.Add(lblPrintedNotes);
            Controls.Add(txtPrintedNotes);
            Controls.Add(lblWorkDuration);
            Controls.Add(txtWorkDuration);
            Controls.Add(lblPaymentTerms);
            Controls.Add(cboPaymentTerms);
            Controls.Add(lblPayableTo);
            Controls.Add(txtPayableTo);
            Controls.Add(lblPayableNumber);
            Controls.Add(txtPayableNumber);
            Controls.Add(lblSignatoryName);
            Controls.Add(txtSignatoryName);
            Controls.Add(lblSignatoryTitle);
            Controls.Add(txtSignatoryTitle);
            Controls.Add(lblInternalNotes);
            Controls.Add(txtInternalNotes);
            Controls.Add(lblQuotationMsg);
            Controls.Add(btnSaveQuotation);
            Controls.Add(btnCancelQuotation);
            Controls.Add(btnSubmitQuotation);
            Controls.Add(btnApproveQuotation);
            Controls.Add(btnRejectQuotation);
            Controls.Add(btnAcceptQuotation);
            Controls.Add(btnDeclineQuotation);
            Name = "QuotationsPage";
            Size = new Size(1152, 1175);
            Load += QuotationsPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvQuotations).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvScope).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSearch;
        private TextBox txtQuotationSearch;
        private Label lblStatusFilter;
        private ComboBox cboQuotationStatusFilter;
        private DataGridView dgvQuotations;
        private Button btnNewQuotation;
        private Button btnDeleteQuotation;
        private Button btnPrintQuotation;
        private Button btnSaveQuotationPdf;
        private Label lblFormMode;
        private Label lblClientName;
        private TextBox txtClientName;
        private Label lblClientAddress;
        private TextBox txtClientAddress;
        private Label lblAttention;
        private TextBox txtAttention;
        private Label lblClientContact;
        private TextBox txtClientContact;
        private Label lblClientEmail;
        private TextBox txtClientEmail;
        private Label lblFromInquiry;
        private ComboBox cboFromInquiry;
        private Label lblQuoteDate;
        private DateTimePicker dtpQuoteDate;
        private Label lblValidUntil;
        private DateTimePicker dtpValidUntil;
        private Label lblPlannedStart;
        private DateTimePicker dtpPlannedStart;
        private Label lblProjectTitle;
        private TextBox txtProjectTitle;
        private Label lblSiteLocation;
        private TextBox txtSiteLocation;
        private Label lblSector;
        private ComboBox cboSector;
        private Label lblScope;
        private DataGridView dgvScope;
        private Button btnRemoveScope;
        private Label lblCosts;
        private DataGridView dgvLines;
        private Button btnRemoveLine;
        private Label lblTotal;
        private Label lblPrintedNotes;
        private TextBox txtPrintedNotes;
        private Label lblWorkDuration;
        private TextBox txtWorkDuration;
        private Label lblPaymentTerms;
        private ComboBox cboPaymentTerms;
        private Label lblPayableTo;
        private TextBox txtPayableTo;
        private Label lblPayableNumber;
        private TextBox txtPayableNumber;
        private Label lblSignatoryName;
        private TextBox txtSignatoryName;
        private Label lblSignatoryTitle;
        private TextBox txtSignatoryTitle;
        private Label lblInternalNotes;
        private TextBox txtInternalNotes;
        private Label lblQuotationMsg;
        private Button btnSaveQuotation;
        private Button btnCancelQuotation;
        private Button btnSubmitQuotation;
        private Button btnApproveQuotation;
        private Button btnRejectQuotation;
        private Button btnAcceptQuotation;
        private Button btnDeclineQuotation;
        private DataGridViewTextBoxColumn colQuoteNo;
        private DataGridViewTextBoxColumn colQuoteDate;
        private DataGridViewTextBoxColumn colClient;
        private DataGridViewTextBoxColumn colProject;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colValidUntil;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colWork;
        private DataGridViewTextBoxColumn colDetails;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colItem;
        private DataGridViewTextBoxColumn colAmount;
    }
}
