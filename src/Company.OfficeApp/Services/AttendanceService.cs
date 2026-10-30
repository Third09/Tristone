using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>What an upload did, for the summary line and "View Rejected Rows".</summary>
public record ImportResult(DateOnly PeriodFrom, DateOnly PeriodTo, int ScansAdded, int ScansSkipped, List<string> Problems)
{
    public string Summary => $"{PeriodFrom:MMM d} - {PeriodTo:MMM d, yyyy}: {ScansAdded} scan{(ScansAdded == 1 ? "" : "s")} imported"
        + (ScansSkipped > 0 ? $", {ScansSkipped} already there" : "")
        + (Problems.Count > 0 ? $", {Problems.Count} row{(Problems.Count == 1 ? "" : "s")} rejected" : "");
}

/// <summary>One line in the Attendance tab.</summary>
public record AttendanceRow(int DayId, int EmployeeId, DateOnly Date, string Name, TimeOnly? TimeIn, TimeOnly? BreakOut, TimeOnly? BreakIn,
                            TimeOnly? TimeOut, int LateMinutes, int EarlyMinutes, int OtMinutes, string OtStatus, string Status, string? FixReason)
{
    public string Day => Date.ToString("ddd");
    public decimal OtHours => Math.Round(OtMinutes / 60m, 2);
    public string Overtime => OtMinutes == 0 ? "" : $"{OtHours:0.##} h ({OtStatus})";
    public string Fixed => FixReason == null ? "" : "Fixed";
    public bool IsProblem => Status is AttendanceStatuses.Absent or AttendanceStatuses.MissingScan || OtStatus == OtStatuses.Waiting;
}

/// <summary>
/// HR & Payroll > Attendance (Project Outline, Part 2). Upload the scanner file; every scan is stored once
/// (uploading the same file again adds nothing), then each employee's days are worked out with
/// <see cref="AttendanceRules"/>: Present, Late, Missing Scan, Absent (a work day with no scans) or Rest Day Work.
/// HR fixes missing scans by hand with a reason (kept in Edit History); overtime is paid only after the HR Supervisor
/// (or an Admin) approves it. Scans are matched to employees by the Biometric ID set in Employees.
/// </summary>
public static class AttendanceService
{
    const string Dept = Departments.HrPayroll;
    public const string FilterAll = "All", FilterLate = "Late", FilterAbsent = "Absent", FilterMissing = "Missing Scan",
                        FilterOt = "Overtime Waiting Approval", FilterFixed = "Fixed by HR";
    public static readonly string[] Filters = [FilterAll, FilterLate, FilterAbsent, FilterMissing, FilterOt, FilterFixed];

    // "001" on the scanner and "1" in Employees are the same person.
    static string Key(string id) => id.Trim().TrimStart('0') is { Length: > 0 } k ? k : "0";

    public static async Task<ImportResult> ImportAsync(string path)
    {
        if (Access.RequirePage(AppPage.HrPayroll) is { } denied) throw new InvalidOperationException(denied);
        var file = ScannerFile.Read(path);   // FormatException with a plain message if it isn't a scanner file
        using var db = Db.Open();

        var byScannerId = (await db.Employees.AsNoTracking().Where(e => e.BiometricId != null && e.BiometricId != "")
                .Select(e => new { e.EmployeeId, e.BiometricId, e.FullName }).ToListAsync())
            .GroupBy(e => Key(e.BiometricId!)).ToDictionary(g => g.Key, g => g.First());

        var problems = new List<string>();
        var scans = new List<(int EmployeeId, DateTime Time)>();
        foreach (var p in file.People)
        {
            if (byScannerId.TryGetValue(Key(p.ScannerId), out var emp))
                scans.AddRange(p.Scans.Select(t => (emp.EmployeeId, t)));
            else if (p.Scans.Count > 0)
                problems.Add($"Scanner ID {p.ScannerId} ({p.Name}, {p.Dept}): {p.Scans.Count} scan(s) skipped, because no employee has Biometric ID {p.ScannerId}. Set it in Employees, then upload the file again.");
        }

        var start = file.PeriodFrom.ToDateTime(TimeOnly.MinValue);
        var end = file.PeriodTo.AddDays(1).ToDateTime(TimeOnly.MinValue);
        var ids = scans.Select(s => s.EmployeeId).Distinct().ToList();
        var existing = (await db.AttendanceScans.Where(s => ids.Contains(s.EmployeeId) && s.ScanTime >= start && s.ScanTime < end)
            .Select(s => new { s.EmployeeId, s.ScanTime }).ToListAsync()).Select(s => (s.EmployeeId, s.ScanTime)).ToHashSet();
        var fresh = scans.Distinct().Where(s => !existing.Contains(s)).ToList();

        var import = new AttendanceImport
        {
            FileName = Path.GetFileName(path), PeriodFrom = file.PeriodFrom, PeriodTo = file.PeriodTo, ScansAdded = fresh.Count,
            ScansSkipped = scans.Count - fresh.Count, RowsRejected = problems.Count,
            Problems = problems.Count == 0 ? null : string.Join("\n", problems), ImportedBy = Session.CurrentUser!.UserId,
        };
        db.AttendanceImports.Add(import);
        await db.SaveChangesAsync();
        db.AttendanceScans.AddRange(fresh.Select(s => new AttendanceScan { EmployeeId = s.EmployeeId, ScanTime = s.Time, ImportId = import.ImportId }));
        AuditService.Add(db, Dept, "attendance_imports", import.ImportId, import.FileName, AuditActions.Added, null,
            new { period = $"{file.PeriodFrom:yyyy-MM-dd} to {file.PeriodTo:yyyy-MM-dd}", scans_added = import.ScansAdded, scans_skipped = import.ScansSkipped, rows_rejected = import.RowsRejected });
        await db.SaveChangesAsync();

        await RecomputeAsync(db, file.PeriodFrom, file.PeriodTo);
        return new ImportResult(file.PeriodFrom, file.PeriodTo, import.ScansAdded, import.ScansSkipped, problems);
    }

    /// <summary>The last upload (for the summary line when the tab opens).</summary>
    public static async Task<(string Summary, List<string> Problems)?> LastImportAsync()
    {
        using var db = Db.Open();
        var i = await db.AttendanceImports.AsNoTracking().OrderByDescending(x => x.ImportId).FirstOrDefaultAsync();
        if (i == null) return null;
        var problems = i.Problems?.Split('\n').ToList() ?? [];
        var r = new ImportResult(i.PeriodFrom, i.PeriodTo, i.ScansAdded, i.ScansSkipped, problems);
        return ($"Last upload {i.ImportedAt:MMM d, h:mm tt} ({i.FileName}), {r.Summary}", problems);
    }

    /// <summary>
    /// Works out every day in the range for every active employee who has a Biometric ID: days with scans from the
    /// scans, work days without scans (up to yesterday, from the employee's first rate date) as Absent.
    /// Days HR fixed by hand are left as they are.
    /// </summary>
    static async Task RecomputeAsync(AppDbContext db, DateOnly from, DateOnly to)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        if (to > today) to = today;
        var employees = await db.Employees.AsNoTracking()
            .Where(e => e.Status == EmployeeStatuses.Active && e.BiometricId != null && e.BiometricId != "")
            .Select(e => new { e.EmployeeId, e.CreatedAt, FirstRate = db.EmployeeRates.Where(r => r.EmployeeId == e.EmployeeId).Min(r => (DateOnly?)r.EffectiveFrom) })
            .ToListAsync();
        var ids = employees.Select(e => e.EmployeeId).ToList();
        var start = from.ToDateTime(TimeOnly.MinValue);
        var end = to.AddDays(1).ToDateTime(TimeOnly.MinValue);
        var scans = (await db.AttendanceScans.AsNoTracking().Where(s => ids.Contains(s.EmployeeId) && s.ScanTime >= start && s.ScanTime < end).ToListAsync())
            .GroupBy(s => (s.EmployeeId, DateOnly.FromDateTime(s.ScanTime)))
            .ToDictionary(g => g.Key, g => g.Select(s => TimeOnly.FromDateTime(s.ScanTime)).ToList());
        var days = (await db.AttendanceDays.Where(d => ids.Contains(d.EmployeeId) && d.WorkDate >= from && d.WorkDate <= to).ToListAsync())
            .ToDictionary(d => (d.EmployeeId, d.WorkDate));

        foreach (var e in employees)
        {
            var employedFrom = e.FirstRate ?? DateOnly.FromDateTime(e.CreatedAt);
            for (var date = from; date <= to; date = date.AddDays(1))
            {
                days.TryGetValue((e.EmployeeId, date), out var row);
                if (row?.FixedBy != null) continue;   // HR's fix stays
                DayResult result;
                if (scans.TryGetValue((e.EmployeeId, date), out var times)) result = AttendanceRules.FromScans(date, times);
                else if (AttendanceRules.IsWorkDay(date) && date < today && date >= employedFrom) result = AttendanceRules.FromTimes(date, null, null, null, null);
                else { if (row != null) db.AttendanceDays.Remove(row); continue; }

                if (row == null) { row = new AttendanceDay { EmployeeId = e.EmployeeId, WorkDate = date }; db.AttendanceDays.Add(row); }
                Apply(row, result);
            }
        }
        await db.SaveChangesAsync();
    }

    static void Apply(AttendanceDay row, DayResult r)
    {
        bool otChanged = row.OtMinutes != r.OtMinutes;
        row.TimeIn = r.TimeIn; row.BreakOut = r.BreakOut; row.BreakIn = r.BreakIn; row.TimeOut = r.TimeOut;
        row.LateMinutes = r.LateMinutes; row.EarlyMinutes = r.EarlyMinutes; row.OtMinutes = r.OtMinutes; row.Status = r.Status;
        if (otChanged || row.DayId == 0)
        {
            row.OtStatus = r.OtMinutes > 0 ? OtStatuses.Waiting : OtStatuses.None;
            row.OtDecidedBy = null; row.OtDecidedAt = null;
        }
        row.UpdatedAt = DateTime.Now;
    }

    public static async Task<List<AttendanceRow>> ListAsync(DateOnly fromDate, DateOnly toDate, string filter, string search)
    {
        using var db = Db.Open();
        var q = from d in db.AttendanceDays
                join e in db.Employees on d.EmployeeId equals e.EmployeeId
                where d.WorkDate >= fromDate && d.WorkDate <= toDate
                select new { d, e.FullName };
        q = filter switch
        {
            FilterLate => q.Where(x => x.d.LateMinutes > 0 || x.d.EarlyMinutes > 0),
            FilterAbsent => q.Where(x => x.d.Status == AttendanceStatuses.Absent),
            FilterMissing => q.Where(x => x.d.Status == AttendanceStatuses.MissingScan),
            FilterOt => q.Where(x => x.d.OtStatus == OtStatuses.Waiting),
            FilterFixed => q.Where(x => x.d.FixedBy != null),
            _ => q,
        };
        if (!string.IsNullOrWhiteSpace(search)) { var s = search.Trim().ToLower(); q = q.Where(x => x.FullName.ToLower().Contains(s)); }
        var list = await q.OrderByDescending(x => x.d.WorkDate).ThenBy(x => x.FullName).ToListAsync();
        return list.Select(x => new AttendanceRow(x.d.DayId, x.d.EmployeeId, x.d.WorkDate, x.FullName, x.d.TimeIn, x.d.BreakOut, x.d.BreakIn,
            x.d.TimeOut, x.d.LateMinutes, x.d.EarlyMinutes, x.d.OtMinutes, x.d.OtStatus, x.d.Status, x.d.FixReason)).ToList();
    }

    public static async Task<AttendanceDay?> GetAsync(int dayId)
    {
        using var db = Db.Open();
        return await db.AttendanceDays.AsNoTracking().FirstOrDefaultAsync(d => d.DayId == dayId);
    }

    public static string? CheckFix(TimeOnly? timeIn, TimeOnly? breakOut, TimeOnly? breakIn, TimeOnly? timeOut, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason)) return "Type the reason for the fix (e.g. forgot to scan after lunch, confirmed by the foreman).";
        var set = new[] { timeIn, breakOut, breakIn, timeOut }.Where(t => t != null).Select(t => t!.Value).ToList();
        if (set.Count == 0) return "Tick at least the time in, or leave the day as Absent.";
        for (int i = 1; i < set.Count; i++) if (set[i] <= set[i - 1]) return "The times must go in order: in, break out, break in, out.";
        return null;
    }

    /// <summary>HR sets the day's times by hand (a forgotten scan). The reason and who did it are kept.</summary>
    public static async Task<string?> FixAsync(int dayId, TimeOnly? timeIn, TimeOnly? breakOut, TimeOnly? breakIn, TimeOnly? timeOut, string reason)
    {
        if (Access.RequirePage(AppPage.HrPayroll) is { } denied) return denied;
        if (CheckFix(timeIn, breakOut, breakIn, timeOut, reason) is { } problem) return problem;
        using var db = Db.Open();
        var d = await db.AttendanceDays.FirstAsync(x => x.DayId == dayId);
        var name = await db.Employees.Where(e => e.EmployeeId == d.EmployeeId).Select(e => e.FullName).FirstAsync();
        var before = Snapshot(d);
        Apply(d, AttendanceRules.FromTimes(d.WorkDate, timeIn, breakOut, breakIn, timeOut));
        d.FixReason = reason.Trim();
        d.FixedBy = Session.CurrentUser!.UserId;
        d.FixedAt = DateTime.Now;
        AuditService.Add(db, Dept, "attendance_days", d.DayId, $"{name} {d.WorkDate:MMM d, yyyy}", AuditActions.Edited, before, Snapshot(d));
        await db.SaveChangesAsync();
        return null;
    }

    public static Task<string?> ApproveOvertimeAsync(int dayId) => DecideOvertimeAsync(dayId, OtStatuses.Approved);

    public static Task<string?> RejectOvertimeAsync(int dayId) => DecideOvertimeAsync(dayId, OtStatuses.Rejected);

    static async Task<string?> DecideOvertimeAsync(int dayId, string decision)
    {
        if (Access.RequireApprover(Dept) is { } denied) return denied;
        using var db = Db.Open();
        var d = await db.AttendanceDays.FirstAsync(x => x.DayId == dayId);
        if (d.OtStatus != OtStatuses.Waiting) return d.OtMinutes == 0 ? "This day has no overtime." : $"This overtime is already {d.OtStatus}.";
        var name = await db.Employees.Where(e => e.EmployeeId == d.EmployeeId).Select(e => e.FullName).FirstAsync();
        d.OtStatus = decision;
        d.OtDecidedBy = Session.CurrentUser!.UserId;
        d.OtDecidedAt = DateTime.Now;
        AuditService.Add(db, Dept, "attendance_days", d.DayId, $"{name} {d.WorkDate:MMM d, yyyy}", AuditActions.Edited,
            new { overtime = OtStatuses.Waiting }, new { overtime = decision, minutes = d.OtMinutes });
        await db.SaveChangesAsync();
        return null;
    }

    static object Snapshot(AttendanceDay d) => new
    {
        time_in = d.TimeIn?.ToString("HH:mm"), break_out = d.BreakOut?.ToString("HH:mm"), break_in = d.BreakIn?.ToString("HH:mm"),
        time_out = d.TimeOut?.ToString("HH:mm"), status = d.Status, late_minutes = d.LateMinutes, overtime_minutes = d.OtMinutes, fix_reason = d.FixReason,
    };
}
