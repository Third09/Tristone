namespace Company.OfficeApp.Pages.Receipts
{
    partial class NeedsReviewTab
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
            lblNeedsReviewInfo = new Label();
            dgvNeedsReview = new DataGridView();
            btnApproveNoReceipt = new Button();
            btnRejectNoReceipt = new Button();
            btnAttachLateReceipt = new Button();
            lblApproverNote = new Label();
            lblReviewMsg = new Label();
            colDate = new DataGridViewTextBoxColumn();
            colMerchant = new DataGridViewTextBoxColumn();
            colAmount = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colProject = new DataGridViewTextBoxColumn();
            colReason = new DataGridViewTextBoxColumn();
            colBy = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvNeedsReview).BeginInit();
            SuspendLayout();
            // 
            // lblNeedsReviewInfo
            // 
            lblNeedsReviewInfo.AutoSize = true;
            lblNeedsReviewInfo.Location = new Point(12, 15);
            lblNeedsReviewInfo.Name = "lblNeedsReviewInfo";
            lblNeedsReviewInfo.Text = "Expenses with no receipt. They count as expenses only after the Receipts Supervisor approves.";
            // 
            // dgvNeedsReview
            // 
            dgvNeedsReview.AllowUserToAddRows = false;
            dgvNeedsReview.AllowUserToDeleteRows = false;
            dgvNeedsReview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNeedsReview.Columns.AddRange(new DataGridViewColumn[] { colDate, colMerchant, colAmount, colCategory, colProject, colReason, colBy, colStatus });
            dgvNeedsReview.MultiSelect = false;
            dgvNeedsReview.ReadOnly = true;
            dgvNeedsReview.RowHeadersVisible = false;
            dgvNeedsReview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNeedsReview.Location = new Point(12, 40);
            dgvNeedsReview.Name = "dgvNeedsReview";
            dgvNeedsReview.Size = new Size(930, 300);
            dgvNeedsReview.TabIndex = 0;
            dgvNeedsReview.SelectionChanged += dgvNeedsReview_SelectionChanged;
            // 
            // btnApproveNoReceipt
            // 
            btnApproveNoReceipt.Location = new Point(12, 348);
            btnApproveNoReceipt.Name = "btnApproveNoReceipt";
            btnApproveNoReceipt.Size = new Size(80, 28);
            btnApproveNoReceipt.Text = "Approve";
            btnApproveNoReceipt.TabIndex = 1;
            btnApproveNoReceipt.Click += btnApproveNoReceipt_Click;
            // 
            // btnRejectNoReceipt
            // 
            btnRejectNoReceipt.Location = new Point(98, 348);
            btnRejectNoReceipt.Name = "btnRejectNoReceipt";
            btnRejectNoReceipt.Size = new Size(70, 28);
            btnRejectNoReceipt.Text = "Reject";
            btnRejectNoReceipt.TabIndex = 2;
            btnRejectNoReceipt.Click += btnRejectNoReceipt_Click;
            // 
            // btnAttachLateReceipt
            // 
            btnAttachLateReceipt.Location = new Point(174, 348);
            btnAttachLateReceipt.Name = "btnAttachLateReceipt";
            btnAttachLateReceipt.Size = new Size(120, 28);
            btnAttachLateReceipt.Text = "Attach Receipt";
            btnAttachLateReceipt.TabIndex = 3;
            btnAttachLateReceipt.Click += btnAttachLateReceipt_Click;
            // 
            // lblApproverNote
            // 
            lblApproverNote.AutoSize = true;
            lblApproverNote.Location = new Point(12, 384);
            lblApproverNote.Name = "lblApproverNote";
            lblApproverNote.Text = "The Receipts Supervisor or an Admin approves these.";
            // 
            // lblReviewMsg
            // 
            lblReviewMsg.AutoSize = true;
            lblReviewMsg.Location = new Point(12, 408);
            lblReviewMsg.Name = "lblReviewMsg";
            // 
            // colDate
            // 
            colDate.DataPropertyName = "Date";
            colDate.HeaderText = "Date";
            colDate.Name = "colDate";
            colDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colMerchant
            // 
            colMerchant.DataPropertyName = "Merchant";
            colMerchant.HeaderText = "Paid To";
            colMerchant.Name = "colMerchant";
            // 
            // colAmount
            // 
            colAmount.DataPropertyName = "Amount";
            colAmount.HeaderText = "Amount";
            colAmount.Name = "colAmount";
            colAmount.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colCategory
            // 
            colCategory.DataPropertyName = "Category";
            colCategory.HeaderText = "Category";
            colCategory.Name = "colCategory";
            // 
            // colProject
            // 
            colProject.DataPropertyName = "ProjectOrOverhead";
            colProject.HeaderText = "Project / Overhead";
            colProject.Name = "colProject";
            // 
            // colReason
            // 
            colReason.DataPropertyName = "NoReceiptReason";
            colReason.HeaderText = "Reason";
            colReason.Name = "colReason";
            // 
            // colBy
            // 
            colBy.DataPropertyName = "UploadedBy";
            colBy.HeaderText = "Submitted By";
            colBy.Name = "colBy";
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            // 
            // NeedsReviewTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblNeedsReviewInfo);
            Controls.Add(dgvNeedsReview);
            Controls.Add(btnApproveNoReceipt);
            Controls.Add(btnRejectNoReceipt);
            Controls.Add(btnAttachLateReceipt);
            Controls.Add(lblApproverNote);
            Controls.Add(lblReviewMsg);
            Name = "NeedsReviewTab";
            Size = new Size(960, 440);
            Load += NeedsReviewTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvNeedsReview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNeedsReviewInfo;
        private DataGridView dgvNeedsReview;
        private Button btnApproveNoReceipt;
        private Button btnRejectNoReceipt;
        private Button btnAttachLateReceipt;
        private Label lblApproverNote;
        private Label lblReviewMsg;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colMerchant;
        private DataGridViewTextBoxColumn colAmount;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colProject;
        private DataGridViewTextBoxColumn colReason;
        private DataGridViewTextBoxColumn colBy;
        private DataGridViewTextBoxColumn colStatus;
    }
}
