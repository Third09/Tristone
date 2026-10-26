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
            lblCompareMaterial.Location = new Point(12, 15);
            lblCompareMaterial.Name = "lblCompareMaterial";
            lblCompareMaterial.Text = "Material to buy";
            // 
            // cboCompareMaterial
            // 
            cboCompareMaterial.Location = new Point(110, 12);
            cboCompareMaterial.Name = "cboCompareMaterial";
            cboCompareMaterial.Size = new Size(260, 23);
            cboCompareMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCompareMaterial.TabIndex = 0;
            cboCompareMaterial.SelectedIndexChanged += cboCompareMaterial_SelectedIndexChanged;
            // 
            // lblAddQuote
            // 
            lblAddQuote.AutoSize = true;
            lblAddQuote.Location = new Point(12, 46);
            lblAddQuote.Name = "lblAddQuote";
            lblAddQuote.Text = "Add a quote";
            // 
            // lblQuoteVendor
            // 
            lblQuoteVendor.AutoSize = true;
            lblQuoteVendor.Location = new Point(12, 66);
            lblQuoteVendor.Name = "lblQuoteVendor";
            lblQuoteVendor.Text = "Vendor";
            // 
            // cboQuoteVendor
            // 
            cboQuoteVendor.Location = new Point(12, 84);
            cboQuoteVendor.Name = "cboQuoteVendor";
            cboQuoteVendor.Size = new Size(200, 23);
            cboQuoteVendor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuoteVendor.TabIndex = 1;
            // 
            // lblQuoteDate
            // 
            lblQuoteDate.AutoSize = true;
            lblQuoteDate.Location = new Point(224, 66);
            lblQuoteDate.Name = "lblQuoteDate";
            lblQuoteDate.Text = "Date inputted";
            // 
            // dtpQuoteDate
            // 
            dtpQuoteDate.Location = new Point(224, 84);
            dtpQuoteDate.Name = "dtpQuoteDate";
            dtpQuoteDate.Size = new Size(120, 23);
            dtpQuoteDate.Format = DateTimePickerFormat.Short;
            dtpQuoteDate.TabIndex = 2;
            // 
            // lblQuoteProduct
            // 
            lblQuoteProduct.AutoSize = true;
            lblQuoteProduct.Location = new Point(356, 66);
            lblQuoteProduct.Name = "lblQuoteProduct";
            lblQuoteProduct.Text = "Product name";
            // 
            // txtQuoteProduct
            // 
            txtQuoteProduct.Location = new Point(356, 84);
            txtQuoteProduct.Name = "txtQuoteProduct";
            txtQuoteProduct.Size = new Size(200, 23);
            txtQuoteProduct.TabIndex = 3;
            // 
            // lblQuoteCategory
            // 
            lblQuoteCategory.AutoSize = true;
            lblQuoteCategory.Location = new Point(568, 66);
            lblQuoteCategory.Name = "lblQuoteCategory";
            lblQuoteCategory.Text = "Category";
            // 
            // cboQuoteCategory
            // 
            cboQuoteCategory.Location = new Point(568, 84);
            cboQuoteCategory.Name = "cboQuoteCategory";
            cboQuoteCategory.Size = new Size(140, 23);
            cboQuoteCategory.TabIndex = 4;
            // 
            // lblQuoteDescription
            // 
            lblQuoteDescription.AutoSize = true;
            lblQuoteDescription.Location = new Point(12, 114);
            lblQuoteDescription.Name = "lblQuoteDescription";
            lblQuoteDescription.Text = "Description";
            // 
            // txtQuoteDescription
            // 
            txtQuoteDescription.Location = new Point(12, 132);
            txtQuoteDescription.Name = "txtQuoteDescription";
            txtQuoteDescription.Size = new Size(332, 23);
            txtQuoteDescription.TabIndex = 5;
            // 
            // lblQuoteUnitPrice
            // 
            lblQuoteUnitPrice.AutoSize = true;
            lblQuoteUnitPrice.Location = new Point(356, 114);
            lblQuoteUnitPrice.Name = "lblQuoteUnitPrice";
            lblQuoteUnitPrice.Text = "Unit price";
            // 
            // numQuoteUnitPrice
            // 
            numQuoteUnitPrice.Location = new Point(356, 132);
            numQuoteUnitPrice.Name = "numQuoteUnitPrice";
            numQuoteUnitPrice.Size = new Size(120, 23);
            numQuoteUnitPrice.DecimalPlaces = 2;
            numQuoteUnitPrice.ThousandsSeparator = true;
            numQuoteUnitPrice.Maximum = 999999999m;
            numQuoteUnitPrice.TabIndex = 6;
            // 
            // lblQuoteQty
            // 
            lblQuoteQty.AutoSize = true;
            lblQuoteQty.Location = new Point(488, 114);
            lblQuoteQty.Name = "lblQuoteQty";
            lblQuoteQty.Text = "Quantity";
            // 
            // numQuoteQty
            // 
            numQuoteQty.Location = new Point(488, 132);
            numQuoteQty.Name = "numQuoteQty";
            numQuoteQty.Size = new Size(100, 23);
            numQuoteQty.DecimalPlaces = 2;
            numQuoteQty.ThousandsSeparator = true;
            numQuoteQty.Maximum = 999999999m;
            numQuoteQty.TabIndex = 7;
            // 
            // btnAddQuote
            // 
            btnAddQuote.Location = new Point(600, 130);
            btnAddQuote.Name = "btnAddQuote";
            btnAddQuote.Size = new Size(108, 28);
            btnAddQuote.Text = "Add Quote";
            btnAddQuote.TabIndex = 8;
            btnAddQuote.Click += btnAddQuote_Click;
            // 
            // lblQuoteMsg
            // 
            lblQuoteMsg.AutoSize = true;
            lblQuoteMsg.Location = new Point(12, 162);
            lblQuoteMsg.Name = "lblQuoteMsg";
            // 
            // lblCheapestQuote
            // 
            lblCheapestQuote.AutoSize = true;
            lblCheapestQuote.Location = new Point(12, 190);
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
            dgvQuotes.Location = new Point(12, 210);
            dgvQuotes.Name = "dgvQuotes";
            dgvQuotes.Size = new Size(930, 180);
            dgvQuotes.TabIndex = 9;
            dgvQuotes.SelectionChanged += dgvQuotes_SelectionChanged;
            dgvQuotes.CellFormatting += dgvQuotes_CellFormatting;
            // 
            // btnDeleteQuote
            // 
            btnDeleteQuote.Location = new Point(12, 398);
            btnDeleteQuote.Name = "btnDeleteQuote";
            btnDeleteQuote.Size = new Size(110, 28);
            btnDeleteQuote.Text = "Delete Quote";
            btnDeleteQuote.TabIndex = 10;
            btnDeleteQuote.Click += btnDeleteQuote_Click;
            // 
            // btnApprovePurchase
            // 
            btnApprovePurchase.Location = new Point(128, 398);
            btnApprovePurchase.Name = "btnApprovePurchase";
            btnApprovePurchase.Size = new Size(160, 28);
            btnApprovePurchase.Text = "Approve as Purchase";
            btnApprovePurchase.TabIndex = 11;
            btnApprovePurchase.Click += btnApprovePurchase_Click;
            // 
            // lblApproverNote
            // 
            lblApproverNote.AutoSize = true;
            lblApproverNote.Location = new Point(300, 404);
            lblApproverNote.Name = "lblApproverNote";
            lblApproverNote.Text = "Approve as Purchase: Inventory Supervisor or Admin only";
            // 
            // lblPurchases
            // 
            lblPurchases.AutoSize = true;
            lblPurchases.Location = new Point(12, 444);
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
            dgvPurchases.Location = new Point(12, 464);
            dgvPurchases.Name = "dgvPurchases";
            dgvPurchases.Size = new Size(930, 180);
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
            Size = new Size(960, 700);
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
