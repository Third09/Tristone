namespace Company.OfficeApp.Forms
{
    partial class ForgotPasswordForm
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
            pnlStep1 = new Panel();
            lblForgotInfo = new Label();
            txtForgotUsername = new TextBox();
            lblForgotError = new Label();
            btnForgotNext = new Button();
            btnForgotCancel = new Button();
            pnlRequestSent = new Panel();
            lblRequestSent = new Label();
            btnForgotClose = new Button();
            pnlAdmin = new Panel();
            lblQuestionCaption = new Label();
            lblSecurityQuestion = new Label();
            lblAnswer = new Label();
            txtSecurityAnswer = new TextBox();
            lblNewPassword = new Label();
            txtNewPassword = new TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            lblAdminError = new Label();
            btnResetPassword = new Button();
            btnAdminCancel = new Button();
            pnlStep1.SuspendLayout();
            pnlRequestSent.SuspendLayout();
            pnlAdmin.SuspendLayout();
            SuspendLayout();
            //
            // pnlStep1  (Step 1: enter username)
            //
            pnlStep1.Controls.Add(lblForgotInfo);
            pnlStep1.Controls.Add(txtForgotUsername);
            pnlStep1.Controls.Add(lblForgotError);
            pnlStep1.Controls.Add(btnForgotNext);
            pnlStep1.Controls.Add(btnForgotCancel);
            pnlStep1.Dock = DockStyle.Fill;
            pnlStep1.Name = "pnlStep1";
            //
            // lblForgotInfo
            //
            lblForgotInfo.AutoSize = true;
            lblForgotInfo.Location = new Point(12, 12);
            lblForgotInfo.Name = "lblForgotInfo";
            lblForgotInfo.Text = "Enter your username.";
            //
            // txtForgotUsername
            //
            txtForgotUsername.Location = new Point(12, 34);
            txtForgotUsername.Name = "txtForgotUsername";
            txtForgotUsername.Size = new Size(260, 23);
            txtForgotUsername.TabIndex = 0;
            //
            // lblForgotError
            //
            lblForgotError.Location = new Point(12, 64);
            lblForgotError.Name = "lblForgotError";
            lblForgotError.Size = new Size(330, 48);
            //
            // btnForgotNext
            //
            btnForgotNext.Location = new Point(12, 118);
            btnForgotNext.Name = "btnForgotNext";
            btnForgotNext.Size = new Size(90, 28);
            btnForgotNext.TabIndex = 1;
            btnForgotNext.Text = "Next";
            btnForgotNext.Click += btnForgotNext_Click;
            //
            // btnForgotCancel
            //
            btnForgotCancel.Location = new Point(108, 118);
            btnForgotCancel.Name = "btnForgotCancel";
            btnForgotCancel.Size = new Size(90, 28);
            btnForgotCancel.TabIndex = 2;
            btnForgotCancel.Text = "Cancel";
            btnForgotCancel.Click += btnClose_Click;
            //
            // pnlRequestSent  (Step 2A: Employee / Supervisor)
            //
            pnlRequestSent.Controls.Add(lblRequestSent);
            pnlRequestSent.Controls.Add(btnForgotClose);
            pnlRequestSent.Dock = DockStyle.Fill;
            pnlRequestSent.Name = "pnlRequestSent";
            //
            // lblRequestSent
            //
            lblRequestSent.Location = new Point(12, 12);
            lblRequestSent.Name = "lblRequestSent";
            lblRequestSent.Size = new Size(330, 48);
            //
            // btnForgotClose
            //
            btnForgotClose.Location = new Point(12, 66);
            btnForgotClose.Name = "btnForgotClose";
            btnForgotClose.Size = new Size(90, 28);
            btnForgotClose.TabIndex = 0;
            btnForgotClose.Text = "Close";
            btnForgotClose.Click += btnClose_Click;
            //
            // pnlAdmin  (Step 2B: Admin security question)
            //
            pnlAdmin.Controls.Add(lblQuestionCaption);
            pnlAdmin.Controls.Add(lblSecurityQuestion);
            pnlAdmin.Controls.Add(lblAnswer);
            pnlAdmin.Controls.Add(txtSecurityAnswer);
            pnlAdmin.Controls.Add(lblNewPassword);
            pnlAdmin.Controls.Add(txtNewPassword);
            pnlAdmin.Controls.Add(lblConfirmPassword);
            pnlAdmin.Controls.Add(txtConfirmPassword);
            pnlAdmin.Controls.Add(lblAdminError);
            pnlAdmin.Controls.Add(btnResetPassword);
            pnlAdmin.Controls.Add(btnAdminCancel);
            pnlAdmin.Dock = DockStyle.Fill;
            pnlAdmin.Name = "pnlAdmin";
            //
            // lblQuestionCaption
            //
            lblQuestionCaption.AutoSize = true;
            lblQuestionCaption.Location = new Point(12, 12);
            lblQuestionCaption.Name = "lblQuestionCaption";
            lblQuestionCaption.Text = "Security question:";
            //
            // lblSecurityQuestion
            //
            lblSecurityQuestion.Location = new Point(12, 32);
            lblSecurityQuestion.Name = "lblSecurityQuestion";
            lblSecurityQuestion.Size = new Size(330, 36);
            //
            // lblAnswer
            //
            lblAnswer.AutoSize = true;
            lblAnswer.Location = new Point(12, 72);
            lblAnswer.Name = "lblAnswer";
            lblAnswer.Text = "Answer";
            //
            // txtSecurityAnswer
            //
            txtSecurityAnswer.Location = new Point(12, 90);
            txtSecurityAnswer.Name = "txtSecurityAnswer";
            txtSecurityAnswer.Size = new Size(260, 23);
            txtSecurityAnswer.TabIndex = 0;
            //
            // lblNewPassword
            //
            lblNewPassword.AutoSize = true;
            lblNewPassword.Location = new Point(12, 122);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Text = "New password (at least 8 characters, with a letter and a number)";
            //
            // txtNewPassword
            //
            txtNewPassword.Location = new Point(12, 140);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(260, 23);
            txtNewPassword.TabIndex = 1;
            txtNewPassword.UseSystemPasswordChar = true;
            //
            // lblConfirmPassword
            //
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Location = new Point(12, 172);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Text = "Type the new password again";
            //
            // txtConfirmPassword
            //
            txtConfirmPassword.Location = new Point(12, 190);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(260, 23);
            txtConfirmPassword.TabIndex = 2;
            txtConfirmPassword.UseSystemPasswordChar = true;
            //
            // lblAdminError
            //
            lblAdminError.Location = new Point(12, 220);
            lblAdminError.Name = "lblAdminError";
            lblAdminError.Size = new Size(330, 36);
            //
            // btnResetPassword
            //
            btnResetPassword.Location = new Point(12, 262);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(120, 28);
            btnResetPassword.TabIndex = 3;
            btnResetPassword.Text = "Reset Password";
            btnResetPassword.Click += btnResetPassword_Click;
            //
            // btnAdminCancel
            //
            btnAdminCancel.Location = new Point(138, 262);
            btnAdminCancel.Name = "btnAdminCancel";
            btnAdminCancel.Size = new Size(90, 28);
            btnAdminCancel.TabIndex = 4;
            btnAdminCancel.Text = "Cancel";
            btnAdminCancel.Click += btnClose_Click;
            //
            // ForgotPasswordForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(380, 304);
            Controls.Add(pnlStep1);
            Controls.Add(pnlRequestSent);
            Controls.Add(pnlAdmin);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ForgotPasswordForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Forgot Password";
            pnlStep1.ResumeLayout(false);
            pnlStep1.PerformLayout();
            pnlRequestSent.ResumeLayout(false);
            pnlAdmin.ResumeLayout(false);
            pnlAdmin.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlStep1;
        private Label lblForgotInfo;
        private TextBox txtForgotUsername;
        private Label lblForgotError;
        private Button btnForgotNext;
        private Button btnForgotCancel;
        private Panel pnlRequestSent;
        private Label lblRequestSent;
        private Button btnForgotClose;
        private Panel pnlAdmin;
        private Label lblQuestionCaption;
        private Label lblSecurityQuestion;
        private Label lblAnswer;
        private TextBox txtSecurityAnswer;
        private Label lblNewPassword;
        private TextBox txtNewPassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private Label lblAdminError;
        private Button btnResetPassword;
        private Button btnAdminCancel;
    }
}
