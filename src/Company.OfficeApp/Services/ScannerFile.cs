using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ExcelDataReader;

namespace Company.OfficeApp.Services;

/// <summary>One person in the scanner file: the scanner's ID for them and every scan time.</summary>
public record ScannerPerson(string ScannerId, string Name, string Dept, List<DateTime> Scans);

/// <summary>What was read from a scanner export.</summary>
public record ScannerFileData(DateOnly PeriodFrom, DateOnly PeriodTo, List<ScannerPerson> People);

/// <summary>
/// Reads the fingerprint scanner's USB export: "All Report.xls" (or an "Individual Report"), sheet
/// "Attendance Logs". Layout (sample received 2026-10-30): row 2 holds the range "01/10/2026 ~ 30/10/2026"
/// (day/month/year); then one block per person: a row "ID | 1 | Name | Jun | Dept. | Foreman", a row of day
/// numbers (1..30), a row of weekdays (Thu, Fri...), then the scan times, each under its day. One cell can hold
/// several times ("07:58 12:01" or one per line), so every hh:mm in a cell counts.
/// Also reads .xlsx and .csv files with the same layout.
/// </summary>
public static partial class ScannerFile
{
    [GeneratedRegex(@"(\d{1,2})/(\d{1,2})/(\d{4})\s*~\s*(\d{1,2})/(\d{1,2})/(\d{4})")]
    private static partial Regex RangePattern();

    [GeneratedRegex(@"\b(\d{1,2}):(\d{2})(?::\d{2})?\b")]
    private static partial Regex TimePattern();

    static readonly string[] Weekdays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

    /// <summary>Reads the file. Throws <see cref="FormatException"/> with a plain message when it isn't a scanner export.</summary>
    public static ScannerFileData Read(string path)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);   // old .xls files use Windows code pages
        var rows = ReadLogSheet(path);
        if (rows == null) throw new FormatException("This file has no \"Attendance Logs\" sheet. Choose the \"All Report\" file exported from the scanner.");
        return Parse(rows);
    }

    /// <summary>The rows of the "Attendance Logs" sheet as text cells (null when there is no such sheet).</summary>
    static List<string[]>? ReadLogSheet(string path)
    {
        using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = Path.GetExtension(path).Equals(".csv", StringComparison.OrdinalIgnoreCase)
            ? ExcelReaderFactory.CreateCsvReader(stream)
            : ExcelReaderFactory.CreateReader(stream);
        do
        {
            var rows = new List<string[]>();
            while (reader.Read())
            {
                var cells = new string[reader.FieldCount];
                for (int i = 0; i < cells.Length; i++) cells[i] = Text(reader.GetValue(i));
                rows.Add(cells);
            }
            bool named = reader.Name?.Contains("Attendance Logs", StringComparison.OrdinalIgnoreCase) == true;
            bool titled = rows.Count > 0 && rows[0].Any(c => c.Contains("Attendance Logs", StringComparison.OrdinalIgnoreCase));
            if (named || titled) return rows;
        } while (reader.NextResult());
        return null;
    }

    // Excel may hand over a time as text ("07:58"), as a DateTime, or as a fraction of a day (0.3319...).
    static string Text(object? v) => v switch
    {
        null => "",
        DateTime d => d.ToString("HH:mm", CultureInfo.InvariantCulture),
        double f when f is > 0 and < 1 => TimeSpan.FromDays(f).ToString(@"hh\:mm", CultureInfo.InvariantCulture),
        double n => n.ToString(CultureInfo.InvariantCulture),
        _ => v.ToString()?.Trim() ?? "",
    };

    /// <summary>Turns the sheet's rows into people and scan times (separate so tests can feed rows directly).</summary>
    public static ScannerFileData Parse(List<string[]> rows)
    {
        var rangeMatch = rows.SelectMany(r => r).Select(c => RangePattern().Match(c)).FirstOrDefault(m => m.Success)
            ?? throw new FormatException("The date range (e.g. 01/10/2026 ~ 30/10/2026) wasn't found in the file.");
        var firstWeekday = rows.Select(r => r.FirstOrDefault(c => Weekdays.Contains(c))).FirstOrDefault(c => c != null);
        var (from, to) = ReadRange(rangeMatch, firstWeekday);
        var dates = new List<DateOnly>();
        for (var d = from; d <= to; d = d.AddDays(1)) dates.Add(d);

        var people = new List<ScannerPerson>();
        for (int r = 0; r < rows.Count; r++)
        {
            if (!IsIdRow(rows[r])) continue;
            var head = rows[r];
            string After(string label)
            {
                int i = Array.FindIndex(head, c => c.Equals(label, StringComparison.OrdinalIgnoreCase));
                return i < 0 ? "" : head.Skip(i + 1).FirstOrDefault(c => c != "") ?? "";
            }
            var person = new ScannerPerson(After("ID"), After("Name"), After("Dept."), []);
            if (r + 1 >= rows.Count) break;

            // Day-number row: which column holds which date.
            var dayRow = rows[r + 1];
            var columnDate = new Dictionary<int, DateOnly>();
            int next = 0;
            for (int c = 0; c < dayRow.Length; c++)
            {
                if (!int.TryParse(dayRow[c], out var dayNo)) continue;
                if (next >= dates.Count || dates[next].Day != dayNo)
                    throw new FormatException($"The day numbers for ID {person.ScannerId} don't match the date range {from:dd/MM/yyyy} ~ {to:dd/MM/yyyy}.");
                columnDate[c] = dates[next++];
            }

            // Time rows: everything after the weekday row until the next person.
            for (int t = r + 3; t < rows.Count && !IsIdRow(rows[t]); t++)
                for (int c = 0; c < rows[t].Length; c++)
                {
                    if (!columnDate.TryGetValue(c, out var date)) continue;
                    foreach (Match m in TimePattern().Matches(rows[t][c]))
                    {
                        int h = int.Parse(m.Groups[1].Value), min = int.Parse(m.Groups[2].Value);
                        if (h < 24 && min < 60) person.Scans.Add(date.ToDateTime(new TimeOnly(h, min)));
                    }
                }
            person.Scans.Sort();
            people.Add(person);
        }
        if (people.Count == 0) throw new FormatException("No employees were found in the file (no \"ID\" rows).");
        return new ScannerFileData(from, to, people);
    }

    static bool IsIdRow(string[] row) => row.Length > 0 && row[0].Equals("ID", StringComparison.OrdinalIgnoreCase);

    // The sample is day/month/year. When both readings are possible, the first weekday printed in the file decides.
    static (DateOnly, DateOnly) ReadRange(Match m, string? firstWeekday)
    {
        int[] n = [.. Enumerable.Range(1, 6).Select(i => int.Parse(m.Groups[i].Value))];
        DateOnly? Make(int day, int month, int year) => month is >= 1 and <= 12 && day >= 1 && day <= DateTime.DaysInMonth(year, month) ? new DateOnly(year, month, day) : null;
        var dayFirst = (Make(n[0], n[1], n[2]), Make(n[3], n[4], n[5]));
        var monthFirst = (Make(n[1], n[0], n[2]), Make(n[4], n[3], n[5]));
        bool Fits((DateOnly? A, DateOnly? B) r) => r.A != null && r.B != null && r.A <= r.B
            && (firstWeekday == null || r.A.Value.DayOfWeek.ToString()[..3] == firstWeekday);
        if (Fits(dayFirst)) return (dayFirst.Item1!.Value, dayFirst.Item2!.Value);
        if (Fits(monthFirst)) return (monthFirst.Item1!.Value, monthFirst.Item2!.Value);
        throw new FormatException($"The date range \"{m.Value}\" couldn't be read.");
    }
}
