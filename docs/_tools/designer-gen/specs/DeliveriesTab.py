# Spec for src/Company.OfficeApp/Pages/Inventory/DeliveriesTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Inventory"
CLASS = "DeliveriesTab"
SIZE = (960, 640)
LOAD_EVENT = "DeliveriesTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
QTY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'
NUM = {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "999999999m"}

CONTROLS = [
    ("Label", "lblRecordTitle", 12, 12, None, None, {"Text": T("Record a delivery (stock goes up; no approval needed)")}),
    ("Label", "lblDeliveryPurchase", 12, 36, None, None, {"Text": T("Approved purchase (fills in vendor, material and price)")}),
    ("ComboBox", "cboDeliveryPurchase", 12, 54, 460, 23, DDL),
    ("Label", "lblDeliveryVendor", 12, 84, None, None, {"Text": T("Vendor")}),
    ("ComboBox", "cboDeliveryVendor", 12, 102, 220, 23, DDL),
    ("Label", "lblDeliveryMaterial", 244, 84, None, None, {"Text": T("Material")}),
    ("ComboBox", "cboDeliveryMaterial", 244, 102, 228, 23, DDL),
    ("Label", "lblDeliveryQty", 12, 132, None, None, {"Text": T("Quantity received")}),
    ("NumericUpDown", "numDeliveryQty", 12, 150, 120, 23, NUM),
    ("Label", "lblDeliveryPrice", 144, 132, None, None, {"Text": T("Unit price on receipt")}),
    ("NumericUpDown", "numDeliveryPrice", 144, 150, 120, 23, NUM),
    ("Label", "lblDeliveryDate", 276, 132, None, None, {"Text": T("Date received")}),
    ("DateTimePicker", "dtpDeliveryDate", 276, 150, 120, 23, {"Format": "DateTimePickerFormat.Short"}),
    # receipt (right)
    ("Label", "lblReceiptTitle", 500, 36, None, None, {"Text": T("Receipt")}),
    ("Label", "lblDeliveryReceiptNo", 500, 56, None, None, {"Text": T("Receipt number")}),
    ("TextBox", "txtDeliveryReceiptNo", 500, 74, 200, 23, {}),
    ("Button", "btnAttachDeliveryReceipt", 500, 102, 120, 28, {"Text": T("Attach Scan")}),
    ("Label", "lblDeliveryReceiptFile", 628, 108, None, None, {"Text": T("No file attached")}),
    ("CheckBox", "chkDeliveryPhysicalOnly", 500, 138, None, None, {"Text": T("Physical copy only (no scan)")}),
    ("Label", "lblDeliveryFiledAt", 500, 162, None, None, {"Text": T("Where the paper copy is filed (e.g. Folder B, Oct 2026)")}),
    ("TextBox", "txtDeliveryFiledAt", 500, 180, 300, 23, {}),
    ("Label", "lblDeliveryMsg", 12, 212, None, None, {}),
    ("Button", "btnSaveDelivery", 12, 234, 120, 28, {"Text": T("Save Delivery")}),
    ("Button", "btnCancelDelivery", 138, 234, 90, 28, {"Text": T("Clear")}),
    # list
    ("Label", "lblDeliveries", 12, 280, None, None, {"Text": T("Deliveries")}),
    ("DataGridView", "dgvDeliveries", 12, 300, 930, 260, {}),
    ("Button", "btnOpenDeliveryReceipt", 12, 568, 140, 28, {"Text": T("Open Receipt Scan")}),
]

GRID_COLUMNS = {
    "dgvDeliveries": [
        (TB, "colDate", "DeliveredOn", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colVendor", "Vendor", "Vendor", {}),
        (TB, "colMaterial", "Material", "Material", {}),
        (TB, "colQty", "Quantity", "Qty", {"DefaultCellStyle": QTY}),
        (TB, "colTotal", "Total", "Total", {"DefaultCellStyle": MONEY}),
        (TB, "colReceiptNo", "ReceiptNo", "Receipt No.", {}),
        (TB, "colCopy", "ReceiptCopy", "Receipt", {}),
    ],
}

EVENTS = {
    "cboDeliveryPurchase": [("SelectedIndexChanged", "cboDeliveryPurchase_SelectedIndexChanged")],
    "btnAttachDeliveryReceipt": [("Click", "btnAttachDeliveryReceipt_Click")],
    "chkDeliveryPhysicalOnly": [("CheckedChanged", "chkDeliveryPhysicalOnly_CheckedChanged")],
    "btnSaveDelivery": [("Click", "btnSaveDelivery_Click")],
    "btnCancelDelivery": [("Click", "btnCancelDelivery_Click")],
    "dgvDeliveries": [("SelectionChanged", "dgvDeliveries_SelectionChanged")],
    "btnOpenDeliveryReceipt": [("Click", "btnOpenDeliveryReceipt_Click")],
}
