# Spec for src/Company.OfficeApp/Pages/Hr/FixScanForm.Designer.cs (pop-up window)
NAMESPACE = "Company.OfficeApp.Pages.Hr"
CLASS = "FixScanForm"
FORM = True
SIZE = (420, 340)
LOAD_EVENT = "FixScanForm_Load"
TIME = {"Format": "DateTimePickerFormat.Custom", "CustomFormat": T("h:mm tt"), "ShowUpDown": "true", "ShowCheckBox": "true"}

CONTROLS = [
    ("Label", "lblFixEmployeeDate", 12, 12, None, None, {"Text": T("")}),
    ("Label", "lblFixHint", 12, 34, None, None, {"Text": T("Tick each time that is known.")}),
    ("Label", "lblFixTimeIn", 12, 64, None, None, {"Text": T("Time in")}),
    ("DateTimePicker", "dtpFixTimeIn", 120, 60, 130, 23, TIME),
    ("Label", "lblFixBreakOut", 12, 94, None, None, {"Text": T("Break out")}),
    ("DateTimePicker", "dtpFixBreakOut", 120, 90, 130, 23, TIME),
    ("Label", "lblFixBreakIn", 12, 124, None, None, {"Text": T("Break in")}),
    ("DateTimePicker", "dtpFixBreakIn", 120, 120, 130, 23, TIME),
    ("Label", "lblFixTimeOut", 12, 154, None, None, {"Text": T("Time out")}),
    ("DateTimePicker", "dtpFixTimeOut", 120, 150, 130, 23, TIME),
    ("Label", "lblFixReason", 12, 186, None, None, {"Text": T("Reason (required)")}),
    ("TextBox", "txtFixReason", 12, 204, 396, 56, {"Multiline": "true", "ScrollBars": "ScrollBars.Vertical"}),
    ("Label", "lblFixMsg", 12, 268, None, None, {"MaximumSize": "new Size(396, 0)"}),
    ("Button", "btnSaveFix", 222, 300, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelFix", 318, 300, 90, 28, {"Text": T("Cancel"), "DialogResult": "DialogResult.Cancel"}),
]

GRID_COLUMNS = {}

EVENTS = {
    "btnSaveFix": [("Click", "btnSaveFix_Click")],
}
