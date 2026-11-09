# Spec for src/Company.OfficeApp/Pages/QuotationsPage.Designer.cs (owner request 2026-11-09: client quotations)
NAMESPACE = "Company.OfficeApp.Pages"
CLASS = "QuotationsPage"
ROOMY = True   # spread out like the Figma pages
SIZE = (960, 840)
LOAD_EVENT = "QuotationsPage_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" }'
NUM = {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "999999999m"}
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
    ("Label", "lblClientName", 12, 290, None, None, {"Text": T("Client name")}),
    ("TextBox", "txtClientName", 12, 308, 280, 23, {"MaxLength": "150"}),
    ("Label", "lblClientContact", 12, 338, None, None, {"Text": T("Contact number")}),
    ("TextBox", "txtClientContact", 12, 356, 280, 23, {"MaxLength": "60"}),
    ("Label", "lblClientEmail", 12, 386, None, None, {"Text": T("Email")}),
    ("TextBox", "txtClientEmail", 12, 404, 280, 23, {"MaxLength": "150"}),
    ("Label", "lblProjectTitle", 310, 290, None, None, {"Text": T("Project (e.g. Two-storey house)"), "Font": 'new Font("Segoe UI", 9F)'}),
    ("TextBox", "txtProjectTitle", 310, 308, 280, 23, {"MaxLength": "150"}),
    ("Label", "lblSiteLocation", 310, 338, None, None, {"Text": T("Site location")}),
    ("TextBox", "txtSiteLocation", 310, 356, 280, 23, {"MaxLength": "255"}),
    ("Label", "lblSector", 310, 386, None, None, {"Text": T("Sector")}),
    ("ComboBox", "cboSector", 310, 404, 280, 23, DDL),
    ("Label", "lblQuoteDate", 608, 290, None, None, {"Text": T("Quotation date")}),
    ("DateTimePicker", "dtpQuoteDate", 608, 308, 140, 23, DATE),
    ("Label", "lblValidUntil", 608, 338, None, None, {"Text": T("Valid until")}),
    ("DateTimePicker", "dtpValidUntil", 608, 356, 140, 23, DATE),
    ("Label", "lblPlannedStart", 608, 386, None, None, {"Text": T("Planned start (tick to set)")}),
    ("DateTimePicker", "dtpPlannedStart", 608, 404, 140, 23, dict(DATE, ShowCheckBox="true", Checked="false")),
    ("Label", "lblFromInquiry", 762, 290, None, None, {"Text": T("From a website inquiry")}),
    ("ComboBox", "cboFromInquiry", 762, 308, 180, 23, DDL),

    ("Label", "lblLines", 12, 440, None, None, {"Text": T("Items (type in the table; a new row appears as you type)")}),
    ("DataGridView", "dgvLines", 12, 458, 620, 160, {}),
    ("Button", "btnRemoveLine", 12, 624, 110, 28, {"Text": T("Remove Line")}),
    ("Label", "lblDiscount", 650, 458, None, None, {"Text": T("Discount (pesos)")}),
    ("NumericUpDown", "numDiscount", 650, 476, 140, 23, NUM),
    ("CheckBox", "chkVat", 650, 506, None, None, {"Text": T("Add 12% VAT")}),
    ("Label", "lblSubtotal", 650, 536, None, None, {"Text": T("Subtotal: -")}),
    ("Label", "lblDiscountShown", 650, 556, None, None, {"Text": T("Discount: -")}),
    ("Label", "lblVatShown", 650, 576, None, None, {"Text": T("VAT: -")}),
    ("Label", "lblTotal", 650, 600, None, None, {"Text": T("TOTAL: -"), "Font": 'new Font("Segoe UI", 11F, FontStyle.Bold)'}),

    ("Label", "lblTerms", 12, 660, None, None, {"Text": T("Terms and conditions (printed)")}),
    ("TextBox", "txtTerms", 12, 678, 460, 80, dict(MULTI, MaxLength="2000")),
    ("Label", "lblNotes", 490, 660, None, None, {"Text": T("Internal notes (not printed)")}),
    ("TextBox", "txtNotes", 490, 678, 452, 80, dict(MULTI, MaxLength="1000")),
    ("Label", "lblQuotationMsg", 12, 766, None, None, {"MaximumSize": "new Size(930, 0)"}),
    ("Button", "btnSaveQuotation", 12, 790, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelQuotation", 108, 790, 90, 28, {"Text": T("Cancel")}),
    ("Button", "btnSubmitQuotation", 204, 790, 150, 28, {"Text": T("Send for Approval")}),
    ("Button", "btnApproveQuotation", 360, 790, 90, 28, {"Text": T("Approve")}),
    ("Button", "btnRejectQuotation", 456, 790, 90, 28, {"Text": T("Reject")}),
    ("Button", "btnAcceptQuotation", 552, 790, 180, 28, {"Text": T("Client Accepted: Make Project")}),
    ("Button", "btnDeclineQuotation", 738, 790, 120, 28, {"Text": T("Client Declined")}),
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
    "dgvLines": [
        (TB, "colItem", "Description", "Item / Description", {"FillWeight": "220"}),
        (TB, "colQty", "Quantity", "Qty", {"FillWeight": "45", "DefaultCellStyle": 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }'}),
        (TB, "colUnit", "Unit", "Unit", {"FillWeight": "45"}),
        (TB, "colUnitPrice", "UnitPrice", "Unit Price", {"FillWeight": "70", "DefaultCellStyle": MONEY}),
        (TB, "colAmount", "Amount", "Amount", {"FillWeight": "75", "DefaultCellStyle": MONEY}),
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
    "numDiscount": [("ValueChanged", "Totals_Changed")],
    "chkVat": [("CheckedChanged", "Totals_Changed")],
    "btnSaveQuotation": [("Click", "btnSaveQuotation_Click")],
    "btnCancelQuotation": [("Click", "btnCancelQuotation_Click")],
    "btnSubmitQuotation": [("Click", "btnSubmitQuotation_Click")],
    "btnApproveQuotation": [("Click", "btnApproveQuotation_Click")],
    "btnRejectQuotation": [("Click", "btnRejectQuotation_Click")],
    "btnAcceptQuotation": [("Click", "btnAcceptQuotation_Click")],
    "btnDeclineQuotation": [("Click", "btnDeclineQuotation_Click")],
}
