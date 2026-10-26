using System.Drawing.Printing;

namespace Company.OfficeApp.Services;

/// <summary>A column in a <see cref="TableReport"/>: header, width share, right-aligned (money) or not.</summary>
public record ReportColumn(string Header, float Width, bool Right = false);

/// <summary>
/// A plain printed table with a title, as many pages as needed, and a total line (Receipts > Reports).
/// To save it as PDF, print to "Microsoft Print to PDF" (<see cref="SaveAsPdf"/> does that directly).
/// </summary>
public sealed class TableReport : IDisposable
{
    readonly PrintDocument document = new();
    readonly string title, subtitle;
    readonly IReadOnlyList<ReportColumn> columns;
    readonly IReadOnlyList<string[]> rows;
    readonly string? totalLine;
    int next;   // first row of the next page

    public PrintDocument Document => document;

    public TableReport(string title, string subtitle, IReadOnlyList<ReportColumn> columns, IReadOnlyList<string[]> rows, string? totalLine)
    {
        (this.title, this.subtitle, this.columns, this.rows, this.totalLine) = (title, subtitle, columns, rows, totalLine);
        document.DocumentName = title;
        document.BeginPrint += (_, _) => next = 0;
        document.PrintPage += PrintPage;
    }

    /// <summary>Prints straight to a PDF file with Windows' built-in "Microsoft Print to PDF" printer.</summary>
    public void SaveAsPdf(string path)
    {
        document.PrinterSettings = new PrinterSettings { PrinterName = "Microsoft Print to PDF", PrintToFile = true, PrintFileName = path };
        document.PrintController = new StandardPrintController();   // no "Printing..." window
        document.Print();
    }

    void PrintPage(object? sender, PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        var area = e.MarginBounds;
        float x = area.Left, y = area.Top;
        using var heading = new Font("Segoe UI", 14, FontStyle.Bold);
        using var bold = new Font("Segoe UI", 9, FontStyle.Bold);
        using var text = new Font("Segoe UI", 9);
        using var small = new Font("Segoe UI", 8);
        var lineHeight = text.GetHeight(g) + 4;

        if (next == 0)
        {
            g.DrawString("Tri-Stone Construction Services", text, Brushes.Black, x, y); y += 20;
            g.DrawString(title, heading, Brushes.Black, x, y); y += 30;
            g.DrawString(subtitle, text, Brushes.Black, x, y); y += 26;
        }

        var total = columns.Sum(c => c.Width);
        var widths = columns.Select(c => c.Width / total * area.Width).ToArray();
        void Row(string[] cells, Font font)
        {
            float cx = x;
            for (int i = 0; i < columns.Count; i++)
            {
                var box = new RectangleF(cx + 2, y, widths[i] - 4, lineHeight);
                using var fmt = new StringFormat { Alignment = columns[i].Right ? StringAlignment.Far : StringAlignment.Near, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
                g.DrawString(i < cells.Length ? cells[i] : "", font, Brushes.Black, box, fmt);
                cx += widths[i];
            }
            y += lineHeight;
        }

        Row(columns.Select(c => c.Header).ToArray(), bold);
        g.DrawLine(Pens.Black, x, y, area.Right, y); y += 2;
        while (next < rows.Count && y + lineHeight * 3 < area.Bottom) Row(rows[next++], text);

        if (next >= rows.Count)
        {
            if (rows.Count == 0) { g.DrawString("(nothing in this range)", text, Brushes.Black, x, y); y += lineHeight; }
            g.DrawLine(Pens.Black, x, y, area.Right, y); y += 4;
            if (totalLine != null) { g.DrawString(totalLine, bold, Brushes.Black, x, y); y += lineHeight; }
            g.DrawString($"Printed {DateTime.Now:MMMM d, yyyy h:mm tt} by {Session.CurrentUser?.FullName}", small, Brushes.Gray, x, y + 10);
            e.HasMorePages = false;
        }
        else e.HasMorePages = true;
    }

    public void Dispose() => document.Dispose();
}
