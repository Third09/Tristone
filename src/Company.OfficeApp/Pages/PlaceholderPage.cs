namespace Company.OfficeApp.Pages;

/// <summary>Shown in the main area for pages that aren't built yet.</summary>
public partial class PlaceholderPage : UserControl
{
    public PlaceholderPage(string pageTitle)
    {
        InitializeComponent();
        lblPlaceholder.Text = $"{pageTitle}: not built yet.";
    }
}
