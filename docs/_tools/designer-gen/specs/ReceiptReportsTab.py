# Spec for src/Company.OfficeApp/Pages/Receipts/ReceiptReportsTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Receipts"
CLASS = "ReceiptReportsTab"
SIZE = (960, 200)
LOAD_EVENT = "ReceiptReportsTab_Load"

CONTROLS = [
    ("Label", "lblReportType", 12, 15, None, None, {"Text": T("Report")}),
    ("ComboBox", "cboReportType", 100, 12, 240, 23, DDL),
    ("Label", "lblReportProject", 12, 45, None, None, {"Text": T("Project")}),
    ("ComboBox", "cboReportProject", 100, 42, 240, 23, DDL),
    ("Label", "lblReportFrom", 12, 75, None, None, {"Text": T("From")}),
    ("DateTimePicker", "dtpReportFrom", 100, 72, 115, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblReportTo", 225, 75, None, None, {"Text": T("To")}),
    ("DateTimePicker", "dtpReportTo", 251, 72, 115, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Button", "btnPreviewReport", 12, 110, 80, 28, {"Text": T("Preview")}),
    ("Button", "btnPrintReport", 98, 110, 70, 28, {"Text": T("Print")}),
    ("Button", "btnReportPdf", 174, 110, 100, 28, {"Text": T("Save as PDF")}),
    ("Button", "btnReportExcel", 280, 110, 100, 28, {"Text": T("Export Excel")}),
    ("Label", "lblReportMsg", 12, 148, None, None, {}),
]

GRID_COLUMNS = {}

EVENTS = {
    "btnPreviewReport": [("Click", "btnPreviewReport_Click")],
    "btnPrintReport": [("Click", "btnPrintReport_Click")],
    "btnReportPdf": [("Click", "btnReportPdf_Click")],
    "btnReportExcel": [("Click", "btnReportExcel_Click")],
}
