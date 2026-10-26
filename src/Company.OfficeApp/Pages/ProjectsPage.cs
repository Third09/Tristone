using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages;

/// <summary>
/// Projects (Admin only): the one shared project list.
/// Plain layout for now; control names match the UI Elements Guide (Part 1b: Projects Page).
/// </summary>
public partial class ProjectsPage : UserControl
{
    int? editingProjectId;   // null = adding a new project

    public ProjectsPage()
    {
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
        StartNew();
    }

    async void ProjectsPage_Load(object sender, EventArgs e) => await LoadProjectsAsync();

    async Task LoadProjectsAsync(int? selectProjectId = null)
    {
        string? status = cboStatusFilter.SelectedIndex > 0 ? (string)cboStatusFilter.SelectedItem! : null;
        string? sector = cboSectorFilter.SelectedIndex > 0 ? (string)cboSectorFilter.SelectedItem! : null;
        dgvProjects.DataSource = await ProjectService.ListAsync(txtProjectSearch.Text, status, sector);

        if (selectProjectId is { } id)
            foreach (DataGridViewRow r in dgvProjects.Rows)
                if (((ProjectRow)r.DataBoundItem!).ProjectId == id) { r.Selected = true; dgvProjects.CurrentCell = r.Cells[0]; }
        btnDeleteProject.Enabled = btnPrintProjectReport.Enabled = Selected != null;
    }

    ProjectRow? Selected => dgvProjects.CurrentRow?.DataBoundItem as ProjectRow;

    async void Filter_Changed(object sender, EventArgs e) => await LoadProjectsAsync();

    async void dgvProjects_SelectionChanged(object sender, EventArgs e)
    {
        btnDeleteProject.Enabled = btnPrintProjectReport.Enabled = Selected != null;
        if (Selected is { } row && await ProjectService.GetAsync(row.ProjectId) is { } p) StartEdit(p);
    }

    // ----- form -----

    void StartNew()
    {
        editingProjectId = null;
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
        ShowSummary(null);
    }

    void StartEdit(Project p)
    {
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
        ShowSummary(p);
    }

    void ShowSummary(Project? p)
    {
        if (p == null)
        {
            lblDaysSpent.Text = "Days: -";
            lblRevenue.Text = "Revenue: -";
            lblExpensesToDate.Text = "Expenses to date: -";
            lblProfitToDate.Text = "Profit to date: -";
            return;
        }
        var s = ProjectService.Summarize(p, DateOnly.FromDateTime(DateTime.Today));
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

    void btnAddProject_Click(object sender, EventArgs e)
    {
        dgvProjects.ClearSelection();
        StartNew();
        txtProjectName.Focus();
    }

    async void btnCancelProject_Click(object sender, EventArgs e)
    {
        if (editingProjectId is { } id && await ProjectService.GetAsync(id) is { } p) StartEdit(p); else StartNew();
    }

    async void btnSaveProject_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (ProjectService.CheckInput(input) is { } problem) { lblProjectFormMsg.Text = problem; return; }

        btnSaveProject.Enabled = false;
        try
        {
            if (editingProjectId is { } id)
            {
                var error = await ProjectService.UpdateAsync(id, input);
                lblProjectFormMsg.Text = error ?? "Saved.";
                if (error == null) await LoadProjectsAsync(id);
            }
            else
            {
                var (newId, error) = await ProjectService.CreateAsync(input);
                lblProjectFormMsg.Text = error ?? "Project added.";
                if (newId != null) await LoadProjectsAsync(newId);
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
        StartNew();
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
        using var report = new ProjectReport(p, ProjectService.Summarize(p, DateOnly.FromDateTime(DateTime.Today)));
        ppdProjectReport.Document = report.Document;
        ppdProjectReport.ShowDialog(this);
    }
}
