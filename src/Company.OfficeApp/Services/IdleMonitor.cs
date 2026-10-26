namespace Company.OfficeApp.Services;

/// <summary>
/// Notices keyboard and mouse use anywhere in the app, so the main window can ask "Still there?"
/// after 5 minutes without any (Project Outline, Part 0).
/// </summary>
public sealed class IdleMonitor : IMessageFilter
{
    public static readonly IdleMonitor Instance = new();

    /// <summary>No activity for this long opens the "Still there?" popup.</summary>
    public static TimeSpan Timeout { get; } = ReadTimeout();

    /// <summary>Seconds the popup waits before signing out by itself.</summary>
    public const int WarningSeconds = 60;

    DateTime lastActivity = DateTime.Now;
    bool started;

    public TimeSpan IdleFor => DateTime.Now - lastActivity;

    public void Start()
    {
        Reset();
        if (!started) { Application.AddMessageFilter(this); started = true; }
    }

    public void Stop()
    {
        if (started) { Application.RemoveMessageFilter(this); started = false; }
    }

    public void Reset() => lastActivity = DateTime.Now;

    // Windows messages that mean someone is using the app.
    const int WM_KEYDOWN = 0x0100, WM_SYSKEYDOWN = 0x0104;
    const int WM_MOUSEMOVE = 0x0200, WM_LBUTTONDOWN = 0x0201, WM_RBUTTONDOWN = 0x0204, WM_MOUSEWHEEL = 0x020A;

    public bool PreFilterMessage(ref Message m)
    {
        if (m.Msg is WM_KEYDOWN or WM_SYSKEYDOWN or WM_MOUSEMOVE or WM_LBUTTONDOWN or WM_RBUTTONDOWN or WM_MOUSEWHEEL)
            lastActivity = DateTime.Now;
        return false;   // never block the message, just watch it
    }

    static TimeSpan ReadTimeout()
    {
#if DEBUG
        // For testing only: set TRISTONE_IDLE_SECONDS=20 to see the popup without waiting 5 minutes.
        if (int.TryParse(Environment.GetEnvironmentVariable("TRISTONE_IDLE_SECONDS"), out var s) && s > 0)
            return TimeSpan.FromSeconds(s);
#endif
        return TimeSpan.FromMinutes(5);
    }
}
