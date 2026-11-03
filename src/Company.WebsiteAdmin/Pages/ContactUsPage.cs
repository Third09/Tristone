using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Website Admin > Contact Us (Figma "admin contact us"): the inquiry counts and Website Traffic, where inquiries are
/// forwarded, the company details shown on the website, and the inquiries from the Contact form. Double-click an
/// inquiry (or Open Inquiry) to read and follow it up in its own window (owner decision 2026-11-01).
/// </summary>
public partial class ContactUsPage : UserControl, IWebsiteEditor
{
    record DetailsForm(string Address, string Phone, string Email, string Footer);

    static readonly string[] DetailKeys = [WebsiteKeys.Address, WebsiteKeys.Phone, WebsiteKeys.Email, WebsiteKeys.Copyright];

    DetailsForm? originalDetails;
    string originalRecipient = "";
    bool filling;
    readonly LatestOnly loads = new();
    Font? bold;

    /// <summary>Raised after an inquiry's status changes, so the sidebar's "(N new)" updates.</summary>
    public event EventHandler? Changed;

    public ContactUsPage()
    {
        InitializeComponent();
        dgvInquiries.AutoGenerateColumns = false;
        RecordForm.NoAutoSelect(dgvInquiries, on => filling = on, () => { filling = true; dgvInquiries.ClearSelection(); filling = false; });
        dgvInquiries.CellFormatting += dgvInquiries_CellFormatting;
        Disposed += (_, _) => bold?.Dispose();
        filling = true;
        cboInquiryStatusFilter.Items.AddRange(["All", .. InquiryStatuses.All]);
        cboInquiryStatusFilter.SelectedIndex = 0;
        filling = false;
        txtRecipientEmail.MaxLength = WebsiteContentService.MaxLength(WebsiteKeys.RecipientEmail);
        txtCompanyAddress.MaxLength = WebsiteContentService.MaxLength(WebsiteKeys.Address);
        txtCompanyPhone.MaxLength = WebsiteContentService.MaxLength(WebsiteKeys.Phone);
        txtCompanyEmail.MaxLength = WebsiteContentService.MaxLength(WebsiteKeys.Email);
        txtCopyright.MaxLength = WebsiteContentService.MaxLength(WebsiteKeys.Copyright);
        // Figma layout: cards on a light gray page; smaller windows scroll.
        BackColor = pnlGap1.BackColor = pnlGap2.BackColor = pnlKpis.BackColor = Theme.Canvas;
        pnlKpiGap1.BackColor = pnlKpiGap2.BackColor = pnlKpiGap3.BackColor = Theme.Canvas;
        Padding = new Padding(24, 8, 24, 24);
        AutoScrollMinSize = new Size(1100, 920);
        foreach (var l in new[] { lblTotalNote, lblPendingNote, lblVisitsNote, lblRecipientNote, lblInquiriesSub, lblInquiryCount, lblContactSettingsMsg })
            l.ForeColor = Theme.TextMuted;
        lblPendingInquiries.ForeColor = Theme.Orange;
        Responsive.OnTypingPause(txtInquirySearch, () => LoadInquiriesAsync());
    }

    public bool HasUnsavedChanges =>
        (originalDetails != null && ReadDetails() != originalDetails) || (originalRecipient != txtRecipientEmail.Text.Trim());

    async void ContactUsPage_Load(object sender, EventArgs e)
    {
        await LoadSettingsAsync();
        await PullAndLoadAsync();
    }

    async void btnRefreshContact_Click(object sender, EventArgs e) => await PullAndLoadAsync();

    /// <summary>Reloads the list (used when a new-inquiry notification is clicked while this page is open).</summary>
    public async void RefreshInquiries() => await LoadInquiriesAsync();

    /// <summary>Gets new inquiries from the website's Google Script (InquiryImport), then shows the list.</summary>
    async Task PullAndLoadAsync()
    {
        InquiryImport.Result pulled;
        lblInquiryCount.Text = "Checking the website for new inquiries...";
        try { pulled = await InquiryImport.PullAsync(); }
        catch (Exception ex) { pulled = new(0, "Can't reach the database. " + ex.Message); }
        await LoadInquiriesAsync();
        if (pulled.Problem != null) lblInquiryCount.Text += "   (New inquiries not checked: " + pulled.Problem + ")";
        else if (pulled.Added > 0) lblInquiryCount.Text += $"   ({pulled.Added} new from the website)";
        if (pulled.Added > 0) Changed?.Invoke(this, EventArgs.Empty);   // sidebar "(N new)"
    }

    async Task LoadSettingsAsync()
    {
        try
        {
            var v = await WebsiteContentService.LoadAsync([.. DetailKeys, WebsiteKeys.RecipientEmail]);
            txtRecipientEmail.Text = originalRecipient = v[WebsiteKeys.RecipientEmail] ?? "";
            txtCompanyAddress.Text = v[WebsiteKeys.Address] ?? "";
            txtCompanyPhone.Text = v[WebsiteKeys.Phone] ?? "";
            txtCompanyEmail.Text = v[WebsiteKeys.Email] ?? "";
            txtCopyright.Text = v[WebsiteKeys.Copyright] ?? "";
            originalDetails = ReadDetails();
        }
        catch (Exception ex) { lblContactSettingsMsg.Text = "Can't reach the database. " + ex.Message; }
    }

    DetailsForm ReadDetails() => new(txtCompanyAddress.Text.Trim(), txtCompanyPhone.Text.Trim(), txtCompanyEmail.Text.Trim(), txtCopyright.Text.Trim());

    async void btnSaveRecipientEmail_Click(object sender, EventArgs e)
    {
        var email = txtRecipientEmail.Text.Trim();
        if (email == originalRecipient) { lblContactSettingsMsg.Text = "Nothing changed."; return; }
        if (MessageBox.Show(this, $"Forward website inquiries to {email} instead of {originalRecipient}?", "Recipient email",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        btnSaveRecipientEmail.Enabled = false;
        try
        {
            var error = await WebsiteContentService.SaveAsync("Inquiry recipient email", new() { [WebsiteKeys.RecipientEmail] = email });
            if (error != null) { lblContactSettingsMsg.Text = error; return; }
            await LoadSettingsAsync();
            // The website's Google Script sends the emails, so it gets the new address too.
            var scriptProblem = await InquiryImport.SetRecipientAsync(email);
            lblContactSettingsMsg.Text = scriptProblem == null
                ? $"Saved. Website inquiries are now emailed to {email}."
                : "Saved here, but the website still emails the old address: " + scriptProblem;
        }
        catch (Exception ex) { lblContactSettingsMsg.Text = "Unable to save changes. Please try again. " + ex.Message; }
        finally { btnSaveRecipientEmail.Enabled = true; }
    }

    async void btnSaveCompanyDetails_Click(object sender, EventArgs e)
    {
        if (originalDetails == null) return;
        var input = ReadDetails();
        var answer = RecordForm.ConfirmSave(this, "the company details", originalDetails, input);
        if (answer == SaveAnswer.NothingChanged) { lblContactSettingsMsg.Text = "Nothing changed."; return; }
        if (answer == SaveAnswer.No) { lblContactSettingsMsg.Text = "Not saved."; return; }
        btnSaveCompanyDetails.Enabled = false;
        try
        {
            var error = await WebsiteContentService.SaveAsync("Company details", new()
            {
                [WebsiteKeys.Address] = input.Address,
                [WebsiteKeys.Phone] = input.Phone,
                [WebsiteKeys.Email] = input.Email,
                [WebsiteKeys.Copyright] = input.Footer,
            });
            if (error != null) { lblContactSettingsMsg.Text = error; return; }
            await LoadSettingsAsync();
            lblContactSettingsMsg.Text = "Details saved. Click Publish Website to put them on the live site.";
        }
        catch (Exception ex) { lblContactSettingsMsg.Text = "Unable to save changes. Please try again. " + ex.Message; }
        finally { btnSaveCompanyDetails.Enabled = true; }
    }

    // ----- inquiries -----

    async void Filter_Changed(object? sender, EventArgs e) { if (!filling) await LoadInquiriesAsync(); }

    async void btnResetInquiryFilter_Click(object sender, EventArgs e)
    {
        filling = true;
        txtInquirySearch.Clear();
        cboInquiryStatusFilter.SelectedIndex = 0;
        dtpInquiryFrom.Checked = dtpInquiryTo.Checked = false;
        filling = false;
        await LoadInquiriesAsync();
    }

    async Task LoadInquiriesAsync()
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        try
        {
            var status = cboInquiryStatusFilter.SelectedIndex > 0 ? (string)cboInquiryStatusFilter.SelectedItem! : null;
            DateOnly? from = dtpInquiryFrom.Checked ? DateOnly.FromDateTime(dtpInquiryFrom.Value) : null;
            DateOnly? to = dtpInquiryTo.Checked ? DateOnly.FromDateTime(dtpInquiryTo.Value) : null;
            var rows = await WebsiteInquiries.ListAsync(status, txtInquirySearch.Text, from, to);
            var counts = await WebsiteInquiries.CountsAsync();
            var visits = await WebsiteInquiries.VisitsThisMonthAsync();
            if (!loads.IsLatest(ticket)) return;

            lblTotalInquiries.Text = $"{counts.New + counts.Contacted + counts.Closed:#,##0}";
            lblPendingInquiries.Text = $"{counts.New:#,##0}";
            lblWebsiteVisits.Text = $"{visits:#,##0}";
            filling = true;
            dgvInquiries.DataSource = rows;
            dgvInquiries.ClearSelection();   // never pick the first row by itself
            filling = false;
            lblInquiryCount.Text = $"Showing {rows.Count} inquir{(rows.Count == 1 ? "y" : "ies")}";
        }
        catch (Exception ex) { lblInquiryCount.Text = "Can't reach the database. " + ex.Message; }
    }

    // New inquiries stand out (a function, not styling).
    void dgvInquiries_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex >= 0 && e.CellStyle != null && dgvInquiries.Rows[e.RowIndex].DataBoundItem is InquiryRow { Status: InquiryStatuses.New })
            e.CellStyle.Font = bold ??= new Font(dgvInquiries.Font, FontStyle.Bold);
    }

    InquiryRow? Selected => dgvInquiries.SelectedRows.Count > 0 ? dgvInquiries.SelectedRows[0].DataBoundItem as InquiryRow : null;

    async void dgvInquiries_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) await OpenAsync();
    }

    async void btnOpenInquiry_Click(object sender, EventArgs e) => await OpenAsync();

    async Task OpenAsync()
    {
        if (Selected is not { } row) { lblInquiryCount.Text = "Click an inquiry in the list first."; return; }
        if (await WebsiteInquiries.GetAsync(row.InquiryId) is not { } inquiry) return;
        using var f = new InquiryForm(inquiry);
        f.ShowDialog(this);
        if (!f.Saved) return;
        await LoadInquiriesAsync();
        Responsive.SelectRow(dgvInquiries, (InquiryRow r) => r.InquiryId == row.InquiryId);
        Changed?.Invoke(this, EventArgs.Empty);
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
