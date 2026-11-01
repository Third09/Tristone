namespace Company.OfficeApp.Forms
{
    partial class SetNewPasswordForm
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

        // ROOMY layout applied (docs/_tools/designer-gen/roomy_designer.py)
        private void InitializeComponent()
        {
            lblResetUser = new Label();
            lblRules = new Label();
            txtResetPassword = new TextBox();
            btnGeneratePassword = new Button();
            btnCopyPassword = new Button();
            lblResetError = new Label();
            btnSaveNewPassword = new Button();
            btnCancelNewPassword = new Button();
            SuspendLayout();
            //
            // lblResetUser
            //
            lblResetUser.AutoSize = true;
            lblResetUser.Location = new Point(38, 32);
            lblResetUser.Name = "lblResetUser";
            lblResetUser.Text = "New password for:";
            //
            // lblRules
            //
            lblRules.AutoSize = true;
            lblRules.Location = new Point(38, 64);
            lblRules.Name = "lblRules";
            lblRules.Text = "At least 8 characters, with a letter and a number.";
            //
            // txtResetPassword
            //
            txtResetPassword.Location = new Point(38, 90);
            txtResetPassword.Name = "txtResetPassword";
            txtResetPassword.Size = new Size(230, 23);
            txtResetPassword.TabIndex = 0;
            //
            // btnGeneratePassword
            //
            btnGeneratePassword.Location = new Point(38, 130);
            btnGeneratePassword.Name = "btnGeneratePassword";
            btnGeneratePassword.Size = new Size(103, 36);
            btnGeneratePassword.TabIndex = 1;
            btnGeneratePassword.Text = "Generate";
            btnGeneratePassword.Click += btnGeneratePassword_Click;
            //
            // btnCopyPassword
            //
            btnCopyPassword.Location = new Point(148, 130);
            btnCopyPassword.Name = "btnCopyPassword";
            btnCopyPassword.Size = new Size(103, 36);
            btnCopyPassword.TabIndex = 2;
            btnCopyPassword.Text = "Copy";
            btnCopyPassword.Click += btnCopyPassword_Click;
            //
            // lblResetError
            //
            lblResetError.Location = new Point(38, 174);
            lblResetError.Name = "lblResetError";
            lblResetError.Size = new Size(368, 36);
            //
            // btnSaveNewPassword
            //
            btnSaveNewPassword.Location = new Point(38, 230);
            btnSaveNewPassword.Name = "btnSaveNewPassword";
            btnSaveNewPassword.Size = new Size(103, 36);
            btnSaveNewPassword.TabIndex = 3;
            btnSaveNewPassword.Text = "Save";
            btnSaveNewPassword.Click += btnSaveNewPassword_Click;
            //
            // btnCancelNewPassword
            //
            btnCancelNewPassword.DialogResult = DialogResult.Cancel;
            btnCancelNewPassword.Location = new Point(148, 230);
            btnCancelNewPassword.Name = "btnCancelNewPassword";
            btnCancelNewPassword.Size = new Size(103, 36);
            btnCancelNewPassword.TabIndex = 4;
            btnCancelNewPassword.Text = "Cancel";
            //
            // SetNewPasswordForm
            //
            AcceptButton = btnSaveNewPassword;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelNewPassword;
            ClientSize = new Size(444, 299);
            Controls.Add(lblResetUser);
            Controls.Add(lblRules);
            Controls.Add(txtResetPassword);
            Controls.Add(btnGeneratePassword);
            Controls.Add(btnCopyPassword);
            Controls.Add(lblResetError);
            Controls.Add(btnSaveNewPassword);
            Controls.Add(btnCancelNewPassword);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SetNewPasswordForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Set New Password";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblResetUser;
        private Label lblRules;
        private TextBox txtResetPassword;
        private Button btnGeneratePassword;
        private Button btnCopyPassword;
        private Label lblResetError;
        private Button btnSaveNewPassword;
        private Button btnCancelNewPassword;
    }
}
