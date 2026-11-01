namespace Company.WebsiteAdmin.Pages
{
    partial class ServicesEditorPage
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
            lblServicesHint = new Label();
            dgvWebServices = new DataGridView();
            btnAddWebService = new Button();
            btnMoveServiceUp = new Button();
            btnMoveServiceDown = new Button();
            btnHideShowService = new Button();
            btnDeleteWebService = new Button();
            lblFormMode = new Label();
            lblServiceName = new Label();
            cboServiceName = new ComboBox();
            lblServiceSector = new Label();
            cboServiceSector = new ComboBox();
            lblServiceLocation = new Label();
            txtServiceLocation = new TextBox();
            lblServiceDesc = new Label();
            lblServiceDescCount = new Label();
            txtServiceDesc = new TextBox();
            lblServiceImage = new Label();
            picServiceImage = new PictureBox();
            btnChooseServiceImage = new Button();
            lblServiceImageName = new Label();
            lblServiceMsg = new Label();
            btnSaveWebService = new Button();
            btnCancelWebService = new Button();
            colSlot = new DataGridViewTextBoxColumn();
            colServiceName = new DataGridViewTextBoxColumn();
            colSector = new DataGridViewTextBoxColumn();
            colLocation = new DataGridViewTextBoxColumn();
            colShown = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvWebServices).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picServiceImage).BeginInit();
            SuspendLayout();
            // 
            // lblServicesHint
            // 
            lblServicesHint.AutoSize = true;
            lblServicesHint.Location = new Point(12, 12);
            lblServicesHint.Name = "lblServicesHint";
            lblServicesHint.Text = "The service cards on the website, in this order. Each card's icon is fixed by its service.";
            // 
            // dgvWebServices
            // 
            dgvWebServices.AllowUserToAddRows = false;
            dgvWebServices.AllowUserToDeleteRows = false;
            dgvWebServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWebServices.Columns.AddRange(new DataGridViewColumn[] { colSlot, colServiceName, colSector, colLocation, colShown });
            dgvWebServices.MultiSelect = false;
            dgvWebServices.ReadOnly = true;
            dgvWebServices.RowHeadersVisible = false;
            dgvWebServices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWebServices.Location = new Point(12, 36);
            dgvWebServices.Name = "dgvWebServices";
            dgvWebServices.Size = new Size(930, 160);
            dgvWebServices.TabIndex = 0;
            dgvWebServices.SelectionChanged += dgvWebServices_SelectionChanged;
            // 
            // btnAddWebService
            // 
            btnAddWebService.Location = new Point(12, 204);
            btnAddWebService.Name = "btnAddWebService";
            btnAddWebService.Size = new Size(130, 28);
            btnAddWebService.Text = "Add New Service";
            btnAddWebService.TabIndex = 1;
            btnAddWebService.Click += btnAddWebService_Click;
            // 
            // btnMoveServiceUp
            // 
            btnMoveServiceUp.Location = new Point(148, 204);
            btnMoveServiceUp.Name = "btnMoveServiceUp";
            btnMoveServiceUp.Size = new Size(80, 28);
            btnMoveServiceUp.Text = "Move Up";
            btnMoveServiceUp.TabIndex = 2;
            btnMoveServiceUp.Click += btnMoveServiceUp_Click;
            // 
            // btnMoveServiceDown
            // 
            btnMoveServiceDown.Location = new Point(234, 204);
            btnMoveServiceDown.Name = "btnMoveServiceDown";
            btnMoveServiceDown.Size = new Size(90, 28);
            btnMoveServiceDown.Text = "Move Down";
            btnMoveServiceDown.TabIndex = 3;
            btnMoveServiceDown.Click += btnMoveServiceDown_Click;
            // 
            // btnHideShowService
            // 
            btnHideShowService.Location = new Point(330, 204);
            btnHideShowService.Name = "btnHideShowService";
            btnHideShowService.Size = new Size(110, 28);
            btnHideShowService.Text = "Unpublish";
            btnHideShowService.TabIndex = 4;
            btnHideShowService.Click += btnHideShowService_Click;
            // 
            // btnDeleteWebService
            // 
            btnDeleteWebService.Location = new Point(446, 204);
            btnDeleteWebService.Name = "btnDeleteWebService";
            btnDeleteWebService.Size = new Size(90, 28);
            btnDeleteWebService.Text = "Delete";
            btnDeleteWebService.TabIndex = 5;
            btnDeleteWebService.Click += btnDeleteWebService_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(12, 248);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Text = "New service";
            // 
            // lblServiceName
            // 
            lblServiceName.AutoSize = true;
            lblServiceName.Location = new Point(12, 272);
            lblServiceName.Name = "lblServiceName";
            lblServiceName.Text = "Name of service";
            // 
            // cboServiceName
            // 
            cboServiceName.Location = new Point(12, 290);
            cboServiceName.Name = "cboServiceName";
            cboServiceName.Size = new Size(300, 23);
            cboServiceName.DropDownStyle = ComboBoxStyle.DropDownList;
            cboServiceName.TabIndex = 6;
            // 
            // lblServiceSector
            // 
            lblServiceSector.AutoSize = true;
            lblServiceSector.Location = new Point(12, 320);
            lblServiceSector.Name = "lblServiceSector";
            lblServiceSector.Text = "Category";
            // 
            // cboServiceSector
            // 
            cboServiceSector.Location = new Point(12, 338);
            cboServiceSector.Name = "cboServiceSector";
            cboServiceSector.Size = new Size(300, 23);
            cboServiceSector.DropDownStyle = ComboBoxStyle.DropDownList;
            cboServiceSector.TabIndex = 7;
            // 
            // lblServiceLocation
            // 
            lblServiceLocation.AutoSize = true;
            lblServiceLocation.Location = new Point(12, 368);
            lblServiceLocation.Name = "lblServiceLocation";
            lblServiceLocation.Text = "Location (e.g. San Juan City)";
            // 
            // txtServiceLocation
            // 
            txtServiceLocation.Location = new Point(12, 386);
            txtServiceLocation.Name = "txtServiceLocation";
            txtServiceLocation.Size = new Size(300, 23);
            txtServiceLocation.TabIndex = 8;
            // 
            // lblServiceDesc
            // 
            lblServiceDesc.AutoSize = true;
            lblServiceDesc.Location = new Point(12, 416);
            lblServiceDesc.Name = "lblServiceDesc";
            lblServiceDesc.Text = "Short description";
            // 
            // lblServiceDescCount
            // 
            lblServiceDescCount.AutoSize = true;
            lblServiceDescCount.Location = new Point(360, 416);
            lblServiceDescCount.Name = "lblServiceDescCount";
            lblServiceDescCount.Text = "0/100";
            // 
            // txtServiceDesc
            // 
            txtServiceDesc.Location = new Point(12, 434);
            txtServiceDesc.Name = "txtServiceDesc";
            txtServiceDesc.Size = new Size(400, 23);
            txtServiceDesc.TabIndex = 9;
            txtServiceDesc.TextChanged += txtServiceDesc_TextChanged;
            // 
            // lblServiceImage
            // 
            lblServiceImage.AutoSize = true;
            lblServiceImage.Location = new Point(440, 272);
            lblServiceImage.Name = "lblServiceImage";
            lblServiceImage.Text = "Service image (JPEG or PNG, 8 MB at most)";
            // 
            // picServiceImage
            // 
            picServiceImage.Location = new Point(440, 290);
            picServiceImage.Name = "picServiceImage";
            picServiceImage.Size = new Size(320, 180);
            picServiceImage.SizeMode = PictureBoxSizeMode.Zoom;
            picServiceImage.BorderStyle = BorderStyle.FixedSingle;
            picServiceImage.TabIndex = 10;
            // 
            // btnChooseServiceImage
            // 
            btnChooseServiceImage.Location = new Point(772, 290);
            btnChooseServiceImage.Name = "btnChooseServiceImage";
            btnChooseServiceImage.Size = new Size(130, 28);
            btnChooseServiceImage.Text = "Choose Picture...";
            btnChooseServiceImage.TabIndex = 11;
            btnChooseServiceImage.Click += btnChooseServiceImage_Click;
            // 
            // lblServiceImageName
            // 
            lblServiceImageName.AutoSize = true;
            lblServiceImageName.Location = new Point(772, 326);
            lblServiceImageName.Name = "lblServiceImageName";
            lblServiceImageName.MaximumSize = new Size(170, 0);
            // 
            // lblServiceMsg
            // 
            lblServiceMsg.AutoSize = true;
            lblServiceMsg.Location = new Point(12, 480);
            lblServiceMsg.Name = "lblServiceMsg";
            lblServiceMsg.MaximumSize = new Size(740, 0);
            // 
            // btnSaveWebService
            // 
            btnSaveWebService.Location = new Point(12, 504);
            btnSaveWebService.Name = "btnSaveWebService";
            btnSaveWebService.Size = new Size(120, 28);
            btnSaveWebService.Text = "Save Changes";
            btnSaveWebService.TabIndex = 12;
            btnSaveWebService.Click += btnSaveWebService_Click;
            // 
            // btnCancelWebService
            // 
            btnCancelWebService.Location = new Point(138, 504);
            btnCancelWebService.Name = "btnCancelWebService";
            btnCancelWebService.Size = new Size(130, 28);
            btnCancelWebService.Text = "Discard Changes";
            btnCancelWebService.TabIndex = 13;
            btnCancelWebService.Click += btnCancelWebService_Click;
            // 
            // colSlot
            // 
            colSlot.DataPropertyName = "Slot";
            colSlot.HeaderText = "Slot";
            colSlot.Name = "colSlot";
            colSlot.FillWeight = 30;
            // 
            // colServiceName
            // 
            colServiceName.DataPropertyName = "ServiceName";
            colServiceName.HeaderText = "Service";
            colServiceName.Name = "colServiceName";
            // 
            // colSector
            // 
            colSector.DataPropertyName = "Sector";
            colSector.HeaderText = "Category";
            colSector.Name = "colSector";
            // 
            // colLocation
            // 
            colLocation.DataPropertyName = "Location";
            colLocation.HeaderText = "Location";
            colLocation.Name = "colLocation";
            // 
            // colShown
            // 
            colShown.DataPropertyName = "Shown";
            colShown.HeaderText = "On Website";
            colShown.Name = "colShown";
            colShown.FillWeight = 50;
            // 
            // ServicesEditorPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblServicesHint);
            Controls.Add(dgvWebServices);
            Controls.Add(btnAddWebService);
            Controls.Add(btnMoveServiceUp);
            Controls.Add(btnMoveServiceDown);
            Controls.Add(btnHideShowService);
            Controls.Add(btnDeleteWebService);
            Controls.Add(lblFormMode);
            Controls.Add(lblServiceName);
            Controls.Add(cboServiceName);
            Controls.Add(lblServiceSector);
            Controls.Add(cboServiceSector);
            Controls.Add(lblServiceLocation);
            Controls.Add(txtServiceLocation);
            Controls.Add(lblServiceDesc);
            Controls.Add(lblServiceDescCount);
            Controls.Add(txtServiceDesc);
            Controls.Add(lblServiceImage);
            Controls.Add(picServiceImage);
            Controls.Add(btnChooseServiceImage);
            Controls.Add(lblServiceImageName);
            Controls.Add(lblServiceMsg);
            Controls.Add(btnSaveWebService);
            Controls.Add(btnCancelWebService);
            Name = "ServicesEditorPage";
            Size = new Size(960, 640);
            Load += ServicesEditorPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvWebServices).EndInit();
            ((System.ComponentModel.ISupportInitialize)picServiceImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblServicesHint;
        private DataGridView dgvWebServices;
        private Button btnAddWebService;
        private Button btnMoveServiceUp;
        private Button btnMoveServiceDown;
        private Button btnHideShowService;
        private Button btnDeleteWebService;
        private Label lblFormMode;
        private Label lblServiceName;
        private ComboBox cboServiceName;
        private Label lblServiceSector;
        private ComboBox cboServiceSector;
        private Label lblServiceLocation;
        private TextBox txtServiceLocation;
        private Label lblServiceDesc;
        private Label lblServiceDescCount;
        private TextBox txtServiceDesc;
        private Label lblServiceImage;
        private PictureBox picServiceImage;
        private Button btnChooseServiceImage;
        private Label lblServiceImageName;
        private Label lblServiceMsg;
        private Button btnSaveWebService;
        private Button btnCancelWebService;
        private DataGridViewTextBoxColumn colSlot;
        private DataGridViewTextBoxColumn colServiceName;
        private DataGridViewTextBoxColumn colSector;
        private DataGridViewTextBoxColumn colLocation;
        private DataGridViewTextBoxColumn colShown;
    }
}
