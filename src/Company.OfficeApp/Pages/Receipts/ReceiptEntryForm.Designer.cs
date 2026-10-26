namespace Company.OfficeApp.Pages.Receipts
{
    partial class ReceiptEntryForm
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
            lblReceiptNo = new Label();
            txtReceiptNo = new TextBox();
            lblPurchaseDate = new Label();
            dtpPurchaseDate = new DateTimePicker();
            lblMerchant = new Label();
            txtMerchant = new TextBox();
            lblReceiptAmount = new Label();
            numReceiptAmount = new NumericUpDown();
            lblReceiptCategory = new Label();
            cboReceiptCategory = new ComboBox();
            rdoProjectExpense = new RadioButton();
            rdoOverheadExpense = new RadioButton();
            cboReceiptForProject = new ComboBox();
            lblUploadedBy = new Label();
            lblReceiptCopy = new Label();
            btnAttachScan = new Button();
            lblScanFile = new Label();
            chkPhysicalOnly = new CheckBox();
            lblFiledLocation = new Label();
            txtFiledLocation = new TextBox();
            chkNoReceipt = new CheckBox();
            lblNoReceiptReason = new Label();
            txtNoReceiptReason = new TextBox();
            lblEntryMsg = new Label();
            btnSaveReceipt = new Button();
            btnCancelReceipt = new Button();
            ((System.ComponentModel.ISupportInitialize)numReceiptAmount).BeginInit();
            SuspendLayout();
            // 
            // lblReceiptNo
            // 
            lblReceiptNo.AutoSize = true;
            lblReceiptNo.Location = new Point(12, 12);
            lblReceiptNo.Name = "lblReceiptNo";
            lblReceiptNo.Text = "Receipt number";
            // 
            // txtReceiptNo
            // 
            txtReceiptNo.Location = new Point(12, 30);
            txtReceiptNo.Name = "txtReceiptNo";
            txtReceiptNo.Size = new Size(200, 23);
            txtReceiptNo.TabIndex = 0;
            // 
            // lblPurchaseDate
            // 
            lblPurchaseDate.AutoSize = true;
            lblPurchaseDate.Location = new Point(222, 12);
            lblPurchaseDate.Name = "lblPurchaseDate";
            lblPurchaseDate.Text = "Date of purchase";
            // 
            // dtpPurchaseDate
            // 
            dtpPurchaseDate.Location = new Point(222, 30);
            dtpPurchaseDate.Name = "dtpPurchaseDate";
            dtpPurchaseDate.Size = new Size(130, 23);
            dtpPurchaseDate.Format = DateTimePickerFormat.Short;
            dtpPurchaseDate.TabIndex = 1;
            // 
            // lblMerchant
            // 
            lblMerchant.AutoSize = true;
            lblMerchant.Location = new Point(12, 60);
            lblMerchant.Name = "lblMerchant";
            lblMerchant.Text = "Merchant / store";
            // 
            // txtMerchant
            // 
            txtMerchant.Location = new Point(12, 78);
            txtMerchant.Name = "txtMerchant";
            txtMerchant.Size = new Size(340, 23);
            txtMerchant.TabIndex = 2;
            // 
            // lblReceiptAmount
            // 
            lblReceiptAmount.AutoSize = true;
            lblReceiptAmount.Location = new Point(12, 108);
            lblReceiptAmount.Name = "lblReceiptAmount";
            lblReceiptAmount.Text = "Amount";
            // 
            // numReceiptAmount
            // 
            numReceiptAmount.Location = new Point(12, 126);
            numReceiptAmount.Name = "numReceiptAmount";
            numReceiptAmount.Size = new Size(140, 23);
            numReceiptAmount.DecimalPlaces = 2;
            numReceiptAmount.ThousandsSeparator = true;
            numReceiptAmount.Maximum = 999999999999m;
            numReceiptAmount.TabIndex = 3;
            // 
            // lblReceiptCategory
            // 
            lblReceiptCategory.AutoSize = true;
            lblReceiptCategory.Location = new Point(162, 108);
            lblReceiptCategory.Name = "lblReceiptCategory";
            lblReceiptCategory.Text = "Category";
            // 
            // cboReceiptCategory
            // 
            cboReceiptCategory.Location = new Point(162, 126);
            cboReceiptCategory.Name = "cboReceiptCategory";
            cboReceiptCategory.Size = new Size(190, 23);
            cboReceiptCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReceiptCategory.TabIndex = 4;
            // 
            // rdoProjectExpense
            // 
            rdoProjectExpense.Location = new Point(12, 160);
            rdoProjectExpense.Name = "rdoProjectExpense";
            rdoProjectExpense.Text = "Project expense";
            rdoProjectExpense.AutoSize = true;
            rdoProjectExpense.Checked = true;
            rdoProjectExpense.TabIndex = 5;
            rdoProjectExpense.CheckedChanged += Options_Changed;
            // 
            // rdoOverheadExpense
            // 
            rdoOverheadExpense.Location = new Point(150, 160);
            rdoOverheadExpense.Name = "rdoOverheadExpense";
            rdoOverheadExpense.Text = "Company overhead";
            rdoOverheadExpense.AutoSize = true;
            rdoOverheadExpense.TabIndex = 6;
            // 
            // cboReceiptForProject
            // 
            cboReceiptForProject.Location = new Point(12, 186);
            cboReceiptForProject.Name = "cboReceiptForProject";
            cboReceiptForProject.Size = new Size(340, 23);
            cboReceiptForProject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReceiptForProject.TabIndex = 7;
            // 
            // lblUploadedBy
            // 
            lblUploadedBy.AutoSize = true;
            lblUploadedBy.Location = new Point(12, 220);
            lblUploadedBy.Name = "lblUploadedBy";
            lblUploadedBy.Text = "";
            // 
            // lblReceiptCopy
            // 
            lblReceiptCopy.AutoSize = true;
            lblReceiptCopy.Location = new Point(12, 252);
            lblReceiptCopy.Name = "lblReceiptCopy";
            lblReceiptCopy.Text = "Receipt copy";
            // 
            // btnAttachScan
            // 
            btnAttachScan.Location = new Point(12, 272);
            btnAttachScan.Name = "btnAttachScan";
            btnAttachScan.Size = new Size(110, 28);
            btnAttachScan.Text = "Attach Scan";
            btnAttachScan.TabIndex = 8;
            btnAttachScan.Click += btnAttachScan_Click;
            // 
            // lblScanFile
            // 
            lblScanFile.AutoSize = true;
            lblScanFile.Location = new Point(130, 278);
            lblScanFile.Name = "lblScanFile";
            lblScanFile.Text = "(no file)";
            // 
            // chkPhysicalOnly
            // 
            chkPhysicalOnly.AutoSize = true;
            chkPhysicalOnly.Location = new Point(12, 310);
            chkPhysicalOnly.Name = "chkPhysicalOnly";
            chkPhysicalOnly.Text = "Physical copy only";
            chkPhysicalOnly.TabIndex = 9;
            chkPhysicalOnly.CheckedChanged += Options_Changed;
            // 
            // lblFiledLocation
            // 
            lblFiledLocation.AutoSize = true;
            lblFiledLocation.Location = new Point(12, 336);
            lblFiledLocation.Name = "lblFiledLocation";
            lblFiledLocation.Text = "Where the paper copy is filed";
            // 
            // txtFiledLocation
            // 
            txtFiledLocation.Location = new Point(12, 354);
            txtFiledLocation.Name = "txtFiledLocation";
            txtFiledLocation.Size = new Size(340, 23);
            txtFiledLocation.TabIndex = 10;
            // 
            // chkNoReceipt
            // 
            chkNoReceipt.AutoSize = true;
            chkNoReceipt.Location = new Point(12, 392);
            chkNoReceipt.Name = "chkNoReceipt";
            chkNoReceipt.Text = "No receipt at all (goes to Needs Review)";
            chkNoReceipt.TabIndex = 11;
            chkNoReceipt.CheckedChanged += Options_Changed;
            // 
            // lblNoReceiptReason
            // 
            lblNoReceiptReason.AutoSize = true;
            lblNoReceiptReason.Location = new Point(12, 418);
            lblNoReceiptReason.Name = "lblNoReceiptReason";
            lblNoReceiptReason.Text = "Why there is no receipt";
            // 
            // txtNoReceiptReason
            // 
            txtNoReceiptReason.Location = new Point(12, 436);
            txtNoReceiptReason.Name = "txtNoReceiptReason";
            txtNoReceiptReason.Size = new Size(490, 50);
            txtNoReceiptReason.Multiline = true;
            txtNoReceiptReason.ScrollBars = ScrollBars.Vertical;
            txtNoReceiptReason.TabIndex = 12;
            // 
            // lblEntryMsg
            // 
            lblEntryMsg.AutoSize = true;
            lblEntryMsg.Location = new Point(12, 494);
            lblEntryMsg.Name = "lblEntryMsg";
            // 
            // btnSaveReceipt
            // 
            btnSaveReceipt.Location = new Point(320, 518);
            btnSaveReceipt.Name = "btnSaveReceipt";
            btnSaveReceipt.Size = new Size(90, 28);
            btnSaveReceipt.Text = "Save";
            btnSaveReceipt.TabIndex = 13;
            btnSaveReceipt.Click += btnSaveReceipt_Click;
            // 
            // btnCancelReceipt
            // 
            btnCancelReceipt.Location = new Point(416, 518);
            btnCancelReceipt.Name = "btnCancelReceipt";
            btnCancelReceipt.Size = new Size(90, 28);
            btnCancelReceipt.Text = "Cancel";
            btnCancelReceipt.DialogResult = DialogResult.Cancel;
            btnCancelReceipt.TabIndex = 14;
            // 
            // ReceiptEntryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 560);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(lblReceiptNo);
            Controls.Add(txtReceiptNo);
            Controls.Add(lblPurchaseDate);
            Controls.Add(dtpPurchaseDate);
            Controls.Add(lblMerchant);
            Controls.Add(txtMerchant);
            Controls.Add(lblReceiptAmount);
            Controls.Add(numReceiptAmount);
            Controls.Add(lblReceiptCategory);
            Controls.Add(cboReceiptCategory);
            Controls.Add(rdoProjectExpense);
            Controls.Add(rdoOverheadExpense);
            Controls.Add(cboReceiptForProject);
            Controls.Add(lblUploadedBy);
            Controls.Add(lblReceiptCopy);
            Controls.Add(btnAttachScan);
            Controls.Add(lblScanFile);
            Controls.Add(chkPhysicalOnly);
            Controls.Add(lblFiledLocation);
            Controls.Add(txtFiledLocation);
            Controls.Add(chkNoReceipt);
            Controls.Add(lblNoReceiptReason);
            Controls.Add(txtNoReceiptReason);
            Controls.Add(lblEntryMsg);
            Controls.Add(btnSaveReceipt);
            Controls.Add(btnCancelReceipt);
            Name = "ReceiptEntryForm";
            Load += ReceiptEntryForm_Load;
            ((System.ComponentModel.ISupportInitialize)numReceiptAmount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblReceiptNo;
        private TextBox txtReceiptNo;
        private Label lblPurchaseDate;
        private DateTimePicker dtpPurchaseDate;
        private Label lblMerchant;
        private TextBox txtMerchant;
        private Label lblReceiptAmount;
        private NumericUpDown numReceiptAmount;
        private Label lblReceiptCategory;
        private ComboBox cboReceiptCategory;
        private RadioButton rdoProjectExpense;
        private RadioButton rdoOverheadExpense;
        private ComboBox cboReceiptForProject;
        private Label lblUploadedBy;
        private Label lblReceiptCopy;
        private Button btnAttachScan;
        private Label lblScanFile;
        private CheckBox chkPhysicalOnly;
        private Label lblFiledLocation;
        private TextBox txtFiledLocation;
        private CheckBox chkNoReceipt;
        private Label lblNoReceiptReason;
        private TextBox txtNoReceiptReason;
        private Label lblEntryMsg;
        private Button btnSaveReceipt;
        private Button btnCancelReceipt;
    }
}
