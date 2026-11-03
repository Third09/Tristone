using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Forms;

/// <summary>
/// GitHub Settings: where Publish Website sends the content (repository, branch) and the token allowed to change it.
/// Per PC (<see cref="PublishSettings"/>). An empty token box keeps the token already saved.
/// </summary>
public partial class PublishSettingsForm : Form
{
    readonly PublishSettings saved = PublishSettings.Load();

    public PublishSettingsForm()
    {
        InitializeComponent();
        txtRepository.Text = saved.Repository;
        txtBranch.Text = saved.Branch;
        if (saved.Token != "") txtToken.PlaceholderText = "A token is saved on this PC. Leave empty to keep it.";
    }

    /// <summary>The settings as typed (the saved token when the box is empty), or null with a message shown.</summary>
    PublishSettings? Typed()
    {
        var s = new PublishSettings { Repository = txtRepository.Text.Trim().Trim('/'), Branch = txtBranch.Text.Trim() };
        s.Token = txtToken.Text.Trim() != "" ? txtToken.Text.Trim() : saved.Token;
        if (s.Repository.Count(c => c == '/') != 1) { lblPublishSettingsMsg.Text = "Type the repository as owner/name, e.g. Third09/tristone_website."; return null; }
        if (s.Branch == "") { lblPublishSettingsMsg.Text = "Please fill in the branch."; return null; }
        if (s.Token == "") { lblPublishSettingsMsg.Text = "Please paste the GitHub token."; return null; }
        return s;
    }

    async void btnTestConnection_Click(object sender, EventArgs e)
    {
        lblPublishSettingsMsg.Text = "";
        if (Typed() is not { } s) return;
        btnTestConnection.Enabled = false;
        lblPublishSettingsMsg.Text = "Checking...";
        try { lblPublishSettingsMsg.Text = await GitHubPublisher.TestAsync(s) ?? "Works. This PC can publish the website."; }
        finally { btnTestConnection.Enabled = true; }
    }

    void btnSaveSettings_Click(object sender, EventArgs e)
    {
        lblPublishSettingsMsg.Text = "";
        if (WebsiteAccess.Require() is { } denied) { lblPublishSettingsMsg.Text = denied; return; }
        if (Typed() is not { } s) return;
        s.Save();
        DialogResult = DialogResult.OK;
    }
}
