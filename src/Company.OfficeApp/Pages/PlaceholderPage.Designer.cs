namespace Company.OfficeApp.Pages
{
    partial class PlaceholderPage
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
            lblPlaceholder = new Label();
            SuspendLayout();
            //
            // lblPlaceholder
            //
            lblPlaceholder.AutoSize = true;
            lblPlaceholder.Location = new Point(12, 12);
            lblPlaceholder.Name = "lblPlaceholder";
            lblPlaceholder.Text = "Not built yet.";
            //
            // PlaceholderPage
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblPlaceholder);
            Name = "PlaceholderPage";
            Size = new Size(600, 400);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPlaceholder;
    }
}
