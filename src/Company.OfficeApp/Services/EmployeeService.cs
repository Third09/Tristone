using System.Text.RegularExpressions;
using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>One row of the Employees table. CurrentRate = the daily rate that applies today.</summary>
public record EmployeeRow(int EmployeeId, string FullName, string? Position, string Department, string PaySchedule,
                          decimal? CurrentRate, string? BiometricId, string Status);

/// <summary>What HR types in the employee form. The daily rate applies from RateFrom.</summary>
public record EmployeeInput(string FullName, string? ContactNumber, string? Address, string? Position, string Department,
                            string? PayGrade, string? BiometricId, bool GovDeductions,
                            string? SssNo, string? PhilHealthNo, string? PagIbigNo, string? Tin,
                            decimal DailyRate, DateOnly RateFrom);

/// <summary>
/// HR & Payroll > Employees (Project Outline, Part 2a). Used by Admins and the HR department.
/// Office staff and site workers both live here; leaving = Inactive (records kept, login turned off).
/// Daily rates are kept as a history so past payslips keep the rate they were paid with.
/// </summary>
public static partial class EmployeeService
{
    public const decimal MaxDailyRate = 100_000m;

    /// <summary>The pay schedule line shown under the department dropdown.</summary>
    public static string PayScheduleText(string? department) => department == null
        ? "Pay schedule: (choose a department)"
        : PaySchedules.For(department) == PaySchedules.Site
            ? "Pay schedule: Site (paid every Saturday)"
            : "Pay schedule: Office (paid the 15th and 30th)";

    /// <summary>The pay grade list (Claude's default 2026-11-21, change it here).</summary>
    public static readonly string[] PayGrades = ["Helper / Unskilled", "Semi-skilled", "Skilled", "Junior Staff", "Senior Staff"];

    /// <summary>Only digits and the given separators, with a digit count between min and max.</summary>
    static bool GoodNumber(string text, int minDigits, int maxDigits, string separators)
    {
        var t = text.Trim();
        if (t.Any(ch => !char.IsAsciiDigit(ch) && !separators.Contains(ch))) return false;
        int digits = t.Count(char.IsAsciiDigit);
        return digits >= minDigits && digits <= maxDigits && t.Length <= 20;
    }

    [GeneratedRegex(@"^[0-9]{1,9}$")]
    private static partial Regex BiometricPattern();

    public static async Task<List<EmployeeRow>> ListAsync(string search, string? status, string? paySchedule)
    {
        using var db = Db.Open();
        var today = DateOnly.FromDateTime(DateTime.Today);
        var q = db.Employees.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(e => e.FullName.ToLower().Contains(s) || (e.BiometricId ?? "").ToLower() == s);
        }
        if (status != null) q = q.Where(e => e.Status == status);
        if (paySchedule != null) q = q.Where(e => e.PaySchedule == paySchedule);

        return await q.OrderBy(e => e.FullName)
            .Select(e => new EmployeeRow(e.EmployeeId, e.FullName, e.Position, e.Department, e.PaySchedule,
                db.EmployeeRates.Where(r => r.EmployeeId == e.EmployeeId && r.EffectiveFrom <= today)
                                .OrderByDescending(r => r.EffectiveFrom).Select(r => (decimal?)r.DailyRate).FirstOrDefault(),
                e.BiometricId, e.Status))
            .ToListAsync();
    }

    public static async Task<Employee?> GetAsync(int employeeId)
    {
        using var db = Db.Open();
        return await db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }

    /// <summary>Rate history, newest first.</summary>
    public static async Task<List<EmployeeRate>> GetRatesAsync(int employeeId)
    {
        using var db = Db.Open();
        return await db.EmployeeRates.AsNoTracking().Where(r => r.EmployeeId == employeeId)
                       .OrderByDescending(r => r.EffectiveFrom).ToListAsync();
    }

    /// <summary>The daily rate that applies on a date (what payroll will use), or null if none yet.</summary>
    public static async Task<decimal?> RateOnAsync(int employeeId, DateOnly date)
    {
        using var db = Db.Open();
        return await RateOn(db, employeeId, date);
    }

    static Task<decimal?> RateOn(AppDbContext db, int employeeId, DateOnly date) =>
        db.EmployeeRates.Where(r => r.EmployeeId == employeeId && r.EffectiveFrom <= date)
          .OrderByDescending(r => r.EffectiveFrom).Select(r => (decimal?)r.DailyRate).FirstOrDefaultAsync();

    /// <summary>Null when the input is acceptable, otherwise the reason.</summary>
    public static string? CheckInput(EmployeeInput i)
    {
        if (string.IsNullOrWhiteSpace(i.FullName)) return "Type the employee's full name.";
        if (i.FullName.Trim().Length > 100) return "The name is too long (100 characters at most).";
        if (!EmployeeDepartments.All.Contains(i.Department)) return "Choose a department (Site for site workers).";
        if (i.DailyRate <= 0) return "Type the daily rate.";
        if (i.DailyRate > MaxDailyRate) return $"The daily rate looks too high (more than {ProjectService.Money(MaxDailyRate)}).";
        if (!string.IsNullOrWhiteSpace(i.BiometricId) && !BiometricPattern().IsMatch(i.BiometricId.Trim()))
            return "Biometric ID: numbers only, up to 9 digits (the ID shown on the scanner).";
        if (!string.IsNullOrWhiteSpace(i.ContactNumber) && !GoodNumber(i.ContactNumber, 7, 13, "+-() "))
            return "Contact number: numbers only, 7 to 13 digits (e.g. 0917 123 4567).";
        if ((i.Address?.Length ?? 0) > 255) return "The address is too long (255 characters at most).";
        if (!EmployeeRoles.For(i.Department).Contains(i.Position))
            return i.Department == EmployeeDepartments.Site ? "Choose the role: Foreman, Laborer or Helper." : "Choose the role: Employee or Supervisor.";
        if ((i.PayGrade?.Length ?? 0) > 30) return "The pay grade is too long (30 characters at most).";
        // Philippine ID lengths (tester report 2026-11-21); dashes and spaces may be typed between the digits
        if (!string.IsNullOrWhiteSpace(i.SssNo) && !GoodNumber(i.SssNo, 10, 10, "- "))
            return "SSS number: 10 digits, like 00-0000000-0.";
        if (!string.IsNullOrWhiteSpace(i.PhilHealthNo) && !GoodNumber(i.PhilHealthNo, 12, 12, "- "))
            return "PhilHealth number: 12 digits, like 00-000000000-0.";
        if (!string.IsNullOrWhiteSpace(i.PagIbigNo) && !GoodNumber(i.PagIbigNo, 12, 12, "- "))
            return "Pag-IBIG number: 12 digits, like 0000-0000-0000.";
        if (!string.IsNullOrWhiteSpace(i.Tin) && !GoodNumber(i.Tin, 9, 14, "- "))
            return "TIN: 9 to 14 digits, like 000-000-000-000.";
        return null;
    }

    /// <summary>Returns (new id, null) or (null, error).</summary>
    public static async Task<(int? EmployeeId, string? Error)> CreateAsync(EmployeeInput input)
    {
        if (RequireHr() is { } denied) return (null, denied);
        if (CheckInput(input) is { } problem) return (null, problem);

        using var db = Db.Open();
        if (await BiometricTakenAsync(db, input.BiometricId, null) is { } owner) return (null, $"Biometric ID {input.BiometricId!.Trim()} already belongs to {owner}.");

        var e = new Employee { CreatedBy = Session.CurrentUser!.UserId, CreatedAt = DateTime.Now };
        Apply(e, input);
        db.Employees.Add(e);
        await db.SaveChangesAsync();

        db.EmployeeRates.Add(new EmployeeRate { EmployeeId = e.EmployeeId, DailyRate = input.DailyRate, EffectiveFrom = input.RateFrom, CreatedBy = Session.CurrentUser.UserId });
        AuditService.Add(db, Departments.HrPayroll, "employees", e.EmployeeId, e.FullName, AuditActions.Added, null,
            Snapshot(e, input.DailyRate, input.RateFrom));
        await db.SaveChangesAsync();
        return (e.EmployeeId, null);
    }

    /// <summary>
    /// Saves the details. If the daily rate differs from the one that applies on RateFrom, a new rate starts
    /// on that date (the rate for the same date is replaced; older rates are kept).
    /// </summary>
    public static async Task<string?> UpdateAsync(int employeeId, EmployeeInput input)
    {
        if (RequireHr() is { } denied) return denied;
        if (CheckInput(input) is { } problem) return problem;

        using var db = Db.Open();
        var e = await db.Employees.FirstAsync(x => x.EmployeeId == employeeId);
        if (await BiometricTakenAsync(db, input.BiometricId, employeeId) is { } owner) return $"Biometric ID {input.BiometricId!.Trim()} already belongs to {owner}.";

        // A Supervisor/Employee login lives in its employee's department, so that department must stay a login department.
        var logins = await db.Users.Where(u => u.EmployeeId == employeeId).ToListAsync();
        if (logins.FirstOrDefault(u => !UserLevels.HasNoDepartment(u.Level)) is { } login && !UserAdminService.DepartmentList.Contains(input.Department))
            return $"{e.FullName} has a {login.Level} login in {login.Department}. Change or turn off that login in User Accounts before moving them to {input.Department}.";

        // The role decides a login's level (owner decision 2026-11-20), so changing it for someone who has a login is
        // the HR Supervisor's call, and nobody changes their own.
        bool roleChanged = (e.Position ?? "") != (input.Position ?? "");
        if (roleChanged && logins.Any(u => !UserLevels.HasNoDepartment(u.Level)))
        {
            if (logins.Any(u => u.UserId == Session.CurrentUser!.UserId)) return "You can't change your own role. Ask the HR Supervisor.";
            if (Access.RequireApprover(Departments.HrPayroll) is { } notSupervisor)
                return $"{e.FullName} has a login, and the role decides what it can do, so only the HR Supervisor can change it.";
        }

        var oldRate = await RateOn(db, employeeId, input.RateFrom);
        var before = Snapshot(e, oldRate, null);
        Apply(e, input);

        // Their login shows the same name and department as the employee record.
        foreach (var u in logins)
        {
            var dept = UserAdminService.DepartmentFor(u.Level, e.Department);
            // an Employee / Supervisor login takes its level from the role (Admin and Website Admin logins don't)
            var level = !UserLevels.HasNoDepartment(u.Level) && EmployeeRoles.Office.Contains(e.Position) ? e.Position! : u.Level;
            if (u.FullName == e.FullName && u.Department == dept && u.Level == level) continue;
            AuditService.Add(db, null, "users", u.UserId, u.FullName, AuditActions.Edited,
                new { full_name = u.FullName, department = u.Department, level = u.Level },
                new { full_name = e.FullName, department = dept, level, reason = "Employee record changed" });
            u.FullName = e.FullName;
            u.Department = dept;
            u.Level = level;
            u.UpdatedAt = DateTime.Now;
        }

        decimal? newRate = null;
        if (oldRate != input.DailyRate)
        {
            var sameDay = await db.EmployeeRates.FirstOrDefaultAsync(r => r.EmployeeId == employeeId && r.EffectiveFrom == input.RateFrom);
            if (sameDay != null) sameDay.DailyRate = input.DailyRate;
            else db.EmployeeRates.Add(new EmployeeRate { EmployeeId = employeeId, DailyRate = input.DailyRate, EffectiveFrom = input.RateFrom, CreatedBy = Session.CurrentUser!.UserId });
            newRate = input.DailyRate;
        }

        AuditService.Add(db, Departments.HrPayroll, "employees", e.EmployeeId, e.FullName, AuditActions.Edited, before,
            Snapshot(e, newRate ?? oldRate, newRate != null ? input.RateFrom : null));
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>The person left: Inactive, and any login of theirs is turned off. All records stay.</summary>
    public static async Task<string?> MarkInactiveAsync(int employeeId)
    {
        if (Access.RequireDelete(AppPage.HrPayroll) is { } denied) return denied;
        using var db = Db.Open();
        var e = await db.Employees.FirstAsync(x => x.EmployeeId == employeeId);
        if (e.Status == EmployeeStatuses.Inactive) return "This employee is already Inactive.";

        var logins = await db.Users.Where(u => u.EmployeeId == employeeId && u.Status != UserStatuses.Disabled).ToListAsync();
        if (logins.Any(u => u.UserId == Session.CurrentUser!.UserId)) return "You can't mark yourself Inactive.";
        if (logins.Any(u => u.IsAdmin) &&
            !await db.Users.AnyAsync(u => u.Level == UserLevels.Admin && u.Status == UserStatuses.Active && u.EmployeeId != employeeId))
            return "This person has the last active Admin login. Make another Admin first.";

        var before = new { status = e.Status };
        e.Status = EmployeeStatuses.Inactive;
        e.UpdatedAt = DateTime.Now;
        AuditService.Add(db, Departments.HrPayroll, "employees", e.EmployeeId, e.FullName, AuditActions.Edited, before, new { status = e.Status });

        foreach (var u in logins)
        {
            AuditService.Add(db, null, "users", u.UserId, u.FullName, AuditActions.Edited,
                new { status = u.Status }, new { status = UserStatuses.Disabled, reason = "Employee marked Inactive" });
            u.Status = UserStatuses.Disabled;
            u.UpdatedAt = DateTime.Now;
        }
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Active again. Their login (if any) stays off until an Admin enables it in User Accounts.</summary>
    public static async Task<string?> ReactivateAsync(int employeeId)
    {
        if (Access.RequireDelete(AppPage.HrPayroll) is { } denied) return denied;
        using var db = Db.Open();
        var e = await db.Employees.FirstAsync(x => x.EmployeeId == employeeId);
        if (e.Status == EmployeeStatuses.Active) return "This employee is already Active.";

        e.Status = EmployeeStatuses.Active;
        e.UpdatedAt = DateTime.Now;
        AuditService.Add(db, Departments.HrPayroll, "employees", e.EmployeeId, e.FullName, AuditActions.Edited,
            new { status = EmployeeStatuses.Inactive }, new { status = e.Status });
        await db.SaveChangesAsync();
        return null;
    }

    static string? RequireHr() => Access.RequireEdit(AppPage.HrPayroll);

    static async Task<string?> BiometricTakenAsync(AppDbContext db, string? biometricId, int? exceptEmployeeId)
    {
        if (string.IsNullOrWhiteSpace(biometricId)) return null;
        var id = biometricId.Trim();
        return await db.Employees.Where(e => e.BiometricId == id && e.EmployeeId != exceptEmployeeId)
                                 .Select(e => e.FullName).FirstOrDefaultAsync();
    }

    static string? Clean(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    static void Apply(Employee e, EmployeeInput i)
    {
        e.FullName = i.FullName.Trim();
        e.ContactNumber = Clean(i.ContactNumber);
        e.Address = Clean(i.Address);
        e.Position = Clean(i.Position);
        e.Department = i.Department;
        e.PaySchedule = PaySchedules.For(i.Department);
        e.PayGrade = Clean(i.PayGrade);
        e.BiometricId = Clean(i.BiometricId);
        e.GovDeductions = i.GovDeductions;
        e.SssNo = Clean(i.SssNo);
        e.PhilHealthNo = Clean(i.PhilHealthNo);
        e.PagIbigNo = Clean(i.PagIbigNo);
        e.Tin = Clean(i.Tin);
        e.UpdatedAt = DateTime.Now;
    }

    // Edit History fields. Government ID numbers are personal, so only whether each is filled in is recorded.
    static object Snapshot(Employee e, decimal? dailyRate, DateOnly? rateFrom) => new
    {
        full_name = e.FullName,
        contact_number = e.ContactNumber,
        address = e.Address,
        position = e.Position,
        department = e.Department,
        pay_schedule = e.PaySchedule,
        pay_grade = e.PayGrade,
        biometric_id = e.BiometricId,
        gov_deductions = e.GovDeductions,
        gov_ids_on_file = new { sss = e.SssNo != null, philhealth = e.PhilHealthNo != null, pagibig = e.PagIbigNo != null, tin = e.Tin != null },
        daily_rate = dailyRate,
        rate_from = rateFrom,
        status = e.Status,
    };
}
