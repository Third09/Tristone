using ClosedXML.Excel;

namespace Company.OfficeApp.Services;

/// <summary>Saves a list as an .xlsx file (ClosedXML). Used by every page's Export button.</summary>
public static class ExcelExport
{
    public record Column<T>(string Header, Func<T, object?> Value, string? NumberFormat = null);

    public static void Save<T>(string path, string sheetName, IEnumerable<T> rows, params Column<T>[] columns)
    {
        using var book = new XLWorkbook();
        var sheet = book.Worksheets.Add(sheetName);

        for (int c = 0; c < columns.Length; c++)
            sheet.Cell(1, c + 1).Value = columns[c].Header;
        sheet.Row(1).Style.Font.Bold = true;

        int r = 2;
        foreach (var row in rows)
        {
            for (int c = 0; c < columns.Length; c++)
            {
                var cell = sheet.Cell(r, c + 1);
                cell.Value = columns[c].Value(row) switch
                {
                    null => Blank.Value,
                    decimal d => d,
                    int i => i,
                    bool b => b ? "Yes" : "No",
                    DateOnly d => d.ToDateTime(TimeOnly.MinValue),
                    DateTime dt => dt,
                    var other => other.ToString(),
                };
                if (columns[c].NumberFormat is { } fmt) cell.Style.NumberFormat.Format = fmt;
            }
            r++;
        }

        sheet.SheetView.FreezeRows(1);
        sheet.Columns().AdjustToContents();
        book.SaveAs(path);
    }

    /// <summary>Asks where to save. Returns the chosen path, or null if cancelled.</summary>
    public static string? AskPath(IWin32Window owner, string suggestedName)
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "Excel workbook (*.xlsx)|*.xlsx",
            FileName = $"{suggestedName} {DateTime.Now:yyyy-MM-dd}.xlsx",
        };
        return dlg.ShowDialog(owner) == DialogResult.OK ? dlg.FileName : null;
    }
}
