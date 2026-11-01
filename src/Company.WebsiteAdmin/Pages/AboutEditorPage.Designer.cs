namespace Company.WebsiteAdmin.Pages
{
    partial class AboutEditorPage
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
            lblAboutHint = new Label();
            btnAboutSection1 = new Button();
            btnAboutSection2 = new Button();
            btnAboutSection3 = new Button();
            lblAboutSection = new Label();
            lblAboutImage = new Label();
            picAboutImage = new PictureBox();
            btnChooseAboutImage = new Button();
            btnRemoveAboutImage = new Button();
            lblAboutImageName = new Label();
            lblAboutHeading = new Label();
            lblAboutHeadingCount = new Label();
            txtAboutHeading = new TextBox();
            lblAboutText1 = new Label();
            lblAboutText1Count = new Label();
            txtAboutText1 = new TextBox();
            lblAboutText2 = new Label();
            lblAboutText2Count = new Label();
            txtAboutText2 = new TextBox();
            lblAboutStatus = new Label();
            lblAboutLastSaved = new Label();
            btnSaveAbout = new Button();
            btnDiscardAbout = new Button();
            ((System.ComponentModel.ISupportInitialize)picAboutImage).BeginInit();
            SuspendLayout();
            // 
            // lblAboutHint
            // 
            lblAboutHint.AutoSize = true;
            lblAboutHint.Location = new Point(12, 12);
            lblAboutHint.Name = "lblAboutHint";
            lblAboutHint.Text = "Manage company information, principles, vision and mission displayed on the live website.";
            // 
            // btnAboutSection1
            // 
            btnAboutSection1.Location = new Point(12, 38);
            btnAboutSection1.Name = "btnAboutSection1";
            btnAboutSection1.Size = new Size(190, 28);
            btnAboutSection1.Text = "1. About Tri-Stone";
            btnAboutSection1.TabIndex = 0;
            btnAboutSection1.Click += btnAboutSection_Click;
            // 
            // btnAboutSection2
            // 
            btnAboutSection2.Location = new Point(208, 38);
            btnAboutSection2.Name = "btnAboutSection2";
            btnAboutSection2.Size = new Size(190, 28);
            btnAboutSection2.Text = "2. Our Principles";
            btnAboutSection2.TabIndex = 1;
            btnAboutSection2.Click += btnAboutSection_Click;
            // 
            // btnAboutSection3
            // 
            btnAboutSection3.Location = new Point(404, 38);
            btnAboutSection3.Name = "btnAboutSection3";
            btnAboutSection3.Size = new Size(190, 28);
            btnAboutSection3.Text = "3. Our Mission and Vision";
            btnAboutSection3.TabIndex = 2;
            btnAboutSection3.Click += btnAboutSection_Click;
            // 
            // lblAboutSection
            // 
            lblAboutSection.AutoSize = true;
            lblAboutSection.Location = new Point(12, 78);
            lblAboutSection.Name = "lblAboutSection";
            lblAboutSection.Text = "Section 1 of 3";
            // 
            // lblAboutImage
            // 
            lblAboutImage.AutoSize = true;
            lblAboutImage.Location = new Point(12, 100);
            lblAboutImage.Name = "lblAboutImage";
            lblAboutImage.Text = "Section image (JPEG or PNG, 8 MB at most)";
            // 
            // picAboutImage
            // 
            picAboutImage.Location = new Point(12, 120);
            picAboutImage.Name = "picAboutImage";
            picAboutImage.Size = new Size(400, 225);
            picAboutImage.SizeMode = PictureBoxSizeMode.Zoom;
            picAboutImage.BorderStyle = BorderStyle.FixedSingle;
            picAboutImage.TabIndex = 3;
            // 
            // btnChooseAboutImage
            // 
            btnChooseAboutImage.Location = new Point(424, 120);
            btnChooseAboutImage.Name = "btnChooseAboutImage";
            btnChooseAboutImage.Size = new Size(130, 28);
            btnChooseAboutImage.Text = "Choose Picture...";
            btnChooseAboutImage.TabIndex = 4;
            btnChooseAboutImage.Click += btnChooseAboutImage_Click;
            // 
            // btnRemoveAboutImage
            // 
            btnRemoveAboutImage.Location = new Point(424, 154);
            btnRemoveAboutImage.Name = "btnRemoveAboutImage";
            btnRemoveAboutImage.Size = new Size(130, 28);
            btnRemoveAboutImage.Text = "Remove Picture";
            btnRemoveAboutImage.TabIndex = 5;
            btnRemoveAboutImage.Click += btnRemoveAboutImage_Click;
            // 
            // lblAboutImageName
            // 
            lblAboutImageName.AutoSize = true;
            lblAboutImageName.Location = new Point(424, 190);
            lblAboutImageName.Name = "lblAboutImageName";
            lblAboutImageName.MaximumSize = new Size(300, 0);
            // 
            // lblAboutHeading
            // 
            lblAboutHeading.AutoSize = true;
            lblAboutHeading.Location = new Point(12, 356);
            lblAboutHeading.Name = "lblAboutHeading";
            lblAboutHeading.Text = "Section heading";
            // 
            // lblAboutHeadingCount
            // 
            lblAboutHeadingCount.AutoSize = true;
            lblAboutHeadingCount.Location = new Point(560, 356);
            lblAboutHeadingCount.Name = "lblAboutHeadingCount";
            lblAboutHeadingCount.Text = "0/100";
            // 
            // txtAboutHeading
            // 
            txtAboutHeading.Location = new Point(12, 374);
            txtAboutHeading.Name = "txtAboutHeading";
            txtAboutHeading.Size = new Size(600, 23);
            txtAboutHeading.TabIndex = 6;
            txtAboutHeading.TextChanged += Field_Changed;
            // 
            // lblAboutText1
            // 
            lblAboutText1.AutoSize = true;
            lblAboutText1.Location = new Point(12, 406);
            lblAboutText1.Name = "lblAboutText1";
            lblAboutText1.Text = "Section description";
            // 
            // lblAboutText1Count
            // 
            lblAboutText1Count.AutoSize = true;
            lblAboutText1Count.Location = new Point(560, 406);
            lblAboutText1Count.Name = "lblAboutText1Count";
            lblAboutText1Count.Text = "0/2000";
            // 
            // txtAboutText1
            // 
            txtAboutText1.Location = new Point(12, 424);
            txtAboutText1.Name = "txtAboutText1";
            txtAboutText1.Size = new Size(600, 90);
            txtAboutText1.Multiline = true;
            txtAboutText1.ScrollBars = ScrollBars.Vertical;
            txtAboutText1.TabIndex = 7;
            txtAboutText1.TextChanged += Field_Changed;
            // 
            // lblAboutText2
            // 
            lblAboutText2.AutoSize = true;
            lblAboutText2.Location = new Point(12, 522);
            lblAboutText2.Name = "lblAboutText2";
            lblAboutText2.Text = "Our Mission";
            // 
            // lblAboutText2Count
            // 
            lblAboutText2Count.AutoSize = true;
            lblAboutText2Count.Location = new Point(560, 522);
            lblAboutText2Count.Name = "lblAboutText2Count";
            lblAboutText2Count.Text = "0/1000";
            // 
            // txtAboutText2
            // 
            txtAboutText2.Location = new Point(12, 540);
            txtAboutText2.Name = "txtAboutText2";
            txtAboutText2.Size = new Size(600, 60);
            txtAboutText2.Multiline = true;
            txtAboutText2.ScrollBars = ScrollBars.Vertical;
            txtAboutText2.TabIndex = 8;
            txtAboutText2.TextChanged += Field_Changed;
            // 
            // lblAboutStatus
            // 
            lblAboutStatus.AutoSize = true;
            lblAboutStatus.Location = new Point(12, 608);
            lblAboutStatus.Name = "lblAboutStatus";
            lblAboutStatus.MaximumSize = new Size(600, 0);
            // 
            // lblAboutLastSaved
            // 
            lblAboutLastSaved.AutoSize = true;
            lblAboutLastSaved.Location = new Point(640, 608);
            lblAboutLastSaved.Name = "lblAboutLastSaved";
            lblAboutLastSaved.Text = "Last saved: -";
            // 
            // btnSaveAbout
            // 
            btnSaveAbout.Location = new Point(12, 640);
            btnSaveAbout.Name = "btnSaveAbout";
            btnSaveAbout.Size = new Size(120, 28);
            btnSaveAbout.Text = "Save Changes";
            btnSaveAbout.TabIndex = 9;
            btnSaveAbout.Click += btnSaveAbout_Click;
            // 
            // btnDiscardAbout
            // 
            btnDiscardAbout.Location = new Point(138, 640);
            btnDiscardAbout.Name = "btnDiscardAbout";
            btnDiscardAbout.Size = new Size(130, 28);
            btnDiscardAbout.Text = "Discard Changes";
            btnDiscardAbout.TabIndex = 10;
            btnDiscardAbout.Click += btnDiscardAbout_Click;
            // 
            // AboutEditorPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(lblAboutHint);
            Controls.Add(btnAboutSection1);
            Controls.Add(btnAboutSection2);
            Controls.Add(btnAboutSection3);
            Controls.Add(lblAboutSection);
            Controls.Add(lblAboutImage);
            Controls.Add(picAboutImage);
            Controls.Add(btnChooseAboutImage);
            Controls.Add(btnRemoveAboutImage);
            Controls.Add(lblAboutImageName);
            Controls.Add(lblAboutHeading);
            Controls.Add(lblAboutHeadingCount);
            Controls.Add(txtAboutHeading);
            Controls.Add(lblAboutText1);
            Controls.Add(lblAboutText1Count);
            Controls.Add(txtAboutText1);
            Controls.Add(lblAboutText2);
            Controls.Add(lblAboutText2Count);
            Controls.Add(txtAboutText2);
            Controls.Add(lblAboutStatus);
            Controls.Add(lblAboutLastSaved);
            Controls.Add(btnSaveAbout);
            Controls.Add(btnDiscardAbout);
            Name = "AboutEditorPage";
            Size = new Size(960, 700);
            Load += AboutEditorPage_Load;
            ((System.ComponentModel.ISupportInitialize)picAboutImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblAboutHint;
        private Button btnAboutSection1;
        private Button btnAboutSection2;
        private Button btnAboutSection3;
        private Label lblAboutSection;
        private Label lblAboutImage;
        private PictureBox picAboutImage;
        private Button btnChooseAboutImage;
        private Button btnRemoveAboutImage;
        private Label lblAboutImageName;
        private Label lblAboutHeading;
        private Label lblAboutHeadingCount;
        private TextBox txtAboutHeading;
        private Label lblAboutText1;
        private Label lblAboutText1Count;
        private TextBox txtAboutText1;
        private Label lblAboutText2;
        private Label lblAboutText2Count;
        private TextBox txtAboutText2;
        private Label lblAboutStatus;
        private Label lblAboutLastSaved;
        private Button btnSaveAbout;
        private Button btnDiscardAbout;
    }
}
