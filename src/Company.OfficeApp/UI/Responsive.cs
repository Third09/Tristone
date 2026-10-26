namespace Company.OfficeApp.UI;

/// <summary>
/// Shared helpers that keep pages usable on any screen size and smooth while the database works.
/// Layout only: no colors or fonts (styling comes later from the Figma design).
/// </summary>
public static class Responsive
{
    /// <summary>
    /// Makes a list page use the space it gets: the table grows wider and taller when the window is bigger than
    /// the page's design size, and everything under the table moves down with it. When the window is smaller
    /// than the design size nothing shrinks; the page scrolls instead (set AutoScroll on the page).
    /// Call once in the page's constructor, after InitializeComponent.
    /// </summary>
    public static void FitGrid(ScrollableControl page, DataGridView grid)
    {
        var designPage = page.Size;
        var designGrid = grid.Size;
        var gridBottom = grid.Bottom;
        var below = (grid.Parent ?? page).Controls.Cast<Control>()
            .Where(c => c != grid && c.Top >= gridBottom)
            .Select(c => (Control: c, Top: c.Top))
            .ToList();

        void Apply()
        {
            int extraW = Math.Max(0, page.ClientSize.Width - designPage.Width);
            int extraH = Math.Max(0, page.ClientSize.Height - designPage.Height);
            grid.Size = new Size(designGrid.Width + extraW, designGrid.Height + extraH);
            int scrollY = page.AutoScrollPosition.Y;   // positions are relative to the scrolled view
            foreach (var (c, top) in below) c.Top = top + extraH + scrollY;
        }

        page.Resize += (_, _) => Apply();
        Apply();
    }

    /// <summary>
    /// Runs <paramref name="action"/> once the person stops typing for a moment, instead of on every key.
    /// Keeps search boxes smooth: "santos" searches once, not six times.
    /// </summary>
    public static void OnTypingPause(TextBox box, Func<Task> action, int milliseconds = 300)
    {
        var timer = new System.Windows.Forms.Timer { Interval = milliseconds };
        timer.Tick += async (_, _) => { timer.Stop(); await action(); };
        box.TextChanged += (_, _) => { timer.Stop(); timer.Start(); };
        box.Disposed += (_, _) => timer.Dispose();
    }

    /// <summary>
    /// Shows the wait cursor on <paramref name="control"/> while a load runs. Use with <c>using</c>.
    /// </summary>
    public static IDisposable Busy(Control control)
    {
        control.UseWaitCursor = true;
        return new Restore(() => { if (!control.IsDisposed) control.UseWaitCursor = false; });
    }

    sealed class Restore(Action undo) : IDisposable
    {
        public void Dispose() => undo();
    }
}

/// <summary>
/// Makes sure only the newest load shows its results. When filters change quickly, an older, slower query
/// could otherwise finish last and overwrite the newer list.
/// </summary>
public sealed class LatestOnly
{
    int version;

    /// <summary>Call at the start of a load; keep the number.</summary>
    public int Start() => ++version;

    /// <summary>True when no newer load has started since <paramref name="ticket"/>.</summary>
    public bool IsLatest(int ticket) => ticket == version;
}
