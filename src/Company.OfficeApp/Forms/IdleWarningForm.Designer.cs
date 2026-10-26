namespace Company.OfficeApp.Forms
{
    partial class IdleWarningForm
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
            lblIdleMessage = new Label();
            lblCountdown = new Label();
            btnStaySignedIn = new Button();
            btnIdleSignOut = new Button();
            tmrCountdown = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            //
            // lblIdleMessage
            //
            lblIdleMessage.AutoSize = true;
            lblIdleMessage.Location = new Point(12, 12);
            lblIdleMessage.Name = "lblIdleMessage";
            lblIdleMessage.Text = "Still there? You will be signed out soon.";
            //
            // lblCountdown
            //
            lblCountdown.AutoSize = true;
            lblCountdown.Location = new Point(12, 36);
            lblCountdown.Name = "lblCountdown";
            lblCountdown.Text = "Signing out in 60 seconds.";
            //
            // btnStaySignedIn
            //
            btnStaySignedIn.Location = new Point(12, 66);
            btnStaySignedIn.Name = "btnStaySignedIn";
            btnStaySignedIn.Size = new Size(120, 28);
            btnStaySignedIn.TabIndex = 0;
            btnStaySignedIn.Text = "Stay Signed In";
            btnStaySignedIn.Click += btnStaySignedIn_Click;
            //
            // btnIdleSignOut
            //
            btnIdleSignOut.Location = new Point(138, 66);
            btnIdleSignOut.Name = "btnIdleSignOut";
            btnIdleSignOut.Size = new Size(90, 28);
            btnIdleSignOut.TabIndex = 1;
            btnIdleSignOut.Text = "Sign Out";
            btnIdleSignOut.Click += btnIdleSignOut_Click;
            //
            // tmrCountdown
            //
            tmrCountdown.Interval = 1000;
            tmrCountdown.Tick += tmrCountdown_Tick;
            //
            // IdleWarningForm
            //
            AcceptButton = btnStaySignedIn;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 108);
            ControlBox = false;
            Controls.Add(lblIdleMessage);
            Controls.Add(lblCountdown);
            Controls.Add(btnStaySignedIn);
            Controls.Add(btnIdleSignOut);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "IdleWarningForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Still there?";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblIdleMessage;
        private Label lblCountdown;
        private Button btnStaySignedIn;
        private Button btnIdleSignOut;
        private System.Windows.Forms.Timer tmrCountdown;
    }
}
