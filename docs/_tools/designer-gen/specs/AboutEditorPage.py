# Spec for src/Company.WebsiteAdmin/Pages/AboutEditorPage.Designer.cs (Figma "admin about us section 1-3", redone 2026-11-01)
# Layout (docked): section choice cards on top; the section form card (left), Live Preview + message (right); save bar.
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "AboutEditorPage"
SIZE = (1240, 960)
LOAD_EVENT = "AboutEditorPage_Load"
CARD = "Company.OfficeApp.UI.Card"
FRAME = "Company.OfficeApp.UI.InputFrame"
SECTION = 'new Font("Segoe UI", 14F)'
FIELD = 'new Font("Segoe UI Semibold", 10F)'
INPUT = 'new Font("Segoe UI", 10.5F)'
BTN = 'new Font("Segoe UI", 10F)'
CHOICE = 'new Font("Segoe UI Semibold", 11F)'
TR = "AnchorStyles.Top | AnchorStyles.Right"
TLR = "AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right"
TOP = "DockStyle.Top"

CONTROLS = [
    (CARD, "cardAboutForm", 24, 136, 752, 712, {"Dock": "DockStyle.Fill", "Padding": "new Padding(24)"}),
    ("Panel", "pnlAboutSide", 776, 136, 440, 712, {"Dock": "DockStyle.Right", "Padding": "new Padding(20, 0, 0, 0)"}),
    ("Panel", "pnlAboutFooter", 24, 848, 1192, 92, {"Dock": "DockStyle.Bottom", "Padding": "new Padding(0, 20, 0, 0)"}),
    ("Panel", "pnlGap1", 24, 116, 1192, 20, {"Dock": TOP}),
    ("Panel", "pnlSections", 24, 8, 1192, 108, {"Dock": TOP}),

    # section choice
    ("Label", "lblSectionsTitle", 0, 4, None, None, {"PARENT": "pnlSections", "Font": SECTION, "Text": T("About Us Page Sections")}),
    ("Button", "btnAboutSection1", 0, 44, 360, 60, {"PARENT": "pnlSections", "Font": CHOICE, "TextAlign": "ContentAlignment.MiddleLeft", "Text": T("ABOUT TRI-STONE  ·  Section 1")}),
    ("Button", "btnAboutSection2", 376, 44, 360, 60, {"PARENT": "pnlSections", "Font": CHOICE, "TextAlign": "ContentAlignment.MiddleLeft", "Text": T("Our Principles  ·  Section 2")}),
    ("Button", "btnAboutSection3", 752, 44, 360, 60, {"PARENT": "pnlSections", "Font": CHOICE, "TextAlign": "ContentAlignment.MiddleLeft", "Text": T("Our Mission and Vision  ·  Section 3")}),

    # form
    ("Label", "lblAboutImage", 24, 18, None, None, {"PARENT": "cardAboutForm", "Font": SECTION, "Text": T("About Tri-Stone - Image")}),
    ("Label", "lblAboutSection", 628, 22, 100, 20, {"PARENT": "cardAboutForm", "Anchor": TR, "TextAlign": "ContentAlignment.MiddleRight", "Text": T("Section 1 of 3")}),
    ("Company.OfficeApp.UI.DropZone", "dzAbout", 24, 60, 704, 180, {"PARENT": "cardAboutForm", "Anchor": TLR}),
    ("Button", "btnRemoveAboutImage", 24, 248, 140, 34, {"PARENT": "cardAboutForm", "Text": T("Remove Picture")}),
    ("Label", "lblAboutImageName", 176, 256, None, None, {"PARENT": "cardAboutForm", "MaximumSize": "new Size(540, 0)"}),
    ("Label", "lblAboutInfo", 24, 296, None, None, {"PARENT": "cardAboutForm", "Font": SECTION, "Text": T("Section Information")}),
    ("Label", "lblAboutHeading", 24, 336, None, None, {"PARENT": "cardAboutForm", "Font": FIELD, "Text": T("Section Heading")}),
    ("Label", "lblAboutHeadingCount", 628, 336, 100, 20, {"PARENT": "cardAboutForm", "Anchor": TR, "TextAlign": "ContentAlignment.MiddleRight", "Text": T("0/100")}),
    (FRAME, "frmAboutHeading", 24, 360, 704, 44, {"PARENT": "cardAboutForm", "Anchor": TLR}),
    ("TextBox", "txtAboutHeading", 12, 11, 680, 23, {"PARENT": "frmAboutHeading", "BorderStyle": "BorderStyle.None", "Font": INPUT}),
    ("Label", "lblAboutText1", 24, 418, None, None, {"PARENT": "cardAboutForm", "Font": FIELD, "Text": T("Section Description")}),
    ("Label", "lblAboutText1Count", 608, 418, 120, 20, {"PARENT": "cardAboutForm", "Anchor": TR, "TextAlign": "ContentAlignment.MiddleRight", "Text": T("0/2000")}),
    (FRAME, "frmAboutText1", 24, 442, 704, 120, {"PARENT": "cardAboutForm", "Anchor": TLR}),
    ("TextBox", "txtAboutText1", 12, 9, 680, 102, {"PARENT": "frmAboutText1", "BorderStyle": "BorderStyle.None", "Font": INPUT, "Multiline": "true"}),
    ("Label", "lblAboutText2", 24, 576, None, None, {"PARENT": "cardAboutForm", "Font": FIELD, "Text": T("Our Mission")}),
    ("Label", "lblAboutText2Count", 608, 576, 120, 20, {"PARENT": "cardAboutForm", "Anchor": TR, "TextAlign": "ContentAlignment.MiddleRight", "Text": T("0/1000")}),
    (FRAME, "frmAboutText2", 24, 600, 704, 88, {"PARENT": "cardAboutForm", "Anchor": TLR}),
    ("TextBox", "txtAboutText2", 12, 9, 680, 70, {"PARENT": "frmAboutText2", "BorderStyle": "BorderStyle.None", "Font": INPUT, "Multiline": "true"}),

    # right: preview + message
    ("Company.OfficeApp.UI.StatusBox", "statusAbout", 20, 346, 420, 78, {"PARENT": "pnlAboutSide", "Dock": TOP}),
    ("Panel", "pnlSideGap", 20, 330, 420, 16, {"PARENT": "pnlAboutSide", "Dock": TOP}),
    (CARD, "cardAboutPreview", 20, 0, 420, 330, {"PARENT": "pnlAboutSide", "Dock": TOP}),
    ("Label", "lblAboutPreviewTitle", 20, 18, None, None, {"PARENT": "cardAboutPreview", "Font": SECTION, "Text": T("Live Preview")}),
    ("Panel", "pnlAboutPreview", 20, 60, 380, 250, {"PARENT": "cardAboutPreview", "Anchor": TLR}),

    # save bar
    (CARD, "cardAboutSave", 0, 20, 1192, 72, {"PARENT": "pnlAboutFooter", "Dock": "DockStyle.Fill", "Padding": "new Padding(16)"}),
    ("Label", "lblAboutLastSaved", 470, 16, 380, 40, {"PARENT": "cardAboutSave", "Anchor": TR, "TextAlign": "ContentAlignment.MiddleRight", "Text": T("Last saved: -")}),
    ("Button", "btnDiscardAbout", 866, 16, 150, 40, {"PARENT": "cardAboutSave", "Anchor": TR, "Font": BTN, "Text": T("Discard Changes")}),
    ("Button", "btnSaveAbout", 1026, 16, 150, 40, {"PARENT": "cardAboutSave", "Anchor": TR, "Font": BTN, "Text": T("Save Changes")}),
]

GRID_COLUMNS = {}

EVENTS = {
    "btnAboutSection1": [("Click", "btnAboutSection_Click")],
    "btnAboutSection2": [("Click", "btnAboutSection_Click")],
    "btnAboutSection3": [("Click", "btnAboutSection_Click")],
    "btnRemoveAboutImage": [("Click", "btnRemoveAboutImage_Click")],
    "txtAboutHeading": [("TextChanged", "Field_Changed")],
    "txtAboutText1": [("TextChanged", "Field_Changed")],
    "txtAboutText2": [("TextChanged", "Field_Changed")],
    "btnSaveAbout": [("Click", "btnSaveAbout_Click")],
    "btnDiscardAbout": [("Click", "btnDiscardAbout_Click")],
}
