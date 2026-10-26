using Company.OfficeApp.Services;

namespace Company.OfficeApp.Forms;

/// <summary>
/// First screen. Plain layout for now; styled later from the Figma design.
/// Control names match the UI Elements Guide (Part A: Login Window).
/// </summary>
public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
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
