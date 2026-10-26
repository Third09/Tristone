using System.Drawing.Printing;

namespace Company.OfficeApp.Services;

/// <summary>
/// The printable subcontractor billing voucher (Billing & Payouts > Print Voucher), with the subcontractor's bank
/// details. Plain text layout for now. To save it as PDF, choose the "Microsoft Print to PDF" printer.
/// </summary>
public sealed class VoucherPrint : IDisposable
{
    readonly PrintDocument document = new();
    readonly VoucherPrintData v;

    public PrintDocument Document => document;

    public VoucherPrint(VoucherPrintData data)
    {
        v = data;
        document.DocumentName = $"Billing Voucher {v.VoucherNo}";
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

        void Line(string l, string value)
        {
            g.DrawString(l, label, Brushes.Black, x, y);
            var box = new RectangleF(x + 200, y, w - 200, 400);
            var size = g.MeasureString(value, text, (int)box.Width);
            g.DrawString(value, text, Brushes.Black, box);
            y += Math.Max(24, size.Height + 6);
        }

        g.DrawString("Tri-Stone Construction Services", text, Brushes.Black, x, y); y += 22;
        g.DrawString($"Subcontractor Billing Voucher {v.VoucherNo}", heading, Brushes.Black, x, y); y += 44;

        Line("Status", v.Status);
        Line("Date prepared", v.PreparedAt.ToString("MMMM d, yyyy"));
        Line("Subcontractor", v.Subcontractor);
        Line("Project", v.Project);
        Line("Scope of work", v.Scope);
        Line("For", v.For);
        y += 10;
        Line("Amount", ProjectService.Money(v.Gross));
        Line("Less retention", ProjectService.Money(v.Retention));
        Line("Amount payable", ProjectService.Money(v.Payable));
        y += 10;
        Line("Bank", v.BankName ?? "-");
        Line("Account name", v.AccountName ?? "-");
        Line("Account number", v.AccountNumber ?? "-");
        y += 10;
        Line("Prepared by", v.PreparedBy);
        Line("Approved by", v.ApprovedBy ?? "(not approved yet)");

        y += 40;
        g.DrawString("Received by: ______________________________     Date: ______________", text, Brushes.Black, x, y); y += 40;
        g.DrawString($"Printed {DateTime.Now:MMMM d, yyyy h:mm tt} by {Session.CurrentUser?.FullName}", small, Brushes.Gray, x, y);
        e.HasMorePages = false;
    }

    public void Dispose() => document.Dispose();
}
