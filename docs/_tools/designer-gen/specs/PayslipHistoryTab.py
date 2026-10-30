# Spec for src/Company.OfficeApp/Pages/Hr/PayslipHistoryTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Hr"
CLASS = "PayslipHistoryTab"
SIZE = (960, 520)
LOAD_EVENT = "PayslipHistoryTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'

CONTROLS = [
    ("Label", "lblPayslipSearch", 12, 15, None, None, {"Text": T("Search")}),
    ("TextBox", "txtPayslipSearch", 62, 12, 200, 23, {}),
    ("Label", "lblPayslipPeriod", 274, 15, None, None, {"Text": T("Payroll")}),
    ("ComboBox", "cboPayslipPeriod", 330, 12, 400, 23, DDL),
    ("DataGridView", "dgvPayslipHistory", 12, 44, 930, 380, {}),
    ("Button", "btnViewPayslip", 12, 432, 80, 28, {"Text": T("View")}),
    ("Button", "btnPrintPayslip", 98, 432, 80, 28, {"Text": T("Print")}),
    ("Button", "btnSavePayslipPdf", 184, 432, 110, 28, {"Text": T("Save as PDF")}),
    ("Label", "lblPayslipMsg", 12, 468, None, None, {"MaximumSize": "new Size(930, 0)"}),
]

GRID_COLUMNS = {
    "dgvPayslipHistory": [
        (TB, "colPsPayDate", "PayDate", "Pay Date", {"DefaultCellStyle": DATE}),
        (TB, "colPsPeriod", "Period", "Period", {}),
        (TB, "colPsName", "Name", "Name", {}),
        (TB, "colPsDays", "DaysPaid", "Days Paid", {}),
        (TB, "colPsGross", "GrossPay", "Gross Pay", {"DefaultCellStyle": MONEY}),
        (TB, "colPsNet", "NetPay", "Net Pay", {"DefaultCellStyle": MONEY}),
    ],
}

EVENTS = {
    "cboPayslipPeriod": [("SelectedIndexChanged", "Filter_Changed")],
    "dgvPayslipHistory": [("SelectionChanged", "dgvPayslipHistory_SelectionChanged"), ("CellDoubleClick", "dgvPayslipHistory_CellDoubleClick")],
    "btnViewPayslip": [("Click", "btnViewPayslip_Click")],
    "btnPrintPayslip": [("Click", "btnPrintPayslip_Click")],
    "btnSavePayslipPdf": [("Click", "btnSavePayslipPdf_Click")],
}
