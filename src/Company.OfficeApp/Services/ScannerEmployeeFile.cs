using System.Globalization;
using System.Text;
using ExcelDataReader;
using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Company.OfficeApp.Services;

/// <summary>What making the scanner's employee file did.</summary>
public record ScannerExportResult(int Employees, List<string> Skipped);

/// <summary>
/// Makes the fingerprint scanner's "Employee Form.xls" from the employee list, so HR doesn't type names on the
/// scanner. The scanner's own form (sample received 2026-10-30) is kept as a blank template in
/// Resources/ScannerEmployeeForm.xls; only its "Employee Form" sheet is filled: from row 9, *ID (= Biometric ID),
/// *Name, *Department (a number 1-16) and *Authority (0 = employee). The schedule columns stay empty, like the
/// scanner's own export; the app works out attendance itself. The "Attendance Rules Form" sheet is left as it was.
/// Fingerprints are still enrolled on the scanner, one person at a time.
/// </summary>
public static class ScannerEmployeeFile
{
    const string TemplateResource = "Company.OfficeApp.Resources.ScannerEmployeeForm.xls";
    const string SheetName = "Employee Form";
    const int FirstDataRow = 8;      // row 9 in Excel
    const int DateRow = 5, DateColumn = 4;   // E6: the date the schedule columns start from

    /// <summary>
    /// The scanner's department number for each of our departments (the scanner only knows numbers 1-16).
    /// 1 is the scanner's "ConstructionCrew" slot; 2 (Foreman) and 10 (ITDept) are already used on the scanner.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, int> DepartmentNumbers = new Dictionary<string, int>
    {
        [EmployeeDepartments.Site] = 1,
        [EmployeeDepartments.Administration] = 3,
        [Departments.HrPayroll] = 4,
        [Departments.Inventory] = 5,
        [Departments.Subcontractors] = 6,
        [Departments.Receipts] = 7,
    };

    /// <summary>Writes the file for every active employee who has a numeric Biometric ID.</summary>
    public static async Task<ScannerExportResult> SaveAsync(string path)
    {
        if (Access.RequirePage(AppPage.HrPayroll) is { } denied) throw new InvalidOperationException(denied);
        using var db = Db.Open();
        var employees = await db.Employees.AsNoTracking().Where(e => e.Status == EmployeeStatuses.Active)
            .OrderBy(e => e.FullName).Select(e => new { e.FullName, e.BiometricId, e.Department }).ToListAsync();

        var skipped = new List<string>();
        var rows = new List<(long Id, string Name, int Dept)>();
        foreach (var e in employees)
        {
            if (string.IsNullOrWhiteSpace(e.BiometricId)) { skipped.Add($"{e.FullName}: no Biometric ID yet."); continue; }
            if (!long.TryParse(e.BiometricId.Trim(), out var id) || id <= 0) { skipped.Add($"{e.FullName}: Biometric ID \"{e.BiometricId}\" isn't a number the scanner accepts."); continue; }
            rows.Add((id, e.FullName, DepartmentNumbers.GetValueOrDefault(e.Department, 1)));
        }
        Write(path, rows.OrderBy(r => r.Id).ToList(), DateOnly.FromDateTime(DateTime.Today));
        return new ScannerExportResult(rows.Count, skipped);
    }

    /// <summary>
    /// Writes the scanner's form with these people (separate so tests can call it without the database).
    /// The scanner writes its own file in a way Excel accepts but NPOI can't open, so the template is read with
    /// ExcelDataReader and written again as a normal .xls (the same as opening it in Excel and saving, which the
    /// scanner's instructions expect): every value and merged cell of both sheets is copied, the day/weekday
    /// headings are filled in for <paramref name="date"/>, and the people go from row 9 down.
    /// </summary>
    public static void Write(string path, IReadOnlyList<(long Id, string Name, int Dept)> rows, DateOnly date)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var template = typeof(ScannerEmployeeFile).Assembly.GetManifestResourceStream(TemplateResource)
            ?? throw new InvalidOperationException("The scanner template is missing from the app.");
        using var reader = ExcelReaderFactory.CreateReader(template);
        var book = new HSSFWorkbook();
        do
        {
            var sheet = book.CreateSheet(reader.Name);
            bool employees = reader.Name == SheetName;
            int r = 0;
            while (reader.Read())
            {
                if (!(employees && r >= FirstDataRow))
                    for (int c = 0; c < reader.FieldCount; c++)
                        switch (reader.GetValue(c))
                        {
                            case double d: Cell(sheet, r, c).SetCellValue(d); break;
                            case string s when s != "": Cell(sheet, r, c).SetCellValue(s); break;
                            case DateTime t: Cell(sheet, r, c).SetCellValue(t.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)); break;
                            case bool b: Cell(sheet, r, c).SetCellValue(b); break;
                        }
                r++;
            }
            foreach (var m in reader.MergeCells ?? [])
                if (!(employees && m.FromRow >= FirstDataRow))
                    sheet.AddMergedRegion(new CellRangeAddress(m.FromRow, m.ToRow, m.FromColumn, m.ToColumn));

            if (!employees) continue;
            // E6 = the start date ("yyyy-MM-dd" text, as the scanner writes it); row 7 = day numbers, row 8 = weekdays.
            Cell(sheet, DateRow, DateColumn).SetCellValue(date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            for (int i = 0; i < Days; i++)
            {
                var day = date.AddDays(i);
                Cell(sheet, DateRow + 1, DateColumn + i).SetCellValue(day.Day);
                Cell(sheet, DateRow + 2, DateColumn + i).SetCellValue(day.DayOfWeek.ToString()[..3]);
            }
            for (int i = 0; i < rows.Count; i++)
            {
                Cell(sheet, FirstDataRow + i, 0).SetCellValue(rows[i].Id);
                Cell(sheet, FirstDataRow + i, 1).SetCellValue(rows[i].Name.Trim());
                Cell(sheet, FirstDataRow + i, 2).SetCellValue(rows[i].Dept);
                Cell(sheet, FirstDataRow + i, 3).SetCellValue(0);   // Authority: 0 = employee (scanner admins are set on the scanner)
            }
        } while (reader.NextResult());

        using var output = File.Create(path);
        book.Write(output);
    }

    const int Days = 31;   // the form's schedule columns E..AI

    static ICell Cell(ISheet sheet, int row, int col) => (sheet.GetRow(row) ?? sheet.CreateRow(row)).GetCell(col) ?? sheet.GetRow(row).CreateCell(col);
}
