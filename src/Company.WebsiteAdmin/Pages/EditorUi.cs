using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Editors with Save Changes. The main window asks before leaving one with unsaved edits and before
/// Publish Website, so nothing typed is lost or left out by accident.
/// </summary>
public interface IWebsiteEditor
{
    bool HasUnsavedChanges { get; }
}

/// <summary>Small helpers shared by the website editors.</summary>
static class EditorUi
{
    /// <summary>Limits a text box and keeps its "12/150" counter up to date.</summary>
    public static void Counter(TextBox box, Label counter, int max)
    {
        box.MaxLength = max;
        void Show() => counter.Text = $"{box.TextLength}/{max}";
        box.TextChanged += (_, _) => Show();
        Show();
    }

    /// <summary>Asks for one picture (JPEG or PNG). Null = cancelled or not usable (the reason is shown).</summary>
    public static string? PickPicture(IWin32Window owner)
    {
        using var dlg = new OpenFileDialog { Filter = WebsiteImages.Filter, Title = "Choose a picture" };
        if (dlg.ShowDialog(owner) != DialogResult.OK) return null;
        if (WebsiteImages.Check(dlg.FileName) is { } problem)
        {
            MessageBox.Show(owner, problem, "Picture", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }
        return dlg.FileName;
    }

    /// <summary>Shows a picture in a PictureBox, disposing the previous one.</summary>
    public static void ShowPicture(PictureBox box, Image? image)
    {
        var old = box.Image;
        box.Image = image;
        old?.Dispose();
    }

    public static string SavedText(SavedStamp? s) => s == null ? "Last saved: -" : $"Last saved: {s.At:MMM d, yyyy h:mm tt} by {s.By}";

    public const string Unsaved = "Unsaved changes: remember to click Save Changes.";
}
