namespace Company.WebsiteAdmin.Forms
{
    partial class LoginForm
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
            lblTitle = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            chkShowPassword = new CheckBox();
            lblError = new Label();
            btnSignIn = new Button();
            btnExit = new Button();
            lblHelp = new Label();
            lnkDbSettings = new LinkLabel();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(12, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Sign in to Tri-Stone Website Admin";
            //
            // lblUsername
            //
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(12, 44);
            lblUsername.Name = "lblUsername";
            lblUsername.Text = "Username";
            //
            // txtUsername
            //
            txtUsername.Location = new Point(12, 62);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(260, 23);
            txtUsername.TabIndex = 0;
            //
            // lblPassword
            //
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(12, 94);
            lblPassword.Name = "lblPassword";
            lblPassword.Text = "Password";
            //
            // txtPassword
            //
            txtPassword.Location = new Point(12, 112);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(260, 23);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            //
            // chkShowPassword
            //
            chkShowPassword.AutoSize = true;
            chkShowPassword.Location = new Point(12, 141);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.TabIndex = 2;
            chkShowPassword.Text = "Show password";
            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;
            //
            // lblError
            //
            lblError.Location = new Point(12, 168);
            lblError.Name = "lblError";
            lblError.Size = new Size(320, 36);
            //
            // btnSignIn
            //
            btnSignIn.Location = new Point(12, 210);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(90, 28);
            btnSignIn.TabIndex = 3;
            btnSignIn.Text = "Sign In";
            btnSignIn.Click += btnSignIn_Click;
            //
            // btnExit
            //
            btnExit.Location = new Point(108, 210);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(90, 28);
            btnExit.TabIndex = 4;
            btnExit.Text = "Exit";
            btnExit.Click += btnExit_Click;
            //
            // lblHelp
            //
            lblHelp.Location = new Point(12, 248);
            lblHelp.Name = "lblHelp";
            lblHelp.Size = new Size(320, 32);
            lblHelp.Text = "Uses a Website Admin account, not your office app login. Forgot your password? Ask another Website Admin.";
            //
            // lnkDbSettings
            //
            lnkDbSettings.AutoSize = true;
            lnkDbSettings.Location = new Point(12, 286);
            lnkDbSettings.Name = "lnkDbSettings";
            lnkDbSettings.TabIndex = 5;
            lnkDbSettings.TabStop = true;
            lnkDbSettings.Text = "Database settings";
            lnkDbSettings.LinkClicked += lnkDbSettings_LinkClicked;
            //
            // LoginForm
            //
            AcceptButton = btnSignIn;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(344, 316);
            Controls.Add(lblTitle);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(chkShowPassword);
            Controls.Add(lblError);
            Controls.Add(btnSignIn);
            Controls.Add(btnExit);
            Controls.Add(lblHelp);
            Controls.Add(lnkDbSettings);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tri-Stone Website Admin - Sign In";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private Label lblError;
        private Button btnSignIn;
        private Button btnExit;
        private Label lblHelp;
        private LinkLabel lnkDbSettings;
    }
}
