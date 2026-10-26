using System.Text.Json;
using System.Text.RegularExpressions;
using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>One row of the User Accounts table.</summary>
public record UserRow(int UserId, string FullName, string Username, string? Department, string Level, string Status, DateTime? LastLoginAt);

/// <summary>What the Admin types in the account form.</summary>
public record UserInput(string FullName, string Username, string Level, string? Department);

/// <summary>
/// User Accounts page (Admin only): create logins, edit them, unlock, disable and enable.
/// Rules (Project Outline, Part 0): only Admins create accounts; Admins have no department; Supervisors and
/// Employees have exactly one. There must always be at least one active Admin.
/// Every change goes to audit_logs (never the password).
/// </summary>
public static partial class UserAdminService
{
    public static readonly string[] DepartmentList = [Departments.HrPayroll, Departments.Inventory, Departments.Subcontractors, Departments.Receipts];
    public static readonly string[] LevelList = [UserLevels.Admin, UserLevels.Supervisor, UserLevels.Employee];

    [GeneratedRegex(@"^[A-Za-z0-9._-]{3,50}$")]
    private static partial Regex UsernamePattern();

    public static async Task<List<UserRow>> ListAsync(string search, string? status)
    {
        using var db = Db.Open();
        var q = db.Users.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(u => u.FullName.ToLower().Contains(s) || u.Username.ToLower().Contains(s));
        }
        if (status != null) q = q.Where(u => u.Status == status);
        return await q.OrderBy(u => u.FullName)
            .Select(u => new UserRow(u.UserId, u.FullName, u.Username, u.Department, u.Level, u.Status, u.LastLoginAt))
            .ToListAsync();
    }

    /// <summary>Null when the input is acceptable, otherwise the reason.</summary>
    public static string? CheckInput(UserInput input)
    {
        if (string.IsNullOrWhiteSpace(input.FullName)) return "Type the person's full name.";
        if (input.FullName.Trim().Length > 100) return "The name is too long (100 characters at most).";
        if (!UsernamePattern().IsMatch(input.Username.Trim()))
            return "Username: 3 to 50 characters, only letters, numbers, dots, dashes and underscores (no spaces).";
        if (!LevelList.Contains(input.Level)) return "Choose a level.";
        if (input.Level == UserLevels.Admin && input.Department != null) return "Admins don't belong to a department.";
        if (input.Level != UserLevels.Admin && !DepartmentList.Contains(input.Department)) return "Choose a department.";
        return null;
    }

    /// <summary>
    /// Active accounts already holding this department + level (the plan is 1 Employee and 1 Supervisor per
    /// department). The page asks the Admin to confirm when this isn't empty; it doesn't block.
    /// </summary>
    public static async Task<List<string>> OthersInSameRoleAsync(UserInput input, int? exceptUserId)
    {
        if (input.Level == UserLevels.Admin) return [];
        using var db = Db.Open();
        return await db.Users
            .Where(u => u.Level == input.Level && u.Department == input.Department && u.Status != UserStatuses.Disabled && u.UserId != exceptUserId)
            .Select(u => u.FullName)
            .ToListAsync();
    }

    /// <summary>Creates a login. Returns (new id, null) or (null, error).</summary>
    public static async Task<(int? UserId, string? Error)> CreateAsync(UserInput input, string password)
    {
        if (RequireAdmin() is { } notAdmin) return (null, notAdmin);
        if (CheckInput(input) is { } problem) return (null, problem);
        if (PasswordService.CheckNewPassword(password, password) is { } pwProblem) return (null, pwProblem);

        using var db = Db.Open();
        if (await UsernameTakenAsync(db, input.Username, null)) return (null, "That username is already used.");

        var user = new User
        {
            FullName = input.FullName.Trim(),
            Username = input.Username.Trim(),
            Level = input.Level,
            Department = input.Department,
            Status = UserStatuses.Active,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        Audit(db, AuditActions.Added, user, null, Snapshot(user));
        await db.SaveChangesAsync();
        return (user.UserId, null);
    }

    /// <summary>Changes name, username, level and department. Returns an error message, or null when saved.</summary>
    public static async Task<string?> UpdateAsync(int userId, UserInput input)
    {
        if (RequireAdmin() is { } notAdmin) return notAdmin;
        if (CheckInput(input) is { } problem) return problem;

        using var db = Db.Open();
        var user = await db.Users.FirstAsync(u => u.UserId == userId);
        if (await UsernameTakenAsync(db, input.Username, userId)) return "That username is already used.";

        if (user.IsAdmin && input.Level != UserLevels.Admin)
        {
            if (user.UserId == Session.CurrentUser!.UserId) return "You can't remove your own Admin level.";
            if (await IsLastActiveAdminAsync(db, user)) return "This is the last active Admin. Make another Admin first.";
        }

        var before = Snapshot(user);
        user.FullName = input.FullName.Trim();
        user.Username = input.Username.Trim();
        user.Level = input.Level;
        user.Department = input.Department;
        user.UpdatedAt = DateTime.Now;

        Audit(db, AuditActions.Edited, user, before, Snapshot(user));
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Locked (5 wrong passwords) back to Active.</summary>
    public static async Task<string?> UnlockAsync(int userId)
    {
        if (RequireAdmin() is { } notAdmin) return notAdmin;
        using var db = Db.Open();
        var user = await db.Users.FirstAsync(u => u.UserId == userId);
        if (user.Status != UserStatuses.Locked) return "This account isn't locked.";

        var before = Snapshot(user);
        user.Status = UserStatuses.Active;
        user.FailedLoginCount = 0;
        user.UpdatedAt = DateTime.Now;
        db.LoginLogs.Add(LoginLogFor(user, LoginActions.AccountUnlocked, $"Unlocked by {Session.CurrentUser!.Username}"));
        Audit(db, AuditActions.Edited, user, before, Snapshot(user));
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Turns an account off (e.g. the person left). Their records stay.</summary>
    public static async Task<string?> DisableAsync(int userId)
    {
        if (RequireAdmin() is { } notAdmin) return notAdmin;
        using var db = Db.Open();
        var user = await db.Users.FirstAsync(u => u.UserId == userId);
        if (user.Status == UserStatuses.Disabled) return "This account is already turned off.";
        if (user.UserId == Session.CurrentUser!.UserId) return "You can't turn off your own account.";
        if (user.IsAdmin && await IsLastActiveAdminAsync(db, user)) return "This is the last active Admin. Make another Admin first.";

        var before = Snapshot(user);
        user.Status = UserStatuses.Disabled;
        user.UpdatedAt = DateTime.Now;
        Audit(db, AuditActions.Edited, user, before, Snapshot(user));
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Turns a Disabled account back on.</summary>
    public static async Task<string?> EnableAsync(int userId)
    {
        if (RequireAdmin() is { } notAdmin) return notAdmin;
        using var db = Db.Open();
        var user = await db.Users.FirstAsync(u => u.UserId == userId);
        if (user.Status != UserStatuses.Disabled) return "This account isn't turned off.";

        var before = Snapshot(user);
        user.Status = UserStatuses.Active;
        user.FailedLoginCount = 0;
        user.UpdatedAt = DateTime.Now;
        Audit(db, AuditActions.Edited, user, before, Snapshot(user));
        await db.SaveChangesAsync();
        return null;
    }

    static string? RequireAdmin() =>
        Session.CurrentUser is { IsAdmin: true } ? null : "Only Admins can manage accounts.";

    static Task<bool> UsernameTakenAsync(AppDbContext db, string username, int? exceptUserId)
    {
        var name = username.Trim().ToLower();
        return db.Users.AnyAsync(u => u.Username.ToLower() == name && u.UserId != exceptUserId);
    }

    static async Task<bool> IsLastActiveAdminAsync(AppDbContext db, User admin) =>
        !await db.Users.AnyAsync(u => u.Level == UserLevels.Admin && u.Status == UserStatuses.Active && u.UserId != admin.UserId);

    // The fields shown in Edit History. Never the password hash or security answer.
    static object Snapshot(User u) => new
    {
        full_name = u.FullName,
        username = u.Username,
        level = u.Level,
        department = u.Department,
        status = u.Status,
    };

    static void Audit(AppDbContext db, string action, User user, object? before, object? after) =>
        db.AuditLogs.Add(new AuditLog
        {
            UserId = Session.CurrentUser?.UserId,
            TableName = "users",
            RecordId = user.UserId,
            RecordName = user.FullName,
            Action = action,
            OldValues = before == null ? null : JsonSerializer.Serialize(before),
            NewValues = after == null ? null : JsonSerializer.Serialize(after),
            ChangedAt = DateTime.Now,
        });

    static LoginLog LoginLogFor(User user, string action, string details) => new()
    {
        UserId = user.UserId,
        UsernameTried = user.Username,
        Action = action,
        Details = details,
        ComputerName = Environment.MachineName,
        LoggedAt = DateTime.Now,
    };
}
