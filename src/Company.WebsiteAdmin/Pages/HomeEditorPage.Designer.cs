namespace Company.WebsiteAdmin.Pages
{
    partial class HomeEditorPage
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
            lblHomeHint = new Label();
            lblHeroImage = new Label();
            picHeroImage = new PictureBox();
            btnChooseHeroImage = new Button();
            btnRemoveHeroImage = new Button();
            lblHeroImageName = new Label();
            lblHeadline = new Label();
            lblHeadlineCount = new Label();
            txtHeadline = new TextBox();
            lblSubheadline = new Label();
            lblSubheadlineCount = new Label();
            txtSubheadline = new TextBox();
            lblHomeStatus = new Label();
            lblHomeLastSaved = new Label();
            btnSaveHome = new Button();
            btnDiscardHome = new Button();
            ((System.ComponentModel.ISupportInitialize)picHeroImage).BeginInit();
            SuspendLayout();
            // 
            // lblHomeHint
            // 
            lblHomeHint.AutoSize = true;
            lblHomeHint.Location = new Point(12, 12);
            lblHomeHint.Name = "lblHomeHint";
            lblHomeHint.Text = "Manage the Homepage's content displayed on the live website. Save Changes keeps your edits; Publish Website (top) puts them on the live site.";
            // 
            // lblHeroImage
            // 
            lblHeroImage.AutoSize = true;
            lblHeroImage.Location = new Point(12, 44);
            lblHeroImage.Name = "lblHeroImage";
            lblHeroImage.Text = "Hero background image (JPEG or PNG, 8 MB at most)";
            // 
            // picHeroImage
            // 
            picHeroImage.Location = new Point(12, 64);
            picHeroImage.Name = "picHeroImage";
            picHeroImage.Size = new Size(480, 270);
            picHeroImage.SizeMode = PictureBoxSizeMode.Zoom;
            picHeroImage.BorderStyle = BorderStyle.FixedSingle;
            picHeroImage.TabIndex = 0;
            // 
            // btnChooseHeroImage
            // 
            btnChooseHeroImage.Location = new Point(504, 64);
            btnChooseHeroImage.Name = "btnChooseHeroImage";
            btnChooseHeroImage.Size = new Size(130, 28);
            btnChooseHeroImage.Text = "Choose Picture...";
            btnChooseHeroImage.TabIndex = 1;
            btnChooseHeroImage.Click += btnChooseHeroImage_Click;
            // 
            // btnRemoveHeroImage
            // 
            btnRemoveHeroImage.Location = new Point(504, 98);
            btnRemoveHeroImage.Name = "btnRemoveHeroImage";
            btnRemoveHeroImage.Size = new Size(130, 28);
            btnRemoveHeroImage.Text = "Remove Picture";
            btnRemoveHeroImage.TabIndex = 2;
            btnRemoveHeroImage.Click += btnRemoveHeroImage_Click;
            // 
            // lblHeroImageName
            // 
            lblHeroImageName.AutoSize = true;
            lblHeroImageName.Location = new Point(504, 134);
            lblHeroImageName.Name = "lblHeroImageName";
            lblHeroImageName.MaximumSize = new Size(300, 0);
            // 
            // lblHeadline
            // 
            lblHeadline.AutoSize = true;
            lblHeadline.Location = new Point(12, 346);
            lblHeadline.Name = "lblHeadline";
            lblHeadline.Text = "Main headline";
            // 
            // lblHeadlineCount
            // 
            lblHeadlineCount.AutoSize = true;
            lblHeadlineCount.Location = new Point(560, 346);
            lblHeadlineCount.Name = "lblHeadlineCount";
            lblHeadlineCount.Text = "0/150";
            // 
            // txtHeadline
            // 
            txtHeadline.Location = new Point(12, 364);
            txtHeadline.Name = "txtHeadline";
            txtHeadline.Size = new Size(600, 23);
            txtHeadline.TabIndex = 3;
            txtHeadline.TextChanged += Field_Changed;
            // 
            // lblSubheadline
            // 
            lblSubheadline.AutoSize = true;
            lblSubheadline.Location = new Point(12, 396);
            lblSubheadline.Name = "lblSubheadline";
            lblSubheadline.Text = "Subheadline";
            // 
            // lblSubheadlineCount
            // 
            lblSubheadlineCount.AutoSize = true;
            lblSubheadlineCount.Location = new Point(560, 396);
            lblSubheadlineCount.Name = "lblSubheadlineCount";
            lblSubheadlineCount.Text = "0/300";
            // 
            // txtSubheadline
            // 
            txtSubheadline.Location = new Point(12, 414);
            txtSubheadline.Name = "txtSubheadline";
            txtSubheadline.Size = new Size(600, 70);
            txtSubheadline.Multiline = true;
            txtSubheadline.ScrollBars = ScrollBars.Vertical;
            txtSubheadline.TabIndex = 4;
            txtSubheadline.TextChanged += Field_Changed;
            // 
            // lblHomeStatus
            // 
            lblHomeStatus.AutoSize = true;
            lblHomeStatus.Location = new Point(12, 494);
            lblHomeStatus.Name = "lblHomeStatus";
            lblHomeStatus.MaximumSize = new Size(600, 0);
            // 
            // lblHomeLastSaved
            // 
            lblHomeLastSaved.AutoSize = true;
            lblHomeLastSaved.Location = new Point(12, 540);
            lblHomeLastSaved.Name = "lblHomeLastSaved";
            lblHomeLastSaved.Text = "Last saved: -";
            // 
            // btnSaveHome
            // 
            btnSaveHome.Location = new Point(12, 562);
            btnSaveHome.Name = "btnSaveHome";
            btnSaveHome.Size = new Size(120, 28);
            btnSaveHome.Text = "Save Changes";
            btnSaveHome.TabIndex = 5;
            btnSaveHome.Click += btnSaveHome_Click;
            // 
            // btnDiscardHome
            // 
            btnDiscardHome.Location = new Point(138, 562);
            btnDiscardHome.Name = "btnDiscardHome";
            btnDiscardHome.Size = new Size(130, 28);
            btnDiscardHome.Text = "Discard Changes";
            btnDiscardHome.TabIndex = 6;
            btnDiscardHome.Click += btnDiscardHome_Click;
            // 
            // HomeEditorPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblHomeHint);
            Controls.Add(lblHeroImage);
            Controls.Add(picHeroImage);
            Controls.Add(btnChooseHeroImage);
            Controls.Add(btnRemoveHeroImage);
            Controls.Add(lblHeroImageName);
            Controls.Add(lblHeadline);
            Controls.Add(lblHeadlineCount);
            Controls.Add(txtHeadline);
            Controls.Add(lblSubheadline);
            Controls.Add(lblSubheadlineCount);
            Controls.Add(txtSubheadline);
            Controls.Add(lblHomeStatus);
            Controls.Add(lblHomeLastSaved);
            Controls.Add(btnSaveHome);
            Controls.Add(btnDiscardHome);
            Name = "HomeEditorPage";
            Size = new Size(960, 640);
            Load += HomeEditorPage_Load;
            ((System.ComponentModel.ISupportInitialize)picHeroImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHomeHint;
        private Label lblHeroImage;
        private PictureBox picHeroImage;
        private Button btnChooseHeroImage;
        private Button btnRemoveHeroImage;
        private Label lblHeroImageName;
        private Label lblHeadline;
        private Label lblHeadlineCount;
        private TextBox txtHeadline;
        private Label lblSubheadline;
        private Label lblSubheadlineCount;
        private TextBox txtSubheadline;
        private Label lblHomeStatus;
        private Label lblHomeLastSaved;
        private Button btnSaveHome;
        private Button btnDiscardHome;
    }
}
