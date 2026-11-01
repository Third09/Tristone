# Spec for src/Company.WebsiteAdmin/Pages/HomeEditorPage.Designer.cs (Figma "ADMIN HOME")
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "HomeEditorPage"
SIZE = (960, 640)
LOAD_EVENT = "HomeEditorPage_Load"
PIC = {"SizeMode": "PictureBoxSizeMode.Zoom", "BorderStyle": "BorderStyle.FixedSingle"}

CONTROLS = [
    ("Label", "lblHomeHint", 12, 12, None, None, {"Text": T("Manage the Homepage's content displayed on the live website. Save Changes keeps your edits; Publish Website (top) puts them on the live site.")}),
    ("Label", "lblHeroImage", 12, 44, None, None, {"Text": T("Hero background image (JPEG or PNG, 8 MB at most)")}),
    ("PictureBox", "picHeroImage", 12, 64, 480, 270, PIC),
    ("Button", "btnChooseHeroImage", 504, 64, 130, 28, {"Text": T("Choose Picture...")}),
    ("Button", "btnRemoveHeroImage", 504, 98, 130, 28, {"Text": T("Remove Picture")}),
    ("Label", "lblHeroImageName", 504, 134, None, None, {"MaximumSize": "new Size(300, 0)"}),
    ("Label", "lblHeadline", 12, 346, None, None, {"Text": T("Main headline")}),
    ("Label", "lblHeadlineCount", 560, 346, None, None, {"Text": T("0/150")}),
    ("TextBox", "txtHeadline", 12, 364, 600, 23, {}),
    ("Label", "lblSubheadline", 12, 396, None, None, {"Text": T("Subheadline")}),
    ("Label", "lblSubheadlineCount", 560, 396, None, None, {"Text": T("0/300")}),
    ("TextBox", "txtSubheadline", 12, 414, 600, 70, {"Multiline": "true", "ScrollBars": "ScrollBars.Vertical"}),
    ("Label", "lblHomeStatus", 12, 494, None, None, {"MaximumSize": "new Size(600, 0)"}),
    ("Label", "lblHomeLastSaved", 12, 540, None, None, {"Text": T("Last saved: -")}),
    ("Button", "btnSaveHome", 12, 562, 120, 28, {"Text": T("Save Changes")}),
    ("Button", "btnDiscardHome", 138, 562, 130, 28, {"Text": T("Discard Changes")}),
]

GRID_COLUMNS = {}

EVENTS = {
    "btnChooseHeroImage": [("Click", "btnChooseHeroImage_Click")],
    "btnRemoveHeroImage": [("Click", "btnRemoveHeroImage_Click")],
    "txtHeadline": [("TextChanged", "Field_Changed")],
    "txtSubheadline": [("TextChanged", "Field_Changed")],
    "btnSaveHome": [("Click", "btnSaveHome_Click")],
    "btnDiscardHome": [("Click", "btnDiscardHome_Click")],
}
