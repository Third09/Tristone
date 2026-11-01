using System.Reflection;
using System.Runtime.CompilerServices;

namespace Company.OfficeApp.UI;

/// <summary>
/// The app's look in one place, like a CSS file (owner request 2026-11-01; colors from the Figma "CCMS SUPERSYSTEM").
/// <see cref="Enable"/> (called once at start-up) styles every window as it opens and every control added later
/// (pages loaded into the main window, pop-ups). Change a color here and every screen follows.
///
/// Rules:
/// - Only the look changes (colors, borders, rounded flat buttons, table style). Sizes and positions stay as
///   designed, and the font stays the same size, so no layout moves. Screens redone like the Figma also use the
///   themed controls in ThemeControls.cs (Card, InputFrame, DropZone, StatusBox) and set their own sizes.
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
    public static readonly Color Canvas = ColorTranslator.FromHtml("#F7F8FA");       // behind the white cards on redone pages

    public const int ButtonRadius = 8;

    static Image? logo;

    /// <summary>The Tri-Stone logo for white backgrounds (Resources\logo-dark.png, made from the owner's white logo).</summary>
    public static Image Logo => logo ??= LoadImage("logo-dark.png");

    static Image LoadImage(string file)
    {
        var asm = typeof(Theme).Assembly;
        var name = asm.GetManifestResourceNames().Single(n => n.EndsWith("." + file));
        using var stream = asm.GetManifestResourceStream(name)!;
        return Image.FromStream(stream);
    }

    /// <summary>The main windows' sidebar and top bar: logo, icons, muted signed-in line.</summary>
    public static void StyleMainWindow(PictureBox logoBox, IEnumerable<(Button Button, string Glyph)> nav, params Label[] muted)
    {
        logoBox.Image = Logo;
        foreach (var (b, glyph) in nav) SetIcon(b, glyph);
        foreach (var l in muted) l.ForeColor = TextMuted;
    }

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
        Round(b);
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
        Round(b);
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.UseVisualStyleBackColor = false;
        b.Cursor = Cursors.Hand;
        b.BackColor = active ? Orange : Page;
        b.ForeColor = active ? Color.White : Text;
        b.FlatAppearance.MouseOverBackColor = active ? OrangeDark : OrangeLight;
    }

    static readonly ConditionalWeakTable<Button, object> rounded = new();
    static readonly ConditionalWeakTable<Button, string> icons = new();

    /// <summary>Puts a small icon (a Segoe MDL2 Assets glyph, e.g. "" = house) before the button's text.</summary>
    public static void SetIcon(Button b, string glyph)
    {
        icons.AddOrUpdate(b, glyph);
        b.Invalidate();
    }

    /// <summary>Draws the button ourselves: rounded corners, hover colour, optional icon (WinForms can't round on its own).</summary>
    static void Round(Button b)
    {
        if (rounded.TryGetValue(b, out _)) return;
        rounded.Add(b, true);
        b.Paint += PaintButton;
        b.MouseEnter += (_, _) => b.Invalidate();
        b.MouseLeave += (_, _) => b.Invalidate();
        b.EnabledChanged += (_, _) => b.Invalidate();
    }

    static Color Blend(Color a, Color b, float t) =>
        Color.FromArgb((int)(a.R + (b.R - a.R) * t), (int)(a.G + (b.G - a.G) * t), (int)(a.B + (b.B - a.B) * t));

    static void PaintButton(object? sender, PaintEventArgs e)
    {
        var b = (Button)sender!;
        bool hover = b.Enabled && b.ClientRectangle.Contains(b.PointToClient(Cursor.Position));
        var back = hover && !b.FlatAppearance.MouseOverBackColor.IsEmpty ? b.FlatAppearance.MouseOverBackColor : b.BackColor;
        var fore = b.ForeColor;
        var border = b.FlatAppearance.BorderColor.IsEmpty ? back : b.FlatAppearance.BorderColor;
        if (!b.Enabled) { back = Blend(back, Color.White, 0.55f); fore = Blend(fore, Color.White, 0.5f); border = Blend(border, Color.White, 0.5f); }
        Rounded.Box(e.Graphics, b, back, border, ButtonRadius, b.FlatAppearance.BorderSize);

        bool left = b.TextAlign is ContentAlignment.MiddleLeft or ContentAlignment.TopLeft or ContentAlignment.BottomLeft;
        int x = left ? 14 : 0;
        if (icons.TryGetValue(b, out var glyph))
        {
            using var f = new Font("Segoe MDL2 Assets", b.Font.Size + 1.5f);
            TextRenderer.DrawText(e.Graphics, glyph, f, new Rectangle(14, 0, 26, b.Height), fore,
                                  TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPadding);
            x = 46;
            left = true;
        }
        var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis |
                    (left ? TextFormatFlags.Left : TextFormatFlags.HorizontalCenter);
        TextRenderer.DrawText(e.Graphics, b.Text, b.Font, new Rectangle(x, 0, b.Width - x - (left ? 8 : 0), b.Height), fore, flags);
    }

    public static readonly Color GridLine = ColorTranslator.FromHtml("#E4E7EC");      // table cell lines
    public static readonly Color GridFrame = ColorTranslator.FromHtml("#D0D5DD");     // the border around a table
    public static readonly Color GridHeader = ColorTranslator.FromHtml("#F2F4F7");    // table header row
    public static readonly Color GridStripe = ColorTranslator.FromHtml("#FAFAFB");    // every other row
    public static readonly Color GridEmpty = ColorTranslator.FromHtml("#F7F8FA");     // below the last row

    /// <summary>
    /// Tables (owner request 2026-11-01: "have grid, show where it starts and ends"): lines between every cell, a
    /// framed header, every other row shaded, a gray area under the last row, a border around the whole table
    /// (drawn on its parent, just outside it) and "No records to show" when it's empty.
    /// </summary>
    static void StyleGrid(DataGridView g)
    {
        g.BorderStyle = BorderStyle.None;
        g.BackgroundColor = GridEmpty;
        g.GridColor = GridLine;
        g.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = HeaderHeight;
        var h = g.ColumnHeadersDefaultCellStyle;
        h.BackColor = GridHeader;
        h.ForeColor = Text;
        h.SelectionBackColor = GridHeader;
        h.SelectionForeColor = Text;
        h.Font = new Font(g.Font, FontStyle.Bold);
        h.Padding = new Padding(6, 0, 6, 0);
        var d = g.DefaultCellStyle;
        if (IsDefault(d.ForeColor, SystemColors.ControlText)) d.ForeColor = Text;
        if (IsDefault(d.BackColor, SystemColors.Window)) d.BackColor = Color.White;
        d.SelectionBackColor = OrangeLight;
        d.SelectionForeColor = Text;
        d.Padding = new Padding(6, 0, 6, 0);
        if (IsDefault(g.AlternatingRowsDefaultCellStyle.BackColor)) g.AlternatingRowsDefaultCellStyle.BackColor = GridStripe;
        g.RowTemplate.Height = RowHeight;
        foreach (DataGridViewRow r in g.Rows) r.Height = RowHeight;   // rows already loaded

        g.Paint += PaintEmptyNote;
        g.RowsAdded += (_, _) => g.Invalidate();
        g.RowsRemoved += (_, _) => g.Invalidate();
        g.DataBindingComplete += (_, _) => g.Invalidate();
        FrameOnParent(g);
    }

    static void PaintEmptyNote(object? sender, PaintEventArgs e)
    {
        var g = (DataGridView)sender!;
        if (g.Rows.Count > 0 || g.Columns.Count == 0) return;
        var top = g.ColumnHeadersVisible ? g.ColumnHeadersHeight : 0;
        TextRenderer.DrawText(e.Graphics, "No records to show", g.Font, new Rectangle(0, top + 16, g.Width, 24), TextMuted,
                              TextFormatFlags.HorizontalCenter);
    }

    /// <summary>Draws a border just outside the table on its parent, and keeps it there when the table moves or grows.</summary>
    static void FrameOnParent(DataGridView g)
    {
        Control? parent = null;
        void Draw(object? s, PaintEventArgs e)
        {
            if (!g.Visible) return;
            var r = g.Bounds;
            r.Inflate(1, 1);
            r.Width -= 1; r.Height -= 1;
            using var pen = new Pen(GridFrame);
            e.Graphics.DrawRectangle(pen, r);
        }
        void Hook()
        {
            if (parent != null) parent.Paint -= Draw;
            parent = g.Parent;
            if (parent != null) { parent.Paint += Draw; parent.Invalidate(); }
        }
        g.ParentChanged += (_, _) => Hook();
        g.LocationChanged += (_, _) => parent?.Invalidate();
        g.SizeChanged += (_, _) => parent?.Invalidate();
        g.VisibleChanged += (_, _) => parent?.Invalidate();
        Hook();
    }

    /// <summary>A choice button that is "on" (e.g. the open About Us section): light orange with an orange border.</summary>
    public static void SetSelected(Button b, bool selected)
    {
        Round(b);
        b.FlatStyle = FlatStyle.Flat;
        b.UseVisualStyleBackColor = false;
        b.BackColor = selected ? OrangeLight : Color.White;
        b.ForeColor = Text;
        b.FlatAppearance.BorderColor = selected ? Orange : Border;
        b.FlatAppearance.BorderSize = 1;
        b.FlatAppearance.MouseOverBackColor = selected ? OrangeLight : Hover;
        b.Invalidate();
    }
}
