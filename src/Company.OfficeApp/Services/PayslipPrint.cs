using System.Drawing.Printing;

namespace Company.OfficeApp.Services;

/// <summary>
/// Payslips (Payroll > Print Payslips, Payslip History > Print): one page per employee with days worked, late
/// deductions, overtime, government deductions (if switched on), cash advance with the balance left, and net pay.
/// Plain layout for now. <see cref="SaveAsPdf"/> prints straight to "Microsoft Print to PDF".
/// </summary>
public sealed class PayslipPrint : IDisposable
{
    readonly PrintDocument document = new();
    readonly IReadOnlyList<PayslipData> slips;
    int page;

    public PrintDocument Document => document;

    public PayslipPrint(IReadOnlyList<PayslipData> slips)
    {
        this.slips = slips;
        document.DocumentName = slips.Count == 1 ? $"Payslip {slips[0].Name} {slips[0].Period.PayDate:yyyy-MM-dd}" : $"Payslips {slips.FirstOrDefault()?.Period.PayDate:yyyy-MM-dd}";
        document.BeginPrint += (_, _) => page = 0;
        document.PrintPage += PrintPage;
    }

    public void SaveAsPdf(string path)
    {
        document.PrinterSettings = new PrinterSettings { PrinterName = "Microsoft Print to PDF", PrintToFile = true, PrintFileName = path };
        document.PrintController = new StandardPrintController();
        document.Print();
    }

    void PrintPage(object? sender, PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        var area = e.MarginBounds;
        var d = slips[page];
        var s = d.Slip;
        float x = area.Left, y = area.Top, right = area.Left + 420;
        using var heading = new Font("Segoe UI", 14, FontStyle.Bold);
        using var bold = new Font("Segoe UI", 10, FontStyle.Bold);
        using var text = new Font("Segoe UI", 10);
        using var small = new Font("Segoe UI", 8);
        using var rightAlign = new StringFormat { Alignment = StringAlignment.Far };
        string M(decimal v) => ProjectService.Money(v);

        void Line(string label, string value, Font? f = null)
        {
            g.DrawString(label, f ?? text, Brushes.Black, x, y);
            g.DrawString(value, f ?? text, Brushes.Black, new RectangleF(x, y, right - x, 20), rightAlign);
            y += 21;
        }
        void Rule() { g.DrawLine(Pens.Black, x, y + 2, right, y + 2); y += 8; }

        g.DrawString("Tri-Stone Construction Services", text, Brushes.Black, x, y); y += 20;
        g.DrawString("Payslip", heading, Brushes.Black, x, y); y += 32;
        Line("Employee", d.Name, bold);
        Line("Position / department", $"{d.Position ?? "-"} / {d.Department}");
        Line("Pay period", $"{d.Period.From:MMM d} - {d.Period.To:MMM d, yyyy}");
        Line("Pay date", d.Period.PayDate.ToString("MMMM d, yyyy"));
        Line("Daily rate", M(s.DailyRate));
        Rule();
        Line($"Basic pay ({s.DaysPaid:0.#} day{(s.DaysPaid == 1 ? "" : "s")})", M(s.BasicPay));
        if (s.HolidayPay > 0) Line($"Holiday pay ({s.HolidayDays} day{(s.HolidayDays == 1 ? "" : "s")})", M(s.HolidayPay));
        if (s.OtPay > 0) Line($"Overtime ({s.OtMinutes / 60m:0.##} h)", M(s.OtPay));
        if (s.LateDeduction > 0) Line($"Less late / undertime ({s.LateMinutes} min)", "-" + M(s.LateDeduction));
        Line("Gross pay", M(s.GrossPay), bold);
        Rule();
        if (d.GovOn)
        {
            Line("SSS", "-" + M(s.Sss));
            Line("PhilHealth", "-" + M(s.PhilHealth));
            Line("Pag-IBIG", "-" + M(s.PagIbig));
            Line("Withholding tax", "-" + M(s.Tax));
        }
        if (s.CashAdvance > 0 || d.CashAdvanceBalance > 0)
            Line($"Cash advance (balance left {M(d.CashAdvanceBalance)})", "-" + M(s.CashAdvance));
        Rule();
        Line("NET PAY", M(s.NetPay), heading);
        y += 40;
        g.DrawString("Received by: ______________________________     Date: ______________", text, Brushes.Black, x, y); y += 40;
        g.DrawString($"Printed {DateTime.Now:MMMM d, yyyy h:mm tt} by {Session.CurrentUser?.FullName}", small, Brushes.Gray, x, y);

        page++;
        e.HasMorePages = page < slips.Count;
    }

    public void Dispose() => document.Dispose();
}
