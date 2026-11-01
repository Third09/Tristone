namespace Company.OfficeApp.Forms
{
    partial class NotificationsForm
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
            lblTitle = new Label();
            dgvPasswordRequests = new DataGridView();
            colName = new DataGridViewTextBoxColumn();
            colUsername = new DataGridViewTextBoxColumn();
            colDepartment = new DataGridViewTextBoxColumn();
            colLevel = new DataGridViewTextBoxColumn();
            colRequestedAt = new DataGridViewTextBoxColumn();
            lblEmpty = new Label();
            btnSetNewPassword = new Button();
            btnDismissRequest = new Button();
            btnCloseNotifications = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPasswordRequests).BeginInit();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(38, 32);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Forgot Password requests";
            //
            // dgvPasswordRequests
            //
            dgvPasswordRequests.AllowUserToAddRows = false;
            dgvPasswordRequests.AllowUserToDeleteRows = false;
            dgvPasswordRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPasswordRequests.Columns.AddRange(new DataGridViewColumn[] { colName, colUsername, colDepartment, colLevel, colRequestedAt });
            dgvPasswordRequests.Location = new Point(38, 61);
            dgvPasswordRequests.MultiSelect = false;
            dgvPasswordRequests.Name = "dgvPasswordRequests";
            dgvPasswordRequests.ReadOnly = true;
            dgvPasswordRequests.RowHeadersVisible = false;
            dgvPasswordRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPasswordRequests.Size = new Size(690, 290);
            dgvPasswordRequests.TabIndex = 0;
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
            // colRequestedAt
            //
            colRequestedAt.DataPropertyName = "RequestedAt";
            colRequestedAt.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy h:mm tt" };
            colRequestedAt.HeaderText = "Requested At";
            colRequestedAt.Name = "colRequestedAt";
            colRequestedAt.ReadOnly = true;
            //
            // lblEmpty
            //
            lblEmpty.AutoSize = true;
            lblEmpty.Location = new Point(38, 359);
            lblEmpty.Name = "lblEmpty";
            lblEmpty.Text = "No waiting requests.";
            lblEmpty.Visible = false;
            //
            // btnSetNewPassword
            //
            btnSetNewPassword.Location = new Point(38, 391);
            btnSetNewPassword.Name = "btnSetNewPassword";
            btnSetNewPassword.Size = new Size(150, 36);
            btnSetNewPassword.TabIndex = 1;
            btnSetNewPassword.Text = "Set New Password";
            btnSetNewPassword.Click += btnSetNewPassword_Click;
            //
            // btnDismissRequest
            //
            btnDismissRequest.Location = new Point(194, 391);
            btnDismissRequest.Name = "btnDismissRequest";
            btnDismissRequest.Size = new Size(103, 36);
            btnDismissRequest.TabIndex = 2;
            btnDismissRequest.Text = "Dismiss";
            btnDismissRequest.Click += btnDismissRequest_Click;
            //
            // btnCloseNotifications
            //
            btnCloseNotifications.Location = new Point(305, 391);
            btnCloseNotifications.Name = "btnCloseNotifications";
            btnCloseNotifications.Size = new Size(103, 36);
            btnCloseNotifications.TabIndex = 3;
            btnCloseNotifications.Text = "Close";
            btnCloseNotifications.Click += btnCloseNotifications_Click;
            //
            // NotificationsForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(766, 460);
            Controls.Add(lblTitle);
            Controls.Add(dgvPasswordRequests);
            Controls.Add(lblEmpty);
            Controls.Add(btnSetNewPassword);
            Controls.Add(btnDismissRequest);
            Controls.Add(btnCloseNotifications);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NotificationsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Notifications";
            Load += NotificationsForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPasswordRequests).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private DataGridView dgvPasswordRequests;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colUsername;
        private DataGridViewTextBoxColumn colDepartment;
        private DataGridViewTextBoxColumn colLevel;
        private DataGridViewTextBoxColumn colRequestedAt;
        private Label lblEmpty;
        private Button btnSetNewPassword;
        private Button btnDismissRequest;
        private Button btnCloseNotifications;
    }
}
