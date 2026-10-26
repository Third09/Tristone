namespace Company.OfficeApp.Pages
{
    partial class InventoryPage
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
            tabInventory = new TabControl();
            tabMaterials = new TabPage();
            tabMaterialUsage = new TabPage();
            tabDeliveries = new TabPage();
            tabEquipment = new TabPage();
            tabVendors = new TabPage();
            tabPriceComparison = new TabPage();
            tabInvEditHistory = new TabPage();
            tabInventory.SuspendLayout();
            SuspendLayout();
            //
            // tabInventory
            //
            tabInventory.Controls.Add(tabMaterials);
            tabInventory.Controls.Add(tabMaterialUsage);
            tabInventory.Controls.Add(tabDeliveries);
            tabInventory.Controls.Add(tabEquipment);
            tabInventory.Controls.Add(tabVendors);
            tabInventory.Controls.Add(tabPriceComparison);
            tabInventory.Controls.Add(tabInvEditHistory);
            tabInventory.Dock = DockStyle.Fill;
            tabInventory.Name = "tabInventory";
            tabInventory.SelectedIndex = 0;
            tabInventory.TabIndex = 0;
            //
            // tabMaterials
            //
            tabMaterials.Name = "tabMaterials";
            tabMaterials.Text = "Materials";
            //
            // tabMaterialUsage
            //
            tabMaterialUsage.Name = "tabMaterialUsage";
            tabMaterialUsage.Text = "Material Usage";
            //
            // tabDeliveries
            //
            tabDeliveries.Name = "tabDeliveries";
            tabDeliveries.Text = "Deliveries";
            //
            // tabEquipment
            //
            tabEquipment.Name = "tabEquipment";
            tabEquipment.Text = "Equipment";
            //
            // tabVendors
            //
            tabVendors.Name = "tabVendors";
            tabVendors.Text = "Vendors";
            //
            // tabPriceComparison
            //
            tabPriceComparison.Name = "tabPriceComparison";
            tabPriceComparison.Text = "Price Comparison";
            //
            // tabInvEditHistory
            //
            tabInvEditHistory.Name = "tabInvEditHistory";
            tabInvEditHistory.Text = "Edit History";
            //
            // InventoryPage
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabInventory);
            Name = "InventoryPage";
            Size = new Size(980, 760);
            Load += InventoryPage_Load;
            tabInventory.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabInventory;
        private TabPage tabMaterials;
        private TabPage tabMaterialUsage;
        private TabPage tabDeliveries;
        private TabPage tabEquipment;
        private TabPage tabVendors;
        private TabPage tabPriceComparison;
        private TabPage tabInvEditHistory;
    }
}
