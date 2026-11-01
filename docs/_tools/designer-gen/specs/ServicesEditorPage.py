# Spec for src/Company.WebsiteAdmin/Pages/ServicesEditorPage.Designer.cs (Figma "admin services")
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "ServicesEditorPage"
SIZE = (960, 640)
LOAD_EVENT = "ServicesEditorPage_Load"
PIC = {"SizeMode": "PictureBoxSizeMode.Zoom", "BorderStyle": "BorderStyle.FixedSingle"}

CONTROLS = [
    ("Label", "lblServicesHint", 12, 12, None, None, {"Text": T("The service cards on the website, in this order. Each card's icon is fixed by its service.")}),
    ("DataGridView", "dgvWebServices", 12, 36, 930, 160, {}),
    ("Button", "btnAddWebService", 12, 204, 130, 28, {"Text": T("Add New Service")}),
    ("Button", "btnMoveServiceUp", 148, 204, 80, 28, {"Text": T("Move Up")}),
    ("Button", "btnMoveServiceDown", 234, 204, 90, 28, {"Text": T("Move Down")}),
    ("Button", "btnHideShowService", 330, 204, 110, 28, {"Text": T("Unpublish")}),
    ("Button", "btnDeleteWebService", 446, 204, 90, 28, {"Text": T("Delete")}),
    ("Label", "lblFormMode", 12, 248, None, None, {"Text": T("New service")}),
    ("Label", "lblServiceName", 12, 272, None, None, {"Text": T("Name of service")}),
    ("ComboBox", "cboServiceName", 12, 290, 300, 23, DDL),
    ("Label", "lblServiceSector", 12, 320, None, None, {"Text": T("Category")}),
    ("ComboBox", "cboServiceSector", 12, 338, 300, 23, DDL),
    ("Label", "lblServiceLocation", 12, 368, None, None, {"Text": T("Location (e.g. San Juan City)")}),
    ("TextBox", "txtServiceLocation", 12, 386, 300, 23, {}),
    ("Label", "lblServiceDesc", 12, 416, None, None, {"Text": T("Short description")}),
    ("Label", "lblServiceDescCount", 360, 416, None, None, {"Text": T("0/100")}),
    ("TextBox", "txtServiceDesc", 12, 434, 400, 23, {}),
    ("Label", "lblServiceImage", 440, 272, None, None, {"Text": T("Service image (JPEG or PNG, 8 MB at most)")}),
    ("PictureBox", "picServiceImage", 440, 290, 320, 180, PIC),
    ("Button", "btnChooseServiceImage", 772, 290, 130, 28, {"Text": T("Choose Picture...")}),
    ("Label", "lblServiceImageName", 772, 326, None, None, {"MaximumSize": "new Size(170, 0)"}),
    ("Label", "lblServiceMsg", 12, 480, None, None, {"MaximumSize": "new Size(740, 0)"}),
    ("Button", "btnSaveWebService", 12, 504, 120, 28, {"Text": T("Save Changes")}),
    ("Button", "btnCancelWebService", 138, 504, 130, 28, {"Text": T("Discard Changes")}),
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
    "txtServiceDesc": [("TextChanged", "txtServiceDesc_TextChanged")],
    "btnChooseServiceImage": [("Click", "btnChooseServiceImage_Click")],
    "btnSaveWebService": [("Click", "btnSaveWebService_Click")],
    "btnCancelWebService": [("Click", "btnCancelWebService_Click")],
}
