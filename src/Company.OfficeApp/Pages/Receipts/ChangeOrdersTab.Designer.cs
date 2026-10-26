namespace Company.OfficeApp.Pages.Receipts
{
    partial class ChangeOrdersTab
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
            lblCoProjectFilter = new Label();
            cboCoProjectFilter = new ComboBox();
            dgvChangeOrders = new DataGridView();
            btnNewChangeOrder = new Button();
            btnApproveCo = new Button();
            btnRejectCo = new Button();
            btnPrintCo = new Button();
            lblAdminNote = new Label();
            lblCoNumber = new Label();
            lblCoProject = new Label();
            cboCoProject = new ComboBox();
            lblCoDate = new Label();
            dtpCoDate = new DateTimePicker();
            lblCoDescription = new Label();
            txtCoDescription = new TextBox();
            lblCoCostChange = new Label();
            numCoCostChange = new NumericUpDown();
            lblCoDaysChange = new Label();
            numCoDaysChange = new NumericUpDown();
            btnAttachCoDoc = new Button();
            btnViewCoDoc = new Button();
            lblCoDocFile = new Label();
            lblCoMsg = new Label();
            btnSubmitCo = new Button();
            btnCancelCo = new Button();
            lblCoImpactTitle = new Label();
            lblCoOriginalPrice = new Label();
            lblCoRevisedPrice = new Label();
            lblCoOriginalEnd = new Label();
            lblCoRevisedEnd = new Label();
            colCoNo = new DataGridViewTextBoxColumn();
            colProject = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();
            colCost = new DataGridViewTextBoxColumn();
            colDays = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvChangeOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCoCostChange).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCoDaysChange).BeginInit();
            SuspendLayout();
            // 
            // lblCoProjectFilter
            // 
            lblCoProjectFilter.AutoSize = true;
            lblCoProjectFilter.Location = new Point(12, 15);
            lblCoProjectFilter.Name = "lblCoProjectFilter";
            lblCoProjectFilter.Text = "Project";
            // 
            // cboCoProjectFilter
            // 
            cboCoProjectFilter.Location = new Point(62, 12);
            cboCoProjectFilter.Name = "cboCoProjectFilter";
            cboCoProjectFilter.Size = new Size(240, 23);
            cboCoProjectFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCoProjectFilter.TabIndex = 0;
            cboCoProjectFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // dgvChangeOrders
            // 
            dgvChangeOrders.AllowUserToAddRows = false;
            dgvChangeOrders.AllowUserToDeleteRows = false;
            dgvChangeOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChangeOrders.Columns.AddRange(new DataGridViewColumn[] { colCoNo, colProject, colDate, colCost, colDays, colStatus });
            dgvChangeOrders.MultiSelect = false;
            dgvChangeOrders.ReadOnly = true;
            dgvChangeOrders.RowHeadersVisible = false;
            dgvChangeOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChangeOrders.Location = new Point(12, 44);
            dgvChangeOrders.Name = "dgvChangeOrders";
            dgvChangeOrders.Size = new Size(930, 170);
            dgvChangeOrders.TabIndex = 1;
            dgvChangeOrders.SelectionChanged += dgvChangeOrders_SelectionChanged;
            // 
            // btnNewChangeOrder
            // 
            btnNewChangeOrder.Location = new Point(12, 222);
            btnNewChangeOrder.Name = "btnNewChangeOrder";
            btnNewChangeOrder.Size = new Size(140, 28);
            btnNewChangeOrder.Text = "New Change Order";
            btnNewChangeOrder.TabIndex = 2;
            btnNewChangeOrder.Click += btnNewChangeOrder_Click;
            // 
            // btnApproveCo
            // 
            btnApproveCo.Location = new Point(158, 222);
            btnApproveCo.Name = "btnApproveCo";
            btnApproveCo.Size = new Size(80, 28);
            btnApproveCo.Text = "Approve";
            btnApproveCo.TabIndex = 3;
            btnApproveCo.Click += btnApproveCo_Click;
            // 
            // btnRejectCo
            // 
            btnRejectCo.Location = new Point(244, 222);
            btnRejectCo.Name = "btnRejectCo";
            btnRejectCo.Size = new Size(70, 28);
            btnRejectCo.Text = "Reject";
            btnRejectCo.TabIndex = 4;
            btnRejectCo.Click += btnRejectCo_Click;
            // 
            // btnPrintCo
            // 
            btnPrintCo.Location = new Point(320, 222);
            btnPrintCo.Name = "btnPrintCo";
            btnPrintCo.Size = new Size(90, 28);
            btnPrintCo.Text = "Print Form";
            btnPrintCo.TabIndex = 5;
            btnPrintCo.Click += btnPrintCo_Click;
            // 
            // lblAdminNote
            // 
            lblAdminNote.AutoSize = true;
            lblAdminNote.Location = new Point(12, 256);
            lblAdminNote.Name = "lblAdminNote";
            lblAdminNote.Text = "An Admin approves change orders.";
            // 
            // lblCoNumber
            // 
            lblCoNumber.AutoSize = true;
            lblCoNumber.Location = new Point(12, 282);
            lblCoNumber.Name = "lblCoNumber";
            lblCoNumber.Text = "CO-001 (new)";
            // 
            // lblCoProject
            // 
            lblCoProject.AutoSize = true;
            lblCoProject.Location = new Point(12, 306);
            lblCoProject.Name = "lblCoProject";
            lblCoProject.Text = "Project";
            // 
            // cboCoProject
            // 
            cboCoProject.Location = new Point(12, 324);
            cboCoProject.Name = "cboCoProject";
            cboCoProject.Size = new Size(280, 23);
            cboCoProject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCoProject.TabIndex = 6;
            cboCoProject.SelectedIndexChanged += Impact_Changed;
            // 
            // lblCoDate
            // 
            lblCoDate.AutoSize = true;
            lblCoDate.Location = new Point(12, 354);
            lblCoDate.Name = "lblCoDate";
            lblCoDate.Text = "Date requested";
            // 
            // dtpCoDate
            // 
            dtpCoDate.Location = new Point(12, 372);
            dtpCoDate.Name = "dtpCoDate";
            dtpCoDate.Size = new Size(125, 23);
            dtpCoDate.Format = DateTimePickerFormat.Short;
            dtpCoDate.TabIndex = 7;
            // 
            // lblCoDescription
            // 
            lblCoDescription.AutoSize = true;
            lblCoDescription.Location = new Point(12, 402);
            lblCoDescription.Name = "lblCoDescription";
            lblCoDescription.Text = "What the client wants changed";
            // 
            // txtCoDescription
            // 
            txtCoDescription.Location = new Point(12, 420);
            txtCoDescription.Name = "txtCoDescription";
            txtCoDescription.Size = new Size(280, 70);
            txtCoDescription.Multiline = true;
            txtCoDescription.ScrollBars = ScrollBars.Vertical;
            txtCoDescription.TabIndex = 8;
            // 
            // lblCoCostChange
            // 
            lblCoCostChange.AutoSize = true;
            lblCoCostChange.Location = new Point(12, 498);
            lblCoCostChange.Name = "lblCoCostChange";
            lblCoCostChange.Text = "Cost change (+ or -)";
            // 
            // numCoCostChange
            // 
            numCoCostChange.Location = new Point(12, 516);
            numCoCostChange.Name = "numCoCostChange";
            numCoCostChange.Size = new Size(140, 23);
            numCoCostChange.DecimalPlaces = 2;
            numCoCostChange.ThousandsSeparator = true;
            numCoCostChange.Maximum = 999999999999m;
            numCoCostChange.Minimum = -999999999999m;
            numCoCostChange.TabIndex = 9;
            numCoCostChange.ValueChanged += Impact_Changed;
            // 
            // lblCoDaysChange
            // 
            lblCoDaysChange.AutoSize = true;
            lblCoDaysChange.Location = new Point(162, 498);
            lblCoDaysChange.Name = "lblCoDaysChange";
            lblCoDaysChange.Text = "Days change (+ or -)";
            // 
            // numCoDaysChange
            // 
            numCoDaysChange.Location = new Point(162, 516);
            numCoDaysChange.Name = "numCoDaysChange";
            numCoDaysChange.Size = new Size(90, 23);
            numCoDaysChange.Maximum = 3650m;
            numCoDaysChange.Minimum = -3650m;
            numCoDaysChange.TabIndex = 10;
            numCoDaysChange.ValueChanged += Impact_Changed;
            // 
            // btnAttachCoDoc
            // 
            btnAttachCoDoc.Location = new Point(12, 548);
            btnAttachCoDoc.Name = "btnAttachCoDoc";
            btnAttachCoDoc.Size = new Size(170, 28);
            btnAttachCoDoc.Text = "Attach Signed Document";
            btnAttachCoDoc.TabIndex = 11;
            btnAttachCoDoc.Click += btnAttachCoDoc_Click;
            // 
            // btnViewCoDoc
            // 
            btnViewCoDoc.Location = new Point(188, 548);
            btnViewCoDoc.Name = "btnViewCoDoc";
            btnViewCoDoc.Size = new Size(70, 28);
            btnViewCoDoc.Text = "View";
            btnViewCoDoc.TabIndex = 12;
            btnViewCoDoc.Click += btnViewCoDoc_Click;
            // 
            // lblCoDocFile
            // 
            lblCoDocFile.AutoSize = true;
            lblCoDocFile.Location = new Point(264, 554);
            lblCoDocFile.Name = "lblCoDocFile";
            lblCoDocFile.Text = "";
            // 
            // lblCoMsg
            // 
            lblCoMsg.AutoSize = true;
            lblCoMsg.Location = new Point(12, 584);
            lblCoMsg.Name = "lblCoMsg";
            // 
            // btnSubmitCo
            // 
            btnSubmitCo.Location = new Point(12, 606);
            btnSubmitCo.Name = "btnSubmitCo";
            btnSubmitCo.Size = new Size(90, 28);
            btnSubmitCo.Text = "Submit";
            btnSubmitCo.TabIndex = 13;
            btnSubmitCo.Click += btnSubmitCo_Click;
            // 
            // btnCancelCo
            // 
            btnCancelCo.Location = new Point(108, 606);
            btnCancelCo.Name = "btnCancelCo";
            btnCancelCo.Size = new Size(90, 28);
            btnCancelCo.Text = "Cancel";
            btnCancelCo.TabIndex = 14;
            btnCancelCo.Click += btnCancelCo_Click;
            // 
            // lblCoImpactTitle
            // 
            lblCoImpactTitle.AutoSize = true;
            lblCoImpactTitle.Location = new Point(340, 306);
            lblCoImpactTitle.Name = "lblCoImpactTitle";
            lblCoImpactTitle.Text = "Budget impact if approved";
            // 
            // lblCoOriginalPrice
            // 
            lblCoOriginalPrice.AutoSize = true;
            lblCoOriginalPrice.Location = new Point(340, 330);
            lblCoOriginalPrice.Name = "lblCoOriginalPrice";
            lblCoOriginalPrice.Text = "Contract price now: -";
            // 
            // lblCoRevisedPrice
            // 
            lblCoRevisedPrice.AutoSize = true;
            lblCoRevisedPrice.Location = new Point(340, 354);
            lblCoRevisedPrice.Name = "lblCoRevisedPrice";
            lblCoRevisedPrice.Text = "Revised contract price: -";
            // 
            // lblCoOriginalEnd
            // 
            lblCoOriginalEnd.AutoSize = true;
            lblCoOriginalEnd.Location = new Point(340, 378);
            lblCoOriginalEnd.Name = "lblCoOriginalEnd";
            lblCoOriginalEnd.Text = "Target end date now: -";
            // 
            // lblCoRevisedEnd
            // 
            lblCoRevisedEnd.AutoSize = true;
            lblCoRevisedEnd.Location = new Point(340, 402);
            lblCoRevisedEnd.Name = "lblCoRevisedEnd";
            lblCoRevisedEnd.Text = "Revised end date: -";
            // 
            // colCoNo
            // 
            colCoNo.DataPropertyName = "CoNo";
            colCoNo.HeaderText = "CO No.";
            colCoNo.Name = "colCoNo";
            // 
            // colProject
            // 
            colProject.DataPropertyName = "Project";
            colProject.HeaderText = "Project";
            colProject.Name = "colProject";
            // 
            // colDate
            // 
            colDate.DataPropertyName = "RequestedOn";
            colDate.HeaderText = "Date";
            colDate.Name = "colDate";
            colDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colCost
            // 
            colCost.DataPropertyName = "CostChange";
            colCost.HeaderText = "Cost Change";
            colCost.Name = "colCost";
            colCost.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colDays
            // 
            colDays.DataPropertyName = "DaysChange";
            colDays.HeaderText = "Days Change";
            colDays.Name = "colDays";
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            // 
            // ChangeOrdersTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblCoProjectFilter);
            Controls.Add(cboCoProjectFilter);
            Controls.Add(dgvChangeOrders);
            Controls.Add(btnNewChangeOrder);
            Controls.Add(btnApproveCo);
            Controls.Add(btnRejectCo);
            Controls.Add(btnPrintCo);
            Controls.Add(lblAdminNote);
            Controls.Add(lblCoNumber);
            Controls.Add(lblCoProject);
            Controls.Add(cboCoProject);
            Controls.Add(lblCoDate);
            Controls.Add(dtpCoDate);
            Controls.Add(lblCoDescription);
            Controls.Add(txtCoDescription);
            Controls.Add(lblCoCostChange);
            Controls.Add(numCoCostChange);
            Controls.Add(lblCoDaysChange);
            Controls.Add(numCoDaysChange);
            Controls.Add(btnAttachCoDoc);
            Controls.Add(btnViewCoDoc);
            Controls.Add(lblCoDocFile);
            Controls.Add(lblCoMsg);
            Controls.Add(btnSubmitCo);
            Controls.Add(btnCancelCo);
            Controls.Add(lblCoImpactTitle);
            Controls.Add(lblCoOriginalPrice);
            Controls.Add(lblCoRevisedPrice);
            Controls.Add(lblCoOriginalEnd);
            Controls.Add(lblCoRevisedEnd);
            Name = "ChangeOrdersTab";
            Size = new Size(960, 650);
            Load += ChangeOrdersTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvChangeOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCoCostChange).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCoDaysChange).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCoProjectFilter;
        private ComboBox cboCoProjectFilter;
        private DataGridView dgvChangeOrders;
        private Button btnNewChangeOrder;
        private Button btnApproveCo;
        private Button btnRejectCo;
        private Button btnPrintCo;
        private Label lblAdminNote;
        private Label lblCoNumber;
        private Label lblCoProject;
        private ComboBox cboCoProject;
        private Label lblCoDate;
        private DateTimePicker dtpCoDate;
        private Label lblCoDescription;
        private TextBox txtCoDescription;
        private Label lblCoCostChange;
        private NumericUpDown numCoCostChange;
        private Label lblCoDaysChange;
        private NumericUpDown numCoDaysChange;
        private Button btnAttachCoDoc;
        private Button btnViewCoDoc;
        private Label lblCoDocFile;
        private Label lblCoMsg;
        private Button btnSubmitCo;
        private Button btnCancelCo;
        private Label lblCoImpactTitle;
        private Label lblCoOriginalPrice;
        private Label lblCoRevisedPrice;
        private Label lblCoOriginalEnd;
        private Label lblCoRevisedEnd;
        private DataGridViewTextBoxColumn colCoNo;
        private DataGridViewTextBoxColumn colProject;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colCost;
        private DataGridViewTextBoxColumn colDays;
        private DataGridViewTextBoxColumn colStatus;
    }
}
