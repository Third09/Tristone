using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>One line of the Payroll tab / Payslip History.</summary>
public record PayrollRow(int PayslipId, int EmployeeId, string Name, decimal DaysPaid, decimal GrossPay, decimal LateDeduction, decimal OtPay,
                         decimal CashAdvance, decimal GovDeductions, decimal NetPay, DateOnly PayDate, string Period);

/// <summary>A pay period with its payroll (if computed).</summary>
public record PayrollView(PayPeriod Period, PayrollRun? Run, List<PayrollRow> Rows, List<string> Warnings, bool Blocked)
{
    public decimal TotalNet => Rows.Sum(r => r.NetPay);
}

/// <summary>Everything printed on a payslip.</summary>
public record PayslipData(Payslip Slip, string Name, string? Position, string Department, PayPeriod Period, bool GovOn, decimal CashAdvanceBalance);

/// <summary>Government deductions for one payday.</summary>
public record GovShares(decimal Sss, decimal PhilHealth, decimal PagIbig, decimal Tax)
{
    public decimal Total => Sss + PhilHealth + PagIbig + Tax;
}

/// <summary>
/// HR & Payroll > Payroll (Project Outline, Part 2). Pay comes only from validated attendance:
/// <list type="bullet">
/// <item>Paid days: Present or Late, or any day HR fixed (not Absent). Missing Scan days aren't paid until fixed.</item>
/// <item>Basic pay: each paid day at the daily rate of that day. Regular holidays not worked (on a work day, once the
/// employee had started) are paid too; Special holidays are no work, no pay.</item>
/// <item>Late and early minutes: deducted at daily rate / paid minutes a day (8 h = 480) per minute.</item>
/// <item>Overtime: approved minutes only, at the overtime rate per hour (₱100).</item>
/// <item>Government deductions only for employees whose switch is on (<see cref="HrSettings"/>).</item>
/// <item>Cash advances: each active one takes its per-payday amount (or what's left), never more than the pay left.</item>
/// </list>
/// Compute makes a Draft that can be computed again; Finalize (HR Supervisor / Admin) recomputes, refuses while there
/// are unfixed missing scans or overtime waiting, then locks the payroll, takes the cash advances and issues the payslips.
/// Days in a finalized payroll can't change in Attendance any more.
/// </summary>
public static class PayrollService
{
    const string Dept = Departments.HrPayroll;

    public static async Task<List<PayPeriod>> PeriodsAsync(string group)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        using var db = Db.Open();
        var holidays = (await db.Holidays.AsNoTracking().Select(h => h.HolidayDate).ToListAsync()).ToHashSet();
        var periods = PayPeriods.Between(group, today.AddMonths(-4), today.AddDays(16), holidays);
        // Older payrolls stay listed.
        var runs = await db.PayrollRuns.AsNoTracking().Where(r => r.PayGroup == group).ToListAsync();
        foreach (var r in runs.Where(r => periods.All(p => p.From != r.PeriodFrom)))
            periods.Add(new PayPeriod(group, r.PeriodFrom, r.PeriodTo, r.PayDate));
        return periods.OrderByDescending(p => p.From).ToList();
    }

    public static async Task<PayrollView> GetAsync(PayPeriod period)
    {
        using var db = Db.Open();
        var run = await db.PayrollRuns.AsNoTracking().FirstOrDefaultAsync(r => r.PayGroup == period.Group && r.PeriodFrom == period.From);
        var rows = run == null ? [] : await RowsAsync(db, run);
        var (warnings, blocked) = await WarningsAsync(db, period);
        return new PayrollView(period, run, rows, warnings, blocked);
    }

    static async Task<List<PayrollRow>> RowsAsync(AppDbContext db, PayrollRun run)
    {
        var label = $"{run.PeriodFrom:MMM d} - {run.PeriodTo:MMM d, yyyy}";
        return (await (from p in db.Payslips
                       join e in db.Employees on p.EmployeeId equals e.EmployeeId
                       where p.RunId == run.RunId
                       orderby e.FullName
                       select new { p, e.FullName }).ToListAsync())
            .Select(x => new PayrollRow(x.p.PayslipId, x.p.EmployeeId, x.FullName, x.p.DaysPaid, x.p.GrossPay, x.p.LateDeduction, x.p.OtPay,
                x.p.CashAdvance, x.p.GovDeductions, x.p.NetPay, run.PayDate, label)).ToList();
    }

    /// <summary>What still needs HR before this payroll can be finalized (Blocked) or what to double-check.</summary>
    static async Task<(List<string> Warnings, bool Blocked)> WarningsAsync(AppDbContext db, PayPeriod p)
    {
        var warnings = new List<string>();
        var ids = await db.Employees.Where(e => e.PaySchedule == p.Group && e.Status == EmployeeStatuses.Active).Select(e => e.EmployeeId).ToListAsync();
        var days = await db.AttendanceDays.AsNoTracking().Where(d => ids.Contains(d.EmployeeId) && d.WorkDate >= p.From && d.WorkDate <= p.To).ToListAsync();
        int missing = days.Count(d => d.Status == AttendanceStatuses.MissingScan && d.FixedBy == null);
        int otWaiting = days.Count(d => d.OtStatus == OtStatuses.Waiting);
        if (missing > 0) warnings.Add($"{missing} missing scan{(missing == 1 ? "" : "s")} not fixed yet (Attendance tab).");
        if (otWaiting > 0) warnings.Add($"{otWaiting} overtime{(otWaiting == 1 ? "" : "s")} waiting for approval (Attendance tab).");
        bool blocked = missing > 0 || otWaiting > 0;

        var today = DateOnly.FromDateTime(DateTime.Today);
        if (p.To >= today) warnings.Add("This period isn't over yet: days after today aren't paid until they are scanned.");
        var settings = await HrSettingsService.LoadAsync();
        var holidays = (await db.Holidays.Where(h => h.HolidayDate >= p.From && h.HolidayDate <= p.To).Select(h => h.HolidayDate).ToListAsync()).ToHashSet();
        var withRows = days.Select(d => d.WorkDate).ToHashSet();
        var empty = new List<DateOnly>();
        for (var d = p.From; d <= p.To && d < today; d = d.AddDays(1))
            if (settings.IsWorkDay(d) && !holidays.Contains(d) && !withRows.Contains(d)) empty.Add(d);
        if (ids.Count > 0 && empty.Count > 0)
            warnings.Add($"No attendance at all for {string.Join(", ", empty.Take(6).Select(d => d.ToString("MMM d")))}{(empty.Count > 6 ? "..." : "")}. Upload the scanner file for those days first.");
        if (ids.Count == 0) warnings.Add($"No active {p.Group.ToLower()} employees.");
        return (warnings, blocked);
    }

    /// <summary>Computes (or computes again) the Draft payroll for the period.</summary>
    public static async Task<string?> ComputeAsync(PayPeriod period)
    {
        if (Access.RequirePage(AppPage.HrPayroll) is { } denied) return denied;
        using var db = Db.Open();
        var run = await db.PayrollRuns.FirstOrDefaultAsync(r => r.PayGroup == period.Group && r.PeriodFrom == period.From);
        if (run?.Status == PayrollStatuses.Final) return "This payroll is already finalized.";
        if (run == null)
        {
            run = new PayrollRun { PayGroup = period.Group, PeriodFrom = period.From, PeriodTo = period.To, PayDate = period.PayDate };
            db.PayrollRuns.Add(run);
        }
        run.ComputedBy = Session.CurrentUser!.UserId;
        run.ComputedAt = DateTime.Now;
        await db.SaveChangesAsync();
        await FillAsync(db, run);
        AuditService.Add(db, Dept, "payroll_runs", run.RunId, $"{period.Group} payroll {period}", AuditActions.Edited, null, new { status = run.Status, computed = DateTime.Now });
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>
    /// HR Supervisor / Admin. Computes once more, then locks it: cash advances are taken and the payslips are final.
    /// </summary>
    public static async Task<string?> FinalizeAsync(PayPeriod period)
    {
        if (Access.RequireApprover(Dept) is { } denied) return denied;
        using var db = Db.Open();
        var (_, blocked) = await WarningsAsync(db, period);
        if (blocked) return "Fix the missing scans and decide the overtime waiting for approval first (Attendance tab).";
        var run = await db.PayrollRuns.FirstOrDefaultAsync(r => r.PayGroup == period.Group && r.PeriodFrom == period.From);
        if (run == null) return "Compute the payroll first.";
        if (run.Status == PayrollStatuses.Final) return "This payroll is already finalized.";

        await using var tx = await db.Database.BeginTransactionAsync();
        var slips = await FillAsync(db, run);
        foreach (var (slip, takes) in slips)
            foreach (var (advance, amount) in takes)
            {
                advance.Balance -= amount;
                if (advance.Balance == 0) advance.Status = CashAdvanceStatuses.PaidOff;
                db.CashAdvanceDeductions.Add(new CashAdvanceDeduction { AdvanceId = advance.AdvanceId, PayslipId = slip.PayslipId, Amount = amount, BalanceAfter = advance.Balance });
            }
        run.Status = PayrollStatuses.Final;
        run.FinalizedBy = Session.CurrentUser!.UserId;
        run.FinalizedAt = DateTime.Now;
        AuditService.Add(db, Dept, "payroll_runs", run.RunId, $"{period.Group} payroll {period}", AuditActions.Edited,
            new { status = PayrollStatuses.Draft }, new { status = PayrollStatuses.Final, payslips = slips.Count, total_net = slips.Sum(s => s.Slip.NetPay) });
        await db.SaveChangesAsync();
        await tx.CommitAsync();
        return null;
    }

    /// <summary>Works out every payslip of the run and saves them (replacing the old Draft ones).</summary>
    static async Task<List<(Payslip Slip, List<(CashAdvance Advance, decimal Amount)> Takes)>> FillAsync(AppDbContext db, PayrollRun run)
    {
        var settings = await HrSettingsService.LoadAsync();
        db.Payslips.RemoveRange(db.Payslips.Where(p => p.RunId == run.RunId));
        await db.SaveChangesAsync();

        var employees = await db.Employees.AsNoTracking().Where(e => e.PaySchedule == run.PayGroup && e.Status == EmployeeStatuses.Active).ToListAsync();
        var ids = employees.Select(e => e.EmployeeId).ToList();
        var rates = (await db.EmployeeRates.AsNoTracking().Where(r => ids.Contains(r.EmployeeId)).ToListAsync())
            .GroupBy(r => r.EmployeeId).ToDictionary(g => g.Key, g => g.OrderBy(r => r.EffectiveFrom).ToList());
        var days = (await db.AttendanceDays.AsNoTracking().Where(d => ids.Contains(d.EmployeeId) && d.WorkDate >= run.PeriodFrom && d.WorkDate <= run.PeriodTo).ToListAsync())
            .GroupBy(d => d.EmployeeId).ToDictionary(g => g.Key, g => g.ToList());
        var holidays = await db.Holidays.AsNoTracking().Where(h => h.HolidayDate >= run.PeriodFrom && h.HolidayDate <= run.PeriodTo && h.Kind == HolidayKinds.Regular).ToListAsync();
        var advances = (await db.CashAdvances.Where(c => ids.Contains(c.EmployeeId) && c.Status == CashAdvanceStatuses.Active && c.GivenOn <= run.PeriodTo)
            .OrderBy(c => c.GivenOn).ThenBy(c => c.AdvanceId).ToListAsync()).GroupBy(c => c.EmployeeId).ToDictionary(g => g.Key, g => g.ToList());

        var result = new List<(Payslip, List<(CashAdvance, decimal)>)>();
        foreach (var e in employees)
        {
            var empRates = rates.GetValueOrDefault(e.EmployeeId) ?? [];
            decimal RateOn(DateOnly d) => empRates.LastOrDefault(r => r.EffectiveFrom <= d)?.DailyRate ?? 0;
            var empDays = days.GetValueOrDefault(e.EmployeeId) ?? [];
            var started = empRates.FirstOrDefault()?.EffectiveFrom;

            var slip = new Payslip { RunId = run.RunId, EmployeeId = e.EmployeeId, DailyRate = RateOn(run.PeriodTo) };
            decimal lateDeduction = 0;
            foreach (var d in empDays)
            {
                bool paid = d.Status is AttendanceStatuses.Present or AttendanceStatuses.Late
                            || (d.FixedBy != null && d.Status is not (AttendanceStatuses.Absent or AttendanceStatuses.RestDayWork));
                var rate = RateOn(d.WorkDate);
                if (paid)
                {
                    slip.DaysPaid += 1;
                    slip.BasicPay += rate;
                    var minutes = d.LateMinutes + d.EarlyMinutes;
                    slip.LateMinutes += minutes;
                    lateDeduction += minutes * rate / settings.MinutesPerDay;
                }
                if (d.OtStatus == OtStatuses.Approved) slip.OtMinutes += d.OtMinutes;
            }
            foreach (var h in holidays)
                if (settings.IsWorkDay(h.HolidayDate) && started != null && h.HolidayDate >= started && h.HolidayDate <= DateOnly.FromDateTime(DateTime.Today)
                    && !empDays.Any(d => d.WorkDate == h.HolidayDate && d.Status != AttendanceStatuses.Absent))
                {
                    slip.HolidayDays++;
                    slip.HolidayPay += RateOn(h.HolidayDate);
                }
            slip.LateDeduction = Math.Round(lateDeduction, 2);
            slip.OtPay = Math.Round(slip.OtMinutes * settings.OvertimeRate / 60m, 2);
            slip.GrossPay = Math.Max(0, slip.BasicPay + slip.HolidayPay + slip.OtPay - slip.LateDeduction);

            var gov = e.GovDeductions && slip.GrossPay > 0 ? Government(settings, slip.DailyRate, slip.GrossPay, run.PayGroup) : new GovShares(0, 0, 0, 0);
            if (gov.Total > slip.GrossPay) gov = gov with { Tax = Math.Max(0, slip.GrossPay - (gov.Sss + gov.PhilHealth + gov.PagIbig)) };
            (slip.Sss, slip.PhilHealth, slip.PagIbig, slip.Tax) = (gov.Sss, gov.PhilHealth, gov.PagIbig, gov.Tax);
            decimal left = Math.Max(0, slip.GrossPay - gov.Total);

            var takes = new List<(CashAdvance, decimal)>();
            foreach (var a in advances.GetValueOrDefault(e.EmployeeId) ?? [])
            {
                var take = Math.Min(Math.Min(a.PerPayday, a.Balance), left);
                if (take <= 0) continue;
                takes.Add((a, take));
                left -= take;
                slip.CashAdvance += take;
            }
            slip.NetPay = Math.Round(left, 2);
            db.Payslips.Add(slip);
            result.Add((slip, takes));
        }
        await db.SaveChangesAsync();
        return result;
    }

    /// <summary>
    /// Government deductions for one payday, from a monthly basis of daily rate x work days per month.
    /// Office pays twice a month (half each payday); site pays weekly (12/52 of the month each payday).
    /// </summary>
    public static GovShares Government(HrSettings s, decimal dailyRate, decimal grossThisPayday, string group)
    {
        decimal basis = dailyRate * s.WorkDaysPerMonth;
        decimal share = group == PaySchedules.Site ? 12m / 52m : 0.5m;
        int paydaysPerYear = group == PaySchedules.Site ? 52 : 24;
        decimal credit = Math.Clamp(Math.Round(basis / 500m, MidpointRounding.AwayFromZero) * 500m, s.SssMinCredit, s.SssMaxCredit);
        decimal sss = Math.Round(credit * s.SssRate / 100m * share, 2);
        decimal ph = Math.Round(Math.Clamp(basis, s.PhilHealthFloor, s.PhilHealthCeiling) * s.PhilHealthRate / 100m * share, 2);
        decimal pi = Math.Round(Math.Min(basis, s.PagIbigCap) * s.PagIbigRate / 100m * share, 2);
        decimal tax = s.WithholdingTax ? Math.Round(AnnualTax((grossThisPayday - sss - ph - pi) * paydaysPerYear) / paydaysPerYear, 2) : 0;
        return new GovShares(sss, ph, pi, Math.Max(0, tax));
    }

    /// <summary>Yearly income tax (TRAIN law table, 2023 onward).</summary>
    public static decimal AnnualTax(decimal taxable) => taxable switch
    {
        <= 250_000m => 0,
        <= 400_000m => (taxable - 250_000m) * 0.15m,
        <= 800_000m => 22_500m + (taxable - 400_000m) * 0.20m,
        <= 2_000_000m => 102_500m + (taxable - 800_000m) * 0.25m,
        <= 8_000_000m => 402_500m + (taxable - 2_000_000m) * 0.30m,
        _ => 2_202_500m + (taxable - 8_000_000m) * 0.35m,
    };

    // ----- payslips -----

    /// <summary>Payslip History: payslips of finalized payrolls.</summary>
    public static async Task<List<PayrollRow>> HistoryAsync(int? runId, string search)
    {
        using var db = Db.Open();
        var q = from p in db.Payslips
                join r in db.PayrollRuns on p.RunId equals r.RunId
                join e in db.Employees on p.EmployeeId equals e.EmployeeId
                where r.Status == PayrollStatuses.Final && (runId == null || r.RunId == runId)
                select new { p, r, e.FullName };
        if (!string.IsNullOrWhiteSpace(search)) { var s = search.Trim().ToLower(); q = q.Where(x => x.FullName.ToLower().Contains(s)); }
        return (await q.OrderByDescending(x => x.r.PayDate).ThenBy(x => x.FullName).ToListAsync())
            .Select(x => new PayrollRow(x.p.PayslipId, x.p.EmployeeId, x.FullName, x.p.DaysPaid, x.p.GrossPay, x.p.LateDeduction, x.p.OtPay,
                x.p.CashAdvance, x.p.GovDeductions, x.p.NetPay, x.r.PayDate, $"{x.r.PeriodFrom:MMM d} - {x.r.PeriodTo:MMM d, yyyy}")).ToList();
    }

    /// <summary>Finalized payrolls for the Payslip History period filter.</summary>
    public static async Task<List<(int RunId, string Label)>> FinalRunsAsync()
    {
        using var db = Db.Open();
        var runs = await db.PayrollRuns.AsNoTracking().Where(r => r.Status == PayrollStatuses.Final).OrderByDescending(r => r.PayDate).ToListAsync();
        return runs.Select(r => (r.RunId, $"{r.PayGroup}: {r.PeriodFrom:MMM d} - {r.PeriodTo:MMM d, yyyy}, paid {r.PayDate:MMM d}")).ToList();
    }

    public static async Task<List<PayslipData>> PayslipsAsync(IReadOnlyCollection<int> payslipIds)
    {
        using var db = Db.Open();
        var list = await (from p in db.Payslips
                          join r in db.PayrollRuns on p.RunId equals r.RunId
                          join e in db.Employees on p.EmployeeId equals e.EmployeeId
                          where payslipIds.Contains(p.PayslipId)
                          orderby e.FullName
                          select new { p, r, e }).ToListAsync();
        var result = new List<PayslipData>();
        foreach (var x in list)
        {
            // Cash advance balance right after this payslip (final) or still owed (draft).
            var after = await db.CashAdvanceDeductions.Where(d => d.PayslipId == x.p.PayslipId).SumAsync(d => (decimal?)d.BalanceAfter);
            var owed = after ?? await db.CashAdvances.Where(c => c.EmployeeId == x.e.EmployeeId && c.Status == CashAdvanceStatuses.Active).SumAsync(c => (decimal?)c.Balance) ?? 0;
            if (after == null && x.r.Status == PayrollStatuses.Draft) owed -= x.p.CashAdvance;
            result.Add(new PayslipData(x.p, x.e.FullName, x.e.Position, x.e.Department,
                new PayPeriod(x.r.PayGroup, x.r.PeriodFrom, x.r.PeriodTo, x.r.PayDate), x.e.GovDeductions, Math.Max(0, owed)));
        }
        return result;
    }
}
