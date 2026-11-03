# Spec for src/Company.WebsiteAdmin/Pages/ServicesEditorPage.Designer.cs (Figma "admin services", redone 2026-11-01)
# Layout (docked): services list card on top; below it the service form card (left), Live Preview + message (right); save bar.
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "ServicesEditorPage"
SIZE = (1240, 960)
LOAD_EVENT = "ServicesEditorPage_Load"
CARD = "Company.OfficeApp.UI.Card"
FRAME = "Company.OfficeApp.UI.InputFrame"
SECTION = 'new Font("Segoe UI", 14F)'
FIELD = 'new Font("Segoe UI Semibold", 10F)'
INPUT = 'new Font("Segoe UI", 10.5F)'
BTN = 'new Font("Segoe UI", 10F)'
TR = "AnchorStyles.Top | AnchorStyles.Right"
TLR = "AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right"
TOP = "DockStyle.Top"

CONTROLS = [
    (CARD, "cardServiceForm", 24, 328, 752, 520, {"Dock": "DockStyle.Fill", "Padding": "new Padding(24)"}),
    ("Panel", "pnlServicesSide", 776, 328, 440, 520, {"Dock": "DockStyle.Right", "Padding": "new Padding(20, 0, 0, 0)"}),
    ("Panel", "pnlServicesFooter", 24, 848, 1192, 92, {"Dock": "DockStyle.Bottom", "Padding": "new Padding(0, 20, 0, 0)"}),
    ("Panel", "pnlGap1", 24, 308, 1192, 20, {"Dock": TOP}),
    (CARD, "cardServicesList", 24, 8, 1192, 300, {"Dock": TOP, "Padding": "new Padding(24)"}),

    # list
    ("Label", "lblServicesTitle", 24, 18, None, None, {"PARENT": "cardServicesList", "Font": SECTION, "Text": T("Featured Services Slots")}),
    ("Label", "lblServicesHint", 24, 48, None, None, {"PARENT": "cardServicesList", "Text": T("The service cards on the website, in this order. Each card's icon is fixed by its service.")}),
    ("Button", "btnAddWebService", 978, 18, 190, 40, {"PARENT": "cardServicesList", "Anchor": TR, "Font": BTN, "Text": T("Add New Service")}),
    ("DataGridView", "dgvWebServices", 24, 78, 1144, 150, {"PARENT": "cardServicesList", "Anchor": TLR}),
    ("Button", "btnMoveServiceUp", 24, 242, 110, 36, {"PARENT": "cardServicesList", "Text": T("Move Up")}),
    ("Button", "btnMoveServiceDown", 142, 242, 110, 36, {"PARENT": "cardServicesList", "Text": T("Move Down")}),
    ("Button", "btnHideShowService", 260, 242, 130, 36, {"PARENT": "cardServicesList", "Text": T("Unpublish")}),
    ("Button", "btnDeleteWebService", 398, 242, 110, 36, {"PARENT": "cardServicesList", "Text": T("Delete")}),

    # form
    ("Label", "lblFormMode", 24, 18, None, None, {"PARENT": "cardServiceForm", "Font": SECTION, "Text": T("Service Image")}),
    ("Company.OfficeApp.UI.DropZone", "dzService", 24, 60, 704, 180, {"PARENT": "cardServiceForm", "Anchor": TLR}),
    ("Label", "lblServiceImageName", 24, 248, None, None, {"PARENT": "cardServiceForm", "MaximumSize": "new Size(700, 0)"}),
    ("Label", "lblServiceInfo", 24, 278, None, None, {"PARENT": "cardServiceForm", "Font": SECTION, "Text": T("Service Information")}),
    ("Label", "lblServiceName", 24, 318, None, None, {"PARENT": "cardServiceForm", "Font": FIELD, "Text": T("Name of Service")}),
    ("ComboBox", "cboServiceName", 24, 342, 340, 27, dict(PARENT="cardServiceForm", Font=INPUT, MaxLength=60)),   # typeable: any service (2026-11-03)
    ("Label", "lblServiceSector", 388, 318, None, None, {"PARENT": "cardServiceForm", "Font": FIELD, "Text": T("Category")}),
    ("ComboBox", "cboServiceSector", 388, 342, 340, 27, dict(DDL, PARENT="cardServiceForm", Font=INPUT, Anchor=TLR)),
    ("Label", "lblServiceLocation", 24, 384, None, None, {"PARENT": "cardServiceForm", "Font": FIELD, "Text": T("Location (e.g. San Juan City)")}),
    (FRAME, "frmServiceLocation", 24, 408, 704, 44, {"PARENT": "cardServiceForm", "Anchor": TLR}),
    ("TextBox", "txtServiceLocation", 12, 11, 680, 23, {"PARENT": "frmServiceLocation", "BorderStyle": "BorderStyle.None", "Font": INPUT}),
    ("Label", "lblServiceDesc", 24, 464, None, None, {"PARENT": "cardServiceForm", "Font": FIELD, "Text": T("Short Description")}),
    ("Label", "lblServiceDescCount", 628, 464, 100, 20, {"PARENT": "cardServiceForm", "Anchor": TR, "TextAlign": "ContentAlignment.MiddleRight", "Text": T("0/100")}),
    (FRAME, "frmServiceDesc", 24, 488, 704, 44, {"PARENT": "cardServiceForm", "Anchor": TLR}),
    ("TextBox", "txtServiceDesc", 12, 11, 680, 23, {"PARENT": "frmServiceDesc", "BorderStyle": "BorderStyle.None", "Font": INPUT}),

    # right: preview + message (docked top; last added sits highest)
    ("Company.OfficeApp.UI.StatusBox", "statusService", 20, 346, 420, 78, {"PARENT": "pnlServicesSide", "Dock": TOP}),
    ("Panel", "pnlSideGap", 20, 330, 420, 16, {"PARENT": "pnlServicesSide", "Dock": TOP}),
    (CARD, "cardServicePreview", 20, 0, 420, 330, {"PARENT": "pnlServicesSide", "Dock": TOP}),
    ("Label", "lblServicePreviewTitle", 20, 18, None, None, {"PARENT": "cardServicePreview", "Font": SECTION, "Text": T("Live Preview")}),
    ("Panel", "pnlServicePreview", 20, 60, 380, 250, {"PARENT": "cardServicePreview", "Anchor": TLR}),

    # save bar
    (CARD, "cardServicesSave", 0, 20, 1192, 72, {"PARENT": "pnlServicesFooter", "Dock": "DockStyle.Fill", "Padding": "new Padding(16)"}),
    ("Label", "lblServiceMsg", 24, 16, 800, 40, {"PARENT": "cardServicesSave", "Anchor": TLR, "TextAlign": "ContentAlignment.MiddleLeft"}),
    ("Button", "btnCancelWebService", 866, 16, 150, 40, {"PARENT": "cardServicesSave", "Anchor": TR, "Font": BTN, "Text": T("Discard Changes")}),
    ("Button", "btnSaveWebService", 1026, 16, 150, 40, {"PARENT": "cardServicesSave", "Anchor": TR, "Font": BTN, "Text": T("Save Changes")}),
]

GRID_COLUMNS = {
    "dgvWebServices": [
        (TB, "colSlot", "Slot", "Slot", {"FillWeight": "30"}),
        (TB, "colServiceName", "ServiceName", "Service", {}),
        (TB, "colSector", "Sector", "Category", {}),
        (TB, "colLocation", "Location", "Location", {}),
        (TB, "colShown", "Shown", "On Website", {"FillWeight": "50"}),
    ],
}

EVENTS = {
    "dgvWebServices": [("SelectionChanged", "dgvWebServices_SelectionChanged")],
    "btnAddWebService": [("Click", "btnAddWebService_Click")],
    "btnMoveServiceUp": [("Click", "btnMoveServiceUp_Click")],
    "btnMoveServiceDown": [("Click", "btnMoveServiceDown_Click")],
    "btnHideShowService": [("Click", "btnHideShowService_Click")],
    "btnDeleteWebService": [("Click", "btnDeleteWebService_Click")],
    "txtServiceDesc": [("TextChanged", "Field_Changed")],
    "txtServiceLocation": [("TextChanged", "Field_Changed")],
    "cboServiceName": [("TextChanged", "Field_Changed")],
    "cboServiceSector": [("SelectedIndexChanged", "Field_Changed")],
    "btnSaveWebService": [("Click", "btnSaveWebService_Click")],
    "btnCancelWebService": [("Click", "btnCancelWebService_Click")],
}
