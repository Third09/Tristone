using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

// Part 2: Attendance. Tables: database/012_attendance.sql.

[Table("attendance_imports")]
public class AttendanceImport
{
    [Column("import_id")] public int ImportId { get; set; }
    [Column("file_name")] public string FileName { get; set; } = "";
    [Column("period_from")] public DateOnly PeriodFrom { get; set; }
    [Column("period_to")] public DateOnly PeriodTo { get; set; }
    [Column("scans_added")] public int ScansAdded { get; set; }
    [Column("scans_skipped")] public int ScansSkipped { get; set; }
    [Column("rows_rejected")] public int RowsRejected { get; set; }
    [Column("problems")] public string? Problems { get; set; }
    [Column("imported_by")] public int? ImportedBy { get; set; }
    [Column("imported_at")] public DateTime ImportedAt { get; set; } = DateTime.Now;
}

[Table("attendance_scans")]
public class AttendanceScan
{
    [Column("scan_id")] public int ScanId { get; set; }
    [Column("employee_id")] public int EmployeeId { get; set; }
    [Column("scan_time")] public DateTime ScanTime { get; set; }
    [Column("import_id")] public int? ImportId { get; set; }
}

[Table("attendance_days")]
public class AttendanceDay
{
    [Column("day_id")] public int DayId { get; set; }
    [Column("employee_id")] public int EmployeeId { get; set; }
    [Column("work_date")] public DateOnly WorkDate { get; set; }
    [Column("time_in")] public TimeOnly? TimeIn { get; set; }
    [Column("break_out")] public TimeOnly? BreakOut { get; set; }
    [Column("break_in")] public TimeOnly? BreakIn { get; set; }
    [Column("time_out")] public TimeOnly? TimeOut { get; set; }
    [Column("late_minutes")] public int LateMinutes { get; set; }
    [Column("early_minutes")] public int EarlyMinutes { get; set; }
    [Column("ot_minutes")] public int OtMinutes { get; set; }

    /// <summary>One of <see cref="OtStatuses"/>.</summary>
    [Column("ot_status")] public string OtStatus { get; set; } = OtStatuses.None;

    /// <summary>One of <see cref="AttendanceStatuses"/>.</summary>
    [Column("status")] public string Status { get; set; } = AttendanceStatuses.Absent;

    [Column("fix_reason")] public string? FixReason { get; set; }
    [Column("fixed_by")] public int? FixedBy { get; set; }
    [Column("fixed_at")] public DateTime? FixedAt { get; set; }
    [Column("ot_decided_by")] public int? OtDecidedBy { get; set; }
    [Column("ot_decided_at")] public DateTime? OtDecidedAt { get; set; }
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public static class AttendanceStatuses
{
    public const string Present = "Present";
    public const string Late = "Late";
    public const string MissingScan = "Missing Scan";
    public const string Absent = "Absent";
    public const string RestDayWork = "Rest Day Work";
}

public static class OtStatuses
{
    public const string None = "None";
    public const string Waiting = "Waiting";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
}
