# Spec for src/Company.OfficeApp/Pages/Inventory/VendorsTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Inventory"
CLASS = "VendorsTab"
SIZE = (960, 660)
LOAD_EVENT = "VendorsTab_Load"
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'

CONTROLS = [
    ("Label", "lblSearch", 12, 15, None, None, {"Text": T("Search")}),
    ("TextBox", "txtVendorSearch", 62, 12, 200, 23, {}),
    ("DataGridView", "dgvVendors", 12, 44, 930, 200, {}),
    ("Button", "btnAddVendor", 12, 252, 100, 28, {"Text": T("Add Vendor")}),
    ("Button", "btnDeleteVendor", 118, 252, 90, 28, {"Text": T("Delete")}),
    ("Button", "btnExportVendors", 214, 252, 90, 28, {"Text": T("Export")}),
    ("Label", "lblFormMode", 12, 296, None, None, {"Text": T("New vendor")}),
    # details (left)
    ("Label", "lblVendorName", 12, 320, None, None, {"Text": T("Vendor / supplier name")}),
    ("TextBox", "txtVendorName", 12, 338, 300, 23, {}),
    ("Label", "lblVendorContact", 12, 368, None, None, {"Text": T("Contact person")}),
    ("TextBox", "txtVendorContact", 12, 386, 300, 23, {}),
    ("Label", "lblVendorPhone", 12, 416, None, None, {"Text": T("Phone")}),
    ("TextBox", "txtVendorPhone", 12, 434, 300, 23, {}),
    ("Label", "lblVendorEmail", 12, 464, None, None, {"Text": T("Email")}),
    ("TextBox", "txtVendorEmail", 12, 482, 300, 23, {}),
    ("Label", "lblVendorAddress", 12, 512, None, None, {"Text": T("Address")}),
    ("TextBox", "txtVendorAddress", 12, 530, 300, 46, {"Multiline": "true"}),
    # bank (middle)
    ("Label", "lblVendorBank", 340, 320, None, None, {"Text": T("Bank name")}),
    ("TextBox", "txtVendorBank", 340, 338, 240, 23, {}),
    ("Label", "lblVendorAccountName", 340, 368, None, None, {"Text": T("Account name")}),
    ("TextBox", "txtVendorAccountName", 340, 386, 240, 23, {}),
    ("Label", "lblVendorAccountNo", 340, 416, None, None, {"Text": T("Account number")}),
    ("TextBox", "txtVendorAccountNo", 340, 434, 240, 23, {}),
    ("Label", "lblVendorVat", 340, 464, None, None, {"Text": T("VAT status")}),
    ("ComboBox", "cboVendorVat", 340, 482, 140, 23, DDL),
    # papers (right)
    ("Label", "lblPapers", 610, 320, None, None, {"Text": T("Vendor papers (save the vendor first)")}),
    ("DataGridView", "dgvVendorDocs", 610, 338, 332, 140, {}),
    ("ComboBox", "cboVendorDocType", 610, 486, 200, 23, DDL),
    ("Button", "btnUploadVendorDoc", 816, 484, 126, 28, {"Text": T("Upload Paper")}),
    ("Button", "btnViewVendorDoc", 610, 516, 90, 28, {"Text": T("View")}),
    ("Button", "btnDeleteVendorDoc", 706, 516, 104, 28, {"Text": T("Delete Paper")}),
    # bottom
    ("Label", "lblVendorMsg", 12, 586, None, None, {}),
    ("Button", "btnSaveVendor", 12, 608, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelVendor", 108, 608, 90, 28, {"Text": T("Cancel")}),
]

GRID_COLUMNS = {
    "dgvVendors": [
        (TB, "colName", "Name", "Name", {}),
        (TB, "colContact", "ContactPerson", "Contact Person", {}),
        (TB, "colPhone", "Phone", "Phone", {}),
        (TB, "colVat", "VatStatus", "VAT Status", {}),
    ],
    "dgvVendorDocs": [
        (TB, "colDocType", "DocType", "Paper", {}),
        (TB, "colDocFile", "OriginalName", "File", {}),
        (TB, "colDocDate", "UploadedAt", "Uploaded", {"DefaultCellStyle": DATE}),
    ],
}

EVENTS = {
    "dgvVendors": [("SelectionChanged", "dgvVendors_SelectionChanged")],
    "btnAddVendor": [("Click", "btnAddVendor_Click")],
    "btnDeleteVendor": [("Click", "btnDeleteVendor_Click")],
    "btnExportVendors": [("Click", "btnExportVendors_Click")],
    "btnUploadVendorDoc": [("Click", "btnUploadVendorDoc_Click")],
    "btnViewVendorDoc": [("Click", "btnViewVendorDoc_Click")],
    "btnDeleteVendorDoc": [("Click", "btnDeleteVendorDoc_Click")],
    "btnSaveVendor": [("Click", "btnSaveVendor_Click")],
    "btnCancelVendor": [("Click", "btnCancelVendor_Click")],
}
