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
            lblSearch.Location = new Point(12, 15);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search";
            // 
            // txtVendorSearch
            // 
            txtVendorSearch.Location = new Point(62, 12);
            txtVendorSearch.Name = "txtVendorSearch";
            txtVendorSearch.Size = new Size(200, 23);
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
            dgvVendors.Location = new Point(12, 44);
            dgvVendors.Name = "dgvVendors";
            dgvVendors.Size = new Size(930, 200);
            dgvVendors.TabIndex = 1;
            dgvVendors.SelectionChanged += dgvVendors_SelectionChanged;
            // 
            // btnAddVendor
            // 
            btnAddVendor.Location = new Point(12, 252);
            btnAddVendor.Name = "btnAddVendor";
            btnAddVendor.Size = new Size(100, 28);
            btnAddVendor.Text = "Add Vendor";
            btnAddVendor.TabIndex = 2;
            btnAddVendor.Click += btnAddVendor_Click;
            // 
            // btnDeleteVendor
            // 
            btnDeleteVendor.Location = new Point(118, 252);
            btnDeleteVendor.Name = "btnDeleteVendor";
            btnDeleteVendor.Size = new Size(90, 28);
            btnDeleteVendor.Text = "Delete";
            btnDeleteVendor.TabIndex = 3;
            btnDeleteVendor.Click += btnDeleteVendor_Click;
            // 
            // btnExportVendors
            // 
            btnExportVendors.Location = new Point(214, 252);
            btnExportVendors.Name = "btnExportVendors";
            btnExportVendors.Size = new Size(90, 28);
            btnExportVendors.Text = "Export";
            btnExportVendors.TabIndex = 4;
            btnExportVendors.Click += btnExportVendors_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(12, 296);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New vendor";
            // 
            // lblVendorName
            // 
            lblVendorName.AutoSize = true;
            lblVendorName.Location = new Point(12, 320);
            lblVendorName.Name = "lblVendorName";
            lblVendorName.Text = "Vendor / supplier name";
            // 
            // txtVendorName
            // 
            txtVendorName.Location = new Point(12, 338);
            txtVendorName.Name = "txtVendorName";
            txtVendorName.Size = new Size(300, 23);
            txtVendorName.TabIndex = 5;
            // 
            // lblVendorContact
            // 
            lblVendorContact.AutoSize = true;
            lblVendorContact.Location = new Point(12, 368);
            lblVendorContact.Name = "lblVendorContact";
            lblVendorContact.Text = "Contact person";
            // 
            // txtVendorContact
            // 
            txtVendorContact.Location = new Point(12, 386);
            txtVendorContact.Name = "txtVendorContact";
            txtVendorContact.Size = new Size(300, 23);
            txtVendorContact.TabIndex = 6;
            // 
            // lblVendorPhone
            // 
            lblVendorPhone.AutoSize = true;
            lblVendorPhone.Location = new Point(12, 416);
            lblVendorPhone.Name = "lblVendorPhone";
            lblVendorPhone.Text = "Phone";
            // 
            // txtVendorPhone
            // 
            txtVendorPhone.Location = new Point(12, 434);
            txtVendorPhone.Name = "txtVendorPhone";
            txtVendorPhone.Size = new Size(300, 23);
            txtVendorPhone.TabIndex = 7;
            // 
            // lblVendorEmail
            // 
            lblVendorEmail.AutoSize = true;
            lblVendorEmail.Location = new Point(12, 464);
            lblVendorEmail.Name = "lblVendorEmail";
            lblVendorEmail.Text = "Email";
            // 
            // txtVendorEmail
            // 
            txtVendorEmail.Location = new Point(12, 482);
            txtVendorEmail.Name = "txtVendorEmail";
            txtVendorEmail.Size = new Size(300, 23);
            txtVendorEmail.TabIndex = 8;
            // 
            // lblVendorAddress
            // 
            lblVendorAddress.AutoSize = true;
            lblVendorAddress.Location = new Point(12, 512);
            lblVendorAddress.Name = "lblVendorAddress";
            lblVendorAddress.Text = "Address";
            // 
            // txtVendorAddress
            // 
            txtVendorAddress.Location = new Point(12, 530);
            txtVendorAddress.Name = "txtVendorAddress";
            txtVendorAddress.Size = new Size(300, 46);
            txtVendorAddress.Multiline = true;
            txtVendorAddress.TabIndex = 9;
            // 
            // lblVendorBank
            // 
            lblVendorBank.AutoSize = true;
            lblVendorBank.Location = new Point(340, 320);
            lblVendorBank.Name = "lblVendorBank";
            lblVendorBank.Text = "Bank name";
            // 
            // txtVendorBank
            // 
            txtVendorBank.Location = new Point(340, 338);
            txtVendorBank.Name = "txtVendorBank";
            txtVendorBank.Size = new Size(240, 23);
            txtVendorBank.TabIndex = 10;
            // 
            // lblVendorAccountName
            // 
            lblVendorAccountName.AutoSize = true;
            lblVendorAccountName.Location = new Point(340, 368);
            lblVendorAccountName.Name = "lblVendorAccountName";
            lblVendorAccountName.Text = "Account name";
            // 
            // txtVendorAccountName
            // 
            txtVendorAccountName.Location = new Point(340, 386);
            txtVendorAccountName.Name = "txtVendorAccountName";
            txtVendorAccountName.Size = new Size(240, 23);
            txtVendorAccountName.TabIndex = 11;
            // 
            // lblVendorAccountNo
            // 
            lblVendorAccountNo.AutoSize = true;
            lblVendorAccountNo.Location = new Point(340, 416);
            lblVendorAccountNo.Name = "lblVendorAccountNo";
            lblVendorAccountNo.Text = "Account number";
            // 
            // txtVendorAccountNo
            // 
            txtVendorAccountNo.Location = new Point(340, 434);
            txtVendorAccountNo.Name = "txtVendorAccountNo";
            txtVendorAccountNo.Size = new Size(240, 23);
            txtVendorAccountNo.TabIndex = 12;
            // 
            // lblVendorVat
            // 
            lblVendorVat.AutoSize = true;
            lblVendorVat.Location = new Point(340, 464);
            lblVendorVat.Name = "lblVendorVat";
            lblVendorVat.Text = "VAT status";
            // 
            // cboVendorVat
            // 
            cboVendorVat.Location = new Point(340, 482);
            cboVendorVat.Name = "cboVendorVat";
            cboVendorVat.Size = new Size(140, 23);
            cboVendorVat.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVendorVat.TabIndex = 13;
            // 
            // lblPapers
            // 
            lblPapers.AutoSize = true;
            lblPapers.Location = new Point(610, 320);
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
            dgvVendorDocs.Location = new Point(610, 338);
            dgvVendorDocs.Name = "dgvVendorDocs";
            dgvVendorDocs.Size = new Size(332, 140);
            dgvVendorDocs.TabIndex = 14;
            // 
            // cboVendorDocType
            // 
            cboVendorDocType.Location = new Point(610, 486);
            cboVendorDocType.Name = "cboVendorDocType";
            cboVendorDocType.Size = new Size(200, 23);
            cboVendorDocType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVendorDocType.TabIndex = 15;
            // 
            // btnUploadVendorDoc
            // 
            btnUploadVendorDoc.Location = new Point(816, 484);
            btnUploadVendorDoc.Name = "btnUploadVendorDoc";
            btnUploadVendorDoc.Size = new Size(126, 28);
            btnUploadVendorDoc.Text = "Upload Paper";
            btnUploadVendorDoc.TabIndex = 16;
            btnUploadVendorDoc.Click += btnUploadVendorDoc_Click;
            // 
            // btnViewVendorDoc
            // 
            btnViewVendorDoc.Location = new Point(610, 516);
            btnViewVendorDoc.Name = "btnViewVendorDoc";
            btnViewVendorDoc.Size = new Size(90, 28);
            btnViewVendorDoc.Text = "View";
            btnViewVendorDoc.TabIndex = 17;
            btnViewVendorDoc.Click += btnViewVendorDoc_Click;
            // 
            // btnDeleteVendorDoc
            // 
            btnDeleteVendorDoc.Location = new Point(706, 516);
            btnDeleteVendorDoc.Name = "btnDeleteVendorDoc";
            btnDeleteVendorDoc.Size = new Size(104, 28);
            btnDeleteVendorDoc.Text = "Delete Paper";
            btnDeleteVendorDoc.TabIndex = 18;
            btnDeleteVendorDoc.Click += btnDeleteVendorDoc_Click;
            // 
            // lblVendorMsg
            // 
            lblVendorMsg.AutoSize = true;
            lblVendorMsg.Location = new Point(12, 586);
            lblVendorMsg.Name = "lblVendorMsg";
            // 
            // btnSaveVendor
            // 
            btnSaveVendor.Location = new Point(12, 608);
            btnSaveVendor.Name = "btnSaveVendor";
            btnSaveVendor.Size = new Size(90, 28);
            btnSaveVendor.Text = "Save";
            btnSaveVendor.TabIndex = 19;
            btnSaveVendor.Click += btnSaveVendor_Click;
            // 
            // btnCancelVendor
            // 
            btnCancelVendor.Location = new Point(108, 608);
            btnCancelVendor.Name = "btnCancelVendor";
            btnCancelVendor.Size = new Size(90, 28);
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
            Size = new Size(960, 660);
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
