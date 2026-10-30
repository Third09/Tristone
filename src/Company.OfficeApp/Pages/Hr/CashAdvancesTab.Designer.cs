namespace Company.OfficeApp.Pages.Hr
{
    partial class CashAdvancesTab
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
            lblCaFilter = new Label();
            cboCashAdvanceFilter = new ComboBox();
            dgvCashAdvances = new DataGridView();
            btnNewCashAdvance = new Button();
            btnApproveCashAdvance = new Button();
            btnRejectCashAdvance = new Button();
            lblApproverNote = new Label();
            lblCaMsg = new Label();
            lblDeductions = new Label();
            dgvCashAdvanceDeductions = new DataGridView();
            colCaDate = new DataGridViewTextBoxColumn();
            colCaName = new DataGridViewTextBoxColumn();
            colCaAmount = new DataGridViewTextBoxColumn();
            colCaPerPayday = new DataGridViewTextBoxColumn();
            colCaBalance = new DataGridViewTextBoxColumn();
            colCaStatus = new DataGridViewTextBoxColumn();
            colCaReason = new DataGridViewTextBoxColumn();
            colDedPayDate = new DataGridViewTextBoxColumn();
            colDedPeriod = new DataGridViewTextBoxColumn();
            colDedAmount = new DataGridViewTextBoxColumn();
            colDedBalance = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvCashAdvances).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCashAdvanceDeductions).BeginInit();
            SuspendLayout();
            // 
            // lblCaFilter
            // 
            lblCaFilter.AutoSize = true;
            lblCaFilter.Location = new Point(12, 15);
            lblCaFilter.Name = "lblCaFilter";
            lblCaFilter.Text = "Show";
            // 
            // cboCashAdvanceFilter
            // 
            cboCashAdvanceFilter.Location = new Point(60, 12);
            cboCashAdvanceFilter.Name = "cboCashAdvanceFilter";
            cboCashAdvanceFilter.Size = new Size(180, 23);
            cboCashAdvanceFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCashAdvanceFilter.TabIndex = 0;
            cboCashAdvanceFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // dgvCashAdvances
            // 
            dgvCashAdvances.AllowUserToAddRows = false;
            dgvCashAdvances.AllowUserToDeleteRows = false;
            dgvCashAdvances.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCashAdvances.Columns.AddRange(new DataGridViewColumn[] { colCaDate, colCaName, colCaAmount, colCaPerPayday, colCaBalance, colCaStatus, colCaReason });
            dgvCashAdvances.MultiSelect = false;
            dgvCashAdvances.ReadOnly = true;
            dgvCashAdvances.RowHeadersVisible = false;
            dgvCashAdvances.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCashAdvances.Location = new Point(12, 44);
            dgvCashAdvances.Name = "dgvCashAdvances";
            dgvCashAdvances.Size = new Size(930, 220);
            dgvCashAdvances.TabIndex = 1;
            dgvCashAdvances.SelectionChanged += dgvCashAdvances_SelectionChanged;
            // 
            // btnNewCashAdvance
            // 
            btnNewCashAdvance.Location = new Point(12, 272);
            btnNewCashAdvance.Name = "btnNewCashAdvance";
            btnNewCashAdvance.Size = new Size(150, 28);
            btnNewCashAdvance.Text = "New Cash Advance";
            btnNewCashAdvance.TabIndex = 2;
            btnNewCashAdvance.Click += btnNewCashAdvance_Click;
            // 
            // btnApproveCashAdvance
            // 
            btnApproveCashAdvance.Location = new Point(168, 272);
            btnApproveCashAdvance.Name = "btnApproveCashAdvance";
            btnApproveCashAdvance.Size = new Size(90, 28);
            btnApproveCashAdvance.Text = "Approve";
            btnApproveCashAdvance.TabIndex = 3;
            btnApproveCashAdvance.Click += btnApproveCashAdvance_Click;
            // 
            // btnRejectCashAdvance
            // 
            btnRejectCashAdvance.Location = new Point(264, 272);
            btnRejectCashAdvance.Name = "btnRejectCashAdvance";
            btnRejectCashAdvance.Size = new Size(90, 28);
            btnRejectCashAdvance.Text = "Reject";
            btnRejectCashAdvance.TabIndex = 4;
            btnRejectCashAdvance.Click += btnRejectCashAdvance_Click;
            // 
            // lblApproverNote
            // 
            lblApproverNote.AutoSize = true;
            lblApproverNote.Location = new Point(12, 308);
            lblApproverNote.Name = "lblApproverNote";
            lblApproverNote.Text = "The HR Supervisor or an Admin approves cash advances.";
            // 
            // lblCaMsg
            // 
            lblCaMsg.AutoSize = true;
            lblCaMsg.Location = new Point(12, 332);
            lblCaMsg.Name = "lblCaMsg";
            // 
            // lblDeductions
            // 
            lblDeductions.AutoSize = true;
            lblDeductions.Location = new Point(12, 360);
            lblDeductions.Name = "lblDeductions";
            lblDeductions.Text = "Deductions taken (selected cash advance)";
            // 
            // dgvCashAdvanceDeductions
            // 
            dgvCashAdvanceDeductions.AllowUserToAddRows = false;
            dgvCashAdvanceDeductions.AllowUserToDeleteRows = false;
            dgvCashAdvanceDeductions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCashAdvanceDeductions.Columns.AddRange(new DataGridViewColumn[] { colDedPayDate, colDedPeriod, colDedAmount, colDedBalance });
            dgvCashAdvanceDeductions.MultiSelect = false;
            dgvCashAdvanceDeductions.ReadOnly = true;
            dgvCashAdvanceDeductions.RowHeadersVisible = false;
            dgvCashAdvanceDeductions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCashAdvanceDeductions.Location = new Point(12, 380);
            dgvCashAdvanceDeductions.Name = "dgvCashAdvanceDeductions";
            dgvCashAdvanceDeductions.Size = new Size(600, 200);
            dgvCashAdvanceDeductions.TabIndex = 5;
            // 
            // colCaDate
            // 
            colCaDate.DataPropertyName = "GivenOn";
            colCaDate.HeaderText = "Date";
            colCaDate.Name = "colCaDate";
            colCaDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colCaName
            // 
            colCaName.DataPropertyName = "Name";
            colCaName.HeaderText = "Name";
            colCaName.Name = "colCaName";
            // 
            // colCaAmount
            // 
            colCaAmount.DataPropertyName = "Amount";
            colCaAmount.HeaderText = "Amount";
            colCaAmount.Name = "colCaAmount";
            colCaAmount.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colCaPerPayday
            // 
            colCaPerPayday.DataPropertyName = "PerPayday";
            colCaPerPayday.HeaderText = "Deduct per Payday";
            colCaPerPayday.Name = "colCaPerPayday";
            colCaPerPayday.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colCaBalance
            // 
            colCaBalance.DataPropertyName = "Balance";
            colCaBalance.HeaderText = "Balance";
            colCaBalance.Name = "colCaBalance";
            colCaBalance.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colCaStatus
            // 
            colCaStatus.DataPropertyName = "Status";
            colCaStatus.HeaderText = "Status";
            colCaStatus.Name = "colCaStatus";
            // 
            // colCaReason
            // 
            colCaReason.DataPropertyName = "Reason";
            colCaReason.HeaderText = "Reason";
            colCaReason.Name = "colCaReason";
            // 
            // colDedPayDate
            // 
            colDedPayDate.DataPropertyName = "PayDate";
            colDedPayDate.HeaderText = "Pay Date";
            colDedPayDate.Name = "colDedPayDate";
            colDedPayDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colDedPeriod
            // 
            colDedPeriod.DataPropertyName = "Period";
            colDedPeriod.HeaderText = "Period";
            colDedPeriod.Name = "colDedPeriod";
            // 
            // colDedAmount
            // 
            colDedAmount.DataPropertyName = "Deducted";
            colDedAmount.HeaderText = "Deducted";
            colDedAmount.Name = "colDedAmount";
            colDedAmount.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colDedBalance
            // 
            colDedBalance.DataPropertyName = "BalanceAfter";
            colDedBalance.HeaderText = "Balance After";
            colDedBalance.Name = "colDedBalance";
            colDedBalance.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // CashAdvancesTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblCaFilter);
            Controls.Add(cboCashAdvanceFilter);
            Controls.Add(dgvCashAdvances);
            Controls.Add(btnNewCashAdvance);
            Controls.Add(btnApproveCashAdvance);
            Controls.Add(btnRejectCashAdvance);
            Controls.Add(lblApproverNote);
            Controls.Add(lblCaMsg);
            Controls.Add(lblDeductions);
            Controls.Add(dgvCashAdvanceDeductions);
            Name = "CashAdvancesTab";
            Size = new Size(960, 600);
            Load += CashAdvancesTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCashAdvances).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCashAdvanceDeductions).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCaFilter;
        private ComboBox cboCashAdvanceFilter;
        private DataGridView dgvCashAdvances;
        private Button btnNewCashAdvance;
        private Button btnApproveCashAdvance;
        private Button btnRejectCashAdvance;
        private Label lblApproverNote;
        private Label lblCaMsg;
        private Label lblDeductions;
        private DataGridView dgvCashAdvanceDeductions;
        private DataGridViewTextBoxColumn colCaDate;
        private DataGridViewTextBoxColumn colCaName;
        private DataGridViewTextBoxColumn colCaAmount;
        private DataGridViewTextBoxColumn colCaPerPayday;
        private DataGridViewTextBoxColumn colCaBalance;
        private DataGridViewTextBoxColumn colCaStatus;
        private DataGridViewTextBoxColumn colCaReason;
        private DataGridViewTextBoxColumn colDedPayDate;
        private DataGridViewTextBoxColumn colDedPeriod;
        private DataGridViewTextBoxColumn colDedAmount;
        private DataGridViewTextBoxColumn colDedBalance;
    }
}
