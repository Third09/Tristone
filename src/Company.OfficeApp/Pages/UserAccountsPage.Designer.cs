namespace Company.OfficeApp.Pages
{
    partial class UserAccountsPage
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
            lblSearch = new Label();
            txtUserSearch = new TextBox();
            lblStatusFilter = new Label();
            cboUserStatusFilter = new ComboBox();
            dgvUsers = new DataGridView();
            colName = new DataGridViewTextBoxColumn();
            colUsername = new DataGridViewTextBoxColumn();
            colDepartment = new DataGridViewTextBoxColumn();
            colLevel = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colLastSignIn = new DataGridViewTextBoxColumn();
            btnCreateAccount = new Button();
            btnSetUserPassword = new Button();
            btnUnlockUser = new Button();
            btnDisableUser = new Button();
            btnEnableUser = new Button();
            lblFormMode = new Label();
            lblUserEmployee = new Label();
            cboUserEmployee = new ComboBox();
            lblNewUsername = new Label();
            txtNewUsername = new TextBox();
            lblNewUserPassword = new Label();
            txtNewUserPassword = new TextBox();
            btnGenerateUserPassword = new Button();
            lblUserLevel = new Label();
            cboUserLevel = new ComboBox();
            lblUserDepartment = new Label();
            cboUserDepartment = new ComboBox();
            lblUserFormMsg = new Label();
            btnSaveUser = new Button();
            btnCancelUser = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            SuspendLayout();
            //
            // lblSearch
            //
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(12, 15);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search";
            //
            // txtUserSearch
            //
            txtUserSearch.Location = new Point(62, 12);
            txtUserSearch.Name = "txtUserSearch";
            txtUserSearch.Size = new Size(200, 23);
            txtUserSearch.TabIndex = 0;
            //
            // lblStatusFilter
            //
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(276, 15);
            lblStatusFilter.Name = "lblStatusFilter";
            lblStatusFilter.Text = "Status";
            //
            // cboUserStatusFilter
            //
            cboUserStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUserStatusFilter.Location = new Point(322, 12);
            cboUserStatusFilter.Name = "cboUserStatusFilter";
            cboUserStatusFilter.Size = new Size(120, 23);
            cboUserStatusFilter.TabIndex = 1;
            cboUserStatusFilter.SelectedIndexChanged += Filter_Changed;
            //
            // dgvUsers
            //
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.Columns.AddRange(new DataGridViewColumn[] { colName, colUsername, colDepartment, colLevel, colStatus, colLastSignIn });
            dgvUsers.Location = new Point(12, 44);
            dgvUsers.MultiSelect = false;
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new Size(700, 220);
            dgvUsers.TabIndex = 2;
            dgvUsers.SelectionChanged += dgvUsers_SelectionChanged;
            //
            // colName
            //
            colName.DataPropertyName = "FullName";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            colName.ReadOnly = true;
            //
            // colUsername
            //
            colUsername.DataPropertyName = "Username";
            colUsername.HeaderText = "Username";
            colUsername.Name = "colUsername";
            colUsername.ReadOnly = true;
            //
            // colDepartment
            //
            colDepartment.DataPropertyName = "Department";
            colDepartment.HeaderText = "Department";
            colDepartment.Name = "colDepartment";
            colDepartment.ReadOnly = true;
            //
            // colLevel
            //
            colLevel.DataPropertyName = "Level";
            colLevel.HeaderText = "Level";
            colLevel.Name = "colLevel";
            colLevel.ReadOnly = true;
            //
            // colStatus
            //
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            //
            // colLastSignIn
            //
            colLastSignIn.DataPropertyName = "LastLoginAt";
            colLastSignIn.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy h:mm tt", NullValue = "Never" };
            colLastSignIn.HeaderText = "Last Sign In";
            colLastSignIn.Name = "colLastSignIn";
            colLastSignIn.ReadOnly = true;
            //
            // btnCreateAccount
            //
            btnCreateAccount.Location = new Point(12, 272);
            btnCreateAccount.Name = "btnCreateAccount";
            btnCreateAccount.Size = new Size(120, 28);
            btnCreateAccount.TabIndex = 3;
            btnCreateAccount.Text = "Create Account";
            btnCreateAccount.Click += btnCreateAccount_Click;
            //
            // btnSetUserPassword
            //
            btnSetUserPassword.Location = new Point(138, 272);
            btnSetUserPassword.Name = "btnSetUserPassword";
            btnSetUserPassword.Size = new Size(130, 28);
            btnSetUserPassword.TabIndex = 4;
            btnSetUserPassword.Text = "Set New Password";
            btnSetUserPassword.Click += btnSetUserPassword_Click;
            //
            // btnUnlockUser
            //
            btnUnlockUser.Location = new Point(274, 272);
            btnUnlockUser.Name = "btnUnlockUser";
            btnUnlockUser.Size = new Size(90, 28);
            btnUnlockUser.TabIndex = 5;
            btnUnlockUser.Text = "Unlock";
            btnUnlockUser.Click += btnUnlockUser_Click;
            //
            // btnDisableUser
            //
            btnDisableUser.Location = new Point(370, 272);
            btnDisableUser.Name = "btnDisableUser";
            btnDisableUser.Size = new Size(90, 28);
            btnDisableUser.TabIndex = 6;
            btnDisableUser.Text = "Disable";
            btnDisableUser.Click += btnDisableUser_Click;
            //
            // btnEnableUser
            //
            btnEnableUser.Location = new Point(466, 272);
            btnEnableUser.Name = "btnEnableUser";
            btnEnableUser.Size = new Size(90, 28);
            btnEnableUser.TabIndex = 7;
            btnEnableUser.Text = "Enable";
            btnEnableUser.Click += btnEnableUser_Click;
            //
            // lblFormMode
            //
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(12, 316);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New account";
            //
            // lblUserEmployee
            //
            lblUserEmployee.AutoSize = true;
            lblUserEmployee.Location = new Point(12, 342);
            lblUserEmployee.Name = "lblUserEmployee";
            lblUserEmployee.Text = "Employee (office staff without a login; add them in HR & Payroll first)";
            //
            // cboUserEmployee
            //
            cboUserEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUserEmployee.Location = new Point(12, 360);
            cboUserEmployee.Name = "cboUserEmployee";
            cboUserEmployee.Size = new Size(300, 23);
            cboUserEmployee.TabIndex = 8;
            cboUserEmployee.SelectedIndexChanged += cboUserEmployee_SelectedIndexChanged;
            //
            // lblNewUsername
            //
            lblNewUsername.AutoSize = true;
            lblNewUsername.Location = new Point(12, 390);
            lblNewUsername.Name = "lblNewUsername";
            lblNewUsername.Text = "Username (letters, numbers, . - _ ; no spaces)";
            //
            // txtNewUsername
            //
            txtNewUsername.Location = new Point(12, 408);
            txtNewUsername.Name = "txtNewUsername";
            txtNewUsername.Size = new Size(260, 23);
            txtNewUsername.TabIndex = 9;
            //
            // lblNewUserPassword
            //
            lblNewUserPassword.AutoSize = true;
            lblNewUserPassword.Location = new Point(12, 438);
            lblNewUserPassword.Name = "lblNewUserPassword";
            lblNewUserPassword.Text = "First password (new accounts only; 8+ characters, a letter and a number)";
            //
            // txtNewUserPassword
            //
            txtNewUserPassword.Location = new Point(12, 456);
            txtNewUserPassword.Name = "txtNewUserPassword";
            txtNewUserPassword.Size = new Size(260, 23);
            txtNewUserPassword.TabIndex = 10;
            //
            // btnGenerateUserPassword
            //
            btnGenerateUserPassword.Location = new Point(278, 455);
            btnGenerateUserPassword.Name = "btnGenerateUserPassword";
            btnGenerateUserPassword.Size = new Size(90, 25);
            btnGenerateUserPassword.TabIndex = 11;
            btnGenerateUserPassword.Text = "Generate";
            btnGenerateUserPassword.Click += btnGenerateUserPassword_Click;
            //
            // lblUserLevel
            //
            lblUserLevel.AutoSize = true;
            lblUserLevel.Location = new Point(12, 486);
            lblUserLevel.Name = "lblUserLevel";
            lblUserLevel.Text = "Level";
            //
            // cboUserLevel
            //
            cboUserLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUserLevel.Location = new Point(12, 504);
            cboUserLevel.Name = "cboUserLevel";
            cboUserLevel.Size = new Size(160, 23);
            cboUserLevel.TabIndex = 12;
            cboUserLevel.SelectedIndexChanged += cboUserLevel_SelectedIndexChanged;
            //
            // lblUserDepartment
            //
            lblUserDepartment.AutoSize = true;
            lblUserDepartment.Location = new Point(12, 534);
            lblUserDepartment.Name = "lblUserDepartment";
            lblUserDepartment.Text = "Department (from the employee; none for Admins)";
            //
            // cboUserDepartment
            //
            cboUserDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUserDepartment.Location = new Point(12, 552);
            cboUserDepartment.Name = "cboUserDepartment";
            cboUserDepartment.Size = new Size(160, 23);
            cboUserDepartment.Enabled = false;
            cboUserDepartment.TabIndex = 13;
            //
            // lblUserFormMsg
            //
            lblUserFormMsg.AutoSize = true;
            lblUserFormMsg.Location = new Point(12, 582);
            lblUserFormMsg.Name = "lblUserFormMsg";
            //
            // btnSaveUser
            //
            btnSaveUser.Location = new Point(12, 604);
            btnSaveUser.Name = "btnSaveUser";
            btnSaveUser.Size = new Size(90, 28);
            btnSaveUser.TabIndex = 14;
            btnSaveUser.Text = "Save";
            btnSaveUser.Click += btnSaveUser_Click;
            //
            // btnCancelUser
            //
            btnCancelUser.Location = new Point(108, 604);
            btnCancelUser.Name = "btnCancelUser";
            btnCancelUser.Size = new Size(90, 28);
            btnCancelUser.TabIndex = 15;
            btnCancelUser.Text = "Cancel";
            btnCancelUser.Click += btnCancelUser_Click;
            //
            // UserAccountsPage
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblSearch);
            Controls.Add(txtUserSearch);
            Controls.Add(lblStatusFilter);
            Controls.Add(cboUserStatusFilter);
            Controls.Add(dgvUsers);
            Controls.Add(btnCreateAccount);
            Controls.Add(btnSetUserPassword);
            Controls.Add(btnUnlockUser);
            Controls.Add(btnDisableUser);
            Controls.Add(btnEnableUser);
            Controls.Add(lblFormMode);
            Controls.Add(lblUserEmployee);
            Controls.Add(cboUserEmployee);
            Controls.Add(lblNewUsername);
            Controls.Add(txtNewUsername);
            Controls.Add(lblNewUserPassword);
            Controls.Add(txtNewUserPassword);
            Controls.Add(btnGenerateUserPassword);
            Controls.Add(lblUserLevel);
            Controls.Add(cboUserLevel);
            Controls.Add(lblUserDepartment);
            Controls.Add(cboUserDepartment);
            Controls.Add(lblUserFormMsg);
            Controls.Add(btnSaveUser);
            Controls.Add(btnCancelUser);
            Name = "UserAccountsPage";
            Size = new Size(730, 650);
            Load += UserAccountsPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSearch;
        private TextBox txtUserSearch;
        private Label lblStatusFilter;
        private ComboBox cboUserStatusFilter;
        private DataGridView dgvUsers;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colUsername;
        private DataGridViewTextBoxColumn colDepartment;
        private DataGridViewTextBoxColumn colLevel;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colLastSignIn;
        private Button btnCreateAccount;
        private Button btnSetUserPassword;
        private Button btnUnlockUser;
        private Button btnDisableUser;
        private Button btnEnableUser;
        private Label lblFormMode;
        private Label lblUserEmployee;
        private ComboBox cboUserEmployee;
        private Label lblNewUsername;
        private TextBox txtNewUsername;
        private Label lblNewUserPassword;
        private TextBox txtNewUserPassword;
        private Button btnGenerateUserPassword;
        private Label lblUserLevel;
        private ComboBox cboUserLevel;
        private Label lblUserDepartment;
        private ComboBox cboUserDepartment;
        private Label lblUserFormMsg;
        private Button btnSaveUser;
        private Button btnCancelUser;
    }
}
