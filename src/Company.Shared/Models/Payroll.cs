using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

// Part 2: HR Settings, cash advances, payroll. Tables: database/013_payroll.sql.

[Table("holidays")]
public class Holiday
{
    [Column("holiday_date")] public DateOnly HolidayDate { get; set; }
    [Column("name")] public string Name { get; set; } = "";

    /// <summary>One of <see cref="HolidayKinds"/>.</summary>
    [Column("kind")] public string Kind { get; set; } = HolidayKinds.Regular;
}

[Table("cash_advances")]
public class CashAdvance
{
    [Column("advance_id")] public int AdvanceId { get; set; }
    [Column("employee_id")] public int EmployeeId { get; set; }
    [Column("given_on")] public DateOnly GivenOn { get; set; }
    [Column("amount")] public decimal Amount { get; set; }
    [Column("per_payday")] public decimal PerPayday { get; set; }
    [Column("balance")] public decimal Balance { get; set; }
    [Column("reason")] public string? Reason { get; set; }

    /// <summary>One of <see cref="CashAdvanceStatuses"/>.</summary>
    [Column("status")] public string Status { get; set; } = CashAdvanceStatuses.Waiting;
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("decided_by")] public int? DecidedBy { get; set; }
    [Column("decided_at")] public DateTime? DecidedAt { get; set; }
}

[Table("payroll_runs")]
public class PayrollRun
{
    [Column("run_id")] public int RunId { get; set; }
    [Column("pay_group")] public string PayGroup { get; set; } = PaySchedules.Office;
    [Column("period_from")] public DateOnly PeriodFrom { get; set; }
    [Column("period_to")] public DateOnly PeriodTo { get; set; }
    [Column("pay_date")] public DateOnly PayDate { get; set; }

    /// <summary>Draft (can be computed again) or Final (locked).</summary>
    [Column("status")] public string Status { get; set; } = PayrollStatuses.Draft;
    [Column("computed_by")] public int? ComputedBy { get; set; }
    [Column("computed_at")] public DateTime ComputedAt { get; set; } = DateTime.Now;
    [Column("finalized_by")] public int? FinalizedBy { get; set; }
    [Column("finalized_at")] public DateTime? FinalizedAt { get; set; }
}

[Table("payslips")]
public class Payslip
{
    [Column("payslip_id")] public int PayslipId { get; set; }
    [Column("run_id")] public int RunId { get; set; }
    [Column("employee_id")] public int EmployeeId { get; set; }
    [Column("daily_rate")] public decimal DailyRate { get; set; }
    [Column("days_paid")] public decimal DaysPaid { get; set; }
    [Column("basic_pay")] public decimal BasicPay { get; set; }
    [Column("holiday_days")] public int HolidayDays { get; set; }
    [Column("holiday_pay")] public decimal HolidayPay { get; set; }
    [Column("late_minutes")] public int LateMinutes { get; set; }
    [Column("late_deduction")] public decimal LateDeduction { get; set; }
    [Column("ot_minutes")] public int OtMinutes { get; set; }
    [Column("ot_pay")] public decimal OtPay { get; set; }
    [Column("gross_pay")] public decimal GrossPay { get; set; }
    [Column("sss")] public decimal Sss { get; set; }
    [Column("philhealth")] public decimal PhilHealth { get; set; }
    [Column("pagibig")] public decimal PagIbig { get; set; }
    [Column("tax")] public decimal Tax { get; set; }
    [Column("cash_advance")] public decimal CashAdvance { get; set; }
    [Column("net_pay")] public decimal NetPay { get; set; }

    [NotMapped] public decimal GovDeductions => Sss + PhilHealth + PagIbig + Tax;
}

[Table("cash_advance_deductions")]
public class CashAdvanceDeduction
{
    [Column("deduction_id")] public int DeductionId { get; set; }
    [Column("advance_id")] public int AdvanceId { get; set; }
    [Column("payslip_id")] public int PayslipId { get; set; }
    [Column("amount")] public decimal Amount { get; set; }
    [Column("balance_after")] public decimal BalanceAfter { get; set; }
}

public static class HolidayKinds
{
    public const string Regular = "Regular";
    public const string Special = "Special";
    public static readonly string[] All = [Regular, Special];
}

public static class CashAdvanceStatuses
{
    public const string Waiting = "Waiting";
    public const string Active = "Active";
    public const string PaidOff = "Paid Off";
    public const string Rejected = "Rejected";
}

public static class PayrollStatuses
{
    public const string Draft = "Draft";
    public const string Final = "Final";
}
