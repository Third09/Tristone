using System.Text.Json;
using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>One line in an Edit History tab: one changed field (or the whole record for added/deleted).</summary>
public record HistoryRow(DateTime ChangedAt, string Who, string Action, string Record, string Field, string OldValue, string NewValue);

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

    /// <summary>
    /// A department's Edit History, newest first, one row per changed field.
    /// <paramref name="userId"/> null = everyone; <paramref name="search"/> matches the record name.
    /// </summary>
    public static async Task<List<HistoryRow>> ListAsync(string department, DateOnly from, DateOnly to, int? userId, string search, int max = 2000)
    {
        using var db = Db.Open();
        var start = from.ToDateTime(TimeOnly.MinValue);
        var end = to.AddDays(1).ToDateTime(TimeOnly.MinValue);
        var q = from a in db.AuditLogs
                join u in db.Users on a.UserId equals u.UserId into who
                from u in who.DefaultIfEmpty()
                where a.Department == department && a.ChangedAt >= start && a.ChangedAt < end
                select new { a, Who = u == null ? "(system)" : u.FullName };
        if (userId != null) q = q.Where(x => x.a.UserId == userId);
        if (!string.IsNullOrWhiteSpace(search)) { var s = search.Trim().ToLower(); q = q.Where(x => (x.a.RecordName ?? "").ToLower().Contains(s)); }
        var rows = await q.OrderByDescending(x => x.a.ChangedAt).Take(max).ToListAsync();

        var result = new List<HistoryRow>();
        foreach (var x in rows)
        {
            var before = Flatten(x.a.OldValues);
            var after = Flatten(x.a.NewValues);
            var record = $"{x.a.RecordName} ({TableLabel(x.a.TableName)})";
            if (x.a.Action != AuditActions.Edited)
            {
                result.Add(new(x.a.ChangedAt, x.Who, x.a.Action, record, "", "", ""));
                continue;
            }
            foreach (var field in after.Keys.Union(before.Keys))
            {
                before.TryGetValue(field, out var o);
                after.TryGetValue(field, out var n);
                if (o != n) result.Add(new(x.a.ChangedAt, x.Who, x.a.Action, record, Label(field), o ?? "", n ?? ""));
            }
        }
        return result;
    }

    /// <summary>People who appear in a department's history (for the "Person" filter).</summary>
    public static async Task<List<(int UserId, string Name)>> PeopleAsync(string department)
    {
        using var db = Db.Open();
        var list = await (from a in db.AuditLogs
                          join u in db.Users on a.UserId equals u.UserId
                          where a.Department == department
                          select new { u.UserId, u.FullName }).Distinct().OrderBy(x => x.FullName).ToListAsync();
        return list.Select(x => (x.UserId, x.FullName)).ToList();
    }

    // {"daily_rate":650,"gov_ids_on_file":{"sss":true}} -> daily_rate=650, gov_ids_on_file.sss=true
    static Dictionary<string, string?> Flatten(string? json)
    {
        var map = new Dictionary<string, string?>();
        if (string.IsNullOrEmpty(json)) return map;
        void Walk(JsonElement e, string prefix)
        {
            if (e.ValueKind == JsonValueKind.Object)
                foreach (var p in e.EnumerateObject()) Walk(p.Value, prefix == "" ? p.Name : $"{prefix}.{p.Name}");
            else map[prefix] = e.ValueKind == JsonValueKind.Null ? null : e.ValueKind == JsonValueKind.String ? e.GetString() : e.ToString();
        }
        using var doc = JsonDocument.Parse(json);
        Walk(doc.RootElement, "");
        return map;
    }

    // "daily_rate" -> "Daily rate"
    static string Label(string field) => field.Length == 0 ? "" :
        char.ToUpper(field[0]) + field[1..].Replace("_", " ").Replace(".", " > ");

    static string TableLabel(string table) => table switch
    {
        "employees" => "employee",
        "material_usages" => "material usage",
        "maintenance_logs" => "maintenance",
        "vendor_documents" => "vendor paper",
        "deliveries" => "delivery",
        "sub_jobs" => "job",
        "sub_job_stages" => "stage",
        "sub_job_documents" => "job paper",
        "sub_progress" => "progress",
        "sub_vouchers" => "voucher",
        "sub_payouts" => "payout",
        "change_orders" => "change order",
        "attendance_imports" => "scanner upload",
        "attendance_days" => "attendance",
        "cash_advances" => "cash advance",
        "payroll_runs" => "payroll",
        "app_settings" => "settings",
        _ => table.TrimEnd('s').Replace("_", " "),
    };
}
