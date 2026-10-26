# Spec for src/Company.OfficeApp/Pages/Receipts/ReceiptVaultTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Receipts"
CLASS = "ReceiptVaultTab"
SIZE = (960, 540)
LOAD_EVENT = "ReceiptVaultTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'

CONTROLS = [
    ("Label", "lblReceiptProject", 12, 15, None, None, {"Text": T("Project")}),
    ("ComboBox", "cboReceiptProject", 80, 12, 220, 23, DDL),
    ("Label", "lblReceiptFrom", 312, 15, None, None, {"Text": T("From")}),
    ("DateTimePicker", "dtpReceiptFrom", 352, 12, 115, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblReceiptTo", 477, 15, None, None, {"Text": T("To")}),
    ("DateTimePicker", "dtpReceiptTo", 503, 12, 115, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblReceiptCategory", 12, 45, None, None, {"Text": T("Category")}),
    ("ComboBox", "cboReceiptCategoryFilter", 80, 42, 150, 23, DDL),
    ("Label", "lblReceiptStatus", 242, 45, None, None, {"Text": T("Status")}),
    ("ComboBox", "cboReceiptStatusFilter", 290, 42, 140, 23, DDL),
    ("Label", "lblReceiptSearch", 442, 45, None, None, {"Text": T("Search")}),
    ("TextBox", "txtReceiptSearch", 492, 42, 200, 23, {}),
    ("DataGridView", "dgvReceipts", 12, 74, 930, 320, {"MultiSelect": "true"}),
    ("Label", "lblReceiptsTotal", 12, 402, None, None, {"Text": T("")}),
    ("Button", "btnAddReceipt", 12, 426, 100, 28, {"Text": T("Add Receipt")}),
    ("Button", "btnOpenReceipt", 118, 426, 70, 28, {"Text": T("Open")}),
    ("Button", "btnEditReceipt", 194, 426, 70, 28, {"Text": T("Edit")}),
    ("Button", "btnApproveReceipt", 270, 426, 80, 28, {"Text": T("Approve")}),
    ("Button", "btnRejectReceipt", 356, 426, 70, 28, {"Text": T("Reject")}),
    ("Button", "btnPrintSelectedReceipts", 432, 426, 110, 28, {"Text": T("Print Selected")}),
    ("Button", "btnExportReceipts", 548, 426, 80, 28, {"Text": T("Export")}),
    ("Label", "lblApproverNote", 12, 462, None, None, {"Text": T("The Receipts Supervisor or an Admin approves receipts.")}),
    ("Label", "lblVaultMsg", 12, 486, None, None, {}),
]

GRID_COLUMNS = {
    "dgvReceipts": [
        (TB, "colReceiptNo", "ReceiptNo", "Receipt No.", {}),
        (TB, "colDate", "Date", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colMerchant", "Merchant", "Merchant", {}),
        (TB, "colAmount", "Amount", "Amount", {"DefaultCellStyle": MONEY}),
        (TB, "colCategory", "Category", "Category", {}),
        (TB, "colProject", "ProjectOrOverhead", "Project / Overhead", {}),
        (TB, "colCopy", "Copy", "Copy", {}),
        (TB, "colStock", "StockPurchase", "Stock Purchase", {}),
        (TB, "colStatus", "Status", "Status", {}),
        (TB, "colUploadedBy", "UploadedBy", "Uploaded By", {}),
    ],
}

EVENTS = {
    "cboReceiptProject": [("SelectedIndexChanged", "Filter_Changed")],
    "dtpReceiptFrom": [("ValueChanged", "Filter_Changed")],
    "dtpReceiptTo": [("ValueChanged", "Filter_Changed")],
    "cboReceiptCategoryFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "cboReceiptStatusFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "dgvReceipts": [("SelectionChanged", "dgvReceipts_SelectionChanged"), ("CellDoubleClick", "dgvReceipts_CellDoubleClick")],
    "btnAddReceipt": [("Click", "btnAddReceipt_Click")],
    "btnOpenReceipt": [("Click", "btnOpenReceipt_Click")],
    "btnEditReceipt": [("Click", "btnEditReceipt_Click")],
    "btnApproveReceipt": [("Click", "btnApproveReceipt_Click")],
    "btnRejectReceipt": [("Click", "btnRejectReceipt_Click")],
    "btnPrintSelectedReceipts": [("Click", "btnPrintSelectedReceipts_Click")],
    "btnExportReceipts": [("Click", "btnExportReceipts_Click")],
}
