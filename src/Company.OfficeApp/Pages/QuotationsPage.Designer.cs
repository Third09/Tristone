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
            lblClientContact = new Label();
            txtClientContact = new TextBox();
            lblClientEmail = new Label();
            txtClientEmail = new TextBox();
            lblProjectTitle = new Label();
            txtProjectTitle = new TextBox();
            lblSiteLocation = new Label();
            txtSiteLocation = new TextBox();
            lblSector = new Label();
            cboSector = new ComboBox();
            lblQuoteDate = new Label();
            dtpQuoteDate = new DateTimePicker();
            lblValidUntil = new Label();
            dtpValidUntil = new DateTimePicker();
            lblPlannedStart = new Label();
            dtpPlannedStart = new DateTimePicker();
            lblFromInquiry = new Label();
            cboFromInquiry = new ComboBox();
            lblLines = new Label();
            dgvLines = new DataGridView();
            btnRemoveLine = new Button();
            lblDiscount = new Label();
            numDiscount = new NumericUpDown();
            chkVat = new CheckBox();
            lblSubtotal = new Label();
            lblDiscountShown = new Label();
            lblVatShown = new Label();
            lblTotal = new Label();
            lblTerms = new Label();
            txtTerms = new TextBox();
            lblNotes = new Label();
            txtNotes = new TextBox();
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
            colItem = new DataGridViewTextBoxColumn();
            colQty = new DataGridViewTextBoxColumn();
            colUnit = new DataGridViewTextBoxColumn();
            colUnitPrice = new DataGridViewTextBoxColumn();
            colAmount = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvQuotations).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDiscount).BeginInit();
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
            lblClientName.Text = "Client name";
            // 
            // txtClientName
            // 
            txtClientName.Location = new Point(38, 423);
            txtClientName.Name = "txtClientName";
            txtClientName.Size = new Size(322, 23);
            txtClientName.MaxLength = 150;
            txtClientName.TabIndex = 7;
            // 
            // lblClientContact
            // 
            lblClientContact.AutoSize = true;
            lblClientContact.Location = new Point(38, 462);
            lblClientContact.Name = "lblClientContact";
            lblClientContact.Text = "Contact number";
            // 
            // txtClientContact
            // 
            txtClientContact.Location = new Point(38, 486);
            txtClientContact.Name = "txtClientContact";
            txtClientContact.Size = new Size(322, 23);
            txtClientContact.MaxLength = 60;
            txtClientContact.TabIndex = 8;
            // 
            // lblClientEmail
            // 
            lblClientEmail.AutoSize = true;
            lblClientEmail.Location = new Point(38, 526);
            lblClientEmail.Name = "lblClientEmail";
            lblClientEmail.Text = "Email";
            // 
            // txtClientEmail
            // 
            txtClientEmail.Location = new Point(38, 549);
            txtClientEmail.Name = "txtClientEmail";
            txtClientEmail.Size = new Size(322, 23);
            txtClientEmail.MaxLength = 150;
            txtClientEmail.TabIndex = 9;
            // 
            // lblProjectTitle
            // 
            lblProjectTitle.AutoSize = true;
            lblProjectTitle.Location = new Point(380, 399);
            lblProjectTitle.Name = "lblProjectTitle";
            lblProjectTitle.Text = "Project (e.g. Two-storey house)";
            lblProjectTitle.Font = new Font("Segoe UI", 9F);
            // 
            // txtProjectTitle
            // 
            txtProjectTitle.Location = new Point(380, 423);
            txtProjectTitle.Name = "txtProjectTitle";
            txtProjectTitle.Size = new Size(322, 23);
            txtProjectTitle.MaxLength = 150;
            txtProjectTitle.TabIndex = 10;
            // 
            // lblSiteLocation
            // 
            lblSiteLocation.AutoSize = true;
            lblSiteLocation.Location = new Point(380, 462);
            lblSiteLocation.Name = "lblSiteLocation";
            lblSiteLocation.Text = "Site location";
            // 
            // txtSiteLocation
            // 
            txtSiteLocation.Location = new Point(380, 486);
            txtSiteLocation.Name = "txtSiteLocation";
            txtSiteLocation.Size = new Size(322, 23);
            txtSiteLocation.MaxLength = 255;
            txtSiteLocation.TabIndex = 11;
            // 
            // lblSector
            // 
            lblSector.AutoSize = true;
            lblSector.Location = new Point(380, 526);
            lblSector.Name = "lblSector";
            lblSector.Text = "Sector";
            // 
            // cboSector
            // 
            cboSector.Location = new Point(380, 549);
            cboSector.Name = "cboSector";
            cboSector.Size = new Size(322, 23);
            cboSector.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSector.TabIndex = 12;
            // 
            // lblQuoteDate
            // 
            lblQuoteDate.AutoSize = true;
            lblQuoteDate.Location = new Point(723, 399);
            lblQuoteDate.Name = "lblQuoteDate";
            lblQuoteDate.Text = "Quotation date";
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
            lblValidUntil.Text = "Valid until";
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
            // lblFromInquiry
            // 
            lblFromInquiry.AutoSize = true;
            lblFromInquiry.Location = new Point(900, 399);
            lblFromInquiry.Name = "lblFromInquiry";
            lblFromInquiry.Text = "From a website inquiry";
            // 
            // cboFromInquiry
            // 
            cboFromInquiry.Location = new Point(900, 423);
            cboFromInquiry.Name = "cboFromInquiry";
            cboFromInquiry.Size = new Size(207, 23);
            cboFromInquiry.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFromInquiry.TabIndex = 16;
            cboFromInquiry.SelectionChangeCommitted += cboFromInquiry_SelectionChangeCommitted;
            // 
            // lblLines
            // 
            lblLines.AutoSize = true;
            lblLines.Location = new Point(38, 597);
            lblLines.Name = "lblLines";
            lblLines.Text = "Items (type in the table; a new row appears as you type)";
            // 
            // dgvLines
            // 
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.Columns.AddRange(new DataGridViewColumn[] { colItem, colQty, colUnit, colUnitPrice, colAmount });
            dgvLines.MultiSelect = false;
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Location = new Point(38, 621);
            dgvLines.Name = "dgvLines";
            dgvLines.Size = new Size(713, 211);
            dgvLines.TabIndex = 17;
            dgvLines.CellEndEdit += Lines_Changed;
            dgvLines.RowsRemoved += Lines_RowsRemoved;
            // 
            // btnRemoveLine
            // 
            btnRemoveLine.Location = new Point(38, 840);
            btnRemoveLine.Name = "btnRemoveLine";
            btnRemoveLine.Size = new Size(126, 36);
            btnRemoveLine.Text = "Remove Line";
            btnRemoveLine.TabIndex = 18;
            btnRemoveLine.Click += btnRemoveLine_Click;
            // 
            // lblDiscount
            // 
            lblDiscount.AutoSize = true;
            lblDiscount.Location = new Point(771, 621);
            lblDiscount.Name = "lblDiscount";
            lblDiscount.Text = "Discount (pesos)";
            // 
            // numDiscount
            // 
            numDiscount.Location = new Point(771, 644);
            numDiscount.Name = "numDiscount";
            numDiscount.Size = new Size(161, 23);
            numDiscount.DecimalPlaces = 2;
            numDiscount.ThousandsSeparator = true;
            numDiscount.Maximum = 999999999m;
            numDiscount.TabIndex = 19;
            numDiscount.ValueChanged += Totals_Changed;
            // 
            // chkVat
            // 
            chkVat.AutoSize = true;
            chkVat.Location = new Point(771, 684);
            chkVat.Name = "chkVat";
            chkVat.Text = "Add 12% VAT";
            chkVat.TabIndex = 20;
            chkVat.CheckedChanged += Totals_Changed;
            // 
            // lblSubtotal
            // 
            lblSubtotal.AutoSize = true;
            lblSubtotal.Location = new Point(771, 724);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Text = "Subtotal: -";
            // 
            // lblDiscountShown
            // 
            lblDiscountShown.AutoSize = true;
            lblDiscountShown.Location = new Point(771, 750);
            lblDiscountShown.Name = "lblDiscountShown";
            lblDiscountShown.Text = "Discount: -";
            // 
            // lblVatShown
            // 
            lblVatShown.AutoSize = true;
            lblVatShown.Location = new Point(771, 776);
            lblVatShown.Name = "lblVatShown";
            lblVatShown.Text = "VAT: -";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(771, 808);
            lblTotal.Name = "lblTotal";
            lblTotal.Text = "TOTAL: -";
            lblTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            // 
            // lblTerms
            // 
            lblTerms.AutoSize = true;
            lblTerms.Location = new Point(38, 887);
            lblTerms.Name = "lblTerms";
            lblTerms.Text = "Terms and conditions (printed)";
            // 
            // txtTerms
            // 
            txtTerms.Location = new Point(38, 911);
            txtTerms.Name = "txtTerms";
            txtTerms.Size = new Size(529, 106);
            txtTerms.Multiline = true;
            txtTerms.ScrollBars = ScrollBars.Vertical;
            txtTerms.MaxLength = 2000;
            txtTerms.TabIndex = 21;
            // 
            // lblNotes
            // 
            lblNotes.AutoSize = true;
            lblNotes.Location = new Point(588, 887);
            lblNotes.Name = "lblNotes";
            lblNotes.Text = "Internal notes (not printed)";
            // 
            // txtNotes
            // 
            txtNotes.Location = new Point(588, 911);
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(520, 106);
            txtNotes.Multiline = true;
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.MaxLength = 1000;
            txtNotes.TabIndex = 22;
            // 
            // lblQuotationMsg
            // 
            lblQuotationMsg.AutoSize = true;
            lblQuotationMsg.Location = new Point(38, 1027);
            lblQuotationMsg.Name = "lblQuotationMsg";
            lblQuotationMsg.MaximumSize = new Size(930, 0);
            // 
            // btnSaveQuotation
            // 
            btnSaveQuotation.Location = new Point(38, 1059);
            btnSaveQuotation.Name = "btnSaveQuotation";
            btnSaveQuotation.Size = new Size(103, 36);
            btnSaveQuotation.Text = "Save";
            btnSaveQuotation.TabIndex = 23;
            btnSaveQuotation.Click += btnSaveQuotation_Click;
            // 
            // btnCancelQuotation
            // 
            btnCancelQuotation.Location = new Point(148, 1059);
            btnCancelQuotation.Name = "btnCancelQuotation";
            btnCancelQuotation.Size = new Size(103, 36);
            btnCancelQuotation.Text = "Cancel";
            btnCancelQuotation.TabIndex = 24;
            btnCancelQuotation.Click += btnCancelQuotation_Click;
            // 
            // btnSubmitQuotation
            // 
            btnSubmitQuotation.Location = new Point(259, 1059);
            btnSubmitQuotation.Name = "btnSubmitQuotation";
            btnSubmitQuotation.Size = new Size(172, 36);
            btnSubmitQuotation.Text = "Send for Approval";
            btnSubmitQuotation.TabIndex = 25;
            btnSubmitQuotation.Click += btnSubmitQuotation_Click;
            // 
            // btnApproveQuotation
            // 
            btnApproveQuotation.Location = new Point(438, 1059);
            btnApproveQuotation.Name = "btnApproveQuotation";
            btnApproveQuotation.Size = new Size(103, 36);
            btnApproveQuotation.Text = "Approve";
            btnApproveQuotation.TabIndex = 26;
            btnApproveQuotation.Click += btnApproveQuotation_Click;
            // 
            // btnRejectQuotation
            // 
            btnRejectQuotation.Location = new Point(548, 1059);
            btnRejectQuotation.Name = "btnRejectQuotation";
            btnRejectQuotation.Size = new Size(103, 36);
            btnRejectQuotation.Text = "Reject";
            btnRejectQuotation.TabIndex = 27;
            btnRejectQuotation.Click += btnRejectQuotation_Click;
            // 
            // btnAcceptQuotation
            // 
            btnAcceptQuotation.Location = new Point(659, 1059);
            btnAcceptQuotation.Name = "btnAcceptQuotation";
            btnAcceptQuotation.Size = new Size(207, 36);
            btnAcceptQuotation.Text = "Client Accepted: Make Project";
            btnAcceptQuotation.TabIndex = 28;
            btnAcceptQuotation.Click += btnAcceptQuotation_Click;
            // 
            // btnDeclineQuotation
            // 
            btnDeclineQuotation.Location = new Point(873, 1059);
            btnDeclineQuotation.Name = "btnDeclineQuotation";
            btnDeclineQuotation.Size = new Size(138, 36);
            btnDeclineQuotation.Text = "Client Declined";
            btnDeclineQuotation.TabIndex = 29;
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
            // colItem
            // 
            colItem.DataPropertyName = "Description";
            colItem.HeaderText = "Item / Description";
            colItem.Name = "colItem";
            colItem.FillWeight = 220;
            // 
            // colQty
            // 
            colQty.DataPropertyName = "Quantity";
            colQty.HeaderText = "Qty";
            colQty.Name = "colQty";
            colQty.FillWeight = 45;
            colQty.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight };
            // 
            // colUnit
            // 
            colUnit.DataPropertyName = "Unit";
            colUnit.HeaderText = "Unit";
            colUnit.Name = "colUnit";
            colUnit.FillWeight = 45;
            // 
            // colUnitPrice
            // 
            colUnitPrice.DataPropertyName = "UnitPrice";
            colUnitPrice.HeaderText = "Unit Price";
            colUnitPrice.Name = "colUnitPrice";
            colUnitPrice.FillWeight = 70;
            colUnitPrice.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" };
            // 
            // colAmount
            // 
            colAmount.DataPropertyName = "Amount";
            colAmount.HeaderText = "Amount";
            colAmount.Name = "colAmount";
            colAmount.FillWeight = 75;
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
            Controls.Add(lblClientContact);
            Controls.Add(txtClientContact);
            Controls.Add(lblClientEmail);
            Controls.Add(txtClientEmail);
            Controls.Add(lblProjectTitle);
            Controls.Add(txtProjectTitle);
            Controls.Add(lblSiteLocation);
            Controls.Add(txtSiteLocation);
            Controls.Add(lblSector);
            Controls.Add(cboSector);
            Controls.Add(lblQuoteDate);
            Controls.Add(dtpQuoteDate);
            Controls.Add(lblValidUntil);
            Controls.Add(dtpValidUntil);
            Controls.Add(lblPlannedStart);
            Controls.Add(dtpPlannedStart);
            Controls.Add(lblFromInquiry);
            Controls.Add(cboFromInquiry);
            Controls.Add(lblLines);
            Controls.Add(dgvLines);
            Controls.Add(btnRemoveLine);
            Controls.Add(lblDiscount);
            Controls.Add(numDiscount);
            Controls.Add(chkVat);
            Controls.Add(lblSubtotal);
            Controls.Add(lblDiscountShown);
            Controls.Add(lblVatShown);
            Controls.Add(lblTotal);
            Controls.Add(lblTerms);
            Controls.Add(txtTerms);
            Controls.Add(lblNotes);
            Controls.Add(txtNotes);
            Controls.Add(lblQuotationMsg);
            Controls.Add(btnSaveQuotation);
            Controls.Add(btnCancelQuotation);
            Controls.Add(btnSubmitQuotation);
            Controls.Add(btnApproveQuotation);
            Controls.Add(btnRejectQuotation);
            Controls.Add(btnAcceptQuotation);
            Controls.Add(btnDeclineQuotation);
            Name = "QuotationsPage";
            Size = new Size(1152, 1141);
            Load += QuotationsPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvQuotations).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDiscount).EndInit();
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
        private Label lblClientContact;
        private TextBox txtClientContact;
        private Label lblClientEmail;
        private TextBox txtClientEmail;
        private Label lblProjectTitle;
        private TextBox txtProjectTitle;
        private Label lblSiteLocation;
        private TextBox txtSiteLocation;
        private Label lblSector;
        private ComboBox cboSector;
        private Label lblQuoteDate;
        private DateTimePicker dtpQuoteDate;
        private Label lblValidUntil;
        private DateTimePicker dtpValidUntil;
        private Label lblPlannedStart;
        private DateTimePicker dtpPlannedStart;
        private Label lblFromInquiry;
        private ComboBox cboFromInquiry;
        private Label lblLines;
        private DataGridView dgvLines;
        private Button btnRemoveLine;
        private Label lblDiscount;
        private NumericUpDown numDiscount;
        private CheckBox chkVat;
        private Label lblSubtotal;
        private Label lblDiscountShown;
        private Label lblVatShown;
        private Label lblTotal;
        private Label lblTerms;
        private TextBox txtTerms;
        private Label lblNotes;
        private TextBox txtNotes;
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
        private DataGridViewTextBoxColumn colItem;
        private DataGridViewTextBoxColumn colQty;
        private DataGridViewTextBoxColumn colUnit;
        private DataGridViewTextBoxColumn colUnitPrice;
        private DataGridViewTextBoxColumn colAmount;
    }
}
