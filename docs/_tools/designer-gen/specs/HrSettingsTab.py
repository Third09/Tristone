# Spec for src/Company.OfficeApp/Pages/Hr/HrSettingsTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Hr"
CLASS = "HrSettingsTab"
SIZE = (960, 660)
LOAD_EVENT = "HrSettingsTab_Load"
TIME = {"Format": "DateTimePickerFormat.Custom", "CustomFormat": T("h:mm tt"), "ShowUpDown": "true"}
MONEY = {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "99999999m"}
PCT = {"DecimalPlaces": "2", "Maximum": "50m"}
DATE = 'new DataGridViewCellStyle { Format = "ddd, MMM d, yyyy" }'

CONTROLS = [
    ("Label", "lblWorkSchedule", 12, 12, None, None, {"Text": T("Work schedule")}),
    ("Label", "lblWorkStart", 12, 40, None, None, {"Text": T("Work start")}),
    ("DateTimePicker", "dtpWorkStart", 140, 36, 110, 23, TIME),
    ("Label", "lblWorkEnd", 12, 70, None, None, {"Text": T("Work end")}),
    ("DateTimePicker", "dtpWorkEnd", 140, 66, 110, 23, TIME),
    ("Label", "lblBreakStart", 12, 100, None, None, {"Text": T("Break start")}),
    ("DateTimePicker", "dtpBreakStart", 140, 96, 110, 23, TIME),
    ("Label", "lblBreakEnd", 12, 130, None, None, {"Text": T("Break end")}),
    ("DateTimePicker", "dtpBreakEnd", 140, 126, 110, 23, TIME),
    ("Label", "lblGraceMinutes", 12, 160, None, None, {"Text": T("Grace minutes")}),
    ("NumericUpDown", "numGraceMinutes", 140, 156, 70, 23, {"Maximum": "120m"}),
    ("Label", "lblWorkDays", 12, 190, None, None, {"Text": T("Work days")}),
    ("CheckedListBox", "clbWorkDays", 140, 186, 140, 130, {"CheckOnClick": "true"}),
    ("Label", "lblOvertimeRate", 12, 326, None, None, {"Text": T("Overtime per hour")}),
    ("NumericUpDown", "numOvertimeRate", 140, 322, 100, 23, MONEY),
    ("Label", "lblOtMinimum", 12, 356, None, None, {"Text": T("Overtime from (min)")}),
    ("NumericUpDown", "numOtMinimum", 140, 352, 70, 23, {"Maximum": "240m"}),
    ("Label", "lblGovTitle", 320, 12, None, None, {"Text": T("Government deductions (only for employees with the switch on)")}),
    ("Label", "lblWorkDaysPerMonth", 320, 40, None, None, {"Text": T("Work days per month")}),
    ("NumericUpDown", "numWorkDaysPerMonth", 520, 36, 70, 23, {"Minimum": "1m", "Maximum": "31m", "Value": "26m"}),
    ("Label", "lblSssRate", 320, 70, None, None, {"Text": T("SSS employee share %")}),
    ("NumericUpDown", "numSssRate", 520, 66, 70, 23, PCT),
    ("Label", "lblSssMin", 320, 100, None, None, {"Text": T("SSS salary credit, minimum")}),
    ("NumericUpDown", "numSssMin", 520, 96, 100, 23, MONEY),
    ("Label", "lblSssMax", 320, 130, None, None, {"Text": T("SSS salary credit, maximum")}),
    ("NumericUpDown", "numSssMax", 520, 126, 100, 23, MONEY),
    ("Label", "lblPhilHealthRate", 320, 160, None, None, {"Text": T("PhilHealth employee share %")}),
    ("NumericUpDown", "numPhilHealthRate", 520, 156, 70, 23, PCT),
    ("Label", "lblPhilHealthFloor", 320, 190, None, None, {"Text": T("PhilHealth salary floor")}),
    ("NumericUpDown", "numPhilHealthFloor", 520, 186, 100, 23, MONEY),
    ("Label", "lblPhilHealthCeiling", 320, 220, None, None, {"Text": T("PhilHealth salary ceiling")}),
    ("NumericUpDown", "numPhilHealthCeiling", 520, 216, 100, 23, MONEY),
    ("Label", "lblPagIbigRate", 320, 250, None, None, {"Text": T("Pag-IBIG employee share %")}),
    ("NumericUpDown", "numPagIbigRate", 520, 246, 70, 23, PCT),
    ("Label", "lblPagIbigCap", 320, 280, None, None, {"Text": T("Pag-IBIG salary cap")}),
    ("NumericUpDown", "numPagIbigCap", 520, 276, 100, 23, MONEY),
    ("CheckBox", "chkWithholdingTax", 320, 310, None, None, {"Text": T("Withhold income tax (TRAIN table)")}),
    ("Label", "lblGovNote", 320, 338, None, None, {"Text": T("Defaults are the 2025 employee shares. Check them with your accountant and change them here when the government updates them."), "MaximumSize": "new Size(400, 0)"}),
    ("Label", "lblSettingsMsg", 12, 392, None, None, {"MaximumSize": "new Size(900, 0)"}),
    ("Button", "btnSaveHrSettings", 12, 414, 120, 28, {"Text": T("Save Settings")}),
    ("Label", "lblHolidays", 12, 460, None, None, {"Text": T("Holidays (no one is marked absent; paydays move to the day before)")}),
    ("DataGridView", "dgvHolidays", 12, 482, 420, 140, {}),
    ("Label", "lblHolidayDate", 450, 482, None, None, {"Text": T("Date")}),
    ("DateTimePicker", "dtpHolidayDate", 450, 500, 120, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblHolidayName", 450, 530, None, None, {"Text": T("Name")}),
    ("TextBox", "txtHolidayName", 450, 548, 200, 23, {}),
    ("Label", "lblHolidayKind", 450, 578, None, None, {"Text": T("Kind")}),
    ("ComboBox", "cboHolidayKind", 450, 596, 120, 23, DDL),
    ("Button", "btnAddHoliday", 670, 546, 110, 28, {"Text": T("Add Holiday")}),
    ("Button", "btnDeleteHoliday", 670, 580, 110, 28, {"Text": T("Delete Holiday")}),
    ("Label", "lblHolidayMsg", 12, 630, None, None, {}),
]

GRID_COLUMNS = {
    "dgvHolidays": [
        (TB, "colHolidayDate", "HolidayDate", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colHolidayName", "Name", "Name", {}),
        (TB, "colHolidayKind", "Kind", "Kind", {}),
    ],
}

EVENTS = {
    "btnSaveHrSettings": [("Click", "btnSaveHrSettings_Click")],
    "dgvHolidays": [("SelectionChanged", "dgvHolidays_SelectionChanged")],
    "btnAddHoliday": [("Click", "btnAddHoliday_Click")],
    "btnDeleteHoliday": [("Click", "btnDeleteHoliday_Click")],
}
