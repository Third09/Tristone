# Spec for src/Company.WebsiteAdmin/Pages/ProjectsEditorPage.Designer.cs (Figma "ADMIN PROJECTS" + uploading UI, redone 2026-11-01)
# Layout (docked, top to bottom): featured projects list card, Project Images card, Project Information card, save bar.
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "ProjectsEditorPage"
SIZE = (1240, 1000)
LOAD_EVENT = "ProjectsEditorPage_Load"
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
    # docked parts: Fill first, then the bottom bar, then the top cards from the bottom up
    (CARD, "cardProjectInfo", 24, 698, 1192, 170, {"Dock": "DockStyle.Fill", "Padding": "new Padding(24)"}),
    ("Panel", "pnlProjectsFooter", 24, 888, 1192, 92, {"Dock": "DockStyle.Bottom", "Padding": "new Padding(0, 20, 0, 0)"}),
    ("Panel", "pnlGap2", 24, 678, 1192, 20, {"Dock": TOP}),
    (CARD, "cardPhotos", 24, 340, 1192, 338, {"Dock": TOP, "Padding": "new Padding(24)"}),
    ("Panel", "pnlGap1", 24, 320, 1192, 20, {"Dock": TOP}),
    (CARD, "cardProjectsList", 24, 8, 1192, 312, {"Dock": TOP, "Padding": "new Padding(24)"}),

    # featured projects list
    ("Label", "lblProjectsTitle", 24, 18, None, None, {"PARENT": "cardProjectsList", "Font": SECTION, "Text": T("Featured Projects Slots")}),
    ("Label", "lblProjectsHint", 24, 48, None, None, {"PARENT": "cardProjectsList", "Text": T("From the office Projects list (ticked \"Show on website\"). The website shows them in this order.")}),
    ("Button", "btnAddWebProject", 978, 18, 190, 40, {"PARENT": "cardProjectsList", "Anchor": TR, "Font": BTN, "Text": T("Add New Project")}),
    ("DataGridView", "dgvWebProjects", 24, 78, 1144, 160, {"PARENT": "cardProjectsList", "Anchor": TLR}),
    ("Button", "btnMoveProjectUp", 24, 252, 110, 36, {"PARENT": "cardProjectsList", "Text": T("Move Up")}),
    ("Button", "btnMoveProjectDown", 142, 252, 110, 36, {"PARENT": "cardProjectsList", "Text": T("Move Down")}),
    ("Button", "btnHideShowProject", 260, 252, 130, 36, {"PARENT": "cardProjectsList", "Text": T("Unpublish")}),
    ("Button", "btnDeleteWebProject", 398, 252, 110, 36, {"PARENT": "cardProjectsList", "Text": T("Delete")}),

    # project images
    ("Label", "lblPhotosTitle", 24, 18, None, None, {"PARENT": "cardPhotos", "Font": SECTION, "Text": T("Project Images")}),
    ("Label", "lblPhotos", 24, 48, None, None, {"PARENT": "cardPhotos", "Text": T("Up to 10 photos, JPEG or PNG, 8 MB each.")}),
    ("Company.OfficeApp.UI.DropZone", "dzPhotos", 24, 78, 430, 200, {"PARENT": "cardPhotos"}),
    ("ProgressBar", "prgUpload", 24, 286, 430, 8, {"PARENT": "cardPhotos", "Visible": "false"}),
    ("Label", "lblUploadStatus", 24, 298, 430, 32, {"PARENT": "cardPhotos"}),
    ("Label", "lblShowcaseTitle", 478, 18, None, None, {"PARENT": "cardPhotos", "Font": SECTION, "Text": T("Showcase Images")}),
    ("Label", "lblShowcaseHint", 478, 48, None, None, {"PARENT": "cardPhotos", "Text": T("Select and arrange the images shown in the project carousel.")}),
    ("DataGridView", "dgvPhotos", 478, 78, 400, 236, {"PARENT": "cardPhotos", "Anchor": TLR}),
    ("PictureBox", "picPhotoPreview", 898, 78, 270, 150, {"PARENT": "cardPhotos", "Anchor": TR, "SizeMode": "PictureBoxSizeMode.Zoom"}),
    ("Button", "btnToggleShowcase", 898, 236, 270, 36, {"PARENT": "cardPhotos", "Anchor": TR, "Text": T("Add to Showcase")}),
    ("Button", "btnPhotoUp", 898, 278, 80, 36, {"PARENT": "cardPhotos", "Anchor": TR, "Text": T("Up")}),
    ("Button", "btnPhotoDown", 984, 278, 86, 36, {"PARENT": "cardPhotos", "Anchor": TR, "Text": T("Down")}),
    ("Button", "btnRemovePhoto", 1076, 278, 92, 36, {"PARENT": "cardPhotos", "Anchor": TR, "Text": T("Remove")}),

    # project information
    ("Label", "lblSlotTitle", 24, 18, None, None, {"PARENT": "cardProjectInfo", "Font": SECTION, "Text": T("Project Information")}),
    ("Label", "lblProjectInfo", 24, 50, None, None, {"PARENT": "cardProjectInfo", "Text": T("Click a project in the list to edit it.")}),
    ("Label", "lblProjectDesc", 24, 80, None, None, {"PARENT": "cardProjectInfo", "Font": FIELD, "Text": T("Project Short Description")}),
    ("Label", "lblProjectDescCount", 1068, 80, 100, 20, {"PARENT": "cardProjectInfo", "Anchor": TR, "TextAlign": "ContentAlignment.MiddleRight", "Text": T("0/100")}),
    (FRAME, "frmProjectDesc", 24, 104, 1144, 44, {"PARENT": "cardProjectInfo", "Anchor": TLR}),
    ("TextBox", "txtProjectDesc", 12, 11, 1120, 23, {"PARENT": "frmProjectDesc", "BorderStyle": "BorderStyle.None", "Font": INPUT}),

    # save bar
    (CARD, "cardProjectsSave", 0, 20, 1192, 72, {"PARENT": "pnlProjectsFooter", "Dock": "DockStyle.Fill", "Padding": "new Padding(16)"}),
    ("Label", "lblWebProjectMsg", 24, 16, 800, 40, {"PARENT": "cardProjectsSave", "Anchor": TLR, "TextAlign": "ContentAlignment.MiddleLeft"}),
    ("Button", "btnDiscardWebProject", 866, 16, 150, 40, {"PARENT": "cardProjectsSave", "Anchor": TR, "Font": BTN, "Text": T("Discard Changes")}),
    ("Button", "btnSaveWebProject", 1026, 16, 150, 40, {"PARENT": "cardProjectsSave", "Anchor": TR, "Font": BTN, "Text": T("Save Changes")}),
]

GRID_COLUMNS = {
    "dgvWebProjects": [
        (TB, "colSlot", "Slot", "Slot", {"FillWeight": "30"}),
        (TB, "colProject", "ProjectName", "Project", {}),
        (TB, "colLocation", "Location", "Location", {}),
        (TB, "colSector", "Sector", "Category", {}),
        (TB, "colShown", "Shown", "On Website", {"FillWeight": "50"}),
        (TB, "colPhotoCount", "Photos", "Photos", {"FillWeight": "40"}),
    ],
    "dgvPhotos": [
        (TB, "colPhotoOrder", "Order", "#", {"FillWeight": "20"}),
        (TB, "colPhotoFile", "FileName", "File", {}),
        (TB, "colPhotoSize", "Size", "Size", {"FillWeight": "40"}),
        (TB, "colPhotoShowcase", "Showcase", "Showcase", {"FillWeight": "45"}),
    ],
}

EVENTS = {
    "dgvWebProjects": [("SelectionChanged", "dgvWebProjects_SelectionChanged")],
    "btnAddWebProject": [("Click", "btnAddWebProject_Click")],
    "btnMoveProjectUp": [("Click", "btnMoveProjectUp_Click")],
    "btnMoveProjectDown": [("Click", "btnMoveProjectDown_Click")],
    "btnHideShowProject": [("Click", "btnHideShowProject_Click")],
    "btnDeleteWebProject": [("Click", "btnDeleteWebProject_Click")],
    "txtProjectDesc": [("TextChanged", "txtProjectDesc_TextChanged")],
    "btnSaveWebProject": [("Click", "btnSaveWebProject_Click")],
    "btnDiscardWebProject": [("Click", "btnDiscardWebProject_Click")],
    "dgvPhotos": [("SelectionChanged", "dgvPhotos_SelectionChanged")],
    "btnRemovePhoto": [("Click", "btnRemovePhoto_Click")],
    "btnPhotoUp": [("Click", "btnPhotoUp_Click")],
    "btnPhotoDown": [("Click", "btnPhotoDown_Click")],
    "btnToggleShowcase": [("Click", "btnToggleShowcase_Click")],
}
