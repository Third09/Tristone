using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > Inquiries: messages from the website Contact form. Read them, copy the email to reply from Gmail,
/// and follow them up with a status (New / Contacted / Closed) and internal notes.
/// Same safe pattern as the other list pages: nothing is selected by itself, the follow-up form is locked until a
/// row is clicked, and Save lists the changes in a Yes/No message first.
/// </summary>
public partial class InquiriesTab : UserControl
{
    /// <summary>The status and notes as loaded, to list what changed before saving.</summary>
    record FollowUp(string Status, string? Notes);

    Inquiry? showing;        // null = nothing selected
    FollowUp? original;
    bool filling;            // true while the list reloads, so the grid's own selection changes are ignored
    readonly LatestOnly loads = new();
    Font? bold;              // for New inquiries, made once

    /// <summary>Raised after a status changes, so the main window can refresh the "Inquiries (N new)" tab title.</summary>
    public event EventHandler? Changed;

    public InquiriesTab()
    {
        InitializeComponent();
        dgvInquiries.AutoGenerateColumns = false;
        RecordForm.NoAutoSelect(dgvInquiries, on => filling = on, RestoreSelection);
        dgvInquiries.CellFormatting += dgvInquiries_CellFormatting;
        Disposed += (_, _) => bold?.Dispose();
        cboInquiryStatusFilter.Items.AddRange(["All", .. InquiryStatuses.All]);
        cboInquiryStatusFilter.SelectedIndex = 0;
        cboInquiryStatus.Items.AddRange(InquiryStatuses.All);
        txtInquiryNotes.MaxLength = WebsiteInquiries.MaxNotesLength;
        Responsive.FitGrid(this, dgvInquiries);
        Responsive.OnTypingPause(txtInquirySearch, () => LoadAsync());
        ShowNothingSelected();
    }

    async void InquiriesTab_Load(object sender, EventArgs e) => await LoadAsync();

    async void Filter_Changed(object? sender, EventArgs e) { if (!filling) await LoadAsync(); }

    async void btnRefreshInquiries_Click(object sender, EventArgs e) => await LoadAsync();

    async Task LoadAsync(int? selectId = null)
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var status = cboInquiryStatusFilter.SelectedIndex > 0 ? (string)cboInquiryStatusFilter.SelectedItem! : null;
        DateOnly? from = dtpInquiryFrom.Checked ? DateOnly.FromDateTime(dtpInquiryFrom.Value) : null;
        DateOnly? to = dtpInquiryTo.Checked ? DateOnly.FromDateTime(dtpInquiryTo.Value) : null;
        var rows = await WebsiteInquiries.ListAsync(status, txtInquirySearch.Text, from, to);
        var counts = await WebsiteInquiries.CountsAsync();
        if (!loads.IsLatest(ticket)) return;

        lblNewCount.Text = $"New: {counts.New}";
        lblContactedCount.Text = $"Contacted: {counts.Contacted}";
        lblClosedCount.Text = $"Closed: {counts.Closed}";

        selectId ??= showing?.InquiryId;   // filtering keeps the inquiry being read selected
        filling = true;
        dgvInquiries.DataSource = rows;
        dgvInquiries.ClearSelection();     // never pick the first row by itself
        if (selectId is { } id) Responsive.SelectRow(dgvInquiries, (InquiryRow r) => r.InquiryId == id);
        filling = false;

        if (Selected is not { } selected) ShowNothingSelected();
        else if (selected.InquiryId != showing?.InquiryId && await WebsiteInquiries.GetAsync(selected.InquiryId) is { } i) Show(i);
    }

    InquiryRow? Selected => dgvInquiries.SelectedRows.Count > 0 ? dgvInquiries.SelectedRows[0].DataBoundItem as InquiryRow : null;

    // New inquiries stand out (a function, not styling).
    void dgvInquiries_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.CellStyle != null && dgvInquiries.Rows[e.RowIndex].DataBoundItem is InquiryRow { Status: InquiryStatuses.New })
            e.CellStyle.Font = bold ??= new Font(dgvInquiries.Font, FontStyle.Bold);
    }

    async void dgvInquiries_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        if (Selected is not { } row) ShowNothingSelected();
        else if (await WebsiteInquiries.GetAsync(row.InquiryId) is { } i) Show(i);
    }

    /// <summary>After the grid picked a row by itself on first show (RecordForm.NoAutoSelect): only the record being shown stays selected.</summary>
    void RestoreSelection()
    {
        filling = true;
        dgvInquiries.ClearSelection();
        if (showing?.InquiryId is { } id) Responsive.SelectRow(dgvInquiries, (InquiryRow r) => r.InquiryId == id);
        filling = false;
        if (Selected == null) ShowNothingSelected();
        
    }

    void ShowNothingSelected()
    {
        showing = null;
        original = null;
        lblInqTitle.Text = "Click an inquiry in the list to read it and follow it up.";
        lblInqName.Text = "Name: -";
        lblInqContact.Text = "Contact number: -";
        lblInqEmail.Text = "Email: -";
        lblInqLocation.Text = "Project location: -";
        lblInqService.Text = "Service required: -";
        lblInqReceived.Text = "Received: -";
        txtInqMessage.Clear();
        cboInquiryStatus.SelectedIndex = -1;
        txtInquiryNotes.Clear();
        lblInquiryMsg.Text = "";
        RecordForm.SetEnabled(this, lblInqTitle, false);
    }

    void Show(Inquiry i)
    {
        RecordForm.SetEnabled(this, lblInqTitle, true);
        showing = i;
        lblInqTitle.Text = $"Inquiry from {i.FullName}";
        lblInqName.Text = $"Name: {i.FullName}";
        lblInqContact.Text = $"Contact number: {i.Phone ?? "-"}";
        lblInqEmail.Text = $"Email: {i.Email ?? "-"}";
        btnCopyEmail.Enabled = i.Email != null;
        lblInqLocation.Text = $"Project location: {i.ProjectLocation ?? "-"}";
        lblInqService.Text = $"Service required: {i.ServiceRequired ?? "-"}";
        lblInqReceived.Text = $"Received: {i.ReceivedAt:MMM d, yyyy h:mm tt}";
        txtInqMessage.Text = i.Message.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        cboInquiryStatus.SelectedItem = i.Status;
        txtInquiryNotes.Text = i.Notes ?? "";
        lblInquiryMsg.Text = "";
        original = ReadForm();
    }

    FollowUp ReadForm() => new((string?)cboInquiryStatus.SelectedItem ?? "", string.IsNullOrWhiteSpace(txtInquiryNotes.Text) ? null : txtInquiryNotes.Text.Trim());

    void btnCopyEmail_Click(object sender, EventArgs e)
    {
        if (showing?.Email is not { } email) return;
        Clipboard.SetText(email);
        lblInquiryMsg.Text = $"Copied {email}. Paste it in Gmail to reply.";
    }

    async void btnSaveInquiry_Click(object sender, EventArgs e)
    {
        if (showing is not { } i || original == null) return;   // locked when nothing is selected
        var input = ReadForm();
        var answer = RecordForm.ConfirmSave(this, $"the inquiry from {i.FullName}", original, input);
        if (answer == SaveAnswer.NothingChanged) { lblInquiryMsg.Text = "Nothing changed."; return; }
        if (answer == SaveAnswer.No) { lblInquiryMsg.Text = "Not saved. Click Cancel to undo your changes."; return; }

        btnSaveInquiry.Enabled = false;
        try
        {
            var error = await WebsiteInquiries.UpdateAsync(i.InquiryId, input.Status, input.Notes);
            if (error != null) { lblInquiryMsg.Text = error; return; }
            showing = null;   // reload the form from the saved inquiry
            await LoadAsync(i.InquiryId);
            lblInquiryMsg.Text = "Saved.";
            Changed?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex) { lblInquiryMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveInquiry.Enabled = showing != null; }
    }

    async void btnCancelInquiry_Click(object sender, EventArgs e)
    {
        if (showing != null && await WebsiteInquiries.GetAsync(showing.InquiryId) is { } i) Show(i);
    }

    void btnExportInquiries_Click(object sender, EventArgs e)
    {
        if (dgvInquiries.DataSource is not List<InquiryRow> rows) return;
        if (ExcelExport.AskPath(this, "Inquiries") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Inquiries", rows,
                new("Date", r => r.ReceivedAt, "mmm d, yyyy h:mm AM/PM"),
                new("Name", r => r.FullName),
                new("Email", r => r.Email),
                new("Contact Number", r => r.Phone),
                new("Service", r => r.ServiceRequired),
                new("Project Location", r => r.ProjectLocation),
                new("Status", r => r.Status));
            MessageBox.Show(this, $"Saved {rows.Count} inquiries to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
