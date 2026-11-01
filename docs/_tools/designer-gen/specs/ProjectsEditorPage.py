# Spec for src/Company.WebsiteAdmin/Pages/ProjectsEditorPage.Designer.cs (Figma "ADMIN PROJECTS" + uploading UI)
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "ProjectsEditorPage"
SIZE = (960, 720)
LOAD_EVENT = "ProjectsEditorPage_Load"
PIC = {"SizeMode": "PictureBoxSizeMode.Zoom", "BorderStyle": "BorderStyle.FixedSingle"}

CONTROLS = [
    ("Label", "lblProjectsHint", 12, 12, None, None, {"Text": T("Featured projects come from the office Projects list (ticked \"Show on website\"). The website shows them in this order.")}),
    ("DataGridView", "dgvWebProjects", 12, 36, 930, 170, {}),
    ("Button", "btnAddWebProject", 12, 214, 130, 28, {"Text": T("Add New Project")}),
    ("Button", "btnMoveProjectUp", 148, 214, 80, 28, {"Text": T("Move Up")}),
    ("Button", "btnMoveProjectDown", 234, 214, 90, 28, {"Text": T("Move Down")}),
    ("Button", "btnHideShowProject", 330, 214, 110, 28, {"Text": T("Unpublish")}),
    ("Button", "btnDeleteWebProject", 446, 214, 90, 28, {"Text": T("Delete")}),
    ("Label", "lblSlotTitle", 12, 256, None, None, {"Text": T("Click a project in the list to edit it.")}),
    ("Label", "lblProjectInfo", 12, 278, None, None, {"Text": T("Location: -    Category: -")}),
    ("Label", "lblProjectDesc", 12, 302, None, None, {"Text": T("Project short description")}),
    ("Label", "lblProjectDescCount", 380, 302, None, None, {"Text": T("0/100")}),
    ("TextBox", "txtProjectDesc", 12, 320, 420, 23, {}),
    ("Button", "btnSaveWebProject", 12, 352, 120, 28, {"Text": T("Save Changes")}),
    ("Button", "btnDiscardWebProject", 138, 352, 130, 28, {"Text": T("Discard Changes")}),
    ("Label", "lblWebProjectMsg", 12, 388, None, None, {"MaximumSize": "new Size(420, 0)"}),
    ("Label", "lblPhotos", 460, 256, None, None, {"Text": T("Project photos (up to 10, JPEG or PNG, 8 MB each). Showcase = shown in the website carousel.")}),
    ("DataGridView", "dgvPhotos", 460, 278, 300, 200, {}),
    ("PictureBox", "picPhotoPreview", 772, 278, 170, 128, PIC),
    ("Button", "btnAddPhotos", 460, 486, 100, 28, {"Text": T("Add Photos...")}),
    ("Button", "btnRemovePhoto", 566, 486, 80, 28, {"Text": T("Remove")}),
    ("Button", "btnPhotoUp", 652, 486, 50, 28, {"Text": T("Up")}),
    ("Button", "btnPhotoDown", 708, 486, 52, 28, {"Text": T("Down")}),
    ("Button", "btnToggleShowcase", 772, 412, 170, 28, {"Text": T("Showcase: Yes / No")}),
    ("ProgressBar", "prgUpload", 460, 522, 300, 18, {"Visible": "false"}),
    ("Label", "lblUploadStatus", 460, 546, None, None, {"MaximumSize": "new Size(480, 0)"}),
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
    "btnAddPhotos": [("Click", "btnAddPhotos_Click")],
    "btnRemovePhoto": [("Click", "btnRemovePhoto_Click")],
    "btnPhotoUp": [("Click", "btnPhotoUp_Click")],
    "btnPhotoDown": [("Click", "btnPhotoDown_Click")],
    "btnToggleShowcase": [("Click", "btnToggleShowcase_Click")],
}
