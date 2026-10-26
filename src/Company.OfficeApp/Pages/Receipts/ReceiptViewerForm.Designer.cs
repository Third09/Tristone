namespace Company.OfficeApp.Pages.Receipts
{
    partial class ReceiptViewerForm
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            pnlScan = new Panel();
            picReceiptScan = new PictureBox();
            btnZoomIn = new Button();
            btnZoomOut = new Button();
            btnOpenOriginal = new Button();
            lblViewReceiptNo = new Label();
            lblViewDate = new Label();
            lblViewMerchant = new Label();
            lblViewAmount = new Label();
            lblViewCategory = new Label();
            lblViewProject = new Label();
            lblViewUploadedBy = new Label();
            lblViewStatus = new Label();
            lblViewCopy = new Label();
            lblViewReason = new Label();
            lblViewerMsg = new Label();
            btnPrintReceipt = new Button();
            btnCloseViewer = new Button();
            ((System.ComponentModel.ISupportInitialize)picReceiptScan).BeginInit();
            SuspendLayout();
            // 
            // pnlScan
            // 
            pnlScan.Location = new Point(12, 12);
            pnlScan.Name = "pnlScan";
            pnlScan.Size = new Size(560, 560);
            pnlScan.AutoScroll = true;
            pnlScan.BorderStyle = BorderStyle.FixedSingle;
            pnlScan.TabIndex = 0;
            // 
            // picReceiptScan
            // 
            picReceiptScan.Location = new Point(0, 0);
            picReceiptScan.Name = "picReceiptScan";
            picReceiptScan.Size = new Size(556, 556);
            picReceiptScan.SizeMode = PictureBoxSizeMode.Zoom;
            picReceiptScan.TabIndex = 1;
            // 
            // btnZoomIn
            // 
            btnZoomIn.Location = new Point(12, 584);
            btnZoomIn.Name = "btnZoomIn";
            btnZoomIn.Size = new Size(90, 28);
            btnZoomIn.Text = "Zoom In";
            btnZoomIn.TabIndex = 2;
            btnZoomIn.Click += btnZoomIn_Click;
            // 
            // btnZoomOut
            // 
            btnZoomOut.Location = new Point(108, 584);
            btnZoomOut.Name = "btnZoomOut";
            btnZoomOut.Size = new Size(90, 28);
            btnZoomOut.Text = "Zoom Out";
            btnZoomOut.TabIndex = 3;
            btnZoomOut.Click += btnZoomOut_Click;
            // 
            // btnOpenOriginal
            // 
            btnOpenOriginal.Location = new Point(204, 584);
            btnOpenOriginal.Name = "btnOpenOriginal";
            btnOpenOriginal.Size = new Size(110, 28);
            btnOpenOriginal.Text = "Open Original";
            btnOpenOriginal.TabIndex = 4;
            btnOpenOriginal.Click += btnOpenOriginal_Click;
            // 
            // lblViewReceiptNo
            // 
            lblViewReceiptNo.AutoSize = true;
            lblViewReceiptNo.Location = new Point(590, 12);
            lblViewReceiptNo.Name = "lblViewReceiptNo";
            // 
            // lblViewDate
            // 
            lblViewDate.AutoSize = true;
            lblViewDate.Location = new Point(590, 36);
            lblViewDate.Name = "lblViewDate";
            // 
            // lblViewMerchant
            // 
            lblViewMerchant.AutoSize = true;
            lblViewMerchant.Location = new Point(590, 60);
            lblViewMerchant.Name = "lblViewMerchant";
            // 
            // lblViewAmount
            // 
            lblViewAmount.AutoSize = true;
            lblViewAmount.Location = new Point(590, 84);
            lblViewAmount.Name = "lblViewAmount";
            // 
            // lblViewCategory
            // 
            lblViewCategory.AutoSize = true;
            lblViewCategory.Location = new Point(590, 108);
            lblViewCategory.Name = "lblViewCategory";
            // 
            // lblViewProject
            // 
            lblViewProject.AutoSize = true;
            lblViewProject.Location = new Point(590, 132);
            lblViewProject.Name = "lblViewProject";
            // 
            // lblViewUploadedBy
            // 
            lblViewUploadedBy.AutoSize = true;
            lblViewUploadedBy.Location = new Point(590, 156);
            lblViewUploadedBy.Name = "lblViewUploadedBy";
            // 
            // lblViewStatus
            // 
            lblViewStatus.AutoSize = true;
            lblViewStatus.Location = new Point(590, 180);
            lblViewStatus.Name = "lblViewStatus";
            // 
            // lblViewCopy
            // 
            lblViewCopy.AutoSize = true;
            lblViewCopy.Location = new Point(590, 204);
            lblViewCopy.Name = "lblViewCopy";
            lblViewCopy.MaximumSize = new Size(290, 0);
            // 
            // lblViewReason
            // 
            lblViewReason.AutoSize = true;
            lblViewReason.Location = new Point(590, 252);
            lblViewReason.Name = "lblViewReason";
            lblViewReason.MaximumSize = new Size(290, 0);
            // 
            // lblViewerMsg
            // 
            lblViewerMsg.AutoSize = true;
            lblViewerMsg.Location = new Point(590, 540);
            lblViewerMsg.Name = "lblViewerMsg";
            lblViewerMsg.MaximumSize = new Size(290, 0);
            // 
            // btnPrintReceipt
            // 
            btnPrintReceipt.Location = new Point(590, 584);
            btnPrintReceipt.Name = "btnPrintReceipt";
            btnPrintReceipt.Size = new Size(90, 28);
            btnPrintReceipt.Text = "Print";
            btnPrintReceipt.TabIndex = 5;
            btnPrintReceipt.Click += btnPrintReceipt_Click;
            // 
            // btnCloseViewer
            // 
            btnCloseViewer.Location = new Point(686, 584);
            btnCloseViewer.Name = "btnCloseViewer";
            btnCloseViewer.Size = new Size(90, 28);
            btnCloseViewer.Text = "Close";
            btnCloseViewer.DialogResult = DialogResult.Cancel;
            btnCloseViewer.TabIndex = 6;
            // 
            // ReceiptViewerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 630);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(pnlScan);
            Controls.Add(picReceiptScan);
            Controls.Add(btnZoomIn);
            Controls.Add(btnZoomOut);
            Controls.Add(btnOpenOriginal);
            Controls.Add(lblViewReceiptNo);
            Controls.Add(lblViewDate);
            Controls.Add(lblViewMerchant);
            Controls.Add(lblViewAmount);
            Controls.Add(lblViewCategory);
            Controls.Add(lblViewProject);
            Controls.Add(lblViewUploadedBy);
            Controls.Add(lblViewStatus);
            Controls.Add(lblViewCopy);
            Controls.Add(lblViewReason);
            Controls.Add(lblViewerMsg);
            Controls.Add(btnPrintReceipt);
            Controls.Add(btnCloseViewer);
            Name = "ReceiptViewerForm";
            Load += ReceiptViewerForm_Load;
            ((System.ComponentModel.ISupportInitialize)picReceiptScan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlScan;
        private PictureBox picReceiptScan;
        private Button btnZoomIn;
        private Button btnZoomOut;
        private Button btnOpenOriginal;
        private Label lblViewReceiptNo;
        private Label lblViewDate;
        private Label lblViewMerchant;
        private Label lblViewAmount;
        private Label lblViewCategory;
        private Label lblViewProject;
        private Label lblViewUploadedBy;
        private Label lblViewStatus;
        private Label lblViewCopy;
        private Label lblViewReason;
        private Label lblViewerMsg;
        private Button btnPrintReceipt;
        private Button btnCloseViewer;
    }
}
