using Company.OfficeApp.Services;

namespace Company.OfficeApp.Forms;

/// <summary>
/// First screen, laid out like the Figma "ADMIN LOGIN" (2026-11-01): logo, title, a card with the fields and a dark
/// Sign In button. Control names match the UI Elements Guide (Part A: Login Window).
/// </summary>
public partial class LoginForm : Form
{
    /// <param name="notice">Shown under the password box, e.g. after an idle sign-out.</param>
    public LoginForm(string? notice = null)
    {
        InitializeComponent();
        Text = "Tri-Stone - Sign In";
        StartPosition = FormStartPosition.CenterScreen;
        AcceptButton = btnSignIn;
        BackColor = Color.White;
        picLogo.Image = UI.Theme.Logo;
        lblSubtitle.ForeColor = UI.Theme.TextMuted;
        btnSignIn.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 55);
        lblError.Text = notice ?? "";
        // Only development copies change the connection here. Office: fixed by Setup; Demo: its own database.
        lnkDbSettings.Visible = !Data.AppEdition.IsOffice && !Data.AppEdition.IsDemo;
        if (Data.AppEdition.IsDemo)
        {
            Text = "Tri-Stone Demo - Sign In";
            if (notice == null) lblError.Text = "Demo: sign in as demo.admin, password Demo1234";
        }
    }

    void chkShowPassword_CheckedChanged(object sender, EventArgs e) =>
        txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;

    async void btnSignIn_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (txtUsername.Text.Trim() == "" || txtPassword.Text == "")
        {
            lblError.Text = "Enter your username and password.";
            return;
        }

        btnSignIn.Enabled = false;
        UseWaitCursor = true;
        try
        {
            var result = await AuthService.SignInAsync(txtUsername.Text, txtPassword.Text);
            if (result.Success)
            {
                DialogResult = DialogResult.OK;
                return;
            }
            lblError.Text = result.Error;
            txtPassword.Clear();
            txtPassword.Focus();
        }
        catch (Exception ex)
        {
            lblError.Text = "Can't reach the database. Check the database settings.\n" + ex.Message;
        }
        finally
        {
            btnSignIn.Enabled = true;
            UseWaitCursor = false;
        }
    }

    void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        using var f = new ForgotPasswordForm(txtUsername.Text);
        f.ShowDialog(this);
    }

    void lnkDbSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        using var f = new DatabaseSettingsForm();
        f.ShowDialog(this);
    }

    void btnExit_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;
}
