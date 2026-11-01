using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Company.OfficeApp.UI;

/// <summary>Rounded-rectangle drawing shared by the themed controls.</summary>
public static class Rounded
{
    public static GraphicsPath Path(Rectangle r, int radius)
    {
        int d = Math.Max(1, Math.Min(radius * 2, Math.Min(r.Width, r.Height)));
        var p = new GraphicsPath();
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    /// <summary>Fills what's behind the rounded corners with the parent's colour, then the rounded box.</summary>
    public static void Box(Graphics g, Control c, Color back, Color border, int radius, int borderWidth = 1)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(c.Parent?.BackColor ?? Theme.Page);
        var r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);
        using var path = Path(r, radius);
        using (var fill = new SolidBrush(back)) g.FillPath(fill, path);
        if (borderWidth > 0)
        {
            using var pen = new Pen(border, borderWidth);
            g.DrawPath(pen, path);
        }
    }
}

/// <summary>
/// A white rounded "card" with a thin border (the Figma sections). Put controls inside it like a Panel.
/// </summary>
public class Card : Panel
{
    public Card()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        BackColor = Color.White;
        Padding = new Padding(20);
    }

    [DefaultValue(12)] public int Radius { get; set; } = 12;

    protected override void OnPaintBackground(PaintEventArgs e) => Rounded.Box(e.Graphics, this, BackColor, Theme.Line, Radius);
}

/// <summary>
/// A rounded frame around one TextBox (set the TextBox's BorderStyle to None and put it inside). Taller and softer
/// than a plain box, like the Figma inputs; the border turns orange while typing.
/// </summary>
public class InputFrame : Panel
{
    bool focused;

    public InputFrame()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        BackColor = Color.White;
        Cursor = Cursors.IBeam;
    }

    TextBox? Box => Controls.OfType<TextBox>().FirstOrDefault();

    protected override void OnControlAdded(ControlEventArgs e)
    {
        base.OnControlAdded(e);
        if (e.Control is TextBox t)
        {
            t.BorderStyle = BorderStyle.None;
            t.GotFocus += (_, _) => { focused = true; Invalidate(); };
            t.LostFocus += (_, _) => { focused = false; Invalidate(); };
        }
    }

    protected override void OnClick(EventArgs e) { base.OnClick(e); Box?.Focus(); }

    protected override void OnLayout(LayoutEventArgs e)
    {
        base.OnLayout(e);
        if (Box is not { } t) return;
        t.BackColor = BackColor;
        if (t.Multiline) t.SetBounds(12, 9, Width - 24, Height - 18);
        else t.SetBounds(12, (Height - t.Height) / 2, Width - 24, t.Height);
    }

    protected override void OnPaintBackground(PaintEventArgs e) =>
        Rounded.Box(e.Graphics, this, Enabled ? BackColor : Theme.HeaderBack, focused ? Theme.Orange : Theme.Border, 8);
}

/// <summary>
/// The Figma picture box: a dashed orange area that says "Drag and Drop your images here / or browse files", or the
/// picture once there is one. Clicking it raises <see cref="BrowseRequested"/>; dropping files raises <see cref="FilesDropped"/>.
/// </summary>
public class DropZone : Control
{
    Image? image;

    public event EventHandler? BrowseRequested;
    public event Action<string[]>? FilesDropped;

    public DropZone()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        BackColor = Color.White;
        AllowDrop = true;
        Cursor = Cursors.Hand;
    }

    [DefaultValue("JPEG, PNG up to 8MB")] public string Hint { get; set; } = "JPEG, PNG up to 8MB";

    /// <summary>The picture to show (the caller owns and disposes it).</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Image? Image { get => image; set { image = value; Invalidate(); } }

    protected override void OnClick(EventArgs e) { base.OnClick(e); if (Enabled) BrowseRequested?.Invoke(this, EventArgs.Empty); }

    protected override void OnDragEnter(DragEventArgs e)
    {
        base.OnDragEnter(e);
        e.Effect = Enabled && e.Data?.GetDataPresent(DataFormats.FileDrop) == true ? DragDropEffects.Copy : DragDropEffects.None;
    }

    protected override void OnDragDrop(DragEventArgs e)
    {
        base.OnDragDrop(e);
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0) FilesDropped?.Invoke(files);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.Page);
        var r = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = Rounded.Path(r, 8);
        using (var fill = new SolidBrush(BackColor)) g.FillPath(fill, path);

        if (image != null)
        {
            var scale = Math.Min((float)(Width - 2) / image.Width, (float)(Height - 2) / image.Height);
            var w = (int)(image.Width * scale); var h = (int)(image.Height * scale);
            g.SetClip(path);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, (Width - w) / 2, (Height - h) / 2, w, h);
            g.ResetClip();
            using var thin = new Pen(Theme.Line);
            g.DrawPath(thin, path);
            return;
        }

        using (var dash = new Pen(Theme.Orange, 1.5f) { DashStyle = DashStyle.Dash }) g.DrawPath(dash, path);
        int mid = Height / 2;
        using var icon = new Font("Segoe MDL2 Assets", 22f);
        using var bold = new Font(Font.FontFamily, 10.5f, FontStyle.Bold);
        TextRenderer.DrawText(g, "", icon, new Rectangle(0, mid - 62, Width, 40), Theme.TextMuted, TextFormatFlags.HorizontalCenter);
        TextRenderer.DrawText(g, "Drag and Drop your images here", bold, new Rectangle(0, mid - 12, Width, 24), Theme.Text, TextFormatFlags.HorizontalCenter);
        TextRenderer.DrawText(g, "or browse files   " + Hint, Font, new Rectangle(0, mid + 18, Width, 22), Theme.TextMuted, TextFormatFlags.HorizontalCenter);
    }
}

/// <summary>
/// The Figma message boxes: Unsaved Changes (orange), Changes Saved Successfully (green), Unable to Save Changes (red).
/// Hidden until <see cref="Show(StatusKind, string, string)"/> is called.
/// </summary>
public class StatusBox : Control
{
    public enum StatusKind { None, Info, Success, Error }

    StatusKind kind;
    string title = "", message = "";

    public StatusBox()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        Visible = false;
    }

    public void Show(StatusKind k, string t, string m)
    {
        kind = k; title = t; message = m;
        Visible = k != StatusKind.None;
        Invalidate();
    }

    public void Clear() => Show(StatusKind.None, "", "");

    protected override void OnPaint(PaintEventArgs e)
    {
        var (back, border, fore, glyph) = kind switch
        {
            StatusKind.Success => (ColorTranslator.FromHtml("#ECFDF3"), ColorTranslator.FromHtml("#75E0A7"), ColorTranslator.FromHtml("#067647"), ""),
            StatusKind.Error => (ColorTranslator.FromHtml("#FEF3F2"), ColorTranslator.FromHtml("#FDA29B"), ColorTranslator.FromHtml("#B42318"), ""),
            _ => (ColorTranslator.FromHtml("#FFF6ED"), ColorTranslator.FromHtml("#FEB273"), ColorTranslator.FromHtml("#B93815"), ""),
        };
        Rounded.Box(e.Graphics, this, back, border, 8);
        using var icon = new Font("Segoe MDL2 Assets", 10f);
        using var bold = new Font(Font.FontFamily, 9.75f, FontStyle.Bold);
        TextRenderer.DrawText(e.Graphics, glyph, icon, new Point(14, 15), fore);
        TextRenderer.DrawText(e.Graphics, title, bold, new Point(36, 12), fore);
        TextRenderer.DrawText(e.Graphics, message, Font, new Rectangle(36, 34, Width - 50, Height - 40), fore,
                              TextFormatFlags.WordBreak | TextFormatFlags.Left | TextFormatFlags.Top);
    }
}
