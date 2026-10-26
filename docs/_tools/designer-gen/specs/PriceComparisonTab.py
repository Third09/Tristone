# Spec for src/Company.OfficeApp/Pages/Inventory/PriceComparisonTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Inventory"
CLASS = "PriceComparisonTab"
SIZE = (960, 700)
LOAD_EVENT = "PriceComparisonTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
QTY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.##" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'
NUM = {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "999999999m"}

CONTROLS = [
    ("Label", "lblCompareMaterial", 12, 15, None, None, {"Text": T("Material to buy")}),
    ("ComboBox", "cboCompareMaterial", 110, 12, 260, 23, DDL),
    # add a quote
    ("Label", "lblAddQuote", 12, 46, None, None, {"Text": T("Add a quote")}),
    ("Label", "lblQuoteVendor", 12, 66, None, None, {"Text": T("Vendor")}),
    ("ComboBox", "cboQuoteVendor", 12, 84, 200, 23, DDL),
    ("Label", "lblQuoteDate", 224, 66, None, None, {"Text": T("Date inputted")}),
    ("DateTimePicker", "dtpQuoteDate", 224, 84, 120, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblQuoteProduct", 356, 66, None, None, {"Text": T("Product name")}),
    ("TextBox", "txtQuoteProduct", 356, 84, 200, 23, {}),
    ("Label", "lblQuoteCategory", 568, 66, None, None, {"Text": T("Category")}),
    ("ComboBox", "cboQuoteCategory", 568, 84, 140, 23, {}),
    ("Label", "lblQuoteDescription", 12, 114, None, None, {"Text": T("Description")}),
    ("TextBox", "txtQuoteDescription", 12, 132, 332, 23, {}),
    ("Label", "lblQuoteUnitPrice", 356, 114, None, None, {"Text": T("Unit price")}),
    ("NumericUpDown", "numQuoteUnitPrice", 356, 132, 120, 23, NUM),
    ("Label", "lblQuoteQty", 488, 114, None, None, {"Text": T("Quantity")}),
    ("NumericUpDown", "numQuoteQty", 488, 132, 100, 23, NUM),
    ("Button", "btnAddQuote", 600, 130, 108, 28, {"Text": T("Add Quote")}),
    ("Label", "lblQuoteMsg", 12, 162, None, None, {}),
    # compare
    ("Label", "lblCheapestQuote", 12, 190, None, None, {"Text": T("")}),
    ("DataGridView", "dgvQuotes", 12, 210, 930, 180, {}),
    ("Button", "btnDeleteQuote", 12, 398, 110, 28, {"Text": T("Delete Quote")}),
    ("Button", "btnApprovePurchase", 128, 398, 160, 28, {"Text": T("Approve as Purchase")}),
    ("Label", "lblApproverNote", 300, 404, None, None, {"Text": T("Approve as Purchase: Inventory Supervisor or Admin only")}),
    # purchases
    ("Label", "lblPurchases", 12, 444, None, None, {"Text": T("Approved purchases")}),
    ("DataGridView", "dgvPurchases", 12, 464, 930, 180, {}),
]

GRID_COLUMNS = {
    "dgvQuotes": [
        (TB, "colQVendor", "Vendor", "Vendor", {}),
        (TB, "colQDate", "QuotedOn", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colQProduct", "ProductName", "Product", {}),
        (TB, "colQUnitPrice", "UnitPrice", "Unit Price", {"DefaultCellStyle": MONEY}),
        (TB, "colQQty", "Quantity", "Qty", {"DefaultCellStyle": QTY}),
        (TB, "colQTotal", "Total", "Total", {"DefaultCellStyle": MONEY}),
        (TB, "colQStatus", "Status", "Status", {}),
    ],
    "dgvPurchases": [
        (TB, "colPDate", "ApprovedAt", "Approved", {"DefaultCellStyle": DATE}),
        (TB, "colPVendor", "Vendor", "Vendor", {}),
        (TB, "colPMaterial", "Material", "Material", {}),
        (TB, "colPQty", "Quantity", "Qty", {"DefaultCellStyle": QTY}),
        (TB, "colPTotal", "Total", "Total", {"DefaultCellStyle": MONEY}),
        (TB, "colPDelivered", "DeliveredQty", "Delivered Qty", {"DefaultCellStyle": QTY}),
        ("DataGridViewCheckBoxColumn", "colPDone", "Delivered", "Fully Delivered", {}),
    ],
}

EVENTS = {
    "cboCompareMaterial": [("SelectedIndexChanged", "cboCompareMaterial_SelectedIndexChanged")],
    "btnAddQuote": [("Click", "btnAddQuote_Click")],
    "dgvQuotes": [("SelectionChanged", "dgvQuotes_SelectionChanged"), ("CellFormatting", "dgvQuotes_CellFormatting")],
    "btnDeleteQuote": [("Click", "btnDeleteQuote_Click")],
    "btnApprovePurchase": [("Click", "btnApprovePurchase_Click")],
}
