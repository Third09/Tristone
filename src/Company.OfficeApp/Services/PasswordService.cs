using System.Security.Cryptography;
using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>A waiting Forgot Password request, with the person's details for the Admin's list.</summary>
public record PasswordRequestRow(int RequestId, int UserId, string FullName, string Username, string? Department, string Level, DateTime RequestedAt);

/// <summary>
/// Forgot Password and Admin password changes.
/// Rules (Project Outline, Part 0): Employees and Supervisors can't reset their own password; Forgot Password
/// only sends a request to the Admins, and an Admin sets a new one. Admins recover their own account with
/// their security question.
/// </summary>
public static class PasswordService
{
    public const int MinPasswordLength = 8;

    /// <summary>Wrong security answers allowed before only another Admin can reset the account.</summary>
    public const int MaxSecurityAnswerFailures = 10;

    public static async Task<User?> FindUserAsync(string username)
    {
        using var db = Db.Open();
        var name = username.Trim().ToLower();
        return await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username.ToLower() == name);
    }

    /// <summary>Employee / Supervisor: sends a request to the Admins. Returns false if one is already waiting.</summary>
    public static async Task<bool> RequestResetAsync(User user)
    {
        using var db = Db.Open();
        if (await db.PasswordRequests.AnyAsync(r => r.UserId == user.UserId && r.Status == PasswordRequestStatuses.Waiting))
            return false;

        db.PasswordRequests.Add(new PasswordRequest { UserId = user.UserId, RequestedAt = DateTime.Now });
        db.LoginLogs.Add(Log(user.UserId, user.Username, LoginActions.PasswordRequest, "Forgot Password request sent to Admins"));
        await db.SaveChangesAsync();
        return true;
    }

    /// <summary>Admin only: resets their own password with their security question. Returns an error message, or null when done.</summary>
    public static async Task<string?> ResetWithSecurityAnswerAsync(int userId, string answer, string newPassword, string confirm)
    {
        if (CheckNewPassword(newPassword, confirm) is { } problem) return problem;

        using var db = Db.Open();
        var user = await db.Users.FirstAsync(u => u.UserId == userId);

        if (!user.IsAdmin || user.SecurityAnswerHash == null) return "This account can't use a security question.";
        if (user.Status == UserStatuses.Disabled) return "This account is turned off. Ask another Admin.";
        if (user.FailedLoginCount >= MaxSecurityAnswerFailures) return "Too many wrong attempts. Ask another Admin to reset your password.";

        if (!BCrypt.Net.BCrypt.Verify(NormalizeAnswer(answer), user.SecurityAnswerHash))
        {
            user.FailedLoginCount++;
            user.UpdatedAt = DateTime.Now;
            db.LoginLogs.Add(Log(user.UserId, user.Username, LoginActions.FailedLogin, "Wrong security answer"));
            await db.SaveChangesAsync();
            return "That answer is not correct.";
        }

        await ApplyNewPasswordAsync(db, user, newPassword, "Reset with security question");
        return null;
    }

    /// <summary>Waiting requests, oldest first. Drives the Admin's notification count.</summary>
    public static async Task<List<PasswordRequestRow>> GetWaitingRequestsAsync()
    {
        using var db = Db.Open();
        return await (from r in db.PasswordRequests
                      join u in db.Users on r.UserId equals u.UserId
                      where r.Status == PasswordRequestStatuses.Waiting
                      orderby r.RequestedAt
                      select new PasswordRequestRow(r.RequestId, u.UserId, u.FullName, u.Username, u.Department, u.Level, r.RequestedAt))
                     .ToListAsync();
    }

    public static async Task<int> CountWaitingRequestsAsync()
    {
        using var db = Db.Open();
        return await db.PasswordRequests.CountAsync(r => r.Status == PasswordRequestStatuses.Waiting);
    }

    /// <summary>
    /// Admin sets someone's new password. Also unlocks a Locked account and closes any waiting request.
    /// Returns an error message, or null when done.
    /// </summary>
    public static async Task<string?> SetNewPasswordAsync(int targetUserId, string newPassword)
    {
        if (CheckNewPassword(newPassword, newPassword) is { } problem) return problem;
        var admin = Session.CurrentUser;
        if (admin is not { IsAdmin: true }) return "Only an Admin can change passwords.";

        using var db = Db.Open();
        var user = await db.Users.FirstAsync(u => u.UserId == targetUserId);
        await ApplyNewPasswordAsync(db, user, newPassword, $"Set by {admin.Username}");
        return null;
    }

    /// <summary>Admin closes a request without changing the password (e.g. sent by mistake). It stays in the history.</summary>
    public static async Task DismissRequestAsync(int requestId)
    {
        using var db = Db.Open();
        var r = await db.PasswordRequests.FirstAsync(x => x.RequestId == requestId);
        r.Status = PasswordRequestStatuses.Dismissed;
        r.HandledBy = Session.CurrentUser?.UserId;
        r.HandledAt = DateTime.Now;
        await db.SaveChangesAsync();
    }

    /// <summary>Null when the new password is acceptable, otherwise the reason.</summary>
    public static string? CheckNewPassword(string password, string confirm)
    {
        if (password.Length < MinPasswordLength) return $"The new password must be at least {MinPasswordLength} characters.";
        if (!password.Any(char.IsDigit) || !password.Any(char.IsLetter)) return "The new password must have at least one letter and one number.";
        if (password != confirm) return "The two passwords don't match.";
        return null;
    }

    /// <summary>A random 10-character password without look-alike characters (no 0/O, 1/l/I).</summary>
    public static string GeneratePassword()
    {
        const string letters = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
        const string digits = "23456789";
        var chars = new char[10];
        for (int i = 0; i < chars.Length; i++)
            chars[i] = i < 2 ? digits[RandomNumberGenerator.GetInt32(digits.Length)]
                             : letters[RandomNumberGenerator.GetInt32(letters.Length)];
        RandomNumberGenerator.Shuffle(chars.AsSpan());
        return new string(chars);
    }

    /// <summary>Security answers ignore spaces at the ends and upper/lower case ("Manila " = "manila").</summary>
    public static string NormalizeAnswer(string answer) => answer.Trim().ToLowerInvariant();

    static async Task ApplyNewPasswordAsync(AppDbContext db, User user, string newPassword, string details)
    {
        var wasLocked = user.Status == UserStatuses.Locked;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 11);
        user.FailedLoginCount = 0;
        if (wasLocked) user.Status = UserStatuses.Active;
        user.UpdatedAt = DateTime.Now;

        db.LoginLogs.Add(Log(user.UserId, user.Username, LoginActions.PasswordChanged, details));
        if (wasLocked) db.LoginLogs.Add(Log(user.UserId, user.Username, LoginActions.AccountUnlocked, details));

        foreach (var r in await db.PasswordRequests.Where(r => r.UserId == user.UserId && r.Status == PasswordRequestStatuses.Waiting).ToListAsync())
        {
            r.Status = PasswordRequestStatuses.Done;
            r.HandledBy = Session.CurrentUser?.UserId;
            r.HandledAt = DateTime.Now;
        }

        await db.SaveChangesAsync();
    }

    static LoginLog Log(int userId, string username, string action, string details) => new()
    {
        UserId = userId,
        UsernameTried = username,
        Action = action,
        Details = details,
        ComputerName = Environment.MachineName,
        LoggedAt = DateTime.Now,
    };
}
