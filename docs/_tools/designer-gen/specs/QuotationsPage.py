# Spec for src/Company.OfficeApp/Pages/QuotationsPage.Designer.cs (owner request 2026-11-09: client quotations)
NAMESPACE = "Company.OfficeApp.Pages"
CLASS = "QuotationsPage"
ROOMY = True   # spread out like the Figma pages
SIZE = (960, 866)
LOAD_EVENT = "QuotationsPage_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" }'
DATE = {"Format": "DateTimePickerFormat.Short"}
MULTI = {"Multiline": "true", "ScrollBars": "ScrollBars.Vertical"}

CONTROLS = [
    ("Label", "lblSearch", 12, 15, None, None, {"Text": T("Search")}),
    ("TextBox", "txtQuotationSearch", 62, 12, 180, 23, {"PlaceholderText": T("No., client or project")}),
    ("Label", "lblStatusFilter", 256, 15, None, None, {"Text": T("Status")}),
    ("ComboBox", "cboQuotationStatusFilter", 306, 12, 130, 23, DDL),
    ("DataGridView", "dgvQuotations", 12, 44, 930, 170, {}),
    ("Button", "btnNewQuotation", 12, 222, 120, 28, {"Text": T("New Quotation")}),
    ("Button", "btnDeleteQuotation", 138, 222, 90, 28, {"Text": T("Delete")}),
    ("Button", "btnPrintQuotation", 234, 222, 90, 28, {"Text": T("Print")}),
    ("Button", "btnSaveQuotationPdf", 330, 222, 120, 28, {"Text": T("Save as PDF")}),

    ("Label", "lblFormMode", 12, 266, None, None, {"Text": T("New quotation")}),
    ("Label", "lblClientName", 12, 290, None, None, {"Text": T("Customer")}),
    ("TextBox", "txtClientName", 12, 308, 280, 23, {"MaxLength": "150"}),
    ("Label", "lblClientAddress", 12, 338, None, None, {"Text": T("Address")}),
    ("TextBox", "txtClientAddress", 12, 356, 280, 23, {"MaxLength": "255"}),
    ("Label", "lblAttention", 12, 386, None, None, {"Text": T("Attention (person it is addressed to)")}),
    ("TextBox", "txtAttention", 12, 404, 280, 23, {"MaxLength": "150"}),
    ("Label", "lblClientContact", 310, 290, None, None, {"Text": T("Contact number (not printed)")}),
    ("TextBox", "txtClientContact", 310, 308, 280, 23, {"MaxLength": "60"}),
    ("Label", "lblClientEmail", 310, 338, None, None, {"Text": T("Email (not printed)")}),
    ("TextBox", "txtClientEmail", 310, 356, 280, 23, {"MaxLength": "150"}),
    ("Label", "lblFromInquiry", 310, 386, None, None, {"Text": T("From a website inquiry")}),
    ("ComboBox", "cboFromInquiry", 310, 404, 280, 23, DDL),
    ("Label", "lblQuoteDate", 608, 290, None, None, {"Text": T("Date")}),
    ("DateTimePicker", "dtpQuoteDate", 608, 308, 140, 23, DATE),
    ("Label", "lblValidUntil", 608, 338, None, None, {"Text": T("Valid until (not printed)")}),
    ("DateTimePicker", "dtpValidUntil", 608, 356, 140, 23, DATE),
    ("Label", "lblPlannedStart", 608, 386, None, None, {"Text": T("Planned start (tick to set)")}),
    ("DateTimePicker", "dtpPlannedStart", 608, 404, 140, 23, dict(DATE, ShowCheckBox="true", Checked="false")),
    ("Label", "lblProjectTitle", 762, 290, None, None, {"Text": T("Project name"), "Font": 'new Font("Segoe UI", 9F)'}),
    ("TextBox", "txtProjectTitle", 762, 308, 180, 23, {"MaxLength": "150"}),
    ("Label", "lblSiteLocation", 762, 338, None, None, {"Text": T("Site location")}),
    ("TextBox", "txtSiteLocation", 762, 356, 180, 23, {"MaxLength": "255"}),
    ("Label", "lblSector", 762, 386, None, None, {"Text": T("Sector")}),
    ("ComboBox", "cboSector", 762, 404, 180, 23, DDL),

    ("Label", "lblScope", 12, 440, None, None, {"Text": T("Scope of Works (printed A, B, C...; price optional)")}),
    ("DataGridView", "dgvScope", 12, 458, 600, 130, {}),
    ("Button", "btnRemoveScope", 12, 594, 110, 28, {"Text": T("Remove Line")}),
    ("Label", "lblCosts", 630, 440, None, None, {"Text": T("Other costs (e.g. Labor Cost)")}),
    ("DataGridView", "dgvLines", 630, 458, 312, 130, {}),
    ("Button", "btnRemoveLine", 630, 594, 110, 28, {"Text": T("Remove Cost")}),
    ("Label", "lblTotal", 750, 598, None, None, {"Text": T("Total Cost: -"), "Font": 'new Font("Segoe UI", 11F, FontStyle.Bold)'}),

    ("Label", "lblPrintedNotes", 12, 632, None, None, {"Text": T("Notes (printed; one per line)")}),
    ("TextBox", "txtPrintedNotes", 12, 650, 460, 70, dict(MULTI, MaxLength="2000")),
    ("Label", "lblWorkDuration", 490, 632, None, None, {"Text": T("Work duration")}),
    ("TextBox", "txtWorkDuration", 490, 650, 140, 23, {"MaxLength": "100", "PlaceholderText": T("e.g. 30 days")}),
    ("Label", "lblPaymentTerms", 642, 632, None, None, {"Text": T("Terms of payment")}),
    ("TextBox", "txtPaymentTerms", 642, 650, 140, 23, {"MaxLength": "150"}),
    ("Label", "lblPayableTo", 794, 632, None, None, {"Text": T("Cash payable to")}),
    ("TextBox", "txtPayableTo", 794, 650, 148, 23, {"MaxLength": "150"}),
    ("Label", "lblPayableNumber", 490, 680, None, None, {"Text": T("Payable number (#)")}),
    ("TextBox", "txtPayableNumber", 490, 698, 140, 23, {"MaxLength": "60"}),
    ("Label", "lblSignatoryName", 642, 680, None, None, {"Text": T("Very truly yours (name)")}),
    ("TextBox", "txtSignatoryName", 642, 698, 140, 23, {"MaxLength": "150"}),
    ("Label", "lblSignatoryTitle", 794, 680, None, None, {"Text": T("Position"), "Font": 'new Font("Segoe UI", 9F)'}),
    ("TextBox", "txtSignatoryTitle", 794, 698, 148, 23, {"MaxLength": "100"}),
    ("Label", "lblInternalNotes", 12, 728, None, None, {"Text": T("Internal notes (not printed)")}),
    ("TextBox", "txtInternalNotes", 12, 746, 930, 40, dict(MULTI, MaxLength="1000")),
    ("Label", "lblQuotationMsg", 12, 794, None, None, {"MaximumSize": "new Size(930, 0)"}),
    ("Button", "btnSaveQuotation", 12, 818, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelQuotation", 108, 818, 90, 28, {"Text": T("Cancel")}),
    ("Button", "btnSubmitQuotation", 204, 818, 150, 28, {"Text": T("Send for Approval")}),
    ("Button", "btnApproveQuotation", 360, 818, 90, 28, {"Text": T("Approve")}),
    ("Button", "btnRejectQuotation", 456, 818, 90, 28, {"Text": T("Reject")}),
    ("Button", "btnAcceptQuotation", 552, 818, 180, 28, {"Text": T("Client Accepted: Make Project")}),
    ("Button", "btnDeclineQuotation", 738, 818, 120, 28, {"Text": T("Client Declined")}),
]

GRID_COLUMNS = {
    "dgvQuotations": [
        (TB, "colQuoteNo", "QuoteNo", "No.", {"FillWeight": "50"}),
        (TB, "colQuoteDate", "QuoteDate", "Date", {"FillWeight": "55", "DefaultCellStyle": 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'}),
        (TB, "colClient", "ClientName", "Client", {}),
        (TB, "colProject", "ProjectTitle", "Project", {}),
        (TB, "colTotal", "Total", "Total", {"FillWeight": "60", "DefaultCellStyle": MONEY}),
        (TB, "colValidUntil", "ValidUntil", "Valid Until", {"FillWeight": "55", "DefaultCellStyle": 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'}),
        (TB, "colStatus", "Status", "Status", {"FillWeight": "50"}),
    ],
    "dgvScope": [
        (TB, "colWork", "Work", "Work", {"FillWeight": "170"}),
        (TB, "colDetails", "Details", "Size / Details", {"FillWeight": "110"}),
        (TB, "colPrice", "Price", "Price", {"FillWeight": "70", "DefaultCellStyle": MONEY}),
    ],
    "dgvLines": [
        (TB, "colItem", "Description", "Description", {"FillWeight": "150"}),
        (TB, "colAmount", "Amount", "Amount", {"FillWeight": "90", "DefaultCellStyle": MONEY}),
    ],
}

EVENTS = {
    "dgvQuotations": [("SelectionChanged", "dgvQuotations_SelectionChanged")],
    "cboQuotationStatusFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "btnNewQuotation": [("Click", "btnNewQuotation_Click")],
    "btnDeleteQuotation": [("Click", "btnDeleteQuotation_Click")],
    "btnPrintQuotation": [("Click", "btnPrintQuotation_Click")],
    "btnSaveQuotationPdf": [("Click", "btnSaveQuotationPdf_Click")],
    "cboFromInquiry": [("SelectionChangeCommitted", "cboFromInquiry_SelectionChangeCommitted")],
    "dgvLines": [("CellEndEdit", "Lines_Changed"), ("RowsRemoved", "Lines_RowsRemoved")],
    "btnRemoveLine": [("Click", "btnRemoveLine_Click")],
    "btnRemoveScope": [("Click", "btnRemoveScope_Click")],
    "dgvScope": [("CellEndEdit", "Lines_Changed"), ("RowsRemoved", "Lines_RowsRemoved")],
    "btnSaveQuotation": [("Click", "btnSaveQuotation_Click")],
    "btnCancelQuotation": [("Click", "btnCancelQuotation_Click")],
    "btnSubmitQuotation": [("Click", "btnSubmitQuotation_Click")],
    "btnApproveQuotation": [("Click", "btnApproveQuotation_Click")],
    "btnRejectQuotation": [("Click", "btnRejectQuotation_Click")],
    "btnAcceptQuotation": [("Click", "btnAcceptQuotation_Click")],
    "btnDeclineQuotation": [("Click", "btnDeclineQuotation_Click")],
}
