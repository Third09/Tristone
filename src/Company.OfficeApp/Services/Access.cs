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

    UserAccounts,
    MyAccount,
}

/// <summary>
/// Who can see and do what (owner decision 2026-11-01). The one place these rules live: the sidebar, every page
/// (<see cref="LockButtons"/>) and every service ask here.
/// - Employees work in their own department: add and edit records, prepare requests. No approving, no deleting.
/// - Supervisors do everything their Employee does, plus approve/reject and delete, in their own department only.
///   The Receipts Supervisor also manages Projects and approves change orders.
/// - Admins see every page but change nothing there; they only handle accounts (User Accounts, password requests,
///   their own password and security question).
/// </summary>
public static class Access
{
    public static bool CanSee(User user, AppPage page)
    {
        if (user.IsAdmin) return true;

        return page switch
        {
            AppPage.Dashboard => user.Level == UserLevels.Supervisor,
            AppPage.Projects => IsSupervisorOf(user, Departments.Receipts),
            AppPage.HrPayroll => user.Department == Departments.HrPayroll,
            AppPage.Inventory => user.Department == Departments.Inventory,
            AppPage.Subcontractors => user.Department == Departments.Subcontractors,
            AppPage.Receipts => user.Department == Departments.Receipts,
            AppPage.MyAccount => true,
            _ => false,   // User Accounts: Admin only
        };
    }

    /// <summary>Can add and edit records on a page (Employees and Supervisors of that department; Admins only on User Accounts).</summary>
    public static bool CanEdit(User user, AppPage page) => page switch
    {
        AppPage.UserAccounts => user.IsAdmin,
        AppPage.MyAccount => true,
        AppPage.Dashboard => false,
        _ => !user.IsAdmin && CanSee(user, page),
    };

    /// <summary>Can delete records (and mark them inactive) on a page: that department's Supervisor; Admins on User Accounts.</summary>
    public static bool CanDelete(User user, AppPage page) =>
        page == AppPage.UserAccounts ? user.IsAdmin : CanEdit(user, page) && user.Level == UserLevels.Supervisor;

    /// <summary>
    /// Can approve a department's requests (material usage, purchases, vouchers, receipts, change orders, cash advances,
    /// overtime, payroll) and change its settings: that department's Supervisor only.
    /// </summary>
    public static bool IsApprover(User user, string department) => IsSupervisorOf(user, department);

    /// <summary>Sees a department's Edit History tab: Admins and that department's Supervisor.</summary>
    public static bool CanSeeHistory(User user, string department) => user.IsAdmin || IsSupervisorOf(user, department);

    static bool IsSupervisorOf(User user, string department) =>
        user.Level == UserLevels.Supervisor && user.Department == department;

    /// <summary>Approver check for the signed-in user; returns the message to show, or null when allowed.</summary>
    public static string? RequireApprover(string department) =>
        Session.CurrentUser is { } u && IsApprover(u, department) ? null : $"Only the {department} Supervisor can do this.";

    /// <summary>Page check for the signed-in user (reading); returns the message to show, or null when allowed.</summary>
    public static string? RequirePage(AppPage page) =>
        Session.CurrentUser is { } u && CanSee(u, page) ? null : $"Only Admins and the {Title(page)} department can do this.";

    /// <summary>Add/edit check for the signed-in user; returns the message to show, or null when allowed.</summary>
    public static string? RequireEdit(AppPage page) =>
        Session.CurrentUser is { } u && CanEdit(u, page) ? null
            : Session.CurrentUser is { IsAdmin: true } ? "Admins can view this page but not change it."
            : page == AppPage.Projects ? "Only the Receipts Supervisor can change projects."
            : $"Only the {Title(page)} department can do this.";

    /// <summary>Delete check for the signed-in user; returns the message to show, or null when allowed.</summary>
    public static string? RequireDelete(AppPage page) =>
        Session.CurrentUser is { } u && CanDelete(u, page) ? null
            : Session.CurrentUser is { IsAdmin: true } ? "Admins can view this page but not change it."
            : $"Only the {(page == AppPage.Projects ? "Receipts" : Title(page))} Supervisor can delete this.";

    /// <summary>
    /// Hides the buttons the signed-in user may not use on a page (every button inside <paramref name="root"/>,
    /// tabs and pop-ups included), sorted by name: Export/Print/View/Open/Preview/Report/Zoom/Close/Cancel/Refresh
    /// buttons stay for everyone; Delete/MarkInactive/Reactivate need <see cref="CanDelete"/>; every other
    /// button adds or changes something and needs <see cref="CanEdit"/>. Approve/Reject buttons are shown by each page
    /// (<see cref="IsApprover"/>). When the user can't edit, record forms stay locked too (<see cref="UI.RecordForm.SetEnabled"/>).
    /// Call at the end of the page's constructor.
    /// </summary>
    public static void LockButtons(Control root, AppPage page)
    {
        var u = Session.CurrentUser!;
        bool edit = CanEdit(u, page), delete = CanDelete(u, page);
        if (!edit) UI.RecordForm.MarkReadOnly(root);
        foreach (var b in Buttons(root))
        {
            var name = b.Name.StartsWith("btn") ? b.Name[3..] : b.Name;
            if (ViewOnly.Any(name.StartsWith) || name.EndsWith("Pdf") || name.StartsWith("Export")) continue;
            if (name.StartsWith("Approve") || name.StartsWith("Reject") || name.StartsWith("Finalize")) continue;
            bool allowed = DeleteKinds.Any(name.StartsWith) ? delete : edit;
            if (!allowed) b.Visible = false;
        }
    }

    static readonly string[] ViewOnly = ["Export", "Print", "View", "Open", "Preview", "Report", "Zoom", "Close", "Cancel", "Refresh"];
    static readonly string[] DeleteKinds = ["Delete", "MarkInactive", "Reactivate"];

    static IEnumerable<Button> Buttons(Control c)
    {
        foreach (Control child in c.Controls)
        {
            if (child is Button b) yield return b;
            foreach (var inner in Buttons(child)) yield return inner;
        }
    }

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

        AppPage.UserAccounts => "User Accounts",
        AppPage.MyAccount => "My Account",
        _ => page.ToString(),
    };
}
