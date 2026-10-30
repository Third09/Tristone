# Spec for src/Company.OfficeApp/Pages/Hr/PayrollTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Hr"
CLASS = "PayrollTab"
SIZE = (960, 600)
LOAD_EVENT = "PayrollTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
DAYS = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "0.#" }'

CONTROLS = [
    ("Label", "lblPayGroup", 12, 15, None, None, {"Text": T("Pay group")}),
    ("ComboBox", "cboPayGroup", 90, 12, 150, 23, DDL),
    ("Label", "lblPayPeriod", 252, 15, None, None, {"Text": T("Pay period")}),
    ("ComboBox", "cboPayPeriod", 330, 12, 330, 23, DDL),
    ("Label", "lblPayrollStatus", 672, 15, None, None, {"Text": T("")}),
    ("Label", "lblPayrollWarnings", 12, 44, None, None, {"Text": T(""), "MaximumSize": "new Size(930, 0)"}),
    ("Button", "btnComputePayroll", 12, 100, 130, 28, {"Text": T("Compute Payroll")}),
    ("Button", "btnFinalizePayroll", 148, 100, 130, 28, {"Text": T("Finalize Payroll")}),
    ("DataGridView", "dgvPayroll", 12, 136, 930, 330, {}),
    ("Label", "lblPayrollTotal", 12, 474, None, None, {"Text": T("")}),
    ("Button", "btnPrintPayslips", 12, 498, 120, 28, {"Text": T("Print Payslips")}),
    ("Button", "btnSavePayslipsPdf", 138, 498, 150, 28, {"Text": T("Save Payslips as PDF")}),
    ("Button", "btnExportPayroll", 294, 498, 80, 28, {"Text": T("Export")}),
    ("Label", "lblApproverNote", 12, 534, None, None, {"Text": T("The HR Supervisor or an Admin finalizes payroll.")}),
    ("Label", "lblPayrollMsg", 12, 558, None, None, {"MaximumSize": "new Size(930, 0)"}),
]

GRID_COLUMNS = {
    "dgvPayroll": [
        (TB, "colPayName", "Name", "Name", {}),
        (TB, "colPayDays", "DaysPaid", "Days Paid", {"DefaultCellStyle": DAYS}),
        (TB, "colPayGross", "GrossPay", "Gross Pay", {"DefaultCellStyle": MONEY}),
        (TB, "colPayLate", "LateDeduction", "Late Deduction", {"DefaultCellStyle": MONEY}),
        (TB, "colPayOt", "OtPay", "Overtime Pay", {"DefaultCellStyle": MONEY}),
        (TB, "colPayCa", "CashAdvance", "Cash Advance", {"DefaultCellStyle": MONEY}),
        (TB, "colPayGov", "GovDeductions", "Gov. Deductions", {"DefaultCellStyle": MONEY}),
        (TB, "colPayNet", "NetPay", "Net Pay", {"DefaultCellStyle": MONEY}),
    ],
}

EVENTS = {
    "cboPayGroup": [("SelectedIndexChanged", "cboPayGroup_SelectedIndexChanged")],
    "cboPayPeriod": [("SelectedIndexChanged", "cboPayPeriod_SelectedIndexChanged")],
    "btnComputePayroll": [("Click", "btnComputePayroll_Click")],
    "btnFinalizePayroll": [("Click", "btnFinalizePayroll_Click")],
    "dgvPayroll": [("CellDoubleClick", "dgvPayroll_CellDoubleClick")],
    "btnPrintPayslips": [("Click", "btnPrintPayslips_Click")],
    "btnSavePayslipsPdf": [("Click", "btnSavePayslipsPdf_Click")],
    "btnExportPayroll": [("Click", "btnExportPayroll_Click")],
}
