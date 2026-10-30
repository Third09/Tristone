using Company.Shared.Models;

namespace Company.OfficeApp.Services;

/// <summary>One worked-out day: the four times and what they mean.</summary>
public record DayResult(TimeOnly? TimeIn, TimeOnly? BreakOut, TimeOnly? BreakIn, TimeOnly? TimeOut,
                        int LateMinutes, int EarlyMinutes, int OtMinutes, string Status);

/// <summary>
/// The work-day rules (Project Outline, Part 2), owner-approved 2026-10-30, with the hours from HR Settings
/// (<see cref="Current"/>; defaults 8 AM to 5 PM, Monday to Saturday, 12-1 unpaid lunch). Four scans a day (in, break
/// out, break in, out). On time until start + grace (8:15); after that, late counts the exact minutes from the start.
/// Coming back from lunch late also counts as late minutes, and leaving before the end counts as early minutes.
/// Overtime = minutes after the end, counted from the minimum (30) on, paid only when HR approves. A day that isn't a
/// work day: any scans make it "Rest Day Work", all of it waiting for approval as overtime.
/// </summary>
public static class AttendanceRules
{
    /// <summary>The settings in use. <see cref="AttendanceService"/> loads them from HR Settings before working out days.</summary>
    public static HrSettings Current { get; set; } = HrSettings.Default;

    public static TimeOnly Start => Current.WorkStart;
    public static TimeOnly Grace => Current.OnTimeUntil;
    public static TimeOnly LunchOut => Current.BreakStart;
    public static TimeOnly LunchIn => Current.BreakEnd;
    public static TimeOnly End => Current.WorkEnd;
    public static int OtMinimumMinutes => Current.OvertimeMinimumMinutes;
    public const int RepeatScanMinutes = 3;   // a second scan within 3 minutes is the same scan (the scanner's own setting)

    static TimeOnly[] Expected => [Start, LunchOut, LunchIn, End];

    public static bool IsWorkDay(DateOnly d) => Current.IsWorkDay(d);

    /// <summary>Works out a day from its raw scans (any number, any order).</summary>
    public static DayResult FromScans(DateOnly date, IEnumerable<TimeOnly> scans)
    {
        var times = new List<TimeOnly>();
        foreach (var t in scans.OrderBy(t => t))
            if (times.Count == 0 || (t - times[^1]).TotalMinutes >= RepeatScanMinutes) times.Add(t);
        // A rest day has no set hours, so with 2 or 3 scans the first is the time in and the last the time out.
        var slots = !IsWorkDay(date) && times.Count is 2 or 3 ? [times[0], null, null, times[^1]] : Assign(times);
        return FromTimes(date, slots[0], slots[1], slots[2], slots[3]);
    }

    /// <summary>Works out a day from the four times (also used when HR fixes a day by hand).</summary>
    public static DayResult FromTimes(DateOnly date, TimeOnly? timeIn, TimeOnly? breakOut, TimeOnly? breakIn, TimeOnly? timeOut)
    {
        int filled = new[] { timeIn, breakOut, breakIn, timeOut }.Count(t => t != null);
        if (filled == 0) return new DayResult(null, null, null, null, 0, 0, 0, AttendanceStatuses.Absent);

        if (!IsWorkDay(date))
        {
            int worked = timeIn is { } a && timeOut is { } b ? Minutes(a, b) - (breakOut is { } c && breakIn is { } d ? Minutes(c, d) : 0) : 0;
            return new DayResult(timeIn, breakOut, breakIn, timeOut, 0, 0, Math.Max(0, worked), AttendanceStatuses.RestDayWork);
        }

        int late = 0, early = 0, ot = 0;
        if (timeIn is { } i && i > Grace) late += Minutes(Start, i);
        if (breakIn is { } bi && bi > LunchIn) late += Minutes(LunchIn, bi);
        if (timeOut is { } o && o < End) early = Minutes(o, End);
        if (timeOut is { } o2 && o2 > End && Minutes(End, o2) >= OtMinimumMinutes) ot = Minutes(End, o2);

        var status = filled < 4 ? AttendanceStatuses.MissingScan : late > 0 ? AttendanceStatuses.Late : AttendanceStatuses.Present;
        return new DayResult(timeIn, breakOut, breakIn, timeOut, late, early, ot, status);
    }

    static int Minutes(TimeOnly a, TimeOnly b) => (int)Math.Round((b.ToTimeSpan() - a.ToTimeSpan()).TotalMinutes);

    /// <summary>
    /// Puts the scans into the four slots (in, break out, break in, out). Four scans fill them in order; with fewer,
    /// each scan goes to the slot whose usual time is nearest, keeping their order; with more, the first and last
    /// are in/out and the two closest to 12:00 and 1:00 are the lunch scans.
    /// </summary>
    public static TimeOnly?[] Assign(List<TimeOnly> times)
    {
        var slots = new TimeOnly?[4];
        if (times.Count == 0) return slots;
        if (times.Count >= 4)
        {
            slots[0] = times[0]; slots[3] = times[^1];
            var middle = times.Skip(1).Take(times.Count - 2).ToList();
            slots[1] = middle.Count == 2 ? middle[0] : Nearest(middle.Take(middle.Count - 1), LunchOut);
            var afterOut = middle.SkipWhile(t => t != slots[1]).Skip(1);
            slots[2] = Nearest(afterOut, LunchIn);
            return slots;
        }
        // Fewer than four: try every way of placing them in order and keep the closest fit.
        double best = double.MaxValue;
        int[]? bestPick = null;
        foreach (var pick in Combinations(4, times.Count))
        {
            double cost = 0;
            for (int k = 0; k < pick.Length; k++) cost += Gap(times[k], Expected[pick[k]]);
            if (cost < best) { best = cost; bestPick = pick; }
        }
        for (int k = 0; k < bestPick!.Length; k++) slots[bestPick[k]] = times[k];
        return slots;
    }

    static TimeOnly? Nearest(IEnumerable<TimeOnly> times, TimeOnly target) =>
        times.Select(t => (TimeOnly?)t).MinBy(t => Gap(t!.Value, target));

    // TimeOnly subtraction wraps around midnight, so measure both ways and keep the shorter.
    static double Gap(TimeOnly a, TimeOnly b) => Math.Abs((a.ToTimeSpan() - b.ToTimeSpan()).TotalMinutes);

    static IEnumerable<int[]> Combinations(int n, int k)
    {
        if (k == 0) { yield return []; yield break; }
        for (int first = 0; first <= n - k; first++)
            foreach (var rest in Combinations(n - first - 1, k - 1))
                yield return [first, .. rest.Select(r => r + first + 1)];
    }
}
