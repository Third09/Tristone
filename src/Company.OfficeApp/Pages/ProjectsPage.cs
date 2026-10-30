using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages;

/// <summary>
/// Projects (Admin only): the one shared project list.
/// Plain layout for now; control names match the UI Elements Guide (Part 1b: Projects Page).
/// </summary>
public partial class ProjectsPage : UserControl
{
    int? editingProjectId;   // null = nothing selected (or the "add only" pop-up)
    ProjectInput? original;  // the form as loaded, to list what changed before saving
    bool filling;            // true while the list reloads, so the grid's own selection changes are ignored
    readonly bool addOnly;   // this copy is the Add Project pop-up (RecordForm)
    readonly LatestOnly loads = new();

    /// <summary>The Add Project pop-up reports the new project's id here.</summary>
    public event Action<int>? Added;

    public ProjectsPage() : this(false) { }

    public ProjectsPage(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        dgvProjects.AutoGenerateColumns = false;
        colContractPrice.DefaultCellStyle.FormatProvider = ProjectService.Peso;

        cboStatusFilter.Items.Add("All");
        cboStatusFilter.Items.AddRange(ProjectStatuses.All);
        cboStatusFilter.SelectedIndex = 0;
        cboSectorFilter.Items.Add("All");
        cboSectorFilter.Items.AddRange(ProjectSectors.All);
        cboSectorFilter.SelectedIndex = 0;
        cboSector.Items.AddRange(ProjectSectors.All);
        cboProjectStatus.Items.AddRange(ProjectStatuses.All);
        Disposed += (_, _) => ppdProjectReport.Dispose();   // a dialog, not owned by the page's controls
        if (addOnly)
        {
            RecordForm.ShowFormOnly(this, lblFormMode, lblDaysSpent, lblRevenue, lblExpensesToDate, lblProfitToDate, btnPrintProjectReport);
            StartNew();
            return;
        }
        Responsive.FitGrid(this, dgvProjects);
        Responsive.OnTypingPause(txtProjectSearch, () => LoadProjectsAsync());
        ShowNothingSelected();
    }

    async void ProjectsPage_Load(object sender, EventArgs e) { if (!addOnly) await LoadProjectsAsync(); }

    async Task LoadProjectsAsync(int? selectProjectId = null)
    {
        if (addOnly) return;   // the Add pop-up has no list (its hidden filters still fire on start-up)
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        string? status = cboStatusFilter.SelectedIndex > 0 ? (string)cboStatusFilter.SelectedItem! : null;
        string? sector = cboSectorFilter.SelectedIndex > 0 ? (string)cboSectorFilter.SelectedItem! : null;
        var rows = await ProjectService.ListAsync(txtProjectSearch.Text, status, sector);
        if (!loads.IsLatest(ticket)) return;   // a newer search already started

        selectProjectId ??= editingProjectId;   // searching keeps the project being edited selected
        filling = true;
        dgvProjects.DataSource = rows;
        dgvProjects.ClearSelection();           // never pick the first row by itself (RecordForm)
        if (selectProjectId is { } id)
            Responsive.SelectRow(dgvProjects, (ProjectRow row) => row.ProjectId == id);
        filling = false;

        if (Selected is not { } selected) ShowNothingSelected();
        else if (selected.ProjectId != editingProjectId && await ProjectService.GetAsync(selected.ProjectId) is { } p) StartEdit(p);
        btnDeleteProject.Enabled = btnPrintProjectReport.Enabled = Selected != null;
    }

    // SelectedRows, not CurrentRow: after ClearSelection (Add Project) the grid still has a current row.
    ProjectRow? Selected => dgvProjects.SelectedRows.Count > 0 ? dgvProjects.SelectedRows[0].DataBoundItem as ProjectRow : null;

    async void Filter_Changed(object sender, EventArgs e) => await LoadProjectsAsync();

    async void dgvProjects_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        btnDeleteProject.Enabled = btnPrintProjectReport.Enabled = Selected != null;
        if (Selected is not { } row) ShowNothingSelected();
        else if (await ProjectService.GetAsync(row.ProjectId) is { } p) StartEdit(p);
    }

    // ----- form -----

    void ShowNothingSelected()
    {
        StartNew();
        lblFormMode.Text = "Click a project in the list to see or change it. To add one, click Add Project.";
        RecordForm.SetEnabled(this, lblFormMode, false);
    }

    void StartNew()
    {
        editingProjectId = null;
        original = null;
        lblFormMode.Text = "New project";
        txtProjectName.Clear();
        txtProjectSite.Clear();
        txtClientName.Clear();
        cboSector.SelectedIndex = -1;
        numContractPrice.Value = 0;
        dtpStartDate.Value = DateTime.Today;
        dtpTargetEndDate.Value = DateTime.Today;
        dtpTargetEndDate.Checked = false;
        cboProjectStatus.SelectedItem = ProjectStatuses.Pending;
        chkShowOnWebsite.Checked = false;
        lblProjectFormMsg.Text = "";
        _ = ShowSummaryAsync(null);
    }

    void StartEdit(Project p)
    {
        RecordForm.SetEnabled(this, lblFormMode, true);
        editingProjectId = p.ProjectId;
        lblFormMode.Text = $"Editing: {p.ProjectName}";
        txtProjectName.Text = p.ProjectName;
        txtProjectSite.Text = p.SiteLocation;
        txtClientName.Text = p.ClientName ?? "";
        cboSector.SelectedItem = p.Sector;
        numContractPrice.Value = p.ContractPrice;
        dtpStartDate.Value = p.StartDate.ToDateTime(TimeOnly.MinValue);
        dtpTargetEndDate.Value = (p.TargetEndDate ?? p.StartDate).ToDateTime(TimeOnly.MinValue);
        dtpTargetEndDate.Checked = p.TargetEndDate != null;
        cboProjectStatus.SelectedItem = p.Status;
        chkShowOnWebsite.Checked = p.ShowOnWebsite;
        lblProjectFormMsg.Text = "";
        original = ReadForm();
        _ = ShowSummaryAsync(p);
    }

    async Task ShowSummaryAsync(Project? p)
    {
        if (p == null)
        {
            lblDaysSpent.Text = "Days: -";
            lblRevenue.Text = "Revenue: -";
            lblExpensesToDate.Text = "Expenses to date: -";
            lblProfitToDate.Text = "Profit to date: -";
            return;
        }
        var s = await ProjectService.SummarizeAsync(p, DateOnly.FromDateTime(DateTime.Today));
        if (editingProjectId != p.ProjectId) return;   // another project was picked meanwhile
        lblDaysSpent.Text = $"Days: {s.DaysText}";
        lblRevenue.Text = $"Revenue: {ProjectService.Money(s.Revenue)}";
        lblExpensesToDate.Text = $"Expenses to date: {ProjectService.Money(s.Expenses)}";
        lblProfitToDate.Text = $"Profit to date: {ProjectService.Money(s.Profit)}";
    }

    ProjectInput ReadForm() => new(
        txtProjectName.Text,
        txtProjectSite.Text,
        txtClientName.Text,
        (string?)cboSector.SelectedItem ?? "",
        numContractPrice.Value,
        DateOnly.FromDateTime(dtpStartDate.Value),
        dtpTargetEndDate.Checked ? DateOnly.FromDateTime(dtpTargetEndDate.Value) : null,
        (string?)cboProjectStatus.SelectedItem ?? "",
        chkShowOnWebsite.Checked);

    async void btnAddProject_Click(object sender, EventArgs e)
    {
        var page = new ProjectsPage(addOnly: true);
        if (RecordForm.AddDialog(this, "Add Project", page, a => page.Added += a) is not { } newId) return;
        editingProjectId = null;
        await LoadProjectsAsync(newId);
        lblProjectFormMsg.Text = "Project added.";
    }

    async void btnCancelProject_Click(object sender, EventArgs e)
    {
        if (addOnly) { FindForm()?.Close(); return; }
        if (editingProjectId is { } id && await ProjectService.GetAsync(id) is { } p) StartEdit(p);
    }

    async void btnSaveProject_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (ProjectService.CheckInput(input) is { } problem) { lblProjectFormMsg.Text = problem; return; }
        if (!addOnly && editingProjectId == null) return;   // the form is locked when nothing is selected

        if (editingProjectId != null && original != null)
        {
            var answer = RecordForm.ConfirmSave(this, original.ProjectName, original, input);
            if (answer == SaveAnswer.NothingChanged) { lblProjectFormMsg.Text = "Nothing changed."; return; }
            if (answer == SaveAnswer.No) { lblProjectFormMsg.Text = "Not saved. Click Cancel to undo your changes."; return; }
        }

        btnSaveProject.Enabled = false;
        try
        {
            if (editingProjectId is { } id)
            {
                var error = await ProjectService.UpdateAsync(id, input);
                if (error != null) { lblProjectFormMsg.Text = error; return; }
                if (await ProjectService.GetAsync(id) is { } fresh) StartEdit(fresh);
                await LoadProjectsAsync(id);
                lblProjectFormMsg.Text = "Saved.";
            }
            else
            {
                var (newId, error) = await ProjectService.CreateAsync(input);
                if (newId is { } added) Added?.Invoke(added); else lblProjectFormMsg.Text = error;
            }
        }
        catch (Exception ex) { lblProjectFormMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveProject.Enabled = true; }
    }

    // ----- list actions -----

    async void btnDeleteProject_Click(object sender, EventArgs e)
    {
        if (Selected is not { } row) return;
        var ok = MessageBox.Show(this, $"Delete the project \"{row.ProjectName}\"? This can't be undone.",
            "Delete project", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (ok != DialogResult.Yes) return;

        var error = await ProjectService.DeleteAsync(row.ProjectId);
        if (error != null) { MessageBox.Show(this, error, "Not deleted", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        editingProjectId = null;
        await LoadProjectsAsync();
    }

    void btnExportProjects_Click(object sender, EventArgs e)
    {
        if (dgvProjects.DataSource is not List<ProjectRow> rows) return;
        if (ExcelExport.AskPath(this, "Projects") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Projects", rows,
                new("Project Name", r => r.ProjectName),
                new("Site / Location", r => r.SiteLocation),
                new("Client", r => r.ClientName),
                new("Sector", r => r.Sector),
                new("Contract Price", r => r.ContractPrice, "#,##0.00"),
                new("Start", r => r.StartDate, "mmm d, yyyy"),
                new("Target End", r => r.TargetEndDate, "mmm d, yyyy"),
                new("Status", r => r.Status),
                new("On Website", r => r.ShowOnWebsite));
            MessageBox.Show(this, $"Saved {rows.Count} projects to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }

    async void btnPrintProjectReport_Click(object sender, EventArgs e)
    {
        if (Selected is not { } row || await ProjectService.GetAsync(row.ProjectId) is not { } p) return;
        using var report = new ProjectReport(p, await ProjectService.SummarizeAsync(p, DateOnly.FromDateTime(DateTime.Today)));
        ppdProjectReport.Document = report.Document;
        ppdProjectReport.ShowDialog(this);
    }
}
