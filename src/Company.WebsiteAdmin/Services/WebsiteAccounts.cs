using Company.OfficeApp.Data;
using Company.Shared;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.WebsiteAdmin.Services;

/// <summary>One row of the Accounts list.</summary>
public record AccountRow(int AccountId, string Username, string FullName, bool IsMain, string Status, DateTime? LastLoginAt)
{
    public string Main => IsMain ? "Yes" : "No";
}

/// <summary>What the main account types in the account form.</summary>
public record AccountInput(string FullName, string Username, bool IsMain);

/// <summary>
/// Managing Website Admin accounts. Only an active MAIN account may do any of this (owner decision 2026-10-30);
/// every method re-checks it against the database, not just the screen. Same safety rules as the office app's
/// User Accounts: you can't turn yourself off or remove your own main status, and the last active main account
/// is protected. Password changes and unlocks are written to website_login_logs.
/// </summary>
public static class WebsiteAccounts
{
    /// <summary>Null when the signed-in account is an active main account, otherwise the message to show.</summary>
    static async Task<string?> RequireMainAsync(AppDbContext db)
    {
        var me = WebsiteAuth.Current == null ? null
            : await db.WebsiteAccounts.AsNoTracking().FirstOrDefaultAsync(a => a.AccountId == WebsiteAuth.Current.AccountId);
        return me is { IsMain: true, Status: UserStatuses.Active } ? null : "Only a main Website Admin account can manage accounts.";
    }

    public static async Task<bool> IsMainAsync()
    {
        using var db = Db.Open();
        return await RequireMainAsync(db) == null;
    }

    public static async Task<List<AccountRow>> ListAsync()
    {
        using var db = Db.Open();
        if (await RequireMainAsync(db) != null) return [];
        return await db.WebsiteAccounts.AsNoTracking().OrderBy(a => a.FullName)
            .Select(a => new AccountRow(a.AccountId, a.Username, a.FullName, a.IsMain, a.Status, a.LastLoginAt))
            .ToListAsync();
    }

    public static string? CheckInput(AccountInput i)
    {
        if (string.IsNullOrWhiteSpace(i.FullName)) return "Type the person's full name.";
        if (i.FullName.Trim().Length > 100) return "The name is too long (100 characters at most).";
        return AccountRules.CheckUsername(i.Username);
    }

    static Task<bool> UsernameTakenAsync(AppDbContext db, string username, int? exceptId)
    {
        var name = username.Trim().ToLower();
        return db.WebsiteAccounts.AnyAsync(a => a.Username.ToLower() == name && a.AccountId != exceptId);
    }

    static Task<bool> IsLastActiveMainAsync(AppDbContext db, int accountId) =>
        db.WebsiteAccounts.AllAsync(a => a.AccountId == accountId || !(a.IsMain && a.Status == UserStatuses.Active));

    public static async Task<(int? Id, string? Error)> CreateAsync(AccountInput i, string password)
    {
        using var db = Db.Open();
        if (await RequireMainAsync(db) is { } denied) return (null, denied);
        if (CheckInput(i) is { } problem) return (null, problem);
        if (AccountRules.CheckNewPassword(password, password) is { } pw) return (null, pw);
        if (await UsernameTakenAsync(db, i.Username, null)) return (null, "That username is already used.");

        var a = new WebsiteAccount
        {
            FullName = i.FullName.Trim(), Username = i.Username.Trim(), IsMain = i.IsMain,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, 11),
        };
        db.WebsiteAccounts.Add(a);
        await db.SaveChangesAsync();
        return (a.AccountId, null);
    }

    public static async Task<string?> UpdateAsync(int accountId, AccountInput i)
    {
        using var db = Db.Open();
        if (await RequireMainAsync(db) is { } denied) return denied;
        if (CheckInput(i) is { } problem) return problem;
        if (await UsernameTakenAsync(db, i.Username, accountId)) return "That username is already used.";
        var a = await db.WebsiteAccounts.FirstAsync(x => x.AccountId == accountId);
        if (a.IsMain && !i.IsMain)
        {
            if (accountId == WebsiteAuth.Current!.AccountId) return "You can't remove your own main status.";
            if (a.Status == UserStatuses.Active && await IsLastActiveMainAsync(db, accountId))
                return "This is the last active main account. Make another one main first.";
        }
        a.FullName = i.FullName.Trim();
        a.Username = i.Username.Trim();
        a.IsMain = i.IsMain;
        a.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Sets a new password. Also unlocks a locked account and resets its wrong-password counter.</summary>
    public static async Task<string?> SetPasswordAsync(int accountId, string password)
    {
        using var db = Db.Open();
        if (await RequireMainAsync(db) is { } denied) return denied;
        if (AccountRules.CheckNewPassword(password, password) is { } pw) return pw;
        var a = await db.WebsiteAccounts.FirstAsync(x => x.AccountId == accountId);
        a.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, 11);
        a.FailedLoginCount = 0;
        a.UpdatedAt = DateTime.Now;
        db.WebsiteLoginLogs.Add(Log(a, LoginActions.PasswordChanged));
        if (a.Status == UserStatuses.Locked)
        {
            a.Status = UserStatuses.Active;
            db.WebsiteLoginLogs.Add(Log(a, LoginActions.AccountUnlocked));
        }
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> UnlockAsync(int accountId)
    {
        using var db = Db.Open();
        if (await RequireMainAsync(db) is { } denied) return denied;
        var a = await db.WebsiteAccounts.FirstAsync(x => x.AccountId == accountId);
        if (a.Status != UserStatuses.Locked) return "This account isn't locked.";
        a.Status = UserStatuses.Active;
        a.FailedLoginCount = 0;
        a.UpdatedAt = DateTime.Now;
        db.WebsiteLoginLogs.Add(Log(a, LoginActions.AccountUnlocked));
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> DisableAsync(int accountId)
    {
        using var db = Db.Open();
        if (await RequireMainAsync(db) is { } denied) return denied;
        var a = await db.WebsiteAccounts.FirstAsync(x => x.AccountId == accountId);
        if (a.Status == UserStatuses.Disabled) return "This account is already turned off.";
        if (accountId == WebsiteAuth.Current!.AccountId) return "You can't turn off your own account.";
        if (a.IsMain && a.Status == UserStatuses.Active && await IsLastActiveMainAsync(db, accountId))
            return "This is the last active main account. Make another one main first.";
        a.Status = UserStatuses.Disabled;
        a.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        return null;
    }

    public static async Task<string?> EnableAsync(int accountId)
    {
        using var db = Db.Open();
        if (await RequireMainAsync(db) is { } denied) return denied;
        var a = await db.WebsiteAccounts.FirstAsync(x => x.AccountId == accountId);
        if (a.Status != UserStatuses.Disabled) return "This account isn't turned off.";
        a.Status = UserStatuses.Active;
        a.FailedLoginCount = 0;
        a.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        return null;
    }

    static WebsiteLoginLog Log(WebsiteAccount a, string action) => new()
    {
        AccountId = a.AccountId, UsernameTried = a.Username, Action = action,
        Details = $"By {WebsiteAuth.Current!.Username}", ComputerName = Environment.MachineName,
    };
}
