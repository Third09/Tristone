# Spec for src/Company.WebsiteAdmin/Pages/AboutEditorPage.Designer.cs (Figma "admin about us section 1-3")
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "AboutEditorPage"
SIZE = (960, 700)
LOAD_EVENT = "AboutEditorPage_Load"
PIC = {"SizeMode": "PictureBoxSizeMode.Zoom", "BorderStyle": "BorderStyle.FixedSingle"}
MULTI = {"Multiline": "true", "ScrollBars": "ScrollBars.Vertical"}

CONTROLS = [
    ("Label", "lblAboutHint", 12, 12, None, None, {"Text": T("Manage company information, principles, vision and mission displayed on the live website.")}),
    ("Button", "btnAboutSection1", 12, 38, 190, 28, {"Text": T("1. About Tri-Stone")}),
    ("Button", "btnAboutSection2", 208, 38, 190, 28, {"Text": T("2. Our Principles")}),
    ("Button", "btnAboutSection3", 404, 38, 190, 28, {"Text": T("3. Our Mission and Vision")}),
    ("Label", "lblAboutSection", 12, 78, None, None, {"Text": T("Section 1 of 3")}),
    ("Label", "lblAboutImage", 12, 100, None, None, {"Text": T("Section image (JPEG or PNG, 8 MB at most)")}),
    ("PictureBox", "picAboutImage", 12, 120, 400, 225, PIC),
    ("Button", "btnChooseAboutImage", 424, 120, 130, 28, {"Text": T("Choose Picture...")}),
    ("Button", "btnRemoveAboutImage", 424, 154, 130, 28, {"Text": T("Remove Picture")}),
    ("Label", "lblAboutImageName", 424, 190, None, None, {"MaximumSize": "new Size(300, 0)"}),
    ("Label", "lblAboutHeading", 12, 356, None, None, {"Text": T("Section heading")}),
    ("Label", "lblAboutHeadingCount", 560, 356, None, None, {"Text": T("0/100")}),
    ("TextBox", "txtAboutHeading", 12, 374, 600, 23, {}),
    ("Label", "lblAboutText1", 12, 406, None, None, {"Text": T("Section description")}),
    ("Label", "lblAboutText1Count", 560, 406, None, None, {"Text": T("0/2000")}),
    ("TextBox", "txtAboutText1", 12, 424, 600, 90, MULTI),
    ("Label", "lblAboutText2", 12, 522, None, None, {"Text": T("Our Mission")}),
    ("Label", "lblAboutText2Count", 560, 522, None, None, {"Text": T("0/1000")}),
    ("TextBox", "txtAboutText2", 12, 540, 600, 60, MULTI),
    ("Label", "lblAboutStatus", 12, 608, None, None, {"MaximumSize": "new Size(600, 0)"}),
    ("Label", "lblAboutLastSaved", 640, 608, None, None, {"Text": T("Last saved: -")}),
    ("Button", "btnSaveAbout", 12, 640, 120, 28, {"Text": T("Save Changes")}),
    ("Button", "btnDiscardAbout", 138, 640, 130, 28, {"Text": T("Discard Changes")}),
]

GRID_COLUMNS = {}

EVENTS = {
    "btnAboutSection1": [("Click", "btnAboutSection_Click")],
    "btnAboutSection2": [("Click", "btnAboutSection_Click")],
    "btnAboutSection3": [("Click", "btnAboutSection_Click")],
    "btnChooseAboutImage": [("Click", "btnChooseAboutImage_Click")],
    "btnRemoveAboutImage": [("Click", "btnRemoveAboutImage_Click")],
    "txtAboutHeading": [("TextChanged", "Field_Changed")],
    "txtAboutText1": [("TextChanged", "Field_Changed")],
    "txtAboutText2": [("TextChanged", "Field_Changed")],
    "btnSaveAbout": [("Click", "btnSaveAbout_Click")],
    "btnDiscardAbout": [("Click", "btnDiscardAbout_Click")],
}
