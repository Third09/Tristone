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
            lblCaEmployee.Location = new Point(38, 32);
            lblCaEmployee.Name = "lblCaEmployee";
            lblCaEmployee.Text = "Employee";
            // 
            // cboCaEmployee
            // 
            cboCaEmployee.Location = new Point(38, 56);
            cboCaEmployee.Name = "cboCaEmployee";
            cboCaEmployee.Size = new Size(425, 23);
            cboCaEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCaEmployee.TabIndex = 0;
            // 
            // lblCaAmount
            // 
            lblCaAmount.AutoSize = true;
            lblCaAmount.Location = new Point(38, 95);
            lblCaAmount.Name = "lblCaAmount";
            lblCaAmount.Text = "Amount given";
            // 
            // numCaAmount
            // 
            numCaAmount.Location = new Point(38, 119);
            numCaAmount.Name = "numCaAmount";
            numCaAmount.Size = new Size(161, 23);
            numCaAmount.DecimalPlaces = 2;
            numCaAmount.ThousandsSeparator = true;
            numCaAmount.Maximum = 9999999m;
            numCaAmount.TabIndex = 1;
            // 
            // lblCaPerPayday
            // 
            lblCaPerPayday.AutoSize = true;
            lblCaPerPayday.Location = new Point(219, 95);
            lblCaPerPayday.Name = "lblCaPerPayday";
            lblCaPerPayday.Text = "Deduct per payday";
            // 
            // numCaPerPayday
            // 
            numCaPerPayday.Location = new Point(219, 119);
            numCaPerPayday.Name = "numCaPerPayday";
            numCaPerPayday.Size = new Size(161, 23);
            numCaPerPayday.DecimalPlaces = 2;
            numCaPerPayday.ThousandsSeparator = true;
            numCaPerPayday.Maximum = 9999999m;
            numCaPerPayday.TabIndex = 2;
            // 
            // lblCaDate
            // 
            lblCaDate.AutoSize = true;
            lblCaDate.Location = new Point(38, 159);
            lblCaDate.Name = "lblCaDate";
            lblCaDate.Text = "Date given";
            // 
            // dtpCaDate
            // 
            dtpCaDate.Location = new Point(38, 182);
            dtpCaDate.Name = "dtpCaDate";
            dtpCaDate.Size = new Size(150, 23);
            dtpCaDate.Format = DateTimePickerFormat.Short;
            dtpCaDate.TabIndex = 3;
            // 
            // lblCaReason
            // 
            lblCaReason.AutoSize = true;
            lblCaReason.Location = new Point(38, 222);
            lblCaReason.Name = "lblCaReason";
            lblCaReason.Text = "Reason";
            // 
            // txtCaReason
            // 
            txtCaReason.Location = new Point(38, 246);
            txtCaReason.Name = "txtCaReason";
            txtCaReason.Size = new Size(425, 79);
            txtCaReason.Multiline = true;
            txtCaReason.ScrollBars = ScrollBars.Vertical;
            txtCaReason.TabIndex = 4;
            // 
            // lblCaEntryMsg
            // 
            lblCaEntryMsg.AutoSize = true;
            lblCaEntryMsg.Location = new Point(38, 335);
            lblCaEntryMsg.Name = "lblCaEntryMsg";
            lblCaEntryMsg.MaximumSize = new Size(370, 0);
            // 
            // btnSaveCashAdvance
            // 
            btnSaveCashAdvance.Location = new Point(249, 396);
            btnSaveCashAdvance.Name = "btnSaveCashAdvance";
            btnSaveCashAdvance.Size = new Size(103, 36);
            btnSaveCashAdvance.Text = "Save";
            btnSaveCashAdvance.TabIndex = 5;
            btnSaveCashAdvance.Click += btnSaveCashAdvance_Click;
            // 
            // btnCancelCashAdvance
            // 
            btnCancelCashAdvance.Location = new Point(360, 396);
            btnCancelCashAdvance.Name = "btnCancelCashAdvance";
            btnCancelCashAdvance.Size = new Size(103, 36);
            btnCancelCashAdvance.Text = "Cancel";
            btnCancelCashAdvance.DialogResult = DialogResult.Cancel;
            btnCancelCashAdvance.TabIndex = 6;
            // 
            // CashAdvanceEntryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(508, 468);
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
