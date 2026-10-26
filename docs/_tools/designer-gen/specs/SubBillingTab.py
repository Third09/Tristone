# Spec for src/Company.OfficeApp/Pages/Subcontractors/SubBillingTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Subcontractors"
CLASS = "SubBillingTab"
SIZE = (960, 580)
LOAD_EVENT = "SubBillingTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
PCT = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "0.##\'%\'", NullValue = "" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'

CONTROLS = [
    ("Label", "lblBillingJob", 12, 15, None, None, {"Text": T("Job")}),
    ("ComboBox", "cboBillingJob", 50, 12, 420, 23, DDL),
    ("Label", "lblBillContract", 12, 46, None, None, {"Text": T("Contract:")}),
    ("Label", "lblBillPaid", 210, 46, None, None, {"Text": T("Paid so far:")}),
    ("Label", "lblBillRetention", 408, 46, None, None, {"Text": T("Retention held:")}),
    ("Label", "lblBillRemaining", 606, 46, None, None, {"Text": T("Remaining:")}),
    ("Label", "lblBillProgress", 804, 46, None, None, {"Text": T("Done:")}),
    ("DataGridView", "dgvBillingStages", 12, 70, 930, 160, {}),
    ("Button", "btnMarkStageReached", 12, 238, 140, 28, {"Text": T("Mark Stage Reached")}),
    ("Button", "btnPrepareVoucher", 158, 238, 120, 28, {"Text": T("Prepare Voucher")}),
    ("Button", "btnApproveVoucher", 284, 238, 120, 28, {"Text": T("Approve Voucher")}),
    ("Button", "btnRejectVoucher", 410, 238, 110, 28, {"Text": T("Reject Voucher")}),
    ("Button", "btnPrintVoucher", 526, 238, 100, 28, {"Text": T("Print Voucher")}),
    ("Button", "btnReleaseRetention", 632, 238, 130, 28, {"Text": T("Release Retention")}),
    ("Label", "lblApproverNote", 12, 272, None, None, {"Text": T("The Subcontractors Supervisor or an Admin approves vouchers.")}),
    ("Label", "lblPayoutTitle", 12, 300, None, None, {"Text": T("Record a payout (select an approved voucher above)")}),
    ("Label", "lblPayoutDate", 12, 322, None, None, {"Text": T("Date paid")}),
    ("DateTimePicker", "dtpPayoutDate", 12, 340, 125, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblPayoutAmount", 147, 322, None, None, {"Text": T("Amount")}),
    ("NumericUpDown", "numPayoutAmount", 147, 340, 130, 23, {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "999999999999m"}),
    ("Label", "lblPayoutMethod", 287, 322, None, None, {"Text": T("Method")}),
    ("ComboBox", "cboPayoutMethod", 287, 340, 120, 23, DDL),
    ("Label", "lblPayoutReference", 417, 322, None, None, {"Text": T("Reference no.")}),
    ("TextBox", "txtPayoutReference", 417, 340, 160, 23, {}),
    ("Button", "btnRecordPayout", 587, 338, 110, 28, {"Text": T("Record Payout")}),
    ("Label", "lblBillMsg", 12, 372, None, None, {}),
    ("Label", "lblPayouts", 12, 398, None, None, {"Text": T("Payouts")}),
    ("DataGridView", "dgvPayouts", 12, 418, 930, 140, {}),
]

GRID_COLUMNS = {
    "dgvBillingStages": [
        (TB, "colStage", "Name", "Stage", {}),
        (TB, "colPct", "PercentText", "%", {}),
        (TB, "colAmount", "Amount", "Amount", {"DefaultCellStyle": MONEY}),
        (TB, "colRetention", "Retention", "Retention", {"DefaultCellStyle": MONEY}),
        (TB, "colPayable", "PayableNow", "Payable Now", {"DefaultCellStyle": MONEY}),
        (TB, "colVoucher", "VoucherNo", "Voucher", {}),
        (TB, "colStatus", "Status", "Status", {}),
    ],
    "dgvPayouts": [
        (TB, "colPayDate", "PaidOn", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colPayFor", "For", "Stage", {}),
        (TB, "colPayAmount", "Amount", "Amount", {"DefaultCellStyle": MONEY}),
        (TB, "colPayMethod", "Method", "Method", {}),
        (TB, "colPayRef", "ReferenceNo", "Reference", {}),
    ],
}

EVENTS = {
    "cboBillingJob": [("SelectedIndexChanged", "cboBillingJob_SelectedIndexChanged")],
    "dgvBillingStages": [("SelectionChanged", "dgvBillingStages_SelectionChanged")],
    "btnMarkStageReached": [("Click", "btnMarkStageReached_Click")],
    "btnPrepareVoucher": [("Click", "btnPrepareVoucher_Click")],
    "btnApproveVoucher": [("Click", "btnApproveVoucher_Click")],
    "btnRejectVoucher": [("Click", "btnRejectVoucher_Click")],
    "btnPrintVoucher": [("Click", "btnPrintVoucher_Click")],
    "btnReleaseRetention": [("Click", "btnReleaseRetention_Click")],
    "btnRecordPayout": [("Click", "btnRecordPayout_Click")],
}
