using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.WebsiteAdmin.Services;

/// <summary>
/// Signing in and out of the Website Admin app with its own accounts (website_accounts).
/// Same rules as the office app: usernames ignore capital letters, 5 wrong passwords lock the account,
/// and every attempt is written to website_login_logs with the PC name.
/// </summary>
public static class WebsiteAuth
{
    public const int MaxFailedAttempts = 5;

    /// <summary>Who is signed in on this PC.</summary>
    public static WebsiteAccount? Current { get; private set; }

    /// <summary>Null when signed in, otherwise the message to show.</summary>
    public static async Task<string?> SignInAsync(string username, string password)
    {
        using var db = Db.Open();
        var name = username.Trim();
        var account = await db.WebsiteAccounts.FirstOrDefaultAsync(a => a.Username.ToLower() == name.ToLower());

        if (account == null)
        {
            await LogAsync(db, null, name, LoginActions.FailedLogin, "Unknown username");
            return "Wrong username or password.";
        }
        if (account.Status == UserStatuses.Disabled)
        {
            await LogAsync(db, account.AccountId, name, LoginActions.FailedLogin, "Account disabled");
            return "This account is turned off.";
        }
        if (account.Status == UserStatuses.Locked)
        {
            await LogAsync(db, account.AccountId, name, LoginActions.FailedLogin, "Account locked");
            return "This account is locked after too many wrong passwords.";
        }

        if (!BCrypt.Net.BCrypt.Verify(password, account.PasswordHash))
        {
            account.FailedLoginCount++;
            account.UpdatedAt = DateTime.Now;
            db.WebsiteLoginLogs.Add(NewLog(account.AccountId, name, LoginActions.FailedLogin,
                $"Wrong password ({account.FailedLoginCount} of {MaxFailedAttempts})"));
            if (account.FailedLoginCount >= MaxFailedAttempts)
            {
                account.Status = UserStatuses.Locked;
                db.WebsiteLoginLogs.Add(NewLog(account.AccountId, name, LoginActions.AccountLocked, $"{MaxFailedAttempts} wrong passwords"));
                await db.SaveChangesAsync();
                return "Too many wrong passwords. This account is now locked.";
            }
            await db.SaveChangesAsync();
            var left = MaxFailedAttempts - account.FailedLoginCount;
            return $"Wrong username or password. {left} attempt{(left == 1 ? "" : "s")} left before the account locks.";
        }

        account.FailedLoginCount = 0;
        account.LastLoginAt = DateTime.Now;
        account.UpdatedAt = DateTime.Now;
        db.WebsiteLoginLogs.Add(NewLog(account.AccountId, account.Username, LoginActions.Login, null));
        await db.SaveChangesAsync();
        Current = account;
        return null;
    }

    public static async Task SignOutAsync()
    {
        if (Current is not { } a) return;
        using var db = Db.Open();
        await LogAsync(db, a.AccountId, a.Username, LoginActions.Logout, null);
        Current = null;
    }

    static WebsiteLoginLog NewLog(int? accountId, string username, string action, string? details) => new()
    {
        AccountId = accountId, UsernameTried = username.Length > 50 ? username[..50] : username, Action = action, Details = details,
        ComputerName = Environment.MachineName,
    };

    static async Task LogAsync(AppDbContext db, int? accountId, string username, string action, string? details)
    {
        db.WebsiteLoginLogs.Add(NewLog(accountId, username, action, details));
        await db.SaveChangesAsync();
    }
}
