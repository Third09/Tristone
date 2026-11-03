namespace Company.WebsiteAdmin.Forms
{
    partial class PublishSettingsForm
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
            lblPublishTitle = new Label();
            lblPublishInfo = new Label();
            lblRepository = new Label();
            txtRepository = new TextBox();
            lblBranch = new Label();
            txtBranch = new TextBox();
            lblToken = new Label();
            txtToken = new TextBox();
            lblTokenHint = new Label();
            btnTestConnection = new Button();
            lblInquiriesTitle = new Label();
            lblInquiriesInfo = new Label();
            lblInquiryScriptUrl = new Label();
            txtInquiryScriptUrl = new TextBox();
            lblInquiryKey = new Label();
            txtInquiryKey = new TextBox();
            btnTestInquiries = new Button();
            lblTokenSteps = new Label();
            lnkOpenGitHubTokens = new LinkLabel();
            lblInquirySteps = new Label();
            lnkOpenScript = new LinkLabel();
            lblPublishSettingsMsg = new Label();
            btnSaveSettings = new Button();
            btnCancelSettings = new Button();
            SuspendLayout();
            //
            // lblPublishTitle
            //
            lblPublishTitle.AutoSize = true;
            lblPublishTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPublishTitle.Location = new Point(32, 20);
            lblPublishTitle.Name = "lblPublishTitle";
            lblPublishTitle.Text = "Publish Website (GitHub)";
            //
            // lblPublishInfo
            //
            lblPublishInfo.Location = new Point(32, 48);
            lblPublishInfo.Name = "lblPublishInfo";
            lblPublishInfo.Size = new Size(470, 40);
            lblPublishInfo.Text = "Publish puts the website's content into its GitHub repository. Netlify then updates the live site in about a minute.";
            //
            // lblRepository
            //
            lblRepository.AutoSize = true;
            lblRepository.Location = new Point(32, 94);
            lblRepository.Name = "lblRepository";
            lblRepository.Text = "Repository (owner/name)";
            //
            // txtRepository
            //
            txtRepository.Location = new Point(32, 118);
            txtRepository.Name = "txtRepository";
            txtRepository.Size = new Size(300, 23);
            txtRepository.TabIndex = 0;
            //
            // lblBranch
            //
            lblBranch.AutoSize = true;
            lblBranch.Location = new Point(352, 94);
            lblBranch.Name = "lblBranch";
            lblBranch.Text = "Branch";
            //
            // txtBranch
            //
            txtBranch.Location = new Point(352, 118);
            txtBranch.Name = "txtBranch";
            txtBranch.Size = new Size(150, 23);
            txtBranch.TabIndex = 1;
            //
            // lblToken
            //
            lblToken.AutoSize = true;
            lblToken.Location = new Point(32, 154);
            lblToken.Name = "lblToken";
            lblToken.Text = "GitHub token";
            //
            // txtToken
            //
            txtToken.Location = new Point(32, 178);
            txtToken.Name = "txtToken";
            txtToken.Size = new Size(470, 23);
            txtToken.TabIndex = 2;
            txtToken.UseSystemPasswordChar = true;
            //
            // lblTokenHint
            //
            lblTokenHint.Location = new Point(32, 206);
            lblTokenHint.Name = "lblTokenHint";
            lblTokenHint.Size = new Size(470, 22);
            lblTokenHint.Text = "Only this repository, permission Contents: Read and write. Kept encrypted on this PC.";
            //
            // btnTestConnection
            //
            btnTestConnection.Location = new Point(32, 316);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(130, 36);
            btnTestConnection.TabIndex = 3;
            btnTestConnection.Text = "Test Publish";
            btnTestConnection.Click += btnTestConnection_Click;
            //
            // lblInquiriesTitle
            //
            lblInquiriesTitle.AutoSize = true;
            lblInquiriesTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblInquiriesTitle.Location = new Point(32, 376);
            lblInquiriesTitle.Name = "lblInquiriesTitle";
            lblInquiriesTitle.Text = "Inquiries (Google Script)";
            //
            // lblInquiriesInfo
            //
            lblInquiriesInfo.Location = new Point(32, 404);
            lblInquiriesInfo.Name = "lblInquiriesInfo";
            lblInquiriesInfo.Size = new Size(470, 40);
            lblInquiriesInfo.Text = "Contact Us gets new inquiries from the website's Google Script. The read key is shown when you run setup in the script.";
            //
            // lblInquiryScriptUrl
            //
            lblInquiryScriptUrl.AutoSize = true;
            lblInquiryScriptUrl.Location = new Point(32, 450);
            lblInquiryScriptUrl.Name = "lblInquiryScriptUrl";
            lblInquiryScriptUrl.Text = "Script web address";
            //
            // txtInquiryScriptUrl
            //
            txtInquiryScriptUrl.Location = new Point(32, 474);
            txtInquiryScriptUrl.Name = "txtInquiryScriptUrl";
            txtInquiryScriptUrl.Size = new Size(470, 23);
            txtInquiryScriptUrl.TabIndex = 4;
            //
            // lblInquiryKey
            //
            lblInquiryKey.AutoSize = true;
            lblInquiryKey.Location = new Point(32, 510);
            lblInquiryKey.Name = "lblInquiryKey";
            lblInquiryKey.Text = "Read key";
            //
            // txtInquiryKey
            //
            txtInquiryKey.Location = new Point(32, 534);
            txtInquiryKey.Name = "txtInquiryKey";
            txtInquiryKey.Size = new Size(470, 23);
            txtInquiryKey.TabIndex = 5;
            txtInquiryKey.UseSystemPasswordChar = true;
            //
            // btnTestInquiries
            //
            btnTestInquiries.Location = new Point(32, 646);
            btnTestInquiries.Name = "btnTestInquiries";
            btnTestInquiries.Size = new Size(130, 36);
            btnTestInquiries.TabIndex = 6;
            btnTestInquiries.Text = "Test Inquiries";
            btnTestInquiries.Click += btnTestInquiries_Click;
            //
            // lblTokenSteps
            //
            lblTokenSteps.Location = new Point(32, 232);
            lblTokenSteps.Name = "lblTokenSteps";
            lblTokenSteps.Size = new Size(470, 56);
            lblTokenSteps.Text = "To make one: on GitHub click your picture > Settings > Developer settings > Fine-grained tokens > Generate new token. Repository access: only tristone_website. Permissions: Contents = Read and write. Copy it right away.";
            //
            // lnkOpenGitHubTokens
            //
            lnkOpenGitHubTokens.AutoSize = true;
            lnkOpenGitHubTokens.Location = new Point(32, 290);
            lnkOpenGitHubTokens.Name = "lnkOpenGitHubTokens";
            lnkOpenGitHubTokens.TabStop = true;
            lnkOpenGitHubTokens.Text = "Open the GitHub token page";
            lnkOpenGitHubTokens.LinkClicked += lnkOpenGitHubTokens_LinkClicked;
            //
            // lblInquirySteps
            //
            lblInquirySteps.Location = new Point(32, 562);
            lblInquirySteps.Name = "lblInquirySteps";
            lblInquirySteps.Size = new Size(470, 56);
            lblInquirySteps.Text = "To get the key: open the script, replace its code with the new code (User Manual: Connecting the inquiries), Save, choose setup and click Run, copy the key from the Execution log. Then Deploy > Manage deployments > Edit > New version > Deploy.";
            //
            // lnkOpenScript
            //
            lnkOpenScript.AutoSize = true;
            lnkOpenScript.Location = new Point(32, 620);
            lnkOpenScript.Name = "lnkOpenScript";
            lnkOpenScript.TabStop = true;
            lnkOpenScript.Text = "Open Google Apps Script";
            lnkOpenScript.LinkClicked += lnkOpenScript_LinkClicked;
            //
            // lblPublishSettingsMsg
            //
            lblPublishSettingsMsg.Location = new Point(32, 700);
            lblPublishSettingsMsg.Name = "lblPublishSettingsMsg";
            lblPublishSettingsMsg.Size = new Size(470, 40);
            //
            // btnSaveSettings
            //
            btnSaveSettings.Location = new Point(276, 748);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(110, 36);
            btnSaveSettings.TabIndex = 7;
            btnSaveSettings.Text = "Save";
            btnSaveSettings.Click += btnSaveSettings_Click;
            //
            // btnCancelSettings
            //
            btnCancelSettings.DialogResult = DialogResult.Cancel;
            btnCancelSettings.Location = new Point(392, 748);
            btnCancelSettings.Name = "btnCancelSettings";
            btnCancelSettings.Size = new Size(110, 36);
            btnCancelSettings.TabIndex = 8;
            btnCancelSettings.Text = "Cancel";
            //
            // PublishSettingsForm
            //
            AcceptButton = btnSaveSettings;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelSettings;
            ClientSize = new Size(534, 806);
            Controls.Add(lblPublishTitle);
            Controls.Add(lblPublishInfo);
            Controls.Add(lblRepository);
            Controls.Add(txtRepository);
            Controls.Add(lblBranch);
            Controls.Add(txtBranch);
            Controls.Add(lblToken);
            Controls.Add(txtToken);
            Controls.Add(lblTokenHint);
            Controls.Add(btnTestConnection);
            Controls.Add(lblInquiriesTitle);
            Controls.Add(lblInquiriesInfo);
            Controls.Add(lblInquiryScriptUrl);
            Controls.Add(txtInquiryScriptUrl);
            Controls.Add(lblInquiryKey);
            Controls.Add(txtInquiryKey);
            Controls.Add(btnTestInquiries);
            Controls.Add(lblTokenSteps);
            Controls.Add(lnkOpenGitHubTokens);
            Controls.Add(lblInquirySteps);
            Controls.Add(lnkOpenScript);
            Controls.Add(lblPublishSettingsMsg);
            Controls.Add(btnSaveSettings);
            Controls.Add(btnCancelSettings);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PublishSettingsForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Website Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPublishTitle;
        private Label lblPublishInfo;
        private Label lblRepository;
        private TextBox txtRepository;
        private Label lblBranch;
        private TextBox txtBranch;
        private Label lblToken;
        private TextBox txtToken;
        private Label lblTokenHint;
        private Button btnTestConnection;
        private Label lblInquiriesTitle;
        private Label lblInquiriesInfo;
        private Label lblInquiryScriptUrl;
        private TextBox txtInquiryScriptUrl;
        private Label lblInquiryKey;
        private TextBox txtInquiryKey;
        private Button btnTestInquiries;
        private Label lblTokenSteps;
        private LinkLabel lnkOpenGitHubTokens;
        private Label lblInquirySteps;
        private LinkLabel lnkOpenScript;
        private Label lblPublishSettingsMsg;
        private Button btnSaveSettings;
        private Button btnCancelSettings;
    }
}
