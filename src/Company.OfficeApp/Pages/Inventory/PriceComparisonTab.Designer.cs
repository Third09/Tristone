namespace Company.OfficeApp.Pages.Inventory
{
    partial class PriceComparisonTab
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
            lblCompareMaterial = new Label();
            cboCompareMaterial = new ComboBox();
            lblAddQuote = new Label();
            lblQuoteVendor = new Label();
            cboQuoteVendor = new ComboBox();
            lblQuoteDate = new Label();
            dtpQuoteDate = new DateTimePicker();
            lblQuoteProduct = new Label();
            txtQuoteProduct = new TextBox();
            lblQuoteCategory = new Label();
            cboQuoteCategory = new ComboBox();
            lblQuoteDescription = new Label();
            txtQuoteDescription = new TextBox();
            lblQuoteUnitPrice = new Label();
            numQuoteUnitPrice = new NumericUpDown();
            lblQuoteQty = new Label();
            numQuoteQty = new NumericUpDown();
            btnAddQuote = new Button();
            lblQuoteMsg = new Label();
            lblCheapestQuote = new Label();
            dgvQuotes = new DataGridView();
            btnDeleteQuote = new Button();
            btnApprovePurchase = new Button();
            lblApproverNote = new Label();
            lblPurchases = new Label();
            dgvPurchases = new DataGridView();
            colQVendor = new DataGridViewTextBoxColumn();
            colQDate = new DataGridViewTextBoxColumn();
            colQProduct = new DataGridViewTextBoxColumn();
            colQUnitPrice = new DataGridViewTextBoxColumn();
            colQQty = new DataGridViewTextBoxColumn();
            colQTotal = new DataGridViewTextBoxColumn();
            colQStatus = new DataGridViewTextBoxColumn();
            colPDate = new DataGridViewTextBoxColumn();
            colPVendor = new DataGridViewTextBoxColumn();
            colPMaterial = new DataGridViewTextBoxColumn();
            colPQty = new DataGridViewTextBoxColumn();
            colPTotal = new DataGridViewTextBoxColumn();
            colPDelivered = new DataGridViewTextBoxColumn();
            colPDone = new DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)numQuoteUnitPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numQuoteQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvQuotes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).BeginInit();
            SuspendLayout();
            // 
            // lblCompareMaterial
            // 
            lblCompareMaterial.AutoSize = true;
            lblCompareMaterial.Location = new Point(38, 36);
            lblCompareMaterial.Name = "lblCompareMaterial";
            lblCompareMaterial.Text = "Material to buy";
            // 
            // cboCompareMaterial
            // 
            cboCompareMaterial.Location = new Point(150, 32);
            cboCompareMaterial.Name = "cboCompareMaterial";
            cboCompareMaterial.Size = new Size(299, 23);
            cboCompareMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCompareMaterial.TabIndex = 0;
            cboCompareMaterial.SelectedIndexChanged += cboCompareMaterial_SelectedIndexChanged;
            // 
            // lblAddQuote
            // 
            lblAddQuote.AutoSize = true;
            lblAddQuote.Location = new Point(38, 77);
            lblAddQuote.Name = "lblAddQuote";
            lblAddQuote.Text = "Add a quote";
            // 
            // lblQuoteVendor
            // 
            lblQuoteVendor.AutoSize = true;
            lblQuoteVendor.Location = new Point(38, 103);
            lblQuoteVendor.Name = "lblQuoteVendor";
            lblQuoteVendor.Text = "Vendor";
            // 
            // cboQuoteVendor
            // 
            cboQuoteVendor.Location = new Point(38, 127);
            cboQuoteVendor.Name = "cboQuoteVendor";
            cboQuoteVendor.Size = new Size(230, 23);
            cboQuoteVendor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuoteVendor.TabIndex = 1;
            // 
            // lblQuoteDate
            // 
            lblQuoteDate.AutoSize = true;
            lblQuoteDate.Location = new Point(282, 103);
            lblQuoteDate.Name = "lblQuoteDate";
            lblQuoteDate.Text = "Date inputted";
            // 
            // dtpQuoteDate
            // 
            dtpQuoteDate.Location = new Point(282, 127);
            dtpQuoteDate.Name = "dtpQuoteDate";
            dtpQuoteDate.Size = new Size(138, 23);
            dtpQuoteDate.Format = DateTimePickerFormat.Short;
            dtpQuoteDate.TabIndex = 2;
            // 
            // lblQuoteProduct
            // 
            lblQuoteProduct.AutoSize = true;
            lblQuoteProduct.Location = new Point(433, 103);
            lblQuoteProduct.Name = "lblQuoteProduct";
            lblQuoteProduct.Text = "Product name";
            // 
            // txtQuoteProduct
            // 
            txtQuoteProduct.Location = new Point(433, 127);
            txtQuoteProduct.Name = "txtQuoteProduct";
            txtQuoteProduct.Size = new Size(230, 23);
            txtQuoteProduct.TabIndex = 3;
            // 
            // lblQuoteCategory
            // 
            lblQuoteCategory.AutoSize = true;
            lblQuoteCategory.Location = new Point(677, 103);
            lblQuoteCategory.Name = "lblQuoteCategory";
            lblQuoteCategory.Text = "Category";
            // 
            // cboQuoteCategory
            // 
            cboQuoteCategory.Location = new Point(677, 127);
            cboQuoteCategory.Name = "cboQuoteCategory";
            cboQuoteCategory.Size = new Size(161, 23);
            cboQuoteCategory.TabIndex = 4;
            // 
            // lblQuoteDescription
            // 
            lblQuoteDescription.AutoSize = true;
            lblQuoteDescription.Location = new Point(38, 166);
            lblQuoteDescription.Name = "lblQuoteDescription";
            lblQuoteDescription.Text = "Description";
            // 
            // txtQuoteDescription
            // 
            txtQuoteDescription.Location = new Point(38, 190);
            txtQuoteDescription.Name = "txtQuoteDescription";
            txtQuoteDescription.Size = new Size(382, 23);
            txtQuoteDescription.TabIndex = 5;
            // 
            // lblQuoteUnitPrice
            // 
            lblQuoteUnitPrice.AutoSize = true;
            lblQuoteUnitPrice.Location = new Point(433, 166);
            lblQuoteUnitPrice.Name = "lblQuoteUnitPrice";
            lblQuoteUnitPrice.Text = "Unit price";
            // 
            // numQuoteUnitPrice
            // 
            numQuoteUnitPrice.Location = new Point(433, 190);
            numQuoteUnitPrice.Name = "numQuoteUnitPrice";
            numQuoteUnitPrice.Size = new Size(138, 23);
            numQuoteUnitPrice.DecimalPlaces = 2;
            numQuoteUnitPrice.ThousandsSeparator = true;
            numQuoteUnitPrice.Maximum = 999999999m;
            numQuoteUnitPrice.TabIndex = 6;
            // 
            // lblQuoteQty
            // 
            lblQuoteQty.AutoSize = true;
            lblQuoteQty.Location = new Point(585, 166);
            lblQuoteQty.Name = "lblQuoteQty";
            lblQuoteQty.Text = "Quantity";
            // 
            // numQuoteQty
            // 
            numQuoteQty.Location = new Point(585, 190);
            numQuoteQty.Name = "numQuoteQty";
            numQuoteQty.Size = new Size(115, 23);
            numQuoteQty.DecimalPlaces = 2;
            numQuoteQty.ThousandsSeparator = true;
            numQuoteQty.Maximum = 999999999m;
            numQuoteQty.TabIndex = 7;
            // 
            // btnAddQuote
            // 
            btnAddQuote.Location = new Point(714, 188);
            btnAddQuote.Name = "btnAddQuote";
            btnAddQuote.Size = new Size(124, 36);
            btnAddQuote.Text = "Add Quote";
            btnAddQuote.TabIndex = 8;
            btnAddQuote.Click += btnAddQuote_Click;
            // 
            // lblQuoteMsg
            // 
            lblQuoteMsg.AutoSize = true;
            lblQuoteMsg.Location = new Point(38, 230);
            lblQuoteMsg.Name = "lblQuoteMsg";
            // 
            // lblCheapestQuote
            // 
            lblCheapestQuote.AutoSize = true;
            lblCheapestQuote.Location = new Point(38, 267);
            lblCheapestQuote.Name = "lblCheapestQuote";
            lblCheapestQuote.Text = "";
            // 
            // dgvQuotes
            // 
            dgvQuotes.AllowUserToAddRows = false;
            dgvQuotes.AllowUserToDeleteRows = false;
            dgvQuotes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvQuotes.Columns.AddRange(new DataGridViewColumn[] { colQVendor, colQDate, colQProduct, colQUnitPrice, colQQty, colQTotal, colQStatus });
            dgvQuotes.MultiSelect = false;
            dgvQuotes.ReadOnly = true;
            dgvQuotes.RowHeadersVisible = false;
            dgvQuotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvQuotes.Location = new Point(38, 293);
            dgvQuotes.Name = "dgvQuotes";
            dgvQuotes.Size = new Size(1070, 238);
            dgvQuotes.TabIndex = 9;
            dgvQuotes.SelectionChanged += dgvQuotes_SelectionChanged;
            dgvQuotes.CellFormatting += dgvQuotes_CellFormatting;
            // 
            // btnDeleteQuote
            // 
            btnDeleteQuote.Location = new Point(38, 541);
            btnDeleteQuote.Name = "btnDeleteQuote";
            btnDeleteQuote.Size = new Size(126, 36);
            btnDeleteQuote.Text = "Delete Quote";
            btnDeleteQuote.TabIndex = 10;
            btnDeleteQuote.Click += btnDeleteQuote_Click;
            // 
            // btnApprovePurchase
            // 
            btnApprovePurchase.Location = new Point(171, 541);
            btnApprovePurchase.Name = "btnApprovePurchase";
            btnApprovePurchase.Size = new Size(184, 36);
            btnApprovePurchase.Text = "Approve as Purchase";
            btnApprovePurchase.TabIndex = 11;
            btnApprovePurchase.Click += btnApprovePurchase_Click;
            // 
            // lblApproverNote
            // 
            lblApproverNote.AutoSize = true;
            lblApproverNote.Location = new Point(369, 549);
            lblApproverNote.Name = "lblApproverNote";
            lblApproverNote.Text = "Approve as Purchase: Inventory Supervisor only";
            // 
            // lblPurchases
            // 
            lblPurchases.AutoSize = true;
            lblPurchases.Location = new Point(38, 602);
            lblPurchases.Name = "lblPurchases";
            lblPurchases.Text = "Approved purchases";
            // 
            // dgvPurchases
            // 
            dgvPurchases.AllowUserToAddRows = false;
            dgvPurchases.AllowUserToDeleteRows = false;
            dgvPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPurchases.Columns.AddRange(new DataGridViewColumn[] { colPDate, colPVendor, colPMaterial, colPQty, colPTotal, colPDelivered, colPDone });
            dgvPurchases.MultiSelect = false;
            dgvPurchases.ReadOnly = true;
            dgvPurchases.RowHeadersVisible = false;
            dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPurchases.Location = new Point(38, 628);
            dgvPurchases.Name = "dgvPurchases";
            dgvPurchases.Size = new Size(1070, 238);
            dgvPurchases.TabIndex = 12;
            // 
            // colQVendor
            // 
            colQVendor.DataPropertyName = "Vendor";
            colQVendor.HeaderText = "Vendor";
            colQVendor.Name = "colQVendor";
            // 
            // colQDate
            // 
            colQDate.DataPropertyName = "QuotedOn";
            colQDate.HeaderText = "Date";
            colQDate.Name = "colQDate";
            colQDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colQProduct
            // 
            colQProduct.DataPropertyName = "ProductName";
            colQProduct.HeaderText = "Product";
            colQProduct.Name = "colQProduct";
            // 
            // colQUnitPrice
            // 
            colQUnitPrice.DataPropertyName = "UnitPrice";
            colQUnitPrice.HeaderText = "Unit Price";
            colQUnitPrice.Name = "colQUnitPrice";
            colQUnitPrice.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colQQty
            // 
            colQQty.DataPropertyName = "Quantity";
            colQQty.HeaderText = "Qty";
            colQQty.Name = "colQQty";
            colQQty.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" };
            // 
            // colQTotal
            // 
            colQTotal.DataPropertyName = "Total";
            colQTotal.HeaderText = "Total";
            colQTotal.Name = "colQTotal";
            colQTotal.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colQStatus
            // 
            colQStatus.DataPropertyName = "Status";
            colQStatus.HeaderText = "Status";
            colQStatus.Name = "colQStatus";
            // 
            // colPDate
            // 
            colPDate.DataPropertyName = "ApprovedAt";
            colPDate.HeaderText = "Approved";
            colPDate.Name = "colPDate";
            colPDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colPVendor
            // 
            colPVendor.DataPropertyName = "Vendor";
            colPVendor.HeaderText = "Vendor";
            colPVendor.Name = "colPVendor";
            // 
            // colPMaterial
            // 
            colPMaterial.DataPropertyName = "Material";
            colPMaterial.HeaderText = "Material";
            colPMaterial.Name = "colPMaterial";
            // 
            // colPQty
            // 
            colPQty.DataPropertyName = "Quantity";
            colPQty.HeaderText = "Qty";
            colPQty.Name = "colPQty";
            colPQty.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" };
            // 
            // colPTotal
            // 
            colPTotal.DataPropertyName = "Total";
            colPTotal.HeaderText = "Total";
            colPTotal.Name = "colPTotal";
            colPTotal.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colPDelivered
            // 
            colPDelivered.DataPropertyName = "DeliveredQty";
            colPDelivered.HeaderText = "Delivered Qty";
            colPDelivered.Name = "colPDelivered";
            colPDelivered.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" };
            // 
            // colPDone
            // 
            colPDone.DataPropertyName = "Delivered";
            colPDone.HeaderText = "Fully Delivered";
            colPDone.Name = "colPDone";
            // 
            // PriceComparisonTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblCompareMaterial);
            Controls.Add(cboCompareMaterial);
            Controls.Add(lblAddQuote);
            Controls.Add(lblQuoteVendor);
            Controls.Add(cboQuoteVendor);
            Controls.Add(lblQuoteDate);
            Controls.Add(dtpQuoteDate);
            Controls.Add(lblQuoteProduct);
            Controls.Add(txtQuoteProduct);
            Controls.Add(lblQuoteCategory);
            Controls.Add(cboQuoteCategory);
            Controls.Add(lblQuoteDescription);
            Controls.Add(txtQuoteDescription);
            Controls.Add(lblQuoteUnitPrice);
            Controls.Add(numQuoteUnitPrice);
            Controls.Add(lblQuoteQty);
            Controls.Add(numQuoteQty);
            Controls.Add(btnAddQuote);
            Controls.Add(lblQuoteMsg);
            Controls.Add(lblCheapestQuote);
            Controls.Add(dgvQuotes);
            Controls.Add(btnDeleteQuote);
            Controls.Add(btnApprovePurchase);
            Controls.Add(lblApproverNote);
            Controls.Add(lblPurchases);
            Controls.Add(dgvPurchases);
            Name = "PriceComparisonTab";
            Size = new Size(1152, 956);
            Load += PriceComparisonTab_Load;
            ((System.ComponentModel.ISupportInitialize)numQuoteUnitPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numQuoteQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvQuotes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCompareMaterial;
        private ComboBox cboCompareMaterial;
        private Label lblAddQuote;
        private Label lblQuoteVendor;
        private ComboBox cboQuoteVendor;
        private Label lblQuoteDate;
        private DateTimePicker dtpQuoteDate;
        private Label lblQuoteProduct;
        private TextBox txtQuoteProduct;
        private Label lblQuoteCategory;
        private ComboBox cboQuoteCategory;
        private Label lblQuoteDescription;
        private TextBox txtQuoteDescription;
        private Label lblQuoteUnitPrice;
        private NumericUpDown numQuoteUnitPrice;
        private Label lblQuoteQty;
        private NumericUpDown numQuoteQty;
        private Button btnAddQuote;
        private Label lblQuoteMsg;
        private Label lblCheapestQuote;
        private DataGridView dgvQuotes;
        private Button btnDeleteQuote;
        private Button btnApprovePurchase;
        private Label lblApproverNote;
        private Label lblPurchases;
        private DataGridView dgvPurchases;
        private DataGridViewTextBoxColumn colQVendor;
        private DataGridViewTextBoxColumn colQDate;
        private DataGridViewTextBoxColumn colQProduct;
        private DataGridViewTextBoxColumn colQUnitPrice;
        private DataGridViewTextBoxColumn colQQty;
        private DataGridViewTextBoxColumn colQTotal;
        private DataGridViewTextBoxColumn colQStatus;
        private DataGridViewTextBoxColumn colPDate;
        private DataGridViewTextBoxColumn colPVendor;
        private DataGridViewTextBoxColumn colPMaterial;
        private DataGridViewTextBoxColumn colPQty;
        private DataGridViewTextBoxColumn colPTotal;
        private DataGridViewTextBoxColumn colPDelivered;
        private DataGridViewCheckBoxColumn colPDone;
    }
}
