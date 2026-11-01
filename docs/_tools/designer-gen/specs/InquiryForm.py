# Spec for src/Company.WebsiteAdmin/Pages/InquiryForm.Designer.cs (pop-up window: one inquiry, opened from Contact Us)
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "InquiryForm"
FORM = True
SIZE = (560, 520)
LOAD_EVENT = None

CONTROLS = [
    ("Label", "lblInqName", 12, 12, None, None, {"Text": T("Name: -")}),
    ("Label", "lblInqContact", 12, 34, None, None, {"Text": T("Contact number: -")}),
    ("Label", "lblInqEmail", 12, 56, None, None, {"Text": T("Email: -")}),
    ("Button", "btnCopyEmail", 430, 50, 118, 27, {"Text": T("Copy Email")}),
    ("Label", "lblInqLocation", 12, 78, None, None, {"Text": T("Project location: -")}),
    ("Label", "lblInqService", 12, 100, None, None, {"Text": T("Service required: -")}),
    ("Label", "lblInqReceived", 12, 122, None, None, {"Text": T("Received: -")}),
    ("Label", "lblInqMessage", 12, 148, None, None, {"Text": T("Message")}),
    ("TextBox", "txtInqMessage", 12, 166, 536, 120, {"Multiline": "true", "ReadOnly": "true", "ScrollBars": "ScrollBars.Vertical"}),
    ("Label", "lblInquiryStatus", 12, 298, None, None, {"Text": T("Status")}),
    ("ComboBox", "cboInquiryStatus", 12, 316, 160, 23, DDL),
    ("Label", "lblInquiryNotes", 12, 346, None, None, {"Text": T("Internal notes (never shown on the website)")}),
    ("TextBox", "txtInquiryNotes", 12, 364, 536, 90, {"Multiline": "true", "ScrollBars": "ScrollBars.Vertical"}),
    ("Label", "lblInquiryMsg", 12, 460, None, None, {"MaximumSize": "new Size(330, 0)"}),
    ("Button", "btnSaveInquiry", 362, 482, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCloseInquiry", 458, 482, 90, 28, {"Text": T("Close"), "DialogResult": "DialogResult.Cancel"}),
]

GRID_COLUMNS = {}

EVENTS = {
    "btnCopyEmail": [("Click", "btnCopyEmail_Click")],
    "btnSaveInquiry": [("Click", "btnSaveInquiry_Click")],
}
