namespace Company.OfficeApp.Pages.Inventory
{
    partial class VendorsTab
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
            txtVendorSearch = new TextBox();
            dgvVendors = new DataGridView();
            btnAddVendor = new Button();
            btnDeleteVendor = new Button();
            btnExportVendors = new Button();
            lblFormMode = new Label();
            lblVendorName = new Label();
            txtVendorName = new TextBox();
            lblVendorContact = new Label();
            txtVendorContact = new TextBox();
            lblVendorPhone = new Label();
            txtVendorPhone = new TextBox();
            lblVendorEmail = new Label();
            txtVendorEmail = new TextBox();
            lblVendorAddress = new Label();
            txtVendorAddress = new TextBox();
            lblVendorBank = new Label();
            txtVendorBank = new TextBox();
            lblVendorAccountName = new Label();
            txtVendorAccountName = new TextBox();
            lblVendorAccountNo = new Label();
            txtVendorAccountNo = new TextBox();
            lblVendorVat = new Label();
            cboVendorVat = new ComboBox();
            lblPapers = new Label();
            dgvVendorDocs = new DataGridView();
            cboVendorDocType = new ComboBox();
            btnUploadVendorDoc = new Button();
            btnViewVendorDoc = new Button();
            btnDeleteVendorDoc = new Button();
            lblVendorMsg = new Label();
            btnSaveVendor = new Button();
            btnCancelVendor = new Button();
            colName = new DataGridViewTextBoxColumn();
            colContact = new DataGridViewTextBoxColumn();
            colPhone = new DataGridViewTextBoxColumn();
            colVat = new DataGridViewTextBoxColumn();
            colDocType = new DataGridViewTextBoxColumn();
            colDocFile = new DataGridViewTextBoxColumn();
            colDocDate = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvVendors).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvVendorDocs).BeginInit();
            SuspendLayout();
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(38, 36);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search";
            // 
            // txtVendorSearch
            // 
            txtVendorSearch.Location = new Point(95, 32);
            txtVendorSearch.Name = "txtVendorSearch";
            txtVendorSearch.Size = new Size(230, 23);
            txtVendorSearch.TabIndex = 0;
            // 
            // dgvVendors
            // 
            dgvVendors.AllowUserToAddRows = false;
            dgvVendors.AllowUserToDeleteRows = false;
            dgvVendors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVendors.Columns.AddRange(new DataGridViewColumn[] { colName, colContact, colPhone, colVat });
            dgvVendors.MultiSelect = false;
            dgvVendors.ReadOnly = true;
            dgvVendors.RowHeadersVisible = false;
            dgvVendors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVendors.Location = new Point(38, 74);
            dgvVendors.Name = "dgvVendors";
            dgvVendors.Size = new Size(1070, 264);
            dgvVendors.TabIndex = 1;
            dgvVendors.SelectionChanged += dgvVendors_SelectionChanged;
            // 
            // btnAddVendor
            // 
            btnAddVendor.Location = new Point(38, 349);
            btnAddVendor.Name = "btnAddVendor";
            btnAddVendor.Size = new Size(115, 36);
            btnAddVendor.Text = "Add Vendor";
            btnAddVendor.TabIndex = 2;
            btnAddVendor.Click += btnAddVendor_Click;
            // 
            // btnDeleteVendor
            // 
            btnDeleteVendor.Location = new Point(160, 349);
            btnDeleteVendor.Name = "btnDeleteVendor";
            btnDeleteVendor.Size = new Size(103, 36);
            btnDeleteVendor.Text = "Delete";
            btnDeleteVendor.TabIndex = 3;
            btnDeleteVendor.Click += btnDeleteVendor_Click;
            // 
            // btnExportVendors
            // 
            btnExportVendors.Location = new Point(270, 349);
            btnExportVendors.Name = "btnExportVendors";
            btnExportVendors.Size = new Size(103, 36);
            btnExportVendors.Text = "Export";
            btnExportVendors.TabIndex = 4;
            btnExportVendors.Click += btnExportVendors_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(38, 407);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New vendor";
            lblFormMode.Font = new Font("Segoe UI Semibold", 11.5F);
            // 
            // lblVendorName
            // 
            lblVendorName.AutoSize = true;
            lblVendorName.Location = new Point(38, 438);
            lblVendorName.Name = "lblVendorName";
            lblVendorName.Text = "Vendor / supplier name";
            // 
            // txtVendorName
            // 
            txtVendorName.Location = new Point(38, 462);
            txtVendorName.Name = "txtVendorName";
            txtVendorName.Size = new Size(345, 23);
            txtVendorName.TabIndex = 5;
            // 
            // lblVendorContact
            // 
            lblVendorContact.AutoSize = true;
            lblVendorContact.Location = new Point(38, 502);
            lblVendorContact.Name = "lblVendorContact";
            lblVendorContact.Text = "Contact person";
            // 
            // txtVendorContact
            // 
            txtVendorContact.Location = new Point(38, 526);
            txtVendorContact.Name = "txtVendorContact";
            txtVendorContact.Size = new Size(345, 23);
            txtVendorContact.TabIndex = 6;
            // 
            // lblVendorPhone
            // 
            lblVendorPhone.AutoSize = true;
            lblVendorPhone.Location = new Point(38, 565);
            lblVendorPhone.Name = "lblVendorPhone";
            lblVendorPhone.Text = "Phone";
            // 
            // txtVendorPhone
            // 
            txtVendorPhone.Location = new Point(38, 589);
            txtVendorPhone.Name = "txtVendorPhone";
            txtVendorPhone.Size = new Size(345, 23);
            txtVendorPhone.TabIndex = 7;
            // 
            // lblVendorEmail
            // 
            lblVendorEmail.AutoSize = true;
            lblVendorEmail.Location = new Point(38, 628);
            lblVendorEmail.Name = "lblVendorEmail";
            lblVendorEmail.Text = "Email";
            // 
            // txtVendorEmail
            // 
            txtVendorEmail.Location = new Point(38, 652);
            txtVendorEmail.Name = "txtVendorEmail";
            txtVendorEmail.Size = new Size(345, 23);
            txtVendorEmail.TabIndex = 8;
            // 
            // lblVendorAddress
            // 
            lblVendorAddress.AutoSize = true;
            lblVendorAddress.Location = new Point(38, 692);
            lblVendorAddress.Name = "lblVendorAddress";
            lblVendorAddress.Text = "Address";
            // 
            // txtVendorAddress
            // 
            txtVendorAddress.Location = new Point(38, 716);
            txtVendorAddress.Name = "txtVendorAddress";
            txtVendorAddress.Size = new Size(345, 61);
            txtVendorAddress.Multiline = true;
            txtVendorAddress.TabIndex = 9;
            // 
            // lblVendorBank
            // 
            lblVendorBank.AutoSize = true;
            lblVendorBank.Location = new Point(415, 438);
            lblVendorBank.Name = "lblVendorBank";
            lblVendorBank.Text = "Bank name";
            // 
            // txtVendorBank
            // 
            txtVendorBank.Location = new Point(415, 462);
            txtVendorBank.Name = "txtVendorBank";
            txtVendorBank.Size = new Size(276, 23);
            txtVendorBank.TabIndex = 10;
            // 
            // lblVendorAccountName
            // 
            lblVendorAccountName.AutoSize = true;
            lblVendorAccountName.Location = new Point(415, 502);
            lblVendorAccountName.Name = "lblVendorAccountName";
            lblVendorAccountName.Text = "Account name";
            // 
            // txtVendorAccountName
            // 
            txtVendorAccountName.Location = new Point(415, 526);
            txtVendorAccountName.Name = "txtVendorAccountName";
            txtVendorAccountName.Size = new Size(276, 23);
            txtVendorAccountName.TabIndex = 11;
            // 
            // lblVendorAccountNo
            // 
            lblVendorAccountNo.AutoSize = true;
            lblVendorAccountNo.Location = new Point(415, 565);
            lblVendorAccountNo.Name = "lblVendorAccountNo";
            lblVendorAccountNo.Text = "Account number";
            // 
            // txtVendorAccountNo
            // 
            txtVendorAccountNo.Location = new Point(415, 589);
            txtVendorAccountNo.Name = "txtVendorAccountNo";
            txtVendorAccountNo.Size = new Size(276, 23);
            txtVendorAccountNo.TabIndex = 12;
            // 
            // lblVendorVat
            // 
            lblVendorVat.AutoSize = true;
            lblVendorVat.Location = new Point(415, 628);
            lblVendorVat.Name = "lblVendorVat";
            lblVendorVat.Text = "VAT status";
            // 
            // cboVendorVat
            // 
            cboVendorVat.Location = new Point(415, 652);
            cboVendorVat.Name = "cboVendorVat";
            cboVendorVat.Size = new Size(161, 23);
            cboVendorVat.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVendorVat.TabIndex = 13;
            // 
            // lblPapers
            // 
            lblPapers.AutoSize = true;
            lblPapers.Location = new Point(726, 438);
            lblPapers.Name = "lblPapers";
            lblPapers.Text = "Vendor papers (save the vendor first)";
            // 
            // dgvVendorDocs
            // 
            dgvVendorDocs.AllowUserToAddRows = false;
            dgvVendorDocs.AllowUserToDeleteRows = false;
            dgvVendorDocs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVendorDocs.Columns.AddRange(new DataGridViewColumn[] { colDocType, colDocFile, colDocDate });
            dgvVendorDocs.MultiSelect = false;
            dgvVendorDocs.ReadOnly = true;
            dgvVendorDocs.RowHeadersVisible = false;
            dgvVendorDocs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVendorDocs.Location = new Point(726, 462);
            dgvVendorDocs.Name = "dgvVendorDocs";
            dgvVendorDocs.Size = new Size(382, 185);
            dgvVendorDocs.TabIndex = 14;
            // 
            // cboVendorDocType
            // 
            cboVendorDocType.Location = new Point(726, 658);
            cboVendorDocType.Name = "cboVendorDocType";
            cboVendorDocType.Size = new Size(230, 23);
            cboVendorDocType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVendorDocType.TabIndex = 15;
            // 
            // btnUploadVendorDoc
            // 
            btnUploadVendorDoc.Location = new Point(962, 655);
            btnUploadVendorDoc.Name = "btnUploadVendorDoc";
            btnUploadVendorDoc.Size = new Size(145, 36);
            btnUploadVendorDoc.Text = "Upload Paper";
            btnUploadVendorDoc.TabIndex = 16;
            btnUploadVendorDoc.Click += btnUploadVendorDoc_Click;
            // 
            // btnViewVendorDoc
            // 
            btnViewVendorDoc.Location = new Point(726, 697);
            btnViewVendorDoc.Name = "btnViewVendorDoc";
            btnViewVendorDoc.Size = new Size(103, 36);
            btnViewVendorDoc.Text = "View";
            btnViewVendorDoc.TabIndex = 17;
            btnViewVendorDoc.Click += btnViewVendorDoc_Click;
            // 
            // btnDeleteVendorDoc
            // 
            btnDeleteVendorDoc.Location = new Point(836, 697);
            btnDeleteVendorDoc.Name = "btnDeleteVendorDoc";
            btnDeleteVendorDoc.Size = new Size(120, 36);
            btnDeleteVendorDoc.Text = "Delete Paper";
            btnDeleteVendorDoc.TabIndex = 18;
            btnDeleteVendorDoc.Click += btnDeleteVendorDoc_Click;
            // 
            // lblVendorMsg
            // 
            lblVendorMsg.AutoSize = true;
            lblVendorMsg.Location = new Point(38, 790);
            lblVendorMsg.Name = "lblVendorMsg";
            // 
            // btnSaveVendor
            // 
            btnSaveVendor.Location = new Point(38, 819);
            btnSaveVendor.Name = "btnSaveVendor";
            btnSaveVendor.Size = new Size(103, 36);
            btnSaveVendor.Text = "Save";
            btnSaveVendor.TabIndex = 19;
            btnSaveVendor.Click += btnSaveVendor_Click;
            // 
            // btnCancelVendor
            // 
            btnCancelVendor.Location = new Point(148, 819);
            btnCancelVendor.Name = "btnCancelVendor";
            btnCancelVendor.Size = new Size(103, 36);
            btnCancelVendor.Text = "Cancel";
            btnCancelVendor.TabIndex = 20;
            btnCancelVendor.Click += btnCancelVendor_Click;
            // 
            // colName
            // 
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            // 
            // colContact
            // 
            colContact.DataPropertyName = "ContactPerson";
            colContact.HeaderText = "Contact Person";
            colContact.Name = "colContact";
            // 
            // colPhone
            // 
            colPhone.DataPropertyName = "Phone";
            colPhone.HeaderText = "Phone";
            colPhone.Name = "colPhone";
            // 
            // colVat
            // 
            colVat.DataPropertyName = "VatStatus";
            colVat.HeaderText = "VAT Status";
            colVat.Name = "colVat";
            // 
            // colDocType
            // 
            colDocType.DataPropertyName = "DocType";
            colDocType.HeaderText = "Paper";
            colDocType.Name = "colDocType";
            // 
            // colDocFile
            // 
            colDocFile.DataPropertyName = "OriginalName";
            colDocFile.HeaderText = "File";
            colDocFile.Name = "colDocFile";
            // 
            // colDocDate
            // 
            colDocDate.DataPropertyName = "UploadedAt";
            colDocDate.HeaderText = "Uploaded";
            colDocDate.Name = "colDocDate";
            colDocDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // VendorsTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblSearch);
            Controls.Add(txtVendorSearch);
            Controls.Add(dgvVendors);
            Controls.Add(btnAddVendor);
            Controls.Add(btnDeleteVendor);
            Controls.Add(btnExportVendors);
            Controls.Add(lblFormMode);
            Controls.Add(lblVendorName);
            Controls.Add(txtVendorName);
            Controls.Add(lblVendorContact);
            Controls.Add(txtVendorContact);
            Controls.Add(lblVendorPhone);
            Controls.Add(txtVendorPhone);
            Controls.Add(lblVendorEmail);
            Controls.Add(txtVendorEmail);
            Controls.Add(lblVendorAddress);
            Controls.Add(txtVendorAddress);
            Controls.Add(lblVendorBank);
            Controls.Add(txtVendorBank);
            Controls.Add(lblVendorAccountName);
            Controls.Add(txtVendorAccountName);
            Controls.Add(lblVendorAccountNo);
            Controls.Add(txtVendorAccountNo);
            Controls.Add(lblVendorVat);
            Controls.Add(cboVendorVat);
            Controls.Add(lblPapers);
            Controls.Add(dgvVendorDocs);
            Controls.Add(cboVendorDocType);
            Controls.Add(btnUploadVendorDoc);
            Controls.Add(btnViewVendorDoc);
            Controls.Add(btnDeleteVendorDoc);
            Controls.Add(lblVendorMsg);
            Controls.Add(btnSaveVendor);
            Controls.Add(btnCancelVendor);
            Name = "VendorsTab";
            Size = new Size(1152, 903);
            Load += VendorsTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvVendors).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvVendorDocs).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSearch;
        private TextBox txtVendorSearch;
        private DataGridView dgvVendors;
        private Button btnAddVendor;
        private Button btnDeleteVendor;
        private Button btnExportVendors;
        private Label lblFormMode;
        private Label lblVendorName;
        private TextBox txtVendorName;
        private Label lblVendorContact;
        private TextBox txtVendorContact;
        private Label lblVendorPhone;
        private TextBox txtVendorPhone;
        private Label lblVendorEmail;
        private TextBox txtVendorEmail;
        private Label lblVendorAddress;
        private TextBox txtVendorAddress;
        private Label lblVendorBank;
        private TextBox txtVendorBank;
        private Label lblVendorAccountName;
        private TextBox txtVendorAccountName;
        private Label lblVendorAccountNo;
        private TextBox txtVendorAccountNo;
        private Label lblVendorVat;
        private ComboBox cboVendorVat;
        private Label lblPapers;
        private DataGridView dgvVendorDocs;
        private ComboBox cboVendorDocType;
        private Button btnUploadVendorDoc;
        private Button btnViewVendorDoc;
        private Button btnDeleteVendorDoc;
        private Label lblVendorMsg;
        private Button btnSaveVendor;
        private Button btnCancelVendor;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colContact;
        private DataGridViewTextBoxColumn colPhone;
        private DataGridViewTextBoxColumn colVat;
        private DataGridViewTextBoxColumn colDocType;
        private DataGridViewTextBoxColumn colDocFile;
        private DataGridViewTextBoxColumn colDocDate;
    }
}
