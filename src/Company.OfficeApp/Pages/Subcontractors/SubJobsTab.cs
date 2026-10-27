using System.ComponentModel;
using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Subcontractors;

/// <summary>
/// Subcontractors > Jobs & Progress: assign a job (subcontractor + project + scope + amount + dates), its payment
/// stages (must add up to 100%) and retention %, supporting papers, and progress updates.
/// Plain layout for now; control names match the UI Elements Guide (Part 4: Tab: Jobs & Progress).
/// </summary>
public partial class SubJobsTab : UserControl
{
    /// <summary>One editable row in the stages grid.</summary>
    public class StageLine
    {
        public string Name { get; set; } = "";
        public decimal Percent { get; set; }
        public decimal Amount { get; private set; }
        internal void SetAmount(decimal amount) => Amount = amount;
    }

    int? editingId;
    bool locked;
    string? progressFile;
    readonly BindingList<StageLine> stages = [];
    readonly LatestOnly loads = new();

    /// <summary>Raised after a job is added, changed or deleted, so the Billing tab can refresh its job list.</summary>
    public event EventHandler? Changed;

    public SubJobsTab()
    {
        InitializeComponent();
        dgvJobs.AutoGenerateColumns = false;
        dgvJobStages.AutoGenerateColumns = false;
        dgvJobDocs.AutoGenerateColumns = false;
        dgvProgressHistory.AutoGenerateColumns = false;
        colAmount.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        colStageAmount.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        dgvJobStages.DataSource = stages;
        Responsive.FitGrid(this, dgvJobs);
        Responsive.FitGrid(this, dgvProgressHistory);
    }

    async void SubJobsTab_Load(object sender, EventArgs e)
    {
        await ReloadListsAsync();
        StartNew();
    }

    /// <summary>Refills the subcontractor and project dropdowns (after the Directory changes) and the job list.</summary>
    public async Task ReloadListsAsync()
    {
        var subs = await SubcontractorService.ListAsync();
        var projects = await MaterialService.ProjectOptionsAsync();
        var subFilter = (cboJobSubFilter.SelectedItem as SubRow)?.SubcontractorId;
        var projectFilter = (cboJobProjectFilter.SelectedItem as ProjectOption)?.ProjectId;

        Fill(cboJobSubFilter, ["All", .. subs], subFilter == null ? null : subs.FirstOrDefault(s => s.SubcontractorId == subFilter));
        Fill(cboJobProjectFilter, ["All", .. projects], projectFilter == null ? null : projects.FirstOrDefault(p => p.ProjectId == projectFilter));
        var sub = (cboJobSubcontractor.SelectedItem as SubRow)?.SubcontractorId;
        var project = (cboJobProject.SelectedItem as ProjectOption)?.ProjectId;
        Fill(cboJobSubcontractor, [.. subs], subs.FirstOrDefault(s => s.SubcontractorId == sub));
        Fill(cboJobProject, [.. projects], projects.FirstOrDefault(p => p.ProjectId == project));
        await LoadAsync(editingId);
    }

    bool filling;

    void Fill(ComboBox box, object[] items, object? select)
    {
        filling = true;
        box.Items.Clear();
        box.Items.AddRange(items);
        if (select != null) box.SelectedItem = select;
        else if (box.Items.Count > 0 && items[0] is string) box.SelectedIndex = 0;
        filling = false;
    }

    async Task LoadAsync(int? selectId = null)
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await SubJobService.ListAsync((cboJobProjectFilter.SelectedItem as ProjectOption)?.ProjectId,
                                                 (cboJobSubFilter.SelectedItem as SubRow)?.SubcontractorId);
        if (!loads.IsLatest(ticket)) return;
        dgvJobs.DataSource = rows;
        dgvJobs.ClearSelection();
        if (selectId is { } id)
            Responsive.SelectRow(dgvJobs, (JobRow row) => row.JobId == id);
    }

    JobRow? Selected => dgvJobs.SelectedRows.Count > 0 ? dgvJobs.SelectedRows[0].DataBoundItem as JobRow : null;

    async void Filter_Changed(object sender, EventArgs e) { if (!filling) await LoadAsync(); }

    async void dgvJobs_SelectionChanged(object sender, EventArgs e)
    {
        btnDeleteJob.Enabled = Selected != null;
        if (Selected is { } row && row.JobId != editingId && await SubJobService.GetAsync(row.JobId) is { } found)
            await StartEditAsync(found.Job, found.Stages, found.Locked);
    }

    void StartNew()
    {
        editingId = null;
        locked = false;
        lblFormMode.Text = "New job";
        cboJobSubcontractor.SelectedIndex = -1;
        cboJobProject.SelectedIndex = -1;
        txtJobScope.Clear();
        numJobAmount.Value = 0;
        numRetentionPct.Value = 10;
        dtpJobStart.Value = DateTime.Today;
        dtpJobEnd.Value = DateTime.Today.AddMonths(1);
        stages.Clear();
        foreach (var s in SubJobService.DefaultStages) stages.Add(new StageLine { Name = s.Name, Percent = s.Percent });
        lblJobMsg.Text = "";
        SetLocked(false);
        ShowJobExtras(false);
        Stages_Changed(this, EventArgs.Empty);
    }

    async Task StartEditAsync(SubJob j, List<SubJobStage> jobStages, bool isLocked)
    {
        editingId = j.JobId;
        lblFormMode.Text = $"Editing job #{j.JobId}";
        cboJobSubcontractor.SelectedItem = cboJobSubcontractor.Items.Cast<SubRow>().FirstOrDefault(s => s.SubcontractorId == j.SubcontractorId);
        var project = cboJobProject.Items.Cast<ProjectOption>().FirstOrDefault(p => p.ProjectId == j.ProjectId);
        if (project == null)   // e.g. the project was cancelled later: still show it
        {
            project = new ProjectOption(j.ProjectId, (Selected?.Project) ?? $"Project #{j.ProjectId}");
            cboJobProject.Items.Add(project);
        }
        cboJobProject.SelectedItem = project;
        txtJobScope.Text = j.Scope;
        numJobAmount.Value = j.ContractAmount;
        numRetentionPct.Value = j.RetentionPct;
        dtpJobStart.Value = j.StartDate.ToDateTime(TimeOnly.MinValue);
        dtpJobEnd.Value = j.EndDate.ToDateTime(TimeOnly.MinValue);
        stages.Clear();
        foreach (var s in jobStages) stages.Add(new StageLine { Name = s.Name, Percent = s.Percent });
        lblJobMsg.Text = isLocked ? "Billing has started: only the scope and dates can change." : "";
        SetLocked(isLocked);
        Stages_Changed(this, EventArgs.Empty);
        ShowJobExtras(true);
        await LoadDocsAndProgressAsync();
    }

    void SetLocked(bool value)
    {
        locked = value;
        foreach (Control c in new Control[] { cboJobSubcontractor, cboJobProject, numJobAmount, numRetentionPct, btnAddStage, btnRemoveStage })
            c.Enabled = !value;
        dgvJobStages.ReadOnly = value;
    }

    void ShowJobExtras(bool hasJob)
    {
        foreach (Control c in new Control[] { btnUploadJobDoc, btnViewJobDoc, numProgressPct, dtpProgressDate, txtProgressNotes, btnAttachProgressFile, btnAddProgressUpdate })
            c.Enabled = hasJob;
        if (!hasJob)
        {
            dgvJobDocs.DataSource = null;
            dgvProgressHistory.DataSource = null;
            prgJobProgress.Value = 0;
            lblProgressPct.Text = "";
        }
        ClearProgressForm();
    }

    void ClearProgressForm()
    {
        numProgressPct.Value = 0;
        dtpProgressDate.Value = DateTime.Today;
        txtProgressNotes.Clear();
        progressFile = null;
        lblProgressFile.Text = "";
        lblProgressMsg.Text = "";
    }

    async Task LoadDocsAndProgressAsync()
    {
        if (editingId is not { } id) return;
        dgvJobDocs.DataSource = await SubJobService.ListDocumentsAsync(id);
        var progress = await SubJobService.ListProgressAsync(id);
        dgvProgressHistory.DataSource = progress;
        var latest = progress.FirstOrDefault()?.Percent ?? 0;
        prgJobProgress.Value = (int)Math.Round(latest);
        lblProgressPct.Text = $"{latest:0.##}% done";
        numProgressPct.Value = latest;
    }

    // ----- stages -----

    void Stages_Changed(object? sender, EventArgs e)
    {
        var amounts = SubBillingService.StageAmounts(numJobAmount.Value, stages.Select(s => s.Percent).ToList());
        for (int i = 0; i < stages.Count; i++) stages[i].SetAmount(amounts[i]);
        stages.ResetBindings();
        var total = stages.Sum(s => s.Percent);
        lblStagesTotal.Text = $"Total: {total:0.##}%";
        lblStagesTotal.ForeColor = total == 100 ? SystemColors.ControlText : Color.Red;   // a warning, not styling
    }

    void dgvJobStages_CellEndEdit(object sender, DataGridViewCellEventArgs e) => BeginInvoke(() => Stages_Changed(this, EventArgs.Empty));

    void dgvJobStages_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        lblJobMsg.Text = "Type the % as a number, e.g. 30.";
        e.Cancel = true;
    }

    void btnAddStage_Click(object sender, EventArgs e)
    {
        var left = 100 - stages.Sum(s => s.Percent);
        stages.Add(new StageLine { Name = $"Stage {stages.Count + 1}", Percent = left > 0 ? left : 0 });
        Stages_Changed(this, EventArgs.Empty);
    }

    void btnRemoveStage_Click(object sender, EventArgs e)
    {
        if (dgvJobStages.CurrentRow is not { Index: >= 0 } row || row.Index >= stages.Count) return;
        stages.RemoveAt(row.Index);
        Stages_Changed(this, EventArgs.Empty);
    }

    // ----- save -----

    JobInput ReadForm() => new((cboJobSubcontractor.SelectedItem as SubRow)?.SubcontractorId ?? 0,
        (cboJobProject.SelectedItem as ProjectOption)?.ProjectId ?? 0, txtJobScope.Text, numJobAmount.Value,
        DateOnly.FromDateTime(dtpJobStart.Value), DateOnly.FromDateTime(dtpJobEnd.Value), numRetentionPct.Value,
        stages.Select(s => new StageInput(s.Name, s.Percent)).ToList());

    void btnNewJob_Click(object sender, EventArgs e) { dgvJobs.ClearSelection(); StartNew(); cboJobSubcontractor.Focus(); }

    async void btnCancelJob_Click(object sender, EventArgs e)
    {
        if (editingId is { } id && await SubJobService.GetAsync(id) is { } found) await StartEditAsync(found.Job, found.Stages, found.Locked);
        else StartNew();
    }

    async void btnSaveJob_Click(object sender, EventArgs e)
    {
        dgvJobStages.EndEdit();
        Stages_Changed(this, EventArgs.Empty);
        var input = ReadForm();
        if (SubJobService.CheckInput(input) is { } problem) { lblJobMsg.Text = problem; return; }
        btnSaveJob.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await SubJobService.UpdateAsync(id, input);
                lblJobMsg.Text = error ?? "Saved.";
                if (error == null) { await LoadAsync(id); Changed?.Invoke(this, EventArgs.Empty); }
            }
            else
            {
                var (newId, error) = await SubJobService.CreateAsync(input);
                lblJobMsg.Text = error ?? "Job added.";
                if (newId != null) { await LoadAsync(newId); Changed?.Invoke(this, EventArgs.Empty); }
            }
        }
        catch (Exception ex) { lblJobMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveJob.Enabled = true; }
    }

    async void btnDeleteJob_Click(object sender, EventArgs e)
    {
        if (Selected is not { } j) return;
        if (MessageBox.Show(this, $"Delete job #{j.JobId} ({j.Subcontractor} - {j.Project})?", "Delete job", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        var error = await SubJobService.DeleteAsync(j.JobId);
        if (error != null) { MessageBox.Show(this, error, "Not deleted", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        StartNew();
        await LoadAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    // ----- papers -----

    async void btnUploadJobDoc_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id) return;
        using var dlg = new OpenFileDialog { Filter = FileStore.ScanFilter, Title = "Choose the contract or scope paper" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        lblJobMsg.Text = await SubJobService.UploadDocumentAsync(id, dlg.FileName) ?? "Paper uploaded.";
        await LoadDocsAndProgressAsync();
    }

    async void btnViewJobDoc_Click(object sender, EventArgs e)
    {
        if (dgvJobDocs.SelectedRows.Count == 0 || dgvJobDocs.SelectedRows[0].DataBoundItem is not JobDocRow d) return;
        if (await FileStore.OpenAsync(d.FilePath) is { } error) lblJobMsg.Text = error;
    }

    // ----- progress -----

    void btnAttachProgressFile_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Filter = FileStore.ScanFilter, Title = "Choose a photo or document" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        if (FileStore.Check(dlg.FileName) is { } problem) { lblProgressMsg.Text = problem; return; }
        progressFile = dlg.FileName;
        lblProgressFile.Text = Path.GetFileName(progressFile);
    }

    async void btnAddProgressUpdate_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id) return;
        btnAddProgressUpdate.Enabled = false;
        try
        {
            var error = await SubJobService.AddProgressAsync(id, numProgressPct.Value, DateOnly.FromDateTime(dtpProgressDate.Value), txtProgressNotes.Text, progressFile);
            if (error != null) { lblProgressMsg.Text = error; return; }
            ClearProgressForm();
            lblProgressMsg.Text = "Progress saved.";
            await LoadDocsAndProgressAsync();
            await LoadAsync(id);
            Changed?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex) { lblProgressMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnAddProgressUpdate.Enabled = true; }
    }

    // Double-click a progress update to open its photo or document.
    async void dgvProgressHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || dgvProgressHistory.Rows[e.RowIndex].DataBoundItem is not ProgressRow { FilePath: { } path }) return;
        if (await FileStore.OpenAsync(path) is { } error) lblProgressMsg.Text = error;
    }
}
