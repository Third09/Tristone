namespace Company.WebsiteAdmin.Pages
{
    partial class ProjectsEditorPage
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
            lblProjectsHint = new Label();
            dgvWebProjects = new DataGridView();
            btnAddWebProject = new Button();
            btnMoveProjectUp = new Button();
            btnMoveProjectDown = new Button();
            btnHideShowProject = new Button();
            btnDeleteWebProject = new Button();
            lblSlotTitle = new Label();
            lblProjectInfo = new Label();
            lblProjectDesc = new Label();
            lblProjectDescCount = new Label();
            txtProjectDesc = new TextBox();
            btnSaveWebProject = new Button();
            btnDiscardWebProject = new Button();
            lblWebProjectMsg = new Label();
            lblPhotos = new Label();
            dgvPhotos = new DataGridView();
            picPhotoPreview = new PictureBox();
            btnAddPhotos = new Button();
            btnRemovePhoto = new Button();
            btnPhotoUp = new Button();
            btnPhotoDown = new Button();
            btnToggleShowcase = new Button();
            prgUpload = new ProgressBar();
            lblUploadStatus = new Label();
            colSlot = new DataGridViewTextBoxColumn();
            colProject = new DataGridViewTextBoxColumn();
            colLocation = new DataGridViewTextBoxColumn();
            colSector = new DataGridViewTextBoxColumn();
            colShown = new DataGridViewTextBoxColumn();
            colPhotoCount = new DataGridViewTextBoxColumn();
            colPhotoOrder = new DataGridViewTextBoxColumn();
            colPhotoFile = new DataGridViewTextBoxColumn();
            colPhotoSize = new DataGridViewTextBoxColumn();
            colPhotoShowcase = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvWebProjects).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPhotos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPhotoPreview).BeginInit();
            SuspendLayout();
            // 
            // lblProjectsHint
            // 
            lblProjectsHint.AutoSize = true;
            lblProjectsHint.Location = new Point(12, 12);
            lblProjectsHint.Name = "lblProjectsHint";
            lblProjectsHint.Text = "Featured projects come from the office Projects list (ticked \"Show on website\"). The website shows them in this order.";
            // 
            // dgvWebProjects
            // 
            dgvWebProjects.AllowUserToAddRows = false;
            dgvWebProjects.AllowUserToDeleteRows = false;
            dgvWebProjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWebProjects.Columns.AddRange(new DataGridViewColumn[] { colSlot, colProject, colLocation, colSector, colShown, colPhotoCount });
            dgvWebProjects.MultiSelect = false;
            dgvWebProjects.ReadOnly = true;
            dgvWebProjects.RowHeadersVisible = false;
            dgvWebProjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWebProjects.Location = new Point(12, 36);
            dgvWebProjects.Name = "dgvWebProjects";
            dgvWebProjects.Size = new Size(930, 170);
            dgvWebProjects.TabIndex = 0;
            dgvWebProjects.SelectionChanged += dgvWebProjects_SelectionChanged;
            // 
            // btnAddWebProject
            // 
            btnAddWebProject.Location = new Point(12, 214);
            btnAddWebProject.Name = "btnAddWebProject";
            btnAddWebProject.Size = new Size(130, 28);
            btnAddWebProject.Text = "Add New Project";
            btnAddWebProject.TabIndex = 1;
            btnAddWebProject.Click += btnAddWebProject_Click;
            // 
            // btnMoveProjectUp
            // 
            btnMoveProjectUp.Location = new Point(148, 214);
            btnMoveProjectUp.Name = "btnMoveProjectUp";
            btnMoveProjectUp.Size = new Size(80, 28);
            btnMoveProjectUp.Text = "Move Up";
            btnMoveProjectUp.TabIndex = 2;
            btnMoveProjectUp.Click += btnMoveProjectUp_Click;
            // 
            // btnMoveProjectDown
            // 
            btnMoveProjectDown.Location = new Point(234, 214);
            btnMoveProjectDown.Name = "btnMoveProjectDown";
            btnMoveProjectDown.Size = new Size(90, 28);
            btnMoveProjectDown.Text = "Move Down";
            btnMoveProjectDown.TabIndex = 3;
            btnMoveProjectDown.Click += btnMoveProjectDown_Click;
            // 
            // btnHideShowProject
            // 
            btnHideShowProject.Location = new Point(330, 214);
            btnHideShowProject.Name = "btnHideShowProject";
            btnHideShowProject.Size = new Size(110, 28);
            btnHideShowProject.Text = "Unpublish";
            btnHideShowProject.TabIndex = 4;
            btnHideShowProject.Click += btnHideShowProject_Click;
            // 
            // btnDeleteWebProject
            // 
            btnDeleteWebProject.Location = new Point(446, 214);
            btnDeleteWebProject.Name = "btnDeleteWebProject";
            btnDeleteWebProject.Size = new Size(90, 28);
            btnDeleteWebProject.Text = "Delete";
            btnDeleteWebProject.TabIndex = 5;
            btnDeleteWebProject.Click += btnDeleteWebProject_Click;
            // 
            // lblSlotTitle
            // 
            lblSlotTitle.AutoSize = true;
            lblSlotTitle.Location = new Point(12, 256);
            lblSlotTitle.Name = "lblSlotTitle";
            lblSlotTitle.Text = "Click a project in the list to edit it.";
            // 
            // lblProjectInfo
            // 
            lblProjectInfo.AutoSize = true;
            lblProjectInfo.Location = new Point(12, 278);
            lblProjectInfo.Name = "lblProjectInfo";
            lblProjectInfo.Text = "Location: -    Category: -";
            // 
            // lblProjectDesc
            // 
            lblProjectDesc.AutoSize = true;
            lblProjectDesc.Location = new Point(12, 302);
            lblProjectDesc.Name = "lblProjectDesc";
            lblProjectDesc.Text = "Project short description";
            // 
            // lblProjectDescCount
            // 
            lblProjectDescCount.AutoSize = true;
            lblProjectDescCount.Location = new Point(380, 302);
            lblProjectDescCount.Name = "lblProjectDescCount";
            lblProjectDescCount.Text = "0/100";
            // 
            // txtProjectDesc
            // 
            txtProjectDesc.Location = new Point(12, 320);
            txtProjectDesc.Name = "txtProjectDesc";
            txtProjectDesc.Size = new Size(420, 23);
            txtProjectDesc.TabIndex = 6;
            txtProjectDesc.TextChanged += txtProjectDesc_TextChanged;
            // 
            // btnSaveWebProject
            // 
            btnSaveWebProject.Location = new Point(12, 352);
            btnSaveWebProject.Name = "btnSaveWebProject";
            btnSaveWebProject.Size = new Size(120, 28);
            btnSaveWebProject.Text = "Save Changes";
            btnSaveWebProject.TabIndex = 7;
            btnSaveWebProject.Click += btnSaveWebProject_Click;
            // 
            // btnDiscardWebProject
            // 
            btnDiscardWebProject.Location = new Point(138, 352);
            btnDiscardWebProject.Name = "btnDiscardWebProject";
            btnDiscardWebProject.Size = new Size(130, 28);
            btnDiscardWebProject.Text = "Discard Changes";
            btnDiscardWebProject.TabIndex = 8;
            btnDiscardWebProject.Click += btnDiscardWebProject_Click;
            // 
            // lblWebProjectMsg
            // 
            lblWebProjectMsg.AutoSize = true;
            lblWebProjectMsg.Location = new Point(12, 388);
            lblWebProjectMsg.Name = "lblWebProjectMsg";
            lblWebProjectMsg.MaximumSize = new Size(420, 0);
            // 
            // lblPhotos
            // 
            lblPhotos.AutoSize = true;
            lblPhotos.Location = new Point(460, 256);
            lblPhotos.Name = "lblPhotos";
            lblPhotos.Text = "Project photos (up to 10, JPEG or PNG, 8 MB each). Showcase = shown in the website carousel.";
            // 
            // dgvPhotos
            // 
            dgvPhotos.AllowUserToAddRows = false;
            dgvPhotos.AllowUserToDeleteRows = false;
            dgvPhotos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhotos.Columns.AddRange(new DataGridViewColumn[] { colPhotoOrder, colPhotoFile, colPhotoSize, colPhotoShowcase });
            dgvPhotos.MultiSelect = false;
            dgvPhotos.ReadOnly = true;
            dgvPhotos.RowHeadersVisible = false;
            dgvPhotos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhotos.Location = new Point(460, 278);
            dgvPhotos.Name = "dgvPhotos";
            dgvPhotos.Size = new Size(300, 200);
            dgvPhotos.TabIndex = 9;
            dgvPhotos.SelectionChanged += dgvPhotos_SelectionChanged;
            // 
            // picPhotoPreview
            // 
            picPhotoPreview.Location = new Point(772, 278);
            picPhotoPreview.Name = "picPhotoPreview";
            picPhotoPreview.Size = new Size(170, 128);
            picPhotoPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPhotoPreview.BorderStyle = BorderStyle.FixedSingle;
            picPhotoPreview.TabIndex = 10;
            // 
            // btnAddPhotos
            // 
            btnAddPhotos.Location = new Point(460, 486);
            btnAddPhotos.Name = "btnAddPhotos";
            btnAddPhotos.Size = new Size(100, 28);
            btnAddPhotos.Text = "Add Photos...";
            btnAddPhotos.TabIndex = 11;
            btnAddPhotos.Click += btnAddPhotos_Click;
            // 
            // btnRemovePhoto
            // 
            btnRemovePhoto.Location = new Point(566, 486);
            btnRemovePhoto.Name = "btnRemovePhoto";
            btnRemovePhoto.Size = new Size(80, 28);
            btnRemovePhoto.Text = "Remove";
            btnRemovePhoto.TabIndex = 12;
            btnRemovePhoto.Click += btnRemovePhoto_Click;
            // 
            // btnPhotoUp
            // 
            btnPhotoUp.Location = new Point(652, 486);
            btnPhotoUp.Name = "btnPhotoUp";
            btnPhotoUp.Size = new Size(50, 28);
            btnPhotoUp.Text = "Up";
            btnPhotoUp.TabIndex = 13;
            btnPhotoUp.Click += btnPhotoUp_Click;
            // 
            // btnPhotoDown
            // 
            btnPhotoDown.Location = new Point(708, 486);
            btnPhotoDown.Name = "btnPhotoDown";
            btnPhotoDown.Size = new Size(52, 28);
            btnPhotoDown.Text = "Down";
            btnPhotoDown.TabIndex = 14;
            btnPhotoDown.Click += btnPhotoDown_Click;
            // 
            // btnToggleShowcase
            // 
            btnToggleShowcase.Location = new Point(772, 412);
            btnToggleShowcase.Name = "btnToggleShowcase";
            btnToggleShowcase.Size = new Size(170, 28);
            btnToggleShowcase.Text = "Showcase: Yes / No";
            btnToggleShowcase.TabIndex = 15;
            btnToggleShowcase.Click += btnToggleShowcase_Click;
            // 
            // prgUpload
            // 
            prgUpload.Location = new Point(460, 522);
            prgUpload.Name = "prgUpload";
            prgUpload.Size = new Size(300, 18);
            prgUpload.Visible = false;
            prgUpload.TabIndex = 16;
            // 
            // lblUploadStatus
            // 
            lblUploadStatus.AutoSize = true;
            lblUploadStatus.Location = new Point(460, 546);
            lblUploadStatus.Name = "lblUploadStatus";
            lblUploadStatus.MaximumSize = new Size(480, 0);
            // 
            // colSlot
            // 
            colSlot.DataPropertyName = "Slot";
            colSlot.HeaderText = "Slot";
            colSlot.Name = "colSlot";
            colSlot.FillWeight = 30;
            // 
            // colProject
            // 
            colProject.DataPropertyName = "ProjectName";
            colProject.HeaderText = "Project";
            colProject.Name = "colProject";
            // 
            // colLocation
            // 
            colLocation.DataPropertyName = "Location";
            colLocation.HeaderText = "Location";
            colLocation.Name = "colLocation";
            // 
            // colSector
            // 
            colSector.DataPropertyName = "Sector";
            colSector.HeaderText = "Category";
            colSector.Name = "colSector";
            // 
            // colShown
            // 
            colShown.DataPropertyName = "Shown";
            colShown.HeaderText = "On Website";
            colShown.Name = "colShown";
            colShown.FillWeight = 50;
            // 
            // colPhotoCount
            // 
            colPhotoCount.DataPropertyName = "Photos";
            colPhotoCount.HeaderText = "Photos";
            colPhotoCount.Name = "colPhotoCount";
            colPhotoCount.FillWeight = 40;
            // 
            // colPhotoOrder
            // 
            colPhotoOrder.DataPropertyName = "Order";
            colPhotoOrder.HeaderText = "#";
            colPhotoOrder.Name = "colPhotoOrder";
            colPhotoOrder.FillWeight = 20;
            // 
            // colPhotoFile
            // 
            colPhotoFile.DataPropertyName = "FileName";
            colPhotoFile.HeaderText = "File";
            colPhotoFile.Name = "colPhotoFile";
            // 
            // colPhotoSize
            // 
            colPhotoSize.DataPropertyName = "Size";
            colPhotoSize.HeaderText = "Size";
            colPhotoSize.Name = "colPhotoSize";
            colPhotoSize.FillWeight = 40;
            // 
            // colPhotoShowcase
            // 
            colPhotoShowcase.DataPropertyName = "Showcase";
            colPhotoShowcase.HeaderText = "Showcase";
            colPhotoShowcase.Name = "colPhotoShowcase";
            colPhotoShowcase.FillWeight = 45;
            // 
            // ProjectsEditorPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblProjectsHint);
            Controls.Add(dgvWebProjects);
            Controls.Add(btnAddWebProject);
            Controls.Add(btnMoveProjectUp);
            Controls.Add(btnMoveProjectDown);
            Controls.Add(btnHideShowProject);
            Controls.Add(btnDeleteWebProject);
            Controls.Add(lblSlotTitle);
            Controls.Add(lblProjectInfo);
            Controls.Add(lblProjectDesc);
            Controls.Add(lblProjectDescCount);
            Controls.Add(txtProjectDesc);
            Controls.Add(btnSaveWebProject);
            Controls.Add(btnDiscardWebProject);
            Controls.Add(lblWebProjectMsg);
            Controls.Add(lblPhotos);
            Controls.Add(dgvPhotos);
            Controls.Add(picPhotoPreview);
            Controls.Add(btnAddPhotos);
            Controls.Add(btnRemovePhoto);
            Controls.Add(btnPhotoUp);
            Controls.Add(btnPhotoDown);
            Controls.Add(btnToggleShowcase);
            Controls.Add(prgUpload);
            Controls.Add(lblUploadStatus);
            Name = "ProjectsEditorPage";
            Size = new Size(960, 720);
            Load += ProjectsEditorPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvWebProjects).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPhotos).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPhotoPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProjectsHint;
        private DataGridView dgvWebProjects;
        private Button btnAddWebProject;
        private Button btnMoveProjectUp;
        private Button btnMoveProjectDown;
        private Button btnHideShowProject;
        private Button btnDeleteWebProject;
        private Label lblSlotTitle;
        private Label lblProjectInfo;
        private Label lblProjectDesc;
        private Label lblProjectDescCount;
        private TextBox txtProjectDesc;
        private Button btnSaveWebProject;
        private Button btnDiscardWebProject;
        private Label lblWebProjectMsg;
        private Label lblPhotos;
        private DataGridView dgvPhotos;
        private PictureBox picPhotoPreview;
        private Button btnAddPhotos;
        private Button btnRemovePhoto;
        private Button btnPhotoUp;
        private Button btnPhotoDown;
        private Button btnToggleShowcase;
        private ProgressBar prgUpload;
        private Label lblUploadStatus;
        private DataGridViewTextBoxColumn colSlot;
        private DataGridViewTextBoxColumn colProject;
        private DataGridViewTextBoxColumn colLocation;
        private DataGridViewTextBoxColumn colSector;
        private DataGridViewTextBoxColumn colShown;
        private DataGridViewTextBoxColumn colPhotoCount;
        private DataGridViewTextBoxColumn colPhotoOrder;
        private DataGridViewTextBoxColumn colPhotoFile;
        private DataGridViewTextBoxColumn colPhotoSize;
        private DataGridViewTextBoxColumn colPhotoShowcase;
    }
}
