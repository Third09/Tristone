namespace Company.OfficeApp.Pages.Inventory
{
    partial class DeliveriesTab
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
            lblRecordTitle = new Label();
            lblDeliveryPurchase = new Label();
            cboDeliveryPurchase = new ComboBox();
            lblDeliveryVendor = new Label();
            cboDeliveryVendor = new ComboBox();
            lblDeliveryMaterial = new Label();
            cboDeliveryMaterial = new ComboBox();
            lblDeliveryQty = new Label();
            numDeliveryQty = new NumericUpDown();
            lblDeliveryPrice = new Label();
            numDeliveryPrice = new NumericUpDown();
            lblDeliveryDate = new Label();
            dtpDeliveryDate = new DateTimePicker();
            lblReceiptTitle = new Label();
            lblDeliveryReceiptNo = new Label();
            txtDeliveryReceiptNo = new TextBox();
            btnAttachDeliveryReceipt = new Button();
            lblDeliveryReceiptFile = new Label();
            chkDeliveryPhysicalOnly = new CheckBox();
            lblDeliveryFiledAt = new Label();
            txtDeliveryFiledAt = new TextBox();
            lblDeliveryMsg = new Label();
            btnSaveDelivery = new Button();
            btnCancelDelivery = new Button();
            lblDeliveries = new Label();
            dgvDeliveries = new DataGridView();
            btnOpenDeliveryReceipt = new Button();
            colDate = new DataGridViewTextBoxColumn();
            colVendor = new DataGridViewTextBoxColumn();
            colMaterial = new DataGridViewTextBoxColumn();
            colQty = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colReceiptNo = new DataGridViewTextBoxColumn();
            colCopy = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)numDeliveryQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDeliveryPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDeliveries).BeginInit();
            SuspendLayout();
            // 
            // lblRecordTitle
            // 
            lblRecordTitle.AutoSize = true;
            lblRecordTitle.Location = new Point(12, 12);
            lblRecordTitle.Name = "lblRecordTitle";
            lblRecordTitle.Text = "Record a delivery (stock goes up; no approval needed)";
            // 
            // lblDeliveryPurchase
            // 
            lblDeliveryPurchase.AutoSize = true;
            lblDeliveryPurchase.Location = new Point(12, 36);
            lblDeliveryPurchase.Name = "lblDeliveryPurchase";
            lblDeliveryPurchase.Text = "Approved purchase (fills in vendor, material and price)";
            // 
            // cboDeliveryPurchase
            // 
            cboDeliveryPurchase.Location = new Point(12, 54);
            cboDeliveryPurchase.Name = "cboDeliveryPurchase";
            cboDeliveryPurchase.Size = new Size(460, 23);
            cboDeliveryPurchase.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDeliveryPurchase.TabIndex = 0;
            cboDeliveryPurchase.SelectedIndexChanged += cboDeliveryPurchase_SelectedIndexChanged;
            // 
            // lblDeliveryVendor
            // 
            lblDeliveryVendor.AutoSize = true;
            lblDeliveryVendor.Location = new Point(12, 84);
            lblDeliveryVendor.Name = "lblDeliveryVendor";
            lblDeliveryVendor.Text = "Vendor";
            // 
            // cboDeliveryVendor
            // 
            cboDeliveryVendor.Location = new Point(12, 102);
            cboDeliveryVendor.Name = "cboDeliveryVendor";
            cboDeliveryVendor.Size = new Size(220, 23);
            cboDeliveryVendor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDeliveryVendor.TabIndex = 1;
            // 
            // lblDeliveryMaterial
            // 
            lblDeliveryMaterial.AutoSize = true;
            lblDeliveryMaterial.Location = new Point(244, 84);
            lblDeliveryMaterial.Name = "lblDeliveryMaterial";
            lblDeliveryMaterial.Text = "Material";
            // 
            // cboDeliveryMaterial
            // 
            cboDeliveryMaterial.Location = new Point(244, 102);
            cboDeliveryMaterial.Name = "cboDeliveryMaterial";
            cboDeliveryMaterial.Size = new Size(228, 23);
            cboDeliveryMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDeliveryMaterial.TabIndex = 2;
            // 
            // lblDeliveryQty
            // 
            lblDeliveryQty.AutoSize = true;
            lblDeliveryQty.Location = new Point(12, 132);
            lblDeliveryQty.Name = "lblDeliveryQty";
            lblDeliveryQty.Text = "Quantity received";
            // 
            // numDeliveryQty
            // 
            numDeliveryQty.Location = new Point(12, 150);
            numDeliveryQty.Name = "numDeliveryQty";
            numDeliveryQty.Size = new Size(120, 23);
            numDeliveryQty.DecimalPlaces = 2;
            numDeliveryQty.ThousandsSeparator = true;
            numDeliveryQty.Maximum = 999999999m;
            numDeliveryQty.TabIndex = 3;
            // 
            // lblDeliveryPrice
            // 
            lblDeliveryPrice.AutoSize = true;
            lblDeliveryPrice.Location = new Point(144, 132);
            lblDeliveryPrice.Name = "lblDeliveryPrice";
            lblDeliveryPrice.Text = "Unit price on receipt";
            // 
            // numDeliveryPrice
            // 
            numDeliveryPrice.Location = new Point(144, 150);
            numDeliveryPrice.Name = "numDeliveryPrice";
            numDeliveryPrice.Size = new Size(120, 23);
            numDeliveryPrice.DecimalPlaces = 2;
            numDeliveryPrice.ThousandsSeparator = true;
            numDeliveryPrice.Maximum = 999999999m;
            numDeliveryPrice.TabIndex = 4;
            // 
            // lblDeliveryDate
            // 
            lblDeliveryDate.AutoSize = true;
            lblDeliveryDate.Location = new Point(276, 132);
            lblDeliveryDate.Name = "lblDeliveryDate";
            lblDeliveryDate.Text = "Date received";
            // 
            // dtpDeliveryDate
            // 
            dtpDeliveryDate.Location = new Point(276, 150);
            dtpDeliveryDate.Name = "dtpDeliveryDate";
            dtpDeliveryDate.Size = new Size(120, 23);
            dtpDeliveryDate.Format = DateTimePickerFormat.Short;
            dtpDeliveryDate.TabIndex = 5;
            // 
            // lblReceiptTitle
            // 
            lblReceiptTitle.AutoSize = true;
            lblReceiptTitle.Location = new Point(500, 36);
            lblReceiptTitle.Name = "lblReceiptTitle";
            lblReceiptTitle.Text = "Receipt";
            // 
            // lblDeliveryReceiptNo
            // 
            lblDeliveryReceiptNo.AutoSize = true;
            lblDeliveryReceiptNo.Location = new Point(500, 56);
            lblDeliveryReceiptNo.Name = "lblDeliveryReceiptNo";
            lblDeliveryReceiptNo.Text = "Receipt number";
            // 
            // txtDeliveryReceiptNo
            // 
            txtDeliveryReceiptNo.Location = new Point(500, 74);
            txtDeliveryReceiptNo.Name = "txtDeliveryReceiptNo";
            txtDeliveryReceiptNo.Size = new Size(200, 23);
            txtDeliveryReceiptNo.TabIndex = 6;
            // 
            // btnAttachDeliveryReceipt
            // 
            btnAttachDeliveryReceipt.Location = new Point(500, 102);
            btnAttachDeliveryReceipt.Name = "btnAttachDeliveryReceipt";
            btnAttachDeliveryReceipt.Size = new Size(120, 28);
            btnAttachDeliveryReceipt.Text = "Attach Scan";
            btnAttachDeliveryReceipt.TabIndex = 7;
            btnAttachDeliveryReceipt.Click += btnAttachDeliveryReceipt_Click;
            // 
            // lblDeliveryReceiptFile
            // 
            lblDeliveryReceiptFile.AutoSize = true;
            lblDeliveryReceiptFile.Location = new Point(628, 108);
            lblDeliveryReceiptFile.Name = "lblDeliveryReceiptFile";
            lblDeliveryReceiptFile.Text = "No file attached";
            // 
            // chkDeliveryPhysicalOnly
            // 
            chkDeliveryPhysicalOnly.AutoSize = true;
            chkDeliveryPhysicalOnly.Location = new Point(500, 138);
            chkDeliveryPhysicalOnly.Name = "chkDeliveryPhysicalOnly";
            chkDeliveryPhysicalOnly.Text = "Physical copy only (no scan)";
            chkDeliveryPhysicalOnly.TabIndex = 8;
            chkDeliveryPhysicalOnly.CheckedChanged += chkDeliveryPhysicalOnly_CheckedChanged;
            // 
            // lblDeliveryFiledAt
            // 
            lblDeliveryFiledAt.AutoSize = true;
            lblDeliveryFiledAt.Location = new Point(500, 162);
            lblDeliveryFiledAt.Name = "lblDeliveryFiledAt";
            lblDeliveryFiledAt.Text = "Where the paper copy is filed (e.g. Folder B, Oct 2026)";
            // 
            // txtDeliveryFiledAt
            // 
            txtDeliveryFiledAt.Location = new Point(500, 180);
            txtDeliveryFiledAt.Name = "txtDeliveryFiledAt";
            txtDeliveryFiledAt.Size = new Size(300, 23);
            txtDeliveryFiledAt.TabIndex = 9;
            // 
            // lblDeliveryMsg
            // 
            lblDeliveryMsg.AutoSize = true;
            lblDeliveryMsg.Location = new Point(12, 212);
            lblDeliveryMsg.Name = "lblDeliveryMsg";
            // 
            // btnSaveDelivery
            // 
            btnSaveDelivery.Location = new Point(12, 234);
            btnSaveDelivery.Name = "btnSaveDelivery";
            btnSaveDelivery.Size = new Size(120, 28);
            btnSaveDelivery.Text = "Save Delivery";
            btnSaveDelivery.TabIndex = 10;
            btnSaveDelivery.Click += btnSaveDelivery_Click;
            // 
            // btnCancelDelivery
            // 
            btnCancelDelivery.Location = new Point(138, 234);
            btnCancelDelivery.Name = "btnCancelDelivery";
            btnCancelDelivery.Size = new Size(90, 28);
            btnCancelDelivery.Text = "Clear";
            btnCancelDelivery.TabIndex = 11;
            btnCancelDelivery.Click += btnCancelDelivery_Click;
            // 
            // lblDeliveries
            // 
            lblDeliveries.AutoSize = true;
            lblDeliveries.Location = new Point(12, 280);
            lblDeliveries.Name = "lblDeliveries";
            lblDeliveries.Text = "Deliveries";
            // 
            // dgvDeliveries
            // 
            dgvDeliveries.AllowUserToAddRows = false;
            dgvDeliveries.AllowUserToDeleteRows = false;
            dgvDeliveries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDeliveries.Columns.AddRange(new DataGridViewColumn[] { colDate, colVendor, colMaterial, colQty, colTotal, colReceiptNo, colCopy });
            dgvDeliveries.MultiSelect = false;
            dgvDeliveries.ReadOnly = true;
            dgvDeliveries.RowHeadersVisible = false;
            dgvDeliveries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDeliveries.Location = new Point(12, 300);
            dgvDeliveries.Name = "dgvDeliveries";
            dgvDeliveries.Size = new Size(930, 260);
            dgvDeliveries.TabIndex = 12;
            dgvDeliveries.SelectionChanged += dgvDeliveries_SelectionChanged;
            // 
            // btnOpenDeliveryReceipt
            // 
            btnOpenDeliveryReceipt.Location = new Point(12, 568);
            btnOpenDeliveryReceipt.Name = "btnOpenDeliveryReceipt";
            btnOpenDeliveryReceipt.Size = new Size(140, 28);
            btnOpenDeliveryReceipt.Text = "Open Receipt Scan";
            btnOpenDeliveryReceipt.TabIndex = 13;
            btnOpenDeliveryReceipt.Click += btnOpenDeliveryReceipt_Click;
            // 
            // colDate
            // 
            colDate.DataPropertyName = "DeliveredOn";
            colDate.HeaderText = "Date";
            colDate.Name = "colDate";
            colDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colVendor
            // 
            colVendor.DataPropertyName = "Vendor";
            colVendor.HeaderText = "Vendor";
            colVendor.Name = "colVendor";
            // 
            // colMaterial
            // 
            colMaterial.DataPropertyName = "Material";
            colMaterial.HeaderText = "Material";
            colMaterial.Name = "colMaterial";
            // 
            // colQty
            // 
            colQty.DataPropertyName = "Quantity";
            colQty.HeaderText = "Qty";
            colQty.Name = "colQty";
            colQty.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" };
            // 
            // colTotal
            // 
            colTotal.DataPropertyName = "Total";
            colTotal.HeaderText = "Total";
            colTotal.Name = "colTotal";
            colTotal.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colReceiptNo
            // 
            colReceiptNo.DataPropertyName = "ReceiptNo";
            colReceiptNo.HeaderText = "Receipt No.";
            colReceiptNo.Name = "colReceiptNo";
            // 
            // colCopy
            // 
            colCopy.DataPropertyName = "ReceiptCopy";
            colCopy.HeaderText = "Receipt";
            colCopy.Name = "colCopy";
            // 
            // DeliveriesTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblRecordTitle);
            Controls.Add(lblDeliveryPurchase);
            Controls.Add(cboDeliveryPurchase);
            Controls.Add(lblDeliveryVendor);
            Controls.Add(cboDeliveryVendor);
            Controls.Add(lblDeliveryMaterial);
            Controls.Add(cboDeliveryMaterial);
            Controls.Add(lblDeliveryQty);
            Controls.Add(numDeliveryQty);
            Controls.Add(lblDeliveryPrice);
            Controls.Add(numDeliveryPrice);
            Controls.Add(lblDeliveryDate);
            Controls.Add(dtpDeliveryDate);
            Controls.Add(lblReceiptTitle);
            Controls.Add(lblDeliveryReceiptNo);
            Controls.Add(txtDeliveryReceiptNo);
            Controls.Add(btnAttachDeliveryReceipt);
            Controls.Add(lblDeliveryReceiptFile);
            Controls.Add(chkDeliveryPhysicalOnly);
            Controls.Add(lblDeliveryFiledAt);
            Controls.Add(txtDeliveryFiledAt);
            Controls.Add(lblDeliveryMsg);
            Controls.Add(btnSaveDelivery);
            Controls.Add(btnCancelDelivery);
            Controls.Add(lblDeliveries);
            Controls.Add(dgvDeliveries);
            Controls.Add(btnOpenDeliveryReceipt);
            Name = "DeliveriesTab";
            Size = new Size(960, 640);
            Load += DeliveriesTab_Load;
            ((System.ComponentModel.ISupportInitialize)numDeliveryQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDeliveryPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDeliveries).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRecordTitle;
        private Label lblDeliveryPurchase;
        private ComboBox cboDeliveryPurchase;
        private Label lblDeliveryVendor;
        private ComboBox cboDeliveryVendor;
        private Label lblDeliveryMaterial;
        private ComboBox cboDeliveryMaterial;
        private Label lblDeliveryQty;
        private NumericUpDown numDeliveryQty;
        private Label lblDeliveryPrice;
        private NumericUpDown numDeliveryPrice;
        private Label lblDeliveryDate;
        private DateTimePicker dtpDeliveryDate;
        private Label lblReceiptTitle;
        private Label lblDeliveryReceiptNo;
        private TextBox txtDeliveryReceiptNo;
        private Button btnAttachDeliveryReceipt;
        private Label lblDeliveryReceiptFile;
        private CheckBox chkDeliveryPhysicalOnly;
        private Label lblDeliveryFiledAt;
        private TextBox txtDeliveryFiledAt;
        private Label lblDeliveryMsg;
        private Button btnSaveDelivery;
        private Button btnCancelDelivery;
        private Label lblDeliveries;
        private DataGridView dgvDeliveries;
        private Button btnOpenDeliveryReceipt;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colVendor;
        private DataGridViewTextBoxColumn colMaterial;
        private DataGridViewTextBoxColumn colQty;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colReceiptNo;
        private DataGridViewTextBoxColumn colCopy;
    }
}
