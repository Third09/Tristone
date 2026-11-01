namespace Company.OfficeApp.Pages.Inventory
{
    partial class MaterialsTab
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
            txtMaterialSearch = new TextBox();
            lblCategoryFilter = new Label();
            cboMaterialCategoryFilter = new ComboBox();
            chkLowStockOnly = new CheckBox();
            lblLowStockCount = new Label();
            dgvMaterials = new DataGridView();
            btnAddMaterial = new Button();
            btnDeleteMaterial = new Button();
            btnExportMaterials = new Button();
            lblFormMode = new Label();
            lblMaterialName = new Label();
            txtMaterialName = new TextBox();
            lblMaterialCategory = new Label();
            cboMaterialCategory = new ComboBox();
            lblMaterialUnit = new Label();
            cboMaterialUnit = new ComboBox();
            lblMaterialPrice = new Label();
            numMaterialPrice = new NumericUpDown();
            lblStartingStock = new Label();
            numStartingStock = new NumericUpDown();
            lblCurrentStock = new Label();
            lblMinimumStock = new Label();
            numMinimumStock = new NumericUpDown();
            lblMaterialMsg = new Label();
            btnSaveMaterial = new Button();
            btnCancelMaterial = new Button();
            colName = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colUnit = new DataGridViewTextBoxColumn();
            colPrice = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colMinimum = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvMaterials).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaterialPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStartingStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinimumStock).BeginInit();
            SuspendLayout();
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(38, 36);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search";
            // 
            // txtMaterialSearch
            // 
            txtMaterialSearch.Location = new Point(95, 32);
            txtMaterialSearch.Name = "txtMaterialSearch";
            txtMaterialSearch.Size = new Size(207, 23);
            txtMaterialSearch.TabIndex = 0;
            // 
            // lblCategoryFilter
            // 
            lblCategoryFilter.AutoSize = true;
            lblCategoryFilter.Location = new Point(318, 36);
            lblCategoryFilter.Name = "lblCategoryFilter";
            lblCategoryFilter.Text = "Category";
            // 
            // cboMaterialCategoryFilter
            // 
            cboMaterialCategoryFilter.Location = new Point(390, 32);
            cboMaterialCategoryFilter.Name = "cboMaterialCategoryFilter";
            cboMaterialCategoryFilter.Size = new Size(161, 23);
            cboMaterialCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaterialCategoryFilter.TabIndex = 1;
            cboMaterialCategoryFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // chkLowStockOnly
            // 
            chkLowStockOnly.AutoSize = true;
            chkLowStockOnly.Location = new Point(567, 34);
            chkLowStockOnly.Name = "chkLowStockOnly";
            chkLowStockOnly.Text = "Low stock only";
            chkLowStockOnly.TabIndex = 2;
            chkLowStockOnly.CheckedChanged += Filter_Changed;
            // 
            // lblLowStockCount
            // 
            lblLowStockCount.AutoSize = true;
            lblLowStockCount.Location = new Point(714, 36);
            lblLowStockCount.Name = "lblLowStockCount";
            lblLowStockCount.Text = "";
            // 
            // dgvMaterials
            // 
            dgvMaterials.AllowUserToAddRows = false;
            dgvMaterials.AllowUserToDeleteRows = false;
            dgvMaterials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMaterials.Columns.AddRange(new DataGridViewColumn[] { colName, colCategory, colUnit, colPrice, colStock, colMinimum });
            dgvMaterials.MultiSelect = false;
            dgvMaterials.ReadOnly = true;
            dgvMaterials.RowHeadersVisible = false;
            dgvMaterials.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaterials.Location = new Point(38, 74);
            dgvMaterials.Name = "dgvMaterials";
            dgvMaterials.Size = new Size(1070, 290);
            dgvMaterials.TabIndex = 3;
            dgvMaterials.SelectionChanged += dgvMaterials_SelectionChanged;
            dgvMaterials.CellFormatting += dgvMaterials_CellFormatting;
            // 
            // btnAddMaterial
            // 
            btnAddMaterial.Location = new Point(38, 375);
            btnAddMaterial.Name = "btnAddMaterial";
            btnAddMaterial.Size = new Size(126, 36);
            btnAddMaterial.Text = "Add Material";
            btnAddMaterial.TabIndex = 4;
            btnAddMaterial.Click += btnAddMaterial_Click;
            // 
            // btnDeleteMaterial
            // 
            btnDeleteMaterial.Location = new Point(171, 375);
            btnDeleteMaterial.Name = "btnDeleteMaterial";
            btnDeleteMaterial.Size = new Size(103, 36);
            btnDeleteMaterial.Text = "Delete";
            btnDeleteMaterial.TabIndex = 5;
            btnDeleteMaterial.Click += btnDeleteMaterial_Click;
            // 
            // btnExportMaterials
            // 
            btnExportMaterials.Location = new Point(282, 375);
            btnExportMaterials.Name = "btnExportMaterials";
            btnExportMaterials.Size = new Size(103, 36);
            btnExportMaterials.Text = "Export";
            btnExportMaterials.TabIndex = 6;
            btnExportMaterials.Click += btnExportMaterials_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(38, 433);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New material";
            lblFormMode.Font = new Font("Segoe UI Semibold", 11.5F);
            // 
            // lblMaterialName
            // 
            lblMaterialName.AutoSize = true;
            lblMaterialName.Location = new Point(38, 465);
            lblMaterialName.Name = "lblMaterialName";
            lblMaterialName.Text = "Name (e.g. Portland Cement 40kg)";
            // 
            // txtMaterialName
            // 
            txtMaterialName.Location = new Point(38, 489);
            txtMaterialName.Name = "txtMaterialName";
            txtMaterialName.Size = new Size(345, 23);
            txtMaterialName.TabIndex = 7;
            // 
            // lblMaterialCategory
            // 
            lblMaterialCategory.AutoSize = true;
            lblMaterialCategory.Location = new Point(38, 528);
            lblMaterialCategory.Name = "lblMaterialCategory";
            lblMaterialCategory.Text = "Category (pick or type)";
            // 
            // cboMaterialCategory
            // 
            cboMaterialCategory.Location = new Point(38, 552);
            cboMaterialCategory.Name = "cboMaterialCategory";
            cboMaterialCategory.Size = new Size(230, 23);
            cboMaterialCategory.TabIndex = 8;
            // 
            // lblMaterialUnit
            // 
            lblMaterialUnit.AutoSize = true;
            lblMaterialUnit.Location = new Point(38, 592);
            lblMaterialUnit.Name = "lblMaterialUnit";
            lblMaterialUnit.Text = "Unit (pick or type)";
            // 
            // cboMaterialUnit
            // 
            cboMaterialUnit.Location = new Point(38, 615);
            cboMaterialUnit.Name = "cboMaterialUnit";
            cboMaterialUnit.Size = new Size(230, 23);
            cboMaterialUnit.TabIndex = 9;
            // 
            // lblMaterialPrice
            // 
            lblMaterialPrice.AutoSize = true;
            lblMaterialPrice.Location = new Point(415, 465);
            lblMaterialPrice.Name = "lblMaterialPrice";
            lblMaterialPrice.Text = "Unit price (pesos)";
            // 
            // numMaterialPrice
            // 
            numMaterialPrice.Location = new Point(415, 489);
            numMaterialPrice.Name = "numMaterialPrice";
            numMaterialPrice.Size = new Size(161, 23);
            numMaterialPrice.DecimalPlaces = 2;
            numMaterialPrice.ThousandsSeparator = true;
            numMaterialPrice.Maximum = 999999999m;
            numMaterialPrice.TabIndex = 10;
            // 
            // lblStartingStock
            // 
            lblStartingStock.AutoSize = true;
            lblStartingStock.Location = new Point(415, 528);
            lblStartingStock.Name = "lblStartingStock";
            lblStartingStock.Text = "Starting stock (new materials only)";
            // 
            // numStartingStock
            // 
            numStartingStock.Location = new Point(415, 552);
            numStartingStock.Name = "numStartingStock";
            numStartingStock.Size = new Size(161, 23);
            numStartingStock.DecimalPlaces = 2;
            numStartingStock.ThousandsSeparator = true;
            numStartingStock.Maximum = 999999999m;
            numStartingStock.TabIndex = 11;
            // 
            // lblCurrentStock
            // 
            lblCurrentStock.AutoSize = true;
            lblCurrentStock.Location = new Point(415, 594);
            lblCurrentStock.Name = "lblCurrentStock";
            lblCurrentStock.Text = "Current stock: -";
            // 
            // lblMinimumStock
            // 
            lblMinimumStock.AutoSize = true;
            lblMinimumStock.Location = new Point(415, 623);
            lblMinimumStock.Name = "lblMinimumStock";
            lblMinimumStock.Text = "Minimum stock (alert below this)";
            // 
            // numMinimumStock
            // 
            numMinimumStock.Location = new Point(415, 647);
            numMinimumStock.Name = "numMinimumStock";
            numMinimumStock.Size = new Size(161, 23);
            numMinimumStock.DecimalPlaces = 2;
            numMinimumStock.ThousandsSeparator = true;
            numMinimumStock.Maximum = 999999999m;
            numMinimumStock.TabIndex = 12;
            // 
            // lblMaterialMsg
            // 
            lblMaterialMsg.AutoSize = true;
            lblMaterialMsg.Location = new Point(38, 697);
            lblMaterialMsg.Name = "lblMaterialMsg";
            // 
            // btnSaveMaterial
            // 
            btnSaveMaterial.Location = new Point(38, 726);
            btnSaveMaterial.Name = "btnSaveMaterial";
            btnSaveMaterial.Size = new Size(103, 36);
            btnSaveMaterial.Text = "Save";
            btnSaveMaterial.TabIndex = 13;
            btnSaveMaterial.Click += btnSaveMaterial_Click;
            // 
            // btnCancelMaterial
            // 
            btnCancelMaterial.Location = new Point(148, 726);
            btnCancelMaterial.Name = "btnCancelMaterial";
            btnCancelMaterial.Size = new Size(103, 36);
            btnCancelMaterial.Text = "Cancel";
            btnCancelMaterial.TabIndex = 14;
            btnCancelMaterial.Click += btnCancelMaterial_Click;
            // 
            // colName
            // 
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            // 
            // colCategory
            // 
            colCategory.DataPropertyName = "Category";
            colCategory.HeaderText = "Category";
            colCategory.Name = "colCategory";
            // 
            // colUnit
            // 
            colUnit.DataPropertyName = "Unit";
            colUnit.HeaderText = "Unit";
            colUnit.Name = "colUnit";
            // 
            // colPrice
            // 
            colPrice.DataPropertyName = "UnitPrice";
            colPrice.HeaderText = "Unit Price";
            colPrice.Name = "colPrice";
            colPrice.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colStock
            // 
            colStock.DataPropertyName = "StockQty";
            colStock.HeaderText = "Stock";
            colStock.Name = "colStock";
            colStock.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" };
            // 
            // colMinimum
            // 
            colMinimum.DataPropertyName = "MinimumStock";
            colMinimum.HeaderText = "Minimum";
            colMinimum.Name = "colMinimum";
            colMinimum.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" };
            // 
            // MaterialsTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblSearch);
            Controls.Add(txtMaterialSearch);
            Controls.Add(lblCategoryFilter);
            Controls.Add(cboMaterialCategoryFilter);
            Controls.Add(chkLowStockOnly);
            Controls.Add(lblLowStockCount);
            Controls.Add(dgvMaterials);
            Controls.Add(btnAddMaterial);
            Controls.Add(btnDeleteMaterial);
            Controls.Add(btnExportMaterials);
            Controls.Add(lblFormMode);
            Controls.Add(lblMaterialName);
            Controls.Add(txtMaterialName);
            Controls.Add(lblMaterialCategory);
            Controls.Add(cboMaterialCategory);
            Controls.Add(lblMaterialUnit);
            Controls.Add(cboMaterialUnit);
            Controls.Add(lblMaterialPrice);
            Controls.Add(numMaterialPrice);
            Controls.Add(lblStartingStock);
            Controls.Add(numStartingStock);
            Controls.Add(lblCurrentStock);
            Controls.Add(lblMinimumStock);
            Controls.Add(numMinimumStock);
            Controls.Add(lblMaterialMsg);
            Controls.Add(btnSaveMaterial);
            Controls.Add(btnCancelMaterial);
            Name = "MaterialsTab";
            Size = new Size(1152, 877);
            Load += MaterialsTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMaterials).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaterialPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStartingStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinimumStock).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSearch;
        private TextBox txtMaterialSearch;
        private Label lblCategoryFilter;
        private ComboBox cboMaterialCategoryFilter;
        private CheckBox chkLowStockOnly;
        private Label lblLowStockCount;
        private DataGridView dgvMaterials;
        private Button btnAddMaterial;
        private Button btnDeleteMaterial;
        private Button btnExportMaterials;
        private Label lblFormMode;
        private Label lblMaterialName;
        private TextBox txtMaterialName;
        private Label lblMaterialCategory;
        private ComboBox cboMaterialCategory;
        private Label lblMaterialUnit;
        private ComboBox cboMaterialUnit;
        private Label lblMaterialPrice;
        private NumericUpDown numMaterialPrice;
        private Label lblStartingStock;
        private NumericUpDown numStartingStock;
        private Label lblCurrentStock;
        private Label lblMinimumStock;
        private NumericUpDown numMinimumStock;
        private Label lblMaterialMsg;
        private Button btnSaveMaterial;
        private Button btnCancelMaterial;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colUnit;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colMinimum;
    }
}
