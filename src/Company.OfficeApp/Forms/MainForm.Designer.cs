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
            lblUserName = new Label();
            lblUserRole = new Label();
            btnSignOut = new Button();
            SuspendLayout();
            //
            // lblUserName
            //
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(12, 12);
            lblUserName.Name = "lblUserName";
            lblUserName.Text = "Signed in as";
            //
            // lblUserRole
            //
            lblUserRole.AutoSize = true;
            lblUserRole.Location = new Point(12, 34);
            lblUserRole.Name = "lblUserRole";
            lblUserRole.Text = "Level";
            //
            // btnSignOut
            //
            btnSignOut.Location = new Point(12, 64);
            btnSignOut.Name = "btnSignOut";
            btnSignOut.Size = new Size(90, 28);
            btnSignOut.TabIndex = 0;
            btnSignOut.Text = "Sign Out";
            btnSignOut.Click += btnSignOut_Click;
            //
            // MainForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 640);
            Controls.Add(lblUserName);
            Controls.Add(lblUserRole);
            Controls.Add(btnSignOut);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tri-Stone Office";
            FormClosing += MainForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUserName;
        private Label lblUserRole;
        private Button btnSignOut;
    }
}
