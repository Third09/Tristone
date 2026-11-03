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
            lblPublishInfo = new Label();
            lblRepository = new Label();
            txtRepository = new TextBox();
            lblBranch = new Label();
            txtBranch = new TextBox();
            lblToken = new Label();
            txtToken = new TextBox();
            lblTokenHint = new Label();
            lblPublishSettingsMsg = new Label();
            btnTestConnection = new Button();
            btnSaveSettings = new Button();
            btnCancelSettings = new Button();
            SuspendLayout();
            //
            // lblPublishInfo
            //
            lblPublishInfo.Location = new Point(32, 24);
            lblPublishInfo.Name = "lblPublishInfo";
            lblPublishInfo.Size = new Size(470, 54);
            lblPublishInfo.Text = "Publish Website puts the website's content into its GitHub repository. Netlify then updates the live site in about a minute.";
            //
            // lblRepository
            //
            lblRepository.AutoSize = true;
            lblRepository.Location = new Point(32, 90);
            lblRepository.Name = "lblRepository";
            lblRepository.Text = "Repository (owner/name)";
            //
            // txtRepository
            //
            txtRepository.Location = new Point(32, 114);
            txtRepository.Name = "txtRepository";
            txtRepository.Size = new Size(300, 23);
            txtRepository.TabIndex = 0;
            //
            // lblBranch
            //
            lblBranch.AutoSize = true;
            lblBranch.Location = new Point(352, 90);
            lblBranch.Name = "lblBranch";
            lblBranch.Text = "Branch";
            //
            // txtBranch
            //
            txtBranch.Location = new Point(352, 114);
            txtBranch.Name = "txtBranch";
            txtBranch.Size = new Size(150, 23);
            txtBranch.TabIndex = 1;
            //
            // lblToken
            //
            lblToken.AutoSize = true;
            lblToken.Location = new Point(32, 156);
            lblToken.Name = "lblToken";
            lblToken.Text = "GitHub token";
            //
            // txtToken
            //
            txtToken.Location = new Point(32, 180);
            txtToken.Name = "txtToken";
            txtToken.Size = new Size(470, 23);
            txtToken.TabIndex = 2;
            txtToken.UseSystemPasswordChar = true;
            //
            // lblTokenHint
            //
            lblTokenHint.Location = new Point(32, 210);
            lblTokenHint.Name = "lblTokenHint";
            lblTokenHint.Size = new Size(470, 40);
            lblTokenHint.Text = "Only this repository, permission Contents: Read and write. Kept encrypted on this PC.";
            //
            // lblPublishSettingsMsg
            //
            lblPublishSettingsMsg.Location = new Point(32, 256);
            lblPublishSettingsMsg.Name = "lblPublishSettingsMsg";
            lblPublishSettingsMsg.Size = new Size(470, 40);
            //
            // btnTestConnection
            //
            btnTestConnection.Location = new Point(32, 306);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(110, 36);
            btnTestConnection.TabIndex = 3;
            btnTestConnection.Text = "Test";
            btnTestConnection.Click += btnTestConnection_Click;
            //
            // btnSaveSettings
            //
            btnSaveSettings.Location = new Point(276, 306);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(110, 36);
            btnSaveSettings.TabIndex = 4;
            btnSaveSettings.Text = "Save";
            btnSaveSettings.Click += btnSaveSettings_Click;
            //
            // btnCancelSettings
            //
            btnCancelSettings.DialogResult = DialogResult.Cancel;
            btnCancelSettings.Location = new Point(392, 306);
            btnCancelSettings.Name = "btnCancelSettings";
            btnCancelSettings.Size = new Size(110, 36);
            btnCancelSettings.TabIndex = 5;
            btnCancelSettings.Text = "Cancel";
            //
            // PublishSettingsForm
            //
            AcceptButton = btnSaveSettings;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelSettings;
            ClientSize = new Size(534, 368);
            Controls.Add(lblPublishInfo);
            Controls.Add(lblRepository);
            Controls.Add(txtRepository);
            Controls.Add(lblBranch);
            Controls.Add(txtBranch);
            Controls.Add(lblToken);
            Controls.Add(txtToken);
            Controls.Add(lblTokenHint);
            Controls.Add(lblPublishSettingsMsg);
            Controls.Add(btnTestConnection);
            Controls.Add(btnSaveSettings);
            Controls.Add(btnCancelSettings);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PublishSettingsForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "GitHub Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPublishInfo;
        private Label lblRepository;
        private TextBox txtRepository;
        private Label lblBranch;
        private TextBox txtBranch;
        private Label lblToken;
        private TextBox txtToken;
        private Label lblTokenHint;
        private Label lblPublishSettingsMsg;
        private Button btnTestConnection;
        private Button btnSaveSettings;
        private Button btnCancelSettings;
    }
}
