using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

public record SubRow(int SubcontractorId, string BusinessName, string Trades, string? PrimaryContact, string? Phone, DateOnly? PcabExpiry)
{
    public override string ToString() => BusinessName;
}

public record SubInput(
    string BusinessName, string? TradeName, string? DtiSecNo, string? Tin,
    string? PrimaryName, string? PrimaryRole, string? PrimaryPhone, string? PrimaryMobile, string? PrimaryEmail,
    string? SecondaryName, string? SecondaryRole, string? SecondaryPhone, string? SecondaryMobile, string? SecondaryEmail,
    string? OfficeAddress, string? YardAddress, string? BankName, string? AccountName, string? AccountNumber,
    IReadOnlyList<string> Trades, string? PcabLicenseNo, string? PcabClassification, DateOnly? PcabExpiry);

/// <summary>
/// Subcontractors > Directory (Project Outline, Part 4): company details, two contacts, addresses, bank details
/// (used on billing vouchers), trade categories and the PCAB license with an expiry reminder. No ratings for now.
/// </summary>
public static class SubcontractorService
{
    const string Dept = Departments.Subcontractors;

    /// <summary>PCAB license already expired, or expiring within <see cref="SubLists.PcabWarningDays"/> days.</summary>
    public static bool PcabDue(DateOnly? expiry, DateOnly today) => expiry is { } d && d <= today.AddDays(SubLists.PcabWarningDays);

    public static async Task<List<SubRow>> ListAsync(string search = "", string? trade = null)
    {
        using var db = Db.Open();
        var q = db.Subcontractors.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(x => x.BusinessName.ToLower().Contains(s) || (x.TradeName != null && x.TradeName.ToLower().Contains(s)));
        }
        if (trade != null) q = q.Where(x => db.SubcontractorTrades.Any(t => t.SubcontractorId == x.SubcontractorId && t.Trade == trade));
        var subs = await q.OrderBy(x => x.BusinessName).ToListAsync();

        var ids = subs.Select(x => x.SubcontractorId).ToList();
        var trades = (await db.SubcontractorTrades.AsNoTracking().Where(t => ids.Contains(t.SubcontractorId)).ToListAsync())
            .GroupBy(t => t.SubcontractorId).ToDictionary(g => g.Key, g => string.Join(", ", g.Select(t => t.Trade).OrderBy(t => t)));

        return subs.Select(x => new SubRow(x.SubcontractorId, x.BusinessName, trades.GetValueOrDefault(x.SubcontractorId, ""),
            x.PrimaryName, x.PrimaryMobile ?? x.PrimaryPhone, x.PcabExpiry)).ToList();
    }

    /// <summary>How many PCAB licenses are expired or about to expire (shown in the tab title).</summary>
    public static async Task<int> PcabDueCountAsync(DateOnly today)
    {
        using var db = Db.Open();
        var limit = today.AddDays(SubLists.PcabWarningDays);
        return await db.Subcontractors.CountAsync(x => x.PcabExpiry != null && x.PcabExpiry <= limit);
    }

    public static async Task<(Subcontractor Sub, List<string> Trades)?> GetAsync(int subcontractorId)
    {
        using var db = Db.Open();
        var s = await db.Subcontractors.AsNoTracking().FirstOrDefaultAsync(x => x.SubcontractorId == subcontractorId);
        if (s == null) return null;
        var trades = await db.SubcontractorTrades.Where(t => t.SubcontractorId == subcontractorId).Select(t => t.Trade).ToListAsync();
        return (s, trades);
    }

    public static string? CheckInput(SubInput i)
    {
        if (string.IsNullOrWhiteSpace(i.BusinessName)) return "Type the official business name.";
        if (i.BusinessName.Trim().Length > 150) return "The business name is too long (150 characters at most).";
        if (i.Trades.Count == 0) return "Tick at least one trade category.";
        if (i.Trades.Any(t => !SubLists.Trades.Contains(t))) return "Choose trades from the list.";
        foreach (var email in new[] { i.PrimaryEmail, i.SecondaryEmail })
            if (!string.IsNullOrWhiteSpace(email) && !email.Contains('@')) return "That email doesn't look right.";
        if (i.PcabExpiry != null && string.IsNullOrWhiteSpace(i.PcabLicenseNo)) return "Type the PCAB license number for that expiry date.";
        return null;
    }

    public static async Task<(int? Id, string? Error)> CreateAsync(SubInput i)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return (null, denied);
        if (CheckInput(i) is { } problem) return (null, problem);
        using var db = Db.Open();
        if (await db.Subcontractors.AnyAsync(x => x.BusinessName.ToLower() == i.BusinessName.Trim().ToLower()))
            return (null, "A subcontractor with that business name already exists.");
        var s = new Subcontractor { CreatedBy = Session.CurrentUser!.UserId };
        Apply(s, i);
        db.Subcontractors.Add(s);
        await db.SaveChangesAsync();
        db.SubcontractorTrades.AddRange(i.Trades.Distinct().Select(t => new SubcontractorTrade { SubcontractorId = s.SubcontractorId, Trade = t }));
        AuditService.Add(db, Dept, "subcontractors", s.SubcontractorId, s.BusinessName, AuditActions.Added, null, Snapshot(s, i.Trades));
        await db.SaveChangesAsync();
        return (s.SubcontractorId, null);
    }

    public static async Task<string?> UpdateAsync(int subcontractorId, SubInput i)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return denied;
        if (CheckInput(i) is { } problem) return problem;
        using var db = Db.Open();
        var s = await db.Subcontractors.FirstAsync(x => x.SubcontractorId == subcontractorId);
        if (await db.Subcontractors.AnyAsync(x => x.BusinessName.ToLower() == i.BusinessName.Trim().ToLower() && x.SubcontractorId != subcontractorId))
            return "A subcontractor with that business name already exists.";
        var oldTrades = await db.SubcontractorTrades.Where(t => t.SubcontractorId == subcontractorId).ToListAsync();
        var before = Snapshot(s, oldTrades.Select(t => t.Trade).ToList());
        Apply(s, i);
        db.SubcontractorTrades.RemoveRange(oldTrades);
        db.SubcontractorTrades.AddRange(i.Trades.Distinct().Select(t => new SubcontractorTrade { SubcontractorId = subcontractorId, Trade = t }));
        AuditService.Add(db, Dept, "subcontractors", s.SubcontractorId, s.BusinessName, AuditActions.Edited, before, Snapshot(s, i.Trades));
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> DeleteAsync(int subcontractorId)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return denied;
        using var db = Db.Open();
        var s = await db.Subcontractors.FirstAsync(x => x.SubcontractorId == subcontractorId);
        if (await db.SubJobs.AnyAsync(j => j.SubcontractorId == subcontractorId))
            return "This subcontractor already has jobs, so it can't be deleted.";
        var trades = await db.SubcontractorTrades.Where(t => t.SubcontractorId == subcontractorId).ToListAsync();
        AuditService.Add(db, Dept, "subcontractors", s.SubcontractorId, s.BusinessName, AuditActions.Deleted, Snapshot(s, trades.Select(t => t.Trade).ToList()), null);
        db.SubcontractorTrades.RemoveRange(trades);
        db.Subcontractors.Remove(s);
        await db.SaveChangesAsync();
        return null;
    }

    static string? Clean(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    static void Apply(Subcontractor s, SubInput i)
    {
        s.BusinessName = i.BusinessName.Trim(); s.TradeName = Clean(i.TradeName); s.DtiSecNo = Clean(i.DtiSecNo); s.Tin = Clean(i.Tin);
        s.PrimaryName = Clean(i.PrimaryName); s.PrimaryRole = Clean(i.PrimaryRole); s.PrimaryPhone = Clean(i.PrimaryPhone);
        s.PrimaryMobile = Clean(i.PrimaryMobile); s.PrimaryEmail = Clean(i.PrimaryEmail);
        s.SecondaryName = Clean(i.SecondaryName); s.SecondaryRole = Clean(i.SecondaryRole); s.SecondaryPhone = Clean(i.SecondaryPhone);
        s.SecondaryMobile = Clean(i.SecondaryMobile); s.SecondaryEmail = Clean(i.SecondaryEmail);
        s.OfficeAddress = Clean(i.OfficeAddress); s.YardAddress = Clean(i.YardAddress);
        s.BankName = Clean(i.BankName); s.AccountName = Clean(i.AccountName); s.AccountNumber = Clean(i.AccountNumber);
        s.PcabLicenseNo = Clean(i.PcabLicenseNo); s.PcabClassification = Clean(i.PcabClassification); s.PcabExpiry = i.PcabExpiry;
        s.UpdatedAt = DateTime.Now;
    }

    // The TIN and the bank account number are kept out of the readable history: only whether one is on file.
    static object Snapshot(Subcontractor s, IReadOnlyList<string> trades) => new
    {
        business_name = s.BusinessName, trade_name = s.TradeName, dti_sec_no = s.DtiSecNo, tin_on_file = s.Tin != null,
        primary_name = s.PrimaryName, primary_role = s.PrimaryRole, primary_phone = s.PrimaryPhone, primary_mobile = s.PrimaryMobile,
        primary_email = s.PrimaryEmail, secondary_name = s.SecondaryName, secondary_role = s.SecondaryRole,
        secondary_phone = s.SecondaryPhone, secondary_mobile = s.SecondaryMobile, secondary_email = s.SecondaryEmail,
        office_address = s.OfficeAddress, yard_address = s.YardAddress, bank_name = s.BankName, account_name = s.AccountName,
        account_number_on_file = s.AccountNumber != null, trades = string.Join(", ", trades.OrderBy(t => t)),
        pcab_license_no = s.PcabLicenseNo, pcab_classification = s.PcabClassification, pcab_expiry = s.PcabExpiry,
    };
}
