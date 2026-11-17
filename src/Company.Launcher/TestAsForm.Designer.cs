namespace Company.Launcher
{
    partial class TestAsForm
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
            lblTestAsTitle = new Label();
            lblTestAsNote = new Label();
            txtTestAsSearch = new TextBox();
            dgvTestUsers = new DataGridView();
            btnOpenAsUser = new Button();
            btnCancelTestAs = new Button();
            colTaName = new DataGridViewTextBoxColumn();
            colTaUsername = new DataGridViewTextBoxColumn();
            colTaLevel = new DataGridViewTextBoxColumn();
            colTaDepartment = new DataGridViewTextBoxColumn();
            colTaOpens = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvTestUsers).BeginInit();
            SuspendLayout();
            // 
            // lblTestAsTitle
            // 
            lblTestAsTitle.AutoSize = true;
            lblTestAsTitle.Location = new Point(38, 32);
            lblTestAsTitle.Name = "lblTestAsTitle";
            lblTestAsTitle.Text = "Test as...";
            lblTestAsTitle.Font = new Font("Segoe UI Semibold", 11.5F);
            // 
            // lblTestAsNote
            // 
            lblTestAsNote.AutoSize = true;
            lblTestAsNote.Location = new Point(38, 69);
            lblTestAsNote.Name = "lblTestAsNote";
            lblTestAsNote.MaximumSize = new Size(610, 0);
            lblTestAsNote.Text = "Demo / Test version only: open Tri-Stone as any login, without its password, to try what that person can do. Sign Out brings you back here.";
            // 
            // txtTestAsSearch
            // 
            txtTestAsSearch.Location = new Point(38, 124);
            txtTestAsSearch.Name = "txtTestAsSearch";
            txtTestAsSearch.Size = new Size(288, 23);
            txtTestAsSearch.PlaceholderText = "Search name, login or department";
            txtTestAsSearch.TabIndex = 0;
            txtTestAsSearch.TextChanged += txtTestAsSearch_TextChanged;
            // 
            // dgvTestUsers
            // 
            dgvTestUsers.AllowUserToAddRows = false;
            dgvTestUsers.AllowUserToDeleteRows = false;
            dgvTestUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTestUsers.Columns.AddRange(new DataGridViewColumn[] { colTaName, colTaUsername, colTaLevel, colTaDepartment, colTaOpens });
            dgvTestUsers.MultiSelect = false;
            dgvTestUsers.ReadOnly = true;
            dgvTestUsers.RowHeadersVisible = false;
            dgvTestUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTestUsers.Location = new Point(38, 164);
            dgvTestUsers.Name = "dgvTestUsers";
            dgvTestUsers.Size = new Size(704, 356);
            dgvTestUsers.TabIndex = 1;
            dgvTestUsers.CellDoubleClick += dgvTestUsers_CellDoubleClick;
            // 
            // btnOpenAsUser
            // 
            btnOpenAsUser.Location = new Point(38, 533);
            btnOpenAsUser.Name = "btnOpenAsUser";
            btnOpenAsUser.Size = new Size(218, 36);
            btnOpenAsUser.Text = "Open as this login";
            btnOpenAsUser.TabIndex = 2;
            btnOpenAsUser.Click += btnOpenAsUser_Click;
            // 
            // btnCancelTestAs
            // 
            btnCancelTestAs.Location = new Point(265, 533);
            btnCancelTestAs.Name = "btnCancelTestAs";
            btnCancelTestAs.Size = new Size(138, 36);
            btnCancelTestAs.Text = "Sign Out";
            btnCancelTestAs.TabIndex = 3;
            btnCancelTestAs.Click += btnCancelTestAs_Click;
            // 
            // colTaName
            // 
            colTaName.DataPropertyName = "FullName";
            colTaName.HeaderText = "Name";
            colTaName.Name = "colTaName";
            colTaName.FillWeight = 150;
            // 
            // colTaUsername
            // 
            colTaUsername.DataPropertyName = "Username";
            colTaUsername.HeaderText = "Login";
            colTaUsername.Name = "colTaUsername";
            colTaUsername.FillWeight = 90;
            // 
            // colTaLevel
            // 
            colTaLevel.DataPropertyName = "Level";
            colTaLevel.HeaderText = "Level";
            colTaLevel.Name = "colTaLevel";
            colTaLevel.FillWeight = 80;
            // 
            // colTaDepartment
            // 
            colTaDepartment.DataPropertyName = "Department";
            colTaDepartment.HeaderText = "Department";
            colTaDepartment.Name = "colTaDepartment";
            colTaDepartment.FillWeight = 100;
            // 
            // colTaOpens
            // 
            colTaOpens.DataPropertyName = "Opens";
            colTaOpens.HeaderText = "Opens";
            colTaOpens.Name = "colTaOpens";
            colTaOpens.FillWeight = 120;
            // 
            // TestAsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 652);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(lblTestAsTitle);
            Controls.Add(lblTestAsNote);
            Controls.Add(txtTestAsSearch);
            Controls.Add(dgvTestUsers);
            Controls.Add(btnOpenAsUser);
            Controls.Add(btnCancelTestAs);
            Name = "TestAsForm";
            Load += TestAsForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTestUsers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTestAsTitle;
        private Label lblTestAsNote;
        private TextBox txtTestAsSearch;
        private DataGridView dgvTestUsers;
        private Button btnOpenAsUser;
        private Button btnCancelTestAs;
        private DataGridViewTextBoxColumn colTaName;
        private DataGridViewTextBoxColumn colTaUsername;
        private DataGridViewTextBoxColumn colTaLevel;
        private DataGridViewTextBoxColumn colTaDepartment;
        private DataGridViewTextBoxColumn colTaOpens;
    }
}
