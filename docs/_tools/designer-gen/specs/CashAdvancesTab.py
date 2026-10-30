# Spec for src/Company.OfficeApp/Pages/Hr/CashAdvancesTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Hr"
CLASS = "CashAdvancesTab"
SIZE = (960, 600)
LOAD_EVENT = "CashAdvancesTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'

CONTROLS = [
    ("Label", "lblCaFilter", 12, 15, None, None, {"Text": T("Show")}),
    ("ComboBox", "cboCashAdvanceFilter", 60, 12, 180, 23, DDL),
    ("DataGridView", "dgvCashAdvances", 12, 44, 930, 220, {}),
    ("Button", "btnNewCashAdvance", 12, 272, 150, 28, {"Text": T("New Cash Advance")}),
    ("Button", "btnApproveCashAdvance", 168, 272, 90, 28, {"Text": T("Approve")}),
    ("Button", "btnRejectCashAdvance", 264, 272, 90, 28, {"Text": T("Reject")}),
    ("Label", "lblApproverNote", 12, 308, None, None, {"Text": T("The HR Supervisor or an Admin approves cash advances.")}),
    ("Label", "lblCaMsg", 12, 332, None, None, {}),
    ("Label", "lblDeductions", 12, 360, None, None, {"Text": T("Deductions taken (selected cash advance)")}),
    ("DataGridView", "dgvCashAdvanceDeductions", 12, 380, 600, 200, {}),
]

GRID_COLUMNS = {
    "dgvCashAdvances": [
        (TB, "colCaDate", "GivenOn", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colCaName", "Name", "Name", {}),
        (TB, "colCaAmount", "Amount", "Amount", {"DefaultCellStyle": MONEY}),
        (TB, "colCaPerPayday", "PerPayday", "Deduct per Payday", {"DefaultCellStyle": MONEY}),
        (TB, "colCaBalance", "Balance", "Balance", {"DefaultCellStyle": MONEY}),
        (TB, "colCaStatus", "Status", "Status", {}),
        (TB, "colCaReason", "Reason", "Reason", {}),
    ],
    "dgvCashAdvanceDeductions": [
        (TB, "colDedPayDate", "PayDate", "Pay Date", {"DefaultCellStyle": DATE}),
        (TB, "colDedPeriod", "Period", "Period", {}),
        (TB, "colDedAmount", "Deducted", "Deducted", {"DefaultCellStyle": MONEY}),
        (TB, "colDedBalance", "BalanceAfter", "Balance After", {"DefaultCellStyle": MONEY}),
    ],
}

EVENTS = {
    "cboCashAdvanceFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "dgvCashAdvances": [("SelectionChanged", "dgvCashAdvances_SelectionChanged")],
    "btnNewCashAdvance": [("Click", "btnNewCashAdvance_Click")],
    "btnApproveCashAdvance": [("Click", "btnApproveCashAdvance_Click")],
    "btnRejectCashAdvance": [("Click", "btnRejectCashAdvance_Click")],
}
