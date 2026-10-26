# Spec for src/Company.OfficeApp/Pages/Hr/EmployeesTab.Designer.cs  (T, DDL, TB come from gen_designer.py)
NAMESPACE = "Company.OfficeApp.Pages.Hr"
CLASS = "EmployeesTab"
SIZE = (960, 700)
LOAD_EVENT = "EmployeesTab_Load"

# (type, name, x, y, width, height, extra properties); width None = AutoSize for labels/checkboxes
CONTROLS = [
    ("Label", "lblSearch", 12, 15, None, None, {"Text": T("Search")}),
    ("TextBox", "txtEmployeeSearch", 62, 12, 180, 23, {}),
    ("Label", "lblStatusFilter", 256, 15, None, None, {"Text": T("Status")}),
    ("ComboBox", "cboEmployeeStatusFilter", 302, 12, 100, 23, DDL),
    ("Label", "lblScheduleFilter", 416, 15, None, None, {"Text": T("Pay schedule")}),
    ("ComboBox", "cboPayScheduleFilter", 500, 12, 100, 23, DDL),
    ("DataGridView", "dgvEmployees", 12, 44, 930, 200, {}),
    ("Button", "btnAddEmployee", 12, 252, 110, 28, {"Text": T("Add Employee")}),
    ("Button", "btnMarkInactive", 128, 252, 110, 28, {"Text": T("Mark Inactive")}),
    ("Button", "btnReactivateEmployee", 244, 252, 100, 28, {"Text": T("Reactivate")}),
    ("Button", "btnExportEmployees", 350, 252, 90, 28, {"Text": T("Export")}),
    ("Label", "lblFormMode", 12, 296, None, None, {"Text": T("New employee")}),
    # personal details (left)
    ("Label", "lblEmployeeName", 12, 320, None, None, {"Text": T("Full name")}),
    ("TextBox", "txtEmployeeName", 12, 338, 300, 23, {}),
    ("Label", "lblEmployeeContact", 12, 368, None, None, {"Text": T("Contact number")}),
    ("TextBox", "txtEmployeeContact", 12, 386, 300, 23, {}),
    ("Label", "lblEmployeeAddress", 12, 416, None, None, {"Text": T("Address")}),
    ("TextBox", "txtEmployeeAddress", 12, 434, 300, 46, {"Multiline": "true"}),
    ("Label", "lblEmployeePosition", 12, 488, None, None, {"Text": T("Position (e.g. Mason, HR Staff)")}),
    ("TextBox", "txtEmployeePosition", 12, 506, 300, 23, {}),
    ("Label", "lblEmployeeDepartment", 12, 536, None, None, {"Text": T("Department (Site for site workers)")}),
    ("ComboBox", "cboEmployeeDepartment", 12, 554, 200, 23, DDL),
    ("Label", "lblPayScheduleValue", 12, 584, None, None, {"Text": T("Pay schedule: (choose a department)")}),
    # pay details (middle)
    ("Label", "lblPayGrade", 340, 320, None, None, {"Text": T("Pay grade (optional)")}),
    ("TextBox", "txtPayGrade", 340, 338, 200, 23, {}),
    ("Label", "lblDailyRate", 340, 368, None, None, {"Text": T("Daily rate (pesos)")}),
    ("NumericUpDown", "numDailyRate", 340, 386, 140, 23, {"DecimalPlaces": "2", "ThousandsSeparator": "true"}),
    ("Label", "lblRateEffective", 340, 416, None, None, {"Text": T("Rate applies from")}),
    ("DateTimePicker", "dtpRateEffective", 340, 434, 140, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblBiometricId", 340, 464, None, None, {"Text": T("Biometric ID (from the scanner)")}),
    ("TextBox", "txtBiometricId", 340, 482, 140, 23, {}),
    ("Label", "lblRateHistory", 340, 512, None, None, {"Text": T("Daily rate history")}),
    ("DataGridView", "dgvRateHistory", 340, 530, 280, 90, {}),
    # government deductions (right)
    ("CheckBox", "chkGovDeductions", 650, 320, None, None, {"Text": T("Government deductions (tax, SSS, PhilHealth, Pag-IBIG)")}),
    ("Label", "lblSssNo", 650, 348, None, None, {"Text": T("SSS number (optional)")}),
    ("TextBox", "txtSssNo", 650, 366, 200, 23, {}),
    ("Label", "lblPhilHealthNo", 650, 396, None, None, {"Text": T("PhilHealth number (optional)")}),
    ("TextBox", "txtPhilHealthNo", 650, 414, 200, 23, {}),
    ("Label", "lblPagIbigNo", 650, 444, None, None, {"Text": T("Pag-IBIG number (optional)")}),
    ("TextBox", "txtPagIbigNo", 650, 462, 200, 23, {}),
    ("Label", "lblEmployeeTin", 650, 492, None, None, {"Text": T("TIN (optional)")}),
    ("TextBox", "txtEmployeeTin", 650, 510, 200, 23, {}),
    # bottom
    ("Label", "lblEmployeeFormMsg", 12, 634, None, None, {}),
    ("Button", "btnSaveEmployee", 12, 656, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelEmployee", 108, 656, 90, 28, {"Text": T("Cancel")}),
]

MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2", NullValue = "-" }'
GRID_COLUMNS = {
    "dgvEmployees": [
        (TB, "colName", "FullName", "Name", {}),
        (TB, "colPosition", "Position", "Position", {}),
        (TB, "colDepartment", "Department", "Department", {}),
        (TB, "colPaySchedule", "PaySchedule", "Pay Schedule", {}),
        (TB, "colCurrentRate", "CurrentRate", "Daily Rate", {"DefaultCellStyle": MONEY}),
        (TB, "colBiometricId", "BiometricId", "Biometric ID", {}),
        (TB, "colStatus", "Status", "Status", {}),
    ],
    "dgvRateHistory": [
        (TB, "colRateFrom", "EffectiveFrom", "Applies From", {"DefaultCellStyle": 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'}),
        (TB, "colRateAmount", "DailyRate", "Daily Rate", {"DefaultCellStyle": MONEY}),
    ],
}

EVENTS = {
    "txtEmployeeSearch": [("TextChanged", "Filter_Changed")],
    "cboEmployeeStatusFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "cboPayScheduleFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "dgvEmployees": [("SelectionChanged", "dgvEmployees_SelectionChanged")],
    "btnAddEmployee": [("Click", "btnAddEmployee_Click")],
    "btnMarkInactive": [("Click", "btnMarkInactive_Click")],
    "btnReactivateEmployee": [("Click", "btnReactivateEmployee_Click")],
    "btnExportEmployees": [("Click", "btnExportEmployees_Click")],
    "cboEmployeeDepartment": [("SelectedIndexChanged", "cboEmployeeDepartment_SelectedIndexChanged")],
    "btnSaveEmployee": [("Click", "btnSaveEmployee_Click")],
    "btnCancelEmployee": [("Click", "btnCancelEmployee_Click")],
}
