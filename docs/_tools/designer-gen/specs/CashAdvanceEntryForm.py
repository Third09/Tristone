# Spec for src/Company.OfficeApp/Pages/Hr/CashAdvanceEntryForm.Designer.cs (pop-up window)
NAMESPACE = "Company.OfficeApp.Pages.Hr"
CLASS = "CashAdvanceEntryForm"
FORM = True
SIZE = (400, 330)
LOAD_EVENT = "CashAdvanceEntryForm_Load"
MONEY = {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "9999999m"}

CONTROLS = [
    ("Label", "lblCaEmployee", 12, 12, None, None, {"Text": T("Employee")}),
    ("ComboBox", "cboCaEmployee", 12, 30, 370, 23, DDL),
    ("Label", "lblCaAmount", 12, 60, None, None, {"Text": T("Amount given")}),
    ("NumericUpDown", "numCaAmount", 12, 78, 140, 23, MONEY),
    ("Label", "lblCaPerPayday", 170, 60, None, None, {"Text": T("Deduct per payday")}),
    ("NumericUpDown", "numCaPerPayday", 170, 78, 140, 23, MONEY),
    ("Label", "lblCaDate", 12, 108, None, None, {"Text": T("Date given")}),
    ("DateTimePicker", "dtpCaDate", 12, 126, 130, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblCaReason", 12, 156, None, None, {"Text": T("Reason")}),
    ("TextBox", "txtCaReason", 12, 174, 370, 60, {"Multiline": "true", "ScrollBars": "ScrollBars.Vertical"}),
    ("Label", "lblCaEntryMsg", 12, 242, None, None, {"MaximumSize": "new Size(370, 0)"}),
    ("Button", "btnSaveCashAdvance", 196, 288, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelCashAdvance", 292, 288, 90, 28, {"Text": T("Cancel"), "DialogResult": "DialogResult.Cancel"}),
]

GRID_COLUMNS = {}

EVENTS = {
    "btnSaveCashAdvance": [("Click", "btnSaveCashAdvance_Click")],
}
