namespace Company.OfficeApp.Pages.Receipts
{
    partial class ReceiptReportsTab
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
            lblReportType = new Label();
            cboReportType = new ComboBox();
            lblReportProject = new Label();
            cboReportProject = new ComboBox();
            lblReportFrom = new Label();
            dtpReportFrom = new DateTimePicker();
            lblReportTo = new Label();
            dtpReportTo = new DateTimePicker();
            btnPreviewReport = new Button();
            btnPrintReport = new Button();
            btnReportPdf = new Button();
            btnReportExcel = new Button();
            lblReportMsg = new Label();
            SuspendLayout();
            // 
            // lblReportType
            // 
            lblReportType.AutoSize = true;
            lblReportType.Location = new Point(38, 36);
            lblReportType.Name = "lblReportType";
            lblReportType.Text = "Report";
            // 
            // cboReportType
            // 
            cboReportType.Location = new Point(139, 32);
            cboReportType.Name = "cboReportType";
            cboReportType.Size = new Size(276, 23);
            cboReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReportType.TabIndex = 0;
            // 
            // lblReportProject
            // 
            lblReportProject.AutoSize = true;
            lblReportProject.Location = new Point(38, 75);
            lblReportProject.Name = "lblReportProject";
            lblReportProject.Text = "Project";
            // 
            // cboReportProject
            // 
            cboReportProject.Location = new Point(139, 71);
            cboReportProject.Name = "cboReportProject";
            cboReportProject.Size = new Size(276, 23);
            cboReportProject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReportProject.TabIndex = 1;
            // 
            // lblReportFrom
            // 
            lblReportFrom.AutoSize = true;
            lblReportFrom.Location = new Point(38, 115);
            lblReportFrom.Name = "lblReportFrom";
            lblReportFrom.Text = "From";
            // 
            // dtpReportFrom
            // 
            dtpReportFrom.Location = new Point(139, 111);
            dtpReportFrom.Name = "dtpReportFrom";
            dtpReportFrom.Size = new Size(132, 23);
            dtpReportFrom.Format = DateTimePickerFormat.Short;
            dtpReportFrom.TabIndex = 2;
            // 
            // lblReportTo
            // 
            lblReportTo.AutoSize = true;
            lblReportTo.Location = new Point(283, 115);
            lblReportTo.Name = "lblReportTo";
            lblReportTo.Text = "To";
            // 
            // dtpReportTo
            // 
            dtpReportTo.Location = new Point(313, 111);
            dtpReportTo.Name = "dtpReportTo";
            dtpReportTo.Size = new Size(132, 23);
            dtpReportTo.Format = DateTimePickerFormat.Short;
            dtpReportTo.TabIndex = 3;
            // 
            // btnPreviewReport
            // 
            btnPreviewReport.Location = new Point(38, 161);
            btnPreviewReport.Name = "btnPreviewReport";
            btnPreviewReport.Size = new Size(92, 36);
            btnPreviewReport.Text = "Preview";
            btnPreviewReport.TabIndex = 4;
            btnPreviewReport.Click += btnPreviewReport_Click;
            // 
            // btnPrintReport
            // 
            btnPrintReport.Location = new Point(137, 161);
            btnPrintReport.Name = "btnPrintReport";
            btnPrintReport.Size = new Size(80, 36);
            btnPrintReport.Text = "Print";
            btnPrintReport.TabIndex = 5;
            btnPrintReport.Click += btnPrintReport_Click;
            // 
            // btnReportPdf
            // 
            btnReportPdf.Location = new Point(224, 161);
            btnReportPdf.Name = "btnReportPdf";
            btnReportPdf.Size = new Size(115, 36);
            btnReportPdf.Text = "Save as PDF";
            btnReportPdf.TabIndex = 6;
            btnReportPdf.Click += btnReportPdf_Click;
            // 
            // btnReportExcel
            // 
            btnReportExcel.Location = new Point(346, 161);
            btnReportExcel.Name = "btnReportExcel";
            btnReportExcel.Size = new Size(115, 36);
            btnReportExcel.Text = "Export Excel";
            btnReportExcel.TabIndex = 7;
            btnReportExcel.Click += btnReportExcel_Click;
            // 
            // lblReportMsg
            // 
            lblReportMsg.AutoSize = true;
            lblReportMsg.Location = new Point(38, 211);
            lblReportMsg.Name = "lblReportMsg";
            // 
            // ReceiptReportsTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblReportType);
            Controls.Add(cboReportType);
            Controls.Add(lblReportProject);
            Controls.Add(cboReportProject);
            Controls.Add(lblReportFrom);
            Controls.Add(dtpReportFrom);
            Controls.Add(lblReportTo);
            Controls.Add(dtpReportTo);
            Controls.Add(btnPreviewReport);
            Controls.Add(btnPrintReport);
            Controls.Add(btnReportPdf);
            Controls.Add(btnReportExcel);
            Controls.Add(lblReportMsg);
            Name = "ReceiptReportsTab";
            Size = new Size(1152, 296);
            Load += ReceiptReportsTab_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblReportType;
        private ComboBox cboReportType;
        private Label lblReportProject;
        private ComboBox cboReportProject;
        private Label lblReportFrom;
        private DateTimePicker dtpReportFrom;
        private Label lblReportTo;
        private DateTimePicker dtpReportTo;
        private Button btnPreviewReport;
        private Button btnPrintReport;
        private Button btnReportPdf;
        private Button btnReportExcel;
        private Label lblReportMsg;
    }
}
