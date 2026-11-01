# Spec for src/Company.WebsiteAdmin/Pages/ContactUsPage.Designer.cs (Figma "admin contact us", redone 2026-11-01)
# Layout (docked): KPI cards + email settings across the top, company details card, client inquiries card (fills the rest).
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "ContactUsPage"
SIZE = (1240, 940)
LOAD_EVENT = "ContactUsPage_Load"
CARD = "Company.OfficeApp.UI.Card"
FRAME = "Company.OfficeApp.UI.InputFrame"
SECTION = 'new Font("Segoe UI", 14F)'
KPI_TITLE = 'new Font("Segoe UI Semibold", 11F)'
KPI = 'new Font("Segoe UI", 24F, FontStyle.Bold)'
FIELD = 'new Font("Segoe UI Semibold", 10F)'
INPUT = 'new Font("Segoe UI", 10.5F)'
BTN = 'new Font("Segoe UI", 10F)'
TR = "AnchorStyles.Top | AnchorStyles.Right"
TLR = "AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right"
ALL = "AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right"
BL = "AnchorStyles.Bottom | AnchorStyles.Left"
BR = "AnchorStyles.Bottom | AnchorStyles.Right"
TOP = "DockStyle.Top"
LEFT = "DockStyle.Left"
DATE = {"Format": "DateTimePickerFormat.Short", "ShowCheckBox": "true", "Checked": "false", "Font": INPUT}

CONTROLS = [
    (CARD, "cardInquiries", 24, 426, 1192, 492, {"Dock": "DockStyle.Fill", "Padding": "new Padding(24)"}),
    ("Panel", "pnlGap2", 24, 406, 1192, 20, {"Dock": TOP}),
    (CARD, "cardCompany", 24, 188, 1192, 218, {"Dock": TOP, "Padding": "new Padding(24)"}),
    ("Panel", "pnlGap1", 24, 168, 1192, 20, {"Dock": TOP}),
    ("Panel", "pnlKpis", 24, 8, 1192, 160, {"Dock": TOP}),

    # KPI row (docked left; the last added sits leftmost; the email card fills the rest)
    (CARD, "cardEmail", 750, 0, 442, 160, {"PARENT": "pnlKpis", "Dock": "DockStyle.Fill"}),
    ("Panel", "pnlKpiGap3", 730, 0, 20, 160, {"PARENT": "pnlKpis", "Dock": LEFT}),
    (CARD, "cardVisits", 500, 0, 230, 160, {"PARENT": "pnlKpis", "Dock": LEFT}),
    ("Panel", "pnlKpiGap2", 480, 0, 20, 160, {"PARENT": "pnlKpis", "Dock": LEFT}),
    (CARD, "cardPending", 250, 0, 230, 160, {"PARENT": "pnlKpis", "Dock": LEFT}),
    ("Panel", "pnlKpiGap1", 230, 0, 20, 160, {"PARENT": "pnlKpis", "Dock": LEFT}),
    (CARD, "cardTotal", 0, 0, 230, 160, {"PARENT": "pnlKpis", "Dock": LEFT}),

    ("Label", "lblTotalTitle", 20, 18, None, None, {"PARENT": "cardTotal", "Font": KPI_TITLE, "Text": T("Total Inquiries")}),
    ("Label", "lblTotalInquiries", 18, 50, None, None, {"PARENT": "cardTotal", "Font": KPI, "Text": T("-")}),
    ("Label", "lblTotalNote", 20, 118, None, None, {"PARENT": "cardTotal", "Text": T("All received inquiries")}),
    ("Label", "lblPendingTitle", 20, 18, None, None, {"PARENT": "cardPending", "Font": KPI_TITLE, "Text": T("Pending Inquiries")}),
    ("Label", "lblPendingInquiries", 18, 50, None, None, {"PARENT": "cardPending", "Font": KPI, "Text": T("-")}),
    ("Label", "lblPendingNote", 20, 118, None, None, {"PARENT": "cardPending", "Text": T("Awaiting response (New)")}),
    ("Label", "lblVisitsTitle", 20, 18, None, None, {"PARENT": "cardVisits", "Font": KPI_TITLE, "Text": T("Website Visits")}),
    ("Label", "lblWebsiteVisits", 18, 50, None, None, {"PARENT": "cardVisits", "Font": KPI, "Text": T("-")}),
    ("Label", "lblVisitsNote", 20, 118, None, None, {"PARENT": "cardVisits", "Text": T("This month")}),
    ("Label", "lblEmailSettingsTitle", 20, 18, None, None, {"PARENT": "cardEmail", "Font": KPI_TITLE, "Text": T("Inquiry Email Settings")}),
    ("Label", "lblRecipientEmail", 20, 50, None, None, {"PARENT": "cardEmail", "Font": FIELD, "Text": T("Recipient Email Address")}),
    (FRAME, "frmRecipient", 20, 72, 272, 40, {"PARENT": "cardEmail", "Anchor": TLR}),
    ("TextBox", "txtRecipientEmail", 12, 9, 248, 23, {"PARENT": "frmRecipient", "BorderStyle": "BorderStyle.None", "Font": INPUT}),
    ("Button", "btnSaveRecipientEmail", 302, 72, 120, 40, {"PARENT": "cardEmail", "Anchor": TR, "Font": BTN, "Text": T("Save Email")}),
    ("Label", "lblRecipientNote", 20, 124, 402, 30, {"PARENT": "cardEmail", "Anchor": TLR, "Text": T("Inquiries from the website's Contact form are forwarded to this address.")}),

    # company details
    ("Label", "lblCompanyDetails", 24, 18, None, None, {"PARENT": "cardCompany", "Font": SECTION, "Text": T("Company Details on the Website")}),
    ("Label", "lblCompanyAddress", 24, 58, None, None, {"PARENT": "cardCompany", "Font": FIELD, "Text": T("Address")}),
    (FRAME, "frmCompanyAddress", 24, 80, 560, 40, {"PARENT": "cardCompany"}),
    ("TextBox", "txtCompanyAddress", 12, 9, 536, 23, {"PARENT": "frmCompanyAddress", "BorderStyle": "BorderStyle.None", "Font": INPUT}),
    ("Label", "lblCompanyPhone", 608, 58, None, None, {"PARENT": "cardCompany", "Font": FIELD, "Text": T("Phone")}),
    (FRAME, "frmCompanyPhone", 608, 80, 240, 40, {"PARENT": "cardCompany"}),
    ("TextBox", "txtCompanyPhone", 12, 9, 216, 23, {"PARENT": "frmCompanyPhone", "BorderStyle": "BorderStyle.None", "Font": INPUT}),
    ("Label", "lblCompanyEmail", 872, 58, None, None, {"PARENT": "cardCompany", "Font": FIELD, "Text": T("Email")}),
    (FRAME, "frmCompanyEmail", 872, 80, 296, 40, {"PARENT": "cardCompany", "Anchor": TLR}),
    ("TextBox", "txtCompanyEmail", 12, 9, 272, 23, {"PARENT": "frmCompanyEmail", "BorderStyle": "BorderStyle.None", "Font": INPUT}),
    ("Label", "lblCopyright", 24, 132, None, None, {"PARENT": "cardCompany", "Font": FIELD, "Text": T("Footer")}),
    (FRAME, "frmCopyright", 24, 154, 560, 40, {"PARENT": "cardCompany"}),
    ("TextBox", "txtCopyright", 12, 9, 536, 23, {"PARENT": "frmCopyright", "BorderStyle": "BorderStyle.None", "Font": INPUT}),
    ("Label", "lblContactSettingsMsg", 608, 154, 400, 40, {"PARENT": "cardCompany", "TextAlign": "ContentAlignment.MiddleLeft"}),
    ("Button", "btnSaveCompanyDetails", 1018, 154, 150, 40, {"PARENT": "cardCompany", "Anchor": TR, "Font": BTN, "Text": T("Save Details")}),

    # client inquiries
    ("Label", "lblClientInquiries", 24, 18, None, None, {"PARENT": "cardInquiries", "Font": SECTION, "Text": T("Client Inquiries")}),
    ("Label", "lblInquiriesSub", 24, 48, None, None, {"PARENT": "cardInquiries", "Text": T("Website contact form submissions. Double-click one to open it.")}),
    ("Button", "btnRefreshContact", 858, 18, 140, 40, {"PARENT": "cardInquiries", "Anchor": TR, "Font": BTN, "Text": T("Refresh")}),
    ("Button", "btnExportInquiries", 1008, 18, 160, 40, {"PARENT": "cardInquiries", "Anchor": TR, "Font": BTN, "Text": T("Export to Excel")}),
    (FRAME, "frmInquirySearch", 24, 82, 380, 40, {"PARENT": "cardInquiries"}),
    ("TextBox", "txtInquirySearch", 12, 9, 356, 23, {"PARENT": "frmInquirySearch", "BorderStyle": "BorderStyle.None", "Font": INPUT,
                                                     "PlaceholderText": T("Search name, email, service or location...")}),
    ("ComboBox", "cboInquiryStatusFilter", 420, 88, 150, 27, dict(DDL, PARENT="cardInquiries", Font=INPUT)),
    ("Label", "lblInquiryFrom", 588, 92, None, None, {"PARENT": "cardInquiries", "Text": T("From")}),
    ("DateTimePicker", "dtpInquiryFrom", 628, 88, 150, 27, dict(DATE, PARENT="cardInquiries")),
    ("Label", "lblInquiryTo", 796, 92, None, None, {"PARENT": "cardInquiries", "Text": T("To")}),
    ("DateTimePicker", "dtpInquiryTo", 820, 88, 150, 27, dict(DATE, PARENT="cardInquiries")),
    ("Button", "btnResetInquiryFilter", 1028, 82, 140, 40, {"PARENT": "cardInquiries", "Anchor": TR, "Text": T("Reset Filter")}),
    ("DataGridView", "dgvInquiries", 24, 138, 1144, 280, {"PARENT": "cardInquiries", "Anchor": ALL}),
    ("Label", "lblInquiryCount", 24, 440, None, None, {"PARENT": "cardInquiries", "Anchor": BL, "Text": T("")}),
    ("Button", "btnOpenInquiry", 1018, 432, 150, 36, {"PARENT": "cardInquiries", "Anchor": BR, "Text": T("Open Inquiry")}),
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
