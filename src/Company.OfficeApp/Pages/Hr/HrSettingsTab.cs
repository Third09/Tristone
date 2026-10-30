using Company.OfficeApp.Services;
using Company.OfficeApp.UI;
using Company.Shared.Models;

namespace Company.OfficeApp.Pages.Hr;

/// <summary>
/// HR & Payroll > Settings (Admin and HR Supervisor): work hours, grace minutes, work days, overtime rate,
/// government deduction rates, and the holiday list. Saving asks Yes/No with the list of changes, then the
/// attendance days are worked out again (days already paid by a finalized payroll never change).
/// Plain layout for now; control names match the UI Elements Guide (Part 2: Tab: Settings).
/// </summary>
public partial class HrSettingsTab : UserControl
{
    /// <summary>What the form shows, as text, for the "Save these changes?" list.</summary>
    record Shown(string WorkStart, string WorkEnd, string BreakStart, string BreakEnd, int GraceMinutes, string WorkDays, decimal OvertimePerHour,
                 int OvertimeFromMinutes, int WorkDaysPerMonth, decimal SssPercent, decimal SssMinimum, decimal SssMaximum, decimal PhilHealthPercent,
                 decimal PhilHealthFloor, decimal PhilHealthCeiling, decimal PagIbigPercent, decimal PagIbigCap, bool WithholdingTax);

    static readonly DayOfWeek[] Week = [DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday];
    Shown? loaded;
    bool filling;

    public HrSettingsTab()
    {
        InitializeComponent();
        dgvHolidays.AutoGenerateColumns = false;
        clbWorkDays.Items.AddRange([.. Week.Select(d => d.ToString())]);
        cboHolidayKind.Items.AddRange(HolidayKinds.All);
        cboHolidayKind.SelectedIndex = 0;
        dtpHolidayDate.Value = DateTime.Today;
        RecordForm.NoAutoSelect(dgvHolidays, on => filling = on, () => { filling = true; dgvHolidays.ClearSelection(); filling = false; UpdateButtons(); });
        UpdateButtons();
    }

    async void HrSettingsTab_Load(object sender, EventArgs e)
    {
        Show(await HrSettingsService.LoadAsync());
        await LoadHolidaysAsync();
    }

    static DateTime At(TimeOnly t) => DateTime.Today.Add(t.ToTimeSpan());
    static TimeOnly Time(DateTimePicker p) => new(p.Value.Hour, p.Value.Minute);

    void Show(HrSettings s)
    {
        dtpWorkStart.Value = At(s.WorkStart); dtpWorkEnd.Value = At(s.WorkEnd);
        dtpBreakStart.Value = At(s.BreakStart); dtpBreakEnd.Value = At(s.BreakEnd);
        numGraceMinutes.Value = s.GraceMinutes;
        for (int i = 0; i < Week.Length; i++) clbWorkDays.SetItemChecked(i, s.WorkDays.Contains(Week[i]));
        numOvertimeRate.Value = s.OvertimeRate; numOtMinimum.Value = s.OvertimeMinimumMinutes;
        numWorkDaysPerMonth.Value = s.WorkDaysPerMonth;
        numSssRate.Value = s.SssRate; numSssMin.Value = s.SssMinCredit; numSssMax.Value = s.SssMaxCredit;
        numPhilHealthRate.Value = s.PhilHealthRate; numPhilHealthFloor.Value = s.PhilHealthFloor; numPhilHealthCeiling.Value = s.PhilHealthCeiling;
        numPagIbigRate.Value = s.PagIbigRate; numPagIbigCap.Value = s.PagIbigCap;
        chkWithholdingTax.Checked = s.WithholdingTax;
        loaded = Snapshot();
    }

    HrSettings Read() => new(Time(dtpWorkStart), Time(dtpWorkEnd), Time(dtpBreakStart), Time(dtpBreakEnd), (int)numGraceMinutes.Value,
        [.. Week.Where((_, i) => clbWorkDays.GetItemChecked(i))], numOvertimeRate.Value, (int)numOtMinimum.Value, (int)numWorkDaysPerMonth.Value,
        numSssRate.Value, numSssMin.Value, numSssMax.Value, numPhilHealthRate.Value, numPhilHealthFloor.Value, numPhilHealthCeiling.Value,
        numPagIbigRate.Value, numPagIbigCap.Value, chkWithholdingTax.Checked);

    Shown Snapshot()
    {
        var s = Read();
        string T(TimeOnly t) => t.ToString("h:mm tt");
        return new(T(s.WorkStart), T(s.WorkEnd), T(s.BreakStart), T(s.BreakEnd), s.GraceMinutes, string.Join(", ", s.WorkDays.Select(d => d.ToString()[..3])),
            s.OvertimeRate, s.OvertimeMinimumMinutes, s.WorkDaysPerMonth, s.SssRate, s.SssMinCredit, s.SssMaxCredit, s.PhilHealthRate,
            s.PhilHealthFloor, s.PhilHealthCeiling, s.PagIbigRate, s.PagIbigCap, s.WithholdingTax);
    }

    async void btnSaveHrSettings_Click(object sender, EventArgs e)
    {
        var s = Read();
        if (s.Check() is { } problem) { lblSettingsMsg.Text = problem; return; }
        var answer = RecordForm.ConfirmSave(this, "HR Settings", loaded!, Snapshot());
        if (answer == SaveAnswer.NothingChanged) { lblSettingsMsg.Text = "Nothing changed."; return; }
        if (answer == SaveAnswer.No) return;
        btnSaveHrSettings.Enabled = false;
        try
        {
            using var busy = Responsive.Busy(this);
            var error = await HrSettingsService.SaveAsync(s);
            lblSettingsMsg.Text = error ?? "Saved. Attendance was worked out again with the new settings (paid days don't change).";
            if (error == null) loaded = Snapshot();
        }
        catch (Exception ex) { lblSettingsMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveHrSettings.Enabled = true; }
    }

    // ----- holidays -----

    async Task LoadHolidaysAsync()
    {
        var list = await HrSettingsService.HolidaysAsync();
        filling = true;
        dgvHolidays.DataSource = list;
        dgvHolidays.ClearSelection();
        filling = false;
        UpdateButtons();
    }

    Holiday? SelectedHoliday => dgvHolidays.SelectedRows.Count > 0 ? dgvHolidays.SelectedRows[0].DataBoundItem as Holiday : null;

    void UpdateButtons() => btnDeleteHoliday.Enabled = SelectedHoliday != null;

    void dgvHolidays_SelectionChanged(object sender, EventArgs e) { if (!filling) UpdateButtons(); }

    async void btnAddHoliday_Click(object sender, EventArgs e)
    {
        var date = DateOnly.FromDateTime(dtpHolidayDate.Value);
        using (Responsive.Busy(this))
            lblHolidayMsg.Text = await HrSettingsService.AddHolidayAsync(date, txtHolidayName.Text, (string)cboHolidayKind.SelectedItem!) ?? $"Added {date:MMM d, yyyy}.";
        if (lblHolidayMsg.Text.StartsWith("Added")) { txtHolidayName.Clear(); await LoadHolidaysAsync(); }
    }

    async void btnDeleteHoliday_Click(object sender, EventArgs e)
    {
        if (SelectedHoliday is not { } h) return;
        if (MessageBox.Show(this, $"Delete the holiday {h.Name} ({h.HolidayDate:MMM d, yyyy})?", "Delete holiday", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        using (Responsive.Busy(this))
            lblHolidayMsg.Text = await HrSettingsService.DeleteHolidayAsync(h.HolidayDate) ?? "Deleted.";
        await LoadHolidaysAsync();
    }
}
