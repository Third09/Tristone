using Company.OfficeApp.UI;
using Company.WebsiteAdmin.Services;
using Microsoft.Web.WebView2.Core;

namespace Company.WebsiteAdmin.Forms;

/// <summary>
/// Preview Website (owner request 2026-11-03): the whole website as it will look after Publish, without putting
/// anything live and without using Netlify. A copy of the website's files is kept from GitHub in
/// C:\ProgramData\TriStone\WebsitePreview (only changed files are downloaded, <see cref="GitHubPublisher.CopySiteAsync"/>);
/// the saved changes (<see cref="PublishService.BuildAsync"/>) are written into its Updated/content, and the page is
/// shown in a WebView2 browser from that folder. Unsaved edits on the open page are not included.
/// </summary>
public partial class PreviewForm : Form
{
    const string Host = "tristone.preview";   // made-up address that WebView2 maps to the copy
    const int PhoneWidth = 390;

    static readonly string SiteFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TriStone", "WebsitePreview");

    bool phone;
    bool ready;

    public PreviewForm()
    {
        InitializeComponent();
        pnlPreviewArea.BackColor = Theme.Canvas;
        lblPreviewInfo.ForeColor = Theme.TextMuted;
    }

    async void PreviewForm_Load(object sender, EventArgs e)
    {
        FitView();
        try
        {
            var data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TriStone", "WebView2");
            var env = await CoreWebView2Environment.CreateAsync(null, data);
            await webPreview.EnsureCoreWebView2Async(env);
            ready = true;
        }
        catch (Exception ex)
        {
            Say("The preview browser (Microsoft Edge WebView2) couldn't start on this PC. " + ex.Message);
            return;
        }
        await ShowPreviewAsync();
    }

    async Task ShowPreviewAsync()
    {
        if (!ready) return;
        btnRefreshPreview.Enabled = false;
        UseWaitCursor = true;
        try
        {
            Directory.CreateDirectory(SiteFolder);
            Say("Getting the website's files from GitHub (only the first time takes a while)...");
            var copyProblem = await GitHubPublisher.CopySiteAsync(PublishSettings.Load(), SiteFolder, "Updated", PublishService.SiteFolder);
            var page = Path.Combine(SiteFolder, "Updated", "html", "index.html");
            if (copyProblem != null && !File.Exists(page)) { Say("Can't show the preview: " + copyProblem); return; }

            Say("Putting in your saved changes...");
            var (site, problem) = await PublishService.BuildAsync();
            if (problem != null) { Say(problem); return; }
            var content = Path.Combine(SiteFolder, PublishService.SiteFolder.Replace('/', Path.DirectorySeparatorChar));
            if (Directory.Exists(content)) Directory.Delete(content, recursive: true);
            foreach (var (path, bytes) in site!.Files)
            {
                var full = Path.Combine(SiteFolder, path.Replace('/', Path.DirectorySeparatorChar));
                Directory.CreateDirectory(Path.GetDirectoryName(full)!);
                await File.WriteAllBytesAsync(full, bytes);
            }

            webPreview.CoreWebView2.SetVirtualHostNameToFolderMapping(Host, SiteFolder, CoreWebView2HostResourceAccessKind.Allow);
            webPreview.CoreWebView2.Navigate($"https://{Host}/Updated/html/index.html");
            Say(copyProblem == null
                ? "Preview of your saved changes. Not live until you click Publish Website."
                : "Preview of your saved changes, on the last copy of the website (GitHub: " + copyProblem + ")");
        }
        catch (Exception ex) { Say("Can't show the preview. " + ex.Message); }
        finally { btnRefreshPreview.Enabled = true; UseWaitCursor = false; }
    }

    void Say(string text) => lblPreviewInfo.Text = text;

    /// <summary>Desktop = the whole width; Phone = a phone-wide page in the middle.</summary>
    void FitView()
    {
        var area = pnlPreviewArea.ClientSize;
        webPreview.Bounds = phone
            ? new Rectangle(Math.Max(0, (area.Width - PhoneWidth) / 2), 0, Math.Min(PhoneWidth, area.Width), area.Height)
            : new Rectangle(Point.Empty, area);
        Theme.SetSelected(btnPreviewDesktop, !phone);
        Theme.SetSelected(btnPreviewPhone, phone);
    }

    void pnlPreviewArea_Resize(object? sender, EventArgs e) => FitView();

    void btnPreviewDesktop_Click(object sender, EventArgs e) { phone = false; FitView(); }

    void btnPreviewPhone_Click(object sender, EventArgs e) { phone = true; FitView(); }

    async void btnRefreshPreview_Click(object sender, EventArgs e) => await ShowPreviewAsync();

    void btnClosePreview_Click(object sender, EventArgs e) => Close();
}
