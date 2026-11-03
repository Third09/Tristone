using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Forms;

/// <summary>
/// Website Settings (per PC, <see cref="PublishSettings"/>): where Publish Website sends the content (GitHub repository,
/// branch, token) and where Contact Us gets new inquiries (the website's Google Script address and read key).
/// Empty token / key boxes keep the ones already saved. Each part can be set up on its own.
/// </summary>
public partial class PublishSettingsForm : Form
{
    readonly PublishSettings saved = PublishSettings.Load();

    public PublishSettingsForm()
    {
        InitializeComponent();
        txtRepository.Text = saved.Repository;
        txtBranch.Text = saved.Branch;
        txtInquiryScriptUrl.Text = saved.InquiryScriptUrl;
        if (saved.Token != "") txtToken.PlaceholderText = "A token is saved on this PC. Leave empty to keep it.";
        if (saved.InquiryKey != "") txtInquiryKey.PlaceholderText = "A key is saved on this PC. Leave empty to keep it.";
    }

    /// <summary>The settings as typed (saved token/key when their box is empty), or null with a message shown.</summary>
    PublishSettings? Typed()
    {
        var s = new PublishSettings
        {
            Repository = txtRepository.Text.Trim().Trim('/'),
            Branch = txtBranch.Text.Trim(),
            InquiryScriptUrl = txtInquiryScriptUrl.Text.Trim(),
        };
        s.Token = txtToken.Text.Trim() != "" ? txtToken.Text.Trim() : saved.Token;
        s.InquiryKey = txtInquiryKey.Text.Trim() != "" ? txtInquiryKey.Text.Trim() : saved.InquiryKey;
        if (s.Repository.Count(c => c == '/') != 1) { lblPublishSettingsMsg.Text = "Type the repository as owner/name, e.g. Third09/tristone_website."; return null; }
        if (s.Branch == "") { lblPublishSettingsMsg.Text = "Please fill in the branch."; return null; }
        if (!s.InquiryScriptUrl.StartsWith("https://script.google.com/"))
        {
            lblPublishSettingsMsg.Text = "The script web address starts with https://script.google.com/ (the Contact form's address).";
            return null;
        }
        return s;
    }

    async void btnTestConnection_Click(object sender, EventArgs e)
    {
        lblPublishSettingsMsg.Text = "";
        if (Typed() is not { } s) return;
        if (s.Token == "") { lblPublishSettingsMsg.Text = "Please paste the GitHub token."; return; }
        btnTestConnection.Enabled = false;
        lblPublishSettingsMsg.Text = "Checking...";
        try { lblPublishSettingsMsg.Text = await GitHubPublisher.TestAsync(s) ?? "Works. This PC can publish the website."; }
        finally { btnTestConnection.Enabled = true; }
    }

    async void btnTestInquiries_Click(object sender, EventArgs e)
    {
        lblPublishSettingsMsg.Text = "";
        if (Typed() is not { } s) return;
        if (s.InquiryKey == "") { lblPublishSettingsMsg.Text = "Please paste the read key from the script."; return; }
        btnTestInquiries.Enabled = false;
        lblPublishSettingsMsg.Text = "Checking...";
        try
        {
            var (count, problem) = await InquiryImport.TestAsync(s);
            lblPublishSettingsMsg.Text = problem ?? $"Works. The script has {count} inquir{(count == 1 ? "y" : "ies")} saved.";
        }
        finally { btnTestInquiries.Enabled = true; }
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
