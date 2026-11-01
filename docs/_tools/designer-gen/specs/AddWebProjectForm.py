# Spec for src/Company.WebsiteAdmin/Pages/AddWebProjectForm.Designer.cs (pop-up window: Add New Project)
NAMESPACE = "Company.WebsiteAdmin.Pages"
CLASS = "AddWebProjectForm"
FORM = True
ROOMY = True   # spread out like the Figma pages (2026-11-01)
SIZE = (460, 230)
LOAD_EVENT = "AddWebProjectForm_Load"

CONTROLS = [
    ("Label", "lblChooseProject", 12, 12, None, None, {"Text": T("Project (from the office Projects list, ticked \"Show on website\")")}),
    ("ComboBox", "cboProjectChoice", 12, 32, 436, 23, DDL),
    ("Label", "lblNewProjectDesc", 12, 66, None, None, {"Text": T("Project short description")}),
    ("Label", "lblNewProjectDescCount", 396, 66, None, None, {"Text": T("0/100")}),
    ("TextBox", "txtNewProjectDesc", 12, 84, 436, 23, {}),
    ("Label", "lblAddProjectMsg", 12, 116, None, None, {"MaximumSize": "new Size(436, 0)"}),
    ("Button", "btnAddProjectOk", 262, 190, 90, 28, {"Text": T("Add")}),
    ("Button", "btnCancelAddProject", 358, 190, 90, 28, {"Text": T("Cancel"), "DialogResult": "DialogResult.Cancel"}),
]

GRID_COLUMNS = {}

EVENTS = {
    "btnAddProjectOk": [("Click", "btnAddProjectOk_Click")],
}
