using Company.OfficeApp.Services;

namespace Company.OfficeApp.Pages.Receipts;

/// <summary>
/// Shows one receipt: the scan (zoomable, for images) and its details; PDF/Word scans open in their own program.
/// Print gives the one-page summary sheet. Plain layout for now; control names match the UI Elements Guide
/// (Part 5: Receipt Viewer Window).
/// </summary>
public partial class ReceiptViewerForm : Form
{
    readonly ReceiptRow row;
    ReceiptView? view;
    string? fullPath;
    float zoom = 1f;

    public ReceiptViewerForm(ReceiptRow row)
    {
        InitializeComponent();
        this.row = row;
        Text = $"Receipt {row.ReceiptNo ?? "(no receipt)"}";
        pnlScan.Controls.Add(picReceiptScan);   // the panel scrolls when zoomed in
        CancelButton = btnCloseViewer;
    }

    async void ReceiptViewerForm_Load(object sender, EventArgs e)
    {
        view = await ReceiptService.ViewAsync(row);
        lblViewReceiptNo.Text = $"Receipt no.: {view.ReceiptNo ?? "(none)"}";
        lblViewDate.Text = $"Date: {view.Date:MMMM d, yyyy}";
        lblViewMerchant.Text = $"Merchant: {view.Merchant}";
        lblViewAmount.Text = $"Amount: {ProjectService.Money(view.Amount)}";
        lblViewCategory.Text = $"Category: {view.Category}";
        lblViewProject.Text = $"For: {view.ProjectOrOverhead}";
        lblViewUploadedBy.Text = $"Uploaded by: {view.UploadedBy}, {view.UploadedAt:MMM d, yyyy}";
        lblViewStatus.Text = view.DecidedBy == null ? $"Status: {view.Status}" : $"Status: {view.Status} by {view.DecidedBy}";
        lblViewCopy.Text = view.Copy switch
        {
            "Paper" => $"Copy: paper only, filed at {view.FiledLocation}",
            "None" => "Copy: no receipt",
            _ => $"Copy: scan ({view.OriginalName})",
        };
        lblViewReason.Text = view.NoReceiptReason == null ? "" : $"No receipt because: {view.NoReceiptReason}";

        fullPath = view.FilePath == null ? null : await FileStore.FullPathAsync(view.FilePath);
        btnOpenOriginal.Enabled = fullPath != null;
        if (fullPath != null && !File.Exists(fullPath)) lblViewerMsg.Text = $"The file is missing from storage:\n{fullPath}";
        else if (ReceiptSheetPrint.IsImage(fullPath))
        {
            using var stream = File.OpenRead(fullPath!);   // copy it, so the file isn't kept locked
            using var img = Image.FromStream(stream);
            picReceiptScan.Image = new Bitmap(img);
        }
        else if (fullPath != null) lblViewerMsg.Text = "This scan is a PDF or Word file: click Open Original to view it.";
        btnZoomIn.Enabled = btnZoomOut.Enabled = picReceiptScan.Image != null;
    }

    void SetZoom(float value)
    {
        zoom = Math.Clamp(value, 1f, 4f);
        var size = pnlScan.ClientSize;
        picReceiptScan.Size = new Size((int)(size.Width * zoom), (int)(size.Height * zoom));
    }

    void btnZoomIn_Click(object sender, EventArgs e) => SetZoom(zoom * 1.5f);

    void btnZoomOut_Click(object sender, EventArgs e) => SetZoom(zoom / 1.5f);

    async void btnOpenOriginal_Click(object sender, EventArgs e)
    {
        if (view?.FilePath is { } path && await FileStore.OpenAsync(path) is { } error) lblViewerMsg.Text = error;
    }

    void btnPrintReceipt_Click(object sender, EventArgs e)
    {
        if (view == null) return;
        using var sheet = new ReceiptSheetPrint([(view, fullPath)]);
        using var preview = new PrintPreviewDialog { Document = sheet.Document, Width = 900, Height = 700 };
        preview.ShowDialog(this);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        picReceiptScan.Image?.Dispose();
        base.OnFormClosed(e);
    }
}
