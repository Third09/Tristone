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
            cardAboutForm = new Company.OfficeApp.UI.Card();
            pnlAboutSide = new Panel();
            pnlAboutFooter = new Panel();
            pnlGap1 = new Panel();
            pnlSections = new Panel();
            lblSectionsTitle = new Label();
            btnAboutSection1 = new Button();
            btnAboutSection2 = new Button();
            btnAboutSection3 = new Button();
            lblAboutImage = new Label();
            lblAboutSection = new Label();
            dzAbout = new Company.OfficeApp.UI.DropZone();
            btnRemoveAboutImage = new Button();
            lblAboutImageName = new Label();
            lblAboutInfo = new Label();
            lblAboutHeading = new Label();
            lblAboutHeadingCount = new Label();
            frmAboutHeading = new Company.OfficeApp.UI.InputFrame();
            txtAboutHeading = new TextBox();
            lblAboutText1 = new Label();
            lblAboutText1Count = new Label();
            frmAboutText1 = new Company.OfficeApp.UI.InputFrame();
            txtAboutText1 = new TextBox();
            lblAboutText2 = new Label();
            lblAboutText2Count = new Label();
            frmAboutText2 = new Company.OfficeApp.UI.InputFrame();
            txtAboutText2 = new TextBox();
            statusAbout = new Company.OfficeApp.UI.StatusBox();
            pnlSideGap = new Panel();
            cardAboutPreview = new Company.OfficeApp.UI.Card();
            lblAboutPreviewTitle = new Label();
            pnlAboutPreview = new Panel();
            cardAboutSave = new Company.OfficeApp.UI.Card();
            lblAboutLastSaved = new Label();
            btnDiscardAbout = new Button();
            btnSaveAbout = new Button();
            pnlSections.Controls.Add(lblSectionsTitle);
            pnlSections.Controls.Add(btnAboutSection1);
            pnlSections.Controls.Add(btnAboutSection2);
            pnlSections.Controls.Add(btnAboutSection3);
            cardAboutForm.Controls.Add(lblAboutImage);
            cardAboutForm.Controls.Add(lblAboutSection);
            cardAboutForm.Controls.Add(dzAbout);
            cardAboutForm.Controls.Add(btnRemoveAboutImage);
            cardAboutForm.Controls.Add(lblAboutImageName);
            cardAboutForm.Controls.Add(lblAboutInfo);
            cardAboutForm.Controls.Add(lblAboutHeading);
            cardAboutForm.Controls.Add(lblAboutHeadingCount);
            cardAboutForm.Controls.Add(frmAboutHeading);
            frmAboutHeading.Controls.Add(txtAboutHeading);
            cardAboutForm.Controls.Add(lblAboutText1);
            cardAboutForm.Controls.Add(lblAboutText1Count);
            cardAboutForm.Controls.Add(frmAboutText1);
            frmAboutText1.Controls.Add(txtAboutText1);
            cardAboutForm.Controls.Add(lblAboutText2);
            cardAboutForm.Controls.Add(lblAboutText2Count);
            cardAboutForm.Controls.Add(frmAboutText2);
            frmAboutText2.Controls.Add(txtAboutText2);
            pnlAboutSide.Controls.Add(statusAbout);
            pnlAboutSide.Controls.Add(pnlSideGap);
            pnlAboutSide.Controls.Add(cardAboutPreview);
            cardAboutPreview.Controls.Add(lblAboutPreviewTitle);
            cardAboutPreview.Controls.Add(pnlAboutPreview);
            pnlAboutFooter.Controls.Add(cardAboutSave);
            cardAboutSave.Controls.Add(lblAboutLastSaved);
            cardAboutSave.Controls.Add(btnDiscardAbout);
            cardAboutSave.Controls.Add(btnSaveAbout);
            SuspendLayout();
            // 
            // cardAboutForm
            // 
            cardAboutForm.Location = new Point(24, 136);
            cardAboutForm.Name = "cardAboutForm";
            cardAboutForm.Size = new Size(752, 712);
            cardAboutForm.Dock = DockStyle.Fill;
            cardAboutForm.Padding = new Padding(24);
            cardAboutForm.TabIndex = 0;
            // 
            // pnlAboutSide
            // 
            pnlAboutSide.Location = new Point(776, 136);
            pnlAboutSide.Name = "pnlAboutSide";
            pnlAboutSide.Size = new Size(440, 712);
            pnlAboutSide.Dock = DockStyle.Right;
            pnlAboutSide.Padding = new Padding(20, 0, 0, 0);
            pnlAboutSide.TabIndex = 1;
            // 
            // pnlAboutFooter
            // 
            pnlAboutFooter.Location = new Point(24, 848);
            pnlAboutFooter.Name = "pnlAboutFooter";
            pnlAboutFooter.Size = new Size(1192, 92);
            pnlAboutFooter.Dock = DockStyle.Bottom;
            pnlAboutFooter.Padding = new Padding(0, 20, 0, 0);
            pnlAboutFooter.TabIndex = 2;
            // 
            // pnlGap1
            // 
            pnlGap1.Location = new Point(24, 116);
            pnlGap1.Name = "pnlGap1";
            pnlGap1.Size = new Size(1192, 20);
            pnlGap1.Dock = DockStyle.Top;
            pnlGap1.TabIndex = 3;
            // 
            // pnlSections
            // 
            pnlSections.Location = new Point(24, 8);
            pnlSections.Name = "pnlSections";
            pnlSections.Size = new Size(1192, 108);
            pnlSections.Dock = DockStyle.Top;
            pnlSections.TabIndex = 4;
            // 
            // lblSectionsTitle
            // 
            lblSectionsTitle.AutoSize = true;
            lblSectionsTitle.Location = new Point(0, 4);
            lblSectionsTitle.Name = "lblSectionsTitle";
            lblSectionsTitle.Font = new Font("Segoe UI", 14F);
            lblSectionsTitle.Text = "About Us Page Sections";
            // 
            // btnAboutSection1
            // 
            btnAboutSection1.Location = new Point(0, 44);
            btnAboutSection1.Name = "btnAboutSection1";
            btnAboutSection1.Size = new Size(360, 60);
            btnAboutSection1.Font = new Font("Segoe UI Semibold", 11F);
            btnAboutSection1.TextAlign = ContentAlignment.MiddleLeft;
            btnAboutSection1.Text = "ABOUT TRI-STONE  ·  Section 1";
            btnAboutSection1.TabIndex = 5;
            btnAboutSection1.Click += btnAboutSection_Click;
            // 
            // btnAboutSection2
            // 
            btnAboutSection2.Location = new Point(376, 44);
            btnAboutSection2.Name = "btnAboutSection2";
            btnAboutSection2.Size = new Size(360, 60);
            btnAboutSection2.Font = new Font("Segoe UI Semibold", 11F);
            btnAboutSection2.TextAlign = ContentAlignment.MiddleLeft;
            btnAboutSection2.Text = "Our Principles  ·  Section 2";
            btnAboutSection2.TabIndex = 6;
            btnAboutSection2.Click += btnAboutSection_Click;
            // 
            // btnAboutSection3
            // 
            btnAboutSection3.Location = new Point(752, 44);
            btnAboutSection3.Name = "btnAboutSection3";
            btnAboutSection3.Size = new Size(360, 60);
            btnAboutSection3.Font = new Font("Segoe UI Semibold", 11F);
            btnAboutSection3.TextAlign = ContentAlignment.MiddleLeft;
            btnAboutSection3.Text = "Our Mission and Vision  ·  Section 3";
            btnAboutSection3.TabIndex = 7;
            btnAboutSection3.Click += btnAboutSection_Click;
            // 
            // lblAboutImage
            // 
            lblAboutImage.AutoSize = true;
            lblAboutImage.Location = new Point(24, 18);
            lblAboutImage.Name = "lblAboutImage";
            lblAboutImage.Font = new Font("Segoe UI", 14F);
            lblAboutImage.Text = "About Tri-Stone - Image";
            // 
            // lblAboutSection
            // 
            lblAboutSection.Location = new Point(628, 22);
            lblAboutSection.Name = "lblAboutSection";
            lblAboutSection.Size = new Size(100, 20);
            lblAboutSection.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAboutSection.TextAlign = ContentAlignment.MiddleRight;
            lblAboutSection.Text = "Section 1 of 3";
            // 
            // dzAbout
            // 
            dzAbout.Location = new Point(24, 60);
            dzAbout.Name = "dzAbout";
            dzAbout.Size = new Size(704, 180);
            dzAbout.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dzAbout.TabIndex = 8;
            // 
            // btnRemoveAboutImage
            // 
            btnRemoveAboutImage.Location = new Point(24, 248);
            btnRemoveAboutImage.Name = "btnRemoveAboutImage";
            btnRemoveAboutImage.Size = new Size(140, 34);
            btnRemoveAboutImage.Text = "Remove Picture";
            btnRemoveAboutImage.TabIndex = 9;
            btnRemoveAboutImage.Click += btnRemoveAboutImage_Click;
            // 
            // lblAboutImageName
            // 
            lblAboutImageName.AutoSize = true;
            lblAboutImageName.Location = new Point(176, 256);
            lblAboutImageName.Name = "lblAboutImageName";
            lblAboutImageName.MaximumSize = new Size(540, 0);
            // 
            // lblAboutInfo
            // 
            lblAboutInfo.AutoSize = true;
            lblAboutInfo.Location = new Point(24, 296);
            lblAboutInfo.Name = "lblAboutInfo";
            lblAboutInfo.Font = new Font("Segoe UI", 14F);
            lblAboutInfo.Text = "Section Information";
            // 
            // lblAboutHeading
            // 
            lblAboutHeading.AutoSize = true;
            lblAboutHeading.Location = new Point(24, 336);
            lblAboutHeading.Name = "lblAboutHeading";
            lblAboutHeading.Font = new Font("Segoe UI Semibold", 10F);
            lblAboutHeading.Text = "Section Heading";
            // 
            // lblAboutHeadingCount
            // 
            lblAboutHeadingCount.Location = new Point(628, 336);
            lblAboutHeadingCount.Name = "lblAboutHeadingCount";
            lblAboutHeadingCount.Size = new Size(100, 20);
            lblAboutHeadingCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAboutHeadingCount.TextAlign = ContentAlignment.MiddleRight;
            lblAboutHeadingCount.Text = "0/100";
            // 
            // frmAboutHeading
            // 
            frmAboutHeading.Location = new Point(24, 360);
            frmAboutHeading.Name = "frmAboutHeading";
            frmAboutHeading.Size = new Size(704, 44);
            frmAboutHeading.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            frmAboutHeading.TabIndex = 10;
            // 
            // txtAboutHeading
            // 
            txtAboutHeading.Location = new Point(12, 11);
            txtAboutHeading.Name = "txtAboutHeading";
            txtAboutHeading.Size = new Size(680, 23);
            txtAboutHeading.BorderStyle = BorderStyle.None;
            txtAboutHeading.Font = new Font("Segoe UI", 10.5F);
            txtAboutHeading.TabIndex = 11;
            txtAboutHeading.TextChanged += Field_Changed;
            // 
            // lblAboutText1
            // 
            lblAboutText1.AutoSize = true;
            lblAboutText1.Location = new Point(24, 418);
            lblAboutText1.Name = "lblAboutText1";
            lblAboutText1.Font = new Font("Segoe UI Semibold", 10F);
            lblAboutText1.Text = "Section Description";
            // 
            // lblAboutText1Count
            // 
            lblAboutText1Count.Location = new Point(608, 418);
            lblAboutText1Count.Name = "lblAboutText1Count";
            lblAboutText1Count.Size = new Size(120, 20);
            lblAboutText1Count.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAboutText1Count.TextAlign = ContentAlignment.MiddleRight;
            lblAboutText1Count.Text = "0/2000";
            // 
            // frmAboutText1
            // 
            frmAboutText1.Location = new Point(24, 442);
            frmAboutText1.Name = "frmAboutText1";
            frmAboutText1.Size = new Size(704, 120);
            frmAboutText1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            frmAboutText1.TabIndex = 12;
            // 
            // txtAboutText1
            // 
            txtAboutText1.Location = new Point(12, 9);
            txtAboutText1.Name = "txtAboutText1";
            txtAboutText1.Size = new Size(680, 102);
            txtAboutText1.BorderStyle = BorderStyle.None;
            txtAboutText1.Font = new Font("Segoe UI", 10.5F);
            txtAboutText1.Multiline = true;
            txtAboutText1.TabIndex = 13;
            txtAboutText1.TextChanged += Field_Changed;
            // 
            // lblAboutText2
            // 
            lblAboutText2.AutoSize = true;
            lblAboutText2.Location = new Point(24, 576);
            lblAboutText2.Name = "lblAboutText2";
            lblAboutText2.Font = new Font("Segoe UI Semibold", 10F);
            lblAboutText2.Text = "Our Mission";
            // 
            // lblAboutText2Count
            // 
            lblAboutText2Count.Location = new Point(608, 576);
            lblAboutText2Count.Name = "lblAboutText2Count";
            lblAboutText2Count.Size = new Size(120, 20);
            lblAboutText2Count.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAboutText2Count.TextAlign = ContentAlignment.MiddleRight;
            lblAboutText2Count.Text = "0/1000";
            // 
            // frmAboutText2
            // 
            frmAboutText2.Location = new Point(24, 600);
            frmAboutText2.Name = "frmAboutText2";
            frmAboutText2.Size = new Size(704, 88);
            frmAboutText2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            frmAboutText2.TabIndex = 14;
            // 
            // txtAboutText2
            // 
            txtAboutText2.Location = new Point(12, 9);
            txtAboutText2.Name = "txtAboutText2";
            txtAboutText2.Size = new Size(680, 70);
            txtAboutText2.BorderStyle = BorderStyle.None;
            txtAboutText2.Font = new Font("Segoe UI", 10.5F);
            txtAboutText2.Multiline = true;
            txtAboutText2.TabIndex = 15;
            txtAboutText2.TextChanged += Field_Changed;
            // 
            // statusAbout
            // 
            statusAbout.Location = new Point(20, 346);
            statusAbout.Name = "statusAbout";
            statusAbout.Size = new Size(420, 78);
            statusAbout.Dock = DockStyle.Top;
            statusAbout.TabIndex = 16;
            // 
            // pnlSideGap
            // 
            pnlSideGap.Location = new Point(20, 330);
            pnlSideGap.Name = "pnlSideGap";
            pnlSideGap.Size = new Size(420, 16);
            pnlSideGap.Dock = DockStyle.Top;
            pnlSideGap.TabIndex = 17;
            // 
            // cardAboutPreview
            // 
            cardAboutPreview.Location = new Point(20, 0);
            cardAboutPreview.Name = "cardAboutPreview";
            cardAboutPreview.Size = new Size(420, 330);
            cardAboutPreview.Dock = DockStyle.Top;
            cardAboutPreview.TabIndex = 18;
            // 
            // lblAboutPreviewTitle
            // 
            lblAboutPreviewTitle.AutoSize = true;
            lblAboutPreviewTitle.Location = new Point(20, 18);
            lblAboutPreviewTitle.Name = "lblAboutPreviewTitle";
            lblAboutPreviewTitle.Font = new Font("Segoe UI", 14F);
            lblAboutPreviewTitle.Text = "Live Preview";
            // 
            // pnlAboutPreview
            // 
            pnlAboutPreview.Location = new Point(20, 60);
            pnlAboutPreview.Name = "pnlAboutPreview";
            pnlAboutPreview.Size = new Size(380, 250);
            pnlAboutPreview.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlAboutPreview.TabIndex = 19;
            // 
            // cardAboutSave
            // 
            cardAboutSave.Location = new Point(0, 20);
            cardAboutSave.Name = "cardAboutSave";
            cardAboutSave.Size = new Size(1192, 72);
            cardAboutSave.Dock = DockStyle.Fill;
            cardAboutSave.Padding = new Padding(16);
            cardAboutSave.TabIndex = 20;
            // 
            // lblAboutLastSaved
            // 
            lblAboutLastSaved.Location = new Point(470, 16);
            lblAboutLastSaved.Name = "lblAboutLastSaved";
            lblAboutLastSaved.Size = new Size(380, 40);
            lblAboutLastSaved.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAboutLastSaved.TextAlign = ContentAlignment.MiddleRight;
            lblAboutLastSaved.Text = "Last saved: -";
            // 
            // btnDiscardAbout
            // 
            btnDiscardAbout.Location = new Point(866, 16);
            btnDiscardAbout.Name = "btnDiscardAbout";
            btnDiscardAbout.Size = new Size(150, 40);
            btnDiscardAbout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDiscardAbout.Font = new Font("Segoe UI", 10F);
            btnDiscardAbout.Text = "Discard Changes";
            btnDiscardAbout.TabIndex = 21;
            btnDiscardAbout.Click += btnDiscardAbout_Click;
            // 
            // btnSaveAbout
            // 
            btnSaveAbout.Location = new Point(1026, 16);
            btnSaveAbout.Name = "btnSaveAbout";
            btnSaveAbout.Size = new Size(150, 40);
            btnSaveAbout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveAbout.Font = new Font("Segoe UI", 10F);
            btnSaveAbout.Text = "Save Changes";
            btnSaveAbout.TabIndex = 22;
            btnSaveAbout.Click += btnSaveAbout_Click;
            // 
            // AboutEditorPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(cardAboutForm);
            Controls.Add(pnlAboutSide);
            Controls.Add(pnlAboutFooter);
            Controls.Add(pnlGap1);
            Controls.Add(pnlSections);
            Name = "AboutEditorPage";
            Size = new Size(1240, 960);
            Load += AboutEditorPage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Company.OfficeApp.UI.Card cardAboutForm;
        private Panel pnlAboutSide;
        private Panel pnlAboutFooter;
        private Panel pnlGap1;
        private Panel pnlSections;
        private Label lblSectionsTitle;
        private Button btnAboutSection1;
        private Button btnAboutSection2;
        private Button btnAboutSection3;
        private Label lblAboutImage;
        private Label lblAboutSection;
        private Company.OfficeApp.UI.DropZone dzAbout;
        private Button btnRemoveAboutImage;
        private Label lblAboutImageName;
        private Label lblAboutInfo;
        private Label lblAboutHeading;
        private Label lblAboutHeadingCount;
        private Company.OfficeApp.UI.InputFrame frmAboutHeading;
        private TextBox txtAboutHeading;
        private Label lblAboutText1;
        private Label lblAboutText1Count;
        private Company.OfficeApp.UI.InputFrame frmAboutText1;
        private TextBox txtAboutText1;
        private Label lblAboutText2;
        private Label lblAboutText2Count;
        private Company.OfficeApp.UI.InputFrame frmAboutText2;
        private TextBox txtAboutText2;
        private Company.OfficeApp.UI.StatusBox statusAbout;
        private Panel pnlSideGap;
        private Company.OfficeApp.UI.Card cardAboutPreview;
        private Label lblAboutPreviewTitle;
        private Panel pnlAboutPreview;
        private Company.OfficeApp.UI.Card cardAboutSave;
        private Label lblAboutLastSaved;
        private Button btnDiscardAbout;
        private Button btnSaveAbout;
    }
}
