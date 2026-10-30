using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Hr;

/// <summary>
/// HR fixes one day's times by hand (a forgotten scan, or a scanner problem), with a reason. Saving asks Yes/No
/// with the list of changes; the fix is kept in Edit History.
/// Plain layout for now; control names match the UI Elements Guide (Part 2: Fix Scan Window).
/// </summary>
public partial class FixScanForm : Form
{
    /// <summary>The four times and the reason as shown when the window opened (for the "Save these changes?" list).</summary>
    record Times(string TimeIn, string BreakOut, string BreakIn, string TimeOut, string Reason);

    readonly AttendanceRow row;
    Times? before;

    public FixScanForm(AttendanceRow row)
    {
        InitializeComponent();
        this.row = row;
        Text = "Fix Scan";
        AcceptButton = btnSaveFix;
        CancelButton = btnCancelFix;
    }

    async void FixScanForm_Load(object sender, EventArgs e)
    {
        lblFixEmployeeDate.Text = $"{row.Name}, {row.Date:dddd, MMMM d, yyyy} ({row.Status})";
        var day = await AttendanceService.GetAsync(row.DayId);
        Set(dtpFixTimeIn, day?.TimeIn, AttendanceRules.Start);
        Set(dtpFixBreakOut, day?.BreakOut, AttendanceRules.LunchOut);
        Set(dtpFixBreakIn, day?.BreakIn, AttendanceRules.LunchIn);
        Set(dtpFixTimeOut, day?.TimeOut, AttendanceRules.End);
        txtFixReason.Text = day?.FixReason ?? "";
        before = Read();
    }

    // A known time is ticked; an empty one shows the usual time, unticked, ready to tick.
    static void Set(DateTimePicker p, TimeOnly? value, TimeOnly usual)
    {
        p.Value = DateTime.Today.Add((value ?? usual).ToTimeSpan());
        p.Checked = value != null;
    }

    static TimeOnly? Get(DateTimePicker p) => p.Checked ? new TimeOnly(p.Value.Hour, p.Value.Minute) : null;

    static string Show(TimeOnly? t) => t?.ToString("h:mm tt") ?? "(none)";

    Times Read() => new(Show(Get(dtpFixTimeIn)), Show(Get(dtpFixBreakOut)), Show(Get(dtpFixBreakIn)), Show(Get(dtpFixTimeOut)), txtFixReason.Text.Trim());

    async void btnSaveFix_Click(object sender, EventArgs e)
    {
        TimeOnly? i = Get(dtpFixTimeIn), bo = Get(dtpFixBreakOut), bi = Get(dtpFixBreakIn), o = Get(dtpFixTimeOut);
        if (AttendanceService.CheckFix(i, bo, bi, o, txtFixReason.Text) is { } problem) { lblFixMsg.Text = problem; return; }
        var answer = RecordForm.ConfirmSave(this, $"{row.Name}'s {row.Date:MMM d}", before!, Read());
        if (answer == SaveAnswer.NothingChanged) { lblFixMsg.Text = "Nothing changed."; return; }
        if (answer == SaveAnswer.No) return;
        btnSaveFix.Enabled = false;
        try
        {
            var error = await AttendanceService.FixAsync(row.DayId, i, bo, bi, o, txtFixReason.Text);
            if (error != null) { lblFixMsg.Text = error; return; }
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { lblFixMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveFix.Enabled = true; }
    }
}
