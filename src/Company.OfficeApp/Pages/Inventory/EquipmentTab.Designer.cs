namespace Company.OfficeApp.Pages.Inventory
{
    partial class EquipmentTab
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
            txtEquipmentSearch = new TextBox();
            lblTypeFilter = new Label();
            cboEquipmentTypeFilter = new ComboBox();
            lblStatusFilter = new Label();
            cboEquipmentStatusFilter = new ComboBox();
            lblMaintenanceDue = new Label();
            dgvEquipment = new DataGridView();
            btnAddEquipment = new Button();
            btnDeleteEquipment = new Button();
            btnExportEquipment = new Button();
            lblFormMode = new Label();
            lblEquipmentName = new Label();
            txtEquipmentName = new TextBox();
            lblEquipmentType = new Label();
            cboEquipmentType = new ComboBox();
            rdoOwned = new RadioButton();
            rdoRented = new RadioButton();
            lblRentalCompany = new Label();
            txtRentalCompany = new TextBox();
            lblRentalRate = new Label();
            numRentalRate = new NumericUpDown();
            cboRentalRateUnit = new ComboBox();
            lblRentalReturn = new Label();
            dtpRentalReturn = new DateTimePicker();
            lblEquipmentStatus = new Label();
            cboEquipmentStatus = new ComboBox();
            lblNextMaintenance = new Label();
            dtpNextMaintenance = new DateTimePicker();
            lblEquipmentMsg = new Label();
            btnSaveEquipment = new Button();
            btnCancelEquipment = new Button();
            lblLogTitle = new Label();
            dgvMaintenanceLog = new DataGridView();
            btnAddMaintenance = new Button();
            btnDeleteMaintenance = new Button();
            colName = new DataGridViewTextBoxColumn();
            colType = new DataGridViewTextBoxColumn();
            colOwnership = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colNext = new DataGridViewTextBoxColumn();
            colLDate = new DataGridViewTextBoxColumn();
            colLKind = new DataGridViewTextBoxColumn();
            colLWork = new DataGridViewTextBoxColumn();
            colLCost = new DataGridViewTextBoxColumn();
            colLBy = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvEquipment).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRentalRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMaintenanceLog).BeginInit();
            SuspendLayout();
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(12, 15);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search";
            // 
            // txtEquipmentSearch
            // 
            txtEquipmentSearch.Location = new Point(62, 12);
            txtEquipmentSearch.Name = "txtEquipmentSearch";
            txtEquipmentSearch.Size = new Size(160, 23);
            txtEquipmentSearch.TabIndex = 0;
            // 
            // lblTypeFilter
            // 
            lblTypeFilter.AutoSize = true;
            lblTypeFilter.Location = new Point(234, 15);
            lblTypeFilter.Name = "lblTypeFilter";
            lblTypeFilter.Text = "Type";
            // 
            // cboEquipmentTypeFilter
            // 
            cboEquipmentTypeFilter.Location = new Point(270, 12);
            cboEquipmentTypeFilter.Name = "cboEquipmentTypeFilter";
            cboEquipmentTypeFilter.Size = new Size(130, 23);
            cboEquipmentTypeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEquipmentTypeFilter.TabIndex = 1;
            cboEquipmentTypeFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblStatusFilter
            // 
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(412, 15);
            lblStatusFilter.Name = "lblStatusFilter";
            lblStatusFilter.Text = "Status";
            // 
            // cboEquipmentStatusFilter
            // 
            cboEquipmentStatusFilter.Location = new Point(458, 12);
            cboEquipmentStatusFilter.Name = "cboEquipmentStatusFilter";
            cboEquipmentStatusFilter.Size = new Size(120, 23);
            cboEquipmentStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEquipmentStatusFilter.TabIndex = 2;
            cboEquipmentStatusFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblMaintenanceDue
            // 
            lblMaintenanceDue.AutoSize = true;
            lblMaintenanceDue.Location = new Point(592, 15);
            lblMaintenanceDue.Name = "lblMaintenanceDue";
            lblMaintenanceDue.Text = "";
            // 
            // dgvEquipment
            // 
            dgvEquipment.AllowUserToAddRows = false;
            dgvEquipment.AllowUserToDeleteRows = false;
            dgvEquipment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEquipment.Columns.AddRange(new DataGridViewColumn[] { colName, colType, colOwnership, colStatus, colNext });
            dgvEquipment.MultiSelect = false;
            dgvEquipment.ReadOnly = true;
            dgvEquipment.RowHeadersVisible = false;
            dgvEquipment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEquipment.Location = new Point(12, 44);
            dgvEquipment.Name = "dgvEquipment";
            dgvEquipment.Size = new Size(930, 180);
            dgvEquipment.TabIndex = 3;
            dgvEquipment.SelectionChanged += dgvEquipment_SelectionChanged;
            dgvEquipment.CellFormatting += dgvEquipment_CellFormatting;
            // 
            // btnAddEquipment
            // 
            btnAddEquipment.Location = new Point(12, 232);
            btnAddEquipment.Name = "btnAddEquipment";
            btnAddEquipment.Size = new Size(120, 28);
            btnAddEquipment.Text = "Add Equipment";
            btnAddEquipment.TabIndex = 4;
            btnAddEquipment.Click += btnAddEquipment_Click;
            // 
            // btnDeleteEquipment
            // 
            btnDeleteEquipment.Location = new Point(138, 232);
            btnDeleteEquipment.Name = "btnDeleteEquipment";
            btnDeleteEquipment.Size = new Size(90, 28);
            btnDeleteEquipment.Text = "Delete";
            btnDeleteEquipment.TabIndex = 5;
            btnDeleteEquipment.Click += btnDeleteEquipment_Click;
            // 
            // btnExportEquipment
            // 
            btnExportEquipment.Location = new Point(234, 232);
            btnExportEquipment.Name = "btnExportEquipment";
            btnExportEquipment.Size = new Size(90, 28);
            btnExportEquipment.Text = "Export";
            btnExportEquipment.TabIndex = 6;
            btnExportEquipment.Click += btnExportEquipment_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(12, 274);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New equipment";
            // 
            // lblEquipmentName
            // 
            lblEquipmentName.AutoSize = true;
            lblEquipmentName.Location = new Point(12, 298);
            lblEquipmentName.Name = "lblEquipmentName";
            lblEquipmentName.Text = "Name (e.g. Concrete Mixer #2)";
            // 
            // txtEquipmentName
            // 
            txtEquipmentName.Location = new Point(12, 316);
            txtEquipmentName.Name = "txtEquipmentName";
            txtEquipmentName.Size = new Size(260, 23);
            txtEquipmentName.TabIndex = 7;
            // 
            // lblEquipmentType
            // 
            lblEquipmentType.AutoSize = true;
            lblEquipmentType.Location = new Point(12, 346);
            lblEquipmentType.Name = "lblEquipmentType";
            lblEquipmentType.Text = "Type";
            // 
            // cboEquipmentType
            // 
            cboEquipmentType.Location = new Point(12, 364);
            cboEquipmentType.Name = "cboEquipmentType";
            cboEquipmentType.Size = new Size(160, 23);
            cboEquipmentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEquipmentType.TabIndex = 8;
            // 
            // rdoOwned
            // 
            rdoOwned.Location = new Point(12, 396);
            rdoOwned.Name = "rdoOwned";
            rdoOwned.Text = "Owned";
            rdoOwned.AutoSize = true;
            rdoOwned.Checked = true;
            rdoOwned.TabIndex = 9;
            // 
            // rdoRented
            // 
            rdoRented.Location = new Point(90, 396);
            rdoRented.Name = "rdoRented";
            rdoRented.Text = "Rented";
            rdoRented.AutoSize = true;
            rdoRented.TabIndex = 10;
            rdoRented.CheckedChanged += Ownership_Changed;
            // 
            // lblRentalCompany
            // 
            lblRentalCompany.AutoSize = true;
            lblRentalCompany.Location = new Point(12, 424);
            lblRentalCompany.Name = "lblRentalCompany";
            lblRentalCompany.Text = "Rental company";
            // 
            // txtRentalCompany
            // 
            txtRentalCompany.Location = new Point(12, 442);
            txtRentalCompany.Name = "txtRentalCompany";
            txtRentalCompany.Size = new Size(260, 23);
            txtRentalCompany.TabIndex = 11;
            // 
            // lblRentalRate
            // 
            lblRentalRate.AutoSize = true;
            lblRentalRate.Location = new Point(12, 472);
            lblRentalRate.Name = "lblRentalRate";
            lblRentalRate.Text = "Rental rate";
            // 
            // numRentalRate
            // 
            numRentalRate.Location = new Point(12, 490);
            numRentalRate.Name = "numRentalRate";
            numRentalRate.Size = new Size(120, 23);
            numRentalRate.DecimalPlaces = 2;
            numRentalRate.ThousandsSeparator = true;
            numRentalRate.Maximum = 99999999m;
            numRentalRate.TabIndex = 12;
            // 
            // cboRentalRateUnit
            // 
            cboRentalRateUnit.Location = new Point(138, 490);
            cboRentalRateUnit.Name = "cboRentalRateUnit";
            cboRentalRateUnit.Size = new Size(100, 23);
            cboRentalRateUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRentalRateUnit.TabIndex = 13;
            // 
            // lblRentalReturn
            // 
            lblRentalReturn.AutoSize = true;
            lblRentalReturn.Location = new Point(12, 520);
            lblRentalReturn.Name = "lblRentalReturn";
            lblRentalReturn.Text = "Return date (tick to set)";
            // 
            // dtpRentalReturn
            // 
            dtpRentalReturn.Location = new Point(12, 538);
            dtpRentalReturn.Name = "dtpRentalReturn";
            dtpRentalReturn.Size = new Size(140, 23);
            dtpRentalReturn.Format = DateTimePickerFormat.Short;
            dtpRentalReturn.ShowCheckBox = true;
            dtpRentalReturn.TabIndex = 14;
            // 
            // lblEquipmentStatus
            // 
            lblEquipmentStatus.AutoSize = true;
            lblEquipmentStatus.Location = new Point(300, 298);
            lblEquipmentStatus.Name = "lblEquipmentStatus";
            lblEquipmentStatus.Text = "Status";
            // 
            // cboEquipmentStatus
            // 
            cboEquipmentStatus.Location = new Point(300, 316);
            cboEquipmentStatus.Name = "cboEquipmentStatus";
            cboEquipmentStatus.Size = new Size(140, 23);
            cboEquipmentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEquipmentStatus.TabIndex = 15;
            // 
            // lblNextMaintenance
            // 
            lblNextMaintenance.AutoSize = true;
            lblNextMaintenance.Location = new Point(300, 346);
            lblNextMaintenance.Name = "lblNextMaintenance";
            lblNextMaintenance.Text = "Next maintenance (tick to set)";
            // 
            // dtpNextMaintenance
            // 
            dtpNextMaintenance.Location = new Point(300, 364);
            dtpNextMaintenance.Name = "dtpNextMaintenance";
            dtpNextMaintenance.Size = new Size(140, 23);
            dtpNextMaintenance.Format = DateTimePickerFormat.Short;
            dtpNextMaintenance.ShowCheckBox = true;
            dtpNextMaintenance.TabIndex = 16;
            // 
            // lblEquipmentMsg
            // 
            lblEquipmentMsg.AutoSize = true;
            lblEquipmentMsg.Location = new Point(12, 572);
            lblEquipmentMsg.Name = "lblEquipmentMsg";
            // 
            // btnSaveEquipment
            // 
            btnSaveEquipment.Location = new Point(12, 594);
            btnSaveEquipment.Name = "btnSaveEquipment";
            btnSaveEquipment.Size = new Size(90, 28);
            btnSaveEquipment.Text = "Save";
            btnSaveEquipment.TabIndex = 17;
            btnSaveEquipment.Click += btnSaveEquipment_Click;
            // 
            // btnCancelEquipment
            // 
            btnCancelEquipment.Location = new Point(108, 594);
            btnCancelEquipment.Name = "btnCancelEquipment";
            btnCancelEquipment.Size = new Size(90, 28);
            btnCancelEquipment.Text = "Cancel";
            btnCancelEquipment.TabIndex = 18;
            btnCancelEquipment.Click += btnCancelEquipment_Click;
            // 
            // lblLogTitle
            // 
            lblLogTitle.AutoSize = true;
            lblLogTitle.Location = new Point(470, 274);
            lblLogTitle.Name = "lblLogTitle";
            lblLogTitle.Text = "Maintenance & repair log (selected equipment)";
            // 
            // dgvMaintenanceLog
            // 
            dgvMaintenanceLog.AllowUserToAddRows = false;
            dgvMaintenanceLog.AllowUserToDeleteRows = false;
            dgvMaintenanceLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMaintenanceLog.Columns.AddRange(new DataGridViewColumn[] { colLDate, colLKind, colLWork, colLCost, colLBy });
            dgvMaintenanceLog.MultiSelect = false;
            dgvMaintenanceLog.ReadOnly = true;
            dgvMaintenanceLog.RowHeadersVisible = false;
            dgvMaintenanceLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaintenanceLog.Location = new Point(470, 298);
            dgvMaintenanceLog.Name = "dgvMaintenanceLog";
            dgvMaintenanceLog.Size = new Size(472, 220);
            dgvMaintenanceLog.TabIndex = 19;
            // 
            // btnAddMaintenance
            // 
            btnAddMaintenance.Location = new Point(470, 526);
            btnAddMaintenance.Name = "btnAddMaintenance";
            btnAddMaintenance.Size = new Size(130, 28);
            btnAddMaintenance.Text = "Add Log Entry";
            btnAddMaintenance.TabIndex = 20;
            btnAddMaintenance.Click += btnAddMaintenance_Click;
            // 
            // btnDeleteMaintenance
            // 
            btnDeleteMaintenance.Location = new Point(606, 526);
            btnDeleteMaintenance.Name = "btnDeleteMaintenance";
            btnDeleteMaintenance.Size = new Size(110, 28);
            btnDeleteMaintenance.Text = "Delete Entry";
            btnDeleteMaintenance.TabIndex = 21;
            btnDeleteMaintenance.Click += btnDeleteMaintenance_Click;
            // 
            // colName
            // 
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            // 
            // colType
            // 
            colType.DataPropertyName = "Type";
            colType.HeaderText = "Type";
            colType.Name = "colType";
            // 
            // colOwnership
            // 
            colOwnership.DataPropertyName = "Ownership";
            colOwnership.HeaderText = "Owned / Rented";
            colOwnership.Name = "colOwnership";
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            // 
            // colNext
            // 
            colNext.DataPropertyName = "NextMaintenance";
            colNext.HeaderText = "Next Maintenance";
            colNext.Name = "colNext";
            colNext.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy", NullValue = "-" };
            // 
            // colLDate
            // 
            colLDate.DataPropertyName = "DoneOn";
            colLDate.HeaderText = "Date";
            colLDate.Name = "colLDate";
            colLDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy", NullValue = "-" };
            // 
            // colLKind
            // 
            colLKind.DataPropertyName = "Kind";
            colLKind.HeaderText = "Type";
            colLKind.Name = "colLKind";
            // 
            // colLWork
            // 
            colLWork.DataPropertyName = "WorkDone";
            colLWork.HeaderText = "Work Done";
            colLWork.Name = "colLWork";
            // 
            // colLCost
            // 
            colLCost.DataPropertyName = "Cost";
            colLCost.HeaderText = "Cost";
            colLCost.Name = "colLCost";
            colLCost.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colLBy
            // 
            colLBy.DataPropertyName = "DoneBy";
            colLBy.HeaderText = "Done By";
            colLBy.Name = "colLBy";
            // 
            // EquipmentTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblSearch);
            Controls.Add(txtEquipmentSearch);
            Controls.Add(lblTypeFilter);
            Controls.Add(cboEquipmentTypeFilter);
            Controls.Add(lblStatusFilter);
            Controls.Add(cboEquipmentStatusFilter);
            Controls.Add(lblMaintenanceDue);
            Controls.Add(dgvEquipment);
            Controls.Add(btnAddEquipment);
            Controls.Add(btnDeleteEquipment);
            Controls.Add(btnExportEquipment);
            Controls.Add(lblFormMode);
            Controls.Add(lblEquipmentName);
            Controls.Add(txtEquipmentName);
            Controls.Add(lblEquipmentType);
            Controls.Add(cboEquipmentType);
            Controls.Add(rdoOwned);
            Controls.Add(rdoRented);
            Controls.Add(lblRentalCompany);
            Controls.Add(txtRentalCompany);
            Controls.Add(lblRentalRate);
            Controls.Add(numRentalRate);
            Controls.Add(cboRentalRateUnit);
            Controls.Add(lblRentalReturn);
            Controls.Add(dtpRentalReturn);
            Controls.Add(lblEquipmentStatus);
            Controls.Add(cboEquipmentStatus);
            Controls.Add(lblNextMaintenance);
            Controls.Add(dtpNextMaintenance);
            Controls.Add(lblEquipmentMsg);
            Controls.Add(btnSaveEquipment);
            Controls.Add(btnCancelEquipment);
            Controls.Add(lblLogTitle);
            Controls.Add(dgvMaintenanceLog);
            Controls.Add(btnAddMaintenance);
            Controls.Add(btnDeleteMaintenance);
            Name = "EquipmentTab";
            Size = new Size(960, 660);
            Load += EquipmentTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvEquipment).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRentalRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMaintenanceLog).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSearch;
        private TextBox txtEquipmentSearch;
        private Label lblTypeFilter;
        private ComboBox cboEquipmentTypeFilter;
        private Label lblStatusFilter;
        private ComboBox cboEquipmentStatusFilter;
        private Label lblMaintenanceDue;
        private DataGridView dgvEquipment;
        private Button btnAddEquipment;
        private Button btnDeleteEquipment;
        private Button btnExportEquipment;
        private Label lblFormMode;
        private Label lblEquipmentName;
        private TextBox txtEquipmentName;
        private Label lblEquipmentType;
        private ComboBox cboEquipmentType;
        private RadioButton rdoOwned;
        private RadioButton rdoRented;
        private Label lblRentalCompany;
        private TextBox txtRentalCompany;
        private Label lblRentalRate;
        private NumericUpDown numRentalRate;
        private ComboBox cboRentalRateUnit;
        private Label lblRentalReturn;
        private DateTimePicker dtpRentalReturn;
        private Label lblEquipmentStatus;
        private ComboBox cboEquipmentStatus;
        private Label lblNextMaintenance;
        private DateTimePicker dtpNextMaintenance;
        private Label lblEquipmentMsg;
        private Button btnSaveEquipment;
        private Button btnCancelEquipment;
        private Label lblLogTitle;
        private DataGridView dgvMaintenanceLog;
        private Button btnAddMaintenance;
        private Button btnDeleteMaintenance;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colType;
        private DataGridViewTextBoxColumn colOwnership;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colNext;
        private DataGridViewTextBoxColumn colLDate;
        private DataGridViewTextBoxColumn colLKind;
        private DataGridViewTextBoxColumn colLWork;
        private DataGridViewTextBoxColumn colLCost;
        private DataGridViewTextBoxColumn colLBy;
    }
}
