# Spec for src/Company.WebsiteAdmin/Pages/HomeEditorPage.Designer.cs (Figma "ADMIN HOME", redone 2026-11-01 to match it)
# Layout: hero card fills the left, Live Preview + status on the right, save bar along the bottom (docked, so it fits any screen).
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "HomeEditorPage"
SIZE = (1240, 760)
LOAD_EVENT = "HomeEditorPage_Load"
CARD = "Company.OfficeApp.UI.Card"
FRAME = "Company.OfficeApp.UI.InputFrame"
SECTION = 'new Font("Segoe UI", 14F)'
FIELD = 'new Font("Segoe UI Semibold", 10F)'
INPUT = 'new Font("Segoe UI", 10.5F)'
TR = "AnchorStyles.Top | AnchorStyles.Right"
TLR = "AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right"

CONTROLS = [
    # docked: the hero card (Fill) is added first so it takes the space the others leave
    (CARD, "cardHero", 24, 8, 772, 640, {"Dock": "DockStyle.Fill", "Padding": "new Padding(24)"}),
    ("Panel", "pnlHomeSide", 796, 8, 420, 640, {"Dock": "DockStyle.Right", "Padding": "new Padding(20, 0, 0, 0)"}),
    ("Panel", "pnlHomeFooter", 24, 648, 1192, 92, {"Dock": "DockStyle.Bottom", "Padding": "new Padding(0, 20, 0, 0)"}),

    # hero card
    ("Label", "lblHeroTitle", 24, 20, None, None, {"PARENT": "cardHero", "Font": SECTION, "Text": T("Hero Background Image")}),
    ("Company.OfficeApp.UI.DropZone", "dzHero", 24, 62, 724, 250, {"PARENT": "cardHero", "Anchor": TLR}),
    ("Button", "btnRemoveHeroImage", 24, 322, 140, 34, {"PARENT": "cardHero", "Text": T("Remove Picture")}),
    ("Label", "lblHeroImageName", 176, 330, None, None, {"PARENT": "cardHero", "MaximumSize": "new Size(560, 0)"}),
    ("Label", "lblHeroText", 24, 376, None, None, {"PARENT": "cardHero", "Font": SECTION, "Text": T("Hero Background Text Content")}),
    ("Label", "lblHeadline", 24, 418, None, None, {"PARENT": "cardHero", "Font": FIELD, "Text": T("Main Headline text")}),
    ("Label", "lblHeadlineCount", 648, 418, 100, 20, {"PARENT": "cardHero", "Anchor": TR, "TextAlign": "ContentAlignment.MiddleRight", "Text": T("0/150")}),
    (FRAME, "frmHeadline", 24, 442, 724, 44, {"PARENT": "cardHero", "Anchor": TLR}),
    ("TextBox", "txtHeadline", 12, 11, 700, 23, {"PARENT": "frmHeadline", "BorderStyle": "BorderStyle.None", "Font": INPUT}),
    ("Label", "lblSubheadline", 24, 502, None, None, {"PARENT": "cardHero", "Font": FIELD, "Text": T("Subheadline Text")}),
    ("Label", "lblSubheadlineCount", 648, 502, 100, 20, {"PARENT": "cardHero", "Anchor": TR, "TextAlign": "ContentAlignment.MiddleRight", "Text": T("0/300")}),
    (FRAME, "frmSubheadline", 24, 526, 724, 96, {"PARENT": "cardHero", "Anchor": TLR}),
    ("TextBox", "txtSubheadline", 12, 9, 700, 78, {"PARENT": "frmSubheadline", "BorderStyle": "BorderStyle.None", "Font": INPUT,
                                                    "Multiline": "true"}),

    # right column (docked to the top: the last one added sits highest)
    ("Company.OfficeApp.UI.StatusBox", "statusHome", 20, 346, 400, 78, {"PARENT": "pnlHomeSide", "Dock": "DockStyle.Top"}),
    ("Panel", "pnlSideGap", 20, 330, 400, 16, {"PARENT": "pnlHomeSide", "Dock": "DockStyle.Top"}),
    (CARD, "cardPreview", 20, 0, 400, 330, {"PARENT": "pnlHomeSide", "Dock": "DockStyle.Top"}),
    ("Label", "lblPreviewTitle", 20, 18, None, None, {"PARENT": "cardPreview", "Font": SECTION, "Text": T("Live Preview")}),
    ("Panel", "pnlPreview", 20, 60, 360, 210, {"PARENT": "cardPreview", "Anchor": TLR}),
    ("Label", "lblPreviewNote", 20, 282, 360, 36, {"PARENT": "cardPreview", "Anchor": TLR,
                                                  "Text": T("How the top of the website will look after Save Changes and Publish Website.")}),

    # save bar
    (CARD, "cardHomeFooter", 0, 20, 1192, 72, {"PARENT": "pnlHomeFooter", "Dock": "DockStyle.Fill", "Padding": "new Padding(16)"}),
    ("Label", "lblHomeLastSaved", 470, 16, 380, 40, {"PARENT": "cardHomeFooter", "Anchor": TR, "TextAlign": "ContentAlignment.MiddleRight", "Text": T("Last saved: -")}),
    ("Button", "btnDiscardHome", 866, 16, 150, 40, {"PARENT": "cardHomeFooter", "Anchor": TR, "Font": 'new Font("Segoe UI", 10F)', "Text": T("Discard Changes")}),
    ("Button", "btnSaveHome", 1026, 16, 150, 40, {"PARENT": "cardHomeFooter", "Anchor": TR, "Font": 'new Font("Segoe UI", 10F)', "Text": T("Save Changes")}),
]

GRID_COLUMNS = {}

EVENTS = {
    "btnRemoveHeroImage": [("Click", "btnRemoveHeroImage_Click")],
    "txtHeadline": [("TextChanged", "Field_Changed")],
    "txtSubheadline": [("TextChanged", "Field_Changed")],
    "btnSaveHome": [("Click", "btnSaveHome_Click")],
    "btnDiscardHome": [("Click", "btnDiscardHome_Click")],
}
