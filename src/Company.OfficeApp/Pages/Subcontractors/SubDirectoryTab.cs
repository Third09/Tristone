using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Subcontractors;

/// <summary>
/// Subcontractors > Directory: company details, contacts, addresses, bank details (printed on billing vouchers),
/// trade categories and the PCAB license, with a reminder when the license is about to expire.
/// Plain layout for now; control names match the UI Elements Guide (Part 4: Tab: Directory).
/// </summary>
public partial class SubDirectoryTab : UserControl
{
    int? editingId;          // null = nothing selected (or the "add only" pop-up)
    SubInput? original;      // the form as loaded, to list what changed before saving
    bool filling;            // true while the list reloads, so the grid's own selection changes are ignored
    readonly bool addOnly;   // this copy is the Add Subcontractor pop-up (RecordForm)
    readonly LatestOnly loads = new();

    static readonly Dictionary<string, string> Labels = new()
    {
        ["DtiSecNo"] = "DTI / SEC number", ["Tin"] = "TIN", ["PcabLicenseNo"] = "PCAB license number",
        ["PcabClassification"] = "PCAB classification", ["PcabExpiry"] = "PCAB expiry",
    };

    /// <summary>Raised after changes, so the page can refresh the PCAB count and the other tabs' dropdowns.</summary>
    public event EventHandler? Changed;

    /// <summary>The Add Subcontractor pop-up reports the new subcontractor's id here.</summary>
    public event Action<int>? Added;

    public SubDirectoryTab() : this(false) { }

    public SubDirectoryTab(bool addOnly)
    {
        this.addOnly = addOnly;
        InitializeComponent();
        dgvSubcontractors.AutoGenerateColumns = false;
        cboSubTradeFilter.Items.Add("All");
        cboSubTradeFilter.Items.AddRange(SubLists.Trades);
        cboSubTradeFilter.SelectedIndex = 0;
        clbSubTrades.Items.AddRange(SubLists.Trades);
        if (addOnly)
        {
            RecordForm.ShowFormOnly(this, lblFormMode);
            StartNew();
            return;
        }
        Responsive.FitGrid(this, dgvSubcontractors);
        Responsive.OnTypingPause(txtSubSearch, () => LoadAsync());
        ShowNothingSelected();
    }

    async void SubDirectoryTab_Load(object sender, EventArgs e) { if (!addOnly) await LoadAsync(); }

    static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

    async Task LoadAsync(int? selectId = null)
    {
        if (addOnly) return;   // the Add pop-up has no list (its hidden filters still fire on start-up)
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        string? trade = cboSubTradeFilter.SelectedIndex > 0 ? (string)cboSubTradeFilter.SelectedItem! : null;
        var rows = await SubcontractorService.ListAsync(txtSubSearch.Text, trade);
        var due = await SubcontractorService.PcabDueCountAsync(Today);
        if (!loads.IsLatest(ticket)) return;

        selectId ??= editingId;   // searching keeps the subcontractor being edited selected
        filling = true;
        dgvSubcontractors.DataSource = rows;
        dgvSubcontractors.ClearSelection();   // never pick the first row by itself (RecordForm)
        if (selectId is { } id)
            Responsive.SelectRow(dgvSubcontractors, (SubRow row) => row.SubcontractorId == id);
        filling = false;
        lblPcabDue.Text = due == 0 ? "" : $"{due} PCAB license{(due == 1 ? " is" : "s are")} expired or expiring within {SubLists.PcabWarningDays} days";

        if (Selected is not { } selected) ShowNothingSelected();
        else if (selected.SubcontractorId != editingId && await SubcontractorService.GetAsync(selected.SubcontractorId) is { } found)
            StartEdit(found.Sub, found.Trades);
        btnDeleteSubcontractor.Enabled = Selected != null;
    }

    SubRow? Selected => dgvSubcontractors.SelectedRows.Count > 0 ? dgvSubcontractors.SelectedRows[0].DataBoundItem as SubRow : null;

    async void Filter_Changed(object sender, EventArgs e) => await LoadAsync();

    // PCAB reminder: expired = light red, expiring soon = light yellow (a function, not styling).
    void dgvSubcontractors_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.CellStyle == null || dgvSubcontractors.Rows[e.RowIndex].DataBoundItem is not SubRow row) return;
        if (row.PcabExpiry is { } d && d < Today) e.CellStyle.BackColor = Color.MistyRose;
        else if (SubcontractorService.PcabDue(row.PcabExpiry, Today)) e.CellStyle.BackColor = Color.LightYellow;
    }

    async void dgvSubcontractors_SelectionChanged(object sender, EventArgs e)
    {
        if (filling) return;
        btnDeleteSubcontractor.Enabled = Selected != null;
        if (Selected is not { } row) ShowNothingSelected();
        else if (await SubcontractorService.GetAsync(row.SubcontractorId) is { } found) StartEdit(found.Sub, found.Trades);
    }

    void ShowNothingSelected()
    {
        StartNew();
        lblFormMode.Text = "Click a subcontractor in the list to see or change it. To add one, click Add Subcontractor.";
        RecordForm.SetEnabled(this, lblFormMode, false);
    }

    IEnumerable<TextBox> Boxes => [txtSubBusinessName, txtSubTradeName, txtSubDtiSec, txtSubTin, txtPrimaryName, txtPrimaryRole,
        txtPrimaryPhone, txtPrimaryMobile, txtPrimaryEmail, txtSecondaryName, txtSecondaryRole, txtSecondaryPhone, txtSecondaryMobile,
        txtSecondaryEmail, txtSubOfficeAddress, txtSubYardAddress, txtSubBankName, txtSubAccountName, txtSubAccountNo, txtPcabNumber,
        txtPcabClassification];

    void StartNew()
    {
        editingId = null;
        original = null;
        lblFormMode.Text = "New subcontractor";
        foreach (var t in Boxes) t.Clear();
        for (int i = 0; i < clbSubTrades.Items.Count; i++) clbSubTrades.SetItemChecked(i, false);
        dtpPcabExpiry.Value = DateTime.Today;
        dtpPcabExpiry.Checked = false;
        lblSubMsg.Text = "";
    }

    void StartEdit(Subcontractor s, List<string> trades)
    {
        RecordForm.SetEnabled(this, lblFormMode, true);
        editingId = s.SubcontractorId;
        lblFormMode.Text = $"Editing: {s.BusinessName}";
        txtSubBusinessName.Text = s.BusinessName; txtSubTradeName.Text = s.TradeName ?? ""; txtSubDtiSec.Text = s.DtiSecNo ?? "";
        txtSubTin.Text = s.Tin ?? "";
        txtPrimaryName.Text = s.PrimaryName ?? ""; txtPrimaryRole.Text = s.PrimaryRole ?? ""; txtPrimaryPhone.Text = s.PrimaryPhone ?? "";
        txtPrimaryMobile.Text = s.PrimaryMobile ?? ""; txtPrimaryEmail.Text = s.PrimaryEmail ?? "";
        txtSecondaryName.Text = s.SecondaryName ?? ""; txtSecondaryRole.Text = s.SecondaryRole ?? ""; txtSecondaryPhone.Text = s.SecondaryPhone ?? "";
        txtSecondaryMobile.Text = s.SecondaryMobile ?? ""; txtSecondaryEmail.Text = s.SecondaryEmail ?? "";
        txtSubOfficeAddress.Text = s.OfficeAddress ?? ""; txtSubYardAddress.Text = s.YardAddress ?? "";
        txtSubBankName.Text = s.BankName ?? ""; txtSubAccountName.Text = s.AccountName ?? ""; txtSubAccountNo.Text = s.AccountNumber ?? "";
        for (int i = 0; i < clbSubTrades.Items.Count; i++) clbSubTrades.SetItemChecked(i, trades.Contains((string)clbSubTrades.Items[i]));
        txtPcabNumber.Text = s.PcabLicenseNo ?? ""; txtPcabClassification.Text = s.PcabClassification ?? "";
        dtpPcabExpiry.Value = (s.PcabExpiry ?? Today).ToDateTime(TimeOnly.MinValue);
        dtpPcabExpiry.Checked = s.PcabExpiry != null;
        lblSubMsg.Text = "";
        original = ReadForm();
    }

    SubInput ReadForm() => new(txtSubBusinessName.Text, txtSubTradeName.Text, txtSubDtiSec.Text, txtSubTin.Text,
        txtPrimaryName.Text, txtPrimaryRole.Text, txtPrimaryPhone.Text, txtPrimaryMobile.Text, txtPrimaryEmail.Text,
        txtSecondaryName.Text, txtSecondaryRole.Text, txtSecondaryPhone.Text, txtSecondaryMobile.Text, txtSecondaryEmail.Text,
        txtSubOfficeAddress.Text, txtSubYardAddress.Text, txtSubBankName.Text, txtSubAccountName.Text, txtSubAccountNo.Text,
        clbSubTrades.CheckedItems.Cast<string>().ToList(), txtPcabNumber.Text, txtPcabClassification.Text,
        dtpPcabExpiry.Checked ? DateOnly.FromDateTime(dtpPcabExpiry.Value) : null);

    async void btnAddSubcontractor_Click(object sender, EventArgs e)
    {
        var page = new SubDirectoryTab(addOnly: true);
        if (RecordForm.AddDialog(this, "Add Subcontractor", page, a => page.Added += a) is not { } newId) return;
        editingId = null;
        await LoadAsync(newId);
        lblSubMsg.Text = "Subcontractor added.";
        Changed?.Invoke(this, EventArgs.Empty);
    }

    async void btnCancelSubcontractor_Click(object sender, EventArgs e)
    {
        if (addOnly) { FindForm()?.Close(); return; }
        if (editingId is { } id && await SubcontractorService.GetAsync(id) is { } found) StartEdit(found.Sub, found.Trades);
    }

    async void btnSaveSubcontractor_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (SubcontractorService.CheckInput(input) is { } problem) { lblSubMsg.Text = problem; return; }
        if (!addOnly && editingId == null) return;   // the form is locked when nothing is selected
        if (editingId != null && original != null)
        {
            var answer = RecordForm.ConfirmSave(this, original.BusinessName, original, input, Labels,
                nameof(SubInput.Tin), nameof(SubInput.AccountNumber));
            if (answer == SaveAnswer.NothingChanged) { lblSubMsg.Text = "Nothing changed."; return; }
            if (answer == SaveAnswer.No) { lblSubMsg.Text = "Not saved. Click Cancel to undo your changes."; return; }
        }
        btnSaveSubcontractor.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await SubcontractorService.UpdateAsync(id, input);
                if (error != null) { lblSubMsg.Text = error; return; }
                editingId = null;   // reload the form from the saved record
                await LoadAsync(id);
                lblSubMsg.Text = "Saved.";
                Changed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                var (newId, error) = await SubcontractorService.CreateAsync(input);
                if (newId is { } added) Added?.Invoke(added); else lblSubMsg.Text = error;
            }
        }
        catch (Exception ex) { lblSubMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveSubcontractor.Enabled = true; }
    }

    async void btnDeleteSubcontractor_Click(object sender, EventArgs e)
    {
        if (Selected is not { } s) return;
        if (MessageBox.Show(this, $"Delete \"{s.BusinessName}\"?", "Delete subcontractor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        var error = await SubcontractorService.DeleteAsync(s.SubcontractorId);
        if (error != null) { MessageBox.Show(this, error, "Not deleted", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        editingId = null;
        await LoadAsync();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    void btnExportSubcontractors_Click(object sender, EventArgs e)
    {
        if (dgvSubcontractors.DataSource is not List<SubRow> rows) return;
        if (ExcelExport.AskPath(this, "Subcontractors") is not { } path) return;
        try
        {
            ExcelExport.Save(path, "Subcontractors", rows, new("Business Name", r => r.BusinessName), new("Trades", r => r.Trades),
                new("Primary Contact", r => r.PrimaryContact), new("Phone", r => r.Phone), new("PCAB Expiry", r => r.PcabExpiry, "mmm d, yyyy"));
            MessageBox.Show(this, $"Saved {rows.Count} subcontractors to:\n{path}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex) { MessageBox.Show(this, "Couldn't save the file. Is it open in Excel?\n" + ex.Message, "Export"); }
    }
}
