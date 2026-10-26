using Company.OfficeApp.Services;

namespace Company.OfficeApp.Forms;

/// <summary>
/// "Still there?" popup after 5 minutes without activity. Counts down 60 seconds, then signs out by itself.
/// DialogResult.OK = stay signed in; anything else = sign out.
/// Plain layout for now; control names match the UI Elements Guide (Part A: "Still There?" Popup).
/// </summary>
public partial class IdleWarningForm : Form
{
    int secondsLeft = IdleMonitor.WarningSeconds;

    public IdleWarningForm()
    {
        InitializeComponent();
        ShowCountdown();
        tmrCountdown.Start();
    }

    void ShowCountdown() => lblCountdown.Text = $"Signing out in {secondsLeft} seconds.";

    void tmrCountdown_Tick(object sender, EventArgs e)
    {
        secondsLeft--;
        ShowCountdown();
        if (secondsLeft <= 0)
        {
            tmrCountdown.Stop();
            DialogResult = DialogResult.Abort;   // nobody answered
        }
    }

    void btnStaySignedIn_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

    void btnIdleSignOut_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;
}
