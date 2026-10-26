# Spec for src/Company.OfficeApp/Pages/Receipts/ReceiptEntryForm.Designer.cs (pop-up window)
NAMESPACE = "Company.OfficeApp.Pages.Receipts"
CLASS = "ReceiptEntryForm"
FORM = True
SIZE = (520, 560)
LOAD_EVENT = "ReceiptEntryForm_Load"

CONTROLS = [
    ("Label", "lblReceiptNo", 12, 12, None, None, {"Text": T("Receipt number")}),
    ("TextBox", "txtReceiptNo", 12, 30, 200, 23, {}),
    ("Label", "lblPurchaseDate", 222, 12, None, None, {"Text": T("Date of purchase")}),
    ("DateTimePicker", "dtpPurchaseDate", 222, 30, 130, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblMerchant", 12, 60, None, None, {"Text": T("Merchant / store")}),
    ("TextBox", "txtMerchant", 12, 78, 340, 23, {}),
    ("Label", "lblReceiptAmount", 12, 108, None, None, {"Text": T("Amount")}),
    ("NumericUpDown", "numReceiptAmount", 12, 126, 140, 23, {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "999999999999m"}),
    ("Label", "lblReceiptCategory", 162, 108, None, None, {"Text": T("Category")}),
    ("ComboBox", "cboReceiptCategory", 162, 126, 190, 23, DDL),
    ("RadioButton", "rdoProjectExpense", 12, 160, None, None, {"Text": T("Project expense"), "AutoSize": "true", "Checked": "true"}),
    ("RadioButton", "rdoOverheadExpense", 150, 160, None, None, {"Text": T("Company overhead"), "AutoSize": "true"}),
    ("ComboBox", "cboReceiptForProject", 12, 186, 340, 23, DDL),
    ("Label", "lblUploadedBy", 12, 220, None, None, {"Text": T("")}),
    ("Label", "lblReceiptCopy", 12, 252, None, None, {"Text": T("Receipt copy")}),
    ("Button", "btnAttachScan", 12, 272, 110, 28, {"Text": T("Attach Scan")}),
    ("Label", "lblScanFile", 130, 278, None, None, {"Text": T("(no file)")}),
    ("CheckBox", "chkPhysicalOnly", 12, 310, None, None, {"Text": T("Physical copy only")}),
    ("Label", "lblFiledLocation", 12, 336, None, None, {"Text": T("Where the paper copy is filed")}),
    ("TextBox", "txtFiledLocation", 12, 354, 340, 23, {}),
    ("CheckBox", "chkNoReceipt", 12, 392, None, None, {"Text": T("No receipt at all (goes to Needs Review)")}),
    ("Label", "lblNoReceiptReason", 12, 418, None, None, {"Text": T("Why there is no receipt")}),
    ("TextBox", "txtNoReceiptReason", 12, 436, 490, 50, {"Multiline": "true", "ScrollBars": "ScrollBars.Vertical"}),
    ("Label", "lblEntryMsg", 12, 494, None, None, {}),
    ("Button", "btnSaveReceipt", 320, 518, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelReceipt", 416, 518, 90, 28, {"Text": T("Cancel"), "DialogResult": "DialogResult.Cancel"}),
]

GRID_COLUMNS = {}

EVENTS = {
    "rdoProjectExpense": [("CheckedChanged", "Options_Changed")],
    "chkPhysicalOnly": [("CheckedChanged", "Options_Changed")],
    "chkNoReceipt": [("CheckedChanged", "Options_Changed")],
    "btnAttachScan": [("Click", "btnAttachScan_Click")],
    "btnSaveReceipt": [("Click", "btnSaveReceipt_Click")],
}
