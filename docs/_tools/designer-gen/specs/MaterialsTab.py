# Spec for src/Company.OfficeApp/Pages/Inventory/MaterialsTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Inventory"
CLASS = "MaterialsTab"
SIZE = (960, 640)
LOAD_EVENT = "MaterialsTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
QTY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" }'
NUM = {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "999999999m"}

CONTROLS = [
    ("Label", "lblSearch", 12, 15, None, None, {"Text": T("Search")}),
    ("TextBox", "txtMaterialSearch", 62, 12, 180, 23, {}),
    ("Label", "lblCategoryFilter", 256, 15, None, None, {"Text": T("Category")}),
    ("ComboBox", "cboMaterialCategoryFilter", 318, 12, 140, 23, DDL),
    ("CheckBox", "chkLowStockOnly", 472, 14, None, None, {"Text": T("Low stock only")}),
    ("Label", "lblLowStockCount", 600, 15, None, None, {"Text": T("")}),
    ("DataGridView", "dgvMaterials", 12, 44, 930, 220, {}),
    ("Button", "btnAddMaterial", 12, 272, 110, 28, {"Text": T("Add Material")}),
    ("Button", "btnDeleteMaterial", 128, 272, 90, 28, {"Text": T("Delete")}),
    ("Button", "btnExportMaterials", 224, 272, 90, 28, {"Text": T("Export")}),
    ("Label", "lblFormMode", 12, 316, None, None, {"Text": T("New material")}),
    ("Label", "lblMaterialName", 12, 340, None, None, {"Text": T("Name (e.g. Portland Cement 40kg)")}),
    ("TextBox", "txtMaterialName", 12, 358, 300, 23, {}),
    ("Label", "lblMaterialCategory", 12, 388, None, None, {"Text": T("Category (pick or type)")}),
    ("ComboBox", "cboMaterialCategory", 12, 406, 200, 23, {}),
    ("Label", "lblMaterialUnit", 12, 436, None, None, {"Text": T("Unit (pick or type)")}),
    ("ComboBox", "cboMaterialUnit", 12, 454, 200, 23, {}),
    ("Label", "lblMaterialPrice", 340, 340, None, None, {"Text": T("Unit price (pesos)")}),
    ("NumericUpDown", "numMaterialPrice", 340, 358, 140, 23, NUM),
    ("Label", "lblStartingStock", 340, 388, None, None, {"Text": T("Starting stock (new materials only)")}),
    ("NumericUpDown", "numStartingStock", 340, 406, 140, 23, NUM),
    ("Label", "lblCurrentStock", 340, 438, None, None, {"Text": T("Current stock: -")}),
    ("Label", "lblMinimumStock", 340, 460, None, None, {"Text": T("Minimum stock (alert below this)")}),
    ("NumericUpDown", "numMinimumStock", 340, 478, 140, 23, NUM),
    ("Label", "lblMaterialMsg", 12, 516, None, None, {}),
    ("Button", "btnSaveMaterial", 12, 538, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelMaterial", 108, 538, 90, 28, {"Text": T("Cancel")}),
]

GRID_COLUMNS = {
    "dgvMaterials": [
        (TB, "colName", "Name", "Name", {}),
        (TB, "colCategory", "Category", "Category", {}),
        (TB, "colUnit", "Unit", "Unit", {}),
        (TB, "colPrice", "UnitPrice", "Unit Price", {"DefaultCellStyle": MONEY}),
        (TB, "colStock", "StockQty", "Stock", {"DefaultCellStyle": QTY}),
        (TB, "colMinimum", "MinimumStock", "Minimum", {"DefaultCellStyle": QTY}),
    ],
}

EVENTS = {
    "cboMaterialCategoryFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "chkLowStockOnly": [("CheckedChanged", "Filter_Changed")],
    "dgvMaterials": [("SelectionChanged", "dgvMaterials_SelectionChanged"), ("CellFormatting", "dgvMaterials_CellFormatting")],
    "btnAddMaterial": [("Click", "btnAddMaterial_Click")],
    "btnDeleteMaterial": [("Click", "btnDeleteMaterial_Click")],
    "btnExportMaterials": [("Click", "btnExportMaterials_Click")],
    "btnSaveMaterial": [("Click", "btnSaveMaterial_Click")],
    "btnCancelMaterial": [("Click", "btnCancelMaterial_Click")],
}
