# Spec for src/Company.OfficeApp/Pages/Subcontractors/SubDirectoryTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Subcontractors"
CLASS = "SubDirectoryTab"
SIZE = (960, 720)
LOAD_EVENT = "SubDirectoryTab_Load"
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy", NullValue = "-" }'
ML = {"Multiline": "true", "ScrollBars": "ScrollBars.Vertical"}

CONTROLS = [
    ("Label", "lblSearch", 12, 15, None, None, {"Text": T("Search")}),
    ("TextBox", "txtSubSearch", 62, 12, 180, 23, {}),
    ("Label", "lblTradeFilter", 254, 15, None, None, {"Text": T("Trade")}),
    ("ComboBox", "cboSubTradeFilter", 294, 12, 200, 23, DDL),
    ("Label", "lblPcabDue", 506, 15, None, None, {"Text": T("")}),
    ("DataGridView", "dgvSubcontractors", 12, 44, 930, 170, {}),
    ("Button", "btnAddSubcontractor", 12, 222, 140, 28, {"Text": T("Add Subcontractor")}),
    ("Button", "btnDeleteSubcontractor", 158, 222, 90, 28, {"Text": T("Delete")}),
    ("Button", "btnExportSubcontractors", 254, 222, 90, 28, {"Text": T("Export")}),
    ("Label", "lblFormMode", 12, 264, None, None, {"Text": T("New subcontractor")}),
    # company, address, bank (left)
    ("Label", "lblSubBusinessName", 12, 288, None, None, {"Text": T("Official business name")}),
    ("TextBox", "txtSubBusinessName", 12, 306, 260, 23, {}),
    ("Label", "lblSubTradeName", 12, 336, None, None, {"Text": T("Trade name (DBA)")}),
    ("TextBox", "txtSubTradeName", 12, 354, 260, 23, {}),
    ("Label", "lblSubDtiSec", 12, 384, None, None, {"Text": T("DTI / SEC reg. no.")}),
    ("TextBox", "txtSubDtiSec", 12, 402, 125, 23, {}),
    ("Label", "lblSubTin", 147, 384, None, None, {"Text": T("TIN")}),
    ("TextBox", "txtSubTin", 147, 402, 125, 23, {}),
    ("Label", "lblSubOfficeAddress", 12, 432, None, None, {"Text": T("Main office address")}),
    ("TextBox", "txtSubOfficeAddress", 12, 450, 260, 38, ML),
    ("Label", "lblSubYardAddress", 12, 494, None, None, {"Text": T("Warehouse / yard location")}),
    ("TextBox", "txtSubYardAddress", 12, 512, 260, 38, ML),
    ("Label", "lblSubBankName", 12, 556, None, None, {"Text": T("Bank name")}),
    ("TextBox", "txtSubBankName", 12, 574, 125, 23, {}),
    ("Label", "lblSubAccountName", 147, 556, None, None, {"Text": T("Account name")}),
    ("TextBox", "txtSubAccountName", 147, 574, 125, 23, {}),
    ("Label", "lblSubAccountNo", 12, 604, None, None, {"Text": T("Account number")}),
    ("TextBox", "txtSubAccountNo", 12, 622, 260, 23, {}),
    # contacts (middle)
    ("Label", "lblPrimaryName", 300, 288, None, None, {"Text": T("Primary contact: name")}),
    ("TextBox", "txtPrimaryName", 300, 306, 145, 23, {}),
    ("Label", "lblPrimaryRole", 455, 288, None, None, {"Text": T("Designation / role")}),
    ("TextBox", "txtPrimaryRole", 455, 306, 145, 23, {}),
    ("Label", "lblPrimaryPhone", 300, 336, None, None, {"Text": T("Phone")}),
    ("TextBox", "txtPrimaryPhone", 300, 354, 145, 23, {}),
    ("Label", "lblPrimaryMobile", 455, 336, None, None, {"Text": T("Mobile")}),
    ("TextBox", "txtPrimaryMobile", 455, 354, 145, 23, {}),
    ("Label", "lblPrimaryEmail", 300, 384, None, None, {"Text": T("Email")}),
    ("TextBox", "txtPrimaryEmail", 300, 402, 300, 23, {}),
    ("Label", "lblSecondaryName", 300, 432, None, None, {"Text": T("Backup contact: name")}),
    ("TextBox", "txtSecondaryName", 300, 450, 145, 23, {}),
    ("Label", "lblSecondaryRole", 455, 432, None, None, {"Text": T("Role")}),
    ("TextBox", "txtSecondaryRole", 455, 450, 145, 23, {}),
    ("Label", "lblSecondaryPhone", 300, 480, None, None, {"Text": T("Phone")}),
    ("TextBox", "txtSecondaryPhone", 300, 498, 145, 23, {}),
    ("Label", "lblSecondaryMobile", 455, 480, None, None, {"Text": T("Mobile")}),
    ("TextBox", "txtSecondaryMobile", 455, 498, 145, 23, {}),
    ("Label", "lblSecondaryEmail", 300, 528, None, None, {"Text": T("Email")}),
    ("TextBox", "txtSecondaryEmail", 300, 546, 300, 23, {}),
    # trades and license (right)
    ("Label", "lblSubTrades", 628, 288, None, None, {"Text": T("Trade categories (tick all that apply)")}),
    ("CheckedListBox", "clbSubTrades", 628, 306, 300, 150, {"CheckOnClick": "true"}),
    ("Label", "lblPcabNumber", 628, 466, None, None, {"Text": T("PCAB license no.")}),
    ("TextBox", "txtPcabNumber", 628, 484, 145, 23, {}),
    ("Label", "lblPcabClassification", 783, 466, None, None, {"Text": T("Classification")}),
    ("TextBox", "txtPcabClassification", 783, 484, 145, 23, {}),
    ("Label", "lblPcabExpiry", 628, 514, None, None, {"Text": T("PCAB expiry (tick to set)")}),
    ("DateTimePicker", "dtpPcabExpiry", 628, 532, 140, 23, {"Format": "DateTimePickerFormat.Short", "ShowCheckBox": "true"}),
    ("Label", "lblSubMsg", 12, 656, None, None, {}),
    ("Button", "btnSaveSubcontractor", 12, 678, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelSubcontractor", 108, 678, 90, 28, {"Text": T("Cancel")}),
]

GRID_COLUMNS = {
    "dgvSubcontractors": [
        (TB, "colBusinessName", "BusinessName", "Business Name", {}),
        (TB, "colTrades", "Trades", "Trades", {}),
        (TB, "colPrimaryContact", "PrimaryContact", "Primary Contact", {}),
        (TB, "colPhone", "Phone", "Phone", {}),
        (TB, "colPcabExpiry", "PcabExpiry", "PCAB Expiry", {"DefaultCellStyle": DATE}),
    ],
}

EVENTS = {
    "cboSubTradeFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "dgvSubcontractors": [("SelectionChanged", "dgvSubcontractors_SelectionChanged"), ("CellFormatting", "dgvSubcontractors_CellFormatting")],
    "btnAddSubcontractor": [("Click", "btnAddSubcontractor_Click")],
    "btnDeleteSubcontractor": [("Click", "btnDeleteSubcontractor_Click")],
    "btnExportSubcontractors": [("Click", "btnExportSubcontractors_Click")],
    "btnSaveSubcontractor": [("Click", "btnSaveSubcontractor_Click")],
    "btnCancelSubcontractor": [("Click", "btnCancelSubcontractor_Click")],
}
