# Spec for src/Company.OfficeApp/Pages/Inventory/EquipmentTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Inventory"
CLASS = "EquipmentTab"
SIZE = (960, 660)
LOAD_EVENT = "EquipmentTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy", NullValue = "-" }'

CONTROLS = [
    ("Label", "lblSearch", 12, 15, None, None, {"Text": T("Search")}),
    ("TextBox", "txtEquipmentSearch", 62, 12, 160, 23, {}),
    ("Label", "lblTypeFilter", 234, 15, None, None, {"Text": T("Type")}),
    ("ComboBox", "cboEquipmentTypeFilter", 270, 12, 130, 23, DDL),
    ("Label", "lblStatusFilter", 412, 15, None, None, {"Text": T("Status")}),
    ("ComboBox", "cboEquipmentStatusFilter", 458, 12, 120, 23, DDL),
    ("Label", "lblMaintenanceDue", 592, 15, None, None, {"Text": T("")}),
    ("DataGridView", "dgvEquipment", 12, 44, 930, 180, {}),
    ("Button", "btnAddEquipment", 12, 232, 120, 28, {"Text": T("Add Equipment")}),
    ("Button", "btnDeleteEquipment", 138, 232, 90, 28, {"Text": T("Delete")}),
    ("Button", "btnExportEquipment", 234, 232, 90, 28, {"Text": T("Export")}),
    ("Label", "lblFormMode", 12, 274, None, None, {"Text": T("New equipment")}),
    # form (left)
    ("Label", "lblEquipmentName", 12, 298, None, None, {"Text": T("Name (e.g. Concrete Mixer #2)")}),
    ("TextBox", "txtEquipmentName", 12, 316, 260, 23, {}),
    ("Label", "lblEquipmentType", 12, 346, None, None, {"Text": T("Type")}),
    ("ComboBox", "cboEquipmentType", 12, 364, 160, 23, DDL),
    ("RadioButton", "rdoOwned", 12, 396, None, None, {"Text": T("Owned"), "AutoSize": "true", "Checked": "true"}),
    ("RadioButton", "rdoRented", 90, 396, None, None, {"Text": T("Rented"), "AutoSize": "true"}),
    ("Label", "lblRentalCompany", 12, 424, None, None, {"Text": T("Rental company")}),
    ("TextBox", "txtRentalCompany", 12, 442, 260, 23, {}),
    ("Label", "lblRentalRate", 12, 472, None, None, {"Text": T("Rental rate")}),
    ("NumericUpDown", "numRentalRate", 12, 490, 120, 23, {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "99999999m"}),
    ("ComboBox", "cboRentalRateUnit", 138, 490, 100, 23, DDL),
    ("Label", "lblRentalReturn", 12, 520, None, None, {"Text": T("Return date (tick to set)")}),
    ("DateTimePicker", "dtpRentalReturn", 12, 538, 140, 23, {"Format": "DateTimePickerFormat.Short", "ShowCheckBox": "true"}),
    ("Label", "lblEquipmentStatus", 300, 298, None, None, {"Text": T("Status")}),
    ("ComboBox", "cboEquipmentStatus", 300, 316, 140, 23, DDL),
    ("Label", "lblNextMaintenance", 300, 346, None, None, {"Text": T("Next maintenance (tick to set)")}),
    ("DateTimePicker", "dtpNextMaintenance", 300, 364, 140, 23, {"Format": "DateTimePickerFormat.Short", "ShowCheckBox": "true"}),
    ("Label", "lblEquipmentMsg", 12, 572, None, None, {}),
    ("Button", "btnSaveEquipment", 12, 594, 90, 28, {"Text": T("Save")}),
    ("Button", "btnCancelEquipment", 108, 594, 90, 28, {"Text": T("Cancel")}),
    # maintenance log (right)
    ("Label", "lblLogTitle", 470, 274, None, None, {"Text": T("Maintenance & repair log (selected equipment)")}),
    ("DataGridView", "dgvMaintenanceLog", 470, 298, 472, 220, {}),
    ("Button", "btnAddMaintenance", 470, 526, 130, 28, {"Text": T("Add Log Entry")}),
    ("Button", "btnDeleteMaintenance", 606, 526, 110, 28, {"Text": T("Delete Entry")}),
]

GRID_COLUMNS = {
    "dgvEquipment": [
        (TB, "colName", "Name", "Name", {}),
        (TB, "colType", "Type", "Type", {}),
        (TB, "colOwnership", "Ownership", "Owned / Rented", {}),
        (TB, "colStatus", "Status", "Status", {}),
        (TB, "colNext", "NextMaintenance", "Next Maintenance", {"DefaultCellStyle": DATE}),
    ],
    "dgvMaintenanceLog": [
        (TB, "colLDate", "DoneOn", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colLKind", "Kind", "Type", {}),
        (TB, "colLWork", "WorkDone", "Work Done", {}),
        (TB, "colLCost", "Cost", "Cost", {"DefaultCellStyle": MONEY}),
        (TB, "colLBy", "DoneBy", "Done By", {}),
    ],
}

EVENTS = {
    "cboEquipmentTypeFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "cboEquipmentStatusFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "dgvEquipment": [("SelectionChanged", "dgvEquipment_SelectionChanged"), ("CellFormatting", "dgvEquipment_CellFormatting")],
    "btnAddEquipment": [("Click", "btnAddEquipment_Click")],
    "btnDeleteEquipment": [("Click", "btnDeleteEquipment_Click")],
    "btnExportEquipment": [("Click", "btnExportEquipment_Click")],
    "rdoRented": [("CheckedChanged", "Ownership_Changed")],
    "btnSaveEquipment": [("Click", "btnSaveEquipment_Click")],
    "btnCancelEquipment": [("Click", "btnCancelEquipment_Click")],
    "btnAddMaintenance": [("Click", "btnAddMaintenance_Click")],
    "btnDeleteMaintenance": [("Click", "btnDeleteMaintenance_Click")],
}
