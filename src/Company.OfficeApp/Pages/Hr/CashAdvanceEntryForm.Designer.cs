namespace Company.OfficeApp.Pages.Hr
{
    partial class CashAdvanceEntryForm
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
            lblCaEmployee = new Label();
            cboCaEmployee = new ComboBox();
            lblCaAmount = new Label();
            numCaAmount = new NumericUpDown();
            lblCaPerPayday = new Label();
            numCaPerPayday = new NumericUpDown();
            lblCaDate = new Label();
            dtpCaDate = new DateTimePicker();
            lblCaReason = new Label();
            txtCaReason = new TextBox();
            lblCaEntryMsg = new Label();
            btnSaveCashAdvance = new Button();
            btnCancelCashAdvance = new Button();
            ((System.ComponentModel.ISupportInitialize)numCaAmount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCaPerPayday).BeginInit();
            SuspendLayout();
            // 
            // lblCaEmployee
            // 
            lblCaEmployee.AutoSize = true;
            lblCaEmployee.Location = new Point(12, 12);
            lblCaEmployee.Name = "lblCaEmployee";
            lblCaEmployee.Text = "Employee";
            // 
            // cboCaEmployee
            // 
            cboCaEmployee.Location = new Point(12, 30);
            cboCaEmployee.Name = "cboCaEmployee";
            cboCaEmployee.Size = new Size(370, 23);
            cboCaEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCaEmployee.TabIndex = 0;
            // 
            // lblCaAmount
            // 
            lblCaAmount.AutoSize = true;
            lblCaAmount.Location = new Point(12, 60);
            lblCaAmount.Name = "lblCaAmount";
            lblCaAmount.Text = "Amount given";
            // 
            // numCaAmount
            // 
            numCaAmount.Location = new Point(12, 78);
            numCaAmount.Name = "numCaAmount";
            numCaAmount.Size = new Size(140, 23);
            numCaAmount.DecimalPlaces = 2;
            numCaAmount.ThousandsSeparator = true;
            numCaAmount.Maximum = 9999999m;
            numCaAmount.TabIndex = 1;
            // 
            // lblCaPerPayday
            // 
            lblCaPerPayday.AutoSize = true;
            lblCaPerPayday.Location = new Point(170, 60);
            lblCaPerPayday.Name = "lblCaPerPayday";
            lblCaPerPayday.Text = "Deduct per payday";
            // 
            // numCaPerPayday
            // 
            numCaPerPayday.Location = new Point(170, 78);
            numCaPerPayday.Name = "numCaPerPayday";
            numCaPerPayday.Size = new Size(140, 23);
            numCaPerPayday.DecimalPlaces = 2;
            numCaPerPayday.ThousandsSeparator = true;
            numCaPerPayday.Maximum = 9999999m;
            numCaPerPayday.TabIndex = 2;
            // 
            // lblCaDate
            // 
            lblCaDate.AutoSize = true;
            lblCaDate.Location = new Point(12, 108);
            lblCaDate.Name = "lblCaDate";
            lblCaDate.Text = "Date given";
            // 
            // dtpCaDate
            // 
            dtpCaDate.Location = new Point(12, 126);
            dtpCaDate.Name = "dtpCaDate";
            dtpCaDate.Size = new Size(130, 23);
            dtpCaDate.Format = DateTimePickerFormat.Short;
            dtpCaDate.TabIndex = 3;
            // 
            // lblCaReason
            // 
            lblCaReason.AutoSize = true;
            lblCaReason.Location = new Point(12, 156);
            lblCaReason.Name = "lblCaReason";
            lblCaReason.Text = "Reason";
            // 
            // txtCaReason
            // 
            txtCaReason.Location = new Point(12, 174);
            txtCaReason.Name = "txtCaReason";
            txtCaReason.Size = new Size(370, 60);
            txtCaReason.Multiline = true;
            txtCaReason.ScrollBars = ScrollBars.Vertical;
            txtCaReason.TabIndex = 4;
            // 
            // lblCaEntryMsg
            // 
            lblCaEntryMsg.AutoSize = true;
            lblCaEntryMsg.Location = new Point(12, 242);
            lblCaEntryMsg.Name = "lblCaEntryMsg";
            lblCaEntryMsg.MaximumSize = new Size(370, 0);
            // 
            // btnSaveCashAdvance
            // 
            btnSaveCashAdvance.Location = new Point(196, 288);
            btnSaveCashAdvance.Name = "btnSaveCashAdvance";
            btnSaveCashAdvance.Size = new Size(90, 28);
            btnSaveCashAdvance.Text = "Save";
            btnSaveCashAdvance.TabIndex = 5;
            btnSaveCashAdvance.Click += btnSaveCashAdvance_Click;
            // 
            // btnCancelCashAdvance
            // 
            btnCancelCashAdvance.Location = new Point(292, 288);
            btnCancelCashAdvance.Name = "btnCancelCashAdvance";
            btnCancelCashAdvance.Size = new Size(90, 28);
            btnCancelCashAdvance.Text = "Cancel";
            btnCancelCashAdvance.DialogResult = DialogResult.Cancel;
            btnCancelCashAdvance.TabIndex = 6;
            // 
            // CashAdvanceEntryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 330);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(lblCaEmployee);
            Controls.Add(cboCaEmployee);
            Controls.Add(lblCaAmount);
            Controls.Add(numCaAmount);
            Controls.Add(lblCaPerPayday);
            Controls.Add(numCaPerPayday);
            Controls.Add(lblCaDate);
            Controls.Add(dtpCaDate);
            Controls.Add(lblCaReason);
            Controls.Add(txtCaReason);
            Controls.Add(lblCaEntryMsg);
            Controls.Add(btnSaveCashAdvance);
            Controls.Add(btnCancelCashAdvance);
            Name = "CashAdvanceEntryForm";
            Load += CashAdvanceEntryForm_Load;
            ((System.ComponentModel.ISupportInitialize)numCaAmount).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCaPerPayday).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCaEmployee;
        private ComboBox cboCaEmployee;
        private Label lblCaAmount;
        private NumericUpDown numCaAmount;
        private Label lblCaPerPayday;
        private NumericUpDown numCaPerPayday;
        private Label lblCaDate;
        private DateTimePicker dtpCaDate;
        private Label lblCaReason;
        private TextBox txtCaReason;
        private Label lblCaEntryMsg;
        private Button btnSaveCashAdvance;
        private Button btnCancelCashAdvance;
    }
}
