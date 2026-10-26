# Spec for src/Company.OfficeApp/Pages/Receipts/NeedsReviewTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Receipts"
CLASS = "NeedsReviewTab"
SIZE = (960, 440)
LOAD_EVENT = "NeedsReviewTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'

CONTROLS = [
    ("Label", "lblNeedsReviewInfo", 12, 15, None, None, {"Text": T("Expenses with no receipt. They count as expenses only after the Receipts Supervisor approves.")}),
    ("DataGridView", "dgvNeedsReview", 12, 40, 930, 300, {}),
    ("Button", "btnApproveNoReceipt", 12, 348, 80, 28, {"Text": T("Approve")}),
    ("Button", "btnRejectNoReceipt", 98, 348, 70, 28, {"Text": T("Reject")}),
    ("Button", "btnAttachLateReceipt", 174, 348, 120, 28, {"Text": T("Attach Receipt")}),
    ("Label", "lblApproverNote", 12, 384, None, None, {"Text": T("The Receipts Supervisor or an Admin approves these.")}),
    ("Label", "lblReviewMsg", 12, 408, None, None, {}),
]

GRID_COLUMNS = {
    "dgvNeedsReview": [
        (TB, "colDate", "Date", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colMerchant", "Merchant", "Paid To", {}),
        (TB, "colAmount", "Amount", "Amount", {"DefaultCellStyle": MONEY}),
        (TB, "colCategory", "Category", "Category", {}),
        (TB, "colProject", "ProjectOrOverhead", "Project / Overhead", {}),
        (TB, "colReason", "NoReceiptReason", "Reason", {}),
        (TB, "colBy", "UploadedBy", "Submitted By", {}),
        (TB, "colStatus", "Status", "Status", {}),
    ],
}

EVENTS = {
    "dgvNeedsReview": [("SelectionChanged", "dgvNeedsReview_SelectionChanged")],
    "btnApproveNoReceipt": [("Click", "btnApproveNoReceipt_Click")],
    "btnRejectNoReceipt": [("Click", "btnRejectNoReceipt_Click")],
    "btnAttachLateReceipt": [("Click", "btnAttachLateReceipt_Click")],
}
