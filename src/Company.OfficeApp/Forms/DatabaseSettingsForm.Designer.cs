namespace Company.OfficeApp.Forms
{
    partial class DatabaseSettingsForm
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
            lblHost = new Label();
            txtHost = new TextBox();
            lblPort = new Label();
            numPort = new NumericUpDown();
            lblDatabase = new Label();
            txtDatabase = new TextBox();
            lblDbUser = new Label();
            txtDbUser = new TextBox();
            lblDbPassword = new Label();
            txtDbPassword = new TextBox();
            lblStatus = new Label();
            btnTest = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(38, 32);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Database connection (saved on this computer only)";
            //
            // lblHost
            //
            lblHost.AutoSize = true;
            lblHost.Location = new Point(38, 74);
            lblHost.Name = "lblHost";
            lblHost.Text = "Server address";
            //
            // txtHost
            //
            txtHost.Location = new Point(38, 98);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(299, 23);
            txtHost.TabIndex = 0;
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Location = new Point(38, 140);
            lblPort.Name = "lblPort";
            lblPort.Text = "Port";
            //
            // numPort
            //
            numPort.Location = new Point(38, 164);
            numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPort.Name = "numPort";
            numPort.Size = new Size(115, 23);
            numPort.TabIndex = 1;
            numPort.Value = new decimal(new int[] { 5432, 0, 0, 0 });
            //
            // lblDatabase
            //
            lblDatabase.AutoSize = true;
            lblDatabase.Location = new Point(38, 206);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Text = "Database name";
            //
            // txtDatabase
            //
            txtDatabase.Location = new Point(38, 230);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.Size = new Size(299, 23);
            txtDatabase.TabIndex = 2;
            //
            // lblDbUser
            //
            lblDbUser.AutoSize = true;
            lblDbUser.Location = new Point(38, 272);
            lblDbUser.Name = "lblDbUser";
            lblDbUser.Text = "Database username";
            //
            // txtDbUser
            //
            txtDbUser.Location = new Point(38, 296);
            txtDbUser.Name = "txtDbUser";
            txtDbUser.Size = new Size(299, 23);
            txtDbUser.TabIndex = 3;
            //
            // lblDbPassword
            //
            lblDbPassword.AutoSize = true;
            lblDbPassword.Location = new Point(38, 338);
            lblDbPassword.Name = "lblDbPassword";
            lblDbPassword.Text = "Database password";
            //
            // txtDbPassword
            //
            txtDbPassword.Location = new Point(38, 362);
            txtDbPassword.Name = "txtDbPassword";
            txtDbPassword.Size = new Size(299, 23);
            txtDbPassword.TabIndex = 4;
            txtDbPassword.UseSystemPasswordChar = true;
            //
            // lblStatus
            //
            lblStatus.Location = new Point(38, 404);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(414, 36);
            //
            // btnTest
            //
            btnTest.Location = new Point(38, 460);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(138, 36);
            btnTest.TabIndex = 5;
            btnTest.Text = "Test Connection";
            btnTest.Click += btnTest_Click;
            //
            // btnSave
            //
            btnSave.Location = new Point(183, 460);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(103, 36);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            //
            // btnCancel
            //
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(293, 460);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(103, 36);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            //
            // DatabaseSettingsForm
            //
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(490, 531);
            Controls.Add(lblTitle);
            Controls.Add(lblHost);
            Controls.Add(txtHost);
            Controls.Add(lblPort);
            Controls.Add(numPort);
            Controls.Add(lblDatabase);
            Controls.Add(txtDatabase);
            Controls.Add(lblDbUser);
            Controls.Add(txtDbUser);
            Controls.Add(lblDbPassword);
            Controls.Add(txtDbPassword);
            Controls.Add(lblStatus);
            Controls.Add(btnTest);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DatabaseSettingsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Database Settings";
            ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblHost;
        private TextBox txtHost;
        private Label lblPort;
        private NumericUpDown numPort;
        private Label lblDatabase;
        private TextBox txtDatabase;
        private Label lblDbUser;
        private TextBox txtDbUser;
        private Label lblDbPassword;
        private TextBox txtDbPassword;
        private Label lblStatus;
        private Button btnTest;
        private Button btnSave;
        private Button btnCancel;
    }
}
