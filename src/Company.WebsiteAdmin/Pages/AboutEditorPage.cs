using System.Drawing.Drawing2D;
using Company.OfficeApp.UI;
using Company.Shared.Models;
using Company.WebsiteAdmin.Services;
using static Company.OfficeApp.UI.StatusBox;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > About Us (Figma "admin about us section 1-3"): the About Us sections, each with a picture, a typed
/// heading and its text, and optionally a second heading and text ("Two parts", like Vision and Mission).
/// Any number of sections (owner request 2026-11-03): Add Section opens this page as a pop-up (<c>addOnly</c>: only
/// the form and the save bar), Move Up / Down sets the website order, Delete Section removes one (at least one stays).
/// One section is edited at a time; Save lists the changes. Laid out like the Figma: section choice cards, the section
/// card, a Live Preview, the message box and the save bar.
/// </summary>
public partial class AboutEditorPage : UserControl, IWebsiteEditor
{
    readonly bool addOnly;   // this copy is the Add Section pop-up
    record AboutForm(string Heading, string Text1, bool TwoParts, string Heading2, string Text2, string Picture);

    List<WebsiteAboutSection> sections = [];
    int? sectionId;
    AboutForm? original;
    string? storedImage, newImage;
    bool removeImage, loading;
    Image? picture;

    /// <summary>Raised by the Add pop-up with the new section's id.</summary>
    public event Action<int>? Added;

    public AboutEditorPage() : this(false) { }

    public AboutEditorPage(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        BackColor = pnlGap1.BackColor = pnlSections.BackColor = pnlAboutSide.BackColor = pnlSideGap.BackColor = pnlAboutFooter.BackColor = Theme.Canvas;
        flpAboutSections.BackColor = Theme.Canvas;
        Padding = new Padding(24, 8, 24, 24);
        foreach (var l in new[] { lblAboutSection, lblAboutImageName, lblAboutHeadingCount, lblAboutHeading2Count, lblAboutText1Count, lblAboutText2Count, lblAboutLastSaved })
            l.ForeColor = Theme.TextMuted;
        EditorUi.Counter(txtAboutHeading, lblAboutHeadingCount, WebAboutService.MaxHeading);
        EditorUi.Counter(txtAboutHeading2, lblAboutHeading2Count, WebAboutService.MaxHeading);
        txtAboutText1.MaxLength = txtAboutText2.MaxLength = WebAboutService.MaxBody;
        dzAbout.BrowseRequested += async (_, _) => { if (EditorUi.PickPicture(this) is { } path) await UsePictureAsync(path); };
        dzAbout.FilesDropped += async files =>
        {
            if (WebsiteImages.Check(files[0]) is { } problem) Say(StatusKind.Error, "Picture not used", problem);
            else await UsePictureAsync(files[0]);
        };
        pnlAboutPreview.Paint += PaintPreview;
        pnlSections.Resize += (_, _) => FitSectionCards();
        lblAboutInfo.SizeChanged += (_, _) => chkAboutTwoParts.Left = lblAboutInfo.Right + 24;   // just after the title
        chkAboutTwoParts.Left = lblAboutInfo.Right + 24;
        Disposed += (_, _) => picture?.Dispose();
        if (addOnly)
        {
            // The pop-up: just the form and its save bar.
            pnlSections.Visible = pnlGap1.Visible = pnlAboutSide.Visible = false;
            Size = new Size(800, 900);
            btnSaveAbout.Text = "Add Section";
            StartNew();
            return;
        }
        AutoScrollMinSize = new Size(1100, 1050);
    }

    public bool HasUnsavedChanges => original != null && ReadForm() != original;

    async void AboutEditorPage_Load(object sender, EventArgs e) { if (!addOnly) await LoadAsync(); }

    /// <summary>Messages: the coloured box on the right (the pop-up has none, it uses the save bar).</summary>
    void Say(StatusKind kind, string title, string text)
    {
        if (!addOnly) { statusAbout.Show(kind, title, text); return; }
        lblAboutLastSaved.Text = kind == StatusKind.None ? "" : $"{title}. {text}";
        lblAboutLastSaved.ForeColor = kind == StatusKind.Error ? Theme.Red : Theme.TextMuted;
    }

    bool OkToLeave() =>
        !HasUnsavedChanges || MessageBox.Show(this, "This section has unsaved changes. Leave it without saving?", "Unsaved changes",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

    /// <summary>Reloads the section list and shows <paramref name="selectId"/> (or the one open, or the first).</summary>
    async Task LoadAsync(int? selectId = null)
    {
        using var busy = Responsive.Busy(this);
        loading = true;
        try
        {
            sections = await WebAboutService.ListAsync();
            sectionId = sections.Any(s => s.AboutSectionId == (selectId ?? sectionId)) ? selectId ?? sectionId : sections.FirstOrDefault()?.AboutSectionId;
            ShowSectionCards();
            var s = sections.FirstOrDefault(x => x.AboutSectionId == sectionId);
            if (s == null) { original = null; return; }
            int n = sections.IndexOf(s) + 1;
            lblAboutImage.Text = $"{s.Heading} - Image";
            lblAboutSection.Text = $"Section {n} of {sections.Count}";
            btnMoveAboutUp.Enabled = n > 1;
            btnMoveAboutDown.Enabled = n < sections.Count;
            btnDeleteAboutSection.Enabled = sections.Count > 1;

            storedImage = s.ImagePath;
            newImage = null;
            removeImage = false;
            txtAboutHeading.Text = s.Heading;
            txtAboutText1.Text = Lines(s.Body);
            chkAboutTwoParts.Checked = s.Heading2 != null || s.Body2 != null;
            txtAboutHeading2.Text = s.Heading2 ?? "";
            txtAboutText2.Text = Lines(s.Body2);
            ShowParts();
            await ShowImageAsync();
            lblAboutLastSaved.Text = EditorUi.SavedText(await WebAboutService.LastSavedAsync(s.AboutSectionId));
            original = ReadForm();
            statusAbout.Clear();
        }
        catch (Exception ex) { Say(StatusKind.Error, "Can't reach the database", ex.Message); }
        finally { loading = false; }
    }

    static string Lines(string? s) => (s ?? "").Replace("\r\n", "\n").Replace("\n", Environment.NewLine);

    /// <summary>One choice card per section, in website order; the open one is highlighted.</summary>
    void ShowSectionCards()
    {
        flpAboutSections.SuspendLayout();
        foreach (Control c in flpAboutSections.Controls.Cast<Control>().ToList()) c.Dispose();
        for (int i = 0; i < sections.Count; i++)
        {
            var s = sections[i];
            var b = new Button
            {
                Name = "btnAboutSection" + (i + 1),
                Text = $"{s.Heading.Replace("&", "&&")}  ·  Section {i + 1}",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 11F),
                Size = new Size(280, 56),
                Margin = new Padding(0, 0, 16, 12),
                Tag = s.AboutSectionId,
            };
            b.Click += btnAboutSection_Click;
            flpAboutSections.Controls.Add(b);
            Theme.SetSelected(b, s.AboutSectionId == sectionId);
        }
        flpAboutSections.ResumeLayout();
        FitSectionCards();
    }

    /// <summary>The cards wrap to more rows when they don't fit; the sections bar grows with them.</summary>
    void FitSectionCards()
    {
        flpAboutSections.Width = pnlSections.ClientSize.Width;
        flpAboutSections.Height = flpAboutSections.GetPreferredSize(new Size(flpAboutSections.Width, 0)).Height;
        pnlSections.Height = flpAboutSections.Bottom + 4;
    }

    /// <summary>The empty form of the Add Section pop-up.</summary>
    void StartNew()
    {
        loading = true;
        lblAboutImage.Text = "New Section - Image";
        lblAboutSection.Text = "New section";
        storedImage = newImage = null;
        removeImage = false;
        txtAboutHeading.Text = txtAboutText1.Text = txtAboutHeading2.Text = txtAboutText2.Text = "";
        chkAboutTwoParts.Checked = false;
        ShowParts();
        _ = ShowImageAsync();
        lblAboutLastSaved.Text = "";
        original = ReadForm();
        loading = false;
    }

    async void btnAboutSection_Click(object? sender, EventArgs e)
    {
        if (sender is not Button { Tag: int wanted } || wanted == sectionId || !OkToLeave()) return;
        sectionId = wanted;
        await LoadAsync();
    }

    void chkAboutTwoParts_CheckedChanged(object? sender, EventArgs e)
    {
        ShowParts();
        Field_Changed(null, EventArgs.Empty);
    }

    /// <summary>One part: a long description. Two parts: first heading + text, then second heading + text.</summary>
    void ShowParts()
    {
        bool two = chkAboutTwoParts.Checked;
        lblAboutHeading.Text = two ? "First Heading" : "Section Heading";
        lblAboutText1.Text = two ? "First Text" : "Section Description";
        lblAboutText2.Text = "Second Text";
        foreach (var c in new Control[] { lblAboutHeading2, lblAboutHeading2Count, frmAboutHeading2, lblAboutText2, lblAboutText2Count, frmAboutText2 })
            c.Visible = two;
        SetFrame(frmAboutText1, txtAboutText1, 442, two ? 100 : 330);
        lblAboutHeading2.Top = lblAboutHeading2Count.Top = 556;
        frmAboutHeading2.Top = 580;
        lblAboutText2.Top = lblAboutText2Count.Top = 638;
        SetFrame(frmAboutText2, txtAboutText2, 662, 110);
        ShowCounts();
    }

    static void SetFrame(Control frame, TextBox box, int top, int height)
    {
        frame.Top = top;
        frame.Height = height;
        box.Height = height - 18;
    }

    void ShowCounts()
    {
        lblAboutText1Count.Text = $"{txtAboutText1.TextLength}/{txtAboutText1.MaxLength}";
        lblAboutText2Count.Text = $"{txtAboutText2.TextLength}/{txtAboutText2.MaxLength}";
    }

    string PictureName => removeImage ? "(none)" : newImage != null ? Path.GetFileName(newImage)
                        : storedImage != null ? Path.GetFileName(storedImage) : "(none)";

    AboutForm ReadForm()
    {
        bool two = chkAboutTwoParts.Checked;
        return new(txtAboutHeading.Text.Trim(), txtAboutText1.Text.Trim(), two,
                   two ? txtAboutHeading2.Text.Trim() : "", two ? txtAboutText2.Text.Trim() : "", PictureName);
    }

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
        if (loading || addOnly) return;
        if (HasUnsavedChanges) statusAbout.Show(StatusKind.Info, "Unsaved Changes", "Remember to save your changes to keep your website content updated.");
        else statusAbout.Clear();
    }

    /// <summary>The section as the website shows it: picture, heading, then the text (and the second part).</summary>
    void PaintPreview(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        var r = pnlAboutPreview.ClientRectangle;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(cardAboutPreview.BackColor);
        var pic = new Rectangle(0, 0, r.Width - 1, 110);
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
        const TextFormatFlags wrap = TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;
        int y = 118;
        void Part(string heading, string text, int height)
        {
            TextRenderer.DrawText(g, heading, head, new Rectangle(0, y, r.Width, 22), Theme.Text, TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            TextRenderer.DrawText(g, text, Font, new Rectangle(0, y + 24, r.Width, height - 24), Theme.TextMuted, wrap);
            y += height;
        }
        if (chkAboutTwoParts.Checked)
        {
            int half = (r.Height - y) / 2;
            Part(txtAboutHeading.Text, txtAboutText1.Text, half);
            Part(txtAboutHeading2.Text, txtAboutText2.Text, half);
        }
        else Part(txtAboutHeading.Text, txtAboutText1.Text, r.Height - y);
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
        var input = ReadForm();
        var data = new AboutSectionInput(input.Heading, input.Text1, input.TwoParts ? input.Heading2 : null, input.TwoParts ? input.Text2 : null);
        if (addOnly)
        {
            btnSaveAbout.Enabled = false;
            try
            {
                var (id, error) = await WebAboutService.CreateAsync(data, newImage);
                if (error != null) { Say(StatusKind.Error, "Not added", error); return; }
                Added?.Invoke(id!.Value);
            }
            catch (Exception ex) { Say(StatusKind.Error, "Not added", "Please try again. " + ex.Message); }
            finally { if (!IsDisposed) btnSaveAbout.Enabled = true; }
            return;
        }

        if (original == null || sectionId is not { } sid) return;
        var labels = new Dictionary<string, string>
        {
            ["Text1"] = input.TwoParts ? "First text" : "Description", ["TwoParts"] = "Two parts",
            ["Heading2"] = "Second heading", ["Text2"] = "Second text",
        };
        var answer = RecordForm.ConfirmSave(this, $"About Us section \"{original.Heading}\"", original, input, labels);
        if (answer == SaveAnswer.NothingChanged) { Say(StatusKind.Info, "Nothing changed", "There is nothing new to save."); return; }
        if (answer == SaveAnswer.No) { Say(StatusKind.Info, "Not saved", "Click Discard Changes to undo your changes."); return; }

        btnSaveAbout.Enabled = false;
        try
        {
            var error = await WebAboutService.UpdateAsync(sid, data, newImage, removeImage);
            if (error != null) { Say(StatusKind.Error, "Unable to Save Changes", error); return; }
            await LoadAsync();
            Say(StatusKind.Success, "Changes Saved Successfully", "Click Publish Website to put them on the live site.");
        }
        catch (Exception ex) { Say(StatusKind.Error, "Unable to Save Changes", "Please try again. " + ex.Message); }
        finally { btnSaveAbout.Enabled = true; }
    }

    async void btnDiscardAbout_Click(object sender, EventArgs e)
    {
        if (addOnly) StartNew();
        else await LoadAsync();
    }

    async void btnAddAboutSection_Click(object? sender, EventArgs e)
    {
        if (!OkToLeave()) return;
        var page = new AboutEditorPage(addOnly: true);
        if (RecordForm.AddDialog(this, "Add About Us Section", page, a => page.Added += a) is not { } newId) return;
        await LoadAsync(newId);
        Say(StatusKind.Success, "Section added", "It shows last on the website. Use Move Up to change its place, then click Publish Website.");
    }

    async void btnMoveAboutUp_Click(object? sender, EventArgs e) => await MoveAsync(-1);
    async void btnMoveAboutDown_Click(object? sender, EventArgs e) => await MoveAsync(+1);

    async Task MoveAsync(int direction)
    {
        if (sectionId is not { } id || !OkToLeave()) return;
        try
        {
            var error = await WebAboutService.MoveAsync(id, direction);
            if (error != null) { Say(StatusKind.Error, "Not moved", error); return; }
            await LoadAsync(id);
            Say(StatusKind.Success, direction < 0 ? "Moved up" : "Moved down", "Click Publish Website to update the live site.");
        }
        catch (Exception ex) { Say(StatusKind.Error, "Can't reach the database", ex.Message); }
    }

    async void btnDeleteAboutSection_Click(object? sender, EventArgs e)
    {
        if (sectionId is not { } id || sections.FirstOrDefault(s => s.AboutSectionId == id) is not { } s) return;
        if (MessageBox.Show(this, $"Delete the About Us section \"{s.Heading}\" from the website?", "Delete section",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        try
        {
            var error = await WebAboutService.DeleteAsync(id);
            if (error != null) { Say(StatusKind.Error, "Not deleted", error); return; }
            sectionId = null;
            await LoadAsync();
            Say(StatusKind.Success, "Section deleted", "Click Publish Website to update the live site.");
        }
        catch (Exception ex) { Say(StatusKind.Error, "Can't reach the database", ex.Message); }
    }
}
