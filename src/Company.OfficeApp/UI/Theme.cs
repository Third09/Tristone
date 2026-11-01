using System.Reflection;
using System.Runtime.CompilerServices;

namespace Company.OfficeApp.UI;

/// <summary>
/// The app's look in one place, like a CSS file (owner request 2026-11-01; colors from the Figma "CCMS SUPERSYSTEM").
/// <see cref="Enable"/> (called once at start-up) styles every window as it opens and every control added later
/// (pages loaded into the main window, pop-ups). Change a color here and every screen follows.
///
/// Rules:
/// - Only the look changes (colors, borders, flat buttons, table style). Sizes and positions stay as designed,
///   and the font stays the same size, so no layout moves.
/// - Anything set by hand in the Visual Studio designer wins: a BackColor / ForeColor / Font that isn't the
///   Windows default is left alone (like an inline style beating CSS).
/// - Buttons are styled by NAME (the same names the UI Elements Guide uses): see <see cref="KindOf"/>.
/// </summary>
public static class Theme
{
    // ----- colors (Figma) -----
    public static readonly Color Orange = ColorTranslator.FromHtml("#F47A20");       // main buttons, open page
    public static readonly Color OrangeDark = ColorTranslator.FromHtml("#D9650F");   // main buttons under the mouse
    public static readonly Color OrangeLight = ColorTranslator.FromHtml("#FFF1E6");  // selected table row, hover
    public static readonly Color Green = ColorTranslator.FromHtml("#1F5C4C");        // Publish Website
    public static readonly Color GreenDark = ColorTranslator.FromHtml("#174639");
    public static readonly Color Red = ColorTranslator.FromHtml("#D92D20");          // Delete, Reject, Log out
    public static readonly Color Page = Color.White;                                 // window and page background
    public static readonly Color Text = ColorTranslator.FromHtml("#1D2939");
    public static readonly Color TextMuted = ColorTranslator.FromHtml("#667085");
    public static readonly Color Border = ColorTranslator.FromHtml("#D0D5DD");
    public static readonly Color Line = ColorTranslator.FromHtml("#EAECF0");         // table lines
    public static readonly Color HeaderBack = ColorTranslator.FromHtml("#F9FAFB");   // table headers
    public static readonly Color Hover = ColorTranslator.FromHtml("#F2F4F7");        // plain buttons under the mouse

    public const int RowHeight = 30;
    public const int HeaderHeight = 34;

    public enum ButtonKind { Plain, Main, Publish, Danger, Nav, NavDanger }

    /// <summary>Which style a button gets, from its name (btnSave... = Main, btnDelete... = Danger, btnNav... = sidebar).</summary>
    public static ButtonKind KindOf(Button b)
    {
        var n = b.Name.StartsWith("btn") ? b.Name[3..] : b.Name;
        if (n == "SignOut") return ButtonKind.NavDanger;
        if (n.StartsWith("Nav")) return ButtonKind.Nav;
        if (n.StartsWith("Publish")) return ButtonKind.Publish;
        if (Danger.Any(n.StartsWith)) return ButtonKind.Danger;
        if (Main.Any(n.StartsWith)) return ButtonKind.Main;
        return ButtonKind.Plain;
    }

    static readonly string[] Main = ["Save", "Add", "Create", "New", "Submit", "Upload", "Approve", "Compute", "Finalize",
                                     "Prepare", "Record", "SignIn", "Open", "Choose", "Set"];
    static readonly string[] Danger = ["Delete", "Remove", "Reject", "Disable", "MarkInactive"];

    static readonly ConditionalWeakTable<Control, object> done = new();

    /// <summary>Call once in Main, before the first window opens.</summary>
    public static void Enable() => Application.Idle += (_, _) =>
    {
        foreach (Form f in Application.OpenForms)
            if (!done.TryGetValue(f, out _)) Apply(f);
    };

    /// <summary>Styles a control and everything inside it, and anything added to it later.</summary>
    public static void Apply(Control root)
    {
        if (done.TryGetValue(root, out _)) return;
        done.Add(root, true);
        Style(root);
        root.ControlAdded += (_, e) => { if (e.Control != null) Apply(e.Control); };
        foreach (Control c in root.Controls) Apply(c);
    }

    static bool IsDefault(Color c, params Color[] defaults) => defaults.Contains(c) || c == Color.Empty;

    /// <summary>
    /// True when a property was set on this control itself (in the designer or in code), not just inherited from
    /// its parent. Asks WinForms' own ShouldSerialize check, the same one the designer uses to decide what to save.
    /// </summary>
    static bool SetByHand(Control c, string property)
    {
        var m = typeof(Control).GetMethod("ShouldSerialize" + property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, Type.EmptyTypes);
        return m != null && (bool)m.Invoke(c, null)!;
    }

    static void Style(Control c)
    {
        switch (c)
        {
            case Button b: StyleButton(b); break;
            case DataGridView g: StyleGrid(g); break;
            case TextBox t:
                if (t.BorderStyle == BorderStyle.Fixed3D) t.BorderStyle = BorderStyle.FixedSingle;
                if (!SetByHand(t, "ForeColor")) t.ForeColor = Text;
                break;
            case NumericUpDown n:
                if (n.BorderStyle == BorderStyle.Fixed3D) n.BorderStyle = BorderStyle.FixedSingle;
                break;
            case ComboBox cb:
                if (cb.FlatStyle == FlatStyle.Standard) cb.FlatStyle = FlatStyle.Flat;
                break;
            case CheckBox or RadioButton:
                if (!SetByHand(c, "ForeColor")) c.ForeColor = Text;
                break;
            case LinkLabel link:
                link.LinkColor = link.VisitedLinkColor = Orange;
                link.ActiveLinkColor = OrangeDark;
                break;
            case Label l:
                if (!SetByHand(l, "ForeColor")) l.ForeColor = Text;
                if (l.Name == "lblPageTitle" && !SetByHand(l, "Font")) l.Font = new Font(l.Font.FontFamily, 14f, FontStyle.Bold);
                break;
            case Form or UserControl or Panel or TabPage or FlowLayoutPanel or GroupBox:
                if (!SetByHand(c, "BackColor")) c.BackColor = Page;
                if (!SetByHand(c, "ForeColor")) c.ForeColor = Text;
                break;
        }
    }

    static void StyleButton(Button b)
    {
        if (b.FlatStyle == FlatStyle.Flat && KindOf(b) == ButtonKind.Nav) return;   // already set by SetActiveNav
        // A button the owner coloured in the designer keeps its colours.
        bool custom = SetByHand(b, "BackColor") || SetByHand(b, "ForeColor");
        b.FlatStyle = FlatStyle.Flat;
        b.Cursor = Cursors.Hand;
        b.UseVisualStyleBackColor = false;
        if (custom) return;

        var kind = KindOf(b);
        var (back, fore, border, hover) = kind switch
        {
            ButtonKind.Main => (Orange, Color.White, Orange, OrangeDark),
            ButtonKind.Publish => (Green, Color.White, Green, GreenDark),
            ButtonKind.Danger => (Color.White, Red, Border, ColorTranslator.FromHtml("#FEF3F2")),
            ButtonKind.Nav => (Page, Text, Page, OrangeLight),
            ButtonKind.NavDanger => (Page, Red, Page, ColorTranslator.FromHtml("#FEF3F2")),
            _ => (Color.White, Text, Border, Hover),
        };
        b.BackColor = back;
        b.ForeColor = fore;
        b.FlatAppearance.BorderColor = border;
        b.FlatAppearance.BorderSize = kind is ButtonKind.Nav or ButtonKind.NavDanger ? 0 : 1;
        b.FlatAppearance.MouseOverBackColor = hover;
        b.FlatAppearance.MouseDownBackColor = hover;
    }

    /// <summary>The sidebar button of the open page: orange with white text (others plain). Called by the main windows.</summary>
    public static void SetActiveNav(Button b, bool active)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.UseVisualStyleBackColor = false;
        b.Cursor = Cursors.Hand;
        b.BackColor = active ? Orange : Page;
        b.ForeColor = active ? Color.White : Text;
        b.FlatAppearance.MouseOverBackColor = active ? OrangeDark : OrangeLight;
    }

    static void StyleGrid(DataGridView g)
    {
        g.BorderStyle = BorderStyle.None;
        g.BackgroundColor = Page;
        g.GridColor = Line;
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = HeaderHeight;
        var h = g.ColumnHeadersDefaultCellStyle;
        h.BackColor = HeaderBack;
        h.ForeColor = TextMuted;
        h.SelectionBackColor = HeaderBack;
        h.SelectionForeColor = TextMuted;
        h.Font = new Font(g.Font, FontStyle.Bold);
        h.Padding = new Padding(6, 0, 6, 0);
        var d = g.DefaultCellStyle;
        if (IsDefault(d.ForeColor, SystemColors.ControlText)) d.ForeColor = Text;
        d.SelectionBackColor = OrangeLight;
        d.SelectionForeColor = Text;
        d.Padding = new Padding(6, 0, 6, 0);
        g.RowTemplate.Height = RowHeight;
        foreach (DataGridViewRow r in g.Rows) r.Height = RowHeight;   // rows already loaded
    }
}
