namespace Company.WebsiteAdmin.Forms
{
    partial class SetAccountPasswordForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblSetPasswordFor = new Label();
            txtNewPassword = new TextBox();
            btnGeneratePassword = new Button();
            btnCopyPassword = new Button();
            lblSetPasswordHint = new Label();
            lblSetPasswordMsg = new Label();
            btnSavePassword = new Button();
            btnCancelPassword = new Button();
            SuspendLayout();
            // 
            // lblSetPasswordFor
            // 
            lblSetPasswordFor.AutoSize = true;
            lblSetPasswordFor.Location = new Point(12, 12);
            lblSetPasswordFor.Name = "lblSetPasswordFor";
            lblSetPasswordFor.Text = "New password for";
            // 
            // txtNewPassword
            // 
            txtNewPassword.Location = new Point(12, 38);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(260, 23);
            txtNewPassword.TabIndex = 0;
            // 
            // btnGeneratePassword
            // 
            btnGeneratePassword.Location = new Point(12, 68);
            btnGeneratePassword.Name = "btnGeneratePassword";
            btnGeneratePassword.Size = new Size(90, 28);
            btnGeneratePassword.Text = "Generate";
            btnGeneratePassword.Click += btnGeneratePassword_Click;
            btnGeneratePassword.TabIndex = 1;
            // 
            // btnCopyPassword
            // 
            btnCopyPassword.Location = new Point(108, 68);
            btnCopyPassword.Name = "btnCopyPassword";
            btnCopyPassword.Size = new Size(90, 28);
            btnCopyPassword.Text = "Copy";
            btnCopyPassword.Click += btnCopyPassword_Click;
            btnCopyPassword.TabIndex = 2;
            // 
            // lblSetPasswordHint
            // 
            lblSetPasswordHint.Location = new Point(12, 104);
            lblSetPasswordHint.Name = "lblSetPasswordHint";
            lblSetPasswordHint.Size = new Size(340, 32);
            lblSetPasswordHint.Text = "At least 8 characters with a letter and a number. Give it to the person in person. A locked account is unlocked too.";
            // 
            // lblSetPasswordMsg
            // 
            lblSetPasswordMsg.Location = new Point(12, 140);
            lblSetPasswordMsg.Name = "lblSetPasswordMsg";
            lblSetPasswordMsg.Size = new Size(340, 32);
            lblSetPasswordMsg.Text = "";
            // 
            // btnSavePassword
            // 
            btnSavePassword.Location = new Point(12, 178);
            btnSavePassword.Name = "btnSavePassword";
            btnSavePassword.Size = new Size(90, 28);
            btnSavePassword.Text = "Save";
            btnSavePassword.Click += btnSavePassword_Click;
            btnSavePassword.TabIndex = 3;
            // 
            // btnCancelPassword
            // 
            btnCancelPassword.Location = new Point(108, 178);
            btnCancelPassword.Name = "btnCancelPassword";
            btnCancelPassword.Size = new Size(90, 28);
            btnCancelPassword.Text = "Cancel";
            btnCancelPassword.DialogResult = DialogResult.Cancel;
            btnCancelPassword.TabIndex = 4;
            // 
            // SetAccountPasswordForm
            // 
            AcceptButton = btnSavePassword;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelPassword;
            ClientSize = new Size(364, 218);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Set New Password";
            Controls.Add(lblSetPasswordFor);
            Controls.Add(txtNewPassword);
            Controls.Add(btnGeneratePassword);
            Controls.Add(btnCopyPassword);
            Controls.Add(lblSetPasswordHint);
            Controls.Add(lblSetPasswordMsg);
            Controls.Add(btnSavePassword);
            Controls.Add(btnCancelPassword);
            Name = "SetAccountPasswordForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSetPasswordFor;
        private TextBox txtNewPassword;
        private Button btnGeneratePassword;
        private Button btnCopyPassword;
        private Label lblSetPasswordHint;
        private Label lblSetPasswordMsg;
        private Button btnSavePassword;
        private Button btnCancelPassword;
    }
}
