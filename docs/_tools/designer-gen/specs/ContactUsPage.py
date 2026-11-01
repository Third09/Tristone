# Spec for src/Company.WebsiteAdmin/Pages/ContactUsPage.Designer.cs (Figma "admin contact us")
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "ContactUsPage"
SIZE = (960, 700)
LOAD_EVENT = "ContactUsPage_Load"
DATE = {"Format": "DateTimePickerFormat.Short", "ShowCheckBox": "true", "Checked": "false"}

CONTROLS = [
    ("Label", "lblContactHint", 12, 12, None, None, {"Text": T("View and manage inquiries submitted through the website.")}),
    ("Button", "btnRefreshContact", 852, 8, 90, 28, {"Text": T("Refresh")}),
    ("Label", "lblTotalInquiries", 12, 42, None, None, {"Text": T("Total inquiries: -")}),
    ("Label", "lblPendingInquiries", 200, 42, None, None, {"Text": T("Pending (New): -")}),
    ("Label", "lblWebsiteVisits", 380, 42, None, None, {"Text": T("Website visits this month: -")}),
    ("Label", "lblRecipientEmail", 12, 74, None, None, {"Text": T("Inquiries are forwarded to")}),
    ("TextBox", "txtRecipientEmail", 170, 71, 260, 23, {}),
    ("Button", "btnSaveRecipientEmail", 436, 69, 110, 27, {"Text": T("Save Email")}),
    ("Label", "lblCompanyDetails", 12, 106, None, None, {"Text": T("Company details on the website (address, phone, email, footer)")}),
    ("Label", "lblCompanyAddress", 12, 130, None, None, {"Text": T("Address")}),
    ("TextBox", "txtCompanyAddress", 100, 127, 446, 23, {}),
    ("Label", "lblCompanyPhone", 12, 160, None, None, {"Text": T("Phone")}),
    ("TextBox", "txtCompanyPhone", 100, 157, 180, 23, {}),
    ("Label", "lblCompanyEmail", 292, 160, None, None, {"Text": T("Email")}),
    ("TextBox", "txtCompanyEmail", 336, 157, 210, 23, {}),
    ("Label", "lblCopyright", 12, 190, None, None, {"Text": T("Footer")}),
    ("TextBox", "txtCopyright", 100, 187, 446, 23, {}),
    ("Button", "btnSaveCompanyDetails", 560, 185, 120, 27, {"Text": T("Save Details")}),
    ("Label", "lblContactSettingsMsg", 560, 130, None, None, {"MaximumSize": "new Size(380, 0)"}),
    ("Label", "lblClientInquiries", 12, 228, None, None, {"Text": T("Client inquiries (double-click one to open it)")}),
    ("TextBox", "txtInquirySearch", 12, 250, 250, 23, {"PlaceholderText": T("Search name, email, service or location")}),
    ("ComboBox", "cboInquiryStatusFilter", 270, 250, 110, 23, DDL),
    ("Label", "lblInquiryFrom", 390, 253, None, None, {"Text": T("From")}),
    ("DateTimePicker", "dtpInquiryFrom", 428, 250, 120, 23, DATE),
    ("Label", "lblInquiryTo", 556, 253, None, None, {"Text": T("To")}),
    ("DateTimePicker", "dtpInquiryTo", 580, 250, 120, 23, DATE),
    ("Button", "btnResetInquiryFilter", 708, 248, 100, 27, {"Text": T("Reset Filter")}),
    ("Button", "btnExportInquiries", 822, 248, 120, 27, {"Text": T("Export to Excel")}),
    ("DataGridView", "dgvInquiries", 12, 282, 930, 330, {}),
    ("Label", "lblInquiryCount", 12, 622, None, None, {"Text": T("")}),
    ("Button", "btnOpenInquiry", 822, 618, 120, 28, {"Text": T("Open Inquiry")}),
]

GRID_COLUMNS = {
    "dgvInquiries": [
        (TB, "colDate", "ReceivedAt", "Date", {"DefaultCellStyle": 'new DataGridViewCellStyle { Format = "yyyy-MM-dd" }', "FillWeight": "50"}),
        (TB, "colName", "FullName", "Name", {}),
        (TB, "colService", "ServiceRequired", "Service", {}),
        (TB, "colLocation", "ProjectLocation", "Location", {}),
        (TB, "colStatus", "Status", "Status", {"FillWeight": "50"}),
    ],
}

EVENTS = {
    "btnRefreshContact": [("Click", "btnRefreshContact_Click")],
    "btnSaveRecipientEmail": [("Click", "btnSaveRecipientEmail_Click")],
    "btnSaveCompanyDetails": [("Click", "btnSaveCompanyDetails_Click")],
    "cboInquiryStatusFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "dtpInquiryFrom": [("ValueChanged", "Filter_Changed")],
    "dtpInquiryTo": [("ValueChanged", "Filter_Changed")],
    "btnResetInquiryFilter": [("Click", "btnResetInquiryFilter_Click")],
    "btnExportInquiries": [("Click", "btnExportInquiries_Click")],
    "dgvInquiries": [("CellDoubleClick", "dgvInquiries_CellDoubleClick")],
    "btnOpenInquiry": [("Click", "btnOpenInquiry_Click")],
}
