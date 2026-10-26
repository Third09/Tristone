# Spec for src/Company.OfficeApp/Pages/Receipts/ChangeOrdersTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Receipts"
CLASS = "ChangeOrdersTab"
SIZE = (960, 650)
LOAD_EVENT = "ChangeOrdersTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'

CONTROLS = [
    ("Label", "lblCoProjectFilter", 12, 15, None, None, {"Text": T("Project")}),
    ("ComboBox", "cboCoProjectFilter", 62, 12, 240, 23, DDL),
    ("DataGridView", "dgvChangeOrders", 12, 44, 930, 170, {}),
    ("Button", "btnNewChangeOrder", 12, 222, 140, 28, {"Text": T("New Change Order")}),
    ("Button", "btnApproveCo", 158, 222, 80, 28, {"Text": T("Approve")}),
    ("Button", "btnRejectCo", 244, 222, 70, 28, {"Text": T("Reject")}),
    ("Button", "btnPrintCo", 320, 222, 90, 28, {"Text": T("Print Form")}),
    ("Label", "lblAdminNote", 12, 256, None, None, {"Text": T("An Admin approves change orders.")}),
    ("Label", "lblCoNumber", 12, 282, None, None, {"Text": T("CO-001 (new)")}),
    ("Label", "lblCoProject", 12, 306, None, None, {"Text": T("Project")}),
    ("ComboBox", "cboCoProject", 12, 324, 280, 23, DDL),
    ("Label", "lblCoDate", 12, 354, None, None, {"Text": T("Date requested")}),
    ("DateTimePicker", "dtpCoDate", 12, 372, 125, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblCoDescription", 12, 402, None, None, {"Text": T("What the client wants changed")}),
    ("TextBox", "txtCoDescription", 12, 420, 280, 70, {"Multiline": "true", "ScrollBars": "ScrollBars.Vertical"}),
    ("Label", "lblCoCostChange", 12, 498, None, None, {"Text": T("Cost change (+ or -)")}),
    ("NumericUpDown", "numCoCostChange", 12, 516, 140, 23, {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "999999999999m", "Minimum": "-999999999999m"}),
    ("Label", "lblCoDaysChange", 162, 498, None, None, {"Text": T("Days change (+ or -)")}),
    ("NumericUpDown", "numCoDaysChange", 162, 516, 90, 23, {"Maximum": "3650m", "Minimum": "-3650m"}),
    ("Button", "btnAttachCoDoc", 12, 548, 170, 28, {"Text": T("Attach Signed Document")}),
    ("Button", "btnViewCoDoc", 188, 548, 70, 28, {"Text": T("View")}),
    ("Label", "lblCoDocFile", 264, 554, None, None, {"Text": T("")}),
    ("Label", "lblCoMsg", 12, 584, None, None, {}),
    ("Button", "btnSubmitCo", 12, 606, 90, 28, {"Text": T("Submit")}),
    ("Button", "btnCancelCo", 108, 606, 90, 28, {"Text": T("Cancel")}),
    ("Label", "lblCoImpactTitle", 340, 306, None, None, {"Text": T("Budget impact if approved")}),
    ("Label", "lblCoOriginalPrice", 340, 330, None, None, {"Text": T("Contract price now: -")}),
    ("Label", "lblCoRevisedPrice", 340, 354, None, None, {"Text": T("Revised contract price: -")}),
    ("Label", "lblCoOriginalEnd", 340, 378, None, None, {"Text": T("Target end date now: -")}),
    ("Label", "lblCoRevisedEnd", 340, 402, None, None, {"Text": T("Revised end date: -")}),
]

GRID_COLUMNS = {
    "dgvChangeOrders": [
        (TB, "colCoNo", "CoNo", "CO No.", {}),
        (TB, "colProject", "Project", "Project", {}),
        (TB, "colDate", "RequestedOn", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colCost", "CostChange", "Cost Change", {"DefaultCellStyle": MONEY}),
        (TB, "colDays", "DaysChange", "Days Change", {}),
        (TB, "colStatus", "Status", "Status", {}),
    ],
}

EVENTS = {
    "cboCoProjectFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "dgvChangeOrders": [("SelectionChanged", "dgvChangeOrders_SelectionChanged")],
    "btnNewChangeOrder": [("Click", "btnNewChangeOrder_Click")],
    "btnApproveCo": [("Click", "btnApproveCo_Click")],
    "btnRejectCo": [("Click", "btnRejectCo_Click")],
    "btnPrintCo": [("Click", "btnPrintCo_Click")],
    "cboCoProject": [("SelectedIndexChanged", "Impact_Changed")],
    "numCoCostChange": [("ValueChanged", "Impact_Changed")],
    "numCoDaysChange": [("ValueChanged", "Impact_Changed")],
    "btnAttachCoDoc": [("Click", "btnAttachCoDoc_Click")],
    "btnViewCoDoc": [("Click", "btnViewCoDoc_Click")],
    "btnSubmitCo": [("Click", "btnSubmitCo_Click")],
    "btnCancelCo": [("Click", "btnCancelCo_Click")],
}
