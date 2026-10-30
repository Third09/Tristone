using Company.OfficeApp.Services;

namespace Company.Launcher;

/// <summary>Which app the launcher opens.</summary>
public enum ChosenApp { None, Office, Website }

/// <summary>
/// Shown after an Admin signs in: Admins can open either app (owner decision 2026-10-30).
/// Plain layout for now, like the other windows.
/// </summary>
public partial class AppChoiceForm : Form
{
    public ChosenApp Choice { get; private set; } = ChosenApp.None;

    public AppChoiceForm()
    {
        InitializeComponent();
        if (Session.CurrentUser is { } u) lblChoiceWho.Text = $"Signed in as {u.FullName} (Admin).";
    }

    /// <summary>Asks the signed-in Admin which app to open; None = Sign Out.</summary>
    public static ChosenApp Ask()
    {
        using var f = new AppChoiceForm();
        f.ShowDialog();
        return f.Choice;
    }

    void btnOpenOffice_Click(object sender, EventArgs e) { Choice = ChosenApp.Office; Close(); }

    void btnOpenWebsite_Click(object sender, EventArgs e) { Choice = ChosenApp.Website; Close(); }

    void btnChoiceSignOut_Click(object sender, EventArgs e) { Choice = ChosenApp.None; Close(); }
}
