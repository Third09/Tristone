# Spec for src/Company.OfficeApp/Pages/Inventory/MaintenanceEntryForm.Designer.cs (a Form; window settings are in code)
NAMESPACE = "Company.OfficeApp.Pages.Inventory"
CLASS = "MaintenanceEntryForm"
SIZE = (420, 384)
FORM = True

CONTROLS = [
    ("Label", "lblMaintDate", 12, 12, None, None, {"Text": T("Date of the work")}),
    ("DateTimePicker", "dtpMaintDate", 12, 30, 140, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblMaintType", 170, 12, None, None, {"Text": T("Type")}),
    ("ComboBox", "cboMaintType", 170, 30, 140, 23, DDL),
    ("Label", "lblMaintWork", 12, 62, None, None, {"Text": T("Work done")}),
    ("TextBox", "txtMaintWork", 12, 80, 380, 70, {"Multiline": "true"}),
    ("Label", "lblMaintCost", 12, 160, None, None, {"Text": T("Cost (pesos)")}),
    ("NumericUpDown", "numMaintCost", 12, 178, 140, 23, {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "99999999m"}),
    ("Label", "lblMaintDoneBy", 12, 210, None, None, {"Text": T("Done by (person or shop)")}),
    ("TextBox", "txtMaintDoneBy", 12, 228, 260, 23, {}),
    ("Label", "lblMaintNextDue", 12, 260, None, None, {"Text": T("Next maintenance date (tick to set; moves the reminder)")}),
    ("DateTimePicker", "dtpMaintNextDue", 12, 278, 140, 23, {"Format": "DateTimePickerFormat.Short", "ShowCheckBox": "true"}),
    ("Label", "lblMaintMsg", 12, 312, None, None, {}),
    ("Button", "btnSaveMaintenance", 12, 340, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelMaintenance", 108, 340, 90, 28, {"Text": T("Cancel")}),
]

EVENTS = {
    "btnSaveMaintenance": [("Click", "btnSaveMaintenance_Click")],
    "btnCancelMaintenance": [("Click", "btnCancelMaintenance_Click")],
}
