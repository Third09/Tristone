namespace Company.WebsiteAdmin.Forms
{
    partial class PreviewForm
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
            pnlPreviewBar = new Panel();
            lblPreviewInfo = new Label();
            btnPreviewDesktop = new Button();
            btnPreviewPhone = new Button();
            btnRefreshPreview = new Button();
            btnClosePreview = new Button();
            pnlPreviewArea = new Panel();
            webPreview = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)webPreview).BeginInit();
            pnlPreviewBar.SuspendLayout();
            pnlPreviewArea.SuspendLayout();
            SuspendLayout();
            //
            // pnlPreviewBar
            //
            pnlPreviewBar.Controls.Add(lblPreviewInfo);
            pnlPreviewBar.Controls.Add(btnPreviewDesktop);
            pnlPreviewBar.Controls.Add(btnPreviewPhone);
            pnlPreviewBar.Controls.Add(btnRefreshPreview);
            pnlPreviewBar.Controls.Add(btnClosePreview);
            pnlPreviewBar.Dock = DockStyle.Top;
            pnlPreviewBar.Name = "pnlPreviewBar";
            pnlPreviewBar.Size = new Size(1200, 60);
            //
            // lblPreviewInfo
            //
            lblPreviewInfo.AutoSize = true;
            lblPreviewInfo.Location = new Point(24, 21);
            lblPreviewInfo.Name = "lblPreviewInfo";
            lblPreviewInfo.Text = "Loading the preview...";
            //
            // btnPreviewDesktop
            //
            btnPreviewDesktop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPreviewDesktop.Location = new Point(676, 12);
            btnPreviewDesktop.Name = "btnPreviewDesktop";
            btnPreviewDesktop.Size = new Size(110, 36);
            btnPreviewDesktop.TabIndex = 0;
            btnPreviewDesktop.Text = "Desktop";
            btnPreviewDesktop.Click += btnPreviewDesktop_Click;
            //
            // btnPreviewPhone
            //
            btnPreviewPhone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPreviewPhone.Location = new Point(792, 12);
            btnPreviewPhone.Name = "btnPreviewPhone";
            btnPreviewPhone.Size = new Size(110, 36);
            btnPreviewPhone.TabIndex = 1;
            btnPreviewPhone.Text = "Phone";
            btnPreviewPhone.Click += btnPreviewPhone_Click;
            //
            // btnRefreshPreview
            //
            btnRefreshPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefreshPreview.Location = new Point(938, 12);
            btnRefreshPreview.Name = "btnRefreshPreview";
            btnRefreshPreview.Size = new Size(110, 36);
            btnRefreshPreview.TabIndex = 2;
            btnRefreshPreview.Text = "Refresh";
            btnRefreshPreview.Click += btnRefreshPreview_Click;
            //
            // btnClosePreview
            //
            btnClosePreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClosePreview.Location = new Point(1066, 12);
            btnClosePreview.Name = "btnClosePreview";
            btnClosePreview.Size = new Size(110, 36);
            btnClosePreview.TabIndex = 3;
            btnClosePreview.Text = "Close";
            btnClosePreview.Click += btnClosePreview_Click;
            //
            // pnlPreviewArea  (gray around the page in Phone view)
            //
            pnlPreviewArea.Controls.Add(webPreview);
            pnlPreviewArea.Dock = DockStyle.Fill;
            pnlPreviewArea.Name = "pnlPreviewArea";
            pnlPreviewArea.Resize += pnlPreviewArea_Resize;
            //
            // webPreview
            //
            webPreview.Location = new Point(0, 0);
            webPreview.Name = "webPreview";
            webPreview.Size = new Size(1200, 740);
            webPreview.TabIndex = 4;
            //
            // PreviewForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 800);
            Controls.Add(pnlPreviewArea);
            Controls.Add(pnlPreviewBar);
            Name = "PreviewForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Preview Website";
            WindowState = FormWindowState.Maximized;
            Load += PreviewForm_Load;
            ((System.ComponentModel.ISupportInitialize)webPreview).EndInit();
            pnlPreviewBar.ResumeLayout(false);
            pnlPreviewBar.PerformLayout();
            pnlPreviewArea.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlPreviewBar;
        private Label lblPreviewInfo;
        private Button btnPreviewDesktop;
        private Button btnPreviewPhone;
        private Button btnRefreshPreview;
        private Button btnClosePreview;
        private Panel pnlPreviewArea;
        private Microsoft.Web.WebView2.WinForms.WebView2 webPreview;
    }
}
