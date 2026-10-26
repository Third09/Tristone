# Spec for src/Company.OfficeApp/Pages/Inventory/UsageTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Inventory"
CLASS = "UsageTab"
SIZE = (960, 700)
LOAD_EVENT = "UsageTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
QTY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'

CONTROLS = [
    # record usage (top)
    ("Label", "lblRecordTitle", 12, 12, None, None, {"Text": T("Record material used on a project (waits for Supervisor approval)")}),
    ("Label", "lblUsageProject", 12, 36, None, None, {"Text": T("Project")}),
    ("ComboBox", "cboUsageProject", 12, 54, 240, 23, DDL),
    ("Label", "lblUsageMaterial", 264, 36, None, None, {"Text": T("Material")}),
    ("ComboBox", "cboUsageMaterial", 264, 54, 240, 23, DDL),
    ("Label", "lblAvailableStock", 516, 57, None, None, {"Text": T("In stock: -")}),
    ("Label", "lblUsageQty", 12, 84, None, None, {"Text": T("Quantity used")}),
    ("NumericUpDown", "numUsageQty", 12, 102, 120, 23, {"DecimalPlaces": "2", "Maximum": "999999999m"}),
    ("Label", "lblUsageDate", 144, 84, None, None, {"Text": T("Date used")}),
    ("DateTimePicker", "dtpUsageDate", 144, 102, 120, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblUsageNotes", 276, 84, None, None, {"Text": T("Notes (optional)")}),
    ("TextBox", "txtUsageNotes", 276, 102, 300, 23, {}),
    ("Button", "btnSubmitUsage", 588, 100, 90, 28, {"Text": T("Submit")}),
    ("Label", "lblUsageMsg", 12, 132, None, None, {}),
    # requests (middle)
    ("Label", "lblStatusFilter", 12, 160, None, None, {"Text": T("Show")}),
    ("ComboBox", "cboUsageStatusFilter", 52, 157, 160, 23, DDL),
    ("DataGridView", "dgvUsageRequests", 12, 188, 930, 200, {}),
    ("Button", "btnApproveUsage", 12, 396, 90, 28, {"Text": T("Approve")}),
    ("Button", "btnRejectUsage", 108, 396, 90, 28, {"Text": T("Reject")}),
    ("Label", "lblApproverNote", 210, 402, None, None, {"Text": T("Approve / Reject: Inventory Supervisor or Admin only")}),
    # project history (bottom)
    ("Label", "lblHistoryTitle", 12, 440, None, None, {"Text": T("Project material history (approved only)")}),
    ("ComboBox", "cboHistoryProject", 12, 460, 240, 23, DDL),
    ("Label", "lblProjectMaterialTotal", 264, 463, None, None, {"Text": T("Total: -")}),
    ("Button", "btnExportProjectMaterials", 852, 458, 90, 28, {"Text": T("Export")}),
    ("DataGridView", "dgvProjectMaterials", 12, 490, 930, 160, {}),
]

GRID_COLUMNS = {
    "dgvUsageRequests": [
        (TB, "colDate", "UsedOn", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colProject", "Project", "Project", {}),
        (TB, "colMaterial", "Material", "Material", {}),
        (TB, "colQty", "Quantity", "Qty", {"DefaultCellStyle": QTY}),
        (TB, "colUnit", "Unit", "Unit", {}),
        (TB, "colCost", "Cost", "Cost", {"DefaultCellStyle": MONEY}),
        (TB, "colRequestedBy", "RequestedBy", "Requested By", {}),
        (TB, "colStatus", "Status", "Status", {}),
    ],
    "dgvProjectMaterials": [
        (TB, "colHDate", "UsedOn", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colHMaterial", "Material", "Material", {}),
        (TB, "colHQty", "Quantity", "Qty", {"DefaultCellStyle": QTY}),
        (TB, "colHUnit", "Unit", "Unit", {}),
        (TB, "colHCost", "Cost", "Cost", {"DefaultCellStyle": MONEY}),
    ],
}

EVENTS = {
    "cboUsageMaterial": [("SelectedIndexChanged", "cboUsageMaterial_SelectedIndexChanged")],
    "btnSubmitUsage": [("Click", "btnSubmitUsage_Click")],
    "cboUsageStatusFilter": [("SelectedIndexChanged", "cboUsageStatusFilter_SelectedIndexChanged")],
    "dgvUsageRequests": [("SelectionChanged", "dgvUsageRequests_SelectionChanged")],
    "btnApproveUsage": [("Click", "btnApproveUsage_Click")],
    "btnRejectUsage": [("Click", "btnRejectUsage_Click")],
    "cboHistoryProject": [("SelectedIndexChanged", "cboHistoryProject_SelectedIndexChanged")],
    "btnExportProjectMaterials": [("Click", "btnExportProjectMaterials_Click")],
}
