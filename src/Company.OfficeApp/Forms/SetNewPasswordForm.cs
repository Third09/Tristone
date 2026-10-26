using Company.OfficeApp.Services;

namespace Company.OfficeApp.Forms;

/// <summary>
/// Admin only: sets a new password for someone and gives it to them in person.
/// Also unlocks a Locked account and closes their waiting request.
/// Plain layout for now; control names match the UI Elements Guide (Part A: Set New Password Window).
/// </summary>
public partial class SetNewPasswordForm : Form
{
    readonly int userId;

    public SetNewPasswordForm(int userId, string displayName)
    {
        InitializeComponent();
        this.userId = userId;
        lblResetUser.Text = $"New password for: {displayName}";
    }

    void btnGeneratePassword_Click(object sender, EventArgs e) => txtResetPassword.Text = PasswordService.GeneratePassword();

    void btnCopyPassword_Click(object sender, EventArgs e)
    {
        if (txtResetPassword.Text == "") return;
        Clipboard.SetText(txtResetPassword.Text);
        lblResetError.Text = "Copied.";
    }

    async void btnSaveNewPassword_Click(object sender, EventArgs e)
    {
        lblResetError.Text = "";
        btnSaveNewPassword.Enabled = false;
        try
        {
            var problem = await PasswordService.SetNewPasswordAsync(userId, txtResetPassword.Text);
            if (problem != null) { lblResetError.Text = problem; return; }
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { lblResetError.Text = "Can't reach the database.\n" + ex.Message; }
        finally { btnSaveNewPassword.Enabled = true; }
    }
}
