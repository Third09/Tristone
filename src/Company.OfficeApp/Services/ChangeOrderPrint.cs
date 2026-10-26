using System.Drawing.Printing;

namespace Company.OfficeApp.Services;

/// <summary>
/// The printable change order form for the client to sign (Change Orders > Print Form).
/// Plain text layout for now. To save it as PDF, choose the "Microsoft Print to PDF" printer.
/// </summary>
public sealed class ChangeOrderPrint : IDisposable
{
    readonly PrintDocument document = new();
    readonly CoPrintData c;

    public PrintDocument Document => document;

    public ChangeOrderPrint(CoPrintData data)
    {
        c = data;
        document.DocumentName = $"Change Order {c.CoNo}";
        document.PrintPage += PrintPage;
    }

    void PrintPage(object? sender, PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        float x = e.MarginBounds.Left, y = e.MarginBounds.Top, w = e.MarginBounds.Width;
        using var heading = new Font("Segoe UI", 16, FontStyle.Bold);
        using var label = new Font("Segoe UI", 10, FontStyle.Bold);
        using var text = new Font("Segoe UI", 10);
        using var small = new Font("Segoe UI", 8);
        static string Signed(decimal v) => (v >= 0 ? "+" : "-") + ProjectService.Money(Math.Abs(v));
        static string Date(DateOnly? d) => d?.ToString("MMMM d, yyyy") ?? "(no target end date)";

        void Line(string l, string value)
        {
            g.DrawString(l, label, Brushes.Black, x, y);
            var box = new RectangleF(x + 210, y, w - 210, 300);
            var size = g.MeasureString(value, text, (int)box.Width);
            g.DrawString(value, text, Brushes.Black, box);
            y += Math.Max(24, size.Height + 6);
        }

        g.DrawString("Tri-Stone Construction Services", text, Brushes.Black, x, y); y += 22;
        g.DrawString($"Change Order {c.CoNo}", heading, Brushes.Black, x, y); y += 44;
        Line("Project", c.Project);
        Line("Client", c.Client ?? "-");
        Line("Date requested", c.RequestedOn.ToString("MMMM d, yyyy"));
        Line("Status", c.Status);
        Line("Change requested", c.Description);
        y += 10;
        Line("Cost change", Signed(c.CostChange));
        Line("Days change", $"{(c.DaysChange >= 0 ? "+" : "")}{c.DaysChange} days");
        Line("Contract price", $"{ProjectService.Money(c.Preview.CurrentPrice)}  ->  {ProjectService.Money(c.Preview.RevisedPrice)}");
        Line("Target end date", $"{Date(c.Preview.CurrentEnd)}  ->  {Date(c.Preview.RevisedEnd)}");
        y += 10;
        Line("Prepared by", c.PreparedBy);
        Line("Approved by", c.ApprovedBy ?? "(not approved yet)");

        y += 50;
        g.DrawString("Client conforme: ______________________________     Date: ______________", text, Brushes.Black, x, y); y += 50;
        g.DrawString("For Tri-Stone:   ______________________________     Date: ______________", text, Brushes.Black, x, y); y += 40;
        g.DrawString($"Printed {DateTime.Now:MMMM d, yyyy h:mm tt} by {Session.CurrentUser?.FullName}", small, Brushes.Gray, x, y);
        e.HasMorePages = false;
    }

    public void Dispose() => document.Dispose();
}
