using Company.Shared.Models;

namespace Company.OfficeApp.Services;

/// <summary>The pages in the sidebar.</summary>
public enum AppPage
{
    Dashboard,
    Projects,
    HrPayroll,
    Inventory,
    Subcontractors,
    Receipts,
    WebsiteAdmin,
    UserAccounts,
    MyAccount,
}

/// <summary>
/// Who can open which page (Project Outline, Part 0): Admins see everything; Supervisors and Employees see
/// only their own department's page; Supervisors also see the Dashboard (their department's numbers).
/// The one place these rules live: the sidebar and every page ask here.
/// </summary>
public static class Access
{
    public static bool CanSee(User user, AppPage page)
    {
        if (user.IsAdmin) return true;

        return page switch
        {
            AppPage.Dashboard => user.Level == UserLevels.Supervisor,
            AppPage.HrPayroll => user.Department == Departments.HrPayroll,
            AppPage.Inventory => user.Department == Departments.Inventory,
            AppPage.Subcontractors => user.Department == Departments.Subcontractors,
            AppPage.Receipts => user.Department == Departments.Receipts,
            AppPage.MyAccount => true,
            _ => false,   // Projects, Website Admin, User Accounts: Admin only
        };
    }

    /// <summary>
    /// Can approve a department's requests (material usage, purchases, vouchers, receipts, cash advances):
    /// Admins, and that department's Supervisor.
    /// </summary>
    public static bool IsApprover(User user, string department) =>
        user.IsAdmin || (user.Level == UserLevels.Supervisor && user.Department == department);

    /// <summary>Approver check for the signed-in user; returns the message to show, or null when allowed.</summary>
    public static string? RequireApprover(string department) =>
        Session.CurrentUser is { } u && IsApprover(u, department) ? null : $"Only an Admin or the {department} Supervisor can approve this.";

    /// <summary>Page check for the signed-in user; returns the message to show, or null when allowed.</summary>
    public static string? RequirePage(AppPage page) =>
        Session.CurrentUser is { } u && CanSee(u, page) ? null : $"Only Admins and the {Title(page)} department can do this.";

    /// <summary>The page shown right after signing in.</summary>
    public static AppPage FirstPage(User user)
    {
        if (CanSee(user, AppPage.Dashboard)) return AppPage.Dashboard;
        return user.Department switch
        {
            Departments.HrPayroll => AppPage.HrPayroll,
            Departments.Inventory => AppPage.Inventory,
            Departments.Subcontractors => AppPage.Subcontractors,
            Departments.Receipts => AppPage.Receipts,
            _ => AppPage.MyAccount,
        };
    }

    public static string Title(AppPage page) => page switch
    {
        AppPage.HrPayroll => "HR & Payroll",
        AppPage.WebsiteAdmin => "Website Admin",
        AppPage.UserAccounts => "User Accounts",
        AppPage.MyAccount => "My Account",
        _ => page.ToString(),
    };
}
