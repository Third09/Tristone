# Spec for src/Company.OfficeApp/Pages/Receipts/ReceiptViewerForm.Designer.cs (pop-up window)
# picReceiptScan is moved inside pnlScan by the form's constructor (the panel scrolls when zoomed in).
NAMESPACE = "Company.OfficeApp.Pages.Receipts"
CLASS = "ReceiptViewerForm"
FORM = True
SIZE = (900, 630)
LOAD_EVENT = "ReceiptViewerForm_Load"

CONTROLS = [
    ("Panel", "pnlScan", 12, 12, 560, 560, {"AutoScroll": "true", "BorderStyle": "BorderStyle.FixedSingle"}),
    ("PictureBox", "picReceiptScan", 0, 0, 556, 556, {"SizeMode": "PictureBoxSizeMode.Zoom"}),
    ("Button", "btnZoomIn", 12, 584, 90, 28, {"Text": T("Zoom In")}),
    ("Button", "btnZoomOut", 108, 584, 90, 28, {"Text": T("Zoom Out")}),
    ("Button", "btnOpenOriginal", 204, 584, 110, 28, {"Text": T("Open Original")}),
    ("Label", "lblViewReceiptNo", 590, 12, None, None, {}),
    ("Label", "lblViewDate", 590, 36, None, None, {}),
    ("Label", "lblViewMerchant", 590, 60, None, None, {}),
    ("Label", "lblViewAmount", 590, 84, None, None, {}),
    ("Label", "lblViewCategory", 590, 108, None, None, {}),
    ("Label", "lblViewProject", 590, 132, None, None, {}),
    ("Label", "lblViewUploadedBy", 590, 156, None, None, {}),
    ("Label", "lblViewStatus", 590, 180, None, None, {}),
    ("Label", "lblViewCopy", 590, 204, None, None, {"MaximumSize": "new Size(290, 0)"}),
    ("Label", "lblViewReason", 590, 252, None, None, {"MaximumSize": "new Size(290, 0)"}),
    ("Label", "lblViewerMsg", 590, 540, None, None, {"MaximumSize": "new Size(290, 0)"}),
    ("Button", "btnPrintReceipt", 590, 584, 90, 28, {"Text": T("Print")}),
    ("Button", "btnCloseViewer", 686, 584, 90, 28, {"Text": T("Close"), "DialogResult": "DialogResult.Cancel"}),
]

GRID_COLUMNS = {}

EVENTS = {
    "btnZoomIn": [("Click", "btnZoomIn_Click")],
    "btnZoomOut": [("Click", "btnZoomOut_Click")],
    "btnOpenOriginal": [("Click", "btnOpenOriginal_Click")],
    "btnPrintReceipt": [("Click", "btnPrintReceipt_Click")],
}
