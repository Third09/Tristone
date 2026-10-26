using System.Text.Json;
using Company.Shared.Data;
using Company.Shared.Models;

namespace Company.OfficeApp.Services;

/// <summary>
/// Adds an Edit History row (audit_logs) to the same save as the change itself.
/// Pass only safe fields as before/after: never passwords, hashes or security answers.
/// </summary>
public static class AuditService
{
    /// <param name="department">Which department's Edit History tab shows it; null = Admin-only records.</param>
    public static void Add(AppDbContext db, string? department, string table, int recordId, string recordName,
                           string action, object? before, object? after)
    {
        db.AuditLogs.Add(new AuditLog
        {
            UserId = Session.CurrentUser?.UserId,
            Department = department,
            TableName = table,
            RecordId = recordId,
            RecordName = recordName,
            Action = action,
            OldValues = before == null ? null : JsonSerializer.Serialize(before),
            NewValues = after == null ? null : JsonSerializer.Serialize(after),
            ChangedAt = DateTime.Now,
        });
    }
}
