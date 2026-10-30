namespace Company.Launcher
{
    partial class AppChoiceForm
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
            lblChoiceWho = new Label();
            lblChoiceQuestion = new Label();
            btnOpenOffice = new Button();
            btnOpenWebsite = new Button();
            btnChoiceSignOut = new Button();
            SuspendLayout();
            //
            // lblChoiceWho
            //
            lblChoiceWho.AutoSize = true;
            lblChoiceWho.Location = new Point(12, 12);
            lblChoiceWho.Name = "lblChoiceWho";
            lblChoiceWho.Text = "Signed in as (name) (Admin).";
            //
            // lblChoiceQuestion
            //
            lblChoiceQuestion.AutoSize = true;
            lblChoiceQuestion.Location = new Point(12, 36);
            lblChoiceQuestion.Name = "lblChoiceQuestion";
            lblChoiceQuestion.Text = "Which app do you want to open?";
            //
            // btnOpenOffice
            //
            btnOpenOffice.Location = new Point(12, 66);
            btnOpenOffice.Name = "btnOpenOffice";
            btnOpenOffice.Size = new Size(150, 32);
            btnOpenOffice.TabIndex = 0;
            btnOpenOffice.Text = "Office App";
            btnOpenOffice.Click += btnOpenOffice_Click;
            //
            // btnOpenWebsite
            //
            btnOpenWebsite.Location = new Point(168, 66);
            btnOpenWebsite.Name = "btnOpenWebsite";
            btnOpenWebsite.Size = new Size(150, 32);
            btnOpenWebsite.TabIndex = 1;
            btnOpenWebsite.Text = "Website Admin";
            btnOpenWebsite.Click += btnOpenWebsite_Click;
            //
            // btnChoiceSignOut
            //
            btnChoiceSignOut.Location = new Point(12, 108);
            btnChoiceSignOut.Name = "btnChoiceSignOut";
            btnChoiceSignOut.Size = new Size(90, 28);
            btnChoiceSignOut.TabIndex = 2;
            btnChoiceSignOut.Text = "Sign Out";
            btnChoiceSignOut.Click += btnChoiceSignOut_Click;
            //
            // AppChoiceForm
            //
            AcceptButton = btnOpenOffice;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(332, 148);
            Controls.Add(lblChoiceWho);
            Controls.Add(lblChoiceQuestion);
            Controls.Add(btnOpenOffice);
            Controls.Add(btnOpenWebsite);
            Controls.Add(btnChoiceSignOut);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AppChoiceForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tri-Stone - Open";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblChoiceWho;
        private Label lblChoiceQuestion;
        private Button btnOpenOffice;
        private Button btnOpenWebsite;
        private Button btnChoiceSignOut;
    }
}
