using Company.OfficeApp.UI;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > Projects (Figma "ADMIN PROJECTS"): the featured projects, picked from the office Projects list.
/// List page pattern: nothing selected by itself, Add opens a pop-up, the form is locked until a row is clicked,
/// Save lists the changes. Photos upload right away (up to 10 per project, with a progress bar and a retry message
/// for files that fail); Showcase photos are the ones in the website carousel.
/// </summary>
public partial class ProjectsEditorPage : UserControl, IWebsiteEditor
{
    record ProjectForm(string Description);

    WebProjectRow? showing;
    ProjectForm? original;
    bool filling, fillingPhotos;
    readonly LatestOnly loads = new();

    public ProjectsEditorPage()
    {
        InitializeComponent();
        dgvWebProjects.AutoGenerateColumns = false;
        dgvPhotos.AutoGenerateColumns = false;
        RecordForm.NoAutoSelect(dgvWebProjects, on => filling = on, RestoreSelection);
        RecordForm.NoAutoSelect(dgvPhotos, on => fillingPhotos = on, () => { fillingPhotos = true; dgvPhotos.ClearSelection(); fillingPhotos = false; ShowPhoto(); });
        EditorUi.Counter(txtProjectDesc, lblProjectDescCount, WebProjectService.MaxDescription);
        Disposed += (_, _) => picPhotoPreview.Image?.Dispose();
        Responsive.FitGrid(this, dgvWebProjects);
        ShowNothingSelected();
    }

    public bool HasUnsavedChanges => showing != null && original != null && ReadForm() != original;

    async void ProjectsEditorPage_Load(object sender, EventArgs e) => await LoadAsync();

    WebProjectRow? Selected => dgvWebProjects.SelectedRows.Count > 0 ? dgvWebProjects.SelectedRows[0].DataBoundItem as WebProjectRow : null;
    PhotoRow? SelectedPhoto => dgvPhotos.SelectedRows.Count > 0 ? dgvPhotos.SelectedRows[0].DataBoundItem as PhotoRow : null;

    async Task LoadAsync(int? selectId = null)
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        List<WebProjectRow> rows;
        try { rows = await WebProjectService.ListAsync(); }
        catch (Exception ex) { lblWebProjectMsg.Text = "Can't reach the database. " + ex.Message; return; }
        if (!loads.IsLatest(ticket)) return;

        selectId ??= showing?.WebProjectId;
        filling = true;
        dgvWebProjects.DataSource = rows;
        dgvWebProjects.ClearSelection();   // never pick the first row by itself
        if (selectId is { } id) Responsive.SelectRow(dgvWebProjects, (WebProjectRow r) => r.WebProjectId == id);
        filling = false;
        if (Selected is { } s) await ShowAsync(s); else ShowNothingSelected();
    }

    void RestoreSelection()
    {
        filling = true;
        dgvWebProjects.ClearSelection();
        if (showing?.WebProjectId is { } id) Responsive.SelectRow(dgvWebProjects, (WebProjectRow r) => r.WebProjectId == id);
        filling = false;
        if (Selected == null) ShowNothingSelected();
    }

    async void dgvWebProjects_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        if (Selected is { } s) await ShowAsync(s); else ShowNothingSelected();
    }

    void ShowNothingSelected()
    {
        showing = null;
        original = null;
        lblSlotTitle.Text = "Click a project in the list to edit it.";
        lblProjectInfo.Text = "Location: -    Category: -";
        txtProjectDesc.Clear();
        dgvPhotos.DataSource = null;
        EditorUi.ShowPicture(picPhotoPreview, null);
        lblUploadStatus.Text = "";
        foreach (var c in FormControls) c.Enabled = false;
    }

    Control[] FormControls => [btnMoveProjectUp, btnMoveProjectDown, btnHideShowProject, btnDeleteWebProject, txtProjectDesc,
                               btnSaveWebProject, btnDiscardWebProject, dgvPhotos, btnAddPhotos, btnRemovePhoto, btnPhotoUp,
                               btnPhotoDown, btnToggleShowcase];

    async Task ShowAsync(WebProjectRow r)
    {
        foreach (var c in FormControls) c.Enabled = true;
        showing = r;
        lblSlotTitle.Text = $"Slot #{r.Slot}: {r.ProjectName} ({r.Shown} on the website)";
        lblProjectInfo.Text = $"Location: {r.Location}    Category: {r.Sector}    (change these in the office app's Projects page)";
        btnHideShowProject.Text = r.Shown == "Visible" ? "Unpublish" : "Publish";
        txtProjectDesc.Text = r.ShortDescription ?? "";
        original = ReadForm();
        await LoadPhotosAsync();
    }

    ProjectForm ReadForm() => new(txtProjectDesc.Text.Trim());

    async void btnAddWebProject_Click(object sender, EventArgs e)
    {
        using var f = new AddWebProjectForm();
        if (f.ShowDialog(this) != DialogResult.OK || f.NewId is not { } id) return;
        showing = null;
        await LoadAsync(id);
        lblWebProjectMsg.Text = "Project added. Add its photos on the right.";
    }

    async Task RunAsync(Func<Task<string?>> action, string done)
    {
        try
        {
            var error = await action();
            if (error != null) { lblWebProjectMsg.Text = error; return; }
            var id = showing?.WebProjectId;
            showing = null;   // reload the form from the saved record
            await LoadAsync(id);
            lblWebProjectMsg.Text = done;
        }
        catch (Exception ex) { lblWebProjectMsg.Text = "Can't reach the database. " + ex.Message; }
    }

    async void btnMoveProjectUp_Click(object sender, EventArgs e)
    {
        if (showing is { } s) await RunAsync(() => WebProjectService.MoveAsync(s.WebProjectId, -1), "Moved up.");
    }

    async void btnMoveProjectDown_Click(object sender, EventArgs e)
    {
        if (showing is { } s) await RunAsync(() => WebProjectService.MoveAsync(s.WebProjectId, +1), "Moved down.");
    }

    async void btnHideShowProject_Click(object sender, EventArgs e)
    {
        if (showing is not { } s) return;
        bool show = s.Shown != "Visible";
        await RunAsync(() => WebProjectService.SetVisibleAsync(s.WebProjectId, show),
                       show ? "Shown again. Click Publish Website to update the live site." : "Hidden. Click Publish Website to update the live site.");
    }

    async void btnDeleteWebProject_Click(object sender, EventArgs e)
    {
        if (showing is not { } s) return;
        if (MessageBox.Show(this, $"Remove {s.ProjectName} and its {s.Photos} photo(s) from the website?\n\nThe office project itself stays.",
                "Delete project", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        try
        {
            var error = await WebProjectService.DeleteAsync(s.WebProjectId);
            if (error != null) { lblWebProjectMsg.Text = error; return; }
            showing = null;
            await LoadAsync();
            lblWebProjectMsg.Text = "Removed from the website.";
        }
        catch (Exception ex) { lblWebProjectMsg.Text = "Can't reach the database. " + ex.Message; }
    }

    void txtProjectDesc_TextChanged(object sender, EventArgs e)
    {
        if (showing != null) lblWebProjectMsg.Text = HasUnsavedChanges ? EditorUi.Unsaved : "";
    }

    async void btnSaveWebProject_Click(object sender, EventArgs e)
    {
        if (showing is not { } s || original == null) return;   // locked when nothing is selected
        var input = ReadForm();
        var answer = RecordForm.ConfirmSave(this, s.ProjectName, original, input);
        if (answer == SaveAnswer.NothingChanged) { lblWebProjectMsg.Text = "Nothing changed."; return; }
        if (answer == SaveAnswer.No) { lblWebProjectMsg.Text = "Not saved. Click Discard Changes to undo your changes."; return; }
        btnSaveWebProject.Enabled = false;
        try { await RunAsync(() => WebProjectService.UpdateAsync(s.WebProjectId, input.Description), "Changes saved successfully."); }
        finally { btnSaveWebProject.Enabled = showing != null; }
    }

    async void btnDiscardWebProject_Click(object sender, EventArgs e)
    {
        if (showing is { } s) { showing = null; await LoadAsync(s.WebProjectId); }
    }

    // ----- photos -----

    async Task LoadPhotosAsync(int? selectPhotoId = null)
    {
        if (showing is not { } s) return;
        var photos = await WebProjectService.PhotosAsync(s.WebProjectId);
        fillingPhotos = true;
        dgvPhotos.DataSource = photos;
        dgvPhotos.ClearSelection();
        if (selectPhotoId is { } id) Responsive.SelectRow(dgvPhotos, (PhotoRow p) => p.PhotoId == id);
        fillingPhotos = false;
        lblPhotos.Text = $"Project photos: {photos.Count} of {WebProjectService.MaxPhotos} (JPEG or PNG, 8 MB each). Showcase = shown in the website carousel.";
        ShowPhoto();
    }

    async void ShowPhoto()
    {
        var p = SelectedPhoto;
        btnRemovePhoto.Enabled = btnPhotoUp.Enabled = btnPhotoDown.Enabled = btnToggleShowcase.Enabled = p != null;
        btnToggleShowcase.Text = p?.Showcase == "Yes" ? "Remove from Showcase" : "Add to Showcase";
        EditorUi.ShowPicture(picPhotoPreview, p == null ? null : await WebsiteImages.LoadAsync(p.FilePath));
    }

    void dgvPhotos_SelectionChanged(object sender, EventArgs e) { if (!fillingPhotos) ShowPhoto(); }

    async void btnAddPhotos_Click(object sender, EventArgs e)
    {
        if (showing is not { } s) return;
        using var dlg = new OpenFileDialog { Filter = WebsiteImages.Filter, Multiselect = true, Title = "Choose photos" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        var files = dlg.FileNames;
        prgUpload.Maximum = files.Length;
        prgUpload.Value = 0;
        prgUpload.Visible = true;
        btnAddPhotos.Enabled = false;
        try
        {
            var progress = new Progress<int>(n =>
            {
                prgUpload.Value = Math.Min(n, prgUpload.Maximum);
                lblUploadStatus.Text = $"Uploading project photos: {n} of {files.Length}...";
            });
            var (added, problems) = await WebProjectService.AddPhotosAsync(s.WebProjectId, files, progress);
            lblUploadStatus.Text = $"{added} of {files.Length} photos uploaded." +
                                   (problems.Count == 0 ? "" : "\nNot uploaded:\n" + string.Join("\n", problems));
            await LoadPhotosAsync();
            await RefreshPhotoCountAsync();
        }
        catch (Exception ex) { lblUploadStatus.Text = "Upload failed. Please retry. " + ex.Message; }
        finally { prgUpload.Visible = false; btnAddPhotos.Enabled = true; }
    }

    /// <summary>The list's Photos column after adding or removing photos (keeps the form as it is).</summary>
    async Task RefreshPhotoCountAsync()
    {
        if (showing is not { } s) return;
        var rows = await WebProjectService.ListAsync();
        if (rows.FirstOrDefault(r => r.WebProjectId == s.WebProjectId) is not { } fresh) return;
        showing = fresh with { ShortDescription = s.ShortDescription };
        filling = true;
        dgvWebProjects.DataSource = rows;
        dgvWebProjects.ClearSelection();
        Responsive.SelectRow(dgvWebProjects, (WebProjectRow r) => r.WebProjectId == s.WebProjectId);
        filling = false;
    }

    async Task PhotoActionAsync(Func<PhotoRow, Task<string?>> action, bool keepSelected = true)
    {
        if (SelectedPhoto is not { } p) return;
        try
        {
            var error = await action(p);
            if (error != null) { lblUploadStatus.Text = error; return; }
            await LoadPhotosAsync(keepSelected ? p.PhotoId : null);
            lblUploadStatus.Text = "";
        }
        catch (Exception ex) { lblUploadStatus.Text = "Can't reach the database. " + ex.Message; }
    }

    async void btnRemovePhoto_Click(object sender, EventArgs e)
    {
        if (SelectedPhoto is not { } p) return;
        if (MessageBox.Show(this, $"Remove the photo \"{p.FileName}\"?", "Remove photo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        await PhotoActionAsync(x => WebProjectService.RemovePhotoAsync(x.PhotoId), keepSelected: false);
        await RefreshPhotoCountAsync();
    }

    async void btnPhotoUp_Click(object sender, EventArgs e) => await PhotoActionAsync(p => WebProjectService.MovePhotoAsync(p.PhotoId, -1));
    async void btnPhotoDown_Click(object sender, EventArgs e) => await PhotoActionAsync(p => WebProjectService.MovePhotoAsync(p.PhotoId, +1));
    async void btnToggleShowcase_Click(object sender, EventArgs e) =>
        await PhotoActionAsync(p => WebProjectService.SetShowcaseAsync(p.PhotoId, p.Showcase != "Yes"));
}
