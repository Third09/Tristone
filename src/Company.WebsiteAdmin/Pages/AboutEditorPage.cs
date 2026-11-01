using Company.OfficeApp.UI;
using Company.Shared.Models;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > About Us (Figma "admin about us section 1-3"): three fixed sections, each with a picture, a typed
/// heading and its text. Section 3 has two texts: Our Vision and Our Mission. One section is edited at a time.
/// </summary>
public partial class AboutEditorPage : UserControl, IWebsiteEditor
{
    record AboutForm(string Heading, string Text1, string Text2, string Picture);

    int section = 1;
    AboutForm? original;
    string? storedImage, newImage;
    bool removeImage, loading;

    public AboutEditorPage()
    {
        InitializeComponent();
        EditorUi.Counter(txtAboutHeading, lblAboutHeadingCount, WebsiteContentService.MaxLength(WebsiteKeys.AboutHeading(1)));
        Disposed += (_, _) => picAboutImage.Image?.Dispose();
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
            string[] names = ["About Tri-Stone", "Our Principles", "Our Mission and Vision"];
            lblAboutSection.Text = $"Section {section} of 3: {names[section - 1]}";
            foreach (var (b, n) in new[] { (btnAboutSection1, 1), (btnAboutSection2, 2), (btnAboutSection3, 3) })
                b.Enabled = n != section;   // the open section's button is greyed out

            bool three = section == 3;
            lblAboutText1.Text = three ? "Our Vision" : "Section description";
            txtAboutText2.Visible = lblAboutText2.Visible = lblAboutText2Count.Visible = three;
            int max1 = WebsiteContentService.MaxLength(Text1Key(section));
            txtAboutText1.MaxLength = max1;
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
            lblAboutStatus.Text = "";
        }
        catch (Exception ex) { lblAboutStatus.Text = "Can't reach the database. " + ex.Message; }
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
        EditorUi.ShowPicture(picAboutImage, removeImage ? null : await WebsiteImages.LoadAsync(storedImage, newImage));
        lblAboutImageName.Text = PictureName == "(none)" ? "No picture yet (the website keeps its current one)." : PictureName;
    }

    void Field_Changed(object? sender, EventArgs e)
    {
        ShowCounts();
        if (!loading) lblAboutStatus.Text = HasUnsavedChanges ? EditorUi.Unsaved : "";
    }

    async void btnChooseAboutImage_Click(object sender, EventArgs e)
    {
        if (EditorUi.PickPicture(this) is not { } path) return;
        newImage = path;
        removeImage = false;
        await ShowImageAsync();
        Field_Changed(null, EventArgs.Empty);
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
        if (answer == SaveAnswer.NothingChanged) { lblAboutStatus.Text = "Nothing changed."; return; }
        if (answer == SaveAnswer.No) { lblAboutStatus.Text = "Not saved. Click Discard Changes to undo your changes."; return; }

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
            if (error != null) { lblAboutStatus.Text = "Unable to save changes: " + error; return; }
            await LoadAsync();
            lblAboutStatus.Text = "Changes saved successfully. Click Publish Website to put them on the live site.";
        }
        catch (Exception ex) { lblAboutStatus.Text = "Unable to save changes. Please try again. " + ex.Message; }
        finally { btnSaveAbout.Enabled = true; }
    }

    async void btnDiscardAbout_Click(object sender, EventArgs e) => await LoadAsync();
}
