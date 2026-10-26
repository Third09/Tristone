using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>An office staff member or site worker. Table: employees (database/003_employees.sql).</summary>
[Table("employees")]
public class Employee
{
    [Column("employee_id")] public int EmployeeId { get; set; }
    [Column("full_name")] public string FullName { get; set; } = "";
    [Column("contact_number")] public string? ContactNumber { get; set; }
    [Column("address")] public string? Address { get; set; }
    [Column("position")] public string? Position { get; set; }

    /// <summary>One of <see cref="EmployeeDepartments"/>.</summary>
    [Column("department")] public string Department { get; set; } = EmployeeDepartments.Site;

    /// <summary>Office (paid 15th and 30th) or Site (paid every Saturday). Follows the department.</summary>
    [Column("pay_schedule")] public string PaySchedule { get; set; } = PaySchedules.Site;

    [Column("pay_grade")] public string? PayGrade { get; set; }
    [Column("biometric_id")] public string? BiometricId { get; set; }
    [Column("gov_deductions")] public bool GovDeductions { get; set; }
    [Column("sss_no")] public string? SssNo { get; set; }
    [Column("philhealth_no")] public string? PhilHealthNo { get; set; }
    [Column("pagibig_no")] public string? PagIbigNo { get; set; }
    [Column("tin")] public string? Tin { get; set; }

    /// <summary>Active or Inactive (left the company; records kept, login turned off).</summary>
    [Column("status")] public string Status { get; set; } = EmployeeStatuses.Active;

    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>A daily rate from a given date. Table: employee_rates.</summary>
[Table("employee_rates")]
public class EmployeeRate
{
    [Column("rate_id")] public int RateId { get; set; }
    [Column("employee_id")] public int EmployeeId { get; set; }
    [Column("daily_rate")] public decimal DailyRate { get; set; }
    [Column("effective_from")] public DateOnly EffectiveFrom { get; set; }
    [Column("created_by")] public int? CreatedBy { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public static class EmployeeDepartments
{
    public const string Administration = "Administration";
    public const string Site = "Site";
    public static readonly string[] All =
        [Administration, Departments.HrPayroll, Departments.Inventory, Departments.Subcontractors, Departments.Receipts, Site];
}

public static class PaySchedules
{
    public const string Office = "Office";
    public const string Site = "Site";

    /// <summary>Site workers are paid weekly; everyone else twice a month.</summary>
    public static string For(string department) => department == EmployeeDepartments.Site ? Site : Office;
}

public static class EmployeeStatuses
{
    public const string Active = "Active";
    public const string Inactive = "Inactive";
}
