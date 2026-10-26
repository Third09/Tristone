using System.Drawing.Printing;

namespace Company.OfficeApp.Services;

/// <summary>
/// Receipt summary sheets for auditing (Receipt Viewer > Print, Vault > Print Selected): one page per receipt
/// with its details and, for image scans, the scan itself. PDF/Word scans are noted, not drawn.
/// </summary>
public sealed class ReceiptSheetPrint : IDisposable
{
    readonly PrintDocument document = new();
    readonly IReadOnlyList<(ReceiptView View, string? ImagePath)> sheets;
    int page;

    public PrintDocument Document => document;

    static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".bmp", ".tif", ".tiff"];

    public static bool IsImage(string? path) => path != null && ImageExtensions.Contains(Path.GetExtension(path).ToLowerInvariant());

    /// <param name="sheets">Each receipt and the full path of its scan (null when none).</param>
    public ReceiptSheetPrint(IReadOnlyList<(ReceiptView View, string? ImagePath)> sheets)
    {
        this.sheets = sheets;
        document.DocumentName = sheets.Count == 1 ? $"Receipt {sheets[0].View.ReceiptNo}" : $"{sheets.Count} receipts";
        document.BeginPrint += (_, _) => page = 0;
        document.PrintPage += PrintPage;
    }

    void PrintPage(object? sender, PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        var area = e.MarginBounds;
        var (v, imagePath) = sheets[page];
        float x = area.Left, y = area.Top;
        using var heading = new Font("Segoe UI", 14, FontStyle.Bold);
        using var label = new Font("Segoe UI", 9, FontStyle.Bold);
        using var text = new Font("Segoe UI", 9);
        using var small = new Font("Segoe UI", 8);

        void Line(string l, string? value)
        {
            g.DrawString(l, label, Brushes.Black, x, y);
            g.DrawString(value ?? "-", text, Brushes.Black, new RectangleF(x + 150, y, area.Width - 150, 40));
            y += 20;
        }

        g.DrawString("Tri-Stone Construction Services", text, Brushes.Black, x, y); y += 20;
        g.DrawString($"Receipt Summary: {v.ReceiptNo ?? "(no receipt)"}", heading, Brushes.Black, x, y); y += 32;
        Line("Date of purchase", v.Date.ToString("MMMM d, yyyy"));
        Line("Merchant", v.Merchant);
        Line("Amount", ProjectService.Money(v.Amount));
        Line("Category", v.Category);
        Line("Project / overhead", v.ProjectOrOverhead);
        Line("Uploaded by", $"{v.UploadedBy}, {v.UploadedAt:MMM d, yyyy}");
        Line("Status", v.DecidedBy == null ? v.Status : $"{v.Status} by {v.DecidedBy}");
        Line("Copy", v.Copy == "Paper" ? $"Paper only, filed at: {v.FiledLocation}" : v.Copy);
        if (v.NoReceiptReason != null) Line("No receipt because", v.NoReceiptReason);
        y += 10;

        if (imagePath != null && IsImage(imagePath) && File.Exists(imagePath))
        {
            using var img = Image.FromFile(imagePath);
            var room = new RectangleF(x, y, area.Width, area.Bottom - y - 20);
            var scale = Math.Min(1f, Math.Min(room.Width / img.Width, room.Height / img.Height));   // shrink to fit, never enlarge
            g.DrawImage(img, x, y, img.Width * scale, img.Height * scale);
        }
        else if (v.OriginalName != null) g.DrawString($"Scan attached: {v.OriginalName} (open it to view)", text, Brushes.Black, x, y);

        g.DrawString($"Printed {DateTime.Now:MMMM d, yyyy h:mm tt} by {Session.CurrentUser?.FullName}", small, Brushes.Gray, x, area.Bottom);
        page++;
        e.HasMorePages = page < sheets.Count;
    }

    public void Dispose() => document.Dispose();
}
