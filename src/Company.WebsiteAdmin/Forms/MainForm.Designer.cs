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
            btnNavHome = new Button();
            btnNavProjects = new Button();
            btnNavServices = new Button();
            btnNavAboutUs = new Button();
            btnNavContactUs = new Button();
            btnSignOut = new Button();
            pnlTopBar = new Panel();
            lblPageTitle = new Label();
            lblUserName = new Label();
            btnPublishWebsite = new Button();
            lblLastPublished = new Label();
            pnlContent = new Panel();
            pnlSidebar.SuspendLayout();
            pnlTopBar.SuspendLayout();
            SuspendLayout();
            //
            // pnlSidebar  (left side; hidden buttons take no space)
            //
            pnlSidebar.Controls.Add(btnNavHome);
            pnlSidebar.Controls.Add(btnNavProjects);
            pnlSidebar.Controls.Add(btnNavServices);
            pnlSidebar.Controls.Add(btnNavAboutUs);
            pnlSidebar.Controls.Add(btnNavContactUs);
            pnlSidebar.Controls.Add(btnSignOut);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.FlowDirection = FlowDirection.TopDown;
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Padding = new Padding(6);
            pnlSidebar.Size = new Size(170, 640);
            pnlSidebar.WrapContents = false;
            //
            // btnNavHome
            //
            btnNavHome.Name = "btnNavHome";
            btnNavHome.Size = new Size(150, 30);
            btnNavHome.TabIndex = 0;
            btnNavHome.Text = "Home";
            btnNavHome.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavProjects
            //
            btnNavProjects.Name = "btnNavProjects";
            btnNavProjects.Size = new Size(150, 30);
            btnNavProjects.TabIndex = 1;
            btnNavProjects.Text = "Projects";
            btnNavProjects.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavServices
            //
            btnNavServices.Name = "btnNavServices";
            btnNavServices.Size = new Size(150, 30);
            btnNavServices.TabIndex = 2;
            btnNavServices.Text = "Services";
            btnNavServices.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavAboutUs
            //
            btnNavAboutUs.Name = "btnNavAboutUs";
            btnNavAboutUs.Size = new Size(150, 30);
            btnNavAboutUs.TabIndex = 3;
            btnNavAboutUs.Text = "About Us";
            btnNavAboutUs.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavContactUs
            //
            btnNavContactUs.Name = "btnNavContactUs";
            btnNavContactUs.Size = new Size(150, 30);
            btnNavContactUs.TabIndex = 4;
            btnNavContactUs.Text = "Contact Us";
            btnNavContactUs.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnSignOut
            //
            btnSignOut.Margin = new Padding(3, 20, 3, 3);
            btnSignOut.Name = "btnSignOut";
            btnSignOut.Size = new Size(150, 30);
            btnSignOut.TabIndex = 5;
            btnSignOut.Text = "Log out";
            btnSignOut.TextAlign = ContentAlignment.MiddleLeft;
            btnSignOut.Click += btnSignOut_Click;
            //
            // pnlTopBar
            //
            pnlTopBar.Controls.Add(lblPageTitle);
            pnlTopBar.Controls.Add(lblUserName);
            pnlTopBar.Controls.Add(btnPublishWebsite);
            pnlTopBar.Controls.Add(lblLastPublished);
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
            // btnPublishWebsite  (puts every saved change on the live website)
            //
            btnPublishWebsite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPublishWebsite.Location = new Point(690, 6);
            btnPublishWebsite.Name = "btnPublishWebsite";
            btnPublishWebsite.Size = new Size(130, 28);
            btnPublishWebsite.TabIndex = 6;
            btnPublishWebsite.Text = "Publish Website";
            btnPublishWebsite.Click += btnPublishWebsite_Click;
            //
            // lblLastPublished
            //
            lblLastPublished.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLastPublished.Location = new Point(420, 36);
            lblLastPublished.Name = "lblLastPublished";
            lblLastPublished.Size = new Size(400, 18);
            lblLastPublished.Text = "Last published: -";
            lblLastPublished.TextAlign = ContentAlignment.TopRight;
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
        private Button btnNavHome;
        private Button btnNavProjects;
        private Button btnNavServices;
        private Button btnNavAboutUs;
        private Button btnNavContactUs;
        private Button btnSignOut;
        private Panel pnlTopBar;
        private Label lblPageTitle;
        private Label lblUserName;
        private Button btnPublishWebsite;
        private Label lblLastPublished;
        private Panel pnlContent;
        private System.Windows.Forms.Timer tmrIdle;
    }
}
