using System.Text.RegularExpressions;
using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>One row of the User Accounts table.</summary>
public record UserRow(int UserId, string FullName, string Username, string? Department, string Level, string Status, DateTime? LastLoginAt, int? EmployeeId);

/// <summary>
/// What the Admin picks in the account form. EmployeeId = the office staff member the login belongs to
/// (null only for old accounts made before employees existed, like the first Admin).
/// </summary>
public record UserInput(int? EmployeeId, string Username, string Level);

/// <summary>An office staff member who can be given a login (shown in the Employee dropdown).</summary>
public record EmployeeOption(int? EmployeeId, string FullName, string Department)
{
    public override string ToString() => EmployeeId == null ? FullName : $"{FullName} ({Department})";
}

/// <summary>
/// User Accounts page (Admin only): create logins, edit them, unlock, disable and enable.
/// Rules (Project Outline, Part 0): only Admins create accounts; every login belongs to one Active office
/// employee (never a site worker), one login per employee; the name and department come from the employee.
/// Admins have no department; Supervisors and Employees get their employee's department.
/// There must always be at least one active Admin. Every change goes to audit_logs (never the password).
/// </summary>
public static partial class UserAdminService
{
    public static readonly string[] DepartmentList = [Departments.HrPayroll, Departments.Inventory, Departments.Subcontractors, Departments.Receipts];
    public static readonly string[] LevelList = [UserLevels.Admin, UserLevels.Supervisor, UserLevels.Employee];

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
            .Select(u => new UserRow(u.UserId, u.FullName, u.Username, u.Department, u.Level, u.Status, u.LastLoginAt, u.EmployeeId))
            .ToListAsync();
    }

    /// <summary>
    /// Active office staff with no login yet (plus the one already linked to <paramref name="forUserId"/>),
    /// for the Employee dropdown. Site workers never get logins.
    /// </summary>
    public static async Task<List<EmployeeOption>> LinkableEmployeesAsync(int? forUserId)
    {
        using var db = Db.Open();
        var current = forUserId == null ? null
            : await db.Users.Where(u => u.UserId == forUserId).Select(u => u.EmployeeId).FirstOrDefaultAsync();
        return await db.Employees
            .Where(e => (e.Status == EmployeeStatuses.Active && e.Department != EmployeeDepartments.Site
                         && !db.Users.Any(u => u.EmployeeId == e.EmployeeId))
                        || e.EmployeeId == current)
            .OrderBy(e => e.FullName)
            .Select(e => new EmployeeOption(e.EmployeeId, e.FullName, e.Department))
            .ToListAsync();
    }

    /// <summary>Null when the username and level are acceptable, otherwise the reason. (The employee is checked when saving.)</summary>
    public static string? CheckInput(UserInput input)
    {
        if (Company.Shared.AccountRules.CheckUsername(input.Username) is { } badName) return badName;
        if (!LevelList.Contains(input.Level)) return "Choose a level.";
        return null;
    }

    /// <summary>The login's department: none for Admins, otherwise the employee's (must be a login department).</summary>
    public static string? DepartmentFor(string level, string employeeDepartment) =>
        level == UserLevels.Admin ? null : employeeDepartment;

    /// <summary>
    /// Works out the login's full name and department from the chosen employee and checks the employee rules.
    /// <paramref name="existing"/> is the account being edited (null when creating).
    /// </summary>
    static async Task<(string FullName, string? Department, string? Error)> ResolveAsync(AppDbContext db, UserInput input, User? existing)
    {
        if (input.EmployeeId is not { } employeeId)
        {
            // Only old accounts made before employees existed may stay unlinked.
            if (existing == null || existing.EmployeeId != null) return ("", null, "Choose the employee this login is for.");
            if (input.Level != UserLevels.Admin && existing.Department == null)
                return ("", null, "Link this account to an employee to give it a department.");
            return (existing.FullName, input.Level == UserLevels.Admin ? null : existing.Department, null);
        }

        var e = await db.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
        if (e == null) return ("", null, "That employee no longer exists.");
        if (e.Department == EmployeeDepartments.Site) return ("", null, "Site workers don't get logins.");
        if (e.Status != EmployeeStatuses.Active) return ("", null, $"{e.FullName} is Inactive.");
        if (await db.Users.AnyAsync(u => u.EmployeeId == employeeId && u.UserId != (existing == null ? 0 : existing.UserId)))
            return ("", null, $"{e.FullName} already has a login.");
        if (input.Level != UserLevels.Admin && !DepartmentList.Contains(e.Department))
            return ("", null, $"{e.FullName} works in {e.Department}, which has no Supervisor or Employee logins. Choose level Admin, or change their department in HR & Payroll first.");

        return (e.FullName, DepartmentFor(input.Level, e.Department), null);
    }

    /// <summary>
    /// Active accounts already holding this department + level (the plan is 1 Employee and 1 Supervisor per
    /// department). The page asks the Admin to confirm when this isn't empty; it doesn't block.
    /// </summary>
    public static async Task<List<string>> OthersInSameRoleAsync(string level, string? department, int? exceptUserId)
    {
        if (level == UserLevels.Admin || department == null) return [];
        using var db = Db.Open();
        return await db.Users
            .Where(u => u.Level == level && u.Department == department && u.Status != UserStatuses.Disabled && u.UserId != exceptUserId)
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
        var (fullName, department, error) = await ResolveAsync(db, input, null);
        if (error != null) return (null, error);

        var user = new User
        {
            EmployeeId = input.EmployeeId,
            FullName = fullName,
            Username = input.Username.Trim(),
            Level = input.Level,
            Department = department,
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

    /// <summary>Changes the linked employee, username and level. Returns an error message, or null when saved.</summary>
    public static async Task<string?> UpdateAsync(int userId, UserInput input)
    {
        if (RequireAdmin() is { } notAdmin) return notAdmin;
        if (CheckInput(input) is { } problem) return problem;

        using var db = Db.Open();
        var user = await db.Users.FirstAsync(u => u.UserId == userId);
        if (await UsernameTakenAsync(db, input.Username, userId)) return "That username is already used.";
        var (fullName, department, error) = await ResolveAsync(db, input, user);
        if (error != null) return error;

        if (user.IsAdmin && input.Level != UserLevels.Admin)
        {
            if (user.UserId == Session.CurrentUser!.UserId) return "You can't remove your own Admin level.";
            if (await IsLastActiveAdminAsync(db, user)) return "This is the last active Admin. Make another Admin first.";
        }

        var before = Snapshot(user);
        user.EmployeeId = input.EmployeeId;
        user.FullName = fullName;
        user.Username = input.Username.Trim();
        user.Level = input.Level;
        user.Department = department;
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
        employee_id = u.EmployeeId,
        full_name = u.FullName,
        username = u.Username,
        level = u.Level,
        department = u.Department,
        status = u.Status,
    };

    static void Audit(AppDbContext db, string action, User user, object? before, object? after) =>
        AuditService.Add(db, null, "users", user.UserId, user.FullName, action, before, after);

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
