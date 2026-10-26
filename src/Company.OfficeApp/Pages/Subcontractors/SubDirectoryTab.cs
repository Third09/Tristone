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
    int? editingId;
    readonly LatestOnly loads = new();

    /// <summary>Raised after changes, so the page can refresh the PCAB count and the other tabs' dropdowns.</summary>
    public event EventHandler? Changed;

    public SubDirectoryTab()
    {
        InitializeComponent();
        dgvSubcontractors.AutoGenerateColumns = false;
        cboSubTradeFilter.Items.Add("All");
        cboSubTradeFilter.Items.AddRange(SubLists.Trades);
        cboSubTradeFilter.SelectedIndex = 0;
        clbSubTrades.Items.AddRange(SubLists.Trades);
        Responsive.FitGrid(this, dgvSubcontractors);
        Responsive.OnTypingPause(txtSubSearch, () => LoadAsync());
        StartNew();
    }

    async void SubDirectoryTab_Load(object sender, EventArgs e) => await LoadAsync();

    static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

    async Task LoadAsync(int? selectId = null)
    {
        var ticket = loads.Start();
        using var busy = Responsive.Busy(this);
        string? trade = cboSubTradeFilter.SelectedIndex > 0 ? (string)cboSubTradeFilter.SelectedItem! : null;
        var rows = await SubcontractorService.ListAsync(txtSubSearch.Text, trade);
        var due = await SubcontractorService.PcabDueCountAsync(Today);
        if (!loads.IsLatest(ticket)) return;
        dgvSubcontractors.DataSource = rows;
        lblPcabDue.Text = due == 0 ? "" : $"{due} PCAB license{(due == 1 ? " is" : "s are")} expired or expiring within {SubLists.PcabWarningDays} days";
        if (selectId is { } id)
            foreach (DataGridViewRow r in dgvSubcontractors.Rows)
                if (((SubRow)r.DataBoundItem!).SubcontractorId == id) { r.Selected = true; dgvSubcontractors.CurrentCell = r.Cells[0]; }
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
        btnDeleteSubcontractor.Enabled = Selected != null;
        if (Selected is { } row && await SubcontractorService.GetAsync(row.SubcontractorId) is { } found) StartEdit(found.Sub, found.Trades);
    }

    IEnumerable<TextBox> Boxes => [txtSubBusinessName, txtSubTradeName, txtSubDtiSec, txtSubTin, txtPrimaryName, txtPrimaryRole,
        txtPrimaryPhone, txtPrimaryMobile, txtPrimaryEmail, txtSecondaryName, txtSecondaryRole, txtSecondaryPhone, txtSecondaryMobile,
        txtSecondaryEmail, txtSubOfficeAddress, txtSubYardAddress, txtSubBankName, txtSubAccountName, txtSubAccountNo, txtPcabNumber,
        txtPcabClassification];

    void StartNew()
    {
        editingId = null;
        lblFormMode.Text = "New subcontractor";
        foreach (var t in Boxes) t.Clear();
        for (int i = 0; i < clbSubTrades.Items.Count; i++) clbSubTrades.SetItemChecked(i, false);
        dtpPcabExpiry.Value = DateTime.Today;
        dtpPcabExpiry.Checked = false;
        lblSubMsg.Text = "";
    }

    void StartEdit(Subcontractor s, List<string> trades)
    {
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
    }

    SubInput ReadForm() => new(txtSubBusinessName.Text, txtSubTradeName.Text, txtSubDtiSec.Text, txtSubTin.Text,
        txtPrimaryName.Text, txtPrimaryRole.Text, txtPrimaryPhone.Text, txtPrimaryMobile.Text, txtPrimaryEmail.Text,
        txtSecondaryName.Text, txtSecondaryRole.Text, txtSecondaryPhone.Text, txtSecondaryMobile.Text, txtSecondaryEmail.Text,
        txtSubOfficeAddress.Text, txtSubYardAddress.Text, txtSubBankName.Text, txtSubAccountName.Text, txtSubAccountNo.Text,
        clbSubTrades.CheckedItems.Cast<string>().ToList(), txtPcabNumber.Text, txtPcabClassification.Text,
        dtpPcabExpiry.Checked ? DateOnly.FromDateTime(dtpPcabExpiry.Value) : null);

    void btnAddSubcontractor_Click(object sender, EventArgs e) { dgvSubcontractors.ClearSelection(); StartNew(); txtSubBusinessName.Focus(); }

    async void btnCancelSubcontractor_Click(object sender, EventArgs e)
    {
        if (editingId is { } id && await SubcontractorService.GetAsync(id) is { } found) StartEdit(found.Sub, found.Trades); else StartNew();
    }

    async void btnSaveSubcontractor_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (SubcontractorService.CheckInput(input) is { } problem) { lblSubMsg.Text = problem; return; }
        btnSaveSubcontractor.Enabled = false;
        try
        {
            if (editingId is { } id)
            {
                var error = await SubcontractorService.UpdateAsync(id, input);
                lblSubMsg.Text = error ?? "Saved.";
                if (error == null) { await LoadAsync(id); Changed?.Invoke(this, EventArgs.Empty); }
            }
            else
            {
                var (newId, error) = await SubcontractorService.CreateAsync(input);
                lblSubMsg.Text = error ?? "Subcontractor added.";
                if (newId != null) { await LoadAsync(newId); Changed?.Invoke(this, EventArgs.Empty); }
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
        StartNew();
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
