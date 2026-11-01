using System.Drawing.Drawing2D;
using Company.OfficeApp.UI;
using Company.Shared.Models;
using Company.WebsiteAdmin.Services;
using static Company.OfficeApp.UI.StatusBox;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > About Us (Figma "admin about us section 1-3"): three fixed sections, each with a picture, a typed
/// heading and its text. Section 3 has two texts: Our Vision and Our Mission. One section is edited at a time.
/// Laid out like the Figma: section choice cards, the section card, a Live Preview, the message box and the save bar.
/// </summary>
public partial class AboutEditorPage : UserControl, IWebsiteEditor
{
    record AboutForm(string Heading, string Text1, string Text2, string Picture);

    static readonly string[] Names = ["About Tri-Stone", "Our Principles", "Our Mission and Vision"];

    int section = 1;
    AboutForm? original;
    string? storedImage, newImage;
    bool removeImage, loading;
    Image? picture;

    public AboutEditorPage()
    {
        InitializeComponent();
        BackColor = pnlGap1.BackColor = pnlSections.BackColor = pnlAboutSide.BackColor = pnlSideGap.BackColor = pnlAboutFooter.BackColor = Theme.Canvas;
        Padding = new Padding(24, 8, 24, 24);
        AutoScrollMinSize = new Size(1100, 940);
        foreach (var l in new[] { lblAboutSection, lblAboutImageName, lblAboutHeadingCount, lblAboutText1Count, lblAboutText2Count, lblAboutLastSaved })
            l.ForeColor = Theme.TextMuted;
        EditorUi.Counter(txtAboutHeading, lblAboutHeadingCount, WebsiteContentService.MaxLength(WebsiteKeys.AboutHeading(1)));
        dzAbout.BrowseRequested += async (_, _) => { if (EditorUi.PickPicture(this) is { } path) await UsePictureAsync(path); };
        dzAbout.FilesDropped += async files =>
        {
            if (WebsiteImages.Check(files[0]) is { } problem) statusAbout.Show(StatusKind.Error, "Picture not used", problem);
            else await UsePictureAsync(files[0]);
        };
        pnlAboutPreview.Paint += PaintPreview;
        Disposed += (_, _) => picture?.Dispose();
    }

    public bool HasUnsavedChanges => original != null && ReadForm() != original;

    static string Text1Key(int s) => s switch { 1 => WebsiteKeys.About1Text, 2 => WebsiteKeys.About2Text, _ => WebsiteKeys.Vision };
    static string[] KeysOf(int s) => s == 3
        ? [WebsiteKeys.AboutImage(3), WebsiteKeys.AboutHeading(3), WebsiteKeys.Vision, WebsiteKeys.Mission]
        : [WebsiteKeys.AboutImage(s), WebsiteKeys.AboutHeading(s), Text1Key(s)];

    async void AboutEditorPage_Load(object sender, EventArgs e) => await LoadAsync();

    async void btnAboutSection_Click(object sender, EventArgs e)
    {
        int wanted = sender == btnAboutSection1 ? 1 : sender == btnAboutSection2 ? 2 : 3;
        if (wanted == section) return;
        if (HasUnsavedChanges && MessageBox.Show(this, "This section has unsaved changes. Leave it without saving?", "Unsaved changes",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        section = wanted;
        await LoadAsync();
    }

    async Task LoadAsync()
    {
        using var busy = Responsive.Busy(this);
        loading = true;
        try
        {
            lblAboutImage.Text = $"{Names[section - 1]} - Image";
            lblAboutSection.Text = $"Section {section} of 3";
            foreach (var (b, n) in new[] { (btnAboutSection1, 1), (btnAboutSection2, 2), (btnAboutSection3, 3) })
                Theme.SetSelected(b, n == section);   // the open section is highlighted

            bool three = section == 3;
            lblAboutText1.Text = three ? "Our Vision" : "Section Description";
            frmAboutText2.Visible = lblAboutText2.Visible = lblAboutText2Count.Visible = three;
            frmAboutText1.Height = three ? 120 : 246;   // sections 1-2: one long text
            txtAboutText1.MaxLength = WebsiteContentService.MaxLength(Text1Key(section));
            txtAboutText2.MaxLength = WebsiteContentService.MaxLength(WebsiteKeys.Mission);

            var v = await WebsiteContentService.LoadAsync(KeysOf(section));
            storedImage = v[WebsiteKeys.AboutImage(section)];
            newImage = null;
            removeImage = false;
            txtAboutHeading.Text = v[WebsiteKeys.AboutHeading(section)] ?? "";
            txtAboutText1.Text = (v[Text1Key(section)] ?? "").Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
            txtAboutText2.Text = three ? v[WebsiteKeys.Mission] ?? "" : "";
            ShowCounts();
            await ShowImageAsync();
            lblAboutLastSaved.Text = EditorUi.SavedText(await WebsiteContentService.LastSavedAsync(KeysOf(section)));
            original = ReadForm();
            statusAbout.Clear();
        }
        catch (Exception ex) { statusAbout.Show(StatusKind.Error, "Can't reach the database", ex.Message); }
        finally { loading = false; }
    }

    void ShowCounts()
    {
        lblAboutText1Count.Text = $"{txtAboutText1.TextLength}/{txtAboutText1.MaxLength}";
        lblAboutText2Count.Text = $"{txtAboutText2.TextLength}/{txtAboutText2.MaxLength}";
    }

    string PictureName => removeImage ? "(none)" : newImage != null ? Path.GetFileName(newImage)
                        : storedImage != null ? Path.GetFileName(storedImage) : "(none)";

    AboutForm ReadForm() => new(txtAboutHeading.Text.Trim(), txtAboutText1.Text.Trim(), section == 3 ? txtAboutText2.Text.Trim() : "", PictureName);

    async Task ShowImageAsync()
    {
        var old = picture;
        picture = removeImage ? null : await WebsiteImages.LoadAsync(storedImage, newImage);
        dzAbout.Image = picture;
        old?.Dispose();
        btnRemoveAboutImage.Visible = picture != null;
        lblAboutImageName.Text = picture == null ? "No picture yet (the website keeps its current one). Click the box or drop a picture on it."
                                                 : PictureName + "   (click the picture to change it)";
        lblAboutImageName.Left = picture == null ? btnRemoveAboutImage.Left : btnRemoveAboutImage.Right + 12;
        pnlAboutPreview.Invalidate();
    }

    async Task UsePictureAsync(string path)
    {
        newImage = path;
        removeImage = false;
        await ShowImageAsync();
        Field_Changed(null, EventArgs.Empty);
    }

    void Field_Changed(object? sender, EventArgs e)
    {
        ShowCounts();
        pnlAboutPreview.Invalidate();
        if (loading) return;
        if (HasUnsavedChanges) statusAbout.Show(StatusKind.Info, "Unsaved Changes", "Remember to save your changes to keep your website content updated.");
        else statusAbout.Clear();
    }

    /// <summary>The section as the website shows it: picture, heading, then the text.</summary>
    void PaintPreview(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        var r = pnlAboutPreview.ClientRectangle;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(cardAboutPreview.BackColor);
        var pic = new Rectangle(0, 0, r.Width - 1, 130);
        using (var path = Rounded.Path(pic, 8))
        {
            g.SetClip(path);
            if (picture != null)
            {
                float scale = Math.Max((float)pic.Width / picture.Width, (float)pic.Height / picture.Height);
                int w = (int)(picture.Width * scale), h = (int)(picture.Height * scale);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(picture, (pic.Width - w) / 2, (pic.Height - h) / 2, w, h);
            }
            else using (var gray = new SolidBrush(Theme.GridEmpty)) g.FillRectangle(gray, pic);
            g.ResetClip();
        }
        using var head = new Font("Segoe UI", 11f, FontStyle.Bold);
        TextRenderer.DrawText(g, txtAboutHeading.Text, head, new Rectangle(0, 140, r.Width, 24), Theme.Text, TextFormatFlags.EndEllipsis);
        var body = section == 3 ? txtAboutText1.Text + "\n\n" + txtAboutText2.Text : txtAboutText1.Text;
        TextRenderer.DrawText(g, body, Font, new Rectangle(0, 168, r.Width, r.Height - 168), Theme.TextMuted,
                              TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis);
    }

    async void btnRemoveAboutImage_Click(object sender, EventArgs e)
    {
        newImage = null;
        removeImage = storedImage != null;
        await ShowImageAsync();
        Field_Changed(null, EventArgs.Empty);
    }

    async void btnSaveAbout_Click(object sender, EventArgs e)
    {
        if (original == null) return;
        var input = ReadForm();
        var labels = new Dictionary<string, string> { ["Text1"] = section == 3 ? "Our Vision" : "Description", ["Text2"] = "Our Mission" };
        var answer = RecordForm.ConfirmSave(this, $"About Us section {section}", original, input, labels);
        if (answer == SaveAnswer.NothingChanged) { statusAbout.Show(StatusKind.Info, "Nothing changed", "There is nothing new to save."); return; }
        if (answer == SaveAnswer.No) { statusAbout.Show(StatusKind.Info, "Not saved", "Click Discard Changes to undo your changes."); return; }

        btnSaveAbout.Enabled = false;
        try
        {
            var texts = new Dictionary<string, string?>
            {
                [WebsiteKeys.AboutHeading(section)] = input.Heading,
                [Text1Key(section)] = input.Text1,
            };
            if (section == 3) texts[WebsiteKeys.Mission] = input.Text2;
            var images = new Dictionary<string, string?>();
            if (newImage != null) images[WebsiteKeys.AboutImage(section)] = newImage;
            else if (removeImage) images[WebsiteKeys.AboutImage(section)] = null;
            var error = await WebsiteContentService.SaveAsync($"About Us section {section}", texts, images);
            if (error != null) { statusAbout.Show(StatusKind.Error, "Unable to Save Changes", error); return; }
            await LoadAsync();
            statusAbout.Show(StatusKind.Success, "Changes Saved Successfully", "Click Publish Website to put them on the live site.");
        }
        catch (Exception ex) { statusAbout.Show(StatusKind.Error, "Unable to Save Changes", "Please try again. " + ex.Message); }
        finally { btnSaveAbout.Enabled = true; }
    }

    async void btnDiscardAbout_Click(object sender, EventArgs e) => await LoadAsync();
}
