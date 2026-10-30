namespace Company.WebsiteAdmin.Pages
{
    partial class AccountsTab
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
            dgvAccounts = new DataGridView();
            btnCreateAccount = new Button();
            btnSetAccountPassword = new Button();
            btnUnlockAccount = new Button();
            btnDisableAccount = new Button();
            btnEnableAccount = new Button();
            lblFormMode = new Label();
            lblFullName = new Label();
            txtFullName = new TextBox();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnGeneratePassword = new Button();
            chkIsMain = new CheckBox();
            lblAccountMsg = new Label();
            btnSaveAccount = new Button();
            btnCancelAccount = new Button();
            colUsername = new DataGridViewTextBoxColumn();
            colFullName = new DataGridViewTextBoxColumn();
            colMain = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colLastLogin = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
            SuspendLayout();
            // 
            // dgvAccounts
            // 
            dgvAccounts.Location = new Point(12, 12);
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.Size = new Size(760, 200);
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dgvAccounts.ReadOnly = true;
            dgvAccounts.MultiSelect = false;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAccounts.Columns.AddRange(new DataGridViewColumn[] { colUsername, colFullName, colMain, colStatus, colLastLogin });
            dgvAccounts.SelectionChanged += dgvAccounts_SelectionChanged;
            dgvAccounts.TabIndex = 0;
            // 
            // btnCreateAccount
            // 
            btnCreateAccount.Location = new Point(12, 220);
            btnCreateAccount.Name = "btnCreateAccount";
            btnCreateAccount.Size = new Size(120, 28);
            btnCreateAccount.Text = "Create Account";
            btnCreateAccount.Click += btnCreateAccount_Click;
            btnCreateAccount.TabIndex = 1;
            // 
            // btnSetAccountPassword
            // 
            btnSetAccountPassword.Location = new Point(138, 220);
            btnSetAccountPassword.Name = "btnSetAccountPassword";
            btnSetAccountPassword.Size = new Size(130, 28);
            btnSetAccountPassword.Text = "Set New Password";
            btnSetAccountPassword.Click += btnSetAccountPassword_Click;
            btnSetAccountPassword.TabIndex = 2;
            // 
            // btnUnlockAccount
            // 
            btnUnlockAccount.Location = new Point(274, 220);
            btnUnlockAccount.Name = "btnUnlockAccount";
            btnUnlockAccount.Size = new Size(90, 28);
            btnUnlockAccount.Text = "Unlock";
            btnUnlockAccount.Click += btnUnlockAccount_Click;
            btnUnlockAccount.TabIndex = 3;
            // 
            // btnDisableAccount
            // 
            btnDisableAccount.Location = new Point(370, 220);
            btnDisableAccount.Name = "btnDisableAccount";
            btnDisableAccount.Size = new Size(90, 28);
            btnDisableAccount.Text = "Disable";
            btnDisableAccount.Click += btnDisableAccount_Click;
            btnDisableAccount.TabIndex = 4;
            // 
            // btnEnableAccount
            // 
            btnEnableAccount.Location = new Point(466, 220);
            btnEnableAccount.Name = "btnEnableAccount";
            btnEnableAccount.Size = new Size(90, 28);
            btnEnableAccount.Text = "Enable";
            btnEnableAccount.Click += btnEnableAccount_Click;
            btnEnableAccount.TabIndex = 5;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(12, 264);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New Website Admin account";
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(12, 290);
            lblFullName.Name = "lblFullName";
            lblFullName.Text = "Full name";
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(12, 308);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(300, 23);
            txtFullName.TabIndex = 6;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(12, 338);
            lblUsername.Name = "lblUsername";
            lblUsername.Text = "Username";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(12, 356);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(300, 23);
            txtUsername.TabIndex = 7;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(12, 386);
            lblPassword.Name = "lblPassword";
            lblPassword.Text = "First password (only when creating; later use Set New Password)";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(12, 404);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(260, 23);
            txtPassword.TabIndex = 8;
            // 
            // btnGeneratePassword
            // 
            btnGeneratePassword.Location = new Point(278, 403);
            btnGeneratePassword.Name = "btnGeneratePassword";
            btnGeneratePassword.Size = new Size(90, 25);
            btnGeneratePassword.Text = "Generate";
            btnGeneratePassword.Click += btnGeneratePassword_Click;
            btnGeneratePassword.TabIndex = 9;
            // 
            // chkIsMain
            // 
            chkIsMain.AutoSize = true;
            chkIsMain.Location = new Point(12, 436);
            chkIsMain.Name = "chkIsMain";
            chkIsMain.Text = "Main account (can manage Website Admin accounts)";
            chkIsMain.TabIndex = 10;
            // 
            // lblAccountMsg
            // 
            lblAccountMsg.Location = new Point(12, 464);
            lblAccountMsg.Name = "lblAccountMsg";
            lblAccountMsg.Size = new Size(560, 18);
            lblAccountMsg.Text = "";
            // 
            // btnSaveAccount
            // 
            btnSaveAccount.Location = new Point(12, 486);
            btnSaveAccount.Name = "btnSaveAccount";
            btnSaveAccount.Size = new Size(90, 28);
            btnSaveAccount.Text = "Save";
            btnSaveAccount.Click += btnSaveAccount_Click;
            btnSaveAccount.TabIndex = 11;
            // 
            // btnCancelAccount
            // 
            btnCancelAccount.Location = new Point(108, 486);
            btnCancelAccount.Name = "btnCancelAccount";
            btnCancelAccount.Size = new Size(90, 28);
            btnCancelAccount.Text = "Cancel";
            btnCancelAccount.Click += btnCancelAccount_Click;
            btnCancelAccount.TabIndex = 12;
            // 
            // colUsername
            // 
            colUsername.DataPropertyName = "Username";
            colUsername.HeaderText = "Username";
            colUsername.Name = "colUsername";
            colUsername.ReadOnly = true;
            // 
            // colFullName
            // 
            colFullName.DataPropertyName = "FullName";
            colFullName.HeaderText = "Name";
            colFullName.Name = "colFullName";
            colFullName.ReadOnly = true;
            // 
            // colMain
            // 
            colMain.DataPropertyName = "Main";
            colMain.HeaderText = "Main";
            colMain.Name = "colMain";
            colMain.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // colLastLogin
            // 
            colLastLogin.DataPropertyName = "LastLoginAt";
            colLastLogin.HeaderText = "Last Sign-in";
            colLastLogin.Name = "colLastLogin";
            colLastLogin.ReadOnly = true;
            colLastLogin.DefaultCellStyle.Format = "MMM d, yyyy h:mm tt";
            // 
            // AccountsTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Size = new Size(790, 530);
            Load += AccountsTab_Load;
            Controls.Add(dgvAccounts);
            Controls.Add(btnCreateAccount);
            Controls.Add(btnSetAccountPassword);
            Controls.Add(btnUnlockAccount);
            Controls.Add(btnDisableAccount);
            Controls.Add(btnEnableAccount);
            Controls.Add(lblFormMode);
            Controls.Add(lblFullName);
            Controls.Add(txtFullName);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnGeneratePassword);
            Controls.Add(chkIsMain);
            Controls.Add(lblAccountMsg);
            Controls.Add(btnSaveAccount);
            Controls.Add(btnCancelAccount);
            Name = "AccountsTab";
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvAccounts;
        private Button btnCreateAccount;
        private Button btnSetAccountPassword;
        private Button btnUnlockAccount;
        private Button btnDisableAccount;
        private Button btnEnableAccount;
        private Label lblFormMode;
        private Label lblFullName;
        private TextBox txtFullName;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnGeneratePassword;
        private CheckBox chkIsMain;
        private Label lblAccountMsg;
        private Button btnSaveAccount;
        private Button btnCancelAccount;
        private DataGridViewTextBoxColumn colUsername;
        private DataGridViewTextBoxColumn colFullName;
        private DataGridViewTextBoxColumn colMain;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colLastLogin;
    }
}
