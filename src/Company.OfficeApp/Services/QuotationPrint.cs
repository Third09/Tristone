using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using Company.Shared.Models;

namespace Company.OfficeApp.Services;

/// <summary>
/// The printed quotation, laid out like the company's own quotation paper ("new quote.docx", owner 2026-11-09):
/// brick-wall logo, QUOTATION, CUSTOMER / ADDRESS / ATTENTION and DATE, a line, Scope of Works (A, B, C... with
/// sizes), the cost lines, Total Cost, Notes (with Work Duration and Terms of Payment), Cash Payable to, then
/// CONFORME BY / VERY TRULY YOURS. Long quotations continue on more pages (the signature part is never split).
/// Until it is approved it carries a "DRAFT" mark. To save it as PDF, choose the "Microsoft Print to PDF" printer.
/// </summary>
public sealed class QuotationPrint : IDisposable
{
    readonly PrintDocument document = new();
    readonly QuotationPrintData d;
    readonly Font text = new("Arial Narrow", 11.5f, FontStyle.Bold);
    readonly Font label = new("Arial Narrow", 11.5f, FontStyle.Bold | FontStyle.Underline);
    readonly Font totalFont = new("Arial Narrow", 13f, FontStyle.Bold | FontStyle.Underline);
    readonly Font title = new("Cascadia Code", 17f, FontStyle.Bold);
    readonly Font small = new("Arial Narrow", 8.5f);

    /// <summary>A piece of the page with its height; drawn at the given top.</summary>
    record Block(float Height, Action<Graphics, float> Draw);
    List<Block>? blocks;
    int next, page;

    public PrintDocument Document => document;

    public QuotationPrint(QuotationPrintData data)
    {
        d = data;
        document.DocumentName = $"Quotation {d.Quotation.QuoteNo}";
        document.BeginPrint += (_, _) => { next = 0; page = 0; blocks = null; };
        document.PrintPage += PrintPage;
    }

    /// <summary>Prints straight to "Microsoft Print to PDF" into <paramref name="path"/> (no dialog).</summary>
    public void SaveAsPdf(string path)
    {
        document.PrinterSettings = new PrinterSettings { PrinterName = "Microsoft Print to PDF", PrintToFile = true, PrintFileName = path };
        document.PrintController = new StandardPrintController();
        document.Print();
    }

    /// <summary>₱5,000 (centavos only when there are some).</summary>
    static string Peso(decimal v) => "₱" + Plain(v);
    static string Plain(decimal v) => v.ToString(v == Math.Round(v) ? "#,##0" : "#,##0.00", CultureInfo.InvariantCulture);

    List<Block> Build(Graphics g, RectangleF m)
    {
        var q = d.Quotation;
        float x = m.Left, w = m.Width;
        float col2 = x + w * 0.62f;          // second column: sizes, amounts, DATE, VERY TRULY YOURS
        float letterX = x + w * 0.05f, itemX = x + w * 0.085f;
        float lineH = text.GetHeight(g) + 3;
        var list = new List<Block>();
        void Gap(float h) => list.Add(new Block(h, (_, _) => { }));
        float Measure(string s, Font f, float width) => g.MeasureString(s, f, (int)width).Height;
        var black = Brushes.Black;

        // ----- logo and title -----
        var logo = UI.Theme.QuoteHeader;
        float logoW = w * 0.92f, logoH = logoW * logo.Height / logo.Width;
        list.Add(new Block(logoH + 6, (gr, y) => gr.DrawImage(logo, x + (w - logoW) / 2, y, logoW, logoH)));
        list.Add(new Block(title.GetHeight(g) + 18, (gr, y) =>
            gr.DrawString("QUOTATION", title, black, new RectangleF(x, y + 2, w, 40), new StringFormat { Alignment = StringAlignment.Center })));

        // ----- CUSTOMER / ADDRESS / ATTENTION on the left, DATE on the right -----
        float valueX = x + w * 0.14f, valueW = col2 - valueX - 10;
        var date = "DATE: " + q.QuoteDate.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture).ToUpperInvariant();
        var rows = new[] { ("CUSTOMER:", q.ClientName, date), ("ADDRESS:", q.ClientAddress ?? "", $"NO.: {q.QuoteNo}"), ("ATTENTION:", q.Attention ?? "", "") };
        foreach (var (l, v, right) in rows)
        {
            var h = Math.Max(lineH, Measure(v, text, valueW) + 2);
            list.Add(new Block(h, (gr, y) =>
            {
                gr.DrawString(l, label, black, x, y);
                gr.DrawString(v, text, black, new RectangleF(valueX, y, valueW, h + 2));
                if (right != "") gr.DrawString(right, right.StartsWith("NO.") ? small : text, black, col2, y + (right.StartsWith("NO.") ? 3 : 0));
            }));
        }
        Gap(14);
        list.Add(new Block(22, (gr, y) => { using var pen = new Pen(Color.Black, 2f); gr.DrawLine(pen, x, y + 4, x + w, y + 4); }));

        // ----- Scope of Works -----
        list.Add(new Block(lineH, (gr, y) => gr.DrawString("Scope of Works:", text, black, x, y)));
        float workW = col2 - itemX - 10, detailsW = x + w - col2;
        for (int n = 0; n < d.Scope.Count; n++)
        {
            var s = d.Scope[n];
            var letter = Letter(n) + ".";
            var h = Math.Max(Measure(s.Work, text, workW), s.Details == null ? 0 : Measure(s.Details, text, detailsW)) + 3;
            list.Add(new Block(Math.Max(lineH, h), (gr, y) =>
            {
                gr.DrawString(letter, text, black, letterX, y);
                gr.DrawString(s.Work, text, black, new RectangleF(itemX, y, workW, h + 2));
                if (s.Details != null) gr.DrawString(s.Details, text, black, new RectangleF(col2, y, detailsW, h + 2));
            }));
        }
        Gap(lineH * 0.8f);

        // ----- cost lines and Total Cost -----
        foreach (var l in d.Lines)
        {
            var h = Math.Max(lineH, Measure(l.Description + " -", text, workW) + 3);
            list.Add(new Block(h, (gr, y) =>
            {
                gr.DrawString(l.Description + " -", text, black, new RectangleF(itemX, y, workW, h + 2));
                gr.DrawString(Peso(l.Amount), text, black, col2, y);
            }));
        }
        Gap(lineH * 0.9f);
        list.Add(new Block(totalFont.GetHeight(g) + 22, (gr, y) =>
            gr.DrawString($"Total Cost: {Plain(d.Total)} pesos", totalFont, black, x + w * 0.025f, y)));

        // ----- Notes, Work Duration, Terms of Payment -----
        var notes = (q.PrintedNotes ?? "").Replace("\r\n", "\n").Split('\n').Select(s => s.Trim()).Where(s => s != "").ToList();
        if (q.WorkDuration != null) notes.Add($"Work Duration:   {q.WorkDuration}");
        if (notes.Count > 0 || q.PaymentTerms != null)
            list.Add(new Block(lineH + 2, (gr, y) => gr.DrawString("Notes:", text, black, x, y)));
        float noteW = x + w - itemX;
        foreach (var note in notes)
        {
            var h = Measure(note, text, noteW) + 3;
            list.Add(new Block(Math.Max(lineH, h), (gr, y) =>
            {
                gr.DrawString("•", text, black, letterX, y);
                gr.DrawString(note, text, black, new RectangleF(itemX, y, noteW, h + 2));
            }));
        }
        if (q.PaymentTerms != null)
        {
            Gap(lineH * 0.6f);
            list.Add(new Block(lineH, (gr, y) =>
            {
                gr.DrawString("o", text, black, itemX, y);
                gr.DrawString($"Terms of Payment:      {q.PaymentTerms}", text, black, itemX + w * 0.04f, y);
            }));
        }

        // ----- Cash Payable to -----
        if (q.PayableTo != null)
        {
            Gap(lineH * 1.4f);
            float payX = x + w * 0.135f;
            var first = "Cash Payable to: ";
            float nameX = payX + g.MeasureString(first, text).Width - 4;
            list.Add(new Block(lineH * (q.PayableNumber == null ? 1 : 2), (gr, y) =>
            {
                gr.DrawString(first + q.PayableTo, text, black, payX, y);
                if (q.PayableNumber != null) gr.DrawString($"# {q.PayableNumber}", text, black, nameX + w * 0.12f, y + lineH);
            }));
        }

        // ----- CONFORME BY / VERY TRULY YOURS (kept together) -----
        Gap(lineH * 2);
        list.Add(new Block(lineH * 6, (gr, y) =>
        {
            gr.DrawString("CONFORME BY:", text, black, x, y);
            gr.DrawString("______________________", text, black, x, y + lineH * 2 - 6);
            gr.DrawString("PRINT NAME & SIGNATURE", text, black, x, y + lineH * 2 + 8);
            gr.DrawString("DATE:__________________", text, black, x, y + lineH * 4);
            gr.DrawString("VERY TRULY YOURS,", text, black, col2, y);
            if (q.SignatoryName != null) gr.DrawString(q.SignatoryName, text, black, col2, y + lineH * 2);
            if (q.SignatoryTitle != null) gr.DrawString(q.SignatoryTitle, text, black, col2 + w * 0.03f, y + lineH * 3 + 6);
        }));
        return list;
    }

    /// <summary>A, B ... Z, AA, AB ...</summary>
    static string Letter(int n) => n < 26 ? ((char)('A' + n)).ToString() : Letter(n / 26 - 1) + (char)('A' + n % 26);

    void PrintPage(object? sender, PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        var m = (RectangleF)e.MarginBounds;
        blocks ??= Build(g, m);
        page++;
        float y = m.Top;
        while (next < blocks.Count)
        {
            var b = blocks[next];
            if (y + b.Height > m.Bottom && y > m.Top) break;   // continue on the next page
            b.Draw(g, y);
            y += b.Height;
            next++;
        }
        bool more = next < blocks.Count;
        if (more || page > 1)
            g.DrawString($"{d.Quotation.QuoteNo}  page {page}", small, Brushes.Gray, m.Right, m.Bottom + 12, new StringFormat { Alignment = StringAlignment.Far });

        // ----- not approved yet: a light DRAFT mark across the page -----
        if (d.Quotation.Status is QuotationStatuses.Draft or QuotationStatuses.Waiting or QuotationStatuses.Rejected)
        {
            var state = g.Save();
            g.TranslateTransform(m.Left + m.Width / 2f, m.Top + m.Height / 2f);
            g.RotateTransform(-35);
            using var wm = new Font("Segoe UI", 90, FontStyle.Bold);
            using var faint = new SolidBrush(Color.FromArgb(28, ColorTranslator.FromHtml("#F47A20")));
            g.DrawString("DRAFT", wm, faint, 0, 0, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            g.Restore(state);
        }
        e.HasMorePages = more;
    }

    public void Dispose()
    {
        document.Dispose();
        foreach (var f in new[] { text, label, totalFont, title, small }) f.Dispose();
    }
}
