using System.Globalization;
using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages;

/// <summary>
/// Quotations (owner request 2026-11-09): price offers to clients, QT-0001..., following the company's own quotation
/// paper: Customer / Address / Attention / Date, Scope of Works (A, B, C... with sizes), cost lines and Total Cost,
/// Notes, Work Duration, Terms of Payment, Cash Payable to, and who signs "Very truly yours". The Receipts Employee
/// prepares one and sends it for approval; the Receipts Supervisor approves or rejects it, and later marks it
/// Client Accepted (a project is made) or Client Declined.
/// List page pattern (RecordForm): nothing selected by itself, New opens this page as a pop-up, the form is locked
/// until a row is clicked (and while the quotation isn't a Draft), Save lists the changes.
/// </summary>
public partial class QuotationsPage : UserControl
{
    readonly bool addOnly;   // this copy is the New Quotation pop-up
    record QuotationForm(string Customer, string? Address, string? Attention, string? Contact, string? Email, string Project,
                         string? Site, string Sector, DateOnly QuoteDate, DateOnly ValidUntil, DateOnly? PlannedStart,
                         string? Notes, string? WorkDuration, string? PaymentTerms, string? PayableTo, string? PayableNumber,
                         string? SignedBy, string? Position, string? InternalNotes, string ScopeOfWorks, string Costs);

    int? editingId;
    string? editingStatus;
    QuotationForm? original;
    int? inquiryId;
    bool filling, loadingForm;
    readonly LatestOnly loads = new();
    readonly bool approver = Access.IsApprover(Session.CurrentUser!, Departments.Receipts);

    /// <summary>Raised by the New Quotation pop-up with the new quotation's id.</summary>
    public event Action<int>? Added;

    public QuotationsPage() : this(false) { }

    public QuotationsPage(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        dgvQuotations.AutoGenerateColumns = false;
        RecordForm.NoAutoSelect(dgvQuotations, on => filling = on, RestoreSelection);
        filling = true;
        cboQuotationStatusFilter.Items.AddRange(["All", .. QuotationStatuses.All]);
        cboQuotationStatusFilter.SelectedIndex = 0;
        filling = false;
        cboSector.Items.AddRange(ProjectSectors.All);
        cboFromInquiry.DisplayMember = nameof(InquiryChoice.Text);

        // Both tables are typed into directly; a new row appears as you type.
        foreach (var grid in new[] { dgvScope, dgvLines })
        {
            grid.ReadOnly = false;
            grid.AllowUserToAddRows = true;
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
        }
        colWork.MaxInputLength = 300;
        colDetails.MaxInputLength = 150;
        colItem.MaxInputLength = 300;

        btnApproveQuotation.Visible = btnRejectQuotation.Visible = approver;
        btnAcceptQuotation.Visible = btnDeclineQuotation.Visible = approver;
        if (addOnly)
        {
            RecordForm.ShowFormOnly(this, lblFormMode, btnSubmitQuotation, btnApproveQuotation, btnRejectQuotation,
                                    btnAcceptQuotation, btnDeclineQuotation);
            _ = StartNewAsync();
            return;
        }
        Responsive.FitGrid(this, dgvQuotations);
        Responsive.OnTypingPause(txtQuotationSearch, () => LoadAsync());
        Access.LockButtons(this, AppPage.Quotations);
        ShowNothingSelected();
    }

    async void QuotationsPage_Load(object sender, EventArgs e) { if (!addOnly) await LoadAsync(); }

    async void Filter_Changed(object? sender, EventArgs e) { if (!filling) await LoadAsync(); }

    QuotationRow? Selected => dgvQuotations.SelectedRows.Count > 0 ? dgvQuotations.SelectedRows[0].DataBoundItem as QuotationRow : null;

    async Task LoadAsync(int? selectId = null)
    {
        if (addOnly) return;
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        var status = cboQuotationStatusFilter.SelectedIndex > 0 ? (string)cboQuotationStatusFilter.SelectedItem! : null;
        List<QuotationRow> rows;
        try { rows = await QuotationService.ListAsync(status, txtQuotationSearch.Text); }
        catch (Exception ex) { lblQuotationMsg.Text = "Can't reach the database. " + ex.Message; return; }
        if (!loads.IsLatest(ticket)) return;
        selectId ??= editingId;
        filling = true;
        dgvQuotations.DataSource = rows;
        dgvQuotations.ClearSelection();   // never pick the first row by itself
        if (selectId is { } id) Responsive.SelectRow(dgvQuotations, (QuotationRow r) => r.QuotationId == id);
        filling = false;
        if (Selected is { } s) await ShowAsync(s.QuotationId); else ShowNothingSelected();
    }

    void RestoreSelection()
    {
        filling = true;
        dgvQuotations.ClearSelection();
        if (editingId is { } id) Responsive.SelectRow(dgvQuotations, (QuotationRow r) => r.QuotationId == id);
        filling = false;
        if (Selected == null) ShowNothingSelected();
    }

    async void dgvQuotations_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        if (Selected is { } s) await ShowAsync(s.QuotationId); else ShowNothingSelected();
    }

    // ----- the form -----

    TextBox[] TextBoxes => [txtClientName, txtClientAddress, txtAttention, txtClientContact, txtClientEmail, txtProjectTitle,
                            txtSiteLocation, txtPrintedNotes, txtWorkDuration, txtPaymentTerms, txtPayableTo, txtPayableNumber,
                            txtSignatoryName, txtSignatoryTitle, txtInternalNotes];

    static string Lines(string? s) => (s ?? "").Replace("\r\n", "\n").Replace("\n", Environment.NewLine);

    void ClearForm()
    {
        loadingForm = true;
        foreach (var t in TextBoxes) t.Clear();
        cboSector.SelectedItem = ProjectSectors.Residential;
        dtpQuoteDate.Value = DateTime.Today;
        dtpValidUntil.Value = DateTime.Today.AddDays(30);
        dtpPlannedStart.Value = DateTime.Today.AddDays(14);
        dtpPlannedStart.Checked = false;
        dgvScope.Rows.Clear();
        dgvLines.Rows.Clear();
        inquiryId = null;
        cboFromInquiry.DataSource = null;
        lblQuotationMsg.Text = "";
        loadingForm = false;
        ShowTotal();
    }

    void ShowNothingSelected()
    {
        editingId = null;
        editingStatus = null;
        original = null;
        ClearForm();
        lblFormMode.Text = "Click a quotation in the list to see it. To make one, click New Quotation.";
        RecordForm.SetEnabled(this, lblFormMode, false);
        btnDeleteQuotation.Enabled = btnPrintQuotation.Enabled = btnSaveQuotationPdf.Enabled = false;
    }

    /// <summary>A new quotation starts with the usual notes, payment and signature details (from the last quotation).</summary>
    async Task StartNewAsync()
    {
        editingId = null;
        editingStatus = QuotationStatuses.Draft;
        ClearForm();
        lblFormMode.Text = $"New quotation {await QuotationService.NextNumberAsync()}";
        var d = await QuotationService.DefaultsAsync();
        txtPrintedNotes.Text = Lines(d.PrintedNotes);
        txtPaymentTerms.Text = d.PaymentTerms;
        txtPayableTo.Text = d.PayableTo;
        txtPayableNumber.Text = d.PayableNumber;
        txtSignatoryName.Text = d.SignatoryName;
        txtSignatoryTitle.Text = d.SignatoryTitle;
        await LoadInquiryChoicesAsync();
    }

    async Task LoadInquiryChoicesAsync()
    {
        var choices = await QuotationService.InquiryChoicesAsync();
        cboFromInquiry.DataSource = choices.Prepend(new InquiryChoice(0, "(none)", "", null, null, null, null)).ToList();
        cboFromInquiry.SelectedIndex = 0;
    }

    async Task ShowAsync(int id)
    {
        if (await QuotationService.GetAsync(id) is not { } found) { ShowNothingSelected(); return; }
        var (q, scope, lines) = found;
        loadingForm = true;
        editingId = q.QuotationId;
        editingStatus = q.Status;
        txtClientName.Text = q.ClientName;
        txtClientAddress.Text = q.ClientAddress ?? "";
        txtAttention.Text = q.Attention ?? "";
        txtClientContact.Text = q.ClientContact ?? "";
        txtClientEmail.Text = q.ClientEmail ?? "";
        txtProjectTitle.Text = q.ProjectTitle;
        txtSiteLocation.Text = q.SiteLocation ?? "";
        cboSector.SelectedItem = q.Sector;
        dtpQuoteDate.Value = q.QuoteDate.ToDateTime(TimeOnly.MinValue);
        dtpValidUntil.Value = q.ValidUntil.ToDateTime(TimeOnly.MinValue);
        dtpPlannedStart.Checked = q.PlannedStart != null;
        if (q.PlannedStart is { } ps) dtpPlannedStart.Value = ps.ToDateTime(TimeOnly.MinValue);
        txtPrintedNotes.Text = Lines(q.PrintedNotes);
        txtWorkDuration.Text = q.WorkDuration ?? "";
        txtPaymentTerms.Text = q.PaymentTerms ?? "";
        txtPayableTo.Text = q.PayableTo ?? "";
        txtPayableNumber.Text = q.PayableNumber ?? "";
        txtSignatoryName.Text = q.SignatoryName ?? "";
        txtSignatoryTitle.Text = q.SignatoryTitle ?? "";
        txtInternalNotes.Text = Lines(q.InternalNotes);
        dgvScope.Rows.Clear();
        foreach (var s in scope) dgvScope.Rows.Add(s.Work, s.Details);
        dgvLines.Rows.Clear();
        foreach (var l in lines) dgvLines.Rows.Add(l.Description, l.Amount.ToString("N2"));
        inquiryId = q.InquiryId;
        await LoadInquiryChoicesAsync();
        loadingForm = false;
        ShowTotal();
        original = ReadForm();

        bool editable = q.Status is QuotationStatuses.Draft or QuotationStatuses.Rejected;
        lblFormMode.Text = $"{q.QuoteNo}  ({q.Status})" + q.Status switch
        {
            QuotationStatuses.Waiting => ": waiting for the Receipts Supervisor's approval",
            QuotationStatuses.Rejected => ": fix it and send it again",
            QuotationStatuses.Approved => ": print it and give it to the client",
            QuotationStatuses.Accepted => ": the client accepted; it is now a project",
            _ => "",
        };
        RecordForm.SetEnabled(this, lblFormMode, editable);
        // The action buttons follow the status, also while the form itself is locked.
        btnSaveQuotation.Enabled = btnCancelQuotation.Enabled = editable;
        btnSubmitQuotation.Enabled = q.Status == QuotationStatuses.Draft;
        btnApproveQuotation.Enabled = btnRejectQuotation.Enabled = q.Status == QuotationStatuses.Waiting;
        btnAcceptQuotation.Enabled = btnDeclineQuotation.Enabled = q.Status == QuotationStatuses.Approved;
        btnDeleteQuotation.Enabled = editable;
        btnPrintQuotation.Enabled = btnSaveQuotationPdf.Enabled = true;
        lblQuotationMsg.Text = "";
    }

    void cboFromInquiry_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        if (cboFromInquiry.SelectedItem is not InquiryChoice c || c.InquiryId == 0) { inquiryId = null; return; }
        inquiryId = c.InquiryId;
        txtClientName.Text = c.FullName;
        txtClientContact.Text = c.Phone ?? "";
        txtClientEmail.Text = c.Email ?? "";
        if (c.Location != null)
        {
            txtSiteLocation.Text = c.Location;
            if (txtClientAddress.Text.Trim() == "") txtClientAddress.Text = c.Location;
        }
        if (c.Service != null && txtProjectTitle.Text.Trim() == "") txtProjectTitle.Text = c.Service;
        lblQuotationMsg.Text = $"Filled in from {c.FullName}'s inquiry.";
    }

    // ----- scope of works, costs and the total -----

    static decimal? Number(object? value)
    {
        var s = (value?.ToString() ?? "").Replace("₱", "").Replace(",", "").Trim();
        if (s == "") return null;
        return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var d) ? d : null;
    }

    static string Cell(DataGridViewRow r, DataGridViewColumn c) => r.Cells[c.Index].Value?.ToString()?.Trim() ?? "";

    List<QuotationScopeLine> ReadScope()
    {
        var scope = new List<QuotationScopeLine>();
        foreach (DataGridViewRow r in dgvScope.Rows)
        {
            if (r.IsNewRow) continue;
            var work = Cell(r, colWork);
            var details = Cell(r, colDetails);
            if (work == "" && details == "") continue;
            scope.Add(new QuotationScopeLine(work, details == "" ? null : details));
        }
        return scope;
    }

    /// <summary>The typed cost lines (empty rows skipped), or the first problem found.</summary>
    (List<QuotationLine> Lines, string? Problem) ReadLines()
    {
        var lines = new List<QuotationLine>();
        foreach (DataGridViewRow r in dgvLines.Rows)
        {
            if (r.IsNewRow) continue;
            var desc = Cell(r, colItem);
            var amountText = Cell(r, colAmount);
            if (desc == "" && amountText == "") continue;
            if (Number(amountText) is not { } amount) return (lines, $"Cost line {lines.Count + 1}: type the amount as a number.");
            lines.Add(new QuotationLine(desc, amount));
        }
        return (lines, null);
    }

    void ShowTotal()
    {
        var (lines, _) = ReadLines();
        lblTotal.Text = $"Total Cost: {ProjectService.Money(QuotationService.Total(lines))}";
    }

    void Lines_Changed(object? sender, DataGridViewCellEventArgs e)
    {
        if (loadingForm) return;
        // show the amount the usual way (25000 -> 25,000.00)
        if (e.RowIndex >= 0 && e.ColumnIndex == colAmount.Index && Number(dgvLines.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) is { } v)
            dgvLines.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = v.ToString("N2");
        ShowTotal();
    }

    void Lines_RowsRemoved(object? sender, DataGridViewRowsRemovedEventArgs e) { if (!loadingForm) ShowTotal(); }

    void btnRemoveLine_Click(object sender, EventArgs e) => RemoveRow(dgvLines);
    void btnRemoveScope_Click(object sender, EventArgs e) => RemoveRow(dgvScope);

    void RemoveRow(DataGridView grid)
    {
        var row = grid.CurrentCell?.OwningRow;
        if (row == null || row.IsNewRow) { lblQuotationMsg.Text = "Click a line in the table first."; return; }
        grid.Rows.Remove(row);
    }

    // ----- saving -----

    QuotationForm ReadForm()
    {
        var (lines, _) = ReadLines();
        var scope = string.Join("; ", ReadScope().Select((s, n) => $"{(char)('A' + n % 26)}. {s.Work}{(s.Details == null ? "" : " (" + s.Details + ")")}"));
        var costs = string.Join("; ", lines.Select(l => $"{l.Description} {l.Amount:N2}"));
        static string? N(string s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        return new QuotationForm(txtClientName.Text.Trim(), N(txtClientAddress.Text), N(txtAttention.Text), N(txtClientContact.Text),
            N(txtClientEmail.Text), txtProjectTitle.Text.Trim(), N(txtSiteLocation.Text), (string?)cboSector.SelectedItem ?? "",
            DateOnly.FromDateTime(dtpQuoteDate.Value), DateOnly.FromDateTime(dtpValidUntil.Value),
            dtpPlannedStart.Checked ? DateOnly.FromDateTime(dtpPlannedStart.Value) : null,
            N(txtPrintedNotes.Text), N(txtWorkDuration.Text), N(txtPaymentTerms.Text), N(txtPayableTo.Text), N(txtPayableNumber.Text),
            N(txtSignatoryName.Text), N(txtSignatoryTitle.Text), N(txtInternalNotes.Text), scope, costs);
    }

    bool HasUnsavedChanges => editingId != null && original != null && ReadForm() != original;

    async void btnSaveQuotation_Click(object sender, EventArgs e)
    {
        if (!addOnly && editingId == null) return;   // the form is locked when nothing is selected
        dgvScope.EndEdit();
        dgvLines.EndEdit();
        var (lines, problem) = ReadLines();
        if (problem != null) { lblQuotationMsg.Text = problem; return; }
        var f = ReadForm();
        var input = new QuotationInput(f.Customer, f.Address, f.Attention, f.Contact, f.Email, f.Project, f.Site, f.Sector, f.QuoteDate,
                                       f.ValidUntil, f.PlannedStart, f.Notes, f.InternalNotes, f.WorkDuration, f.PaymentTerms,
                                       f.PayableTo, f.PayableNumber, f.SignedBy, f.Position, inquiryId, ReadScope(), lines);
        if (QuotationService.CheckInput(input) is { } bad) { lblQuotationMsg.Text = bad; return; }
        if (editingId != null && original != null)
        {
            var answer = RecordForm.ConfirmSave(this, original.Customer, original, f);
            if (answer == SaveAnswer.NothingChanged) { lblQuotationMsg.Text = "Nothing changed."; return; }
            if (answer == SaveAnswer.No) { lblQuotationMsg.Text = "Not saved. Click Cancel to undo your changes."; return; }
        }
        btnSaveQuotation.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await QuotationService.UpdateAsync(id, input);
                if (error != null) { lblQuotationMsg.Text = error; return; }
                editingId = null;
                await LoadAsync(id);
                lblQuotationMsg.Text = "Saved. Click Send for Approval when it's ready.";
            }
            else
            {
                var (newId, error) = await QuotationService.CreateAsync(input);
                if (newId is { } added) Added?.Invoke(added); else lblQuotationMsg.Text = error;
            }
        }
        catch (Exception ex) { lblQuotationMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveQuotation.Enabled = true; }
    }

    async void btnCancelQuotation_Click(object sender, EventArgs e)
    {
        if (addOnly) { FindForm()?.Close(); return; }
        if (editingId is { } id) await ShowAsync(id);
    }

    async void btnNewQuotation_Click(object sender, EventArgs e)
    {
        var page = new QuotationsPage(addOnly: true);
        if (RecordForm.AddDialog(this, "New Quotation", page, a => page.Added += a) is not { } newId) return;
        editingId = null;
        await LoadAsync(newId);
        lblQuotationMsg.Text = "Quotation saved as a Draft. Click Send for Approval when it's ready.";
    }

    /// <summary>Runs a status step and reloads the quotation; shows the result.</summary>
    async Task StepAsync(Func<int, Task<string?>> step, string done)
    {
        if (editingId is not { } id) return;
        try
        {
            var error = await step(id);
            if (error != null) { lblQuotationMsg.Text = error; return; }
            editingId = null;
            await LoadAsync(id);
            lblQuotationMsg.Text = done;
        }
        catch (Exception ex) { lblQuotationMsg.Text = "Can't reach the database. " + ex.Message; }
    }

    async void btnSubmitQuotation_Click(object sender, EventArgs e)
    {
        if (HasUnsavedChanges) { lblQuotationMsg.Text = "Save your changes first."; return; }
        await StepAsync(QuotationService.SubmitAsync, "Sent to the Receipts Supervisor for approval.");
    }

    async void btnApproveQuotation_Click(object sender, EventArgs e) =>
        await StepAsync(QuotationService.ApproveAsync, "Approved. Print it (or save it as PDF) and give it to the client.");

    async void btnRejectQuotation_Click(object sender, EventArgs e) =>
        await StepAsync(QuotationService.RejectAsync, "Rejected. The Receipts Employee can fix it and send it again.");

    async void btnDeclineQuotation_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(this, "Mark this quotation as declined by the client?", "Client Declined", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        await StepAsync(QuotationService.DeclineAsync, "Marked as declined by the client.");
    }

    async void btnAcceptQuotation_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id) return;
        var (lines, _) = ReadLines();
        if (MessageBox.Show(this, $"The client accepted. Make the project \"{txtProjectTitle.Text.Trim()}\" for {txtClientName.Text.Trim()} " +
                $"with a contract price of {ProjectService.Money(QuotationService.Total(lines))}?\n\nIt starts as Pending; add its other details in Projects.",
                "Client Accepted", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        try
        {
            var (projectId, error) = await QuotationService.AcceptAsync(id);
            if (projectId == null) { lblQuotationMsg.Text = error; return; }
            editingId = null;
            await LoadAsync(id);
            lblQuotationMsg.Text = "Accepted: the project was made. Open Projects to add its target end date and other details.";
        }
        catch (Exception ex) { lblQuotationMsg.Text = "Can't reach the database. " + ex.Message; }
    }

    async void btnDeleteQuotation_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id || Selected is not { } row) return;
        if (MessageBox.Show(this, $"Delete quotation {row.QuoteNo} for {row.ClientName}?", "Delete quotation", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        try
        {
            var error = await QuotationService.DeleteAsync(id);
            if (error != null) { lblQuotationMsg.Text = error; return; }
            editingId = null;
            await LoadAsync();
            lblQuotationMsg.Text = "Quotation deleted.";
        }
        catch (Exception ex) { lblQuotationMsg.Text = "Can't reach the database. " + ex.Message; }
    }

    // ----- printing -----

    async void btnPrintQuotation_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id) return;
        using var print = new QuotationPrint(await QuotationService.PrintDataAsync(id));
        using var preview = new PrintPreviewDialog { Document = print.Document, Width = 1000, Height = 800 };
        preview.ShowDialog(this);
    }

    async void btnSaveQuotationPdf_Click(object sender, EventArgs e)
    {
        if (editingId is not { } id || Selected is not { } row) return;
        var safeClient = string.Concat(row.ClientName.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
        using var dlg = new SaveFileDialog { Filter = "PDF|*.pdf", FileName = $"Quotation {row.QuoteNo} - {safeClient}.pdf" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            using var print = new QuotationPrint(await QuotationService.PrintDataAsync(id));
            print.SaveAsPdf(dlg.FileName);
            lblQuotationMsg.Text = $"Saved {dlg.FileName}";
        }
        catch (Exception ex) { lblQuotationMsg.Text = "Couldn't save the PDF. " + ex.Message; }
    }
}
