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
            cardServiceForm = new Company.OfficeApp.UI.Card();
            pnlServicesSide = new Panel();
            pnlServicesFooter = new Panel();
            pnlGap1 = new Panel();
            cardServicesList = new Company.OfficeApp.UI.Card();
            lblServicesTitle = new Label();
            lblServicesHint = new Label();
            btnAddWebService = new Button();
            dgvWebServices = new DataGridView();
            btnMoveServiceUp = new Button();
            btnMoveServiceDown = new Button();
            btnHideShowService = new Button();
            btnDeleteWebService = new Button();
            btnDuplicateService = new Button();
            lblFormMode = new Label();
            dzService = new Company.OfficeApp.UI.DropZone();
            lblServiceImageName = new Label();
            lblServiceInfo = new Label();
            lblServiceName = new Label();
            cboServiceName = new ComboBox();
            lblServiceLocation = new Label();
            frmServiceLocation = new Company.OfficeApp.UI.InputFrame();
            txtServiceLocation = new TextBox();
            lblServiceDesc = new Label();
            lblServiceDescCount = new Label();
            frmServiceDesc = new Company.OfficeApp.UI.InputFrame();
            txtServiceDesc = new TextBox();
            lblServiceIcon = new Label();
            cboServiceIcon = new ComboBox();
            statusService = new Company.OfficeApp.UI.StatusBox();
            pnlSideGap = new Panel();
            cardServicePreview = new Company.OfficeApp.UI.Card();
            lblServicePreviewTitle = new Label();
            pnlServicePreview = new Panel();
            cardServicesSave = new Company.OfficeApp.UI.Card();
            lblServiceMsg = new Label();
            btnCancelWebService = new Button();
            btnSaveWebService = new Button();
            cardServicesList.Controls.Add(lblServicesTitle);
            cardServicesList.Controls.Add(lblServicesHint);
            cardServicesList.Controls.Add(btnAddWebService);
            cardServicesList.Controls.Add(dgvWebServices);
            cardServicesList.Controls.Add(btnMoveServiceUp);
            cardServicesList.Controls.Add(btnMoveServiceDown);
            cardServicesList.Controls.Add(btnHideShowService);
            cardServicesList.Controls.Add(btnDeleteWebService);
            cardServicesList.Controls.Add(btnDuplicateService);
            cardServiceForm.Controls.Add(lblFormMode);
            cardServiceForm.Controls.Add(dzService);
            cardServiceForm.Controls.Add(lblServiceImageName);
            cardServiceForm.Controls.Add(lblServiceInfo);
            cardServiceForm.Controls.Add(lblServiceName);
            cardServiceForm.Controls.Add(cboServiceName);
            cardServiceForm.Controls.Add(lblServiceLocation);
            cardServiceForm.Controls.Add(frmServiceLocation);
            frmServiceLocation.Controls.Add(txtServiceLocation);
            cardServiceForm.Controls.Add(lblServiceDesc);
            cardServiceForm.Controls.Add(lblServiceDescCount);
            cardServiceForm.Controls.Add(frmServiceDesc);
            frmServiceDesc.Controls.Add(txtServiceDesc);
            cardServiceForm.Controls.Add(lblServiceIcon);
            cardServiceForm.Controls.Add(cboServiceIcon);
            pnlServicesSide.Controls.Add(statusService);
            pnlServicesSide.Controls.Add(pnlSideGap);
            pnlServicesSide.Controls.Add(cardServicePreview);
            cardServicePreview.Controls.Add(lblServicePreviewTitle);
            cardServicePreview.Controls.Add(pnlServicePreview);
            pnlServicesFooter.Controls.Add(cardServicesSave);
            cardServicesSave.Controls.Add(lblServiceMsg);
            cardServicesSave.Controls.Add(btnCancelWebService);
            cardServicesSave.Controls.Add(btnSaveWebService);
            colSlot = new DataGridViewTextBoxColumn();
            colServiceName = new DataGridViewTextBoxColumn();
            colLocation = new DataGridViewTextBoxColumn();
            colShown = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvWebServices).BeginInit();
            SuspendLayout();
            // 
            // cardServiceForm
            // 
            cardServiceForm.Location = new Point(24, 328);
            cardServiceForm.Name = "cardServiceForm";
            cardServiceForm.Size = new Size(752, 520);
            cardServiceForm.Dock = DockStyle.Fill;
            cardServiceForm.Padding = new Padding(24);
            cardServiceForm.TabIndex = 0;
            // 
            // pnlServicesSide
            // 
            pnlServicesSide.Location = new Point(776, 328);
            pnlServicesSide.Name = "pnlServicesSide";
            pnlServicesSide.Size = new Size(440, 520);
            pnlServicesSide.Dock = DockStyle.Right;
            pnlServicesSide.Padding = new Padding(20, 0, 0, 0);
            pnlServicesSide.TabIndex = 1;
            // 
            // pnlServicesFooter
            // 
            pnlServicesFooter.Location = new Point(24, 848);
            pnlServicesFooter.Name = "pnlServicesFooter";
            pnlServicesFooter.Size = new Size(1192, 92);
            pnlServicesFooter.Dock = DockStyle.Bottom;
            pnlServicesFooter.Padding = new Padding(0, 20, 0, 0);
            pnlServicesFooter.TabIndex = 2;
            // 
            // pnlGap1
            // 
            pnlGap1.Location = new Point(24, 308);
            pnlGap1.Name = "pnlGap1";
            pnlGap1.Size = new Size(1192, 20);
            pnlGap1.Dock = DockStyle.Top;
            pnlGap1.TabIndex = 3;
            // 
            // cardServicesList
            // 
            cardServicesList.Location = new Point(24, 8);
            cardServicesList.Name = "cardServicesList";
            cardServicesList.Size = new Size(1192, 300);
            cardServicesList.Dock = DockStyle.Top;
            cardServicesList.Padding = new Padding(24);
            cardServicesList.TabIndex = 4;
            // 
            // lblServicesTitle
            // 
            lblServicesTitle.AutoSize = true;
            lblServicesTitle.Location = new Point(24, 18);
            lblServicesTitle.Name = "lblServicesTitle";
            lblServicesTitle.Font = new Font("Segoe UI", 14F);
            lblServicesTitle.Text = "Featured Services Slots";
            // 
            // lblServicesHint
            // 
            lblServicesHint.AutoSize = true;
            lblServicesHint.Location = new Point(24, 48);
            lblServicesHint.Name = "lblServicesHint";
            lblServicesHint.Text = "The service cards on the website, in this order. Each card has its own icon; one without a picture shows the website's picture for that icon.";
            // 
            // btnAddWebService
            // 
            btnAddWebService.Location = new Point(978, 18);
            btnAddWebService.Name = "btnAddWebService";
            btnAddWebService.Size = new Size(190, 40);
            btnAddWebService.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddWebService.Font = new Font("Segoe UI", 10F);
            btnAddWebService.Text = "Add New Service";
            btnAddWebService.TabIndex = 5;
            btnAddWebService.Click += btnAddWebService_Click;
            // 
            // dgvWebServices
            // 
            dgvWebServices.AllowUserToAddRows = false;
            dgvWebServices.AllowUserToDeleteRows = false;
            dgvWebServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWebServices.Columns.AddRange(new DataGridViewColumn[] { colSlot, colServiceName, colLocation, colShown });
            dgvWebServices.MultiSelect = false;
            dgvWebServices.ReadOnly = true;
            dgvWebServices.RowHeadersVisible = false;
            dgvWebServices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWebServices.Location = new Point(24, 78);
            dgvWebServices.Name = "dgvWebServices";
            dgvWebServices.Size = new Size(1144, 150);
            dgvWebServices.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvWebServices.TabIndex = 6;
            dgvWebServices.SelectionChanged += dgvWebServices_SelectionChanged;
            // 
            // btnMoveServiceUp
            // 
            btnMoveServiceUp.Location = new Point(24, 242);
            btnMoveServiceUp.Name = "btnMoveServiceUp";
            btnMoveServiceUp.Size = new Size(110, 36);
            btnMoveServiceUp.Text = "Move Up";
            btnMoveServiceUp.TabIndex = 7;
            btnMoveServiceUp.Click += btnMoveServiceUp_Click;
            // 
            // btnMoveServiceDown
            // 
            btnMoveServiceDown.Location = new Point(142, 242);
            btnMoveServiceDown.Name = "btnMoveServiceDown";
            btnMoveServiceDown.Size = new Size(110, 36);
            btnMoveServiceDown.Text = "Move Down";
            btnMoveServiceDown.TabIndex = 8;
            btnMoveServiceDown.Click += btnMoveServiceDown_Click;
            // 
            // btnHideShowService
            // 
            btnHideShowService.Location = new Point(260, 242);
            btnHideShowService.Name = "btnHideShowService";
            btnHideShowService.Size = new Size(130, 36);
            btnHideShowService.Text = "Unpublish";
            btnHideShowService.TabIndex = 9;
            btnHideShowService.Click += btnHideShowService_Click;
            // 
            // btnDeleteWebService
            // 
            btnDeleteWebService.Location = new Point(398, 242);
            btnDeleteWebService.Name = "btnDeleteWebService";
            btnDeleteWebService.Size = new Size(110, 36);
            btnDeleteWebService.Text = "Delete";
            btnDeleteWebService.TabIndex = 10;
            btnDeleteWebService.Click += btnDeleteWebService_Click;
            //
            // btnDuplicateService  (a hidden copy of the chosen card, right after it)
            //
            btnDuplicateService.Location = new Point(516, 242);
            btnDuplicateService.Name = "btnDuplicateService";
            btnDuplicateService.Size = new Size(120, 36);
            btnDuplicateService.TabIndex = 9;
            btnDuplicateService.Text = "Duplicate";
            btnDuplicateService.Click += btnDuplicateService_Click;
            // 
            // lblFormMode
            // 
            lblFormMode.AutoSize = true;
            lblFormMode.Location = new Point(24, 18);
            lblFormMode.Name = "lblFormMode";
            lblFormMode.Font = new Font("Segoe UI", 14F);
            lblFormMode.Text = "Service Image";
            // 
            // dzService
            // 
            dzService.Location = new Point(24, 60);
            dzService.Name = "dzService";
            dzService.Size = new Size(704, 180);
            dzService.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dzService.TabIndex = 11;
            // 
            // lblServiceImageName
            // 
            lblServiceImageName.AutoSize = true;
            lblServiceImageName.Location = new Point(24, 248);
            lblServiceImageName.Name = "lblServiceImageName";
            lblServiceImageName.MaximumSize = new Size(700, 0);
            // 
            // lblServiceInfo
            // 
            lblServiceInfo.AutoSize = true;
            lblServiceInfo.Location = new Point(24, 278);
            lblServiceInfo.Name = "lblServiceInfo";
            lblServiceInfo.Font = new Font("Segoe UI", 14F);
            lblServiceInfo.Text = "Service Information";
            // 
            // lblServiceName
            // 
            lblServiceName.AutoSize = true;
            lblServiceName.Location = new Point(24, 318);
            lblServiceName.Name = "lblServiceName";
            lblServiceName.Font = new Font("Segoe UI Semibold", 10F);
            lblServiceName.Text = "Name of Service";
            // 
            // cboServiceName
            // 
            cboServiceName.Location = new Point(24, 342);
            cboServiceName.Name = "cboServiceName";
            cboServiceName.Size = new Size(704, 27);
            cboServiceName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboServiceName.DropDownStyle = ComboBoxStyle.DropDown;
            cboServiceName.MaxLength = 60;
            cboServiceName.Font = new Font("Segoe UI", 10.5F);
            cboServiceName.TabIndex = 12;
            cboServiceName.TextChanged += Field_Changed;
            // 
            // lblServiceLocation
            // 
            lblServiceLocation.AutoSize = true;
            lblServiceLocation.Location = new Point(24, 384);
            lblServiceLocation.Name = "lblServiceLocation";
            lblServiceLocation.Font = new Font("Segoe UI Semibold", 10F);
            lblServiceLocation.Text = "Location (e.g. San Juan City)";
            // 
            // frmServiceLocation
            // 
            frmServiceLocation.Location = new Point(24, 408);
            frmServiceLocation.Name = "frmServiceLocation";
            frmServiceLocation.Size = new Size(704, 44);
            frmServiceLocation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            frmServiceLocation.TabIndex = 14;
            // 
            // txtServiceLocation
            // 
            txtServiceLocation.Location = new Point(12, 11);
            txtServiceLocation.Name = "txtServiceLocation";
            txtServiceLocation.Size = new Size(680, 23);
            txtServiceLocation.BorderStyle = BorderStyle.None;
            txtServiceLocation.Font = new Font("Segoe UI", 10.5F);
            txtServiceLocation.TabIndex = 15;
            txtServiceLocation.TextChanged += Field_Changed;
            // 
            // lblServiceDesc
            // 
            lblServiceDesc.AutoSize = true;
            lblServiceDesc.Location = new Point(24, 464);
            lblServiceDesc.Name = "lblServiceDesc";
            lblServiceDesc.Font = new Font("Segoe UI Semibold", 10F);
            lblServiceDesc.Text = "Short Description";
            // 
            // lblServiceDescCount
            // 
            lblServiceDescCount.Location = new Point(628, 464);
            lblServiceDescCount.Name = "lblServiceDescCount";
            lblServiceDescCount.Size = new Size(100, 20);
            lblServiceDescCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblServiceDescCount.TextAlign = ContentAlignment.MiddleRight;
            lblServiceDescCount.Text = "0/100";
            // 
            // frmServiceDesc
            // 
            frmServiceDesc.Location = new Point(24, 488);
            frmServiceDesc.Name = "frmServiceDesc";
            frmServiceDesc.Size = new Size(704, 44);
            frmServiceDesc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            frmServiceDesc.TabIndex = 16;
            // 
            // txtServiceDesc
            // 
            txtServiceDesc.Location = new Point(12, 11);
            txtServiceDesc.Name = "txtServiceDesc";
            txtServiceDesc.Size = new Size(680, 23);
            txtServiceDesc.BorderStyle = BorderStyle.None;
            txtServiceDesc.Font = new Font("Segoe UI", 10.5F);
            txtServiceDesc.TabIndex = 17;
            txtServiceDesc.TextChanged += Field_Changed;
            // 
            // lblServiceIcon
            // 
            lblServiceIcon.AutoSize = true;
            lblServiceIcon.Location = new Point(24, 544);
            lblServiceIcon.Name = "lblServiceIcon";
            lblServiceIcon.Font = new Font("Segoe UI Semibold", 10F);
            lblServiceIcon.Text = "Icon (also picks the website's picture when you add none)";
            // 
            // cboServiceIcon  (owner-drawn: icon + name)
            // 
            cboServiceIcon.Location = new Point(24, 568);
            cboServiceIcon.Name = "cboServiceIcon";
            cboServiceIcon.Size = new Size(340, 40);
            cboServiceIcon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboServiceIcon.DrawMode = DrawMode.OwnerDrawFixed;
            cboServiceIcon.ItemHeight = 34;
            cboServiceIcon.Font = new Font("Segoe UI", 10.5F);
            cboServiceIcon.TabIndex = 18;
            cboServiceIcon.SelectedIndexChanged += Field_Changed;
            // 
            // statusService
            // 
            statusService.Location = new Point(20, 346);
            statusService.Name = "statusService";
            statusService.Size = new Size(420, 78);
            statusService.Dock = DockStyle.Top;
            statusService.TabIndex = 18;
            // 
            // pnlSideGap
            // 
            pnlSideGap.Location = new Point(20, 330);
            pnlSideGap.Name = "pnlSideGap";
            pnlSideGap.Size = new Size(420, 16);
            pnlSideGap.Dock = DockStyle.Top;
            pnlSideGap.TabIndex = 19;
            // 
            // cardServicePreview
            // 
            cardServicePreview.Location = new Point(20, 0);
            cardServicePreview.Name = "cardServicePreview";
            cardServicePreview.Size = new Size(420, 330);
            cardServicePreview.Dock = DockStyle.Top;
            cardServicePreview.TabIndex = 20;
            // 
            // lblServicePreviewTitle
            // 
            lblServicePreviewTitle.AutoSize = true;
            lblServicePreviewTitle.Location = new Point(20, 18);
            lblServicePreviewTitle.Name = "lblServicePreviewTitle";
            lblServicePreviewTitle.Font = new Font("Segoe UI", 14F);
            lblServicePreviewTitle.Text = "Live Preview";
            // 
            // pnlServicePreview
            // 
            pnlServicePreview.Location = new Point(20, 60);
            pnlServicePreview.Name = "pnlServicePreview";
            pnlServicePreview.Size = new Size(380, 250);
            pnlServicePreview.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlServicePreview.TabIndex = 21;
            // 
            // cardServicesSave
            // 
            cardServicesSave.Location = new Point(0, 20);
            cardServicesSave.Name = "cardServicesSave";
            cardServicesSave.Size = new Size(1192, 72);
            cardServicesSave.Dock = DockStyle.Fill;
            cardServicesSave.Padding = new Padding(16);
            cardServicesSave.TabIndex = 22;
            // 
            // lblServiceMsg
            // 
            lblServiceMsg.Location = new Point(24, 16);
            lblServiceMsg.Name = "lblServiceMsg";
            lblServiceMsg.Size = new Size(800, 40);
            lblServiceMsg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblServiceMsg.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnCancelWebService
            // 
            btnCancelWebService.Location = new Point(866, 16);
            btnCancelWebService.Name = "btnCancelWebService";
            btnCancelWebService.Size = new Size(150, 40);
            btnCancelWebService.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelWebService.Font = new Font("Segoe UI", 10F);
            btnCancelWebService.Text = "Discard Changes";
            btnCancelWebService.TabIndex = 23;
            btnCancelWebService.Click += btnCancelWebService_Click;
            // 
            // btnSaveWebService
            // 
            btnSaveWebService.Location = new Point(1026, 16);
            btnSaveWebService.Name = "btnSaveWebService";
            btnSaveWebService.Size = new Size(150, 40);
            btnSaveWebService.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveWebService.Font = new Font("Segoe UI", 10F);
            btnSaveWebService.Text = "Save Changes";
            btnSaveWebService.TabIndex = 24;
            btnSaveWebService.Click += btnSaveWebService_Click;
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
            Controls.Add(cardServiceForm);
            Controls.Add(pnlServicesSide);
            Controls.Add(pnlServicesFooter);
            Controls.Add(pnlGap1);
            Controls.Add(cardServicesList);
            Name = "ServicesEditorPage";
            Size = new Size(1240, 960);
            Load += ServicesEditorPage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvWebServices).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Company.OfficeApp.UI.Card cardServiceForm;
        private Panel pnlServicesSide;
        private Panel pnlServicesFooter;
        private Panel pnlGap1;
        private Company.OfficeApp.UI.Card cardServicesList;
        private Label lblServicesTitle;
        private Label lblServicesHint;
        private Button btnAddWebService;
        private DataGridView dgvWebServices;
        private Button btnMoveServiceUp;
        private Button btnMoveServiceDown;
        private Button btnHideShowService;
        private Button btnDeleteWebService;
        private Button btnDuplicateService;
        private Label lblFormMode;
        private Company.OfficeApp.UI.DropZone dzService;
        private Label lblServiceImageName;
        private Label lblServiceInfo;
        private Label lblServiceName;
        private ComboBox cboServiceName;
        private Label lblServiceLocation;
        private Company.OfficeApp.UI.InputFrame frmServiceLocation;
        private TextBox txtServiceLocation;
        private Label lblServiceDesc;
        private Label lblServiceDescCount;
        private Company.OfficeApp.UI.InputFrame frmServiceDesc;
        private TextBox txtServiceDesc;
        private Label lblServiceIcon;
        private ComboBox cboServiceIcon;
        private Company.OfficeApp.UI.StatusBox statusService;
        private Panel pnlSideGap;
        private Company.OfficeApp.UI.Card cardServicePreview;
        private Label lblServicePreviewTitle;
        private Panel pnlServicePreview;
        private Company.OfficeApp.UI.Card cardServicesSave;
        private Label lblServiceMsg;
        private Button btnCancelWebService;
        private Button btnSaveWebService;
        private DataGridViewTextBoxColumn colSlot;
        private DataGridViewTextBoxColumn colServiceName;
        private DataGridViewTextBoxColumn colLocation;
        private DataGridViewTextBoxColumn colShown;
    }
}
