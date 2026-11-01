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
            cardProjectInfo = new Company.OfficeApp.UI.Card();
            pnlProjectsFooter = new Panel();
            pnlGap2 = new Panel();
            cardPhotos = new Company.OfficeApp.UI.Card();
            pnlGap1 = new Panel();
            cardProjectsList = new Company.OfficeApp.UI.Card();
            lblProjectsTitle = new Label();
            lblProjectsHint = new Label();
            btnAddWebProject = new Button();
            dgvWebProjects = new DataGridView();
            btnMoveProjectUp = new Button();
            btnMoveProjectDown = new Button();
            btnHideShowProject = new Button();
            btnDeleteWebProject = new Button();
            lblPhotosTitle = new Label();
            lblPhotos = new Label();
            dzPhotos = new Company.OfficeApp.UI.DropZone();
            prgUpload = new ProgressBar();
            lblUploadStatus = new Label();
            lblShowcaseTitle = new Label();
            lblShowcaseHint = new Label();
            dgvPhotos = new DataGridView();
            picPhotoPreview = new PictureBox();
            btnToggleShowcase = new Button();
            btnPhotoUp = new Button();
            btnPhotoDown = new Button();
            btnRemovePhoto = new Button();
            lblSlotTitle = new Label();
            lblProjectInfo = new Label();
            lblProjectDesc = new Label();
            lblProjectDescCount = new Label();
            frmProjectDesc = new Company.OfficeApp.UI.InputFrame();
            txtProjectDesc = new TextBox();
            cardProjectsSave = new Company.OfficeApp.UI.Card();
            lblWebProjectMsg = new Label();
            btnDiscardWebProject = new Button();
            btnSaveWebProject = new Button();
            cardProjectsList.Controls.Add(lblProjectsTitle);
            cardProjectsList.Controls.Add(lblProjectsHint);
            cardProjectsList.Controls.Add(btnAddWebProject);
            cardProjectsList.Controls.Add(dgvWebProjects);
            cardProjectsList.Controls.Add(btnMoveProjectUp);
            cardProjectsList.Controls.Add(btnMoveProjectDown);
            cardProjectsList.Controls.Add(btnHideShowProject);
            cardProjectsList.Controls.Add(btnDeleteWebProject);
            cardPhotos.Controls.Add(lblPhotosTitle);
            cardPhotos.Controls.Add(lblPhotos);
            cardPhotos.Controls.Add(dzPhotos);
            cardPhotos.Controls.Add(prgUpload);
            cardPhotos.Controls.Add(lblUploadStatus);
            cardPhotos.Controls.Add(lblShowcaseTitle);
            cardPhotos.Controls.Add(lblShowcaseHint);
            cardPhotos.Controls.Add(dgvPhotos);
            cardPhotos.Controls.Add(picPhotoPreview);
            cardPhotos.Controls.Add(btnToggleShowcase);
            cardPhotos.Controls.Add(btnPhotoUp);
            cardPhotos.Controls.Add(btnPhotoDown);
            cardPhotos.Controls.Add(btnRemovePhoto);
            cardProjectInfo.Controls.Add(lblSlotTitle);
            cardProjectInfo.Controls.Add(lblProjectInfo);
            cardProjectInfo.Controls.Add(lblProjectDesc);
            cardProjectInfo.Controls.Add(lblProjectDescCount);
            cardProjectInfo.Controls.Add(frmProjectDesc);
            frmProjectDesc.Controls.Add(txtProjectDesc);
            pnlProjectsFooter.Controls.Add(cardProjectsSave);
            cardProjectsSave.Controls.Add(lblWebProjectMsg);
            cardProjectsSave.Controls.Add(btnDiscardWebProject);
            cardProjectsSave.Controls.Add(btnSaveWebProject);
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
            // cardProjectInfo
            // 
            cardProjectInfo.Location = new Point(24, 698);
            cardProjectInfo.Name = "cardProjectInfo";
            cardProjectInfo.Size = new Size(1192, 170);
            cardProjectInfo.Dock = DockStyle.Fill;
            cardProjectInfo.Padding = new Padding(24);
            cardProjectInfo.TabIndex = 0;
            // 
            // pnlProjectsFooter
            // 
            pnlProjectsFooter.Location = new Point(24, 888);
            pnlProjectsFooter.Name = "pnlProjectsFooter";
            pnlProjectsFooter.Size = new Size(1192, 92);
            pnlProjectsFooter.Dock = DockStyle.Bottom;
            pnlProjectsFooter.Padding = new Padding(0, 20, 0, 0);
            pnlProjectsFooter.TabIndex = 1;
            // 
            // pnlGap2
            // 
            pnlGap2.Location = new Point(24, 678);
            pnlGap2.Name = "pnlGap2";
            pnlGap2.Size = new Size(1192, 20);
            pnlGap2.Dock = DockStyle.Top;
            pnlGap2.TabIndex = 2;
            // 
            // cardPhotos
            // 
            cardPhotos.Location = new Point(24, 340);
            cardPhotos.Name = "cardPhotos";
            cardPhotos.Size = new Size(1192, 338);
            cardPhotos.Dock = DockStyle.Top;
            cardPhotos.Padding = new Padding(24);
            cardPhotos.TabIndex = 3;
            // 
            // pnlGap1
            // 
            pnlGap1.Location = new Point(24, 320);
            pnlGap1.Name = "pnlGap1";
            pnlGap1.Size = new Size(1192, 20);
            pnlGap1.Dock = DockStyle.Top;
            pnlGap1.TabIndex = 4;
            // 
            // cardProjectsList
            // 
            cardProjectsList.Location = new Point(24, 8);
            cardProjectsList.Name = "cardProjectsList";
            cardProjectsList.Size = new Size(1192, 312);
            cardProjectsList.Dock = DockStyle.Top;
            cardProjectsList.Padding = new Padding(24);
            cardProjectsList.TabIndex = 5;
            // 
            // lblProjectsTitle
            // 
            lblProjectsTitle.AutoSize = true;
            lblProjectsTitle.Location = new Point(24, 18);
            lblProjectsTitle.Name = "lblProjectsTitle";
            lblProjectsTitle.Font = new Font("Segoe UI", 14F);
            lblProjectsTitle.Text = "Featured Projects Slots";
            // 
            // lblProjectsHint
            // 
            lblProjectsHint.AutoSize = true;
            lblProjectsHint.Location = new Point(24, 48);
            lblProjectsHint.Name = "lblProjectsHint";
            lblProjectsHint.Text = "From the office Projects list (ticked \"Show on website\"). The website shows them in this order.";
            // 
            // btnAddWebProject
            // 
            btnAddWebProject.Location = new Point(978, 18);
            btnAddWebProject.Name = "btnAddWebProject";
            btnAddWebProject.Size = new Size(190, 40);
            btnAddWebProject.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddWebProject.Font = new Font("Segoe UI", 10F);
            btnAddWebProject.Text = "Add New Project";
            btnAddWebProject.TabIndex = 6;
            btnAddWebProject.Click += btnAddWebProject_Click;
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
            dgvWebProjects.Location = new Point(24, 78);
            dgvWebProjects.Name = "dgvWebProjects";
            dgvWebProjects.Size = new Size(1144, 160);
            dgvWebProjects.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvWebProjects.TabIndex = 7;
            dgvWebProjects.SelectionChanged += dgvWebProjects_SelectionChanged;
            // 
            // btnMoveProjectUp
            // 
            btnMoveProjectUp.Location = new Point(24, 252);
            btnMoveProjectUp.Name = "btnMoveProjectUp";
            btnMoveProjectUp.Size = new Size(110, 36);
            btnMoveProjectUp.Text = "Move Up";
            btnMoveProjectUp.TabIndex = 8;
            btnMoveProjectUp.Click += btnMoveProjectUp_Click;
            // 
            // btnMoveProjectDown
            // 
            btnMoveProjectDown.Location = new Point(142, 252);
            btnMoveProjectDown.Name = "btnMoveProjectDown";
            btnMoveProjectDown.Size = new Size(110, 36);
            btnMoveProjectDown.Text = "Move Down";
            btnMoveProjectDown.TabIndex = 9;
            btnMoveProjectDown.Click += btnMoveProjectDown_Click;
            // 
            // btnHideShowProject
            // 
            btnHideShowProject.Location = new Point(260, 252);
            btnHideShowProject.Name = "btnHideShowProject";
            btnHideShowProject.Size = new Size(130, 36);
            btnHideShowProject.Text = "Unpublish";
            btnHideShowProject.TabIndex = 10;
            btnHideShowProject.Click += btnHideShowProject_Click;
            // 
            // btnDeleteWebProject
            // 
            btnDeleteWebProject.Location = new Point(398, 252);
            btnDeleteWebProject.Name = "btnDeleteWebProject";
            btnDeleteWebProject.Size = new Size(110, 36);
            btnDeleteWebProject.Text = "Delete";
            btnDeleteWebProject.TabIndex = 11;
            btnDeleteWebProject.Click += btnDeleteWebProject_Click;
            // 
            // lblPhotosTitle
            // 
            lblPhotosTitle.AutoSize = true;
            lblPhotosTitle.Location = new Point(24, 18);
            lblPhotosTitle.Name = "lblPhotosTitle";
            lblPhotosTitle.Font = new Font("Segoe UI", 14F);
            lblPhotosTitle.Text = "Project Images";
            // 
            // lblPhotos
            // 
            lblPhotos.AutoSize = true;
            lblPhotos.Location = new Point(24, 48);
            lblPhotos.Name = "lblPhotos";
            lblPhotos.Text = "Up to 10 photos, JPEG or PNG, 8 MB each.";
            // 
            // dzPhotos
            // 
            dzPhotos.Location = new Point(24, 78);
            dzPhotos.Name = "dzPhotos";
            dzPhotos.Size = new Size(430, 200);
            dzPhotos.TabIndex = 12;
            // 
            // prgUpload
            // 
            prgUpload.Location = new Point(24, 286);
            prgUpload.Name = "prgUpload";
            prgUpload.Size = new Size(430, 8);
            prgUpload.Visible = false;
            prgUpload.TabIndex = 13;
            // 
            // lblUploadStatus
            // 
            lblUploadStatus.Location = new Point(24, 298);
            lblUploadStatus.Name = "lblUploadStatus";
            lblUploadStatus.Size = new Size(430, 32);
            // 
            // lblShowcaseTitle
            // 
            lblShowcaseTitle.AutoSize = true;
            lblShowcaseTitle.Location = new Point(478, 18);
            lblShowcaseTitle.Name = "lblShowcaseTitle";
            lblShowcaseTitle.Font = new Font("Segoe UI", 14F);
            lblShowcaseTitle.Text = "Showcase Images";
            // 
            // lblShowcaseHint
            // 
            lblShowcaseHint.AutoSize = true;
            lblShowcaseHint.Location = new Point(478, 48);
            lblShowcaseHint.Name = "lblShowcaseHint";
            lblShowcaseHint.Text = "Select and arrange the images shown in the project carousel.";
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
            dgvPhotos.Location = new Point(478, 78);
            dgvPhotos.Name = "dgvPhotos";
            dgvPhotos.Size = new Size(400, 236);
            dgvPhotos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvPhotos.TabIndex = 14;
            dgvPhotos.SelectionChanged += dgvPhotos_SelectionChanged;
            // 
            // picPhotoPreview
            // 
            picPhotoPreview.Location = new Point(898, 78);
            picPhotoPreview.Name = "picPhotoPreview";
            picPhotoPreview.Size = new Size(270, 150);
            picPhotoPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picPhotoPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPhotoPreview.TabIndex = 15;
            // 
            // btnToggleShowcase
            // 
            btnToggleShowcase.Location = new Point(898, 236);
            btnToggleShowcase.Name = "btnToggleShowcase";
            btnToggleShowcase.Size = new Size(270, 36);
            btnToggleShowcase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnToggleShowcase.Text = "Add to Showcase";
            btnToggleShowcase.TabIndex = 16;
            btnToggleShowcase.Click += btnToggleShowcase_Click;
            // 
            // btnPhotoUp
            // 
            btnPhotoUp.Location = new Point(898, 278);
            btnPhotoUp.Name = "btnPhotoUp";
            btnPhotoUp.Size = new Size(80, 36);
            btnPhotoUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPhotoUp.Text = "Up";
            btnPhotoUp.TabIndex = 17;
            btnPhotoUp.Click += btnPhotoUp_Click;
            // 
            // btnPhotoDown
            // 
            btnPhotoDown.Location = new Point(984, 278);
            btnPhotoDown.Name = "btnPhotoDown";
            btnPhotoDown.Size = new Size(86, 36);
            btnPhotoDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPhotoDown.Text = "Down";
            btnPhotoDown.TabIndex = 18;
            btnPhotoDown.Click += btnPhotoDown_Click;
            // 
            // btnRemovePhoto
            // 
            btnRemovePhoto.Location = new Point(1076, 278);
            btnRemovePhoto.Name = "btnRemovePhoto";
            btnRemovePhoto.Size = new Size(92, 36);
            btnRemovePhoto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRemovePhoto.Text = "Remove";
            btnRemovePhoto.TabIndex = 19;
            btnRemovePhoto.Click += btnRemovePhoto_Click;
            // 
            // lblSlotTitle
            // 
            lblSlotTitle.AutoSize = true;
            lblSlotTitle.Location = new Point(24, 18);
            lblSlotTitle.Name = "lblSlotTitle";
            lblSlotTitle.Font = new Font("Segoe UI", 14F);
            lblSlotTitle.Text = "Project Information";
            // 
            // lblProjectInfo
            // 
            lblProjectInfo.AutoSize = true;
            lblProjectInfo.Location = new Point(24, 50);
            lblProjectInfo.Name = "lblProjectInfo";
            lblProjectInfo.Text = "Click a project in the list to edit it.";
            // 
            // lblProjectDesc
            // 
            lblProjectDesc.AutoSize = true;
            lblProjectDesc.Location = new Point(24, 80);
            lblProjectDesc.Name = "lblProjectDesc";
            lblProjectDesc.Font = new Font("Segoe UI Semibold", 10F);
            lblProjectDesc.Text = "Project Short Description";
            // 
            // lblProjectDescCount
            // 
            lblProjectDescCount.Location = new Point(1068, 80);
            lblProjectDescCount.Name = "lblProjectDescCount";
            lblProjectDescCount.Size = new Size(100, 20);
            lblProjectDescCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblProjectDescCount.TextAlign = ContentAlignment.MiddleRight;
            lblProjectDescCount.Text = "0/100";
            // 
            // frmProjectDesc
            // 
            frmProjectDesc.Location = new Point(24, 104);
            frmProjectDesc.Name = "frmProjectDesc";
            frmProjectDesc.Size = new Size(1144, 44);
            frmProjectDesc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            frmProjectDesc.TabIndex = 20;
            // 
            // txtProjectDesc
            // 
            txtProjectDesc.Location = new Point(12, 11);
            txtProjectDesc.Name = "txtProjectDesc";
            txtProjectDesc.Size = new Size(1120, 23);
            txtProjectDesc.BorderStyle = BorderStyle.None;
            txtProjectDesc.Font = new Font("Segoe UI", 10.5F);
            txtProjectDesc.TabIndex = 21;
            txtProjectDesc.TextChanged += txtProjectDesc_TextChanged;
            // 
            // cardProjectsSave
            // 
            cardProjectsSave.Location = new Point(0, 20);
            cardProjectsSave.Name = "cardProjectsSave";
            cardProjectsSave.Size = new Size(1192, 72);
            cardProjectsSave.Dock = DockStyle.Fill;
            cardProjectsSave.Padding = new Padding(16);
            cardProjectsSave.TabIndex = 22;
            // 
            // lblWebProjectMsg
            // 
            lblWebProjectMsg.Location = new Point(24, 16);
            lblWebProjectMsg.Name = "lblWebProjectMsg";
            lblWebProjectMsg.Size = new Size(800, 40);
            lblWebProjectMsg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblWebProjectMsg.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnDiscardWebProject
            // 
            btnDiscardWebProject.Location = new Point(866, 16);
            btnDiscardWebProject.Name = "btnDiscardWebProject";
            btnDiscardWebProject.Size = new Size(150, 40);
            btnDiscardWebProject.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDiscardWebProject.Font = new Font("Segoe UI", 10F);
            btnDiscardWebProject.Text = "Discard Changes";
            btnDiscardWebProject.TabIndex = 23;
            btnDiscardWebProject.Click += btnDiscardWebProject_Click;
            // 
            // btnSaveWebProject
            // 
            btnSaveWebProject.Location = new Point(1026, 16);
            btnSaveWebProject.Name = "btnSaveWebProject";
            btnSaveWebProject.Size = new Size(150, 40);
            btnSaveWebProject.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveWebProject.Font = new Font("Segoe UI", 10F);
            btnSaveWebProject.Text = "Save Changes";
            btnSaveWebProject.TabIndex = 24;
            btnSaveWebProject.Click += btnSaveWebProject_Click;
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
            Controls.Add(cardProjectInfo);
            Controls.Add(pnlProjectsFooter);
            Controls.Add(pnlGap2);
            Controls.Add(cardPhotos);
            Controls.Add(pnlGap1);
            Controls.Add(cardProjectsList);
            Name = "ProjectsEditorPage";
            Size = new Size(1240, 1000);
            Load += ProjectsEditorPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvWebProjects).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPhotos).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPhotoPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Company.OfficeApp.UI.Card cardProjectInfo;
        private Panel pnlProjectsFooter;
        private Panel pnlGap2;
        private Company.OfficeApp.UI.Card cardPhotos;
        private Panel pnlGap1;
        private Company.OfficeApp.UI.Card cardProjectsList;
        private Label lblProjectsTitle;
        private Label lblProjectsHint;
        private Button btnAddWebProject;
        private DataGridView dgvWebProjects;
        private Button btnMoveProjectUp;
        private Button btnMoveProjectDown;
        private Button btnHideShowProject;
        private Button btnDeleteWebProject;
        private Label lblPhotosTitle;
        private Label lblPhotos;
        private Company.OfficeApp.UI.DropZone dzPhotos;
        private ProgressBar prgUpload;
        private Label lblUploadStatus;
        private Label lblShowcaseTitle;
        private Label lblShowcaseHint;
        private DataGridView dgvPhotos;
        private PictureBox picPhotoPreview;
        private Button btnToggleShowcase;
        private Button btnPhotoUp;
        private Button btnPhotoDown;
        private Button btnRemovePhoto;
        private Label lblSlotTitle;
        private Label lblProjectInfo;
        private Label lblProjectDesc;
        private Label lblProjectDescCount;
        private Company.OfficeApp.UI.InputFrame frmProjectDesc;
        private TextBox txtProjectDesc;
        private Company.OfficeApp.UI.Card cardProjectsSave;
        private Label lblWebProjectMsg;
        private Button btnDiscardWebProject;
        private Button btnSaveWebProject;
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
