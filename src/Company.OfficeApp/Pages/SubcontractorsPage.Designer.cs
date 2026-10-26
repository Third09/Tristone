namespace Company.OfficeApp.Pages
{
    partial class SubcontractorsPage
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
            tabSubcontractors = new TabControl();
            tabSubDirectory = new TabPage();
            tabSubJobs = new TabPage();
            tabSubBilling = new TabPage();
            tabSubEditHistory = new TabPage();
            tabSubcontractors.SuspendLayout();
            SuspendLayout();
            //
            // tabSubcontractors
            //
            tabSubcontractors.Controls.Add(tabSubDirectory);
            tabSubcontractors.Controls.Add(tabSubJobs);
            tabSubcontractors.Controls.Add(tabSubBilling);
            tabSubcontractors.Controls.Add(tabSubEditHistory);
            tabSubcontractors.Dock = DockStyle.Fill;
            tabSubcontractors.Name = "tabSubcontractors";
            tabSubcontractors.SelectedIndex = 0;
            tabSubcontractors.TabIndex = 0;
            //
            // tabSubDirectory
            //
            tabSubDirectory.Name = "tabSubDirectory";
            tabSubDirectory.Text = "Directory";
            //
            // tabSubJobs
            //
            tabSubJobs.Name = "tabSubJobs";
            tabSubJobs.Text = "Jobs & Progress";
            //
            // tabSubBilling
            //
            tabSubBilling.Name = "tabSubBilling";
            tabSubBilling.Text = "Billing & Payouts";
            //
            // tabSubEditHistory
            //
            tabSubEditHistory.Name = "tabSubEditHistory";
            tabSubEditHistory.Text = "Edit History";
            //
            // SubcontractorsPage
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabSubcontractors);
            Name = "SubcontractorsPage";
            Size = new Size(980, 760);
            Load += SubcontractorsPage_Load;
            tabSubcontractors.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabSubcontractors;
        private TabPage tabSubDirectory;
        private TabPage tabSubJobs;
        private TabPage tabSubBilling;
        private TabPage tabSubEditHistory;
    }
}
