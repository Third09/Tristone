# Spec for src/Company.OfficeApp/Forms/LoginForm.Designer.cs (Figma "ADMIN LOGIN", 2026-11-01): the one Tri-Stone Sign In.
# Centered logo, title and subtitle, a card with the fields, a dark full-width Sign In button.
NAMESPACE = "Company.OfficeApp.Forms"
CLASS = "LoginForm"
FORM = True
SIZE = (520, 640)
LOAD_EVENT = None
CARD = "Company.OfficeApp.UI.Card"
FRAME = "Company.OfficeApp.UI.InputFrame"
FIELD = 'new Font("Segoe UI Semibold", 10F)'
INPUT = 'new Font("Segoe UI", 10.5F)'
CENTER = "ContentAlignment.MiddleCenter"

CONTROLS = [
    ("PictureBox", "picLogo", 160, 40, 200, 56, {"SizeMode": "PictureBoxSizeMode.Zoom"}),
    ("Label", "lblTitle", 20, 116, 480, 36, {"Font": 'new Font("Segoe UI", 18F, FontStyle.Bold)', "TextAlign": CENTER, "Text": T("Sign in to Tri-Stone")}),
    ("Label", "lblSubtitle", 20, 154, 480, 24, {"Font": 'new Font("Segoe UI", 10.5F)', "TextAlign": CENTER, "Text": T("Sign in to access the company system.")}),
    (CARD, "cardSignIn", 50, 196, 420, 340, {"Padding": "new Padding(24)"}),
    ("Label", "lblUsername", 28, 26, None, None, {"PARENT": "cardSignIn", "Font": FIELD, "Text": T("Username")}),
    (FRAME, "frmUsername", 28, 50, 364, 42, {"PARENT": "cardSignIn"}),
    ("TextBox", "txtUsername", 12, 10, 340, 23, {"PARENT": "frmUsername", "BorderStyle": "BorderStyle.None", "Font": INPUT, "PlaceholderText": T("Enter your username")}),
    ("Label", "lblPassword", 28, 104, None, None, {"PARENT": "cardSignIn", "Font": FIELD, "Text": T("Password")}),
    (FRAME, "frmPassword", 28, 128, 364, 42, {"PARENT": "cardSignIn"}),
    ("TextBox", "txtPassword", 12, 10, 340, 23, {"PARENT": "frmPassword", "BorderStyle": "BorderStyle.None", "Font": INPUT,
                                                 "PlaceholderText": T("Enter your password"), "UseSystemPasswordChar": "true"}),
    ("CheckBox", "chkShowPassword", 28, 182, None, None, {"PARENT": "cardSignIn", "Text": T("Show password")}),
    ("LinkLabel", "lnkForgotPassword", 282, 183, 110, 20, {"PARENT": "cardSignIn", "TextAlign": "ContentAlignment.MiddleRight", "Text": T("Forgot Password?")}),
    ("Label", "lblError", 28, 210, 364, 40, {"PARENT": "cardSignIn", "ForeColor": "Color.FromArgb(180, 35, 24)"}),
    ("Button", "btnSignIn", 28, 256, 364, 44, {"PARENT": "cardSignIn", "Font": 'new Font("Segoe UI Semibold", 10.5F)',
                                               "BackColor": "Color.FromArgb(17, 17, 17)", "ForeColor": "Color.White", "Text": T("Sign In")}),
    ("LinkLabel", "lnkDbSettings", 50, 560, 200, 20, {"Text": T("Database settings")}),
    ("Button", "btnExit", 370, 552, 100, 36, {"Text": T("Exit")}),
]

GRID_COLUMNS = {}

EVENTS = {
    "chkShowPassword": [("CheckedChanged", "chkShowPassword_CheckedChanged")],
    "btnSignIn": [("Click", "btnSignIn_Click")],
    "btnExit": [("Click", "btnExit_Click")],
    "lnkForgotPassword": [("LinkClicked", "lnkForgotPassword_LinkClicked")],
    "lnkDbSettings": [("LinkClicked", "lnkDbSettings_LinkClicked")],
}
