using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Shared.Models;

/// <summary>
/// One change to a record: who, what, when, before and after. Table: audit_logs.
/// Shown in each department's Edit History tab.
/// </summary>
[Table("audit_logs")]
public class AuditLog
{
    [Column("audit_id")] public int AuditId { get; set; }
    [Column("user_id")] public int? UserId { get; set; }

    /// <summary>Which department's Edit History shows it; null for account/system changes (Admin only).</summary>
    [Column("department")] public string? Department { get; set; }

    [Column("table_name")] public string TableName { get; set; } = "";
    [Column("record_id")] public int? RecordId { get; set; }
    [Column("record_name")] public string? RecordName { get; set; }

    /// <summary>One of <see cref="AuditActions"/>.</summary>
    [Column("action")] public string Action { get; set; } = "";

    /// <summary>JSON text of the changed fields before / after. Never contains passwords or answers.</summary>
    [Column("old_values", TypeName = "jsonb")] public string? OldValues { get; set; }
    [Column("new_values", TypeName = "jsonb")] public string? NewValues { get; set; }

    [Column("changed_at")] public DateTime ChangedAt { get; set; } = DateTime.Now;
}

public static class AuditActions
{
    public const string Added = "Added";
    public const string Edited = "Edited";
    public const string Deleted = "Deleted";
}
