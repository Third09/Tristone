using System.Drawing.Drawing2D;
using Company.OfficeApp.UI;
using Company.Shared.Models;
using Company.WebsiteAdmin.Services;
using static Company.OfficeApp.UI.StatusBox;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > Home (Figma "ADMIN HOME"): the hero picture, main headline and subheadline. Save Changes keeps
/// the edits (not live yet); Publish Website in the top bar makes them live. Laid out like the Figma: the hero card,
/// a Live Preview of the website's top (picture with the headline over it), the message box and the save bar.
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
    Image? picture;          // what the drop zone and the preview show

    public HomeEditorPage()
    {
        InitializeComponent();
        BackColor = pnlHomeSide.BackColor = pnlHomeFooter.BackColor = pnlSideGap.BackColor = Theme.Canvas;
        Padding = new Padding(24, 8, 24, 24);
        AutoScrollMinSize = new Size(1100, 700);   // smaller windows scroll instead of squeezing the cards
        foreach (var l in new[] { lblHeadlineCount, lblSubheadlineCount, lblHeroImageName, lblPreviewNote, lblHomeLastSaved })
            l.ForeColor = Theme.TextMuted;
        EditorUi.Counter(txtHeadline, lblHeadlineCount, WebsiteContentService.MaxLength(WebsiteKeys.Headline));
        EditorUi.Counter(txtSubheadline, lblSubheadlineCount, WebsiteContentService.MaxLength(WebsiteKeys.Subheadline));
        dzHero.BrowseRequested += async (_, _) => { if (EditorUi.PickPicture(this) is { } path) await UsePictureAsync(path); };
        dzHero.FilesDropped += async files =>
        {
            if (WebsiteImages.Check(files[0]) is { } problem) statusHome.Show(StatusKind.Error, "Picture not used", problem);
            else await UsePictureAsync(files[0]);
        };
        pnlPreview.Paint += PaintPreview;
        pnlPreview.Resize += (_, _) => pnlPreview.Invalidate();
        Disposed += (_, _) => picture?.Dispose();
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
            statusHome.Clear();
        }
        catch (Exception ex) { statusHome.Show(StatusKind.Error, "Can't reach the database", ex.Message); }
        finally { loading = false; }
    }

    string PictureName => removeImage ? "(none)" : newImage != null ? Path.GetFileName(newImage)
                        : storedImage != null ? Path.GetFileName(storedImage) : "(none)";

    HomeForm ReadForm() => new(txtHeadline.Text.Trim(), txtSubheadline.Text.Trim(), PictureName);

    async Task ShowImageAsync()
    {
        var old = picture;
        picture = removeImage ? null : await WebsiteImages.LoadAsync(storedImage, newImage);
        dzHero.Image = picture;
        old?.Dispose();
        btnRemoveHeroImage.Visible = picture != null;
        lblHeroImageName.Text = picture == null ? "No picture yet (the website keeps its current one). Click the box or drop a picture on it."
                                                : PictureName + "   (click the picture to change it)";
        lblHeroImageName.Left = picture == null ? btnRemoveHeroImage.Left : btnRemoveHeroImage.Right + 12;
        pnlPreview.Invalidate();
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
        pnlPreview.Invalidate();
        if (loading) return;
        if (HasUnsavedChanges) statusHome.Show(StatusKind.Info, "Unsaved Changes", "Remember to save your changes to keep your website content updated.");
        else statusHome.Clear();
    }

    /// <summary>The website's top as it will look: the picture (filling the box) darkened, with the headline over it.</summary>
    void PaintPreview(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        var r = pnlPreview.ClientRectangle;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(cardPreview.BackColor);
        using var clip = Rounded.Path(new Rectangle(0, 0, r.Width - 1, r.Height - 1), 8);
        g.SetClip(clip);
        if (picture != null)
        {
            float scale = Math.Max((float)r.Width / picture.Width, (float)r.Height / picture.Height);   // fill, crop the rest
            int w = (int)(picture.Width * scale), h = (int)(picture.Height * scale);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(picture, (r.Width - w) / 2, (r.Height - h) / 2, w, h);
        }
        else g.Clear(Theme.Text);
        using (var shade = new SolidBrush(Color.FromArgb(110, 0, 0, 0))) g.FillRectangle(shade, r);
        using var head = new Font("Segoe UI", 12f, FontStyle.Bold);
        using var sub = new Font("Segoe UI", 8f);
        var flags = TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis;
        TextRenderer.DrawText(g, txtHeadline.Text, head, new Rectangle(16, r.Height / 2 - 52, r.Width - 32, 56), Color.White, flags);
        TextRenderer.DrawText(g, txtSubheadline.Text, sub, new Rectangle(24, r.Height / 2 + 8, r.Width - 48, 60), Color.FromArgb(230, 255, 255, 255), flags);
        g.ResetClip();
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
        if (answer == SaveAnswer.NothingChanged) { statusHome.Show(StatusKind.Info, "Nothing changed", "There is nothing new to save."); return; }
        if (answer == SaveAnswer.No) { statusHome.Show(StatusKind.Info, "Not saved", "Click Discard Changes to undo your changes."); return; }

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
            if (error != null) { statusHome.Show(StatusKind.Error, "Unable to Save Changes", error); return; }
            await LoadAsync();
            statusHome.Show(StatusKind.Success, "Changes Saved Successfully", "Click Publish Website to put them on the live site.");
        }
        catch (Exception ex) { statusHome.Show(StatusKind.Error, "Unable to Save Changes", "Your changes could not be saved. Please try again. " + ex.Message); }
        finally { btnSaveHome.Enabled = true; }
    }

    async void btnDiscardHome_Click(object sender, EventArgs e) => await LoadAsync();
}
