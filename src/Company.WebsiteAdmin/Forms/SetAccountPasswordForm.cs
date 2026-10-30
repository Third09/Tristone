using Company.Shared;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Forms;

/// <summary>
/// Accounts > Set New Password (main accounts only). Like the office app: Generate makes an easy-to-read password,
/// Copy puts it on the clipboard, and the main account gives it to the person in person. Also unlocks the account.
/// </summary>
public partial class SetAccountPasswordForm : Form
{
    readonly int accountId;

    public SetAccountPasswordForm(int accountId, string who)
    {
        InitializeComponent();
        this.accountId = accountId;
        lblSetPasswordFor.Text = $"New password for {who}";
        lblSetPasswordMsg.Text = "";
    }

    void btnGeneratePassword_Click(object sender, EventArgs e) => txtNewPassword.Text = AccountRules.GeneratePassword();

    void btnCopyPassword_Click(object sender, EventArgs e)
    {
        if (txtNewPassword.Text != "") Clipboard.SetText(txtNewPassword.Text);
    }

    async void btnSavePassword_Click(object sender, EventArgs e)
    {
        btnSavePassword.Enabled = false;
        try
        {
            var error = await WebsiteAccounts.SetPasswordAsync(accountId, txtNewPassword.Text);
            if (error != null) { lblSetPasswordMsg.Text = error; return; }
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { lblSetPasswordMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSavePassword.Enabled = true; }
    }
}
