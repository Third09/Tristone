namespace Company.OfficeApp.Pages.Subcontractors
{
    partial class SubJobsTab
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
            lblProjectFilter = new Label();
            cboJobProjectFilter = new ComboBox();
            lblSubFilter = new Label();
            cboJobSubFilter = new ComboBox();
            dgvJobs = new DataGridView();
            btnNewJob = new Button();
            btnDeleteJob = new Button();
            lblFormMode = new Label();
            lblJobSubcontractor = new Label();
            cboJobSubcontractor = new ComboBox();
            lblJobProject = new Label();
            cboJobProject = new ComboBox();
            lblJobScope = new Label();
            txtJobScope = new TextBox();
            lblJobAmount = new Label();
            numJobAmount = new NumericUpDown();
            lblRetentionPct = new Label();
            numRetentionPct = new NumericUpDown();
            lblJobStart = new Label();
            dtpJobStart = new DateTimePicker();
            lblJobEnd = new Label();
            dtpJobEnd = new DateTimePicker();
            lblJobMsg = new Label();
            btnSaveJob = new Button();
            btnCancelJob = new Button();
            lblStages = new Label();
            dgvJobStages = new DataGridView();
            lblStagesTotal = new Label();
            btnAddStage = new Button();
            btnRemoveStage = new Button();
            lblJobDocs = new Label();
            dgvJobDocs = new DataGridView();
            btnUploadJobDoc = new Button();
            btnViewJobDoc = new Button();
            lblProgressTitle = new Label();
            prgJobProgress = new ProgressBar();
            lblProgressPct = new Label();
            lblProgressPctInput = new Label();
            numProgressPct = new NumericUpDown();
            lblProgressDate = new Label();
            dtpProgressDate = new DateTimePicker();
            lblProgressNotes = new Label();
            txtProgressNotes = new TextBox();
            btnAttachProgressFile = new Button();
            btnAddProgressUpdate = new Button();
            lblProgressFile = new Label();
            lblProgressMsg = new Label();
            dgvProgressHistory = new DataGridView();
            colSub = new DataGridViewTextBoxColumn();
            colProject = new DataGridViewTextBoxColumn();
            colScope = new DataGridViewTextBoxColumn();
            colAmount = new DataGridViewTextBoxColumn();
            colDone = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colStageName = new DataGridViewTextBoxColumn();
            colStagePct = new DataGridViewTextBoxColumn();
            colStageAmount = new DataGridViewTextBoxColumn();
            colDocFile = new DataGridViewTextBoxColumn();
            colDocUploaded = new DataGridViewTextBoxColumn();
            colPDate = new DataGridViewTextBoxColumn();
            colPPct = new DataGridViewTextBoxColumn();
            colPNotes = new DataGridViewTextBoxColumn();
            colPFile = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvJobs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numJobAmount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRetentionPct).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvJobStages).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvJobDocs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numProgressPct).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProgressHistory).BeginInit();
            SuspendLayout();
            // 
            // lblProjectFilter
            // 
            lblProjectFilter.AutoSize = true;
            lblProjectFilter.Location = new Point(12, 15);
            lblProjectFilter.Name = "lblProjectFilter";
            lblProjectFilter.Text = "Project";
            // 
            // cboJobProjectFilter
            // 
            cboJobProjectFilter.Location = new Point(62, 12);
            cboJobProjectFilter.Name = "cboJobProjectFilter";
            cboJobProjectFilter.Size = new Size(220, 23);
            cboJobProjectFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboJobProjectFilter.TabIndex = 0;
            cboJobProjectFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // lblSubFilter
            // 
            lblSubFilter.AutoSize = true;
            lblSubFilter.Location = new Point(294, 15);
            lblSubFilter.Name = "lblSubFilter";
            lblSubFilter.Text = "Subcontractor";
            // 
            // cboJobSubFilter
            // 
            cboJobSubFilter.Location = new Point(380, 12);
            cboJobSubFilter.Name = "cboJobSubFilter";
            cboJobSubFilter.Size = new Size(220, 23);
            cboJobSubFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboJobSubFilter.TabIndex = 1;
            cboJobSubFilter.SelectedIndexChanged += Filter_Changed;
            // 
            // dgvJobs
            // 
            dgvJobs.AllowUserToAddRows = false;
            dgvJobs.AllowUserToDeleteRows = false;
            dgvJobs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvJobs.Columns.AddRange(new DataGridViewColumn[] { colSub, colProject, colScope, colAmount, colDone, colStatus });
            dgvJobs.MultiSelect = false;
            dgvJobs.ReadOnly = true;
            dgvJobs.RowHeadersVisible = false;
            dgvJobs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJobs.Location = new Point(12, 44);
            dgvJobs.Name = "dgvJobs";
            dgvJobs.Size = new Size(930, 150);
            dgvJobs.TabIndex = 2;
            dgvJobs.SelectionChanged += dgvJobs_SelectionChanged;
            // 
            // btnNewJob
            // 
            btnNewJob.Location = new Point(12, 202);
            btnNewJob.Name = "btnNewJob";
            btnNewJob.Size = new Size(90, 28);
            btnNewJob.Text = "New Job";
            btnNewJob.TabIndex = 3;
            btnNewJob.Click += btnNewJob_Click;
            // 
            // btnDeleteJob
            // 
            btnDeleteJob.Location = new Point(108, 202);
            btnDeleteJob.Name = "btnDeleteJob";
            btnDeleteJob.Size = new Size(90, 28);
            btnDeleteJob.Text = "Delete";
            btnDeleteJob.TabIndex = 4;
            btnDeleteJob.Click += btnDeleteJob_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(12, 244);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New job";
            // 
            // lblJobSubcontractor
            // 
            lblJobSubcontractor.AutoSize = true;
            lblJobSubcontractor.Location = new Point(12, 268);
            lblJobSubcontractor.Name = "lblJobSubcontractor";
            lblJobSubcontractor.Text = "Subcontractor";
            // 
            // cboJobSubcontractor
            // 
            cboJobSubcontractor.Location = new Point(12, 286);
            cboJobSubcontractor.Name = "cboJobSubcontractor";
            cboJobSubcontractor.Size = new Size(260, 23);
            cboJobSubcontractor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboJobSubcontractor.TabIndex = 5;
            // 
            // lblJobProject
            // 
            lblJobProject.AutoSize = true;
            lblJobProject.Location = new Point(12, 316);
            lblJobProject.Name = "lblJobProject";
            lblJobProject.Text = "Project";
            // 
            // cboJobProject
            // 
            cboJobProject.Location = new Point(12, 334);
            cboJobProject.Name = "cboJobProject";
            cboJobProject.Size = new Size(260, 23);
            cboJobProject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboJobProject.TabIndex = 6;
            // 
            // lblJobScope
            // 
            lblJobScope.AutoSize = true;
            lblJobScope.Location = new Point(12, 364);
            lblJobScope.Name = "lblJobScope";
            lblJobScope.Text = "Scope of work";
            // 
            // txtJobScope
            // 
            txtJobScope.Location = new Point(12, 382);
            txtJobScope.Name = "txtJobScope";
            txtJobScope.Size = new Size(260, 56);
            txtJobScope.Multiline = true;
            txtJobScope.ScrollBars = ScrollBars.Vertical;
            txtJobScope.TabIndex = 7;
            // 
            // lblJobAmount
            // 
            lblJobAmount.AutoSize = true;
            lblJobAmount.Location = new Point(12, 446);
            lblJobAmount.Name = "lblJobAmount";
            lblJobAmount.Text = "Contract amount";
            // 
            // numJobAmount
            // 
            numJobAmount.Location = new Point(12, 464);
            numJobAmount.Name = "numJobAmount";
            numJobAmount.Size = new Size(130, 23);
            numJobAmount.DecimalPlaces = 2;
            numJobAmount.ThousandsSeparator = true;
            numJobAmount.Maximum = 999999999999m;
            numJobAmount.TabIndex = 8;
            numJobAmount.ValueChanged += Stages_Changed;
            // 
            // lblRetentionPct
            // 
            lblRetentionPct.AutoSize = true;
            lblRetentionPct.Location = new Point(152, 446);
            lblRetentionPct.Name = "lblRetentionPct";
            lblRetentionPct.Text = "Retention %";
            // 
            // numRetentionPct
            // 
            numRetentionPct.Location = new Point(152, 464);
            numRetentionPct.Name = "numRetentionPct";
            numRetentionPct.Size = new Size(70, 23);
            numRetentionPct.DecimalPlaces = 2;
            numRetentionPct.Maximum = 99m;
            numRetentionPct.Value = 10m;
            numRetentionPct.TabIndex = 9;
            // 
            // lblJobStart
            // 
            lblJobStart.AutoSize = true;
            lblJobStart.Location = new Point(12, 494);
            lblJobStart.Name = "lblJobStart";
            lblJobStart.Text = "Start date";
            // 
            // dtpJobStart
            // 
            dtpJobStart.Location = new Point(12, 512);
            dtpJobStart.Name = "dtpJobStart";
            dtpJobStart.Size = new Size(125, 23);
            dtpJobStart.Format = DateTimePickerFormat.Short;
            dtpJobStart.TabIndex = 10;
            // 
            // lblJobEnd
            // 
            lblJobEnd.AutoSize = true;
            lblJobEnd.Location = new Point(147, 494);
            lblJobEnd.Name = "lblJobEnd";
            lblJobEnd.Text = "End date";
            // 
            // dtpJobEnd
            // 
            dtpJobEnd.Location = new Point(147, 512);
            dtpJobEnd.Name = "dtpJobEnd";
            dtpJobEnd.Size = new Size(125, 23);
            dtpJobEnd.Format = DateTimePickerFormat.Short;
            dtpJobEnd.TabIndex = 11;
            // 
            // lblJobMsg
            // 
            lblJobMsg.AutoSize = true;
            lblJobMsg.Location = new Point(12, 546);
            lblJobMsg.Name = "lblJobMsg";
            // 
            // btnSaveJob
            // 
            btnSaveJob.Location = new Point(12, 568);
            btnSaveJob.Name = "btnSaveJob";
            btnSaveJob.Size = new Size(90, 28);
            btnSaveJob.Text = "Save Job";
            btnSaveJob.TabIndex = 12;
            btnSaveJob.Click += btnSaveJob_Click;
            // 
            // btnCancelJob
            // 
            btnCancelJob.Location = new Point(108, 568);
            btnCancelJob.Name = "btnCancelJob";
            btnCancelJob.Size = new Size(90, 28);
            btnCancelJob.Text = "Cancel";
            btnCancelJob.TabIndex = 13;
            btnCancelJob.Click += btnCancelJob_Click;
            // 
            // lblStages
            // 
            lblStages.AutoSize = true;
            lblStages.Location = new Point(300, 268);
            lblStages.Name = "lblStages";
            lblStages.Text = "Payment stages (must add up to 100%)";
            // 
            // dgvJobStages
            // 
            dgvJobStages.AllowUserToAddRows = false;
            dgvJobStages.AllowUserToDeleteRows = false;
            dgvJobStages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvJobStages.Columns.AddRange(new DataGridViewColumn[] { colStageName, colStagePct, colStageAmount });
            dgvJobStages.MultiSelect = false;
            dgvJobStages.ReadOnly = true;
            dgvJobStages.RowHeadersVisible = false;
            dgvJobStages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJobStages.Location = new Point(300, 286);
            dgvJobStages.Name = "dgvJobStages";
            dgvJobStages.Size = new Size(300, 150);
            dgvJobStages.ReadOnly = false;
            dgvJobStages.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvJobStages.TabIndex = 14;
            dgvJobStages.CellEndEdit += dgvJobStages_CellEndEdit;
            dgvJobStages.DataError += dgvJobStages_DataError;
            // 
            // lblStagesTotal
            // 
            lblStagesTotal.AutoSize = true;
            lblStagesTotal.Location = new Point(300, 444);
            lblStagesTotal.Name = "lblStagesTotal";
            lblStagesTotal.Text = "Total: 100%";
            // 
            // btnAddStage
            // 
            btnAddStage.Location = new Point(300, 466);
            btnAddStage.Name = "btnAddStage";
            btnAddStage.Size = new Size(100, 28);
            btnAddStage.Text = "Add Stage";
            btnAddStage.TabIndex = 15;
            btnAddStage.Click += btnAddStage_Click;
            // 
            // btnRemoveStage
            // 
            btnRemoveStage.Location = new Point(406, 466);
            btnRemoveStage.Name = "btnRemoveStage";
            btnRemoveStage.Size = new Size(110, 28);
            btnRemoveStage.Text = "Remove Stage";
            btnRemoveStage.TabIndex = 16;
            btnRemoveStage.Click += btnRemoveStage_Click;
            // 
            // lblJobDocs
            // 
            lblJobDocs.AutoSize = true;
            lblJobDocs.Location = new Point(628, 268);
            lblJobDocs.Name = "lblJobDocs";
            lblJobDocs.Text = "Supporting papers (contract, scope)";
            // 
            // dgvJobDocs
            // 
            dgvJobDocs.AllowUserToAddRows = false;
            dgvJobDocs.AllowUserToDeleteRows = false;
            dgvJobDocs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvJobDocs.Columns.AddRange(new DataGridViewColumn[] { colDocFile, colDocUploaded });
            dgvJobDocs.MultiSelect = false;
            dgvJobDocs.ReadOnly = true;
            dgvJobDocs.RowHeadersVisible = false;
            dgvJobDocs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJobDocs.Location = new Point(628, 286);
            dgvJobDocs.Name = "dgvJobDocs";
            dgvJobDocs.Size = new Size(314, 150);
            dgvJobDocs.TabIndex = 17;
            // 
            // btnUploadJobDoc
            // 
            btnUploadJobDoc.Location = new Point(628, 444);
            btnUploadJobDoc.Name = "btnUploadJobDoc";
            btnUploadJobDoc.Size = new Size(130, 28);
            btnUploadJobDoc.Text = "Upload Document";
            btnUploadJobDoc.TabIndex = 18;
            btnUploadJobDoc.Click += btnUploadJobDoc_Click;
            // 
            // btnViewJobDoc
            // 
            btnViewJobDoc.Location = new Point(764, 444);
            btnViewJobDoc.Name = "btnViewJobDoc";
            btnViewJobDoc.Size = new Size(80, 28);
            btnViewJobDoc.Text = "View";
            btnViewJobDoc.TabIndex = 19;
            btnViewJobDoc.Click += btnViewJobDoc_Click;
            // 
            // lblProgressTitle
            // 
            lblProgressTitle.AutoSize = true;
            lblProgressTitle.Location = new Point(12, 612);
            lblProgressTitle.Name = "lblProgressTitle";
            lblProgressTitle.Text = "Progress (selected job)";
            // 
            // prgJobProgress
            // 
            prgJobProgress.Location = new Point(12, 632);
            prgJobProgress.Name = "prgJobProgress";
            prgJobProgress.Size = new Size(260, 20);
            prgJobProgress.TabIndex = 20;
            // 
            // lblProgressPct
            // 
            lblProgressPct.AutoSize = true;
            lblProgressPct.Location = new Point(280, 634);
            lblProgressPct.Name = "lblProgressPct";
            lblProgressPct.Text = "";
            // 
            // lblProgressPctInput
            // 
            lblProgressPctInput.AutoSize = true;
            lblProgressPctInput.Location = new Point(12, 662);
            lblProgressPctInput.Name = "lblProgressPctInput";
            lblProgressPctInput.Text = "% completed";
            // 
            // numProgressPct
            // 
            numProgressPct.Location = new Point(12, 680);
            numProgressPct.Name = "numProgressPct";
            numProgressPct.Size = new Size(70, 23);
            numProgressPct.DecimalPlaces = 0;
            numProgressPct.TabIndex = 21;
            // 
            // lblProgressDate
            // 
            lblProgressDate.AutoSize = true;
            lblProgressDate.Location = new Point(92, 662);
            lblProgressDate.Name = "lblProgressDate";
            lblProgressDate.Text = "Date";
            // 
            // dtpProgressDate
            // 
            dtpProgressDate.Location = new Point(92, 680);
            dtpProgressDate.Name = "dtpProgressDate";
            dtpProgressDate.Size = new Size(125, 23);
            dtpProgressDate.Format = DateTimePickerFormat.Short;
            dtpProgressDate.TabIndex = 22;
            // 
            // lblProgressNotes
            // 
            lblProgressNotes.AutoSize = true;
            lblProgressNotes.Location = new Point(227, 662);
            lblProgressNotes.Name = "lblProgressNotes";
            lblProgressNotes.Text = "Notes";
            // 
            // txtProgressNotes
            // 
            txtProgressNotes.Location = new Point(227, 680);
            txtProgressNotes.Name = "txtProgressNotes";
            txtProgressNotes.Size = new Size(300, 23);
            txtProgressNotes.TabIndex = 23;
            // 
            // btnAttachProgressFile
            // 
            btnAttachProgressFile.Location = new Point(537, 678);
            btnAttachProgressFile.Name = "btnAttachProgressFile";
            btnAttachProgressFile.Size = new Size(130, 28);
            btnAttachProgressFile.Text = "Attach Photo / File";
            btnAttachProgressFile.TabIndex = 24;
            btnAttachProgressFile.Click += btnAttachProgressFile_Click;
            // 
            // btnAddProgressUpdate
            // 
            btnAddProgressUpdate.Location = new Point(673, 678);
            btnAddProgressUpdate.Name = "btnAddProgressUpdate";
            btnAddProgressUpdate.Size = new Size(110, 28);
            btnAddProgressUpdate.Text = "Add Update";
            btnAddProgressUpdate.TabIndex = 25;
            btnAddProgressUpdate.Click += btnAddProgressUpdate_Click;
            // 
            // lblProgressFile
            // 
            lblProgressFile.AutoSize = true;
            lblProgressFile.Location = new Point(537, 710);
            lblProgressFile.Name = "lblProgressFile";
            lblProgressFile.Text = "";
            // 
            // lblProgressMsg
            // 
            lblProgressMsg.AutoSize = true;
            lblProgressMsg.Location = new Point(12, 710);
            lblProgressMsg.Name = "lblProgressMsg";
            // 
            // dgvProgressHistory
            // 
            dgvProgressHistory.AllowUserToAddRows = false;
            dgvProgressHistory.AllowUserToDeleteRows = false;
            dgvProgressHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProgressHistory.Columns.AddRange(new DataGridViewColumn[] { colPDate, colPPct, colPNotes, colPFile });
            dgvProgressHistory.MultiSelect = false;
            dgvProgressHistory.ReadOnly = true;
            dgvProgressHistory.RowHeadersVisible = false;
            dgvProgressHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProgressHistory.Location = new Point(12, 734);
            dgvProgressHistory.Name = "dgvProgressHistory";
            dgvProgressHistory.Size = new Size(930, 120);
            dgvProgressHistory.TabIndex = 26;
            dgvProgressHistory.CellDoubleClick += dgvProgressHistory_CellDoubleClick;
            // 
            // colSub
            // 
            colSub.DataPropertyName = "Subcontractor";
            colSub.HeaderText = "Subcontractor";
            colSub.Name = "colSub";
            // 
            // colProject
            // 
            colProject.DataPropertyName = "Project";
            colProject.HeaderText = "Project";
            colProject.Name = "colProject";
            // 
            // colScope
            // 
            colScope.DataPropertyName = "Scope";
            colScope.HeaderText = "Scope";
            colScope.Name = "colScope";
            // 
            // colAmount
            // 
            colAmount.DataPropertyName = "ContractAmount";
            colAmount.HeaderText = "Contract Amount";
            colAmount.Name = "colAmount";
            colAmount.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            // 
            // colDone
            // 
            colDone.DataPropertyName = "PercentDone";
            colDone.HeaderText = "% Done";
            colDone.Name = "colDone";
            colDone.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "0.##'%'" };
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            // 
            // colStageName
            // 
            colStageName.DataPropertyName = "Name";
            colStageName.HeaderText = "Stage Name";
            colStageName.Name = "colStageName";
            // 
            // colStagePct
            // 
            colStagePct.DataPropertyName = "Percent";
            colStagePct.HeaderText = "%";
            colStagePct.Name = "colStagePct";
            colStagePct.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "0.##'%'" };
            // 
            // colStageAmount
            // 
            colStageAmount.DataPropertyName = "Amount";
            colStageAmount.HeaderText = "Amount";
            colStageAmount.Name = "colStageAmount";
            colStageAmount.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" };
            colStageAmount.ReadOnly = true;
            // 
            // colDocFile
            // 
            colDocFile.DataPropertyName = "OriginalName";
            colDocFile.HeaderText = "File";
            colDocFile.Name = "colDocFile";
            // 
            // colDocUploaded
            // 
            colDocUploaded.DataPropertyName = "UploadedAt";
            colDocUploaded.HeaderText = "Uploaded";
            colDocUploaded.Name = "colDocUploaded";
            colDocUploaded.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy h:mm tt" };
            // 
            // colPDate
            // 
            colPDate.DataPropertyName = "Date";
            colPDate.HeaderText = "Date";
            colPDate.Name = "colPDate";
            colPDate.DefaultCellStyle = new DataGridViewCellStyle { Format = "MMM d, yyyy" };
            // 
            // colPPct
            // 
            colPPct.DataPropertyName = "Percent";
            colPPct.HeaderText = "%";
            colPPct.Name = "colPPct";
            colPPct.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "0.##'%'" };
            // 
            // colPNotes
            // 
            colPNotes.DataPropertyName = "Notes";
            colPNotes.HeaderText = "Notes";
            colPNotes.Name = "colPNotes";
            // 
            // colPFile
            // 
            colPFile.DataPropertyName = "FileName";
            colPFile.HeaderText = "Files";
            colPFile.Name = "colPFile";
            // 
            // SubJobsTab
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblProjectFilter);
            Controls.Add(cboJobProjectFilter);
            Controls.Add(lblSubFilter);
            Controls.Add(cboJobSubFilter);
            Controls.Add(dgvJobs);
            Controls.Add(btnNewJob);
            Controls.Add(btnDeleteJob);
            Controls.Add(lblFormMode);
            Controls.Add(lblJobSubcontractor);
            Controls.Add(cboJobSubcontractor);
            Controls.Add(lblJobProject);
            Controls.Add(cboJobProject);
            Controls.Add(lblJobScope);
            Controls.Add(txtJobScope);
            Controls.Add(lblJobAmount);
            Controls.Add(numJobAmount);
            Controls.Add(lblRetentionPct);
            Controls.Add(numRetentionPct);
            Controls.Add(lblJobStart);
            Controls.Add(dtpJobStart);
            Controls.Add(lblJobEnd);
            Controls.Add(dtpJobEnd);
            Controls.Add(lblJobMsg);
            Controls.Add(btnSaveJob);
            Controls.Add(btnCancelJob);
            Controls.Add(lblStages);
            Controls.Add(dgvJobStages);
            Controls.Add(lblStagesTotal);
            Controls.Add(btnAddStage);
            Controls.Add(btnRemoveStage);
            Controls.Add(lblJobDocs);
            Controls.Add(dgvJobDocs);
            Controls.Add(btnUploadJobDoc);
            Controls.Add(btnViewJobDoc);
            Controls.Add(lblProgressTitle);
            Controls.Add(prgJobProgress);
            Controls.Add(lblProgressPct);
            Controls.Add(lblProgressPctInput);
            Controls.Add(numProgressPct);
            Controls.Add(lblProgressDate);
            Controls.Add(dtpProgressDate);
            Controls.Add(lblProgressNotes);
            Controls.Add(txtProgressNotes);
            Controls.Add(btnAttachProgressFile);
            Controls.Add(btnAddProgressUpdate);
            Controls.Add(lblProgressFile);
            Controls.Add(lblProgressMsg);
            Controls.Add(dgvProgressHistory);
            Name = "SubJobsTab";
            Size = new Size(960, 870);
            Load += SubJobsTab_Load;
            ((System.ComponentModel.ISupportInitialize)dgvJobs).EndInit();
            ((System.ComponentModel.ISupportInitialize)numJobAmount).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRetentionPct).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvJobStages).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvJobDocs).EndInit();
            ((System.ComponentModel.ISupportInitialize)numProgressPct).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProgressHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProjectFilter;
        private ComboBox cboJobProjectFilter;
        private Label lblSubFilter;
        private ComboBox cboJobSubFilter;
        private DataGridView dgvJobs;
        private Button btnNewJob;
        private Button btnDeleteJob;
        private Label lblFormMode;
        private Label lblJobSubcontractor;
        private ComboBox cboJobSubcontractor;
        private Label lblJobProject;
        private ComboBox cboJobProject;
        private Label lblJobScope;
        private TextBox txtJobScope;
        private Label lblJobAmount;
        private NumericUpDown numJobAmount;
        private Label lblRetentionPct;
        private NumericUpDown numRetentionPct;
        private Label lblJobStart;
        private DateTimePicker dtpJobStart;
        private Label lblJobEnd;
        private DateTimePicker dtpJobEnd;
        private Label lblJobMsg;
        private Button btnSaveJob;
        private Button btnCancelJob;
        private Label lblStages;
        private DataGridView dgvJobStages;
        private Label lblStagesTotal;
        private Button btnAddStage;
        private Button btnRemoveStage;
        private Label lblJobDocs;
        private DataGridView dgvJobDocs;
        private Button btnUploadJobDoc;
        private Button btnViewJobDoc;
        private Label lblProgressTitle;
        private ProgressBar prgJobProgress;
        private Label lblProgressPct;
        private Label lblProgressPctInput;
        private NumericUpDown numProgressPct;
        private Label lblProgressDate;
        private DateTimePicker dtpProgressDate;
        private Label lblProgressNotes;
        private TextBox txtProgressNotes;
        private Button btnAttachProgressFile;
        private Button btnAddProgressUpdate;
        private Label lblProgressFile;
        private Label lblProgressMsg;
        private DataGridView dgvProgressHistory;
        private DataGridViewTextBoxColumn colSub;
        private DataGridViewTextBoxColumn colProject;
        private DataGridViewTextBoxColumn colScope;
        private DataGridViewTextBoxColumn colAmount;
        private DataGridViewTextBoxColumn colDone;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colStageName;
        private DataGridViewTextBoxColumn colStagePct;
        private DataGridViewTextBoxColumn colStageAmount;
        private DataGridViewTextBoxColumn colDocFile;
        private DataGridViewTextBoxColumn colDocUploaded;
        private DataGridViewTextBoxColumn colPDate;
        private DataGridViewTextBoxColumn colPPct;
        private DataGridViewTextBoxColumn colPNotes;
        private DataGridViewTextBoxColumn colPFile;
    }
}
