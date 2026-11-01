namespace Company.OfficeApp.Forms
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            pnlSidebar = new FlowLayoutPanel();
            picLogo = new PictureBox();
            btnNavDashboard = new Button();
            btnNavProjects = new Button();
            btnNavHr = new Button();
            btnNavInventory = new Button();
            btnNavSubcontractors = new Button();
            btnNavReceipts = new Button();

            btnNavUsers = new Button();
            btnNavMyAccount = new Button();
            btnSignOut = new Button();
            pnlTopBar = new Panel();
            lblPageTitle = new Label();
            lblUserName = new Label();
            lblUserRole = new Label();
            btnNotifications = new Button();
            pnlContent = new Panel();
            tmrIdle = new System.Windows.Forms.Timer(components);
            pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlTopBar.SuspendLayout();
            SuspendLayout();
            //
            // pnlSidebar  (left side; hidden buttons take no space)
            //
            pnlSidebar.Controls.Add(picLogo);
            pnlSidebar.Controls.Add(btnNavDashboard);
            pnlSidebar.Controls.Add(btnNavProjects);
            pnlSidebar.Controls.Add(btnNavHr);
            pnlSidebar.Controls.Add(btnNavInventory);
            pnlSidebar.Controls.Add(btnNavSubcontractors);
            pnlSidebar.Controls.Add(btnNavReceipts);

            pnlSidebar.Controls.Add(btnNavUsers);
            pnlSidebar.Controls.Add(btnNavMyAccount);
            pnlSidebar.Controls.Add(btnSignOut);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.FlowDirection = FlowDirection.TopDown;
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Padding = new Padding(16, 20, 16, 16);
            pnlSidebar.Size = new Size(250, 640);
            pnlSidebar.WrapContents = false;
            //
            // picLogo  (Tri-Stone logo; the picture is set in code from Resources\logo-dark.png)
            //
            picLogo.Margin = new Padding(4, 0, 0, 24);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(200, 56);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabStop = false;
            //
            // btnNavDashboard
            //
            btnNavDashboard.Margin = new Padding(0, 0, 0, 6);
            btnNavDashboard.Name = "btnNavDashboard";
            btnNavDashboard.Font = new Font("Segoe UI", 10.5F);
            btnNavDashboard.Size = new Size(218, 44);
            btnNavDashboard.TabIndex = 0;
            btnNavDashboard.Text = "Dashboard";
            btnNavDashboard.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavProjects
            //
            btnNavProjects.Margin = new Padding(0, 0, 0, 6);
            btnNavProjects.Name = "btnNavProjects";
            btnNavProjects.Font = new Font("Segoe UI", 10.5F);
            btnNavProjects.Size = new Size(218, 44);
            btnNavProjects.TabIndex = 1;
            btnNavProjects.Text = "Projects";
            btnNavProjects.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavHr
            //
            btnNavHr.Margin = new Padding(0, 0, 0, 6);
            btnNavHr.Name = "btnNavHr";
            btnNavHr.Font = new Font("Segoe UI", 10.5F);
            btnNavHr.Size = new Size(218, 44);
            btnNavHr.TabIndex = 2;
            btnNavHr.Text = "HR && Payroll";
            btnNavHr.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavInventory
            //
            btnNavInventory.Margin = new Padding(0, 0, 0, 6);
            btnNavInventory.Name = "btnNavInventory";
            btnNavInventory.Font = new Font("Segoe UI", 10.5F);
            btnNavInventory.Size = new Size(218, 44);
            btnNavInventory.TabIndex = 3;
            btnNavInventory.Text = "Inventory";
            btnNavInventory.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavSubcontractors
            //
            btnNavSubcontractors.Margin = new Padding(0, 0, 0, 6);
            btnNavSubcontractors.Name = "btnNavSubcontractors";
            btnNavSubcontractors.Font = new Font("Segoe UI", 10.5F);
            btnNavSubcontractors.Size = new Size(218, 44);
            btnNavSubcontractors.TabIndex = 4;
            btnNavSubcontractors.Text = "Subcontractors";
            btnNavSubcontractors.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavReceipts
            //
            btnNavReceipts.Margin = new Padding(0, 0, 0, 6);
            btnNavReceipts.Name = "btnNavReceipts";
            btnNavReceipts.Font = new Font("Segoe UI", 10.5F);
            btnNavReceipts.Size = new Size(218, 44);
            btnNavReceipts.TabIndex = 5;
            btnNavReceipts.Text = "Receipts";
            btnNavReceipts.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavUsers
            //
            btnNavUsers.Margin = new Padding(0, 0, 0, 6);
            btnNavUsers.Name = "btnNavUsers";
            btnNavUsers.Font = new Font("Segoe UI", 10.5F);
            btnNavUsers.Size = new Size(218, 44);
            btnNavUsers.TabIndex = 7;
            btnNavUsers.Text = "User Accounts";
            btnNavUsers.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavMyAccount
            //
            btnNavMyAccount.Margin = new Padding(0, 0, 0, 6);
            btnNavMyAccount.Name = "btnNavMyAccount";
            btnNavMyAccount.Font = new Font("Segoe UI", 10.5F);
            btnNavMyAccount.Size = new Size(218, 44);
            btnNavMyAccount.TabIndex = 8;
            btnNavMyAccount.Text = "My Account";
            btnNavMyAccount.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnSignOut
            //
            btnSignOut.Margin = new Padding(0, 24, 0, 0);
            btnSignOut.Name = "btnSignOut";
            btnSignOut.Font = new Font("Segoe UI", 10.5F);
            btnSignOut.Size = new Size(218, 44);
            btnSignOut.TabIndex = 9;
            btnSignOut.Text = "Sign Out";
            btnSignOut.TextAlign = ContentAlignment.MiddleLeft;
            btnSignOut.Click += btnSignOut_Click;
            //
            // pnlTopBar
            //
            pnlTopBar.Controls.Add(lblPageTitle);
            pnlTopBar.Controls.Add(lblUserName);
            pnlTopBar.Controls.Add(lblUserRole);
            pnlTopBar.Controls.Add(btnNotifications);
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Name = "pnlTopBar";
            pnlTopBar.Size = new Size(830, 92);
            //
            // lblPageTitle
            //
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblPageTitle.Location = new Point(24, 10);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Text = "Page title";
            //
            // lblUserName
            //
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(26, 52);
            lblUserName.Name = "lblUserName";
            lblUserName.Text = "User name";
            //
            // lblUserRole
            //
            lblUserRole.AutoSize = true;
            lblUserRole.Location = new Point(26, 70);
            lblUserRole.Name = "lblUserRole";
            lblUserRole.Text = "Department - Level";
            //
            // btnNotifications  (Admin only)
            //
            btnNotifications.Location = new Point(320, 24);
            btnNotifications.Name = "btnNotifications";
            btnNotifications.Size = new Size(160, 28);
            btnNotifications.TabIndex = 10;
            btnNotifications.Text = "Password requests";
            btnNotifications.Click += btnNotifications_Click;
            //
            // pnlContent  (the chosen page loads here)
            //
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Name = "pnlContent";
            //
            // tmrIdle  (checks once a second for 5 minutes without activity)
            //
            tmrIdle.Interval = 1000;
            tmrIdle.Tick += tmrIdle_Tick;
            //
            // MainForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 640);
            Controls.Add(pnlContent);
            Controls.Add(pnlTopBar);
            Controls.Add(pnlSidebar);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            WindowState = FormWindowState.Maximized;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tri-Stone Office";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlSidebar.ResumeLayout(false);
            pnlTopBar.ResumeLayout(false);
            pnlTopBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel pnlSidebar;
        private PictureBox picLogo;
        private Button btnNavDashboard;
        private Button btnNavProjects;
        private Button btnNavHr;
        private Button btnNavInventory;
        private Button btnNavSubcontractors;
        private Button btnNavReceipts;

        private Button btnNavUsers;
        private Button btnNavMyAccount;
        private Button btnSignOut;
        private Panel pnlTopBar;
        private Label lblPageTitle;
        private Label lblUserName;
        private Label lblUserRole;
        private Button btnNotifications;
        private Panel pnlContent;
        private System.Windows.Forms.Timer tmrIdle;
    }
}
