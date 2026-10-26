using Company.OfficeApp.Services;

namespace Company.OfficeApp.Pages;

/// <summary>
/// My Account. Everyone sees their details. Only Admins can change their password and security question;
/// for Employees and Supervisors those parts are hidden.
/// Plain layout for now; control names match the UI Elements Guide (Part A: My Account Page).
/// </summary>
public partial class MyAccountPage : UserControl
{
    public MyAccountPage()
    {
        InitializeComponent();
        cboSecurityQuestion.Items.AddRange(AccountService.SecurityQuestions);

        var me = Session.CurrentUser!;
        lblMyName.Text = $"Name: {me.FullName}";
        lblMyUsername.Text = $"Username: {me.Username}";
        lblMyRole.Text = me.IsAdmin ? "Level: Admin" : $"Department: {me.Department}    Level: {me.Level}";

        grpChangePassword.Visible = grpSecurityQuestion.Visible = me.IsAdmin;
        lblAskAdmin.Visible = !me.IsAdmin;
        ShowSecurityQuestionStatus();
    }

    void ShowSecurityQuestionStatus()
    {
        var q = Session.CurrentUser!.SecurityQuestion;
        lblSqStatus.Text = q == null
            ? "No security question set yet. Set one so you can reset your own password if you forget it."
            : $"Current question: {q}";
        if (q != null) cboSecurityQuestion.Text = q;
    }

    async void btnSavePassword_Click(object sender, EventArgs e)
    {
        lblPasswordMsg.Text = "";
        btnSavePassword.Enabled = false;
        try
        {
            var problem = await AccountService.ChangeOwnPasswordAsync(txtCurrentPassword.Text, txtNewPassword.Text, txtConfirmPassword.Text);
            lblPasswordMsg.Text = problem ?? "Password changed.";
            if (problem == null) { txtCurrentPassword.Clear(); txtNewPassword.Clear(); txtConfirmPassword.Clear(); }
        }
        catch (Exception ex) { lblPasswordMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSavePassword.Enabled = true; }
    }

    async void btnSaveSecurityQuestion_Click(object sender, EventArgs e)
    {
        lblSqMsg.Text = "";
        btnSaveSecurityQuestion.Enabled = false;
        try
        {
            var problem = await AccountService.SetSecurityQuestionAsync(txtSqCurrentPassword.Text, cboSecurityQuestion.Text, txtSecurityAnswer.Text);
            lblSqMsg.Text = problem ?? "Security question saved.";
            if (problem == null)
            {
                txtSecurityAnswer.Clear();
                txtSqCurrentPassword.Clear();
                ShowSecurityQuestionStatus();
            }
        }
        catch (Exception ex) { lblSqMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveSecurityQuestion.Enabled = true; }
    }
}
