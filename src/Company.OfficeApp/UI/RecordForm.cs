using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Company.OfficeApp.UI;

/// <summary>What the person answered when asked to confirm an edit.</summary>
public enum SaveAnswer { NothingChanged, Yes, No }

/// <summary>
/// The safe add/edit pattern for list pages (owner-reported bug 2026-10-30: a list that selected its first row by itself
/// left the form silently editing that record, so typing a "new" one overwrote it). Every list page:
/// - never lets the grid pick a row by itself, and locks the form until a row is clicked (<see cref="SetEnabled"/>);
/// - adds in a pop-up: a second copy of the same page in "add only" mode (<see cref="ShowFormOnly"/>, <see cref="AddDialog"/>),
///   so each form exists once;
/// - asks before saving an edit, listing every change (<see cref="ConfirmSave"/>).
/// Pages have no panels: everything from the form's title label down is the form.
/// </summary>
public static partial class RecordForm
{
    /// <summary>Locks or unlocks the form: every control from <paramref name="formTop"/> down, except that title itself.</summary>
    public static void SetEnabled(Control page, Control formTop, bool on)
    {
        foreach (Control c in page.Controls)
            if (c != formTop && c.Top >= formTop.Top) c.Enabled = on;
    }

    /// <summary>
    /// Turns a page into its "add only" copy for the pop-up: hides the list (everything above <paramref name="formTop"/>)
    /// and the parts that only make sense for a saved record (<paramref name="alsoHide"/>), moves the form to the top
    /// and sizes the page to it. Call in the constructor instead of Responsive.FitGrid.
    /// </summary>
    public static void ShowFormOnly(UserControl page, Control formTop, params Control[] alsoHide)
    {
        int shift = formTop.Top - 12;
        var shown = new List<Control>();
        foreach (Control c in page.Controls)
        {
            if (c.Top < formTop.Top || alsoHide.Contains(c)) { c.Visible = false; continue; }
            c.Top -= shift;
            shown.Add(c);
        }
        page.AutoScroll = false;
        page.Size = new Size(shown.Max(c => c.Right) + 12, shown.Max(c => c.Bottom) + 12);
    }

    /// <summary>
    /// Shows an "add only" page in a pop-up window. <paramref name="added"/> is the page's Added event:
    /// the window closes when the page reports the new record's id, which is returned (null = cancelled).
    /// </summary>
    public static int? AddDialog(IWin32Window owner, string title, UserControl page, Action<Action<int>> subscribeAdded)
    {
        using var f = new Form
        {
            Text = title, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false,
            ShowInTaskbar = false, StartPosition = FormStartPosition.CenterParent, AutoScaleMode = AutoScaleMode.None,
        };
        var area = Screen.FromHandle(owner.Handle).WorkingArea;
        f.ClientSize = new Size(Math.Min(page.Width, area.Width - 40), Math.Min(page.Height, area.Height - 60));
        f.AutoScroll = true;   // a tall form scrolls on a small screen
        page.Location = new Point(0, 0);
        f.Controls.Add(page);
        int? newId = null;
        subscribeAdded(id => { newId = id; f.DialogResult = DialogResult.OK; });
        f.ShowDialog(owner);
        return newId;
    }

    /// <summary>
    /// Compares what the form held when the record was loaded with what it holds now and, if anything changed,
    /// asks "Save these changes to (name)?" with one line per change. <paramref name="privateFields"/> (government IDs,
    /// TIN, bank account numbers) are listed as "changed", never shown.
    /// </summary>
    public static SaveAnswer ConfirmSave(IWin32Window owner, string recordName, object before, object after,
                                         IReadOnlyDictionary<string, string>? labels = null, params string[] privateFields)
    {
        var lines = Describe(before, after, labels, privateFields);
        if (lines.Count == 0) return SaveAnswer.NothingChanged;
        var ok = MessageBox.Show(owner,
            $"Save these changes to {recordName}?\n\n" + string.Join("\n", lines.Select(l => "- " + l)),
            "Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        return ok == DialogResult.Yes ? SaveAnswer.Yes : SaveAnswer.No;
    }

    /// <summary>One line per changed value of two form snapshots (records of the same type).</summary>
    public static List<string> Describe(object before, object after, IReadOnlyDictionary<string, string>? labels = null,
                                        params string[] privateFields)
    {
        var lines = new List<string>();
        foreach (var p in before.GetType().GetProperties().Where(p => p.CanRead && p.GetIndexParameters().Length == 0))
        {
            if (p.Name == "EqualityContract") continue;
            string a = Show(p.GetValue(before)), b = Show(p.GetValue(after));
            if (a == b) continue;
            var label = labels != null && labels.TryGetValue(p.Name, out var l) ? l : Humanize(p.Name);
            // Ids (a project or subcontractor picked from a list) mean nothing to a person; privates must not be shown.
            if (privateFields.Contains(p.Name) || p.Name.EndsWith("Id")) lines.Add($"{label}: changed");
            else lines.Add($"{label}: {a} -> {b}");
        }
        return lines;
    }

    static string Show(object? v) => v switch
    {
        null => "(empty)",
        string s => s.Trim() == "" ? "(empty)" : s.Trim(),
        decimal d => d.ToString("#,##0.##", CultureInfo.InvariantCulture),
        DateOnly d => d.ToString("d MMM yyyy", CultureInfo.InvariantCulture),
        DateTime d => d.ToString("d MMM yyyy", CultureInfo.InvariantCulture),
        bool b => b ? "Yes" : "No",
        IEnumerable list => string.Join(", ", list.Cast<object>().Select(Show)) is var j && j != "" ? j : "(none)",
        // A small record inside a list, e.g. a payment stage: "Down payment 30".
        _ when v.GetType().IsClass => string.Join(" ", v.GetType().GetProperties()
                                          .Where(p => p.Name != "EqualityContract").Select(p => Show(p.GetValue(v)))),
        _ => v.ToString() ?? "",
    };

    /// <summary>"ContractPrice" -> "Contract price".</summary>
    static string Humanize(string name)
    {
        var words = Words().Replace(name, " $1").Trim();
        return char.ToUpper(words[0]) + words[1..].ToLower();
    }

    [GeneratedRegex("(?<!^)([A-Z])")]
    private static partial Regex Words();
}
