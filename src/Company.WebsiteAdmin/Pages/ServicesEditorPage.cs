using System.Drawing.Drawing2D;
using Company.OfficeApp.UI;
using Company.Shared.Models;
using Company.WebsiteAdmin.Services;
using static Company.OfficeApp.UI.StatusBox;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > Services (Figma "admin services"): the service cards. The name comes from the 3 fixed services,
/// a short description and one picture per card (no location since 2026-11-08, owner request).
/// List page pattern (RecordForm): Add opens this page as a pop-up (<c>addOnly</c>: only the form and the save bar),
/// the form is locked until a row is clicked, Save lists the changes. A Live Preview draws the card as the website shows it.
/// </summary>
public partial class ServicesEditorPage : UserControl, IWebsiteEditor
{
    readonly bool addOnly;   // this copy is the Add New Service pop-up
    record ServiceForm(string Service, string Description, string Icon, string Picture);

    int? editingId;
    WebServiceRow? showingRow;
    ServiceForm? original;
    string? storedImage, newImage;
    Image? picture;
    bool filling, loading;
    readonly LatestOnly loads = new();

    /// <summary>Raised by the Add pop-up with the new service's id.</summary>
    public event Action<int>? Added;

    public ServicesEditorPage() : this(false) { }

    public ServicesEditorPage(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        BackColor = pnlGap1.BackColor = pnlServicesSide.BackColor = pnlSideGap.BackColor = pnlServicesFooter.BackColor = Theme.Canvas;
        Padding = new Padding(24, 8, 24, 24);
        foreach (var l in new[] { lblServicesHint, lblServiceImageName, lblServiceDescCount, lblServiceMsg }) l.ForeColor = Theme.TextMuted;
        dgvWebServices.AutoGenerateColumns = false;
        RecordForm.NoAutoSelect(dgvWebServices, on => filling = on, RestoreSelection);
        cboServiceName.Items.AddRange([.. WebsiteServiceNames.Suggestions.Select(s => s.Name)]);
        cboServiceName.MaxDropDownItems = 15;
        cboServiceIcon.MaxDropDownItems = 12;
        cboServiceIcon.Items.AddRange(WebsiteServiceIcons.All);
        cboServiceIcon.DrawItem += DrawIconItem;
        // Typing or picking a listed service sets its usual icon.
        cboServiceName.TextChanged += (_, _) =>
        {
            if (loading) return;
            var known = WebsiteServiceNames.Suggestions.Any(s => s.Name.Equals(cboServiceName.Text.Trim(), StringComparison.OrdinalIgnoreCase));
            if (known) cboServiceIcon.SelectedItem = WebsiteServiceIcons.For(cboServiceName.Text);
        };
        EditorUi.Counter(txtServiceDesc, lblServiceDescCount, WebServiceService.MaxDescription);
        dzService.BrowseRequested += async (_, _) => { if (EditorUi.PickPicture(this) is { } path) await UsePictureAsync(path); };
        dzService.FilesDropped += async files =>
        {
            if (WebsiteImages.Check(files[0]) is { } problem) Say(StatusKind.Error, "Picture not used", problem);
            else await UsePictureAsync(files[0]);
        };
        pnlServicePreview.Paint += PaintPreview;
        Disposed += (_, _) => picture?.Dispose();
        if (addOnly)
        {
            // The pop-up: just the form and its save bar.
            cardServicesList.Visible = pnlGap1.Visible = pnlServicesSide.Visible = false;
            Size = new Size(800, 760);
            StartNew();
            return;
        }
        AutoScrollMinSize = new Size(1100, 1010);
        ShowNothingSelected();
    }

    public bool HasUnsavedChanges => editingId != null && original != null && ReadForm() != original;

    async void ServicesEditorPage_Load(object sender, EventArgs e) { if (!addOnly) await LoadAsync(); }

    WebServiceRow? Selected => dgvWebServices.SelectedRows.Count > 0 ? dgvWebServices.SelectedRows[0].DataBoundItem as WebServiceRow : null;

    /// <summary>Messages: the coloured box on the right (the pop-up has none) and a short line in the save bar.</summary>
    void Say(StatusKind kind, string title, string text)
    {
        if (!addOnly) statusService.Show(kind, title, text);
        lblServiceMsg.Text = kind == StatusKind.None ? "" : $"{title}. {text}";
    }

    async Task LoadAsync(int? selectId = null)
    {
        if (addOnly) return;
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        List<WebServiceRow> rows;
        try { rows = await WebServiceService.ListAsync(); }
        catch (Exception ex) { Say(StatusKind.Error, "Can't reach the database", ex.Message); return; }
        if (!loads.IsLatest(ticket)) return;

        selectId ??= editingId;
        filling = true;
        dgvWebServices.DataSource = rows;
        dgvWebServices.ClearSelection();   // never pick the first row by itself
        if (selectId is { } id) Responsive.SelectRow(dgvWebServices, (WebServiceRow r) => r.WebServiceId == id);
        filling = false;
        if (Selected is { } s) await ShowAsync(s); else ShowNothingSelected();
    }

    void RestoreSelection()
    {
        filling = true;
        dgvWebServices.ClearSelection();
        if (editingId is { } id) Responsive.SelectRow(dgvWebServices, (WebServiceRow r) => r.WebServiceId == id);
        filling = false;
        if (Selected == null) ShowNothingSelected();
    }

    async void dgvWebServices_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        if (Selected is { } s) await ShowAsync(s); else ShowNothingSelected();
    }

    void SetFormEnabled(bool on)
    {
        RecordForm.SetEnabled(cardServiceForm, lblFormMode, on);
        btnSaveWebService.Enabled = btnCancelWebService.Enabled = on;
        btnMoveServiceUp.Enabled = btnMoveServiceDown.Enabled = btnHideShowService.Enabled = btnDeleteWebService.Enabled = btnDuplicateService.Enabled = on && !addOnly;
    }

    void ClearForm()
    {
        loading = true;
        cboServiceName.SelectedIndex = -1;
        cboServiceName.Text = "";
        cboServiceIcon.SelectedItem = WebsiteServiceIcons.Ruler;
        txtServiceDesc.Clear();
        storedImage = newImage = null;
        SetPicture(null);
        loading = false;
        Say(StatusKind.None, "", "");
    }

    void ShowNothingSelected()
    {
        editingId = null;
        showingRow = null;
        original = null;
        ClearForm();
        lblFormMode.Text = "Service Image";
        lblServiceImageName.Text = "Click a service in the list to edit it.";
        SetFormEnabled(false);
    }

    void StartNew()
    {
        editingId = null;
        ClearForm();
        lblFormMode.Text = "New Service";
        lblServiceImageName.Text = "Click the box or drop a picture on it.";
        btnSaveWebService.Text = "Add Service";
        btnCancelWebService.Text = "Cancel";
    }

    async Task ShowAsync(WebServiceRow row)
    {
        if (await WebServiceService.GetAsync(row.WebServiceId) is not { } s) { ShowNothingSelected(); return; }
        SetFormEnabled(true);
        loading = true;
        editingId = s.WebServiceId;
        showingRow = row;
        lblFormMode.Text = $"Service Image - Slot {row.Slot}";
        btnHideShowService.Text = s.IsVisible ? "Unpublish" : "Publish";
        cboServiceName.Text = s.ServiceName;
        cboServiceIcon.SelectedItem = s.Icon;
        txtServiceDesc.Text = s.ShortDescription ?? "";
        storedImage = s.ImagePath;
        newImage = null;
        SetPicture(await WebsiteImages.LoadAsync(storedImage));
        loading = false;
        Say(StatusKind.None, "", "");
        original = ReadForm();
    }

    string PictureName => newImage != null ? Path.GetFileName(newImage) : storedImage != null ? Path.GetFileName(storedImage) : "(none)";

    ServiceForm ReadForm() => new(cboServiceName.Text.Trim(),
                                  txtServiceDesc.Text.Trim(), WebsiteServiceIcons.Label(Icon), PictureName);

    string Icon => (string?)cboServiceIcon.SelectedItem ?? WebsiteServiceIcons.Ruler;

    void SetPicture(Image? image)
    {
        var old = picture;
        picture = image;
        dzService.Image = image;
        old?.Dispose();
        lblServiceImageName.Text = image == null ? "No picture: the website shows its own picture for the chosen icon. Click the box to add one." : PictureName + "   (click the picture to change it)";
        pnlServicePreview.Invalidate();
    }

    async Task UsePictureAsync(string path)
    {
        newImage = path;
        SetPicture(await WebsiteImages.LoadAsync(null, path));
        Field_Changed(null, EventArgs.Empty);
    }

    void Field_Changed(object? sender, EventArgs e)
    {
        pnlServicePreview.Invalidate();
        if (loading || editingId == null || original == null) return;
        if (HasUnsavedChanges) Say(StatusKind.Info, "Unsaved Changes", "Remember to save your changes to keep your website content updated.");
        else Say(StatusKind.None, "", "");
    }

    /// <summary>Icon list: the icon picture, then its name.</summary>
    void DrawIconItem(object? sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index < 0) return;
        var icon = (string)cboServiceIcon.Items[e.Index]!;
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.DrawImage(ServiceIconImages.Get(icon), e.Bounds.X + 4, e.Bounds.Y + 3, e.Bounds.Height - 6, e.Bounds.Height - 6);
        var text = new Rectangle(e.Bounds.X + e.Bounds.Height + 6, e.Bounds.Y, e.Bounds.Width - e.Bounds.Height - 6, e.Bounds.Height);
        TextRenderer.DrawText(e.Graphics, WebsiteServiceIcons.Label(icon), e.Font ?? Font, text, e.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        e.DrawFocusRectangle();
    }

    /// <summary>The service card as the website shows it: picture on top, then the name, description and where.</summary>
    void PaintPreview(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        var r = pnlServicePreview.ClientRectangle;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(cardServicePreview.BackColor);
        if (editingId == null) return;
        var box = new Rectangle(0, 0, r.Width - 1, r.Height - 1);
        using var path = Rounded.Path(box, 8);
        using (var pen = new Pen(Theme.Line)) g.DrawPath(pen, path);
        var pic = new Rectangle(1, 1, r.Width - 2, 140);
        g.SetClip(path);
        if (picture != null)
        {
            float scale = Math.Max((float)pic.Width / picture.Width, (float)pic.Height / picture.Height);
            int w = (int)(picture.Width * scale), h = (int)(picture.Height * scale);
            g.SetClip(pic, CombineMode.Intersect);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(picture, pic.X + (pic.Width - w) / 2, pic.Y + (pic.Height - h) / 2, w, h);
        }
        else if (ServiceIconImages.DefaultPicture(Icon) is { } site)
        {
            // No picture of its own: the website's picture for this icon (from the Preview Website copy, when there is one).
            float scale = Math.Max((float)pic.Width / site.Width, (float)pic.Height / site.Height);
            int w = (int)(site.Width * scale), h = (int)(site.Height * scale);
            g.SetClip(pic, CombineMode.Intersect);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(site, pic.X + (pic.Width - w) / 2, pic.Y + (pic.Height - h) / 2, w, h);
        }
        else using (var gray = new SolidBrush(Theme.GridEmpty)) g.FillRectangle(gray, pic);
        g.ResetClip();
        using var head = new Font("Segoe UI", 11f, FontStyle.Bold);
        TextRenderer.DrawText(g, cboServiceName.Text.Trim(), head, new Rectangle(14, 150, r.Width - 64, 24), Theme.Text, TextFormatFlags.EndEllipsis);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.DrawImage(ServiceIconImages.Get(Icon), r.Width - 44, 146, 30, 30);
        TextRenderer.DrawText(g, txtServiceDesc.Text, Font, new Rectangle(14, 176, r.Width - 28, 40), Theme.TextMuted, TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis);
    }

    async void btnAddWebService_Click(object sender, EventArgs e)
    {
        var page = new ServicesEditorPage(addOnly: true);
        if (RecordForm.AddDialog(this, "Add New Service", page, a => page.Added += a) is not { } newId) return;
        editingId = null;
        await LoadAsync(newId);
        Say(StatusKind.Success, "Service added", "Click Publish Website to put it on the live site.");
    }

    async void btnSaveWebService_Click(object sender, EventArgs e)
    {
        if (!addOnly && editingId == null) return;   // the form is locked when nothing is selected
        var form = ReadForm();
        var input = new WebServiceInput(form.Service, null, form.Description, Icon);   // no location since 2026-11-08 (owner)
        if (editingId != null && original != null)
        {
            var answer = RecordForm.ConfirmSave(this, original.Service, original, form);
            if (answer == SaveAnswer.NothingChanged) { Say(StatusKind.Info, "Nothing changed", "There is nothing new to save."); return; }
            if (answer == SaveAnswer.No) { Say(StatusKind.Info, "Not saved", "Click Discard Changes to undo your changes."); return; }
        }
        btnSaveWebService.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await WebServiceService.UpdateAsync(id, input, newImage);
                if (error != null) { Say(StatusKind.Error, "Unable to Save Changes", error); return; }
                editingId = null;   // reload the form from the saved record
                await LoadAsync(id);
                Say(StatusKind.Success, "Changes Saved Successfully", "Click Publish Website to put them on the live site.");
            }
            else
            {
                var (newId, error) = await WebServiceService.CreateAsync(input, newImage);
                if (newId is { } added) Added?.Invoke(added); else Say(StatusKind.Error, "Not added", error ?? "");
            }
        }
        catch (Exception ex) { Say(StatusKind.Error, "Unable to Save Changes", "Please try again. " + ex.Message); }
        finally { btnSaveWebService.Enabled = true; }
    }

    async void btnCancelWebService_Click(object sender, EventArgs e)
    {
        if (addOnly) { FindForm()?.Close(); return; }
        if (showingRow is { } r) await ShowAsync(r);
    }

    async Task RunAsync(Func<int, Task<string?>> action, string done)
    {
        if (editingId is not { } id) return;
        try
        {
            var error = await action(id);
            if (error != null) { Say(StatusKind.Error, "Not done", error); return; }
            editingId = null;
            await LoadAsync(id);
            Say(StatusKind.Success, done, "Click Publish Website to update the live site.");
        }
        catch (Exception ex) { Say(StatusKind.Error, "Can't reach the database", ex.Message); }
    }

    async void btnDuplicateService_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id) return;
        if (HasUnsavedChanges && MessageBox.Show(this, "This card has unsaved changes. They won't be in the copy. Copy it anyway?",
                "Duplicate", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        try
        {
            var (copyId, error) = await WebServiceService.DuplicateAsync(id);
            if (copyId == null) { Say(StatusKind.Error, "Not copied", error ?? ""); return; }
            editingId = null;
            await LoadAsync(copyId);
            Say(StatusKind.Success, "Copy made", "The copy is hidden: change what is different, save, then click Publish above the list to show it.");
        }
        catch (Exception ex) { Say(StatusKind.Error, "Can't reach the database", ex.Message); }
    }

    async void btnMoveServiceUp_Click(object sender, EventArgs e) => await RunAsync(id => WebServiceService.MoveAsync(id, -1), "Moved up");
    async void btnMoveServiceDown_Click(object sender, EventArgs e) => await RunAsync(id => WebServiceService.MoveAsync(id, +1), "Moved down");

    async void btnHideShowService_Click(object sender, EventArgs e)
    {
        if (showingRow is not { } r) return;
        bool show = r.Shown != "Visible";
        await RunAsync(id => WebServiceService.SetVisibleAsync(id, show), show ? "Shown again" : "Hidden");
    }

    async void btnDeleteWebService_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id || showingRow is not { } r) return;
        if (MessageBox.Show(this, $"Delete the service card \"{r.ServiceName}\" from the website?",
                "Delete service", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        try
        {
            var error = await WebServiceService.DeleteAsync(id);
            if (error != null) { Say(StatusKind.Error, "Not deleted", error); return; }
            editingId = null;
            await LoadAsync();
            Say(StatusKind.Success, "Service deleted", "Click Publish Website to update the live site.");
        }
        catch (Exception ex) { Say(StatusKind.Error, "Can't reach the database", ex.Message); }
    }
}
