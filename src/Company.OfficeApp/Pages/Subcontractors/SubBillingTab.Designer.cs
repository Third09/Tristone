namespace Company.OfficeApp.Pages.Subcontractors
{
    partial class SubBillingTab
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
            lblBillingJob = new Label();
            cboBillingJob = new ComboBox();
            lblBillContract = new Label();
            lblBillPaid = new Label();
            lblBillRetention = new Label();
            lblBillRemaining = new Label();
            lblBillProgress = new Label();
            dgvBillingStages = new DataGridView();
            btnMarkStageReached = new Button();
            btnPrepareVoucher = new Button();
            btnApproveVoucher = new Button();
            btnRejectVoucher = new Button();
            btnPrintVoucher = new Button();
            btnReleaseRetention = new Button();
            lblApproverNote = new Label();
            lblPayoutTitle = new Label();
            lblPayoutDate = new Label();
            dtpPayoutDate = new DateTimePicker();
            lblPayoutAmount = new Label();
            numPayoutAmount = new NumericUpDown();
            lblPayoutMethod = new Label();
            cboPayoutMethod = new ComboBox();
            lblPayoutReference = new Label();
            txtPayoutReference = new TextBox();
            btnRecordPayout = new Button();
            lblBillMsg = new Label();
            lblPayouts = new Label();
            dgvPayouts = new DataGridView();
            colStage = new DataGridViewTextBoxColumn();
            colPct = new DataGridViewTextBoxColumn();
            colAmount = new DataGridViewTextBoxColumn();
            colRetention = new DataGridViewTextBoxColumn();
            colPayable = new DataGridViewTextBoxColumn();
            colVoucher = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colPayDate = new DataGridViewTextBoxColumn();
            colPayFor = new DataGridViewTextBoxColumn();
            colPayAmount = new DataGridViewTextBoxColumn();
            colPayMethod = new DataGridViewTextBoxColumn();
            colPayRef = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvBillingStages).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPayoutAmount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPayouts).BeginInit();
            SuspendLayout();
            // 
            // lblBillingJob
            // 
            lblBillingJob.AutoSize = true;
            lblBillingJob.Location = new Point(12, 15);
            lblBillingJob.Name = "lblBillingJob";
            lblBillingJob.Text = "Job";
            // 
            // cboBillingJob
            // 
            cboBillingJob.Location = new Point(50, 12);
            cboBillingJob.Name = "cboBillingJob";
            cboBillingJob.Size = new Size(420, 23);
            cboBillingJob.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBillingJob.TabIndex = 0;
            cboBillingJob.SelectedIndexChanged += cboBillingJob_SelectedIndexChanged;
            // 
            // lblBillContract
            // 
            lblBillContract.AutoSize = true;
            lblBillContract.Location = new Point(12, 46);
            lblBillContract.Name = "lblBillContract";
            lblBillContract.Text = "Contract:";
            // 
            // lblBillPaid
            // 
            lblBillPaid.AutoSize = true;
            lblBillPaid.Location = new Point(210, 46);
            lblBillPaid.Name = "lblBillPaid";
            lblBillPaid.Text = "Paid so far:";
            // 
            // lblBillRetention
            // 
            lblBillRetention.AutoSize = true;
            lblBillRetention.Location = new Point(408, 46);
            lblBillRetention.Name = "lblBillRetention";
            lblBillRetention.Text = "Retention held:";
            // 
            // lblBillRemaining
            // 
            lblBillRemaining.AutoSize = true;
            lblBillRemaining.Location = new Point(606, 46);
            lblBillRemaining.Name = "lblBillRemaining";
            lblBillRemaining.Text = "Remaining:";
            // 
            // lblBillProgress
            // 
            lblBillProgress.AutoSize = true;
            lblBillProgress.Location = new Point(804, 46);
            lblBillProgress.Name = "lblBillProgress";
            lblBillProgress.Text = "Done:";
            // 
            // dgvBillingStages
            // 
            dgvBillingStages.AllowUserToAddRows = false;
            dgvBillingStages.AllowUserToDeleteRows = false;
            dgvBillingStages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBillingStages.Columns.AddRange(new DataGridViewColumn[] { colStage, colPct, colAmount, colRetention, colPayable, colVoucher, colStatus });
            dgvBillingStages.MultiSelect = false;
            dgvBillingStages.ReadOnly = true;
            dgvBillingStages.RowHeadersVisible = false;
            dgvBillingStages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBillingStages.Location = new Point(12, 70);
            dgvBillingStages.Name = "dgvBillingStages";
            dgvBillingStages.Size = new Size(930, 160);
            dgvBillingStages.TabIndex = 1;
            dgvBillingStages.SelectionChanged += dgvBillingStages_SelectionChanged;
            // 
            // btnMarkStageReached
            // 
            btnMarkStageReached.Location = new Point(12, 238);
            btnMarkStageReached.Name = "btnMarkStageReached";
            btnMarkStageReached.Size = new Size(140, 28);
            btnMarkStageReached.Text = "Mark Stage Reached";
            btnMarkStageReached.TabIndex = 2;
            btnMarkStageReached.Click += btnMarkStageReached_Click;
            // 
            // btnPrepareVoucher
            // 
            btnPrepareVoucher.Location = new Point(158, 238);
            btnPrepareVoucher.Name = "btnPrepareVoucher";
            btnPrepareVoucher.Size = new Size(120, 28);
            btnPrepareVoucher.Text = "Prepare Voucher";
            btnPrepareVoucher.TabIndex = 3;
            btnPrepareVoucher.Click += btnPrepareVoucher_Click;
            // 
            // btnApproveVoucher
            // 
            btnApproveVoucher.Location = new Point(284, 238);
            btnApproveVoucher.Name = "btnApproveVoucher";
            btnApproveVoucher.Size = new Size(120, 28);
            btnApproveVoucher.Text = "Approve Voucher";
            btnApproveVoucher.TabIndex = 4;
            btnApproveVoucher.Click += btnApproveVoucher_Click;
            // 
            // btnRejectVoucher
            // 
            btnRejectVoucher.Location = new Point(410, 238);
            btnRejectVoucher.Name = "btnRejectVoucher";
            btnRejectVoucher.Size = new Size(110, 28);
            btnRejectVoucher.Text = "Reject Voucher";
            btnRejectVoucher.TabIndex = 5;
            btnRejectVoucher.Click += btnRejectVoucher_Click;
            // 
            // btnPrintVoucher
            // 
            btnPrintVoucher.Location = new Point(526, 238);
            btnPrintVoucher.Name = "btnPrintVoucher";
            btnPrintVoucher.Size = new Size(100, 28);
            btnPrintVoucher.Text = "Print Voucher";
            btnPrintVoucher.TabIndex = 6;
            btnPrintVoucher.Click += btnPrintVoucher_Click;
            // 
            // btnReleaseRetention
            // 
            btnReleaseRetention.Location = new Point(632, 238);
            btnReleaseRetention.Name = "btnReleaseRetention";
            btnReleaseRetention.Size = new Size(130, 28);
            btnReleaseRetention.Text = "Release Retention";
            btnReleaseRetention.TabIndex = 7;
            btnReleaseRetention.Click += btnReleaseRetention_Click;
            // 
            // lblApproverNote
            // 
            lblApproverNote.AutoSize = true;
            lblApproverNote.Location = new Point(12, 272);
            lblApproverNote.Name = "lblApproverNote";
            lblApproverNote.Text = "The Subcontractors Supervisor or an Admin approves vouchers.";
            // 
            // lblPayoutTitle
            // 
            lblPayoutTitle.AutoSize = true;
            lblPayoutTitle.Location = new Point(12, 300);
            lblPayoutTitle.Name = "lblPayoutTitle";
            lblPayoutTitle.Text = "Record a payout (select an approved voucher above)";
            // 
            // lblPayoutDate
            // 
            lblPayoutDate.AutoSize = true;
            lblPayoutDate.Location = new Point(12, 322);
            lblPayoutDate.Name = "lblPayoutDate";
            lblPayoutDate.Text = "Date paid";
            // 
            // dtpPayoutDate
            // 
            dtpPayoutDate.Location = new Point(12, 340);
            dtpPayoutDate.Name = "dtpPayoutDate";
            dtpPayoutDate.Size = new Size(125, 23);
            dtpPayoutDate.Format = DateTimePickerFormat.Short;
            dtpPayoutDate.TabIndex = 8;
            // 
            // lblPayoutAmount
            // 
            lblPayoutAmount.AutoSize = true;
            lblPayoutAmount.Location = new Point(147, 322);
            lblPayoutAmount.Name = "lblPayoutAmount";
            lblPayoutAmount.Text = "Amount";
            // 
            // numPayoutAmount
            // 
            numPayoutAmount.Location = new Point(147, 340);
            numPayoutAmount.Name = "numPayoutAmount";
            numPayoutAmount.Size = new Size(130, 23);
            numPayoutAmount.DecimalPlaces = 2;
            numPayoutAmount.ThousandsSeparator = true;
            numPayoutAmount.Maximum = 999999999999m;
            numPayoutAmount.TabIndex = 9;
            // 
            // lblPayoutMethod
            // 
            lblPayoutMethod.AutoSize = true;
            lblPayoutMethod.Location = new Point(287, 322);
            lblPayoutMethod.Name = "lblPayoutMethod";
            lblPayoutMethod.Text = "Method";
            // 
            // cboPayoutMethod
            // 
            cboPayoutMethod.Location = new Point(287, 340);
            cboPayoutMethod.Name = "cboPayoutMethod";
            cboPayoutMethod.Size = new Size(120, 23);
            cboPayoutMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPayoutMethod.TabIndex = 10;
            // 
            // lblPayoutReference
            // 
            lblPayoutReference.AutoSize = true;
            lblPayoutReference.Location = new Point(417, 322);
            lblPayoutReference.Name = "lblPayoutReference";
            lblPayoutReference.Text = "Reference no.";
            // 
            // txtPayoutReference
            // 
            txtPayoutReference.Location = new Point(417, 340);
            txtPayoutReference.Name = "txtPayoutReference";
            txtPayoutReference.Size = new Size(160, 23);
            txtPayoutReference.TabIndex = 11;
            // 
            // btnRecordPayout
            // 
            btnRecordPayout.Location = new Point(587, 338);
            btnRecordPayout.Name = "btnRecordPayout";
            btnRecordPayout.Size = new Size(110, 28);
            btnRecordPayout.Text = "Record Payout";
            btnRecordPayout.TabIndex = 12;
            btnRecordPayout.Click += btnRecordPayout_Click;
            // 
            // lblBillMsg
            // 
            lblBillMsg.AutoSize = true;
            lblBillMsg.Location = new Point(12, 372);
            lblBillMsg.Name = "lblBillMsg";
            // 
            // lblPayouts
            // 
            lblPayouts.AutoSize = true;
            lblPayouts.Location = new Point(12, 398);
            lblPayouts.Name = "lblPayouts";
            lblPayouts.Text = "Payouts";
            // 
            // dgvPayouts
            // 
            dgvPayouts.AllowUserToAddRows = false;
            dgvPayouts.AllowUserToDeleteRows = false;
            dgvPayouts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayouts.Columns.AddRange(new DataGridViewColumn[] { colPayDate, colPayFor, colPayAmount, colPayMethod, colPayRef });
            dgvPayouts.MultiSelect = false;
            dgvPayouts.ReadOnly = true;
            dgvPayouts.RowHeadersVisible = false;
            dgvPayouts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayouts.Location = new Point(12, 418);
            dgvPayouts.Name = "dgvPayouts";
            dgvPayouts.Size = new Size(930, 140);
            dgvPayouts.TabIndex = 13;
            // 
            // colStage
            // 
            colStage.DataPropertyName = "Name";
            colStage.HeaderText = "Stage";
            colStage.Name = "colStage";
            // 
            // colPct
            // 
            colPct.DataPropertyName = "PercentText";
            colPct.HeaderText = "%";
            colPct.Name = "colPct";
            // 
            // colAmount
            // 
            colAmount.DataPropertyName = "Amount";
            colAmount.HeaderText = "Amount";
            colAmount.Name = "colAmount";
            colAmount.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colRetention
            // 
            colRetention.DataPropertyName = "Retention";
            colRetention.HeaderText = "Retention";
            colRetention.Name = "colRetention";
            colRetention.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colPayable
            // 
            colPayable.DataPropertyName = "PayableNow";
            colPayable.HeaderText = "Payable Now";
            colPayable.Name = "colPayable";
            colPayable.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colVoucher
            // 
            colVoucher.DataPropertyName = "VoucherNo";
            colVoucher.HeaderText = "Voucher";
            colVoucher.Name = "colVoucher";
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            // 
            // colPayDate
            // 
            colPayDate.DataPropertyName = "PaidOn";
            colPayDate.HeaderText = "Date";
            colPayDate.Name = "colPayDate";
            colPayDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colPayFor
            // 
            colPayFor.DataPropertyName = "For";
            colPayFor.HeaderText = "Stage";
            colPayFor.Name = "colPayFor";
            // 
            // colPayAmount
            // 
            colPayAmount.DataPropertyName = "Amount";
            colPayAmount.HeaderText = "Amount";
            colPayAmount.Name = "colPayAmount";
            colPayAmount.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colPayMethod
            // 
            colPayMethod.DataPropertyName = "Method";
            colPayMethod.HeaderText = "Method";
            colPayMethod.Name = "colPayMethod";
            // 
            // colPayRef
            // 
            colPayRef.DataPropertyName = "ReferenceNo";
            colPayRef.HeaderText = "Reference";
            colPayRef.Name = "colPayRef";
            // 
            // SubBillingTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblBillingJob);
            Controls.Add(cboBillingJob);
            Controls.Add(lblBillContract);
            Controls.Add(lblBillPaid);
            Controls.Add(lblBillRetention);
            Controls.Add(lblBillRemaining);
            Controls.Add(lblBillProgress);
            Controls.Add(dgvBillingStages);
            Controls.Add(btnMarkStageReached);
            Controls.Add(btnPrepareVoucher);
            Controls.Add(btnApproveVoucher);
            Controls.Add(btnRejectVoucher);
            Controls.Add(btnPrintVoucher);
            Controls.Add(btnReleaseRetention);
            Controls.Add(lblApproverNote);
            Controls.Add(lblPayoutTitle);
            Controls.Add(lblPayoutDate);
            Controls.Add(dtpPayoutDate);
            Controls.Add(lblPayoutAmount);
            Controls.Add(numPayoutAmount);
            Controls.Add(lblPayoutMethod);
            Controls.Add(cboPayoutMethod);
            Controls.Add(lblPayoutReference);
            Controls.Add(txtPayoutReference);
            Controls.Add(btnRecordPayout);
            Controls.Add(lblBillMsg);
            Controls.Add(lblPayouts);
            Controls.Add(dgvPayouts);
            Name = "SubBillingTab";
            Size = new Size(960, 580);
            Load += SubBillingTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvBillingStages).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPayoutAmount).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPayouts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBillingJob;
        private ComboBox cboBillingJob;
        private Label lblBillContract;
        private Label lblBillPaid;
        private Label lblBillRetention;
        private Label lblBillRemaining;
        private Label lblBillProgress;
        private DataGridView dgvBillingStages;
        private Button btnMarkStageReached;
        private Button btnPrepareVoucher;
        private Button btnApproveVoucher;
        private Button btnRejectVoucher;
        private Button btnPrintVoucher;
        private Button btnReleaseRetention;
        private Label lblApproverNote;
        private Label lblPayoutTitle;
        private Label lblPayoutDate;
        private DateTimePicker dtpPayoutDate;
        private Label lblPayoutAmount;
        private NumericUpDown numPayoutAmount;
        private Label lblPayoutMethod;
        private ComboBox cboPayoutMethod;
        private Label lblPayoutReference;
        private TextBox txtPayoutReference;
        private Button btnRecordPayout;
        private Label lblBillMsg;
        private Label lblPayouts;
        private DataGridView dgvPayouts;
        private DataGridViewTextBoxColumn colStage;
        private DataGridViewTextBoxColumn colPct;
        private DataGridViewTextBoxColumn colAmount;
        private DataGridViewTextBoxColumn colRetention;
        private DataGridViewTextBoxColumn colPayable;
        private DataGridViewTextBoxColumn colVoucher;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colPayDate;
        private DataGridViewTextBoxColumn colPayFor;
        private DataGridViewTextBoxColumn colPayAmount;
        private DataGridViewTextBoxColumn colPayMethod;
        private DataGridViewTextBoxColumn colPayRef;
    }
}
