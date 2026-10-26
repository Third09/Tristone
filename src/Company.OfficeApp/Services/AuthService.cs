using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>Result of a sign-in attempt: either the signed-in user, or a message to show.</summary>
public record SignInResult(User? User, string? Error)
{
    public bool Success => User != null;
}

/// <summary>Signing in and out. Every attempt is written to login_logs (Dashboard > Log History).</summary>
public static class AuthService
{
    public const int MaxFailedAttempts = 5;

    public static async Task<SignInResult> SignInAsync(string username, string password)
    {
        using var db = Db.Open();
        var name = username.Trim();

        // Usernames are matched without caring about upper/lower case.
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == name.ToLower());

        if (user == null)
        {
            await LogAsync(db, null, name, LoginActions.FailedLogin, "Unknown username");
            return new SignInResult(null, "Wrong username or password.");
        }

        if (user.Status == UserStatuses.Disabled)
        {
            await LogAsync(db, user.UserId, name, LoginActions.FailedLogin, "Account disabled");
            return new SignInResult(null, "This account is turned off. Please contact an Admin.");
        }

        if (user.Status == UserStatuses.Locked)
        {
            await LogAsync(db, user.UserId, name, LoginActions.FailedLogin, "Account locked");
            return new SignInResult(null, "This account is locked after too many wrong passwords. Please contact an Admin.");
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            user.FailedLoginCount++;
            user.UpdatedAt = DateTime.Now;
            db.LoginLogs.Add(NewLog(user.UserId, name, LoginActions.FailedLogin, $"Wrong password ({user.FailedLoginCount} of {MaxFailedAttempts})"));

            if (user.FailedLoginCount >= MaxFailedAttempts)
            {
                user.Status = UserStatuses.Locked;
                db.LoginLogs.Add(NewLog(user.UserId, name, LoginActions.AccountLocked, $"{MaxFailedAttempts} wrong passwords"));
                await db.SaveChangesAsync();
                return new SignInResult(null, "Too many wrong passwords. This account is now locked. Please contact an Admin.");
            }

            await db.SaveChangesAsync();
            var left = MaxFailedAttempts - user.FailedLoginCount;
            return new SignInResult(null, $"Wrong username or password. {left} attempt{(left == 1 ? "" : "s")} left before the account locks.");
        }

        user.FailedLoginCount = 0;
        user.LastLoginAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;
        db.LoginLogs.Add(NewLog(user.UserId, user.Username, LoginActions.Login, null));
        await db.SaveChangesAsync();

        Session.Start(user);
        return new SignInResult(user, null);
    }

    /// <summary>Signs the current user out. <paramref name="idle"/> = signed out by the "Still there?" timer.</summary>
    public static async Task SignOutAsync(bool idle = false)
    {
        if (Session.CurrentUser is not { } user) return;
        using var db = Db.Open();
        await LogAsync(db, user.UserId, user.Username, idle ? LoginActions.IdleSignOut : LoginActions.Logout, null);
        Session.End();
    }

    static LoginLog NewLog(int? userId, string username, string action, string? details) => new()
    {
        UserId = userId,
        UsernameTried = username,
        Action = action,
        Details = details,
        ComputerName = Environment.MachineName,
        LoggedAt = DateTime.Now,
    };

    static async Task LogAsync(AppDbContext db, int? userId, string username, string action, string? details)
    {
        db.LoginLogs.Add(NewLog(userId, username, action, details));
        await db.SaveChangesAsync();
    }
}
