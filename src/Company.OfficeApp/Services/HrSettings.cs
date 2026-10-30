using System.Globalization;
using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>
/// HR & Payroll > Settings: work hours, overtime rate and the government deduction rates. Kept in app_settings
/// ("hr.*" keys); anything not saved yet uses the default below.
/// Government deductions (only for employees whose switch is on) are worked out from a monthly basis of
/// daily rate x <see cref="WorkDaysPerMonth"/>: SSS = <see cref="SssRate"/>% of the salary credit (rounded to the
/// nearest 500, between the minimum and maximum); PhilHealth = <see cref="PhilHealthRate"/>% of the basis kept
/// between the floor and ceiling; Pag-IBIG = <see cref="PagIbigRate"/>% of the basis up to the cap; withholding
/// tax from the TRAIN table (2023 onward) on pay after those. The defaults are the 2025 employee shares; HR or the
/// accountant should check them and change them here when the government updates them.
/// </summary>
public record HrSettings(
    TimeOnly WorkStart, TimeOnly WorkEnd, TimeOnly BreakStart, TimeOnly BreakEnd, int GraceMinutes,
    IReadOnlyList<DayOfWeek> WorkDays, decimal OvertimeRate, int OvertimeMinimumMinutes,
    int WorkDaysPerMonth, decimal SssRate, decimal SssMinCredit, decimal SssMaxCredit,
    decimal PhilHealthRate, decimal PhilHealthFloor, decimal PhilHealthCeiling,
    decimal PagIbigRate, decimal PagIbigCap, bool WithholdingTax)
{
    public static readonly HrSettings Default = new(
        new(8, 0), new(17, 0), new(12, 0), new(13, 0), 15,
        [DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday],
        100m, 30, 26, 5m, 5000m, 35000m, 2.5m, 10000m, 100000m, 2m, 10000m, true);

    public TimeOnly OnTimeUntil => WorkStart.AddMinutes(GraceMinutes);

    /// <summary>Paid minutes in a normal day (work hours minus the unpaid break).</summary>
    public int MinutesPerDay => (int)((WorkEnd - WorkStart) - (BreakEnd - BreakStart)).TotalMinutes;

    public bool IsWorkDay(DateOnly d) => WorkDays.Contains(d.DayOfWeek);

    public string? Check()
    {
        if (WorkEnd <= WorkStart) return "Work end must be after work start.";
        if (BreakStart <= WorkStart || BreakEnd >= WorkEnd || BreakEnd <= BreakStart) return "The break must be inside the work hours.";
        if (GraceMinutes is < 0 or > 120) return "Grace minutes must be from 0 to 120.";
        if (WorkDays.Count == 0) return "Tick at least one work day.";
        if (OvertimeRate < 0) return "The overtime rate can't be negative.";
        if (WorkDaysPerMonth is < 1 or > 31) return "Work days per month must be from 1 to 31.";
        if (SssMaxCredit < SssMinCredit || PhilHealthCeiling < PhilHealthFloor) return "A maximum can't be below its minimum.";
        if (new[] { SssRate, PhilHealthRate, PagIbigRate }.Any(r => r is < 0 or > 50)) return "Rates must be from 0 to 50%.";
        return null;
    }
}

/// <summary>Reads and saves <see cref="HrSettings"/> and the holiday list.</summary>
public static class HrSettingsService
{
    const string Dept = Departments.HrPayroll;
    const string Prefix = "hr.";

    public static async Task<HrSettings> LoadAsync()
    {
        using var db = Db.Open();
        var map = await db.AppSettings.AsNoTracking().Where(s => s.Key.StartsWith(Prefix)).ToDictionaryAsync(s => s.Key[Prefix.Length..], s => s.Value);
        var d = HrSettings.Default;
        TimeOnly T(string k, TimeOnly def) => map.TryGetValue(k, out var v) && TimeOnly.TryParseExact(v, "HH:mm", out var t) ? t : def;
        decimal M(string k, decimal def) => map.TryGetValue(k, out var v) && decimal.TryParse(v, NumberStyles.Number, CultureInfo.InvariantCulture, out var n) ? n : def;
        int I(string k, int def) => map.TryGetValue(k, out var v) && int.TryParse(v, out var n) ? n : def;
        var days = map.TryGetValue("work_days", out var wd)
            ? wd.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => (DayOfWeek)int.Parse(x)).ToList()
            : d.WorkDays.ToList();
        return new HrSettings(T("work_start", d.WorkStart), T("work_end", d.WorkEnd), T("break_start", d.BreakStart), T("break_end", d.BreakEnd),
            I("grace_minutes", d.GraceMinutes), days, M("ot_rate", d.OvertimeRate), I("ot_minimum_minutes", d.OvertimeMinimumMinutes),
            I("work_days_per_month", d.WorkDaysPerMonth), M("sss_rate", d.SssRate), M("sss_min_credit", d.SssMinCredit), M("sss_max_credit", d.SssMaxCredit),
            M("philhealth_rate", d.PhilHealthRate), M("philhealth_floor", d.PhilHealthFloor), M("philhealth_ceiling", d.PhilHealthCeiling),
            M("pagibig_rate", d.PagIbigRate), M("pagibig_cap", d.PagIbigCap),
            map.TryGetValue("withholding_tax", out var wt) ? wt == "true" : d.WithholdingTax);
    }

    static Dictionary<string, string> ToMap(HrSettings s) => new()
    {
        ["work_start"] = s.WorkStart.ToString("HH:mm"), ["work_end"] = s.WorkEnd.ToString("HH:mm"),
        ["break_start"] = s.BreakStart.ToString("HH:mm"), ["break_end"] = s.BreakEnd.ToString("HH:mm"),
        ["grace_minutes"] = s.GraceMinutes.ToString(), ["work_days"] = string.Join(",", s.WorkDays.Select(d => (int)d).Order()),
        ["ot_rate"] = N(s.OvertimeRate), ["ot_minimum_minutes"] = s.OvertimeMinimumMinutes.ToString(),
        ["work_days_per_month"] = s.WorkDaysPerMonth.ToString(),
        ["sss_rate"] = N(s.SssRate), ["sss_min_credit"] = N(s.SssMinCredit), ["sss_max_credit"] = N(s.SssMaxCredit),
        ["philhealth_rate"] = N(s.PhilHealthRate), ["philhealth_floor"] = N(s.PhilHealthFloor), ["philhealth_ceiling"] = N(s.PhilHealthCeiling),
        ["pagibig_rate"] = N(s.PagIbigRate), ["pagibig_cap"] = N(s.PagIbigCap), ["withholding_tax"] = s.WithholdingTax ? "true" : "false",
    };

    static string N(decimal v) => v.ToString(CultureInfo.InvariantCulture);

    /// <summary>Admin / HR Supervisor. Saves every setting and works out the attendance days again with them.</summary>
    public static async Task<string?> SaveAsync(HrSettings s)
    {
        if (Access.RequireApprover(Dept) is { } denied) return denied;
        if (s.Check() is { } problem) return problem;
        var before = ToMap(await LoadAsync());
        var after = ToMap(s);
        using (var db = Db.Open())
        {
            var rows = await db.AppSettings.Where(x => x.Key.StartsWith(Prefix)).ToDictionaryAsync(x => x.Key);
            foreach (var (k, v) in after)
            {
                if (!rows.TryGetValue(Prefix + k, out var row)) db.AppSettings.Add(row = new AppSetting { Key = Prefix + k });
                row.Value = v; row.UpdatedBy = Session.CurrentUser!.UserId; row.UpdatedAt = DateTime.Now;
            }
            AuditService.Add(db, Dept, "app_settings", 0, "HR settings", AuditActions.Edited, before, after);
            await db.SaveChangesAsync();
        }
        await AttendanceService.RecomputeRecentAsync();
        return null;
    }

    // ----- holidays -----

    public static async Task<List<Holiday>> HolidaysAsync(int? year = null)
    {
        using var db = Db.Open();
        var q = db.Holidays.AsNoTracking();
        if (year is { } y) q = q.Where(h => h.HolidayDate.Year == y);
        return await q.OrderBy(h => h.HolidayDate).ToListAsync();
    }

    public static async Task<string?> AddHolidayAsync(DateOnly date, string name, string kind)
    {
        if (Access.RequireApprover(Dept) is { } denied) return denied;
        if (string.IsNullOrWhiteSpace(name)) return "Type the holiday's name.";
        if (!HolidayKinds.All.Contains(kind)) return "Choose Regular or Special.";
        using (var db = Db.Open())
        {
            if (await db.Holidays.AnyAsync(h => h.HolidayDate == date)) return $"{date:MMM d, yyyy} is already a holiday.";
            db.Holidays.Add(new Holiday { HolidayDate = date, Name = name.Trim(), Kind = kind });
            AuditService.Add(db, Dept, "holidays", date.DayNumber, $"{name.Trim()} {date:MMM d, yyyy}", AuditActions.Added, null, new { date, name = name.Trim(), kind });
            await db.SaveChangesAsync();
        }
        await AttendanceService.RecomputeRecentAsync();
        return null;
    }

    public static async Task<string?> DeleteHolidayAsync(DateOnly date)
    {
        if (Access.RequireApprover(Dept) is { } denied) return denied;
        using (var db = Db.Open())
        {
            var h = await db.Holidays.FirstOrDefaultAsync(x => x.HolidayDate == date);
            if (h == null) return null;
            AuditService.Add(db, Dept, "holidays", date.DayNumber, $"{h.Name} {date:MMM d, yyyy}", AuditActions.Deleted, new { date, name = h.Name, kind = h.Kind }, null);
            db.Holidays.Remove(h);
            await db.SaveChangesAsync();
        }
        await AttendanceService.RecomputeRecentAsync();
        return null;
    }
}
