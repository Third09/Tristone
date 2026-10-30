# Spec for src/Company.OfficeApp/Pages/Hr/AttendanceTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Hr"
CLASS = "AttendanceTab"
SIZE = (960, 610)
LOAD_EVENT = "AttendanceTab_Load"
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'
TIME = 'new DataGridViewCellStyle { Format = "h:mm tt", NullValue = "" }'
NUM = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#", NullValue = "" }'

CONTROLS = [
    ("Button", "btnUploadBiometrics", 12, 10, 160, 28, {"Text": T("Upload Scanner File")}),
    ("Label", "lblImportSummary", 180, 16, None, None, {"Text": T("No scanner file uploaded yet.")}),
    ("Button", "btnViewImportErrors", 12, 42, 160, 28, {"Text": T("View Rejected Rows")}),
    ("Label", "lblAttendanceFrom", 12, 83, None, None, {"Text": T("From")}),
    ("DateTimePicker", "dtpAttendanceFrom", 52, 80, 115, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblAttendanceTo", 177, 83, None, None, {"Text": T("To")}),
    ("DateTimePicker", "dtpAttendanceTo", 203, 80, 115, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblAttendanceFilter", 330, 83, None, None, {"Text": T("Show")}),
    ("ComboBox", "cboAttendanceFilter", 372, 80, 200, 23, DDL),
    ("Label", "lblAttendanceSearch", 584, 83, None, None, {"Text": T("Search")}),
    ("TextBox", "txtAttendanceSearch", 634, 80, 180, 23, {}),
    ("DataGridView", "dgvAttendance", 12, 112, 930, 380, {}),
    ("Label", "lblAttendanceCount", 12, 500, None, None, {"Text": T("")}),
    ("Button", "btnFixScan", 12, 522, 130, 28, {"Text": T("Fix Missing Scan")}),
    ("Button", "btnApproveOvertime", 148, 522, 130, 28, {"Text": T("Approve Overtime")}),
    ("Button", "btnRejectOvertime", 284, 522, 120, 28, {"Text": T("Reject Overtime")}),
    ("Button", "btnExportAttendance", 410, 522, 80, 28, {"Text": T("Export")}),
    ("Label", "lblApproverNote", 12, 558, None, None, {"Text": T("The HR Supervisor or an Admin approves overtime.")}),
    ("Label", "lblAttendanceMsg", 12, 582, None, None, {}),
]

GRID_COLUMNS = {
    "dgvAttendance": [
        (TB, "colDate", "Date", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colDay", "Day", "Day", {}),
        (TB, "colName", "Name", "Name", {}),
        (TB, "colTimeIn", "TimeIn", "Time In", {"DefaultCellStyle": TIME}),
        (TB, "colBreakOut", "BreakOut", "Break Out", {"DefaultCellStyle": TIME}),
        (TB, "colBreakIn", "BreakIn", "Break In", {"DefaultCellStyle": TIME}),
        (TB, "colTimeOut", "TimeOut", "Time Out", {"DefaultCellStyle": TIME}),
        (TB, "colLate", "LateMinutes", "Late (min)", {"DefaultCellStyle": NUM}),
        (TB, "colEarly", "EarlyMinutes", "Early (min)", {"DefaultCellStyle": NUM}),
        (TB, "colOvertime", "Overtime", "Overtime", {}),
        (TB, "colStatus", "Status", "Status", {}),
        (TB, "colFixed", "Fixed", "Fixed", {}),
    ],
}

EVENTS = {
    "btnUploadBiometrics": [("Click", "btnUploadBiometrics_Click")],
    "btnViewImportErrors": [("Click", "btnViewImportErrors_Click")],
    "dtpAttendanceFrom": [("ValueChanged", "Filter_Changed")],
    "dtpAttendanceTo": [("ValueChanged", "Filter_Changed")],
    "cboAttendanceFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "dgvAttendance": [("SelectionChanged", "dgvAttendance_SelectionChanged"), ("CellFormatting", "dgvAttendance_CellFormatting"),
                      ("CellDoubleClick", "dgvAttendance_CellDoubleClick")],
    "btnFixScan": [("Click", "btnFixScan_Click")],
    "btnApproveOvertime": [("Click", "btnApproveOvertime_Click")],
    "btnRejectOvertime": [("Click", "btnRejectOvertime_Click")],
    "btnExportAttendance": [("Click", "btnExportAttendance_Click")],
}
