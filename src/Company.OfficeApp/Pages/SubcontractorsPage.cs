using Company.OfficeApp.Pages.Shared;
using Company.OfficeApp.Pages.Subcontractors;
using Company.OfficeApp.Services;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages;

/// <summary>
/// Subcontractors (Admin and the Subcontractors department). 4 tabs; each tab's content is its own UserControl
/// in Pages/Subcontractors. Edit History is for Admins and the Subcontractors Supervisor only.
/// The Directory tab title carries the PCAB reminder: "Directory (1 PCAB expiring)".
/// </summary>
public partial class SubcontractorsPage : UserControl
{
    readonly SubDirectoryTab directory = new() { Dock = DockStyle.Fill };
    readonly SubJobsTab jobs = new() { Dock = DockStyle.Fill };
    readonly SubBillingTab billing = new() { Dock = DockStyle.Fill };

    public SubcontractorsPage()
    {
        InitializeComponent();
        tabSubDirectory.Controls.Add(directory);
        tabSubJobs.Controls.Add(jobs);
        tabSubBilling.Controls.Add(billing);
        tabSubEditHistory.Controls.Add(new EditHistoryTab(Departments.Subcontractors) { Dock = DockStyle.Fill });

        if (!Access.IsApprover(Session.CurrentUser!, Departments.Subcontractors))
            tabSubcontractors.TabPages.Remove(tabSubEditHistory);

        // A change in one tab refreshes the tabs that show the same data.
        directory.Changed += async (_, _) => { await jobs.ReloadListsAsync(); await billing.ReloadJobsAsync(); await RefreshAlertsAsync(); };
        jobs.Changed += async (_, _) => await billing.ReloadJobsAsync();
        billing.Changed += async (_, _) => await jobs.ReloadListsAsync();
    }

    async void SubcontractorsPage_Load(object sender, EventArgs e) => await RefreshAlertsAsync();

    async Task RefreshAlertsAsync()
    {
        var due = await SubcontractorService.PcabDueCountAsync(DateOnly.FromDateTime(DateTime.Today));
        tabSubDirectory.Text = due == 0 ? "Directory" : $"Directory ({due} PCAB expiring)";
    }
}
