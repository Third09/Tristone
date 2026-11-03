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
            picLogo = new PictureBox();
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
            btnWebsiteSettings = new Button();
            btnPreviewWebsite = new Button();
            lblLastPublished = new Label();
            pnlContent = new Panel();
            pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlTopBar.SuspendLayout();
            SuspendLayout();
            //
            // pnlSidebar  (left side; hidden buttons take no space)
            //
            pnlSidebar.Controls.Add(picLogo);
            pnlSidebar.Controls.Add(btnNavHome);
            pnlSidebar.Controls.Add(btnNavProjects);
            pnlSidebar.Controls.Add(btnNavServices);
            pnlSidebar.Controls.Add(btnNavAboutUs);
            pnlSidebar.Controls.Add(btnNavContactUs);
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
            // btnNavHome
            //
            btnNavHome.Margin = new Padding(0, 0, 0, 6);
            btnNavHome.Name = "btnNavHome";
            btnNavHome.Font = new Font("Segoe UI", 10.5F);
            btnNavHome.Size = new Size(218, 44);
            btnNavHome.TabIndex = 0;
            btnNavHome.Text = "Home";
            btnNavHome.TextAlign = ContentAlignment.MiddleLeft;
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
            // btnNavServices
            //
            btnNavServices.Margin = new Padding(0, 0, 0, 6);
            btnNavServices.Name = "btnNavServices";
            btnNavServices.Font = new Font("Segoe UI", 10.5F);
            btnNavServices.Size = new Size(218, 44);
            btnNavServices.TabIndex = 2;
            btnNavServices.Text = "Services";
            btnNavServices.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavAboutUs
            //
            btnNavAboutUs.Margin = new Padding(0, 0, 0, 6);
            btnNavAboutUs.Name = "btnNavAboutUs";
            btnNavAboutUs.Font = new Font("Segoe UI", 10.5F);
            btnNavAboutUs.Size = new Size(218, 44);
            btnNavAboutUs.TabIndex = 3;
            btnNavAboutUs.Text = "About Us";
            btnNavAboutUs.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnNavContactUs
            //
            btnNavContactUs.Margin = new Padding(0, 0, 0, 6);
            btnNavContactUs.Name = "btnNavContactUs";
            btnNavContactUs.Font = new Font("Segoe UI", 10.5F);
            btnNavContactUs.Size = new Size(218, 44);
            btnNavContactUs.TabIndex = 4;
            btnNavContactUs.Text = "Contact Us";
            btnNavContactUs.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnSignOut
            //
            btnSignOut.Margin = new Padding(0, 24, 0, 0);
            btnSignOut.Name = "btnSignOut";
            btnSignOut.Font = new Font("Segoe UI", 10.5F);
            btnSignOut.Size = new Size(218, 44);
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
            pnlTopBar.Controls.Add(btnWebsiteSettings);
            pnlTopBar.Controls.Add(btnPreviewWebsite);
            pnlTopBar.Controls.Add(lblLastPublished);
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
            lblUserName.Text = "Signed in:";
            //
            // btnPublishWebsite  (puts every saved change on the live website)
            //
            btnPublishWebsite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPublishWebsite.Location = new Point(650, 18);
            btnPublishWebsite.Name = "btnPublishWebsite";
            btnPublishWebsite.Font = new Font("Segoe UI", 10F);
            btnPublishWebsite.Size = new Size(170, 40);
            btnPublishWebsite.TabIndex = 6;
            btnPublishWebsite.Text = "Publish Website";
            btnPublishWebsite.Click += btnPublishWebsite_Click;
            //
            // btnWebsiteSettings  (where Publish sends the content and Contact Us gets inquiries; per PC)
            //
            btnWebsiteSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnWebsiteSettings.Location = new Point(504, 18);
            btnWebsiteSettings.Name = "btnWebsiteSettings";
            btnWebsiteSettings.Font = new Font("Segoe UI", 10F);
            btnWebsiteSettings.Size = new Size(136, 40);
            btnWebsiteSettings.TabIndex = 7;
            btnWebsiteSettings.Text = "Website Settings";
            btnWebsiteSettings.Click += btnWebsiteSettings_Click;
            //
            // btnPreviewWebsite  (the whole website with the saved changes, not live)
            //
            btnPreviewWebsite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPreviewWebsite.Location = new Point(358, 18);
            btnPreviewWebsite.Name = "btnPreviewWebsite";
            btnPreviewWebsite.Font = new Font("Segoe UI", 10F);
            btnPreviewWebsite.Size = new Size(136, 40);
            btnPreviewWebsite.TabIndex = 8;
            btnPreviewWebsite.Text = "Preview Website";
            btnPreviewWebsite.Click += btnPreviewWebsite_Click;
            //
            // lblLastPublished
            //
            lblLastPublished.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLastPublished.Location = new Point(420, 64);
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
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlSidebar.ResumeLayout(false);
            pnlTopBar.ResumeLayout(false);
            pnlTopBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel pnlSidebar;
        private PictureBox picLogo;
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
        private Button btnWebsiteSettings;
        private Button btnPreviewWebsite;
        private Label lblLastPublished;
        private Panel pnlContent;
        private System.Windows.Forms.Timer tmrIdle;
    }
}
