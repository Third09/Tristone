# Spec for src/Company.OfficeApp/Pages/DashboardPage.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages"
CLASS = "DashboardPage"
SIZE = (960, 756)
LOAD_EVENT = "DashboardPage_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
PCT = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "0.0\'%\'", NullValue = "-" }'
STAMP = 'new DataGridViewCellStyle { Format = "MMM d, yyyy h:mm tt" }'
CARD = {"BorderStyle": "BorderStyle.FixedSingle"}

CONTROLS = [
    ("Label", "lblPeriod", 12, 15, None, None, {"Text": T("Period")}),
    ("ComboBox", "cboPeriod", 60, 12, 130, 23, DDL),
    ("Label", "lblFrom", 202, 15, None, None, {"Text": T("From")}),
    ("DateTimePicker", "dtpFrom", 240, 12, 115, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblTo", 365, 15, None, None, {"Text": T("To")}),
    ("DateTimePicker", "dtpTo", 390, 12, 115, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblDepartment", 517, 15, None, None, {"Text": T("Department")}),
    ("ComboBox", "cboDepartment", 600, 12, 170, 23, DDL),
    ("Button", "btnRefreshDashboard", 782, 10, 90, 28, {"Text": T("Refresh")}),
    # KPI cards
    ("Panel", "pnlKpiCompleted", 12, 48, 225, 64, CARD),
    ("Label", "lblKpiCompletedTitle", 8, 8, None, None, {"Text": T("Completed Projects"), "PARENT": "pnlKpiCompleted"}),
    ("Label", "lblKpiCompleted", 8, 32, None, None, {"Text": T("-"), "PARENT": "pnlKpiCompleted"}),
    ("Panel", "pnlKpiRevenue", 245, 48, 225, 64, CARD),
    ("Label", "lblKpiRevenueTitle", 8, 8, None, None, {"Text": T("Gross Revenue"), "PARENT": "pnlKpiRevenue"}),
    ("Label", "lblKpiRevenue", 8, 32, None, None, {"Text": T("-"), "PARENT": "pnlKpiRevenue"}),
    ("Panel", "pnlKpiExpenses", 478, 48, 225, 64, CARD),
    ("Label", "lblKpiExpensesTitle", 8, 8, None, None, {"Text": T("Operational Expenses"), "PARENT": "pnlKpiExpenses"}),
    ("Label", "lblKpiExpenses", 8, 32, None, None, {"Text": T("-"), "PARENT": "pnlKpiExpenses"}),
    ("Panel", "pnlKpiNet", 711, 48, 225, 64, CARD),
    ("Label", "lblKpiNetTitle", 8, 8, None, None, {"Text": T("Net Revenue"), "PARENT": "pnlKpiNet"}),
    ("Label", "lblKpiNet", 8, 32, None, None, {"Text": T("-"), "PARENT": "pnlKpiNet"}),
    ("Label", "lblKpiBreakdown", 12, 120, None, None, {"Text": T(""), "MaximumSize": "new Size(930, 0)"}),
    # chart and top projects
    ("Label", "lblChartTitle", 12, 168, None, None, {"Text": T("Project status (all projects, today)")}),
    ("ScottPlot.WinForms.FormsPlot", "chartProjectStatus", 12, 188, 440, 260, {}),
    ("Label", "lblTopProjects", 470, 168, None, None, {"Text": T("Top performing projects (active in the period; click a column title to sort)")}),
    ("DataGridView", "dgvTopProjects", 470, 188, 472, 260, {}),
    ("Button", "btnExportTopProjects", 470, 454, 90, 28, {"Text": T("Export")}),
    # log history (Admin only)
    ("Label", "lblLogTitle", 12, 494, None, None, {"Text": T("Log history")}),
    ("Label", "lblLogType", 12, 520, None, None, {"Text": T("Activity")}),
    ("ComboBox", "cboLogType", 70, 517, 170, 23, DDL),
    ("Label", "lblLogSearch", 252, 520, None, None, {"Text": T("Search")}),
    ("TextBox", "txtLogSearch", 302, 517, 200, 23, {}),
    ("Button", "btnExportLogs", 514, 515, 90, 28, {"Text": T("Export")}),
    ("DataGridView", "dgvLogHistory", 12, 548, 930, 190, {}),
]

GRID_COLUMNS = {
    "dgvTopProjects": [
        (TB, "colTopProject", "Project", "Project", {"SortMode": "DataGridViewColumnSortMode.Programmatic"}),
        (TB, "colTopRevenue", "Revenue", "Revenue", {"DefaultCellStyle": MONEY, "SortMode": "DataGridViewColumnSortMode.Programmatic"}),
        (TB, "colTopExpenses", "Expenses", "Expenses", {"DefaultCellStyle": MONEY, "SortMode": "DataGridViewColumnSortMode.Programmatic"}),
        (TB, "colTopProfit", "Profit", "Profit", {"DefaultCellStyle": MONEY, "SortMode": "DataGridViewColumnSortMode.Programmatic"}),
        (TB, "colTopMargin", "MarginPercent", "Margin %", {"DefaultCellStyle": PCT, "SortMode": "DataGridViewColumnSortMode.Programmatic"}),
    ],
    "dgvLogHistory": [
        (TB, "colLogWhen", "When", "Date & Time", {"DefaultCellStyle": STAMP}),
        (TB, "colLogName", "Name", "Name", {}),
        (TB, "colLogDept", "Department", "Department", {}),
        (TB, "colLogActivity", "Activity", "Activity", {}),
        (TB, "colLogDetails", "Details", "Details", {}),
    ],
}

EVENTS = {
    "cboPeriod": [("SelectedIndexChanged", "cboPeriod_SelectedIndexChanged")],
    "btnRefreshDashboard": [("Click", "btnRefreshDashboard_Click")],
    "dgvTopProjects": [("ColumnHeaderMouseClick", "dgvTopProjects_ColumnHeaderMouseClick")],
    "btnExportTopProjects": [("Click", "btnExportTopProjects_Click")],
    "cboLogType": [("SelectedIndexChanged", "cboLogType_SelectedIndexChanged")],
    "btnExportLogs": [("Click", "btnExportLogs_Click")],
}
