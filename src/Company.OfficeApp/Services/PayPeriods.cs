using Company.Shared.Models;

namespace Company.OfficeApp.Services;

/// <summary>One pay period of a pay group.</summary>
public record PayPeriod(string Group, DateOnly From, DateOnly To, DateOnly PayDate)
{
    public override string ToString() => $"{From:MMM d} - {To:MMM d, yyyy}, paid {PayDate:MMM d}";
}

/// <summary>
/// The pay calendar (Project Outline, Part 2).
/// Office: payday the 15th covers the 26th of last month to the 10th; payday the 30th (February: its last day) covers
/// the 11th to the 25th. Site: payday every Saturday, covering the Saturday before to Friday (HR uploads on Friday, so
/// Saturday's attendance is paid the next week). A payday on a Sunday or holiday moves to the day before.
/// </summary>
public static class PayPeriods
{
    /// <summary>The periods whose payday is in [from, to] (newest first).</summary>
    public static List<PayPeriod> Between(string group, DateOnly from, DateOnly to, ISet<DateOnly> holidays)
    {
        var list = new List<PayPeriod>();
        if (group == PaySchedules.Site)
        {
            var sat = from.AddDays(((int)DayOfWeek.Saturday - (int)from.DayOfWeek + 7) % 7);
            for (; sat <= to; sat = sat.AddDays(7))
                list.Add(new PayPeriod(group, sat.AddDays(-7), sat.AddDays(-1), Move(sat, holidays)));
        }
        else
        {
            for (var m = new DateOnly(from.Year, from.Month, 1); m <= to; m = m.AddMonths(1))
            {
                var prev = m.AddMonths(-1);
                list.Add(new PayPeriod(group, new DateOnly(prev.Year, prev.Month, 26), new DateOnly(m.Year, m.Month, 10), Move(new DateOnly(m.Year, m.Month, 15), holidays)));
                list.Add(new PayPeriod(group, new DateOnly(m.Year, m.Month, 11), new DateOnly(m.Year, m.Month, 25),
                    Move(new DateOnly(m.Year, m.Month, Math.Min(30, DateTime.DaysInMonth(m.Year, m.Month))), holidays)));
            }
            list = list.Where(p => p.PayDate >= from.AddDays(-2) && p.PayDate <= to.AddDays(2)).ToList();
        }
        return list.OrderByDescending(p => p.From).ToList();
    }

    // Sunday or holiday -> the day before (again if that is one too).
    static DateOnly Move(DateOnly payday, ISet<DateOnly> holidays)
    {
        while (payday.DayOfWeek == DayOfWeek.Sunday || holidays.Contains(payday)) payday = payday.AddDays(-1);
        return payday;
    }
}
