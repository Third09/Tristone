using Company.OfficeApp.UI;
using Company.Shared.Models;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > Services (Figma "admin services"): the service cards. The name comes from the 3 fixed services,
/// the category from the office sectors; location and description are typed; one picture per card.
/// List page pattern (RecordForm): Add opens this page as a pop-up (<c>addOnly</c>), the form is locked until a row
/// is clicked, Save lists the changes.
/// </summary>
public partial class ServicesEditorPage : UserControl, IWebsiteEditor
{
    readonly bool addOnly;   // this copy is the Add New Service pop-up
    record ServiceForm(string Service, string Category, string Location, string Description, string Picture);

    int? editingId;
    WebServiceRow? showingRow;
    ServiceForm? original;
    string? storedImage, newImage;
    bool filling;
    readonly LatestOnly loads = new();

    /// <summary>Raised by the Add pop-up with the new service's id.</summary>
    public event Action<int>? Added;

    public ServicesEditorPage() : this(false) { }

    public ServicesEditorPage(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        dgvWebServices.AutoGenerateColumns = false;
        RecordForm.NoAutoSelect(dgvWebServices, on => filling = on, RestoreSelection);
        cboServiceName.Items.AddRange(WebsiteServiceNames.All);
        cboServiceSector.Items.AddRange(ProjectSectors.All);
        txtServiceLocation.MaxLength = WebServiceService.MaxLocation;
        EditorUi.Counter(txtServiceDesc, lblServiceDescCount, WebServiceService.MaxDescription);
        Disposed += (_, _) => picServiceImage.Image?.Dispose();
        if (addOnly)
        {
            RecordForm.ShowFormOnly(this, lblFormMode);
            StartNew();
            return;
        }
        Responsive.FitGrid(this, dgvWebServices);
        ShowNothingSelected();
    }

    public bool HasUnsavedChanges => editingId != null && original != null && ReadForm() != original;

    async void ServicesEditorPage_Load(object sender, EventArgs e) { if (!addOnly) await LoadAsync(); }

    WebServiceRow? Selected => dgvWebServices.SelectedRows.Count > 0 ? dgvWebServices.SelectedRows[0].DataBoundItem as WebServiceRow : null;

    async Task LoadAsync(int? selectId = null)
    {
        if (addOnly) return;
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        List<WebServiceRow> rows;
        try { rows = await WebServiceService.ListAsync(); }
        catch (Exception ex) { lblServiceMsg.Text = "Can't reach the database. " + ex.Message; return; }
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

    void ClearForm()
    {
        cboServiceName.SelectedIndex = -1;
        cboServiceSector.SelectedIndex = -1;
        txtServiceLocation.Clear();
        txtServiceDesc.Clear();
        storedImage = newImage = null;
        EditorUi.ShowPicture(picServiceImage, null);
        lblServiceImageName.Text = "";
        lblServiceMsg.Text = "";
    }

    void ShowNothingSelected()
    {
        editingId = null;
        showingRow = null;
        original = null;
        ClearForm();
        lblFormMode.Text = "Click a service in the list to edit it.";
        RecordForm.SetEnabled(this, lblFormMode, false);
        btnMoveServiceUp.Enabled = btnMoveServiceDown.Enabled = btnHideShowService.Enabled = btnDeleteWebService.Enabled = false;
    }

    void StartNew()
    {
        editingId = null;
        ClearForm();
        lblFormMode.Text = "New service";
        btnSaveWebService.Text = "Add Service";
        btnCancelWebService.Text = "Cancel";
    }

    async Task ShowAsync(WebServiceRow row)
    {
        if (await WebServiceService.GetAsync(row.WebServiceId) is not { } s) { ShowNothingSelected(); return; }
        RecordForm.SetEnabled(this, lblFormMode, true);
        btnMoveServiceUp.Enabled = btnMoveServiceDown.Enabled = btnHideShowService.Enabled = btnDeleteWebService.Enabled = true;
        editingId = s.WebServiceId;
        showingRow = row;
        lblFormMode.Text = $"Slot #{row.Slot}: {s.ServiceName}{(s.Location == null ? "" : " - " + s.Location)} ({row.Shown} on the website)";
        btnHideShowService.Text = s.IsVisible ? "Unpublish" : "Publish";
        cboServiceName.SelectedItem = s.ServiceName;
        cboServiceSector.SelectedItem = s.Sector;
        txtServiceLocation.Text = s.Location ?? "";
        txtServiceDesc.Text = s.ShortDescription ?? "";
        storedImage = s.ImagePath;
        newImage = null;
        await ShowImageAsync();
        lblServiceMsg.Text = "";
        original = ReadForm();
    }

    string PictureName => newImage != null ? Path.GetFileName(newImage) : storedImage != null ? Path.GetFileName(storedImage) : "(none)";

    ServiceForm ReadForm() => new((string?)cboServiceName.SelectedItem ?? "", (string?)cboServiceSector.SelectedItem ?? "",
                                  txtServiceLocation.Text.Trim(), txtServiceDesc.Text.Trim(), PictureName);

    async Task ShowImageAsync()
    {
        EditorUi.ShowPicture(picServiceImage, await WebsiteImages.LoadAsync(storedImage, newImage));
        lblServiceImageName.Text = PictureName == "(none)" ? "No picture yet." : PictureName;
    }

    async void btnChooseServiceImage_Click(object sender, EventArgs e)
    {
        if (EditorUi.PickPicture(this) is not { } path) return;
        newImage = path;
        await ShowImageAsync();
        if (editingId != null) lblServiceMsg.Text = EditorUi.Unsaved;
    }

    void txtServiceDesc_TextChanged(object sender, EventArgs e)
    {
        if (editingId != null && original != null) lblServiceMsg.Text = HasUnsavedChanges ? EditorUi.Unsaved : "";
    }

    async void btnAddWebService_Click(object sender, EventArgs e)
    {
        var page = new ServicesEditorPage(addOnly: true);
        if (RecordForm.AddDialog(this, "Add New Service", page, a => page.Added += a) is not { } newId) return;
        editingId = null;
        await LoadAsync(newId);
        lblServiceMsg.Text = "Service added. Click Publish Website to put it on the live site.";
    }

    async void btnSaveWebService_Click(object sender, EventArgs e)
    {
        if (!addOnly && editingId == null) return;   // the form is locked when nothing is selected
        var form = ReadForm();
        var input = new WebServiceInput(form.Service, form.Category, form.Location, form.Description);
        if (editingId != null && original != null)
        {
            var answer = RecordForm.ConfirmSave(this, original.Service, original, form);
            if (answer == SaveAnswer.NothingChanged) { lblServiceMsg.Text = "Nothing changed."; return; }
            if (answer == SaveAnswer.No) { lblServiceMsg.Text = "Not saved. Click Discard Changes to undo your changes."; return; }
        }
        btnSaveWebService.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await WebServiceService.UpdateAsync(id, input, newImage);
                if (error != null) { lblServiceMsg.Text = "Unable to save changes: " + error; return; }
                editingId = null;   // reload the form from the saved record
                await LoadAsync(id);
                lblServiceMsg.Text = "Changes saved successfully. Click Publish Website to put them on the live site.";
            }
            else
            {
                var (newId, error) = await WebServiceService.CreateAsync(input, newImage);
                if (newId is { } added) Added?.Invoke(added); else lblServiceMsg.Text = error;
            }
        }
        catch (Exception ex) { lblServiceMsg.Text = "Unable to save changes. Please try again. " + ex.Message; }
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
            if (error != null) { lblServiceMsg.Text = error; return; }
            editingId = null;
            await LoadAsync(id);
            lblServiceMsg.Text = done;
        }
        catch (Exception ex) { lblServiceMsg.Text = "Can't reach the database. " + ex.Message; }
    }

    async void btnMoveServiceUp_Click(object sender, EventArgs e) => await RunAsync(id => WebServiceService.MoveAsync(id, -1), "Moved up.");
    async void btnMoveServiceDown_Click(object sender, EventArgs e) => await RunAsync(id => WebServiceService.MoveAsync(id, +1), "Moved down.");

    async void btnHideShowService_Click(object sender, EventArgs e)
    {
        if (showingRow is not { } r) return;
        bool show = r.Shown != "Visible";
        await RunAsync(id => WebServiceService.SetVisibleAsync(id, show),
                       show ? "Shown again. Click Publish Website to update the live site." : "Hidden. Click Publish Website to update the live site.");
    }

    async void btnDeleteWebService_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id || showingRow is not { } r) return;
        if (MessageBox.Show(this, $"Delete the service card \"{r.ServiceName}{(r.Location == null ? "" : " - " + r.Location)}\" from the website?",
                "Delete service", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        try
        {
            var error = await WebServiceService.DeleteAsync(id);
            if (error != null) { lblServiceMsg.Text = error; return; }
            editingId = null;
            await LoadAsync();
            lblServiceMsg.Text = "Service deleted.";
        }
        catch (Exception ex) { lblServiceMsg.Text = "Can't reach the database. " + ex.Message; }
    }
}
