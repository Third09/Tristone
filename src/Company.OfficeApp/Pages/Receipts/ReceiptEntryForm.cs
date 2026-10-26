using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Receipts;

/// <summary>
/// Add or edit a receipt (Receipt Vault > Add Receipt / Edit), or attach a receipt that turned up later for a
/// no-receipt expense (Needs Review > Attach Receipt: only the receipt fields can be changed).
/// Plain layout for now; control names match the UI Elements Guide (Part 5: Receipt Entry Window).
/// </summary>
public partial class ReceiptEntryForm : Form
{
    readonly int? receiptId;
    readonly bool lateReceipt;
    Receipt? existing;
    string? newScan;

    /// <param name="receiptId">null = new receipt.</param>
    /// <param name="lateReceipt">Attach a receipt to a no-receipt expense.</param>
    public ReceiptEntryForm(int? receiptId = null, bool lateReceipt = false)
    {
        InitializeComponent();
        this.receiptId = receiptId;
        this.lateReceipt = lateReceipt;
        Text = lateReceipt ? "Attach Receipt" : receiptId == null ? "Add Receipt" : "Edit Receipt";
        AcceptButton = btnSaveReceipt;
        CancelButton = btnCancelReceipt;
        cboReceiptCategory.Items.AddRange(ReceiptLists.Categories);
    }

    async void ReceiptEntryForm_Load(object sender, EventArgs e)
    {
        var projects = await MaterialService.ProjectOptionsAsync();
        cboReceiptForProject.Items.AddRange([.. projects]);
        lblUploadedBy.Text = $"Uploaded by: {Session.CurrentUser!.FullName}";
        if (receiptId is { } id && await ReceiptService.GetAsync(id) is { } r)
        {
            existing = r;
            txtReceiptNo.Text = r.ReceiptNo ?? "";
            dtpPurchaseDate.Value = r.PurchaseDate.ToDateTime(TimeOnly.MinValue);
            txtMerchant.Text = r.Merchant;
            numReceiptAmount.Value = r.Amount;
            cboReceiptCategory.SelectedItem = r.Category;
            rdoOverheadExpense.Checked = r.ExpenseType == ExpenseTypes.Overhead;
            rdoProjectExpense.Checked = r.ExpenseType == ExpenseTypes.Project;
            var project = projects.FirstOrDefault(p => p.ProjectId == r.ProjectId);
            if (project == null && r.ProjectId is { } pid) { project = new ProjectOption(pid, $"Project #{pid}"); cboReceiptForProject.Items.Add(project); }
            cboReceiptForProject.SelectedItem = project;
            lblScanFile.Text = r.OriginalName ?? "(no file)";
            chkPhysicalOnly.Checked = r.PhysicalOnly;
            txtFiledLocation.Text = r.FiledLocation ?? "";
            chkNoReceipt.Checked = r.NoReceipt && !lateReceipt;
            txtNoReceiptReason.Text = r.NoReceiptReason ?? "";
            var who = await ReceiptService.UserNameAsync(r.UploadedBy);
            lblUploadedBy.Text = $"Uploaded by: {who}, {r.UploadedAt:MMM d, yyyy}";
        }
        else
        {
            dtpPurchaseDate.Value = DateTime.Today;
            cboReceiptCategory.SelectedIndex = 0;
        }

        if (lateReceipt)
        {
            foreach (Control c in new Control[] { dtpPurchaseDate, txtMerchant, numReceiptAmount, cboReceiptCategory, rdoProjectExpense, rdoOverheadExpense, cboReceiptForProject, chkNoReceipt, txtNoReceiptReason })
                c.Enabled = false;
            lblEntryMsg.Text = "Type the receipt number and attach the scan (or tick Physical copy only).";
        }
        Options_Changed(this, EventArgs.Empty);
    }

    void Options_Changed(object? sender, EventArgs e)
    {
        cboReceiptForProject.Enabled = rdoProjectExpense.Checked && !lateReceipt;
        bool none = chkNoReceipt.Checked;
        txtReceiptNo.Enabled = btnAttachScan.Enabled = chkPhysicalOnly.Enabled = !none;
        txtFiledLocation.Enabled = lblFiledLocation.Enabled = !none && chkPhysicalOnly.Checked;
        btnAttachScan.Enabled = !none && !chkPhysicalOnly.Checked;
        txtNoReceiptReason.Enabled = lblNoReceiptReason.Enabled = none && !lateReceipt;
    }

    void btnAttachScan_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Filter = FileStore.ScanFilter, Title = "Choose the receipt scan or photo" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        if (FileStore.Check(dlg.FileName) is { } problem) { lblEntryMsg.Text = problem; return; }
        newScan = dlg.FileName;
        lblScanFile.Text = Path.GetFileName(newScan);
    }

    ReceiptInput ReadForm() => new(txtReceiptNo.Text, DateOnly.FromDateTime(dtpPurchaseDate.Value), txtMerchant.Text, numReceiptAmount.Value,
        (string?)cboReceiptCategory.SelectedItem ?? "", rdoProjectExpense.Checked ? ExpenseTypes.Project : ExpenseTypes.Overhead,
        (cboReceiptForProject.SelectedItem as ProjectOption)?.ProjectId, newScan, chkPhysicalOnly.Checked, txtFiledLocation.Text,
        chkNoReceipt.Checked, txtNoReceiptReason.Text);

    async void btnSaveReceipt_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        if (!lateReceipt && ReceiptService.CheckInput(input, existing?.FilePath != null) is { } problem) { lblEntryMsg.Text = problem; return; }
        btnSaveReceipt.Enabled = false;
        try
        {
            string? error;
            if (lateReceipt) error = await ReceiptService.AttachLateReceiptAsync(receiptId!.Value, txtReceiptNo.Text, newScan, chkPhysicalOnly.Checked, txtFiledLocation.Text);
            else if (receiptId is { } id) error = await ReceiptService.UpdateAsync(id, input);
            else error = (await ReceiptService.CreateAsync(input)).Error;
            if (error != null) { lblEntryMsg.Text = error; return; }
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { lblEntryMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveReceipt.Enabled = true; }
    }
}
