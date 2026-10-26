namespace Company.OfficeApp.Pages.Inventory
{
    partial class MaintenanceEntryForm
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
            lblMaintDate = new Label();
            dtpMaintDate = new DateTimePicker();
            lblMaintType = new Label();
            cboMaintType = new ComboBox();
            lblMaintWork = new Label();
            txtMaintWork = new TextBox();
            lblMaintCost = new Label();
            numMaintCost = new NumericUpDown();
            lblMaintDoneBy = new Label();
            txtMaintDoneBy = new TextBox();
            lblMaintNextDue = new Label();
            dtpMaintNextDue = new DateTimePicker();
            lblMaintMsg = new Label();
            btnSaveMaintenance = new Button();
            btnCancelMaintenance = new Button();
            ((System.ComponentModel.ISupportInitialize)numMaintCost).BeginInit();
            SuspendLayout();
            // 
            // lblMaintDate
            // 
            lblMaintDate.AutoSize = true;
            lblMaintDate.Location = new Point(12, 12);
            lblMaintDate.Name = "lblMaintDate";
            lblMaintDate.Text = "Date of the work";
            // 
            // dtpMaintDate
            // 
            dtpMaintDate.Location = new Point(12, 30);
            dtpMaintDate.Name = "dtpMaintDate";
            dtpMaintDate.Size = new Size(140, 23);
            dtpMaintDate.Format = DateTimePickerFormat.Short;
            dtpMaintDate.TabIndex = 0;
            // 
            // lblMaintType
            // 
            lblMaintType.AutoSize = true;
            lblMaintType.Location = new Point(170, 12);
            lblMaintType.Name = "lblMaintType";
            lblMaintType.Text = "Type";
            // 
            // cboMaintType
            // 
            cboMaintType.Location = new Point(170, 30);
            cboMaintType.Name = "cboMaintType";
            cboMaintType.Size = new Size(140, 23);
            cboMaintType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaintType.TabIndex = 1;
            // 
            // lblMaintWork
            // 
            lblMaintWork.AutoSize = true;
            lblMaintWork.Location = new Point(12, 62);
            lblMaintWork.Name = "lblMaintWork";
            lblMaintWork.Text = "Work done";
            // 
            // txtMaintWork
            // 
            txtMaintWork.Location = new Point(12, 80);
            txtMaintWork.Name = "txtMaintWork";
            txtMaintWork.Size = new Size(380, 70);
            txtMaintWork.Multiline = true;
            txtMaintWork.TabIndex = 2;
            // 
            // lblMaintCost
            // 
            lblMaintCost.AutoSize = true;
            lblMaintCost.Location = new Point(12, 160);
            lblMaintCost.Name = "lblMaintCost";
            lblMaintCost.Text = "Cost (pesos)";
            // 
            // numMaintCost
            // 
            numMaintCost.Location = new Point(12, 178);
            numMaintCost.Name = "numMaintCost";
            numMaintCost.Size = new Size(140, 23);
            numMaintCost.DecimalPlaces = 2;
            numMaintCost.ThousandsSeparator = true;
            numMaintCost.Maximum = 99999999m;
            numMaintCost.TabIndex = 3;
            // 
            // lblMaintDoneBy
            // 
            lblMaintDoneBy.AutoSize = true;
            lblMaintDoneBy.Location = new Point(12, 210);
            lblMaintDoneBy.Name = "lblMaintDoneBy";
            lblMaintDoneBy.Text = "Done by (person or shop)";
            // 
            // txtMaintDoneBy
            // 
            txtMaintDoneBy.Location = new Point(12, 228);
            txtMaintDoneBy.Name = "txtMaintDoneBy";
            txtMaintDoneBy.Size = new Size(260, 23);
            txtMaintDoneBy.TabIndex = 4;
            // 
            // lblMaintNextDue
            // 
            lblMaintNextDue.AutoSize = true;
            lblMaintNextDue.Location = new Point(12, 260);
            lblMaintNextDue.Name = "lblMaintNextDue";
            lblMaintNextDue.Text = "Next maintenance date (tick to set; moves the reminder)";
            // 
            // dtpMaintNextDue
            // 
            dtpMaintNextDue.Location = new Point(12, 278);
            dtpMaintNextDue.Name = "dtpMaintNextDue";
            dtpMaintNextDue.Size = new Size(140, 23);
            dtpMaintNextDue.Format = DateTimePickerFormat.Short;
            dtpMaintNextDue.ShowCheckBox = true;
            dtpMaintNextDue.TabIndex = 5;
            // 
            // lblMaintMsg
            // 
            lblMaintMsg.AutoSize = true;
            lblMaintMsg.Location = new Point(12, 312);
            lblMaintMsg.Name = "lblMaintMsg";
            // 
            // btnSaveMaintenance
            // 
            btnSaveMaintenance.Location = new Point(12, 340);
            btnSaveMaintenance.Name = "btnSaveMaintenance";
            btnSaveMaintenance.Size = new Size(90, 28);
            btnSaveMaintenance.Text = "Save";
            btnSaveMaintenance.TabIndex = 6;
            btnSaveMaintenance.Click += btnSaveMaintenance_Click;
            // 
            // btnCancelMaintenance
            // 
            btnCancelMaintenance.Location = new Point(108, 340);
            btnCancelMaintenance.Name = "btnCancelMaintenance";
            btnCancelMaintenance.Size = new Size(90, 28);
            btnCancelMaintenance.Text = "Cancel";
            btnCancelMaintenance.TabIndex = 7;
            btnCancelMaintenance.Click += btnCancelMaintenance_Click;
            // 
            // MaintenanceEntryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 384);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(lblMaintDate);
            Controls.Add(dtpMaintDate);
            Controls.Add(lblMaintType);
            Controls.Add(cboMaintType);
            Controls.Add(lblMaintWork);
            Controls.Add(txtMaintWork);
            Controls.Add(lblMaintCost);
            Controls.Add(numMaintCost);
            Controls.Add(lblMaintDoneBy);
            Controls.Add(txtMaintDoneBy);
            Controls.Add(lblMaintNextDue);
            Controls.Add(dtpMaintNextDue);
            Controls.Add(lblMaintMsg);
            Controls.Add(btnSaveMaintenance);
            Controls.Add(btnCancelMaintenance);
            Name = "MaintenanceEntryForm";
            ((System.ComponentModel.ISupportInitialize)numMaintCost).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMaintDate;
        private DateTimePicker dtpMaintDate;
        private Label lblMaintType;
        private ComboBox cboMaintType;
        private Label lblMaintWork;
        private TextBox txtMaintWork;
        private Label lblMaintCost;
        private NumericUpDown numMaintCost;
        private Label lblMaintDoneBy;
        private TextBox txtMaintDoneBy;
        private Label lblMaintNextDue;
        private DateTimePicker dtpMaintNextDue;
        private Label lblMaintMsg;
        private Button btnSaveMaintenance;
        private Button btnCancelMaintenance;
    }
}
