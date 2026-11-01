using Company.OfficeApp.UI;
using Company.Shared.Models;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > Home (Figma "ADMIN HOME"): the hero picture, main headline and subheadline. Save Changes keeps
/// the edits (not live yet); Publish Website in the top bar makes them live.
/// </summary>
public partial class HomeEditorPage : UserControl, IWebsiteEditor
{
    record HomeForm(string Headline, string Subheadline, string Picture);

    static readonly string[] Keys = [WebsiteKeys.HeroImage, WebsiteKeys.Headline, WebsiteKeys.Subheadline];

    HomeForm? original;
    string? storedImage;     // the saved picture's relative path
    string? newImage;        // a picked local file, not saved yet
    bool removeImage;        // Remove Picture clicked, not saved yet
    bool loading;

    public HomeEditorPage()
    {
        InitializeComponent();
        EditorUi.Counter(txtHeadline, lblHeadlineCount, WebsiteContentService.MaxLength(WebsiteKeys.Headline));
        EditorUi.Counter(txtSubheadline, lblSubheadlineCount, WebsiteContentService.MaxLength(WebsiteKeys.Subheadline));
        Disposed += (_, _) => picHeroImage.Image?.Dispose();
    }

    public bool HasUnsavedChanges => original != null && ReadForm() != original;

    async void HomeEditorPage_Load(object sender, EventArgs e) => await LoadAsync();

    async Task LoadAsync()
    {
        using var busy = Responsive.Busy(this);
        loading = true;
        try
        {
            var v = await WebsiteContentService.LoadAsync(Keys);
            storedImage = v[WebsiteKeys.HeroImage];
            newImage = null;
            removeImage = false;
            txtHeadline.Text = v[WebsiteKeys.Headline] ?? "";
            txtSubheadline.Text = v[WebsiteKeys.Subheadline] ?? "";
            await ShowImageAsync();
            lblHomeLastSaved.Text = EditorUi.SavedText(await WebsiteContentService.LastSavedAsync(Keys));
            original = ReadForm();
            lblHomeStatus.Text = "";
        }
        catch (Exception ex) { lblHomeStatus.Text = "Can't reach the database. " + ex.Message; }
        finally { loading = false; }
    }

    string PictureName => removeImage ? "(none)" : newImage != null ? Path.GetFileName(newImage)
                        : storedImage != null ? Path.GetFileName(storedImage) : "(none)";

    HomeForm ReadForm() => new(txtHeadline.Text.Trim(), txtSubheadline.Text.Trim(), PictureName);

    async Task ShowImageAsync()
    {
        EditorUi.ShowPicture(picHeroImage, removeImage ? null : await WebsiteImages.LoadAsync(storedImage, newImage));
        lblHeroImageName.Text = PictureName == "(none)" ? "No picture yet (the website keeps its current one)." : PictureName;
    }

    void Field_Changed(object? sender, EventArgs e)
    {
        if (!loading) lblHomeStatus.Text = HasUnsavedChanges ? EditorUi.Unsaved : "";
    }

    async void btnChooseHeroImage_Click(object sender, EventArgs e)
    {
        if (EditorUi.PickPicture(this) is not { } path) return;
        newImage = path;
        removeImage = false;
        await ShowImageAsync();
        Field_Changed(null, EventArgs.Empty);
    }

    async void btnRemoveHeroImage_Click(object sender, EventArgs e)
    {
        newImage = null;
        removeImage = storedImage != null;
        await ShowImageAsync();
        Field_Changed(null, EventArgs.Empty);
    }

    async void btnSaveHome_Click(object sender, EventArgs e)
    {
        if (original == null) return;
        var input = ReadForm();
        var answer = RecordForm.ConfirmSave(this, "the Home page", original, input);
        if (answer == SaveAnswer.NothingChanged) { lblHomeStatus.Text = "Nothing changed."; return; }
        if (answer == SaveAnswer.No) { lblHomeStatus.Text = "Not saved. Click Discard Changes to undo your changes."; return; }

        btnSaveHome.Enabled = false;
        try
        {
            var images = new Dictionary<string, string?>();
            if (newImage != null) images[WebsiteKeys.HeroImage] = newImage;
            else if (removeImage) images[WebsiteKeys.HeroImage] = null;
            var error = await WebsiteContentService.SaveAsync("Home", new()
            {
                [WebsiteKeys.Headline] = input.Headline,
                [WebsiteKeys.Subheadline] = input.Subheadline,
            }, images);
            if (error != null) { lblHomeStatus.Text = "Unable to save changes: " + error; return; }
            await LoadAsync();
            lblHomeStatus.Text = "Changes saved successfully. Click Publish Website to put them on the live site.";
        }
        catch (Exception ex) { lblHomeStatus.Text = "Unable to save changes. Please try again. " + ex.Message; }
        finally { btnSaveHome.Enabled = true; }
    }

    async void btnDiscardHome_Click(object sender, EventArgs e) => await LoadAsync();
}
