namespace Company.OfficeApp.Forms
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            picLogo = new PictureBox();
            lblTitle = new Label();
            lblSubtitle = new Label();
            cardSignIn = new Company.OfficeApp.UI.Card();
            lblUsername = new Label();
            frmUsername = new Company.OfficeApp.UI.InputFrame();
            txtUsername = new TextBox();
            lblPassword = new Label();
            frmPassword = new Company.OfficeApp.UI.InputFrame();
            txtPassword = new TextBox();
            chkShowPassword = new CheckBox();
            lnkForgotPassword = new LinkLabel();
            lblError = new Label();
            btnSignIn = new Button();
            lnkDbSettings = new LinkLabel();
            btnExit = new Button();
            cardSignIn.Controls.Add(lblUsername);
            cardSignIn.Controls.Add(frmUsername);
            frmUsername.Controls.Add(txtUsername);
            cardSignIn.Controls.Add(lblPassword);
            cardSignIn.Controls.Add(frmPassword);
            frmPassword.Controls.Add(txtPassword);
            cardSignIn.Controls.Add(chkShowPassword);
            cardSignIn.Controls.Add(lnkForgotPassword);
            cardSignIn.Controls.Add(lblError);
            cardSignIn.Controls.Add(btnSignIn);
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // picLogo
            // 
            picLogo.Location = new Point(160, 40);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(200, 56);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(20, 116);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(480, 36);
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Text = "Sign in to Tri-Stone";
            // 
            // lblSubtitle
            // 
            lblSubtitle.Location = new Point(20, 154);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(480, 24);
            lblSubtitle.Font = new Font("Segoe UI", 10.5F);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitle.Text = "Sign in to access the company system.";
            // 
            // cardSignIn
            // 
            cardSignIn.Location = new Point(50, 196);
            cardSignIn.Name = "cardSignIn";
            cardSignIn.Size = new Size(420, 340);
            cardSignIn.Padding = new Padding(24);
            cardSignIn.TabIndex = 1;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(28, 26);
            lblUsername.Name = "lblUsername";
            lblUsername.Font = new Font("Segoe UI Semibold", 10F);
            lblUsername.Text = "Username";
            // 
            // frmUsername
            // 
            frmUsername.Location = new Point(28, 50);
            frmUsername.Name = "frmUsername";
            frmUsername.Size = new Size(364, 42);
            frmUsername.TabIndex = 2;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(12, 10);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(340, 23);
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Font = new Font("Segoe UI", 10.5F);
            txtUsername.PlaceholderText = "Enter your username";
            txtUsername.TabIndex = 3;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(28, 104);
            lblPassword.Name = "lblPassword";
            lblPassword.Font = new Font("Segoe UI Semibold", 10F);
            lblPassword.Text = "Password";
            // 
            // frmPassword
            // 
            frmPassword.Location = new Point(28, 128);
            frmPassword.Name = "frmPassword";
            frmPassword.Size = new Size(364, 42);
            frmPassword.TabIndex = 4;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(12, 10);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(340, 23);
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Segoe UI", 10.5F);
            txtPassword.PlaceholderText = "Enter your password";
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TabIndex = 5;
            // 
            // chkShowPassword
            // 
            chkShowPassword.AutoSize = true;
            chkShowPassword.Location = new Point(28, 182);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Text = "Show password";
            chkShowPassword.TabIndex = 6;
            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;
            // 
            // lnkForgotPassword
            // 
            lnkForgotPassword.Location = new Point(282, 183);
            lnkForgotPassword.Name = "lnkForgotPassword";
            lnkForgotPassword.Size = new Size(110, 20);
            lnkForgotPassword.TextAlign = ContentAlignment.MiddleRight;
            lnkForgotPassword.Text = "Forgot Password?";
            lnkForgotPassword.TabIndex = 7;
            lnkForgotPassword.LinkClicked += lnkForgotPassword_LinkClicked;
            // 
            // lblError
            // 
            lblError.Location = new Point(28, 210);
            lblError.Name = "lblError";
            lblError.Size = new Size(364, 40);
            lblError.ForeColor = Color.FromArgb(180, 35, 24);
            // 
            // btnSignIn
            // 
            btnSignIn.Location = new Point(28, 256);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(364, 44);
            btnSignIn.Font = new Font("Segoe UI Semibold", 10.5F);
            btnSignIn.BackColor = Color.FromArgb(17, 17, 17);
            btnSignIn.ForeColor = Color.White;
            btnSignIn.Text = "Sign In";
            btnSignIn.TabIndex = 8;
            btnSignIn.Click += btnSignIn_Click;
            // 
            // lnkDbSettings
            // 
            lnkDbSettings.Location = new Point(50, 560);
            lnkDbSettings.Name = "lnkDbSettings";
            lnkDbSettings.Size = new Size(200, 20);
            lnkDbSettings.Text = "Database settings";
            lnkDbSettings.TabIndex = 9;
            lnkDbSettings.LinkClicked += lnkDbSettings_LinkClicked;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(370, 552);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(100, 36);
            btnExit.Text = "Exit";
            btnExit.TabIndex = 10;
            btnExit.Click += btnExit_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 640);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(picLogo);
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(cardSignIn);
            Controls.Add(lnkDbSettings);
            Controls.Add(btnExit);
            Name = "LoginForm";
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picLogo;
        private Label lblTitle;
        private Label lblSubtitle;
        private Company.OfficeApp.UI.Card cardSignIn;
        private Label lblUsername;
        private Company.OfficeApp.UI.InputFrame frmUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private Company.OfficeApp.UI.InputFrame frmPassword;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private LinkLabel lnkForgotPassword;
        private Label lblError;
        private Button btnSignIn;
        private LinkLabel lnkDbSettings;
        private Button btnExit;
    }
}
