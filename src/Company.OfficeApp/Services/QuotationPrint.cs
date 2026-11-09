using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using Company.Shared.Models;

namespace Company.OfficeApp.Services;

/// <summary>
/// The printed quotation for the client (Quotations > Print / Save as PDF): logo and company details, client, the
/// lines (continued on more pages when needed), totals, terms and signature lines. Until it is approved it carries a
/// "DRAFT" mark. To save it as PDF, choose the "Microsoft Print to PDF" printer.
/// </summary>
public sealed class QuotationPrint : IDisposable
{
    readonly PrintDocument document = new();
    readonly QuotationPrintData d;
    int nextLine;   // first line still to print (pages continue the table)
    int page;

    public PrintDocument Document => document;

    public QuotationPrint(QuotationPrintData data)
    {
        d = data;
        document.DocumentName = $"Quotation {d.Quotation.QuoteNo}";
        document.BeginPrint += (_, _) => { nextLine = 0; page = 0; };
        document.PrintPage += PrintPage;
    }

    /// <summary>Prints straight to "Microsoft Print to PDF" into <paramref name="path"/> (no dialog).</summary>
    public void SaveAsPdf(string path)
    {
        document.PrinterSettings = new PrinterSettings { PrinterName = "Microsoft Print to PDF", PrintToFile = true, PrintFileName = path };
        document.PrintController = new StandardPrintController();
        document.Print();
    }

    static readonly Color Orange = ColorTranslator.FromHtml("#F47A20");
    static readonly Color Dark = ColorTranslator.FromHtml("#1D2939");
    static readonly Color Muted = ColorTranslator.FromHtml("#667085");
    static readonly Color Line = ColorTranslator.FromHtml("#D0D5DD");
    static readonly Color Shade = ColorTranslator.FromHtml("#F2F4F7");

    void PrintPage(object? sender, PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var q = d.Quotation;
        var m = e.MarginBounds;
        float x = m.Left, y = m.Top, w = m.Width;
        page++;
        using var title = new Font("Segoe UI", 20, FontStyle.Bold);
        using var bold = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        using var text = new Font("Segoe UI", 9.5f);
        using var small = new Font("Segoe UI", 8);
        using var dark = new SolidBrush(Dark);
        using var muted = new SolidBrush(Muted);
        using var linePen = new Pen(Line);
        using var orangeBrush = new SolidBrush(Orange);
        static string Money(decimal v) => ProjectService.Money(v);

        // ----- header: logo + company on the left, QUOTATION box on the right -----
        try { var logo = UI.Theme.Logo; g.DrawImage(logo, x, y, 170, 170f * logo.Height / logo.Width); } catch { /* print without it */ }
        float cy = y + 52;
        foreach (var l in new[] { d.CompanyAddress, d.CompanyPhone, d.CompanyEmail }.Where(s => !string.IsNullOrWhiteSpace(s)))
        {
            g.DrawString(l, small, muted, new RectangleF(x, cy, w * 0.55f, 30)); cy += 15;
        }
        var right = new StringFormat { Alignment = StringAlignment.Far };
        g.DrawString("QUOTATION", title, orangeBrush, x + w, y - 4, right);
        g.DrawString($"No. {q.QuoteNo}", bold, dark, x + w, y + 36, right);
        g.DrawString($"Date: {q.QuoteDate:MMMM d, yyyy}", text, dark, x + w, y + 54, right);
        g.DrawString($"Valid until: {q.ValidUntil:MMMM d, yyyy}", text, dark, x + w, y + 70, right);
        if (page > 1) g.DrawString($"Page {page}", small, muted, x + w, y + 88, right);
        y = Math.Max(cy, y + 104) + 8;
        g.FillRectangle(orangeBrush, x, y, w, 3); y += 14;

        if (page == 1)
        {
            // ----- client and project -----
            g.DrawString("Prepared for", bold, muted, x, y);
            g.DrawString("Project", bold, muted, x + w / 2, y); y += 18;
            var client = string.Join("\n", new[] { q.ClientName, q.ClientContact, q.ClientEmail }.Where(s => !string.IsNullOrWhiteSpace(s)));
            var project = string.Join("\n", new[] { q.ProjectTitle, q.SiteLocation, q.Sector, q.PlannedStart is { } s ? $"Planned start: {s:MMMM d, yyyy}" : null }.Where(s => !string.IsNullOrWhiteSpace(s)));
            var h1 = g.MeasureString(client, text, (int)(w / 2 - 10)).Height;
            var h2 = g.MeasureString(project, text, (int)(w / 2)).Height;
            g.DrawString(client, text, dark, new RectangleF(x, y, w / 2 - 10, h1 + 4));
            g.DrawString(project, text, dark, new RectangleF(x + w / 2, y, w / 2, h2 + 4));
            y += Math.Max(h1, h2) + 16;
        }

        // ----- table -----
        // edges: # | Item | Qty (right-aligned) | Unit | Unit Price (right-aligned) | Amount (right-aligned)
        float[] col = [x, x + 30, x + w * 0.50f, x + w * 0.59f, x + w * 0.68f, x + w * 0.84f, x + w];
        var farFmt = new StringFormat { Alignment = StringAlignment.Far };
        void Header()
        {
            using var shade = new SolidBrush(Shade);
            g.FillRectangle(shade, x, y, w, 24);
            g.DrawRectangle(linePen, x, y, w, 24);
            g.DrawString("#", bold, dark, col[0] + 6, y + 4);
            g.DrawString("Item / Description", bold, dark, col[1] + 4, y + 4);
            g.DrawString("Qty", bold, dark, col[3] - 6, y + 4, farFmt);
            g.DrawString("Unit", bold, dark, col[3] + 6, y + 4);
            g.DrawString("Unit Price", bold, dark, col[5] - 6, y + 4, farFmt);
            g.DrawString("Amount", bold, dark, col[6] - 6, y + 4, farFmt);
            y += 24;
        }
        Header();
        float bottomRoom = 230;   // totals, terms and signatures need this much on the last page
        while (nextLine < d.Lines.Count)
        {
            var l = d.Lines[nextLine];
            var descH = g.MeasureString(l.Description, text, (int)(col[2] - col[1] - 8)).Height;
            var rowH = Math.Max(22, descH + 8);
            bool lastLine = nextLine == d.Lines.Count - 1;
            if (y + rowH > m.Bottom - (lastLine ? bottomRoom : 30)) { e.HasMorePages = true; return; }
            g.DrawRectangle(linePen, x, y, w, rowH);
            g.DrawString((nextLine + 1).ToString(), text, dark, col[0] + 6, y + 4);
            g.DrawString(l.Description, text, dark, new RectangleF(col[1] + 4, y + 4, col[2] - col[1] - 8, rowH));
            g.DrawString(l.Quantity.ToString("#,##0.##"), text, dark, col[3] - 6, y + 4, farFmt);
            g.DrawString(l.Unit, text, dark, col[3] + 6, y + 4);
            g.DrawString(Money(l.UnitPrice), text, dark, col[5] - 6, y + 4, farFmt);
            g.DrawString(Money(l.Amount), text, dark, col[6] - 6, y + 4, farFmt);
            y += rowH;
            nextLine++;
        }

        // ----- totals -----
        y += 8;
        void Total(string label, string value, Font f)
        {
            g.DrawString(label, f, dark, col[5] - 20, y, farFmt);
            g.DrawString(value, f, dark, col[6] - 6, y, farFmt);
            y += 20;
        }
        var t = d.Totals;
        Total("Subtotal", Money(t.Subtotal), text);
        if (t.Discount > 0) Total("Less discount", "-" + Money(t.Discount), text);
        if (q.VatApplies) Total($"VAT {QuotationService.VatRate:P0}", Money(t.Vat), text);
        g.FillRectangle(orangeBrush, col[4], y + 2, col[6] - col[4], 2); y += 8;
        using (var big = new Font("Segoe UI", 12, FontStyle.Bold)) Total("TOTAL", Money(t.Total), big);
        if (!q.VatApplies) { g.DrawString("VAT not included.", small, muted, col[6] - 6, y, farFmt); y += 16; }

        // ----- terms -----
        if (!string.IsNullOrWhiteSpace(q.Terms))
        {
            y += 6;
            g.DrawString("Terms and conditions", bold, dark, x, y); y += 18;
            var termsH = g.MeasureString(q.Terms, small, (int)w).Height;
            g.DrawString(q.Terms, small, muted, new RectangleF(x, y, w, termsH + 4)); y += termsH + 10;
        }

        // ----- signatures -----
        y = Math.Max(y + 20, m.Bottom - 90);
        float half = w / 2 - 20;
        g.DrawLine(linePen, x, y + 30, x + half, y + 30);
        g.DrawLine(linePen, x + w - half, y + 30, x + w, y + 30);
        g.DrawString($"Prepared by: {d.PreparedBy}", text, dark, x, y + 34);
        g.DrawString(d.ApprovedBy is { } a ? $"Approved by: {a}" : "Approved by:", text, dark, x, y + 50);
        g.DrawString("Client conforme (signature over printed name)", text, dark, x + w - half, y + 34);
        g.DrawString("Date:", text, dark, x + w - half, y + 50);
        g.DrawString($"Printed {DateTime.Now:MMMM d, yyyy h:mm tt}", small, muted, x, m.Bottom + 10);

        // ----- not approved yet: a light DRAFT mark across the page -----
        if (q.Status is QuotationStatuses.Draft or QuotationStatuses.Waiting or QuotationStatuses.Rejected)
        {
            var state = g.Save();
            g.TranslateTransform(m.Left + m.Width / 2f, m.Top + m.Height / 2f);
            g.RotateTransform(-35);
            using var wm = new Font("Segoe UI", 90, FontStyle.Bold);
            using var faint = new SolidBrush(Color.FromArgb(28, Orange));
            g.DrawString("DRAFT", wm, faint, 0, 0, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            g.Restore(state);
        }
        e.HasMorePages = false;
    }

    public void Dispose() => document.Dispose();
}
