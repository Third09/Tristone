namespace Company.OfficeApp.Pages.Hr
{
    partial class FixScanForm
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
            lblFixEmployeeDate = new Label();
            lblFixHint = new Label();
            lblFixTimeIn = new Label();
            dtpFixTimeIn = new DateTimePicker();
            lblFixBreakOut = new Label();
            dtpFixBreakOut = new DateTimePicker();
            lblFixBreakIn = new Label();
            dtpFixBreakIn = new DateTimePicker();
            lblFixTimeOut = new Label();
            dtpFixTimeOut = new DateTimePicker();
            lblFixReason = new Label();
            txtFixReason = new TextBox();
            lblFixMsg = new Label();
            btnSaveFix = new Button();
            btnCancelFix = new Button();
            SuspendLayout();
            // 
            // lblFixEmployeeDate
            // 
            lblFixEmployeeDate.AutoSize = true;
            lblFixEmployeeDate.Location = new Point(12, 12);
            lblFixEmployeeDate.Name = "lblFixEmployeeDate";
            lblFixEmployeeDate.Text = "";
            // 
            // lblFixHint
            // 
            lblFixHint.AutoSize = true;
            lblFixHint.Location = new Point(12, 34);
            lblFixHint.Name = "lblFixHint";
            lblFixHint.Text = "Tick each time that is known.";
            // 
            // lblFixTimeIn
            // 
            lblFixTimeIn.AutoSize = true;
            lblFixTimeIn.Location = new Point(12, 64);
            lblFixTimeIn.Name = "lblFixTimeIn";
            lblFixTimeIn.Text = "Time in";
            // 
            // dtpFixTimeIn
            // 
            dtpFixTimeIn.Location = new Point(120, 60);
            dtpFixTimeIn.Name = "dtpFixTimeIn";
            dtpFixTimeIn.Size = new Size(130, 23);
            dtpFixTimeIn.Format = DateTimePickerFormat.Custom;
            dtpFixTimeIn.CustomFormat = "h:mm tt";
            dtpFixTimeIn.ShowUpDown = true;
            dtpFixTimeIn.ShowCheckBox = true;
            dtpFixTimeIn.TabIndex = 0;
            // 
            // lblFixBreakOut
            // 
            lblFixBreakOut.AutoSize = true;
            lblFixBreakOut.Location = new Point(12, 94);
            lblFixBreakOut.Name = "lblFixBreakOut";
            lblFixBreakOut.Text = "Break out";
            // 
            // dtpFixBreakOut
            // 
            dtpFixBreakOut.Location = new Point(120, 90);
            dtpFixBreakOut.Name = "dtpFixBreakOut";
            dtpFixBreakOut.Size = new Size(130, 23);
            dtpFixBreakOut.Format = DateTimePickerFormat.Custom;
            dtpFixBreakOut.CustomFormat = "h:mm tt";
            dtpFixBreakOut.ShowUpDown = true;
            dtpFixBreakOut.ShowCheckBox = true;
            dtpFixBreakOut.TabIndex = 1;
            // 
            // lblFixBreakIn
            // 
            lblFixBreakIn.AutoSize = true;
            lblFixBreakIn.Location = new Point(12, 124);
            lblFixBreakIn.Name = "lblFixBreakIn";
            lblFixBreakIn.Text = "Break in";
            // 
            // dtpFixBreakIn
            // 
            dtpFixBreakIn.Location = new Point(120, 120);
            dtpFixBreakIn.Name = "dtpFixBreakIn";
            dtpFixBreakIn.Size = new Size(130, 23);
            dtpFixBreakIn.Format = DateTimePickerFormat.Custom;
            dtpFixBreakIn.CustomFormat = "h:mm tt";
            dtpFixBreakIn.ShowUpDown = true;
            dtpFixBreakIn.ShowCheckBox = true;
            dtpFixBreakIn.TabIndex = 2;
            // 
            // lblFixTimeOut
            // 
            lblFixTimeOut.AutoSize = true;
            lblFixTimeOut.Location = new Point(12, 154);
            lblFixTimeOut.Name = "lblFixTimeOut";
            lblFixTimeOut.Text = "Time out";
            // 
            // dtpFixTimeOut
            // 
            dtpFixTimeOut.Location = new Point(120, 150);
            dtpFixTimeOut.Name = "dtpFixTimeOut";
            dtpFixTimeOut.Size = new Size(130, 23);
            dtpFixTimeOut.Format = DateTimePickerFormat.Custom;
            dtpFixTimeOut.CustomFormat = "h:mm tt";
            dtpFixTimeOut.ShowUpDown = true;
            dtpFixTimeOut.ShowCheckBox = true;
            dtpFixTimeOut.TabIndex = 3;
            // 
            // lblFixReason
            // 
            lblFixReason.AutoSize = true;
            lblFixReason.Location = new Point(12, 186);
            lblFixReason.Name = "lblFixReason";
            lblFixReason.Text = "Reason (required)";
            // 
            // txtFixReason
            // 
            txtFixReason.Location = new Point(12, 204);
            txtFixReason.Name = "txtFixReason";
            txtFixReason.Size = new Size(396, 56);
            txtFixReason.Multiline = true;
            txtFixReason.ScrollBars = ScrollBars.Vertical;
            txtFixReason.TabIndex = 4;
            // 
            // lblFixMsg
            // 
            lblFixMsg.AutoSize = true;
            lblFixMsg.Location = new Point(12, 268);
            lblFixMsg.Name = "lblFixMsg";
            lblFixMsg.MaximumSize = new Size(396, 0);
            // 
            // btnSaveFix
            // 
            btnSaveFix.Location = new Point(222, 300);
            btnSaveFix.Name = "btnSaveFix";
            btnSaveFix.Size = new Size(90, 28);
            btnSaveFix.Text = "Save";
            btnSaveFix.TabIndex = 5;
            btnSaveFix.Click += btnSaveFix_Click;
            // 
            // btnCancelFix
            // 
            btnCancelFix.Location = new Point(318, 300);
            btnCancelFix.Name = "btnCancelFix";
            btnCancelFix.Size = new Size(90, 28);
            btnCancelFix.Text = "Cancel";
            btnCancelFix.DialogResult = DialogResult.Cancel;
            btnCancelFix.TabIndex = 6;
            // 
            // FixScanForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 340);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(lblFixEmployeeDate);
            Controls.Add(lblFixHint);
            Controls.Add(lblFixTimeIn);
            Controls.Add(dtpFixTimeIn);
            Controls.Add(lblFixBreakOut);
            Controls.Add(dtpFixBreakOut);
            Controls.Add(lblFixBreakIn);
            Controls.Add(dtpFixBreakIn);
            Controls.Add(lblFixTimeOut);
            Controls.Add(dtpFixTimeOut);
            Controls.Add(lblFixReason);
            Controls.Add(txtFixReason);
            Controls.Add(lblFixMsg);
            Controls.Add(btnSaveFix);
            Controls.Add(btnCancelFix);
            Name = "FixScanForm";
            Load += FixScanForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFixEmployeeDate;
        private Label lblFixHint;
        private Label lblFixTimeIn;
        private DateTimePicker dtpFixTimeIn;
        private Label lblFixBreakOut;
        private DateTimePicker dtpFixBreakOut;
        private Label lblFixBreakIn;
        private DateTimePicker dtpFixBreakIn;
        private Label lblFixTimeOut;
        private DateTimePicker dtpFixTimeOut;
        private Label lblFixReason;
        private TextBox txtFixReason;
        private Label lblFixMsg;
        private Button btnSaveFix;
        private Button btnCancelFix;
    }
}
