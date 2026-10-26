namespace Company.OfficeApp.Pages.Shared
{
    partial class EditHistoryTab
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
            lblHistFrom = new Label();
            dtpHistFrom = new DateTimePicker();
            lblHistTo = new Label();
            dtpHistTo = new DateTimePicker();
            lblHistUser = new Label();
            cboHistUser = new ComboBox();
            lblHistSearch = new Label();
            txtHistSearch = new TextBox();
            dgvEditHistory = new DataGridView();
            lblHistCount = new Label();
            btnExportHistory = new Button();
            colChangedAt = new DataGridViewTextBoxColumn();
            colWho = new DataGridViewTextBoxColumn();
            colAction = new DataGridViewTextBoxColumn();
            colRecord = new DataGridViewTextBoxColumn();
            colField = new DataGridViewTextBoxColumn();
            colOld = new DataGridViewTextBoxColumn();
            colNew = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvEditHistory).BeginInit();
            SuspendLayout();
            // 
            // lblHistFrom
            // 
            lblHistFrom.AutoSize = true;
            lblHistFrom.Location = new Point(12, 15);
            lblHistFrom.Name = "lblHistFrom";
            lblHistFrom.Text = "From";
            // 
            // dtpHistFrom
            // 
            dtpHistFrom.Location = new Point(52, 12);
            dtpHistFrom.Name = "dtpHistFrom";
            dtpHistFrom.Size = new Size(120, 23);
            dtpHistFrom.Format = DateTimePickerFormat.Short;
            dtpHistFrom.TabIndex = 0;
            dtpHistFrom.ValueChanged += Filter_Changed;
            // 
            // lblHistTo
            // 
            lblHistTo.AutoSize = true;
            lblHistTo.Location = new Point(184, 15);
            lblHistTo.Name = "lblHistTo";
            lblHistTo.Text = "To";
            // 
            // dtpHistTo
            // 
            dtpHistTo.Location = new Point(210, 12);
            dtpHistTo.Name = "dtpHistTo";
            dtpHistTo.Size = new Size(120, 23);
            dtpHistTo.Format = DateTimePickerFormat.Short;
            dtpHistTo.TabIndex = 1;
            dtpHistTo.ValueChanged += Filter_Changed;
            // 
            // lblHistUser
            // 
            lblHistUser.AutoSize = true;
            lblHistUser.Location = new Point(344, 15);
            lblHistUser.Name = "lblHistUser";
            lblHistUser.Text = "Person";
            // 
            // cboHistUser
            // 
            cboHistUser.Location = new Point(394, 12);
            cboHistUser.Name = "cboHistUser";
            cboHistUser.Size = new Size(180, 23);
            cboHistUser.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHistUser.TabIndex = 2;
            cboHistUser.SelectedIndexChanged += Filter_Changed;
            // 
            // lblHistSearch
            // 
            lblHistSearch.AutoSize = true;
            lblHistSearch.Location = new Point(588, 15);
            lblHistSearch.Name = "lblHistSearch";
            lblHistSearch.Text = "Search record";
            // 
            // txtHistSearch
            // 
            txtHistSearch.Location = new Point(676, 12);
            txtHistSearch.Name = "txtHistSearch";
            txtHistSearch.Size = new Size(160, 23);
            txtHistSearch.TabIndex = 3;
            // 
            // dgvEditHistory
            // 
            dgvEditHistory.AllowUserToAddRows = false;
            dgvEditHistory.AllowUserToDeleteRows = false;
            dgvEditHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEditHistory.Columns.AddRange(new DataGridViewColumn[] { colChangedAt, colWho, colAction, colRecord, colField, colOld, colNew });
            dgvEditHistory.MultiSelect = false;
            dgvEditHistory.ReadOnly = true;
            dgvEditHistory.RowHeadersVisible = false;
            dgvEditHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEditHistory.Location = new Point(12, 44);
            dgvEditHistory.Name = "dgvEditHistory";
            dgvEditHistory.Size = new Size(930, 420);
            dgvEditHistory.TabIndex = 4;
            // 
            // lblHistCount
            // 
            lblHistCount.AutoSize = true;
            lblHistCount.Location = new Point(12, 476);
            lblHistCount.Name = "lblHistCount";
            lblHistCount.Text = "0 changes";
            // 
            // btnExportHistory
            // 
            btnExportHistory.Location = new Point(852, 472);
            btnExportHistory.Name = "btnExportHistory";
            btnExportHistory.Size = new Size(90, 28);
            btnExportHistory.Text = "Export";
            btnExportHistory.TabIndex = 5;
            btnExportHistory.Click += btnExportHistory_Click;
            // 
            // colChangedAt
            // 
            colChangedAt.DataPropertyName = "ChangedAt";
            colChangedAt.HeaderText = "Date & Time";
            colChangedAt.Name = "colChangedAt";
            colChangedAt.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy h:mm tt" };
            // 
            // colWho
            // 
            colWho.DataPropertyName = "Who";
            colWho.HeaderText = "Who";
            colWho.Name = "colWho";
            // 
            // colAction
            // 
            colAction.DataPropertyName = "Action";
            colAction.HeaderText = "Action";
            colAction.Name = "colAction";
            // 
            // colRecord
            // 
            colRecord.DataPropertyName = "Record";
            colRecord.HeaderText = "Record";
            colRecord.Name = "colRecord";
            // 
            // colField
            // 
            colField.DataPropertyName = "Field";
            colField.HeaderText = "Field";
            colField.Name = "colField";
            // 
            // colOld
            // 
            colOld.DataPropertyName = "OldValue";
            colOld.HeaderText = "Old Value";
            colOld.Name = "colOld";
            // 
            // colNew
            // 
            colNew.DataPropertyName = "NewValue";
            colNew.HeaderText = "New Value";
            colNew.Name = "colNew";
            // 
            // EditHistoryTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblHistFrom);
            Controls.Add(dtpHistFrom);
            Controls.Add(lblHistTo);
            Controls.Add(dtpHistTo);
            Controls.Add(lblHistUser);
            Controls.Add(cboHistUser);
            Controls.Add(lblHistSearch);
            Controls.Add(txtHistSearch);
            Controls.Add(dgvEditHistory);
            Controls.Add(lblHistCount);
            Controls.Add(btnExportHistory);
            Name = "EditHistoryTab";
            Size = new Size(960, 520);
            Load += EditHistoryTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvEditHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHistFrom;
        private DateTimePicker dtpHistFrom;
        private Label lblHistTo;
        private DateTimePicker dtpHistTo;
        private Label lblHistUser;
        private ComboBox cboHistUser;
        private Label lblHistSearch;
        private TextBox txtHistSearch;
        private DataGridView dgvEditHistory;
        private Label lblHistCount;
        private Button btnExportHistory;
        private DataGridViewTextBoxColumn colChangedAt;
        private DataGridViewTextBoxColumn colWho;
        private DataGridViewTextBoxColumn colAction;
        private DataGridViewTextBoxColumn colRecord;
        private DataGridViewTextBoxColumn colField;
        private DataGridViewTextBoxColumn colOld;
        private DataGridViewTextBoxColumn colNew;
    }
}
