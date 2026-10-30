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
            pnlTopBar = new Panel();
            lblAppTitle = new Label();
            lblUserName = new Label();
            btnSignOut = new Button();
            tabWebsiteAdmin = new TabControl();
            tabPageContent = new TabPage();
            lblPageContentComingSoon = new Label();
            tabAnnouncements = new TabPage();
            lblAnnouncementsComingSoon = new Label();
            tabWebsiteProjects = new TabPage();
            lblWebsiteProjectsComingSoon = new Label();
            tabDocumentVault = new TabPage();
            lblDocumentVaultComingSoon = new Label();
            tabInquiries = new TabPage();
            lblInquiriesComingSoon = new Label();
            tabWebsiteTraffic = new TabPage();
            lblWebsiteTrafficComingSoon = new Label();
            pnlTopBar.SuspendLayout();
            tabWebsiteAdmin.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTopBar
            // 
            pnlTopBar.Controls.Add(lblAppTitle);
            pnlTopBar.Controls.Add(lblUserName);
            pnlTopBar.Controls.Add(btnSignOut);
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Name = "pnlTopBar";
            pnlTopBar.Size = new Size(1000, 44);
            // 
            // lblAppTitle
            // 
            lblAppTitle.AutoSize = true;
            lblAppTitle.Location = new Point(12, 14);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Text = "Tri-Stone Website Admin";
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(220, 14);
            lblUserName.Name = "lblUserName";
            lblUserName.Text = "Signed in:";
            // 
            // btnSignOut
            // 
            btnSignOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSignOut.Location = new Point(898, 8);
            btnSignOut.Name = "btnSignOut";
            btnSignOut.Size = new Size(90, 28);
            btnSignOut.TabIndex = 1;
            btnSignOut.Text = "Sign Out";
            btnSignOut.Click += btnSignOut_Click;
            // 
            // tabWebsiteAdmin
            // 
            tabWebsiteAdmin.Controls.Add(tabPageContent);
            tabWebsiteAdmin.Controls.Add(tabAnnouncements);
            tabWebsiteAdmin.Controls.Add(tabWebsiteProjects);
            tabWebsiteAdmin.Controls.Add(tabDocumentVault);
            tabWebsiteAdmin.Controls.Add(tabInquiries);
            tabWebsiteAdmin.Controls.Add(tabWebsiteTraffic);
            tabWebsiteAdmin.Dock = DockStyle.Fill;
            tabWebsiteAdmin.Name = "tabWebsiteAdmin";
            tabWebsiteAdmin.SelectedIndex = 0;
            tabWebsiteAdmin.TabIndex = 0;
            // 
            // tabPageContent
            // 
            tabPageContent.Controls.Add(lblPageContentComingSoon);
            tabPageContent.Name = "tabPageContent";
            tabPageContent.Padding = new Padding(3);
            tabPageContent.TabIndex = 0;
            tabPageContent.Text = "Page Content";
            // 
            // lblPageContentComingSoon
            // 
            lblPageContentComingSoon.AutoSize = true;
            lblPageContentComingSoon.Location = new Point(12, 12);
            lblPageContentComingSoon.Name = "lblPageContentComingSoon";
            lblPageContentComingSoon.Text = "Page Content: coming next.";
            // 
            // tabAnnouncements
            // 
            tabAnnouncements.Controls.Add(lblAnnouncementsComingSoon);
            tabAnnouncements.Name = "tabAnnouncements";
            tabAnnouncements.Padding = new Padding(3);
            tabAnnouncements.TabIndex = 1;
            tabAnnouncements.Text = "Announcements";
            // 
            // lblAnnouncementsComingSoon
            // 
            lblAnnouncementsComingSoon.AutoSize = true;
            lblAnnouncementsComingSoon.Location = new Point(12, 12);
            lblAnnouncementsComingSoon.Name = "lblAnnouncementsComingSoon";
            lblAnnouncementsComingSoon.Text = "Announcements: coming next.";
            // 
            // tabWebsiteProjects
            // 
            tabWebsiteProjects.Controls.Add(lblWebsiteProjectsComingSoon);
            tabWebsiteProjects.Name = "tabWebsiteProjects";
            tabWebsiteProjects.Padding = new Padding(3);
            tabWebsiteProjects.TabIndex = 2;
            tabWebsiteProjects.Text = "Website Projects";
            // 
            // lblWebsiteProjectsComingSoon
            // 
            lblWebsiteProjectsComingSoon.AutoSize = true;
            lblWebsiteProjectsComingSoon.Location = new Point(12, 12);
            lblWebsiteProjectsComingSoon.Name = "lblWebsiteProjectsComingSoon";
            lblWebsiteProjectsComingSoon.Text = "Website Projects: coming next.";
            // 
            // tabDocumentVault
            // 
            tabDocumentVault.Controls.Add(lblDocumentVaultComingSoon);
            tabDocumentVault.Name = "tabDocumentVault";
            tabDocumentVault.Padding = new Padding(3);
            tabDocumentVault.TabIndex = 3;
            tabDocumentVault.Text = "Document Vault";
            // 
            // lblDocumentVaultComingSoon
            // 
            lblDocumentVaultComingSoon.AutoSize = true;
            lblDocumentVaultComingSoon.Location = new Point(12, 12);
            lblDocumentVaultComingSoon.Name = "lblDocumentVaultComingSoon";
            lblDocumentVaultComingSoon.Text = "Document Vault: coming next.";
            // 
            // tabInquiries
            // 
            tabInquiries.Controls.Add(lblInquiriesComingSoon);
            tabInquiries.Name = "tabInquiries";
            tabInquiries.Padding = new Padding(3);
            tabInquiries.TabIndex = 4;
            tabInquiries.Text = "Inquiries";
            // 
            // lblInquiriesComingSoon
            // 
            lblInquiriesComingSoon.AutoSize = true;
            lblInquiriesComingSoon.Location = new Point(12, 12);
            lblInquiriesComingSoon.Name = "lblInquiriesComingSoon";
            lblInquiriesComingSoon.Text = "Inquiries: coming next.";
            // 
            // tabWebsiteTraffic
            // 
            tabWebsiteTraffic.Controls.Add(lblWebsiteTrafficComingSoon);
            tabWebsiteTraffic.Name = "tabWebsiteTraffic";
            tabWebsiteTraffic.Padding = new Padding(3);
            tabWebsiteTraffic.TabIndex = 5;
            tabWebsiteTraffic.Text = "Website Traffic";
            // 
            // lblWebsiteTrafficComingSoon
            // 
            lblWebsiteTrafficComingSoon.AutoSize = true;
            lblWebsiteTrafficComingSoon.Location = new Point(12, 12);
            lblWebsiteTrafficComingSoon.Name = "lblWebsiteTrafficComingSoon";
            lblWebsiteTrafficComingSoon.Text = "Website Traffic: coming next.";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 640);
            Controls.Add(tabWebsiteAdmin);
            Controls.Add(pnlTopBar);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tri-Stone Website Admin";
            WindowState = FormWindowState.Maximized;
            FormClosing += MainForm_FormClosing;
            pnlTopBar.ResumeLayout(false);
            pnlTopBar.PerformLayout();
            tabPageContent.ResumeLayout(false);
            tabPageContent.PerformLayout();
            tabAnnouncements.ResumeLayout(false);
            tabAnnouncements.PerformLayout();
            tabWebsiteProjects.ResumeLayout(false);
            tabWebsiteProjects.PerformLayout();
            tabDocumentVault.ResumeLayout(false);
            tabDocumentVault.PerformLayout();
            tabInquiries.ResumeLayout(false);
            tabInquiries.PerformLayout();
            tabWebsiteTraffic.ResumeLayout(false);
            tabWebsiteTraffic.PerformLayout();
            tabWebsiteAdmin.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlTopBar;
        private Label lblAppTitle;
        private Label lblUserName;
        private Button btnSignOut;
        private TabControl tabWebsiteAdmin;
        private TabPage tabPageContent;
        private Label lblPageContentComingSoon;
        private TabPage tabAnnouncements;
        private Label lblAnnouncementsComingSoon;
        private TabPage tabWebsiteProjects;
        private Label lblWebsiteProjectsComingSoon;
        private TabPage tabDocumentVault;
        private Label lblDocumentVaultComingSoon;
        private TabPage tabInquiries;
        private Label lblInquiriesComingSoon;
        private TabPage tabWebsiteTraffic;
        private Label lblWebsiteTrafficComingSoon;
    }
}
