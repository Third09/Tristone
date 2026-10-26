using System.Drawing.Printing;
using Company.Shared.Models;

namespace Company.OfficeApp.Services;

/// <summary>
/// The ready-made printable project report (Projects page > Print Report). Plain text layout for now.
/// To save it as PDF, choose the "Microsoft Print to PDF" printer.
/// </summary>
public sealed class ProjectReport : IDisposable
{
    readonly PrintDocument document = new();
    readonly List<(string Label, string Value)> lines;
    readonly string title;

    public PrintDocument Document => document;

    public ProjectReport(Project p, ProjectSummary s)
    {
        title = p.ProjectName;
        document.DocumentName = $"Project Report - {p.ProjectName}";
        lines =
        [
            ("Project", p.ProjectName),
            ("Site / location", p.SiteLocation),
            ("Client", p.ClientName ?? "-"),
            ("Sector", p.Sector),
            ("Status", p.Status),
            ("Start date", p.StartDate.ToString("MMMM d, yyyy")),
            ("Target end date", p.TargetEndDate?.ToString("MMMM d, yyyy") ?? "-"),
            ("Completed", p.CompletedDate?.ToString("MMMM d, yyyy") ?? "-"),
            ("Days", s.DaysText),
            ("", ""),
            ("Contract price (revenue)", ProjectService.Money(s.Revenue)),
            ("Expenses to date", ProjectService.Money(s.Expenses)),
            ("Profit to date", ProjectService.Money(s.Profit)),
        ];
        document.PrintPage += PrintPage;
    }

    void PrintPage(object? sender, PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        float x = e.MarginBounds.Left, y = e.MarginBounds.Top;
        using var heading = new Font("Segoe UI", 16, FontStyle.Bold);
        using var label = new Font("Segoe UI", 10, FontStyle.Bold);
        using var text = new Font("Segoe UI", 10);
        using var small = new Font("Segoe UI", 8);

        g.DrawString("Tri-Stone Construction Services", text, Brushes.Black, x, y); y += 22;
        g.DrawString($"Project Report: {title}", heading, Brushes.Black, x, y); y += 44;

        foreach (var (l, v) in lines)
        {
            if (l != "") { g.DrawString(l, label, Brushes.Black, x, y); g.DrawString(v, text, Brushes.Black, x + 200, y); }
            y += 24;
        }

        y += 20;
        g.DrawString($"Printed {DateTime.Now:MMMM d, yyyy h:mm tt} by {Session.CurrentUser?.FullName}", small, Brushes.Gray, x, y);
        e.HasMorePages = false;
    }

    public void Dispose() => document.Dispose();
}
