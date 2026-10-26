using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Forms;

/// <summary>
/// Forgot Password. Step 1 asks for the username, then:
///   Employee / Supervisor -> a request goes to the Admins (pnlRequestSent);
///   Admin -> security question + new password (pnlAdmin).
/// Plain layout for now; control names match the UI Elements Guide (Part A: Forgot Password Window).
/// </summary>
public partial class ForgotPasswordForm : Form
{
    User? user;

    public ForgotPasswordForm(string username = "")
    {
        InitializeComponent();
        txtForgotUsername.Text = username.Trim();
        ShowStep(pnlStep1);
    }

    void ShowStep(Panel step)
    {
        foreach (var p in new[] { pnlStep1, pnlRequestSent, pnlAdmin }) p.Visible = p == step;
        AcceptButton = step == pnlStep1 ? btnForgotNext : step == pnlAdmin ? btnResetPassword : btnForgotClose;
    }

    async void btnForgotNext_Click(object sender, EventArgs e)
    {
        lblForgotError.Text = "";
        if (txtForgotUsername.Text.Trim() == "") { lblForgotError.Text = "Enter your username."; return; }

        btnForgotNext.Enabled = false;
        try
        {
            user = await PasswordService.FindUserAsync(txtForgotUsername.Text);

            if (user == null) { lblForgotError.Text = "No account with that username."; return; }
            if (user.Status == UserStatuses.Disabled) { lblForgotError.Text = "This account is turned off. Please contact an Admin."; return; }

            if (user.IsAdmin)
            {
                if (user.SecurityQuestion == null || user.SecurityAnswerHash == null)
                {
                    lblForgotError.Text = "No security question is set for this Admin account. Ask another Admin to reset your password.";
                    return;
                }
                lblSecurityQuestion.Text = user.SecurityQuestion;
                ShowStep(pnlAdmin);
                txtSecurityAnswer.Focus();
                return;
            }

            var sent = await PasswordService.RequestResetAsync(user);
            lblRequestSent.Text = sent
                ? "Your request was sent to the Admins. They will give you a new password."
                : "You already sent a request. An Admin will give you a new password soon.";
            ShowStep(pnlRequestSent);
        }
        catch (Exception ex) { lblForgotError.Text = "Can't reach the database.\n" + ex.Message; }
        finally { btnForgotNext.Enabled = true; }
    }

    async void btnResetPassword_Click(object sender, EventArgs e)
    {
        lblAdminError.Text = "";
        btnResetPassword.Enabled = false;
        try
        {
            var problem = await PasswordService.ResetWithSecurityAnswerAsync(user!.UserId, txtSecurityAnswer.Text, txtNewPassword.Text, txtConfirmPassword.Text);
            if (problem != null) { lblAdminError.Text = problem; return; }

            MessageBox.Show(this, "Your password was changed. You can sign in with the new password now.", "Password changed",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { lblAdminError.Text = "Can't reach the database.\n" + ex.Message; }
        finally { btnResetPassword.Enabled = true; }
    }

    void btnClose_Click(object sender, EventArgs e) => Close();
}
