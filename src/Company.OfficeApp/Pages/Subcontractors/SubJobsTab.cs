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

    int? editingId;          // null = nothing selected (or the "add only" pop-up)
    JobInput? original;      // the form as loaded, to list what changed before saving
    readonly bool addOnly;   // this copy is the New Job pop-up (RecordForm)
    bool locked;
    string? progressFile;
    readonly BindingList<StageLine> stages = [];
    readonly LatestOnly loads = new();

    /// <summary>Raised after a job is added, changed or deleted, so the Billing tab can refresh its job list.</summary>
    public event EventHandler? Changed;

    /// <summary>The New Job pop-up reports the new job's id here.</summary>
    public event Action<int>? Added;

    public SubJobsTab() : this(false) { }

    public SubJobsTab(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        dgvJobs.AutoGenerateColumns = false;
        RecordForm.NoAutoSelect(dgvJobs, on => filling = on, RestoreSelection);
        dgvJobStages.AutoGenerateColumns = false;
        dgvJobDocs.AutoGenerateColumns = false;
        dgvProgressHistory.AutoGenerateColumns = false;
        colAmount.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        colStageAmount.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        dgvJobStages.DataSource = stages;
        if (addOnly)
        {
            // Papers and progress are added after the job exists, on the Jobs & Progress tab.
            RecordForm.ShowFormOnly(this, lblFormMode, lblJobDocs, dgvJobDocs, btnUploadJobDoc, btnViewJobDoc,
                lblProgressTitle, prgJobProgress, lblProgressPct, lblProgressPctInput, lblProgressDate, lblProgressNotes,
                numProgressPct, dtpProgressDate, txtProgressNotes, btnAttachProgressFile, btnAddProgressUpdate,
                lblProgressMsg, lblProgressFile, dgvProgressHistory);
            return;
        }
        Responsive.FitGrid(this, dgvJobs);
        Responsive.FitGrid(this, dgvProgressHistory);
    }

    async void SubJobsTab_Load(object sender, EventArgs e)
    {
        if (addOnly) StartNew(); else ShowNothingSelected();
        await ReloadListsAsync();
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
        if (!addOnly) await LoadAsync(editingId);
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
        if (addOnly) return;   // the Add pop-up has no list (its hidden filters still fire on start-up)
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await SubJobService.ListAsync((cboJobProjectFilter.SelectedItem as ProjectOption)?.ProjectId,
                                                 (cboJobSubFilter.SelectedItem as SubRow)?.SubcontractorId);
        if (!loads.IsLatest(ticket)) return;

        selectId ??= editingId;   // filtering keeps the job being edited selected
        filling = true;
        dgvJobs.DataSource = rows;
        dgvJobs.ClearSelection();   // never pick the first row by itself (RecordForm)
        if (selectId is { } id)
            Responsive.SelectRow(dgvJobs, (JobRow row) => row.JobId == id);
        filling = false;

        if (Selected is not { } selected) ShowNothingSelected();
        else if (selected.JobId != editingId && await SubJobService.GetAsync(selected.JobId) is { } found)
            await StartEditAsync(found.Job, found.Stages, found.Locked);
        btnDeleteJob.Enabled = Selected != null;
    }

    JobRow? Selected => dgvJobs.SelectedRows.Count > 0 ? dgvJobs.SelectedRows[0].DataBoundItem as JobRow : null;

    async void Filter_Changed(object sender, EventArgs e) { if (!filling) await LoadAsync(); }

    async void dgvJobs_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        btnDeleteJob.Enabled = Selected != null;
        if (Selected is not { } row) ShowNothingSelected();
        else if (row.JobId != editingId && await SubJobService.GetAsync(row.JobId) is { } found)
            await StartEditAsync(found.Job, found.Stages, found.Locked);
    }

    /// <summary>After the grid picked a row by itself on first show (RecordForm.NoAutoSelect): only the record being shown stays selected.</summary>
    void RestoreSelection()
    {
        filling = true;
        dgvJobs.ClearSelection();
        if (editingId is { } id) Responsive.SelectRow(dgvJobs, (JobRow r) => r.JobId == id);
        filling = false;
        if (Selected == null) ShowNothingSelected();
        btnDeleteJob.Enabled = Selected != null;
    }

    void ShowNothingSelected()
    {
        StartNew();
        lblFormMode.Text = "Click a job in the list to see or change it. To add one, click New Job.";
        RecordForm.SetEnabled(this, lblFormMode, false);
    }

    void StartNew()
    {
        editingId = null;
        original = null;
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
        RecordForm.SetEnabled(this, lblFormMode, true);
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
        original = ReadForm();
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

    async void btnNewJob_Click(object sender, EventArgs e)
    {
        var page = new SubJobsTab(addOnly: true);
        if (RecordForm.AddDialog(this, "New Job", page, a => page.Added += a) is not { } newId) return;
        editingId = null;
        await LoadAsync(newId);
        lblJobMsg.Text = "Job added. You can upload its papers now.";
        Changed?.Invoke(this, EventArgs.Empty);
    }

    async void btnCancelJob_Click(object sender, EventArgs e)
    {
        if (addOnly) { FindForm()?.Close(); return; }
        if (editingId is { } id && await SubJobService.GetAsync(id) is { } found) await StartEditAsync(found.Job, found.Stages, found.Locked);
    }

    async void btnSaveJob_Click(object sender, EventArgs e)
    {
        dgvJobStages.EndEdit();
        Stages_Changed(this, EventArgs.Empty);
        var input = ReadForm();
        if (SubJobService.CheckInput(input) is { } problem) { lblJobMsg.Text = problem; return; }
        if (!addOnly && editingId == null) return;   // the form is locked when nothing is selected
        if (editingId is { } jobId && original != null)
        {
            var answer = RecordForm.ConfirmSave(this, $"job #{jobId}", original, input,
                new Dictionary<string, string> { ["SubcontractorId"] = "Subcontractor", ["ProjectId"] = "Project", ["RetentionPct"] = "Retention %" });
            if (answer == SaveAnswer.NothingChanged) { lblJobMsg.Text = "Nothing changed."; return; }
            if (answer == SaveAnswer.No) { lblJobMsg.Text = "Not saved. Click Cancel to undo your changes."; return; }
        }
        btnSaveJob.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await SubJobService.UpdateAsync(id, input);
                if (error != null) { lblJobMsg.Text = error; return; }
                editingId = null;   // reload the form from the saved job
                await LoadAsync(id);
                lblJobMsg.Text = "Saved.";
                Changed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                var (newId, error) = await SubJobService.CreateAsync(input);
                if (newId is { } added) Added?.Invoke(added); else lblJobMsg.Text = error;
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
        editingId = null;
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
