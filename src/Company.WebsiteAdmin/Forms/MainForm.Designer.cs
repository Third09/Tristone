namespace Company.WebsiteAdmin.Forms
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
            tmrIdle = new System.Windows.Forms.Timer(components);
            pnlSidebar = new FlowLayoutPanel();
            btnNavPageContent = new Button();
            btnNavAnnouncements = new Button();
            btnNavWebsiteProjects = new Button();
            btnNavDocumentVault = new Button();
            btnNavInquiries = new Button();
            btnNavWebsiteTraffic = new Button();
            btnSignOut = new Button();
            pnlTopBar = new Panel();
            lblPageTitle = new Label();
            lblUserName = new Label();
            pnlContent = new Panel();
            pnlSidebar.SuspendLayout();
            pnlTopBar.SuspendLayout();
            SuspendLayout();
            //
            // pnlSidebar  (left side; hidden buttons take no space)
            //
            pnlSidebar.Controls.Add(btnNavPageContent);
            pnlSidebar.Controls.Add(btnNavAnnouncements);
            pnlSidebar.Controls.Add(btnNavWebsiteProjects);
            pnlSidebar.Controls.Add(btnNavDocumentVault);
            pnlSidebar.Controls.Add(btnNavInquiries);
            pnlSidebar.Controls.Add(btnNavWebsiteTraffic);
            pnlSidebar.Controls.Add(btnSignOut);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.FlowDirection = FlowDirection.TopDown;
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Padding = new Padding(6);
            pnlSidebar.Size = new Size(170, 640);
            pnlSidebar.WrapContents = false;
            //
            // btnNavPageContent
            //
            btnNavPageContent.Name = "btnNavPageContent";
            btnNavPageContent.Size = new Size(150, 30);
            btnNavPageContent.TabIndex = 0;
            btnNavPageContent.Text = "Page Content";
            btnNavPageContent.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavAnnouncements
            //
            btnNavAnnouncements.Name = "btnNavAnnouncements";
            btnNavAnnouncements.Size = new Size(150, 30);
            btnNavAnnouncements.TabIndex = 1;
            btnNavAnnouncements.Text = "Announcements";
            btnNavAnnouncements.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavWebsiteProjects
            //
            btnNavWebsiteProjects.Name = "btnNavWebsiteProjects";
            btnNavWebsiteProjects.Size = new Size(150, 30);
            btnNavWebsiteProjects.TabIndex = 2;
            btnNavWebsiteProjects.Text = "Website Projects";
            btnNavWebsiteProjects.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavDocumentVault
            //
            btnNavDocumentVault.Name = "btnNavDocumentVault";
            btnNavDocumentVault.Size = new Size(150, 30);
            btnNavDocumentVault.TabIndex = 3;
            btnNavDocumentVault.Text = "Document Vault";
            btnNavDocumentVault.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavInquiries
            //
            btnNavInquiries.Name = "btnNavInquiries";
            btnNavInquiries.Size = new Size(150, 30);
            btnNavInquiries.TabIndex = 4;
            btnNavInquiries.Text = "Inquiries";
            btnNavInquiries.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavWebsiteTraffic
            //
            btnNavWebsiteTraffic.Name = "btnNavWebsiteTraffic";
            btnNavWebsiteTraffic.Size = new Size(150, 30);
            btnNavWebsiteTraffic.TabIndex = 5;
            btnNavWebsiteTraffic.Text = "Website Traffic";
            btnNavWebsiteTraffic.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnSignOut
            //
            btnSignOut.Margin = new Padding(3, 20, 3, 3);
            btnSignOut.Name = "btnSignOut";
            btnSignOut.Size = new Size(150, 30);
            btnSignOut.TabIndex = 7;
            btnSignOut.Text = "Sign Out";
            btnSignOut.TextAlign = ContentAlignment.MiddleLeft;
            btnSignOut.Click += btnSignOut_Click;
            //
            // pnlTopBar
            //
            pnlTopBar.Controls.Add(lblPageTitle);
            pnlTopBar.Controls.Add(lblUserName);
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Name = "pnlTopBar";
            pnlTopBar.Size = new Size(830, 56);
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
            lblUserName.Text = "Signed in:";
            //
            // pnlContent  (the chosen page loads here)
            //
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Name = "pnlContent";
            //
            // tmrIdle  (checks once a second for 5 minutes without activity, like the office app)
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
            Text = "Tri-Stone Website Admin";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            pnlSidebar.ResumeLayout(false);
            pnlTopBar.ResumeLayout(false);
            pnlTopBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel pnlSidebar;
        private Button btnNavPageContent;
        private Button btnNavAnnouncements;
        private Button btnNavWebsiteProjects;
        private Button btnNavDocumentVault;
        private Button btnNavInquiries;
        private Button btnNavWebsiteTraffic;
        private Button btnSignOut;
        private Panel pnlTopBar;
        private Label lblPageTitle;
        private Label lblUserName;
        private Panel pnlContent;
        private System.Windows.Forms.Timer tmrIdle;
    }
}
