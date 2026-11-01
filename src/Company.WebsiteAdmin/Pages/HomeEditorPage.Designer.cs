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
            cardHero = new Company.OfficeApp.UI.Card();
            pnlHomeSide = new Panel();
            pnlHomeFooter = new Panel();
            lblHeroTitle = new Label();
            dzHero = new Company.OfficeApp.UI.DropZone();
            btnRemoveHeroImage = new Button();
            lblHeroImageName = new Label();
            lblHeroText = new Label();
            lblHeadline = new Label();
            lblHeadlineCount = new Label();
            frmHeadline = new Company.OfficeApp.UI.InputFrame();
            txtHeadline = new TextBox();
            lblSubheadline = new Label();
            lblSubheadlineCount = new Label();
            frmSubheadline = new Company.OfficeApp.UI.InputFrame();
            txtSubheadline = new TextBox();
            statusHome = new Company.OfficeApp.UI.StatusBox();
            pnlSideGap = new Panel();
            cardPreview = new Company.OfficeApp.UI.Card();
            lblPreviewTitle = new Label();
            pnlPreview = new Panel();
            lblPreviewNote = new Label();
            cardHomeFooter = new Company.OfficeApp.UI.Card();
            lblHomeLastSaved = new Label();
            btnDiscardHome = new Button();
            btnSaveHome = new Button();
            cardHero.Controls.Add(lblHeroTitle);
            cardHero.Controls.Add(dzHero);
            cardHero.Controls.Add(btnRemoveHeroImage);
            cardHero.Controls.Add(lblHeroImageName);
            cardHero.Controls.Add(lblHeroText);
            cardHero.Controls.Add(lblHeadline);
            cardHero.Controls.Add(lblHeadlineCount);
            cardHero.Controls.Add(frmHeadline);
            frmHeadline.Controls.Add(txtHeadline);
            cardHero.Controls.Add(lblSubheadline);
            cardHero.Controls.Add(lblSubheadlineCount);
            cardHero.Controls.Add(frmSubheadline);
            frmSubheadline.Controls.Add(txtSubheadline);
            pnlHomeSide.Controls.Add(statusHome);
            pnlHomeSide.Controls.Add(pnlSideGap);
            pnlHomeSide.Controls.Add(cardPreview);
            cardPreview.Controls.Add(lblPreviewTitle);
            cardPreview.Controls.Add(pnlPreview);
            cardPreview.Controls.Add(lblPreviewNote);
            pnlHomeFooter.Controls.Add(cardHomeFooter);
            cardHomeFooter.Controls.Add(lblHomeLastSaved);
            cardHomeFooter.Controls.Add(btnDiscardHome);
            cardHomeFooter.Controls.Add(btnSaveHome);
            SuspendLayout();
            // 
            // cardHero
            // 
            cardHero.Location = new Point(24, 8);
            cardHero.Name = "cardHero";
            cardHero.Size = new Size(772, 640);
            cardHero.Dock = DockStyle.Fill;
            cardHero.Padding = new Padding(24);
            cardHero.TabIndex = 0;
            // 
            // pnlHomeSide
            // 
            pnlHomeSide.Location = new Point(796, 8);
            pnlHomeSide.Name = "pnlHomeSide";
            pnlHomeSide.Size = new Size(420, 640);
            pnlHomeSide.Dock = DockStyle.Right;
            pnlHomeSide.Padding = new Padding(20, 0, 0, 0);
            pnlHomeSide.TabIndex = 1;
            // 
            // pnlHomeFooter
            // 
            pnlHomeFooter.Location = new Point(24, 648);
            pnlHomeFooter.Name = "pnlHomeFooter";
            pnlHomeFooter.Size = new Size(1192, 92);
            pnlHomeFooter.Dock = DockStyle.Bottom;
            pnlHomeFooter.Padding = new Padding(0, 20, 0, 0);
            pnlHomeFooter.TabIndex = 2;
            // 
            // lblHeroTitle
            // 
            lblHeroTitle.AutoSize = true;
            lblHeroTitle.Location = new Point(24, 20);
            lblHeroTitle.Name = "lblHeroTitle";
            lblHeroTitle.Font = new Font("Segoe UI", 14F);
            lblHeroTitle.Text = "Hero Background Image";
            // 
            // dzHero
            // 
            dzHero.Location = new Point(24, 62);
            dzHero.Name = "dzHero";
            dzHero.Size = new Size(724, 250);
            dzHero.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dzHero.TabIndex = 3;
            // 
            // btnRemoveHeroImage
            // 
            btnRemoveHeroImage.Location = new Point(24, 322);
            btnRemoveHeroImage.Name = "btnRemoveHeroImage";
            btnRemoveHeroImage.Size = new Size(140, 34);
            btnRemoveHeroImage.Text = "Remove Picture";
            btnRemoveHeroImage.TabIndex = 4;
            btnRemoveHeroImage.Click += btnRemoveHeroImage_Click;
            // 
            // lblHeroImageName
            // 
            lblHeroImageName.AutoSize = true;
            lblHeroImageName.Location = new Point(176, 330);
            lblHeroImageName.Name = "lblHeroImageName";
            lblHeroImageName.MaximumSize = new Size(560, 0);
            // 
            // lblHeroText
            // 
            lblHeroText.AutoSize = true;
            lblHeroText.Location = new Point(24, 376);
            lblHeroText.Name = "lblHeroText";
            lblHeroText.Font = new Font("Segoe UI", 14F);
            lblHeroText.Text = "Hero Background Text Content";
            // 
            // lblHeadline
            // 
            lblHeadline.AutoSize = true;
            lblHeadline.Location = new Point(24, 418);
            lblHeadline.Name = "lblHeadline";
            lblHeadline.Font = new Font("Segoe UI Semibold", 10F);
            lblHeadline.Text = "Main Headline text";
            // 
            // lblHeadlineCount
            // 
            lblHeadlineCount.Location = new Point(648, 418);
            lblHeadlineCount.Name = "lblHeadlineCount";
            lblHeadlineCount.Size = new Size(100, 20);
            lblHeadlineCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblHeadlineCount.TextAlign = ContentAlignment.MiddleRight;
            lblHeadlineCount.Text = "0/150";
            // 
            // frmHeadline
            // 
            frmHeadline.Location = new Point(24, 442);
            frmHeadline.Name = "frmHeadline";
            frmHeadline.Size = new Size(724, 44);
            frmHeadline.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            frmHeadline.TabIndex = 5;
            // 
            // txtHeadline
            // 
            txtHeadline.Location = new Point(12, 11);
            txtHeadline.Name = "txtHeadline";
            txtHeadline.Size = new Size(700, 23);
            txtHeadline.BorderStyle = BorderStyle.None;
            txtHeadline.Font = new Font("Segoe UI", 10.5F);
            txtHeadline.TabIndex = 6;
            txtHeadline.TextChanged += Field_Changed;
            // 
            // lblSubheadline
            // 
            lblSubheadline.AutoSize = true;
            lblSubheadline.Location = new Point(24, 502);
            lblSubheadline.Name = "lblSubheadline";
            lblSubheadline.Font = new Font("Segoe UI Semibold", 10F);
            lblSubheadline.Text = "Subheadline Text";
            // 
            // lblSubheadlineCount
            // 
            lblSubheadlineCount.Location = new Point(648, 502);
            lblSubheadlineCount.Name = "lblSubheadlineCount";
            lblSubheadlineCount.Size = new Size(100, 20);
            lblSubheadlineCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSubheadlineCount.TextAlign = ContentAlignment.MiddleRight;
            lblSubheadlineCount.Text = "0/300";
            // 
            // frmSubheadline
            // 
            frmSubheadline.Location = new Point(24, 526);
            frmSubheadline.Name = "frmSubheadline";
            frmSubheadline.Size = new Size(724, 96);
            frmSubheadline.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            frmSubheadline.TabIndex = 7;
            // 
            // txtSubheadline
            // 
            txtSubheadline.Location = new Point(12, 9);
            txtSubheadline.Name = "txtSubheadline";
            txtSubheadline.Size = new Size(700, 78);
            txtSubheadline.BorderStyle = BorderStyle.None;
            txtSubheadline.Font = new Font("Segoe UI", 10.5F);
            txtSubheadline.Multiline = true;
            txtSubheadline.TabIndex = 8;
            txtSubheadline.TextChanged += Field_Changed;
            // 
            // statusHome
            // 
            statusHome.Location = new Point(20, 346);
            statusHome.Name = "statusHome";
            statusHome.Size = new Size(400, 78);
            statusHome.Dock = DockStyle.Top;
            statusHome.TabIndex = 9;
            // 
            // pnlSideGap
            // 
            pnlSideGap.Location = new Point(20, 330);
            pnlSideGap.Name = "pnlSideGap";
            pnlSideGap.Size = new Size(400, 16);
            pnlSideGap.Dock = DockStyle.Top;
            pnlSideGap.TabIndex = 10;
            // 
            // cardPreview
            // 
            cardPreview.Location = new Point(20, 0);
            cardPreview.Name = "cardPreview";
            cardPreview.Size = new Size(400, 330);
            cardPreview.Dock = DockStyle.Top;
            cardPreview.TabIndex = 11;
            // 
            // lblPreviewTitle
            // 
            lblPreviewTitle.AutoSize = true;
            lblPreviewTitle.Location = new Point(20, 18);
            lblPreviewTitle.Name = "lblPreviewTitle";
            lblPreviewTitle.Font = new Font("Segoe UI", 14F);
            lblPreviewTitle.Text = "Live Preview";
            // 
            // pnlPreview
            // 
            pnlPreview.Location = new Point(20, 60);
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Size = new Size(360, 210);
            pnlPreview.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlPreview.TabIndex = 12;
            // 
            // lblPreviewNote
            // 
            lblPreviewNote.Location = new Point(20, 282);
            lblPreviewNote.Name = "lblPreviewNote";
            lblPreviewNote.Size = new Size(360, 36);
            lblPreviewNote.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblPreviewNote.Text = "How the top of the website will look after Save Changes and Publish Website.";
            // 
            // cardHomeFooter
            // 
            cardHomeFooter.Location = new Point(0, 20);
            cardHomeFooter.Name = "cardHomeFooter";
            cardHomeFooter.Size = new Size(1192, 72);
            cardHomeFooter.Dock = DockStyle.Fill;
            cardHomeFooter.Padding = new Padding(16);
            cardHomeFooter.TabIndex = 13;
            // 
            // lblHomeLastSaved
            // 
            lblHomeLastSaved.Location = new Point(470, 16);
            lblHomeLastSaved.Name = "lblHomeLastSaved";
            lblHomeLastSaved.Size = new Size(380, 40);
            lblHomeLastSaved.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblHomeLastSaved.TextAlign = ContentAlignment.MiddleRight;
            lblHomeLastSaved.Text = "Last saved: -";
            // 
            // btnDiscardHome
            // 
            btnDiscardHome.Location = new Point(866, 16);
            btnDiscardHome.Name = "btnDiscardHome";
            btnDiscardHome.Size = new Size(150, 40);
            btnDiscardHome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDiscardHome.Font = new Font("Segoe UI", 10F);
            btnDiscardHome.Text = "Discard Changes";
            btnDiscardHome.TabIndex = 14;
            btnDiscardHome.Click += btnDiscardHome_Click;
            // 
            // btnSaveHome
            // 
            btnSaveHome.Location = new Point(1026, 16);
            btnSaveHome.Name = "btnSaveHome";
            btnSaveHome.Size = new Size(150, 40);
            btnSaveHome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveHome.Font = new Font("Segoe UI", 10F);
            btnSaveHome.Text = "Save Changes";
            btnSaveHome.TabIndex = 15;
            btnSaveHome.Click += btnSaveHome_Click;
            // 
            // HomeEditorPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(cardHero);
            Controls.Add(pnlHomeSide);
            Controls.Add(pnlHomeFooter);
            Name = "HomeEditorPage";
            Size = new Size(1240, 760);
            Load += HomeEditorPage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Company.OfficeApp.UI.Card cardHero;
        private Panel pnlHomeSide;
        private Panel pnlHomeFooter;
        private Label lblHeroTitle;
        private Company.OfficeApp.UI.DropZone dzHero;
        private Button btnRemoveHeroImage;
        private Label lblHeroImageName;
        private Label lblHeroText;
        private Label lblHeadline;
        private Label lblHeadlineCount;
        private Company.OfficeApp.UI.InputFrame frmHeadline;
        private TextBox txtHeadline;
        private Label lblSubheadline;
        private Label lblSubheadlineCount;
        private Company.OfficeApp.UI.InputFrame frmSubheadline;
        private TextBox txtSubheadline;
        private Company.OfficeApp.UI.StatusBox statusHome;
        private Panel pnlSideGap;
        private Company.OfficeApp.UI.Card cardPreview;
        private Label lblPreviewTitle;
        private Panel pnlPreview;
        private Label lblPreviewNote;
        private Company.OfficeApp.UI.Card cardHomeFooter;
        private Label lblHomeLastSaved;
        private Button btnDiscardHome;
        private Button btnSaveHome;
    }
}
