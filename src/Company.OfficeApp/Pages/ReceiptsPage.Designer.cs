namespace Company.OfficeApp.Pages
{
    partial class ReceiptsPage
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
            tabReceipts = new TabControl();
            tabReceiptVault = new TabPage();
            tabNeedsReview = new TabPage();
            tabChangeOrders = new TabPage();
            tabReceiptReports = new TabPage();
            tabRecEditHistory = new TabPage();
            tabReceipts.SuspendLayout();
            SuspendLayout();
            //
            // tabReceipts
            //
            tabReceipts.Controls.Add(tabReceiptVault);
            tabReceipts.Controls.Add(tabNeedsReview);
            tabReceipts.Controls.Add(tabChangeOrders);
            tabReceipts.Controls.Add(tabReceiptReports);
            tabReceipts.Controls.Add(tabRecEditHistory);
            tabReceipts.Dock = DockStyle.Fill;
            tabReceipts.Name = "tabReceipts";
            tabReceipts.SelectedIndex = 0;
            tabReceipts.TabIndex = 0;
            //
            // tabReceiptVault
            //
            tabReceiptVault.Name = "tabReceiptVault";
            tabReceiptVault.Text = "Receipt Vault";
            //
            // tabNeedsReview
            //
            tabNeedsReview.Name = "tabNeedsReview";
            tabNeedsReview.Text = "Needs Review";
            //
            // tabChangeOrders
            //
            tabChangeOrders.Name = "tabChangeOrders";
            tabChangeOrders.Text = "Change Orders";
            //
            // tabReceiptReports
            //
            tabReceiptReports.Name = "tabReceiptReports";
            tabReceiptReports.Text = "Reports";
            //
            // tabRecEditHistory
            //
            tabRecEditHistory.Name = "tabRecEditHistory";
            tabRecEditHistory.Text = "Edit History";
            //
            // ReceiptsPage
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabReceipts);
            Name = "ReceiptsPage";
            Size = new Size(980, 760);
            Load += ReceiptsPage_Load;
            tabReceipts.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabReceipts;
        private TabPage tabReceiptVault;
        private TabPage tabNeedsReview;
        private TabPage tabChangeOrders;
        private TabPage tabReceiptReports;
        private TabPage tabRecEditHistory;
    }
}
