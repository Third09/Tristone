# Spec for src/Company.Launcher/TestAsForm.Designer.cs (pop-up window, Demo / Test version only, owner request 2026-11-17)
NAMESPACE = "Company.Launcher"
CLASS = "TestAsForm"
FORM = True
ROOMY = True
SIZE = (640, 470)
LOAD_EVENT = "TestAsForm_Load"

CONTROLS = [
    ("Label", "lblTestAsTitle", 12, 12, None, None, {"Text": T("Test as...")}),
    ("Label", "lblTestAsNote", 12, 40, None, None, {"MaximumSize": "new Size(610, 0)",
        "Text": T("Demo / Test version only: open Tri-Stone as any login, without its password, to try what that person can do. Sign Out brings you back here.")}),
    ("TextBox", "txtTestAsSearch", 12, 82, 250, 23, {"PlaceholderText": T("Search name, login or department")}),
    ("DataGridView", "dgvTestUsers", 12, 112, 612, 270, {}),
    ("Button", "btnOpenAsUser", 12, 392, 190, 28, {"Text": T("Open as this login")}),
    ("Button", "btnCancelTestAs", 210, 392, 120, 28, {"Text": T("Sign Out")}),
]

GRID_COLUMNS = {
    "dgvTestUsers": [
        (TB, "colTaName", "FullName", "Name", {"FillWeight": "150"}),
        (TB, "colTaUsername", "Username", "Login", {"FillWeight": "90"}),
        (TB, "colTaLevel", "Level", "Level", {"FillWeight": "80"}),
        (TB, "colTaDepartment", "Department", "Department", {"FillWeight": "100"}),
        (TB, "colTaOpens", "Opens", "Opens", {"FillWeight": "120"}),
    ],
}

EVENTS = {
    "btnOpenAsUser": [("Click", "btnOpenAsUser_Click")],
    "btnCancelTestAs": [("Click", "btnCancelTestAs_Click")],
    "dgvTestUsers": [("CellDoubleClick", "dgvTestUsers_CellDoubleClick")],
    "txtTestAsSearch": [("TextChanged", "txtTestAsSearch_TextChanged")],
}
