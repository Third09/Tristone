using Company.OfficeApp.Forms;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Forms;

/// <summary>
/// First screen of the Website Admin app: signs in with a Website Admin account (not an office-app login).
/// Plain layout for now, like the office app; styled later from the Figma design.
/// </summary>
public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
        lblError.Text = "";
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
            var error = await WebsiteAuth.SignInAsync(txtUsername.Text, txtPassword.Text);
            if (error == null) { DialogResult = DialogResult.OK; return; }
            lblError.Text = error;
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

    void lnkDbSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        using var f = new DatabaseSettingsForm();
        f.ShowDialog(this);
    }

    void btnExit_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;
}
