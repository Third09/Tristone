namespace Company.OfficeApp.Pages.Subcontractors
{
    partial class SubDirectoryTab
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
            txtSubSearch = new TextBox();
            lblTradeFilter = new Label();
            cboSubTradeFilter = new ComboBox();
            lblPcabDue = new Label();
            dgvSubcontractors = new DataGridView();
            btnAddSubcontractor = new Button();
            btnDeleteSubcontractor = new Button();
            btnExportSubcontractors = new Button();
            lblFormMode = new Label();
            lblSubBusinessName = new Label();
            txtSubBusinessName = new TextBox();
            lblSubTradeName = new Label();
            txtSubTradeName = new TextBox();
            lblSubDtiSec = new Label();
            txtSubDtiSec = new TextBox();
            lblSubTin = new Label();
            txtSubTin = new TextBox();
            lblSubOfficeAddress = new Label();
            txtSubOfficeAddress = new TextBox();
            lblSubYardAddress = new Label();
            txtSubYardAddress = new TextBox();
            lblSubBankName = new Label();
            txtSubBankName = new TextBox();
            lblSubAccountName = new Label();
            txtSubAccountName = new TextBox();
            lblSubAccountNo = new Label();
            txtSubAccountNo = new TextBox();
            lblPrimaryName = new Label();
            txtPrimaryName = new TextBox();
            lblPrimaryRole = new Label();
            txtPrimaryRole = new TextBox();
            lblPrimaryPhone = new Label();
            txtPrimaryPhone = new TextBox();
            lblPrimaryMobile = new Label();
            txtPrimaryMobile = new TextBox();
            lblPrimaryEmail = new Label();
            txtPrimaryEmail = new TextBox();
            lblSecondaryName = new Label();
            txtSecondaryName = new TextBox();
            lblSecondaryRole = new Label();
            txtSecondaryRole = new TextBox();
            lblSecondaryPhone = new Label();
            txtSecondaryPhone = new TextBox();
            lblSecondaryMobile = new Label();
            txtSecondaryMobile = new TextBox();
            lblSecondaryEmail = new Label();
            txtSecondaryEmail = new TextBox();
            lblSubTrades = new Label();
            clbSubTrades = new CheckedListBox();
            lblPcabNumber = new Label();
            txtPcabNumber = new TextBox();
            lblPcabClassification = new Label();
            txtPcabClassification = new TextBox();
            lblPcabExpiry = new Label();
            dtpPcabExpiry = new DateTimePicker();
            lblSubMsg = new Label();
            btnSaveSubcontractor = new Button();
            btnCancelSubcontractor = new Button();
            colBusinessName = new DataGridViewTextBoxColumn();
            colTrades = new DataGridViewTextBoxColumn();
            colPrimaryContact = new DataGridViewTextBoxColumn();
            colPhone = new DataGridViewTextBoxColumn();
            colPcabExpiry = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvSubcontractors).BeginInit();
            SuspendLayout();
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(12, 15);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search";
            // 
            // txtSubSearch
            // 
            txtSubSearch.Location = new Point(62, 12);
            txtSubSearch.Name = "txtSubSearch";
            txtSubSearch.Size = new Size(180, 23);
            txtSubSearch.TabIndex = 0;
            // 
            // lblTradeFilter
            // 
            lblTradeFilter.AutoSize = true;
            lblTradeFilter.Location = new Point(254, 15);
            lblTradeFilter.Name = "lblTradeFilter";
            lblTradeFilter.Text = "Trade";
            // 
            // cboSubTradeFilter
            // 
            cboSubTradeFilter.Location = new Point(294, 12);
            cboSubTradeFilter.Name = "cboSubTradeFilter";
            cboSubTradeFilter.Size = new Size(200, 23);
            cboSubTradeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSubTradeFilter.TabIndex = 1;
            cboSubTradeFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblPcabDue
            // 
            lblPcabDue.AutoSize = true;
            lblPcabDue.Location = new Point(506, 15);
            lblPcabDue.Name = "lblPcabDue";
            lblPcabDue.Text = "";
            // 
            // dgvSubcontractors
            // 
            dgvSubcontractors.AllowUserToAddRows = false;
            dgvSubcontractors.AllowUserToDeleteRows = false;
            dgvSubcontractors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSubcontractors.Columns.AddRange(new DataGridViewColumn[] { colBusinessName, colTrades, colPrimaryContact, colPhone, colPcabExpiry });
            dgvSubcontractors.MultiSelect = false;
            dgvSubcontractors.ReadOnly = true;
            dgvSubcontractors.RowHeadersVisible = false;
            dgvSubcontractors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSubcontractors.Location = new Point(12, 44);
            dgvSubcontractors.Name = "dgvSubcontractors";
            dgvSubcontractors.Size = new Size(930, 170);
            dgvSubcontractors.TabIndex = 2;
            dgvSubcontractors.SelectionChanged += dgvSubcontractors_SelectionChanged;
            dgvSubcontractors.CellFormatting += dgvSubcontractors_CellFormatting;
            // 
            // btnAddSubcontractor
            // 
            btnAddSubcontractor.Location = new Point(12, 222);
            btnAddSubcontractor.Name = "btnAddSubcontractor";
            btnAddSubcontractor.Size = new Size(140, 28);
            btnAddSubcontractor.Text = "Add Subcontractor";
            btnAddSubcontractor.TabIndex = 3;
            btnAddSubcontractor.Click += btnAddSubcontractor_Click;
            // 
            // btnDeleteSubcontractor
            // 
            btnDeleteSubcontractor.Location = new Point(158, 222);
            btnDeleteSubcontractor.Name = "btnDeleteSubcontractor";
            btnDeleteSubcontractor.Size = new Size(90, 28);
            btnDeleteSubcontractor.Text = "Delete";
            btnDeleteSubcontractor.TabIndex = 4;
            btnDeleteSubcontractor.Click += btnDeleteSubcontractor_Click;
            // 
            // btnExportSubcontractors
            // 
            btnExportSubcontractors.Location = new Point(254, 222);
            btnExportSubcontractors.Name = "btnExportSubcontractors";
            btnExportSubcontractors.Size = new Size(90, 28);
            btnExportSubcontractors.Text = "Export";
            btnExportSubcontractors.TabIndex = 5;
            btnExportSubcontractors.Click += btnExportSubcontractors_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(12, 264);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New subcontractor";
            // 
            // lblSubBusinessName
            // 
            lblSubBusinessName.AutoSize = true;
            lblSubBusinessName.Location = new Point(12, 288);
            lblSubBusinessName.Name = "lblSubBusinessName";
            lblSubBusinessName.Text = "Official business name";
            // 
            // txtSubBusinessName
            // 
            txtSubBusinessName.Location = new Point(12, 306);
            txtSubBusinessName.Name = "txtSubBusinessName";
            txtSubBusinessName.Size = new Size(260, 23);
            txtSubBusinessName.TabIndex = 6;
            // 
            // lblSubTradeName
            // 
            lblSubTradeName.AutoSize = true;
            lblSubTradeName.Location = new Point(12, 336);
            lblSubTradeName.Name = "lblSubTradeName";
            lblSubTradeName.Text = "Trade name (DBA)";
            // 
            // txtSubTradeName
            // 
            txtSubTradeName.Location = new Point(12, 354);
            txtSubTradeName.Name = "txtSubTradeName";
            txtSubTradeName.Size = new Size(260, 23);
            txtSubTradeName.TabIndex = 7;
            // 
            // lblSubDtiSec
            // 
            lblSubDtiSec.AutoSize = true;
            lblSubDtiSec.Location = new Point(12, 384);
            lblSubDtiSec.Name = "lblSubDtiSec";
            lblSubDtiSec.Text = "DTI / SEC reg. no.";
            // 
            // txtSubDtiSec
            // 
            txtSubDtiSec.Location = new Point(12, 402);
            txtSubDtiSec.Name = "txtSubDtiSec";
            txtSubDtiSec.Size = new Size(125, 23);
            txtSubDtiSec.TabIndex = 8;
            // 
            // lblSubTin
            // 
            lblSubTin.AutoSize = true;
            lblSubTin.Location = new Point(147, 384);
            lblSubTin.Name = "lblSubTin";
            lblSubTin.Text = "TIN";
            // 
            // txtSubTin
            // 
            txtSubTin.Location = new Point(147, 402);
            txtSubTin.Name = "txtSubTin";
            txtSubTin.Size = new Size(125, 23);
            txtSubTin.TabIndex = 9;
            // 
            // lblSubOfficeAddress
            // 
            lblSubOfficeAddress.AutoSize = true;
            lblSubOfficeAddress.Location = new Point(12, 432);
            lblSubOfficeAddress.Name = "lblSubOfficeAddress";
            lblSubOfficeAddress.Text = "Main office address";
            // 
            // txtSubOfficeAddress
            // 
            txtSubOfficeAddress.Location = new Point(12, 450);
            txtSubOfficeAddress.Name = "txtSubOfficeAddress";
            txtSubOfficeAddress.Size = new Size(260, 38);
            txtSubOfficeAddress.Multiline = true;
            txtSubOfficeAddress.ScrollBars = ScrollBars.Vertical;
            txtSubOfficeAddress.TabIndex = 10;
            // 
            // lblSubYardAddress
            // 
            lblSubYardAddress.AutoSize = true;
            lblSubYardAddress.Location = new Point(12, 494);
            lblSubYardAddress.Name = "lblSubYardAddress";
            lblSubYardAddress.Text = "Warehouse / yard location";
            // 
            // txtSubYardAddress
            // 
            txtSubYardAddress.Location = new Point(12, 512);
            txtSubYardAddress.Name = "txtSubYardAddress";
            txtSubYardAddress.Size = new Size(260, 38);
            txtSubYardAddress.Multiline = true;
            txtSubYardAddress.ScrollBars = ScrollBars.Vertical;
            txtSubYardAddress.TabIndex = 11;
            // 
            // lblSubBankName
            // 
            lblSubBankName.AutoSize = true;
            lblSubBankName.Location = new Point(12, 556);
            lblSubBankName.Name = "lblSubBankName";
            lblSubBankName.Text = "Bank name";
            // 
            // txtSubBankName
            // 
            txtSubBankName.Location = new Point(12, 574);
            txtSubBankName.Name = "txtSubBankName";
            txtSubBankName.Size = new Size(125, 23);
            txtSubBankName.TabIndex = 12;
            // 
            // lblSubAccountName
            // 
            lblSubAccountName.AutoSize = true;
            lblSubAccountName.Location = new Point(147, 556);
            lblSubAccountName.Name = "lblSubAccountName";
            lblSubAccountName.Text = "Account name";
            // 
            // txtSubAccountName
            // 
            txtSubAccountName.Location = new Point(147, 574);
            txtSubAccountName.Name = "txtSubAccountName";
            txtSubAccountName.Size = new Size(125, 23);
            txtSubAccountName.TabIndex = 13;
            // 
            // lblSubAccountNo
            // 
            lblSubAccountNo.AutoSize = true;
            lblSubAccountNo.Location = new Point(12, 604);
            lblSubAccountNo.Name = "lblSubAccountNo";
            lblSubAccountNo.Text = "Account number";
            // 
            // txtSubAccountNo
            // 
            txtSubAccountNo.Location = new Point(12, 622);
            txtSubAccountNo.Name = "txtSubAccountNo";
            txtSubAccountNo.Size = new Size(260, 23);
            txtSubAccountNo.TabIndex = 14;
            // 
            // lblPrimaryName
            // 
            lblPrimaryName.AutoSize = true;
            lblPrimaryName.Location = new Point(300, 288);
            lblPrimaryName.Name = "lblPrimaryName";
            lblPrimaryName.Text = "Primary contact: name";
            // 
            // txtPrimaryName
            // 
            txtPrimaryName.Location = new Point(300, 306);
            txtPrimaryName.Name = "txtPrimaryName";
            txtPrimaryName.Size = new Size(145, 23);
            txtPrimaryName.TabIndex = 15;
            // 
            // lblPrimaryRole
            // 
            lblPrimaryRole.AutoSize = true;
            lblPrimaryRole.Location = new Point(455, 288);
            lblPrimaryRole.Name = "lblPrimaryRole";
            lblPrimaryRole.Text = "Designation / role";
            // 
            // txtPrimaryRole
            // 
            txtPrimaryRole.Location = new Point(455, 306);
            txtPrimaryRole.Name = "txtPrimaryRole";
            txtPrimaryRole.Size = new Size(145, 23);
            txtPrimaryRole.TabIndex = 16;
            // 
            // lblPrimaryPhone
            // 
            lblPrimaryPhone.AutoSize = true;
            lblPrimaryPhone.Location = new Point(300, 336);
            lblPrimaryPhone.Name = "lblPrimaryPhone";
            lblPrimaryPhone.Text = "Phone";
            // 
            // txtPrimaryPhone
            // 
            txtPrimaryPhone.Location = new Point(300, 354);
            txtPrimaryPhone.Name = "txtPrimaryPhone";
            txtPrimaryPhone.Size = new Size(145, 23);
            txtPrimaryPhone.TabIndex = 17;
            // 
            // lblPrimaryMobile
            // 
            lblPrimaryMobile.AutoSize = true;
            lblPrimaryMobile.Location = new Point(455, 336);
            lblPrimaryMobile.Name = "lblPrimaryMobile";
            lblPrimaryMobile.Text = "Mobile";
            // 
            // txtPrimaryMobile
            // 
            txtPrimaryMobile.Location = new Point(455, 354);
            txtPrimaryMobile.Name = "txtPrimaryMobile";
            txtPrimaryMobile.Size = new Size(145, 23);
            txtPrimaryMobile.TabIndex = 18;
            // 
            // lblPrimaryEmail
            // 
            lblPrimaryEmail.AutoSize = true;
            lblPrimaryEmail.Location = new Point(300, 384);
            lblPrimaryEmail.Name = "lblPrimaryEmail";
            lblPrimaryEmail.Text = "Email";
            // 
            // txtPrimaryEmail
            // 
            txtPrimaryEmail.Location = new Point(300, 402);
            txtPrimaryEmail.Name = "txtPrimaryEmail";
            txtPrimaryEmail.Size = new Size(300, 23);
            txtPrimaryEmail.TabIndex = 19;
            // 
            // lblSecondaryName
            // 
            lblSecondaryName.AutoSize = true;
            lblSecondaryName.Location = new Point(300, 432);
            lblSecondaryName.Name = "lblSecondaryName";
            lblSecondaryName.Text = "Backup contact: name";
            // 
            // txtSecondaryName
            // 
            txtSecondaryName.Location = new Point(300, 450);
            txtSecondaryName.Name = "txtSecondaryName";
            txtSecondaryName.Size = new Size(145, 23);
            txtSecondaryName.TabIndex = 20;
            // 
            // lblSecondaryRole
            // 
            lblSecondaryRole.AutoSize = true;
            lblSecondaryRole.Location = new Point(455, 432);
            lblSecondaryRole.Name = "lblSecondaryRole";
            lblSecondaryRole.Text = "Role";
            // 
            // txtSecondaryRole
            // 
            txtSecondaryRole.Location = new Point(455, 450);
            txtSecondaryRole.Name = "txtSecondaryRole";
            txtSecondaryRole.Size = new Size(145, 23);
            txtSecondaryRole.TabIndex = 21;
            // 
            // lblSecondaryPhone
            // 
            lblSecondaryPhone.AutoSize = true;
            lblSecondaryPhone.Location = new Point(300, 480);
            lblSecondaryPhone.Name = "lblSecondaryPhone";
            lblSecondaryPhone.Text = "Phone";
            // 
            // txtSecondaryPhone
            // 
            txtSecondaryPhone.Location = new Point(300, 498);
            txtSecondaryPhone.Name = "txtSecondaryPhone";
            txtSecondaryPhone.Size = new Size(145, 23);
            txtSecondaryPhone.TabIndex = 22;
            // 
            // lblSecondaryMobile
            // 
            lblSecondaryMobile.AutoSize = true;
            lblSecondaryMobile.Location = new Point(455, 480);
            lblSecondaryMobile.Name = "lblSecondaryMobile";
            lblSecondaryMobile.Text = "Mobile";
            // 
            // txtSecondaryMobile
            // 
            txtSecondaryMobile.Location = new Point(455, 498);
            txtSecondaryMobile.Name = "txtSecondaryMobile";
            txtSecondaryMobile.Size = new Size(145, 23);
            txtSecondaryMobile.TabIndex = 23;
            // 
            // lblSecondaryEmail
            // 
            lblSecondaryEmail.AutoSize = true;
            lblSecondaryEmail.Location = new Point(300, 528);
            lblSecondaryEmail.Name = "lblSecondaryEmail";
            lblSecondaryEmail.Text = "Email";
            // 
            // txtSecondaryEmail
            // 
            txtSecondaryEmail.Location = new Point(300, 546);
            txtSecondaryEmail.Name = "txtSecondaryEmail";
            txtSecondaryEmail.Size = new Size(300, 23);
            txtSecondaryEmail.TabIndex = 24;
            // 
            // lblSubTrades
            // 
            lblSubTrades.AutoSize = true;
            lblSubTrades.Location = new Point(628, 288);
            lblSubTrades.Name = "lblSubTrades";
            lblSubTrades.Text = "Trade categories (tick all that apply)";
            // 
            // clbSubTrades
            // 
            clbSubTrades.Location = new Point(628, 306);
            clbSubTrades.Name = "clbSubTrades";
            clbSubTrades.Size = new Size(300, 150);
            clbSubTrades.CheckOnClick = true;
            clbSubTrades.TabIndex = 25;
            // 
            // lblPcabNumber
            // 
            lblPcabNumber.AutoSize = true;
            lblPcabNumber.Location = new Point(628, 466);
            lblPcabNumber.Name = "lblPcabNumber";
            lblPcabNumber.Text = "PCAB license no.";
            // 
            // txtPcabNumber
            // 
            txtPcabNumber.Location = new Point(628, 484);
            txtPcabNumber.Name = "txtPcabNumber";
            txtPcabNumber.Size = new Size(145, 23);
            txtPcabNumber.TabIndex = 26;
            // 
            // lblPcabClassification
            // 
            lblPcabClassification.AutoSize = true;
            lblPcabClassification.Location = new Point(783, 466);
            lblPcabClassification.Name = "lblPcabClassification";
            lblPcabClassification.Text = "Classification";
            // 
            // txtPcabClassification
            // 
            txtPcabClassification.Location = new Point(783, 484);
            txtPcabClassification.Name = "txtPcabClassification";
            txtPcabClassification.Size = new Size(145, 23);
            txtPcabClassification.TabIndex = 27;
            // 
            // lblPcabExpiry
            // 
            lblPcabExpiry.AutoSize = true;
            lblPcabExpiry.Location = new Point(628, 514);
            lblPcabExpiry.Name = "lblPcabExpiry";
            lblPcabExpiry.Text = "PCAB expiry (tick to set)";
            // 
            // dtpPcabExpiry
            // 
            dtpPcabExpiry.Location = new Point(628, 532);
            dtpPcabExpiry.Name = "dtpPcabExpiry";
            dtpPcabExpiry.Size = new Size(140, 23);
            dtpPcabExpiry.Format = DateTimePickerFormat.Short;
            dtpPcabExpiry.ShowCheckBox = true;
            dtpPcabExpiry.TabIndex = 28;
            // 
            // lblSubMsg
            // 
            lblSubMsg.AutoSize = true;
            lblSubMsg.Location = new Point(12, 656);
            lblSubMsg.Name = "lblSubMsg";
            // 
            // btnSaveSubcontractor
            // 
            btnSaveSubcontractor.Location = new Point(12, 678);
            btnSaveSubcontractor.Name = "btnSaveSubcontractor";
            btnSaveSubcontractor.Size = new Size(90, 28);
            btnSaveSubcontractor.Text = "Save";
            btnSaveSubcontractor.TabIndex = 29;
            btnSaveSubcontractor.Click += btnSaveSubcontractor_Click;
            // 
            // btnCancelSubcontractor
            // 
            btnCancelSubcontractor.Location = new Point(108, 678);
            btnCancelSubcontractor.Name = "btnCancelSubcontractor";
            btnCancelSubcontractor.Size = new Size(90, 28);
            btnCancelSubcontractor.Text = "Cancel";
            btnCancelSubcontractor.TabIndex = 30;
            btnCancelSubcontractor.Click += btnCancelSubcontractor_Click;
            // 
            // colBusinessName
            // 
            colBusinessName.DataPropertyName = "BusinessName";
            colBusinessName.HeaderText = "Business Name";
            colBusinessName.Name = "colBusinessName";
            // 
            // colTrades
            // 
            colTrades.DataPropertyName = "Trades";
            colTrades.HeaderText = "Trades";
            colTrades.Name = "colTrades";
            // 
            // colPrimaryContact
            // 
            colPrimaryContact.DataPropertyName = "PrimaryContact";
            colPrimaryContact.HeaderText = "Primary Contact";
            colPrimaryContact.Name = "colPrimaryContact";
            // 
            // colPhone
            // 
            colPhone.DataPropertyName = "Phone";
            colPhone.HeaderText = "Phone";
            colPhone.Name = "colPhone";
            // 
            // colPcabExpiry
            // 
            colPcabExpiry.DataPropertyName = "PcabExpiry";
            colPcabExpiry.HeaderText = "PCAB Expiry";
            colPcabExpiry.Name = "colPcabExpiry";
            colPcabExpiry.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy", NullValue = "-" };
            // 
            // SubDirectoryTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblSearch);
            Controls.Add(txtSubSearch);
            Controls.Add(lblTradeFilter);
            Controls.Add(cboSubTradeFilter);
            Controls.Add(lblPcabDue);
            Controls.Add(dgvSubcontractors);
            Controls.Add(btnAddSubcontractor);
            Controls.Add(btnDeleteSubcontractor);
            Controls.Add(btnExportSubcontractors);
            Controls.Add(lblFormMode);
            Controls.Add(lblSubBusinessName);
            Controls.Add(txtSubBusinessName);
            Controls.Add(lblSubTradeName);
            Controls.Add(txtSubTradeName);
            Controls.Add(lblSubDtiSec);
            Controls.Add(txtSubDtiSec);
            Controls.Add(lblSubTin);
            Controls.Add(txtSubTin);
            Controls.Add(lblSubOfficeAddress);
            Controls.Add(txtSubOfficeAddress);
            Controls.Add(lblSubYardAddress);
            Controls.Add(txtSubYardAddress);
            Controls.Add(lblSubBankName);
            Controls.Add(txtSubBankName);
            Controls.Add(lblSubAccountName);
            Controls.Add(txtSubAccountName);
            Controls.Add(lblSubAccountNo);
            Controls.Add(txtSubAccountNo);
            Controls.Add(lblPrimaryName);
            Controls.Add(txtPrimaryName);
            Controls.Add(lblPrimaryRole);
            Controls.Add(txtPrimaryRole);
            Controls.Add(lblPrimaryPhone);
            Controls.Add(txtPrimaryPhone);
            Controls.Add(lblPrimaryMobile);
            Controls.Add(txtPrimaryMobile);
            Controls.Add(lblPrimaryEmail);
            Controls.Add(txtPrimaryEmail);
            Controls.Add(lblSecondaryName);
            Controls.Add(txtSecondaryName);
            Controls.Add(lblSecondaryRole);
            Controls.Add(txtSecondaryRole);
            Controls.Add(lblSecondaryPhone);
            Controls.Add(txtSecondaryPhone);
            Controls.Add(lblSecondaryMobile);
            Controls.Add(txtSecondaryMobile);
            Controls.Add(lblSecondaryEmail);
            Controls.Add(txtSecondaryEmail);
            Controls.Add(lblSubTrades);
            Controls.Add(clbSubTrades);
            Controls.Add(lblPcabNumber);
            Controls.Add(txtPcabNumber);
            Controls.Add(lblPcabClassification);
            Controls.Add(txtPcabClassification);
            Controls.Add(lblPcabExpiry);
            Controls.Add(dtpPcabExpiry);
            Controls.Add(lblSubMsg);
            Controls.Add(btnSaveSubcontractor);
            Controls.Add(btnCancelSubcontractor);
            Name = "SubDirectoryTab";
            Size = new Size(960, 720);
            Load += SubDirectoryTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSubcontractors).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSearch;
        private TextBox txtSubSearch;
        private Label lblTradeFilter;
        private ComboBox cboSubTradeFilter;
        private Label lblPcabDue;
        private DataGridView dgvSubcontractors;
        private Button btnAddSubcontractor;
        private Button btnDeleteSubcontractor;
        private Button btnExportSubcontractors;
        private Label lblFormMode;
        private Label lblSubBusinessName;
        private TextBox txtSubBusinessName;
        private Label lblSubTradeName;
        private TextBox txtSubTradeName;
        private Label lblSubDtiSec;
        private TextBox txtSubDtiSec;
        private Label lblSubTin;
        private TextBox txtSubTin;
        private Label lblSubOfficeAddress;
        private TextBox txtSubOfficeAddress;
        private Label lblSubYardAddress;
        private TextBox txtSubYardAddress;
        private Label lblSubBankName;
        private TextBox txtSubBankName;
        private Label lblSubAccountName;
        private TextBox txtSubAccountName;
        private Label lblSubAccountNo;
        private TextBox txtSubAccountNo;
        private Label lblPrimaryName;
        private TextBox txtPrimaryName;
        private Label lblPrimaryRole;
        private TextBox txtPrimaryRole;
        private Label lblPrimaryPhone;
        private TextBox txtPrimaryPhone;
        private Label lblPrimaryMobile;
        private TextBox txtPrimaryMobile;
        private Label lblPrimaryEmail;
        private TextBox txtPrimaryEmail;
        private Label lblSecondaryName;
        private TextBox txtSecondaryName;
        private Label lblSecondaryRole;
        private TextBox txtSecondaryRole;
        private Label lblSecondaryPhone;
        private TextBox txtSecondaryPhone;
        private Label lblSecondaryMobile;
        private TextBox txtSecondaryMobile;
        private Label lblSecondaryEmail;
        private TextBox txtSecondaryEmail;
        private Label lblSubTrades;
        private CheckedListBox clbSubTrades;
        private Label lblPcabNumber;
        private TextBox txtPcabNumber;
        private Label lblPcabClassification;
        private TextBox txtPcabClassification;
        private Label lblPcabExpiry;
        private DateTimePicker dtpPcabExpiry;
        private Label lblSubMsg;
        private Button btnSaveSubcontractor;
        private Button btnCancelSubcontractor;
        private DataGridViewTextBoxColumn colBusinessName;
        private DataGridViewTextBoxColumn colTrades;
        private DataGridViewTextBoxColumn colPrimaryContact;
        private DataGridViewTextBoxColumn colPhone;
        private DataGridViewTextBoxColumn colPcabExpiry;
    }
}
