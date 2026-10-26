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
            btnNavDashboard = new Button();
            btnNavProjects = new Button();
            btnNavHr = new Button();
            btnNavInventory = new Button();
            btnNavSubcontractors = new Button();
            btnNavReceipts = new Button();
            btnNavWebsite = new Button();
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
            pnlTopBar.SuspendLayout();
            SuspendLayout();
            //
            // pnlSidebar  (left side; hidden buttons take no space)
            //
            pnlSidebar.Controls.Add(btnNavDashboard);
            pnlSidebar.Controls.Add(btnNavProjects);
            pnlSidebar.Controls.Add(btnNavHr);
            pnlSidebar.Controls.Add(btnNavInventory);
            pnlSidebar.Controls.Add(btnNavSubcontractors);
            pnlSidebar.Controls.Add(btnNavReceipts);
            pnlSidebar.Controls.Add(btnNavWebsite);
            pnlSidebar.Controls.Add(btnNavUsers);
            pnlSidebar.Controls.Add(btnNavMyAccount);
            pnlSidebar.Controls.Add(btnSignOut);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.FlowDirection = FlowDirection.TopDown;
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Padding = new Padding(6);
            pnlSidebar.Size = new Size(170, 640);
            pnlSidebar.WrapContents = false;
            //
            // btnNavDashboard
            //
            btnNavDashboard.Name = "btnNavDashboard";
            btnNavDashboard.Size = new Size(150, 30);
            btnNavDashboard.TabIndex = 0;
            btnNavDashboard.Text = "Dashboard";
            btnNavDashboard.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavProjects
            //
            btnNavProjects.Name = "btnNavProjects";
            btnNavProjects.Size = new Size(150, 30);
            btnNavProjects.TabIndex = 1;
            btnNavProjects.Text = "Projects";
            btnNavProjects.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavHr
            //
            btnNavHr.Name = "btnNavHr";
            btnNavHr.Size = new Size(150, 30);
            btnNavHr.TabIndex = 2;
            btnNavHr.Text = "HR && Payroll";
            btnNavHr.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavInventory
            //
            btnNavInventory.Name = "btnNavInventory";
            btnNavInventory.Size = new Size(150, 30);
            btnNavInventory.TabIndex = 3;
            btnNavInventory.Text = "Inventory";
            btnNavInventory.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavSubcontractors
            //
            btnNavSubcontractors.Name = "btnNavSubcontractors";
            btnNavSubcontractors.Size = new Size(150, 30);
            btnNavSubcontractors.TabIndex = 4;
            btnNavSubcontractors.Text = "Subcontractors";
            btnNavSubcontractors.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavReceipts
            //
            btnNavReceipts.Name = "btnNavReceipts";
            btnNavReceipts.Size = new Size(150, 30);
            btnNavReceipts.TabIndex = 5;
            btnNavReceipts.Text = "Receipts";
            btnNavReceipts.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavWebsite
            //
            btnNavWebsite.Name = "btnNavWebsite";
            btnNavWebsite.Size = new Size(150, 30);
            btnNavWebsite.TabIndex = 6;
            btnNavWebsite.Text = "Website Admin";
            btnNavWebsite.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavUsers
            //
            btnNavUsers.Name = "btnNavUsers";
            btnNavUsers.Size = new Size(150, 30);
            btnNavUsers.TabIndex = 7;
            btnNavUsers.Text = "User Accounts";
            btnNavUsers.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavMyAccount
            //
            btnNavMyAccount.Name = "btnNavMyAccount";
            btnNavMyAccount.Size = new Size(150, 30);
            btnNavMyAccount.TabIndex = 8;
            btnNavMyAccount.Text = "My Account";
            btnNavMyAccount.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnSignOut
            //
            btnSignOut.Margin = new Padding(3, 20, 3, 3);
            btnSignOut.Name = "btnSignOut";
            btnSignOut.Size = new Size(150, 30);
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
            pnlTopBar.Size = new Size(830, 76);
            //
            // lblPageTitle
            //
            lblPageTitle.AutoSize = true;
            lblPageTitle.Location = new Point(12, 8);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Text = "Page title";
            //
            // lblUserName
            //
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(12, 30);
            lblUserName.Name = "lblUserName";
            lblUserName.Text = "User name";
            //
            // lblUserRole
            //
            lblUserRole.AutoSize = true;
            lblUserRole.Location = new Point(12, 50);
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
            MinimumSize = new Size(800, 500);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tri-Stone Office";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            pnlSidebar.ResumeLayout(false);
            pnlTopBar.ResumeLayout(false);
            pnlTopBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel pnlSidebar;
        private Button btnNavDashboard;
        private Button btnNavProjects;
        private Button btnNavHr;
        private Button btnNavInventory;
        private Button btnNavSubcontractors;
        private Button btnNavReceipts;
        private Button btnNavWebsite;
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
