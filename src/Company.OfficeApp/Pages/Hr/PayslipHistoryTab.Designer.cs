namespace Company.OfficeApp.Pages.Hr
{
    partial class PayslipHistoryTab
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
            lblPayslipSearch = new Label();
            txtPayslipSearch = new TextBox();
            lblPayslipPeriod = new Label();
            cboPayslipPeriod = new ComboBox();
            dgvPayslipHistory = new DataGridView();
            btnViewPayslip = new Button();
            btnPrintPayslip = new Button();
            btnSavePayslipPdf = new Button();
            lblPayslipMsg = new Label();
            colPsPayDate = new DataGridViewTextBoxColumn();
            colPsPeriod = new DataGridViewTextBoxColumn();
            colPsName = new DataGridViewTextBoxColumn();
            colPsDays = new DataGridViewTextBoxColumn();
            colPsGross = new DataGridViewTextBoxColumn();
            colPsNet = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvPayslipHistory).BeginInit();
            SuspendLayout();
            // 
            // lblPayslipSearch
            // 
            lblPayslipSearch.AutoSize = true;
            lblPayslipSearch.Location = new Point(12, 15);
            lblPayslipSearch.Name = "lblPayslipSearch";
            lblPayslipSearch.Text = "Search";
            // 
            // txtPayslipSearch
            // 
            txtPayslipSearch.Location = new Point(62, 12);
            txtPayslipSearch.Name = "txtPayslipSearch";
            txtPayslipSearch.Size = new Size(200, 23);
            txtPayslipSearch.TabIndex = 0;
            // 
            // lblPayslipPeriod
            // 
            lblPayslipPeriod.AutoSize = true;
            lblPayslipPeriod.Location = new Point(274, 15);
            lblPayslipPeriod.Name = "lblPayslipPeriod";
            lblPayslipPeriod.Text = "Payroll";
            // 
            // cboPayslipPeriod
            // 
            cboPayslipPeriod.Location = new Point(330, 12);
            cboPayslipPeriod.Name = "cboPayslipPeriod";
            cboPayslipPeriod.Size = new Size(400, 23);
            cboPayslipPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPayslipPeriod.TabIndex = 1;
            cboPayslipPeriod.SelectedIndexChanged += Filter_Changed;
            // 
            // dgvPayslipHistory
            // 
            dgvPayslipHistory.AllowUserToAddRows = false;
            dgvPayslipHistory.AllowUserToDeleteRows = false;
            dgvPayslipHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayslipHistory.Columns.AddRange(new DataGridViewColumn[] { colPsPayDate, colPsPeriod, colPsName, colPsDays, colPsGross, colPsNet });
            dgvPayslipHistory.MultiSelect = false;
            dgvPayslipHistory.ReadOnly = true;
            dgvPayslipHistory.RowHeadersVisible = false;
            dgvPayslipHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayslipHistory.Location = new Point(12, 44);
            dgvPayslipHistory.Name = "dgvPayslipHistory";
            dgvPayslipHistory.Size = new Size(930, 380);
            dgvPayslipHistory.TabIndex = 2;
            dgvPayslipHistory.SelectionChanged += dgvPayslipHistory_SelectionChanged;
            dgvPayslipHistory.CellDoubleClick += dgvPayslipHistory_CellDoubleClick;
            // 
            // btnViewPayslip
            // 
            btnViewPayslip.Location = new Point(12, 432);
            btnViewPayslip.Name = "btnViewPayslip";
            btnViewPayslip.Size = new Size(80, 28);
            btnViewPayslip.Text = "View";
            btnViewPayslip.TabIndex = 3;
            btnViewPayslip.Click += btnViewPayslip_Click;
            // 
            // btnPrintPayslip
            // 
            btnPrintPayslip.Location = new Point(98, 432);
            btnPrintPayslip.Name = "btnPrintPayslip";
            btnPrintPayslip.Size = new Size(80, 28);
            btnPrintPayslip.Text = "Print";
            btnPrintPayslip.TabIndex = 4;
            btnPrintPayslip.Click += btnPrintPayslip_Click;
            // 
            // btnSavePayslipPdf
            // 
            btnSavePayslipPdf.Location = new Point(184, 432);
            btnSavePayslipPdf.Name = "btnSavePayslipPdf";
            btnSavePayslipPdf.Size = new Size(110, 28);
            btnSavePayslipPdf.Text = "Save as PDF";
            btnSavePayslipPdf.TabIndex = 5;
            btnSavePayslipPdf.Click += btnSavePayslipPdf_Click;
            // 
            // lblPayslipMsg
            // 
            lblPayslipMsg.AutoSize = true;
            lblPayslipMsg.Location = new Point(12, 468);
            lblPayslipMsg.Name = "lblPayslipMsg";
            lblPayslipMsg.MaximumSize = new Size(930, 0);
            // 
            // colPsPayDate
            // 
            colPsPayDate.DataPropertyName = "PayDate";
            colPsPayDate.HeaderText = "Pay Date";
            colPsPayDate.Name = "colPsPayDate";
            colPsPayDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colPsPeriod
            // 
            colPsPeriod.DataPropertyName = "Period";
            colPsPeriod.HeaderText = "Period";
            colPsPeriod.Name = "colPsPeriod";
            // 
            // colPsName
            // 
            colPsName.DataPropertyName = "Name";
            colPsName.HeaderText = "Name";
            colPsName.Name = "colPsName";
            // 
            // colPsDays
            // 
            colPsDays.DataPropertyName = "DaysPaid";
            colPsDays.HeaderText = "Days Paid";
            colPsDays.Name = "colPsDays";
            // 
            // colPsGross
            // 
            colPsGross.DataPropertyName = "GrossPay";
            colPsGross.HeaderText = "Gross Pay";
            colPsGross.Name = "colPsGross";
            colPsGross.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colPsNet
            // 
            colPsNet.DataPropertyName = "NetPay";
            colPsNet.HeaderText = "Net Pay";
            colPsNet.Name = "colPsNet";
            colPsNet.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // PayslipHistoryTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblPayslipSearch);
            Controls.Add(txtPayslipSearch);
            Controls.Add(lblPayslipPeriod);
            Controls.Add(cboPayslipPeriod);
            Controls.Add(dgvPayslipHistory);
            Controls.Add(btnViewPayslip);
            Controls.Add(btnPrintPayslip);
            Controls.Add(btnSavePayslipPdf);
            Controls.Add(lblPayslipMsg);
            Name = "PayslipHistoryTab";
            Size = new Size(960, 520);
            Load += PayslipHistoryTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPayslipHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPayslipSearch;
        private TextBox txtPayslipSearch;
        private Label lblPayslipPeriod;
        private ComboBox cboPayslipPeriod;
        private DataGridView dgvPayslipHistory;
        private Button btnViewPayslip;
        private Button btnPrintPayslip;
        private Button btnSavePayslipPdf;
        private Label lblPayslipMsg;
        private DataGridViewTextBoxColumn colPsPayDate;
        private DataGridViewTextBoxColumn colPsPeriod;
        private DataGridViewTextBoxColumn colPsName;
        private DataGridViewTextBoxColumn colPsDays;
        private DataGridViewTextBoxColumn colPsGross;
        private DataGridViewTextBoxColumn colPsNet;
    }
}
