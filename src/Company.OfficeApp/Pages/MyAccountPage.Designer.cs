namespace Company.OfficeApp.Pages
{
    partial class MyAccountPage
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
            lblMyName = new Label();
            lblMyUsername = new Label();
            lblMyRole = new Label();
            lblAskAdmin = new Label();
            grpChangePassword = new GroupBox();
            lblCurrentPassword = new Label();
            txtCurrentPassword = new TextBox();
            lblNewPassword = new Label();
            txtNewPassword = new TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            btnSavePassword = new Button();
            lblPasswordMsg = new Label();
            grpSecurityQuestion = new GroupBox();
            lblSqStatus = new Label();
            lblQuestion = new Label();
            cboSecurityQuestion = new ComboBox();
            lblSqAnswer = new Label();
            txtSecurityAnswer = new TextBox();
            lblSqCurrentPassword = new Label();
            txtSqCurrentPassword = new TextBox();
            btnSaveSecurityQuestion = new Button();
            lblSqMsg = new Label();
            grpChangePassword.SuspendLayout();
            grpSecurityQuestion.SuspendLayout();
            SuspendLayout();
            //
            // lblMyName
            //
            lblMyName.AutoSize = true;
            lblMyName.Location = new Point(12, 12);
            lblMyName.Name = "lblMyName";
            lblMyName.Text = "Name:";
            //
            // lblMyUsername
            //
            lblMyUsername.AutoSize = true;
            lblMyUsername.Location = new Point(12, 34);
            lblMyUsername.Name = "lblMyUsername";
            lblMyUsername.Text = "Username:";
            //
            // lblMyRole
            //
            lblMyRole.AutoSize = true;
            lblMyRole.Location = new Point(12, 56);
            lblMyRole.Name = "lblMyRole";
            lblMyRole.Text = "Level:";
            //
            // lblAskAdmin  (Employees and Supervisors only)
            //
            lblAskAdmin.AutoSize = true;
            lblAskAdmin.Location = new Point(12, 88);
            lblAskAdmin.Name = "lblAskAdmin";
            lblAskAdmin.Text = "Only Admins can change passwords. If you need a new password, ask an Admin.";
            //
            // grpChangePassword  (Admin only)
            //
            grpChangePassword.Controls.Add(lblCurrentPassword);
            grpChangePassword.Controls.Add(txtCurrentPassword);
            grpChangePassword.Controls.Add(lblNewPassword);
            grpChangePassword.Controls.Add(txtNewPassword);
            grpChangePassword.Controls.Add(lblConfirmPassword);
            grpChangePassword.Controls.Add(txtConfirmPassword);
            grpChangePassword.Controls.Add(btnSavePassword);
            grpChangePassword.Controls.Add(lblPasswordMsg);
            grpChangePassword.Location = new Point(12, 88);
            grpChangePassword.Name = "grpChangePassword";
            grpChangePassword.Size = new Size(400, 232);
            grpChangePassword.TabStop = false;
            grpChangePassword.Text = "Change password";
            //
            // lblCurrentPassword
            //
            lblCurrentPassword.AutoSize = true;
            lblCurrentPassword.Location = new Point(12, 24);
            lblCurrentPassword.Name = "lblCurrentPassword";
            lblCurrentPassword.Text = "Current password";
            //
            // txtCurrentPassword
            //
            txtCurrentPassword.Location = new Point(12, 42);
            txtCurrentPassword.Name = "txtCurrentPassword";
            txtCurrentPassword.Size = new Size(260, 23);
            txtCurrentPassword.TabIndex = 0;
            txtCurrentPassword.UseSystemPasswordChar = true;
            //
            // lblNewPassword
            //
            lblNewPassword.AutoSize = true;
            lblNewPassword.Location = new Point(12, 72);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Text = "New password (at least 8 characters, with a letter and a number)";
            //
            // txtNewPassword
            //
            txtNewPassword.Location = new Point(12, 90);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(260, 23);
            txtNewPassword.TabIndex = 1;
            txtNewPassword.UseSystemPasswordChar = true;
            //
            // lblConfirmPassword
            //
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Location = new Point(12, 120);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Text = "Type the new password again";
            //
            // txtConfirmPassword
            //
            txtConfirmPassword.Location = new Point(12, 138);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(260, 23);
            txtConfirmPassword.TabIndex = 2;
            txtConfirmPassword.UseSystemPasswordChar = true;
            //
            // btnSavePassword
            //
            btnSavePassword.Location = new Point(12, 170);
            btnSavePassword.Name = "btnSavePassword";
            btnSavePassword.Size = new Size(120, 28);
            btnSavePassword.TabIndex = 3;
            btnSavePassword.Text = "Save Password";
            btnSavePassword.Click += btnSavePassword_Click;
            //
            // lblPasswordMsg
            //
            lblPasswordMsg.AutoSize = true;
            lblPasswordMsg.Location = new Point(12, 204);
            lblPasswordMsg.Name = "lblPasswordMsg";
            //
            // grpSecurityQuestion  (Admin only)
            //
            grpSecurityQuestion.Controls.Add(lblSqStatus);
            grpSecurityQuestion.Controls.Add(lblQuestion);
            grpSecurityQuestion.Controls.Add(cboSecurityQuestion);
            grpSecurityQuestion.Controls.Add(lblSqAnswer);
            grpSecurityQuestion.Controls.Add(txtSecurityAnswer);
            grpSecurityQuestion.Controls.Add(lblSqCurrentPassword);
            grpSecurityQuestion.Controls.Add(txtSqCurrentPassword);
            grpSecurityQuestion.Controls.Add(btnSaveSecurityQuestion);
            grpSecurityQuestion.Controls.Add(lblSqMsg);
            grpSecurityQuestion.Location = new Point(12, 332);
            grpSecurityQuestion.Name = "grpSecurityQuestion";
            grpSecurityQuestion.Size = new Size(400, 272);
            grpSecurityQuestion.TabStop = false;
            grpSecurityQuestion.Text = "Security question (to reset your own password)";
            //
            // lblSqStatus
            //
            lblSqStatus.Location = new Point(12, 22);
            lblSqStatus.Name = "lblSqStatus";
            lblSqStatus.Size = new Size(376, 34);
            //
            // lblQuestion
            //
            lblQuestion.AutoSize = true;
            lblQuestion.Location = new Point(12, 60);
            lblQuestion.Name = "lblQuestion";
            lblQuestion.Text = "Question (pick one or type your own)";
            //
            // cboSecurityQuestion
            //
            cboSecurityQuestion.Location = new Point(12, 78);
            cboSecurityQuestion.Name = "cboSecurityQuestion";
            cboSecurityQuestion.Size = new Size(376, 23);
            cboSecurityQuestion.TabIndex = 4;
            //
            // lblSqAnswer
            //
            lblSqAnswer.AutoSize = true;
            lblSqAnswer.Location = new Point(12, 108);
            lblSqAnswer.Name = "lblSqAnswer";
            lblSqAnswer.Text = "Answer (capital letters don't matter)";
            //
            // txtSecurityAnswer
            //
            txtSecurityAnswer.Location = new Point(12, 126);
            txtSecurityAnswer.Name = "txtSecurityAnswer";
            txtSecurityAnswer.Size = new Size(260, 23);
            txtSecurityAnswer.TabIndex = 5;
            txtSecurityAnswer.UseSystemPasswordChar = true;
            //
            // lblSqCurrentPassword
            //
            lblSqCurrentPassword.AutoSize = true;
            lblSqCurrentPassword.Location = new Point(12, 156);
            lblSqCurrentPassword.Name = "lblSqCurrentPassword";
            lblSqCurrentPassword.Text = "Your current password (to confirm it's you)";
            //
            // txtSqCurrentPassword
            //
            txtSqCurrentPassword.Location = new Point(12, 174);
            txtSqCurrentPassword.Name = "txtSqCurrentPassword";
            txtSqCurrentPassword.Size = new Size(260, 23);
            txtSqCurrentPassword.TabIndex = 6;
            txtSqCurrentPassword.UseSystemPasswordChar = true;
            //
            // btnSaveSecurityQuestion
            //
            btnSaveSecurityQuestion.Location = new Point(12, 206);
            btnSaveSecurityQuestion.Name = "btnSaveSecurityQuestion";
            btnSaveSecurityQuestion.Size = new Size(170, 28);
            btnSaveSecurityQuestion.TabIndex = 7;
            btnSaveSecurityQuestion.Text = "Save Security Question";
            btnSaveSecurityQuestion.Click += btnSaveSecurityQuestion_Click;
            //
            // lblSqMsg
            //
            lblSqMsg.AutoSize = true;
            lblSqMsg.Location = new Point(12, 242);
            lblSqMsg.Name = "lblSqMsg";
            //
            // MyAccountPage
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblMyName);
            Controls.Add(lblMyUsername);
            Controls.Add(lblMyRole);
            Controls.Add(lblAskAdmin);
            Controls.Add(grpChangePassword);
            Controls.Add(grpSecurityQuestion);
            Name = "MyAccountPage";
            Size = new Size(640, 620);
            grpChangePassword.ResumeLayout(false);
            grpChangePassword.PerformLayout();
            grpSecurityQuestion.ResumeLayout(false);
            grpSecurityQuestion.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMyName;
        private Label lblMyUsername;
        private Label lblMyRole;
        private Label lblAskAdmin;
        private GroupBox grpChangePassword;
        private Label lblCurrentPassword;
        private TextBox txtCurrentPassword;
        private Label lblNewPassword;
        private TextBox txtNewPassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private Button btnSavePassword;
        private Label lblPasswordMsg;
        private GroupBox grpSecurityQuestion;
        private Label lblSqStatus;
        private Label lblQuestion;
        private ComboBox cboSecurityQuestion;
        private Label lblSqAnswer;
        private TextBox txtSecurityAnswer;
        private Label lblSqCurrentPassword;
        private TextBox txtSqCurrentPassword;
        private Button btnSaveSecurityQuestion;
        private Label lblSqMsg;
    }
}
