namespace Company.OfficeApp.Pages.Inventory
{
    partial class UsageTab
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
            lblUsageProject = new Label();
            cboUsageProject = new ComboBox();
            lblUsageMaterial = new Label();
            cboUsageMaterial = new ComboBox();
            lblAvailableStock = new Label();
            lblUsageQty = new Label();
            numUsageQty = new NumericUpDown();
            lblUsageDate = new Label();
            dtpUsageDate = new DateTimePicker();
            lblUsageNotes = new Label();
            txtUsageNotes = new TextBox();
            btnSubmitUsage = new Button();
            lblUsageMsg = new Label();
            lblStatusFilter = new Label();
            cboUsageStatusFilter = new ComboBox();
            dgvUsageRequests = new DataGridView();
            btnApproveUsage = new Button();
            btnRejectUsage = new Button();
            lblApproverNote = new Label();
            lblHistoryTitle = new Label();
            cboHistoryProject = new ComboBox();
            lblProjectMaterialTotal = new Label();
            btnExportProjectMaterials = new Button();
            dgvProjectMaterials = new DataGridView();
            colDate = new DataGridViewTextBoxColumn();
            colProject = new DataGridViewTextBoxColumn();
            colMaterial = new DataGridViewTextBoxColumn();
            colQty = new DataGridViewTextBoxColumn();
            colUnit = new DataGridViewTextBoxColumn();
            colCost = new DataGridViewTextBoxColumn();
            colRequestedBy = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colHDate = new DataGridViewTextBoxColumn();
            colHMaterial = new DataGridViewTextBoxColumn();
            colHQty = new DataGridViewTextBoxColumn();
            colHUnit = new DataGridViewTextBoxColumn();
            colHCost = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)numUsageQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvUsageRequests).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProjectMaterials).BeginInit();
            SuspendLayout();
            // 
            // lblRecordTitle
            // 
            lblRecordTitle.AutoSize = true;
            lblRecordTitle.Location = new Point(12, 12);
            lblRecordTitle.Name = "lblRecordTitle";
            lblRecordTitle.Text = "Record material used on a project (waits for Supervisor approval)";
            // 
            // lblUsageProject
            // 
            lblUsageProject.AutoSize = true;
            lblUsageProject.Location = new Point(12, 36);
            lblUsageProject.Name = "lblUsageProject";
            lblUsageProject.Text = "Project";
            // 
            // cboUsageProject
            // 
            cboUsageProject.Location = new Point(12, 54);
            cboUsageProject.Name = "cboUsageProject";
            cboUsageProject.Size = new Size(240, 23);
            cboUsageProject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUsageProject.TabIndex = 0;
            // 
            // lblUsageMaterial
            // 
            lblUsageMaterial.AutoSize = true;
            lblUsageMaterial.Location = new Point(264, 36);
            lblUsageMaterial.Name = "lblUsageMaterial";
            lblUsageMaterial.Text = "Material";
            // 
            // cboUsageMaterial
            // 
            cboUsageMaterial.Location = new Point(264, 54);
            cboUsageMaterial.Name = "cboUsageMaterial";
            cboUsageMaterial.Size = new Size(240, 23);
            cboUsageMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUsageMaterial.TabIndex = 1;
            cboUsageMaterial.SelectedIndexChanged += cboUsageMaterial_SelectedIndexChanged;
            // 
            // lblAvailableStock
            // 
            lblAvailableStock.AutoSize = true;
            lblAvailableStock.Location = new Point(516, 57);
            lblAvailableStock.Name = "lblAvailableStock";
            lblAvailableStock.Text = "In stock: -";
            // 
            // lblUsageQty
            // 
            lblUsageQty.AutoSize = true;
            lblUsageQty.Location = new Point(12, 84);
            lblUsageQty.Name = "lblUsageQty";
            lblUsageQty.Text = "Quantity used";
            // 
            // numUsageQty
            // 
            numUsageQty.Location = new Point(12, 102);
            numUsageQty.Name = "numUsageQty";
            numUsageQty.Size = new Size(120, 23);
            numUsageQty.DecimalPlaces = 2;
            numUsageQty.Maximum = 999999999m;
            numUsageQty.TabIndex = 2;
            // 
            // lblUsageDate
            // 
            lblUsageDate.AutoSize = true;
            lblUsageDate.Location = new Point(144, 84);
            lblUsageDate.Name = "lblUsageDate";
            lblUsageDate.Text = "Date used";
            // 
            // dtpUsageDate
            // 
            dtpUsageDate.Location = new Point(144, 102);
            dtpUsageDate.Name = "dtpUsageDate";
            dtpUsageDate.Size = new Size(120, 23);
            dtpUsageDate.Format = DateTimePickerFormat.Short;
            dtpUsageDate.TabIndex = 3;
            // 
            // lblUsageNotes
            // 
            lblUsageNotes.AutoSize = true;
            lblUsageNotes.Location = new Point(276, 84);
            lblUsageNotes.Name = "lblUsageNotes";
            lblUsageNotes.Text = "Notes (optional)";
            // 
            // txtUsageNotes
            // 
            txtUsageNotes.Location = new Point(276, 102);
            txtUsageNotes.Name = "txtUsageNotes";
            txtUsageNotes.Size = new Size(300, 23);
            txtUsageNotes.TabIndex = 4;
            // 
            // btnSubmitUsage
            // 
            btnSubmitUsage.Location = new Point(588, 100);
            btnSubmitUsage.Name = "btnSubmitUsage";
            btnSubmitUsage.Size = new Size(90, 28);
            btnSubmitUsage.Text = "Submit";
            btnSubmitUsage.TabIndex = 5;
            btnSubmitUsage.Click += btnSubmitUsage_Click;
            // 
            // lblUsageMsg
            // 
            lblUsageMsg.AutoSize = true;
            lblUsageMsg.Location = new Point(12, 132);
            lblUsageMsg.Name = "lblUsageMsg";
            // 
            // lblStatusFilter
            // 
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(12, 160);
            lblStatusFilter.Name = "lblStatusFilter";
            lblStatusFilter.Text = "Show";
            // 
            // cboUsageStatusFilter
            // 
            cboUsageStatusFilter.Location = new Point(52, 157);
            cboUsageStatusFilter.Name = "cboUsageStatusFilter";
            cboUsageStatusFilter.Size = new Size(160, 23);
            cboUsageStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUsageStatusFilter.TabIndex = 6;
            cboUsageStatusFilter.SelectedIndexChanged += cboUsageStatusFilter_SelectedIndexChanged;
            // 
            // dgvUsageRequests
            // 
            dgvUsageRequests.AllowUserToAddRows = false;
            dgvUsageRequests.AllowUserToDeleteRows = false;
            dgvUsageRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsageRequests.Columns.AddRange(new DataGridViewColumn[] { colDate, colProject, colMaterial, colQty, colUnit, colCost, colRequestedBy, colStatus });
            dgvUsageRequests.MultiSelect = false;
            dgvUsageRequests.ReadOnly = true;
            dgvUsageRequests.RowHeadersVisible = false;
            dgvUsageRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsageRequests.Location = new Point(12, 188);
            dgvUsageRequests.Name = "dgvUsageRequests";
            dgvUsageRequests.Size = new Size(930, 200);
            dgvUsageRequests.TabIndex = 7;
            dgvUsageRequests.SelectionChanged += dgvUsageRequests_SelectionChanged;
            // 
            // btnApproveUsage
            // 
            btnApproveUsage.Location = new Point(12, 396);
            btnApproveUsage.Name = "btnApproveUsage";
            btnApproveUsage.Size = new Size(90, 28);
            btnApproveUsage.Text = "Approve";
            btnApproveUsage.TabIndex = 8;
            btnApproveUsage.Click += btnApproveUsage_Click;
            // 
            // btnRejectUsage
            // 
            btnRejectUsage.Location = new Point(108, 396);
            btnRejectUsage.Name = "btnRejectUsage";
            btnRejectUsage.Size = new Size(90, 28);
            btnRejectUsage.Text = "Reject";
            btnRejectUsage.TabIndex = 9;
            btnRejectUsage.Click += btnRejectUsage_Click;
            // 
            // lblApproverNote
            // 
            lblApproverNote.AutoSize = true;
            lblApproverNote.Location = new Point(210, 402);
            lblApproverNote.Name = "lblApproverNote";
            lblApproverNote.Text = "Approve / Reject: Inventory Supervisor or Admin only";
            // 
            // lblHistoryTitle
            // 
            lblHistoryTitle.AutoSize = true;
            lblHistoryTitle.Location = new Point(12, 440);
            lblHistoryTitle.Name = "lblHistoryTitle";
            lblHistoryTitle.Text = "Project material history (approved only)";
            // 
            // cboHistoryProject
            // 
            cboHistoryProject.Location = new Point(12, 460);
            cboHistoryProject.Name = "cboHistoryProject";
            cboHistoryProject.Size = new Size(240, 23);
            cboHistoryProject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHistoryProject.TabIndex = 10;
            cboHistoryProject.SelectedIndexChanged += cboHistoryProject_SelectedIndexChanged;
            // 
            // lblProjectMaterialTotal
            // 
            lblProjectMaterialTotal.AutoSize = true;
            lblProjectMaterialTotal.Location = new Point(264, 463);
            lblProjectMaterialTotal.Name = "lblProjectMaterialTotal";
            lblProjectMaterialTotal.Text = "Total: -";
            // 
            // btnExportProjectMaterials
            // 
            btnExportProjectMaterials.Location = new Point(852, 458);
            btnExportProjectMaterials.Name = "btnExportProjectMaterials";
            btnExportProjectMaterials.Size = new Size(90, 28);
            btnExportProjectMaterials.Text = "Export";
            btnExportProjectMaterials.TabIndex = 11;
            btnExportProjectMaterials.Click += btnExportProjectMaterials_Click;
            // 
            // dgvProjectMaterials
            // 
            dgvProjectMaterials.AllowUserToAddRows = false;
            dgvProjectMaterials.AllowUserToDeleteRows = false;
            dgvProjectMaterials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProjectMaterials.Columns.AddRange(new DataGridViewColumn[] { colHDate, colHMaterial, colHQty, colHUnit, colHCost });
            dgvProjectMaterials.MultiSelect = false;
            dgvProjectMaterials.ReadOnly = true;
            dgvProjectMaterials.RowHeadersVisible = false;
            dgvProjectMaterials.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProjectMaterials.Location = new Point(12, 490);
            dgvProjectMaterials.Name = "dgvProjectMaterials";
            dgvProjectMaterials.Size = new Size(930, 160);
            dgvProjectMaterials.TabIndex = 12;
            // 
            // colDate
            // 
            colDate.DataPropertyName = "UsedOn";
            colDate.HeaderText = "Date";
            colDate.Name = "colDate";
            colDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colProject
            // 
            colProject.DataPropertyName = "Project";
            colProject.HeaderText = "Project";
            colProject.Name = "colProject";
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
            // colUnit
            // 
            colUnit.DataPropertyName = "Unit";
            colUnit.HeaderText = "Unit";
            colUnit.Name = "colUnit";
            // 
            // colCost
            // 
            colCost.DataPropertyName = "Cost";
            colCost.HeaderText = "Cost";
            colCost.Name = "colCost";
            colCost.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colRequestedBy
            // 
            colRequestedBy.DataPropertyName = "RequestedBy";
            colRequestedBy.HeaderText = "Requested By";
            colRequestedBy.Name = "colRequestedBy";
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            // 
            // colHDate
            // 
            colHDate.DataPropertyName = "UsedOn";
            colHDate.HeaderText = "Date";
            colHDate.Name = "colHDate";
            colHDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colHMaterial
            // 
            colHMaterial.DataPropertyName = "Material";
            colHMaterial.HeaderText = "Material";
            colHMaterial.Name = "colHMaterial";
            // 
            // colHQty
            // 
            colHQty.DataPropertyName = "Quantity";
            colHQty.HeaderText = "Qty";
            colHQty.Name = "colHQty";
            colHQty.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" };
            // 
            // colHUnit
            // 
            colHUnit.DataPropertyName = "Unit";
            colHUnit.HeaderText = "Unit";
            colHUnit.Name = "colHUnit";
            // 
            // colHCost
            // 
            colHCost.DataPropertyName = "Cost";
            colHCost.HeaderText = "Cost";
            colHCost.Name = "colHCost";
            colHCost.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // UsageTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblRecordTitle);
            Controls.Add(lblUsageProject);
            Controls.Add(cboUsageProject);
            Controls.Add(lblUsageMaterial);
            Controls.Add(cboUsageMaterial);
            Controls.Add(lblAvailableStock);
            Controls.Add(lblUsageQty);
            Controls.Add(numUsageQty);
            Controls.Add(lblUsageDate);
            Controls.Add(dtpUsageDate);
            Controls.Add(lblUsageNotes);
            Controls.Add(txtUsageNotes);
            Controls.Add(btnSubmitUsage);
            Controls.Add(lblUsageMsg);
            Controls.Add(lblStatusFilter);
            Controls.Add(cboUsageStatusFilter);
            Controls.Add(dgvUsageRequests);
            Controls.Add(btnApproveUsage);
            Controls.Add(btnRejectUsage);
            Controls.Add(lblApproverNote);
            Controls.Add(lblHistoryTitle);
            Controls.Add(cboHistoryProject);
            Controls.Add(lblProjectMaterialTotal);
            Controls.Add(btnExportProjectMaterials);
            Controls.Add(dgvProjectMaterials);
            Name = "UsageTab";
            Size = new Size(960, 700);
            Load += UsageTab_Load;
            ((System.ComponentModel.ISupportInitialize)numUsageQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvUsageRequests).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProjectMaterials).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRecordTitle;
        private Label lblUsageProject;
        private ComboBox cboUsageProject;
        private Label lblUsageMaterial;
        private ComboBox cboUsageMaterial;
        private Label lblAvailableStock;
        private Label lblUsageQty;
        private NumericUpDown numUsageQty;
        private Label lblUsageDate;
        private DateTimePicker dtpUsageDate;
        private Label lblUsageNotes;
        private TextBox txtUsageNotes;
        private Button btnSubmitUsage;
        private Label lblUsageMsg;
        private Label lblStatusFilter;
        private ComboBox cboUsageStatusFilter;
        private DataGridView dgvUsageRequests;
        private Button btnApproveUsage;
        private Button btnRejectUsage;
        private Label lblApproverNote;
        private Label lblHistoryTitle;
        private ComboBox cboHistoryProject;
        private Label lblProjectMaterialTotal;
        private Button btnExportProjectMaterials;
        private DataGridView dgvProjectMaterials;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colProject;
        private DataGridViewTextBoxColumn colMaterial;
        private DataGridViewTextBoxColumn colQty;
        private DataGridViewTextBoxColumn colUnit;
        private DataGridViewTextBoxColumn colCost;
        private DataGridViewTextBoxColumn colRequestedBy;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colHDate;
        private DataGridViewTextBoxColumn colHMaterial;
        private DataGridViewTextBoxColumn colHQty;
        private DataGridViewTextBoxColumn colHUnit;
        private DataGridViewTextBoxColumn colHCost;
    }
}
