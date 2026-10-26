# Spec for src/Company.OfficeApp/Pages/Shared/EditHistoryTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Shared"
CLASS = "EditHistoryTab"
SIZE = (960, 520)
LOAD_EVENT = "EditHistoryTab_Load"

CONTROLS = [
    ("Label", "lblHistFrom", 12, 15, None, None, {"Text": T("From")}),
    ("DateTimePicker", "dtpHistFrom", 52, 12, 120, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblHistTo", 184, 15, None, None, {"Text": T("To")}),
    ("DateTimePicker", "dtpHistTo", 210, 12, 120, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblHistUser", 344, 15, None, None, {"Text": T("Person")}),
    ("ComboBox", "cboHistUser", 394, 12, 180, 23, DDL),
    ("Label", "lblHistSearch", 588, 15, None, None, {"Text": T("Search record")}),
    ("TextBox", "txtHistSearch", 676, 12, 160, 23, {}),
    ("DataGridView", "dgvEditHistory", 12, 44, 930, 420, {}),
    ("Label", "lblHistCount", 12, 476, None, None, {"Text": T("0 changes")}),
    ("Button", "btnExportHistory", 852, 472, 90, 28, {"Text": T("Export")}),
]

DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy h:mm tt" }'
GRID_COLUMNS = {
    "dgvEditHistory": [
        (TB, "colChangedAt", "ChangedAt", "Date & Time", {"DefaultCellStyle": DATE}),
        (TB, "colWho", "Who", "Who", {}),
        (TB, "colAction", "Action", "Action", {}),
        (TB, "colRecord", "Record", "Record", {}),
        (TB, "colField", "Field", "Field", {}),
        (TB, "colOld", "OldValue", "Old Value", {}),
        (TB, "colNew", "NewValue", "New Value", {}),
    ],
}

EVENTS = {
    "dtpHistFrom": [("ValueChanged", "Filter_Changed")],
    "dtpHistTo": [("ValueChanged", "Filter_Changed")],
    "cboHistUser": [("SelectedIndexChanged", "Filter_Changed")],
    "btnExportHistory": [("Click", "btnExportHistory_Click")],
}
