using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Receipts;

/// <summary>
/// Receipts > Change Orders: the Receipts Employee logs a change order with a budget impact preview; an Admin
/// approves it, then the project's revenue and target end date update everywhere. Printable form for signing.
/// Plain layout for now; control names match the UI Elements Guide (Part 5: Tab: Change Orders).
/// </summary>
public partial class ChangeOrdersTab : UserControl
{
    int? editingId;
    string? newDoc, storedDoc;
    bool filling;
    readonly LatestOnly loads = new(), previews = new();

    public ChangeOrdersTab()
    {
        InitializeComponent();
        dgvChangeOrders.AutoGenerateColumns = false;
        colCost.DefaultCellStyle.FormatProvider = ProjectService.Peso;
        bool admin = Session.CurrentUser!.IsAdmin;
        btnApproveCo.Visible = btnRejectCo.Visible = admin;
        lblAdminNote.Visible = !admin;
        Responsive.FitGrid(this, dgvChangeOrders);
    }

    async void ChangeOrdersTab_Load(object sender, EventArgs e)
    {
        await ReloadProjectsAsync();
        await StartNewAsync();
    }

    public async Task ReloadProjectsAsync()
    {
        var projects = await MaterialService.ProjectOptionsAsync();
        filling = true;
        var keepFilter = (cboCoProjectFilter.SelectedItem as ProjectOption)?.ProjectId;
        cboCoProjectFilter.Items.Clear();
        cboCoProjectFilter.Items.AddRange(["All", .. projects]);
        cboCoProjectFilter.SelectedItem = projects.FirstOrDefault(p => p.ProjectId == keepFilter);
        if (cboCoProjectFilter.SelectedIndex < 0) cboCoProjectFilter.SelectedIndex = 0;
        var keep = (cboCoProject.SelectedItem as ProjectOption)?.ProjectId;
        cboCoProject.Items.Clear();
        cboCoProject.Items.AddRange([.. projects]);
        cboCoProject.SelectedItem = projects.FirstOrDefault(p => p.ProjectId == keep);
        filling = false;
        await LoadAsync(editingId);
    }

    async Task LoadAsync(int? selectId = null)
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var rows = await ChangeOrderService.ListAsync((cboCoProjectFilter.SelectedItem as ProjectOption)?.ProjectId);
        if (!loads.IsLatest(ticket)) return;
        filling = true;
        dgvChangeOrders.DataSource = rows;
        dgvChangeOrders.ClearSelection();
        if (selectId is { } id)
            foreach (DataGridViewRow r in dgvChangeOrders.Rows)
                if (((CoRow)r.DataBoundItem!).ChangeOrderId == id) { r.Selected = true; dgvChangeOrders.CurrentCell = r.Cells[0]; }
        filling = false;
        UpdateButtons();
    }

    CoRow? Selected => dgvChangeOrders.SelectedRows.Count > 0 ? dgvChangeOrders.SelectedRows[0].DataBoundItem as CoRow : null;

    void UpdateButtons()
    {
        btnApproveCo.Enabled = btnRejectCo.Enabled = Selected is { Status: ApprovalStatuses.Waiting };
        btnPrintCo.Enabled = Selected != null;
    }

    async void Filter_Changed(object sender, EventArgs e) { if (!filling) await LoadAsync(); }

    async void dgvChangeOrders_SelectionChanged(object sender, EventArgs e)
    {
        UpdateButtons();
        if (filling || Selected is not { } row || row.ChangeOrderId == editingId) return;
        if (await ChangeOrderService.GetAsync(row.ChangeOrderId) is { } c) await StartEditAsync(c);
    }

    async Task StartNewAsync()
    {
        editingId = null;
        newDoc = storedDoc = null;
        lblCoNumber.Text = $"{await ChangeOrderService.NextNumberAsync()} (new)";
        cboCoProject.SelectedIndex = -1;
        dtpCoDate.Value = DateTime.Today;
        txtCoDescription.Clear();
        numCoCostChange.Value = 0;
        numCoDaysChange.Value = 0;
        lblCoDocFile.Text = "(no file)";
        lblCoMsg.Text = "";
        SetEditable(true);
        Impact_Changed(this, EventArgs.Empty);
    }

    async Task StartEditAsync(ChangeOrder c)
    {
        editingId = c.ChangeOrderId;
        newDoc = null;
        storedDoc = c.FilePath;
        lblCoNumber.Text = $"{c.CoNo} ({c.Status})";
        var project = cboCoProject.Items.Cast<ProjectOption>().FirstOrDefault(p => p.ProjectId == c.ProjectId);
        if (project == null) { project = new ProjectOption(c.ProjectId, Selected?.Project ?? $"Project #{c.ProjectId}"); cboCoProject.Items.Add(project); }
        cboCoProject.SelectedItem = project;
        dtpCoDate.Value = c.RequestedOn.ToDateTime(TimeOnly.MinValue);
        txtCoDescription.Text = c.Description;
        numCoCostChange.Value = c.CostChange;
        numCoDaysChange.Value = c.DaysChange;
        lblCoDocFile.Text = c.OriginalName ?? "(no file)";
        lblCoMsg.Text = c.Status == ApprovalStatuses.Waiting ? "" : $"This change order is {c.Status}; it can't be changed.";
        SetEditable(c.Status == ApprovalStatuses.Waiting);
        await ShowImpactAsync();
    }

    void SetEditable(bool editable)
    {
        foreach (Control c in new Control[] { cboCoProject, dtpCoDate, txtCoDescription, numCoCostChange, numCoDaysChange, btnAttachCoDoc, btnSubmitCo })
            c.Enabled = editable;
        btnViewCoDoc.Enabled = storedDoc != null;
    }

    async void Impact_Changed(object? sender, EventArgs e) => await ShowImpactAsync();

    async Task ShowImpactAsync()
    {
        if (cboCoProject.SelectedItem is not ProjectOption p)
        {
            lblCoOriginalPrice.Text = "Contract price now: -"; lblCoRevisedPrice.Text = "Revised contract price: -";
            lblCoOriginalEnd.Text = "Target end date now: -"; lblCoRevisedEnd.Text = "Revised end date: -";
            return;
        }
        var ticket = previews.Start();
        var status = Selected?.ChangeOrderId == editingId ? Selected?.Status : null;
        var preview = await ChangeOrderService.PreviewAsync(p.ProjectId, numCoCostChange.Value, (int)numCoDaysChange.Value);
        if (!previews.IsLatest(ticket)) return;
        if (status == ApprovalStatuses.Approved)   // already counted: show it as before -> after
            preview = new CoPreview(preview.CurrentPrice - numCoCostChange.Value, preview.CurrentPrice,
                preview.CurrentEnd?.AddDays(-(int)numCoDaysChange.Value), preview.CurrentEnd);
        static string D(DateOnly? d) => d?.ToString("MMM d, yyyy") ?? "(not set)";
        lblCoOriginalPrice.Text = $"Contract price now: {ProjectService.Money(preview.CurrentPrice)}";
        lblCoRevisedPrice.Text = $"Revised contract price: {ProjectService.Money(preview.RevisedPrice)}";
        lblCoOriginalEnd.Text = $"Target end date now: {D(preview.CurrentEnd)}";
        lblCoRevisedEnd.Text = $"Revised end date: {D(preview.RevisedEnd)}";
    }

    async void btnNewChangeOrder_Click(object sender, EventArgs e)
    {
        filling = true; dgvChangeOrders.ClearSelection(); filling = false;
        await StartNewAsync();
        cboCoProject.Focus();
    }

    async void btnCancelCo_Click(object sender, EventArgs e)
    {
        if (editingId is { } id && await ChangeOrderService.GetAsync(id) is { } c) await StartEditAsync(c); else await StartNewAsync();
    }

    void btnAttachCoDoc_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Filter = FileStore.ScanFilter, Title = "Choose the client's signed document" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        if (FileStore.Check(dlg.FileName) is { } problem) { lblCoMsg.Text = problem; return; }
        newDoc = dlg.FileName;
        lblCoDocFile.Text = Path.GetFileName(newDoc);
    }

    async void btnViewCoDoc_Click(object sender, EventArgs e)
    {
        if (storedDoc != null && await FileStore.OpenAsync(storedDoc) is { } error) lblCoMsg.Text = error;
    }

    CoInput ReadForm() => new((cboCoProject.SelectedItem as ProjectOption)?.ProjectId ?? 0, DateOnly.FromDateTime(dtpCoDate.Value),
        txtCoDescription.Text, numCoCostChange.Value, (int)numCoDaysChange.Value, newDoc);

    async void btnSubmitCo_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (ChangeOrderService.CheckInput(input) is { } problem) { lblCoMsg.Text = problem; return; }
        btnSubmitCo.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await ChangeOrderService.UpdateAsync(id, input);
                lblCoMsg.Text = error ?? "Saved. It waits for an Admin's approval.";
                if (error == null) await LoadAsync(id);
            }
            else
            {
                var (newId, error) = await ChangeOrderService.CreateAsync(input);
                lblCoMsg.Text = error ?? "Submitted. It waits for an Admin's approval.";
                if (newId is { } created && await ChangeOrderService.GetAsync(created) is { } c) { await LoadAsync(created); await StartEditAsync(c); lblCoMsg.Text = "Submitted. It waits for an Admin's approval."; }
            }
        }
        catch (Exception ex) { lblCoMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSubmitCo.Enabled = editingId == null || Selected?.Status == ApprovalStatuses.Waiting; }
    }

    async Task DecideAsync(bool approve)
    {
        if (Selected is not { } c) return;
        var error = approve ? await ChangeOrderService.ApproveAsync(c.ChangeOrderId) : await ChangeOrderService.RejectAsync(c.ChangeOrderId);
        lblCoMsg.Text = error ?? (approve ? $"{c.CoNo} approved: the project's revenue and end date are updated." : $"{c.CoNo} rejected.");
        if (error != null) return;
        editingId = null;   // reload the form with the new status
        await LoadAsync(c.ChangeOrderId);
        if (await ChangeOrderService.GetAsync(c.ChangeOrderId) is { } fresh) { var msg = lblCoMsg.Text; await StartEditAsync(fresh); lblCoMsg.Text = msg; }
    }

    async void btnApproveCo_Click(object sender, EventArgs e)
    {
        if (Selected is { } c && MessageBox.Show(this, $"Approve {c.CoNo}? The project's revenue and end date will change.", "Approve change order",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            await DecideAsync(true);
    }

    async void btnRejectCo_Click(object sender, EventArgs e)
    {
        if (Selected is { } c && MessageBox.Show(this, $"Reject {c.CoNo}?", "Reject change order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            await DecideAsync(false);
    }

    async void btnPrintCo_Click(object sender, EventArgs e)
    {
        if (Selected is not { } c) return;
        using var form = new ChangeOrderPrint(await ChangeOrderService.PrintDataAsync(c.ChangeOrderId));
        using var preview = new PrintPreviewDialog { Document = form.Document, Width = 900, Height = 700 };
        preview.ShowDialog(this);
    }
}
