using Company.OfficeApp.Data;
using Company.Shared.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

public record JobRow(int JobId, string Subcontractor, string Project, string Scope, decimal ContractAmount, decimal PercentDone, string Status)
{
    public override string ToString() => $"#{JobId} {Subcontractor} - {Project}";
}

public record StageInput(string Name, decimal Percent);

public record JobInput(int SubcontractorId, int ProjectId, string Scope, decimal ContractAmount, DateOnly StartDate, DateOnly EndDate,
                       decimal RetentionPct, IReadOnlyList<StageInput> Stages);

public record JobDocRow(int DocumentId, string OriginalName, DateTime UploadedAt, string FilePath);

public record ProgressRow(int ProgressId, DateOnly Date, decimal Percent, string? Notes, string? FileName, string? FilePath);

/// <summary>
/// Subcontractors > Jobs & Progress (Project Outline, Part 4). A job = subcontractor + project + scope + agreed
/// amount + dates + papers, paid by fixed stages that must add up to 100%, with a retention % held back from every
/// payment (default 10). Once a stage is reached or a voucher exists, the amount, retention and stages are locked.
/// </summary>
public static class SubJobService
{
    const string Dept = Departments.Subcontractors;
    public const string Ongoing = "Ongoing", Finished = "Finished", PaidInFull = "Paid in Full";

    /// <summary>Default stages for a new job (can be changed before saving).</summary>
    public static readonly StageInput[] DefaultStages = [new("Down payment", 30), new("Midway", 40), new("Completion", 30)];

    public static async Task<List<JobRow>> ListAsync(int? projectId = null, int? subcontractorId = null)
    {
        using var db = Db.Open();
        var jobs = await (from j in db.SubJobs
                          join s in db.Subcontractors on j.SubcontractorId equals s.SubcontractorId
                          join p in db.Projects on j.ProjectId equals p.ProjectId
                          where (projectId == null || j.ProjectId == projectId) && (subcontractorId == null || j.SubcontractorId == subcontractorId)
                          orderby j.StartDate descending, j.JobId descending
                          select new { j.JobId, Sub = s.BusinessName, Project = p.ProjectName, j.Scope, j.ContractAmount }).ToListAsync();
        var ids = jobs.Select(j => j.JobId).ToList();
        var percent = await LatestPercentAsync(db, ids);
        var paid = await db.SubPayouts.Where(x => ids.Contains(x.JobId)).GroupBy(x => x.JobId)
            .Select(g => new { g.Key, Sum = g.Sum(x => x.Amount) }).ToDictionaryAsync(x => x.Key, x => x.Sum);

        return jobs.Select(j =>
        {
            var pct = percent.GetValueOrDefault(j.JobId);
            var status = paid.GetValueOrDefault(j.JobId) >= j.ContractAmount ? PaidInFull : pct >= 100 ? Finished : Ongoing;
            return new JobRow(j.JobId, j.Sub, j.Project, j.Scope, j.ContractAmount, pct, status);
        }).ToList();
    }

    /// <summary>The latest progress % of each job (by date, then the newest entry).</summary>
    internal static async Task<Dictionary<int, decimal>> LatestPercentAsync(AppDbContext db, List<int> jobIds)
    {
        var rows = await db.SubProgress.AsNoTracking().Where(p => jobIds.Contains(p.JobId))
            .Select(p => new { p.JobId, p.ProgressDate, p.ProgressId, p.Percent }).ToListAsync();
        return rows.GroupBy(r => r.JobId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(r => r.ProgressDate).ThenByDescending(r => r.ProgressId).First().Percent);
    }

    public static async Task<(SubJob Job, List<SubJobStage> Stages, bool Locked)?> GetAsync(int jobId)
    {
        using var db = Db.Open();
        var j = await db.SubJobs.AsNoTracking().FirstOrDefaultAsync(x => x.JobId == jobId);
        if (j == null) return null;
        var stages = await db.SubJobStages.AsNoTracking().Where(s => s.JobId == jobId).OrderBy(s => s.StageNo).ToListAsync();
        return (j, stages, await IsLockedAsync(db, jobId));
    }

    static async Task<bool> IsLockedAsync(AppDbContext db, int jobId) =>
        await db.SubJobStages.AnyAsync(s => s.JobId == jobId && s.ReachedOn != null) ||
        await db.SubVouchers.AnyAsync(v => v.JobId == jobId && v.Status != VoucherStatuses.Rejected);

    public static string? CheckInput(JobInput i)
    {
        if (i.SubcontractorId <= 0) return "Choose the subcontractor.";
        if (i.ProjectId <= 0) return "Choose the project.";
        if (string.IsNullOrWhiteSpace(i.Scope)) return "Type the scope of work.";
        if (i.ContractAmount <= 0) return "Type the agreed contract amount.";
        if (i.EndDate < i.StartDate) return "The end date can't be before the start date.";
        if (i.RetentionPct < 0 || i.RetentionPct >= 100) return "Retention must be from 0 to 99%.";
        if (i.Stages.Count == 0) return "Add at least one payment stage.";
        if (i.Stages.Any(s => string.IsNullOrWhiteSpace(s.Name))) return "Every stage needs a name.";
        if (i.Stages.Any(s => s.Percent <= 0)) return "Every stage needs a % above 0.";
        var total = i.Stages.Sum(s => s.Percent);
        if (total != 100) return $"The stages add up to {total:0.##}%. They must add up to 100%.";
        return null;
    }

    public static async Task<(int? Id, string? Error)> CreateAsync(JobInput i)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return (null, denied);
        if (CheckInput(i) is { } problem) return (null, problem);
        using var db = Db.Open();
        var p = await db.Projects.FirstOrDefaultAsync(x => x.ProjectId == i.ProjectId);
        if (p == null || p.Status == ProjectStatuses.Cancelled) return (null, "Choose a project that isn't Cancelled.");
        var j = new SubJob { CreatedBy = Session.CurrentUser!.UserId };
        Apply(j, i);
        db.SubJobs.Add(j);
        await db.SaveChangesAsync();
        AddStages(db, j.JobId, i.Stages);
        AuditService.Add(db, Dept, "sub_jobs", j.JobId, await JobNameAsync(db, j), AuditActions.Added, null, Snapshot(j, i.Stages));
        await db.SaveChangesAsync();
        return (j.JobId, null);
    }

    public static async Task<string?> UpdateAsync(int jobId, JobInput i)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return denied;
        if (CheckInput(i) is { } problem) return problem;
        using var db = Db.Open();
        var j = await db.SubJobs.FirstAsync(x => x.JobId == jobId);
        var stages = await db.SubJobStages.Where(s => s.JobId == jobId).OrderBy(s => s.StageNo).ToListAsync();
        var oldStages = stages.Select(s => new StageInput(s.Name, s.Percent)).ToList();
        var moneyChanged = j.ContractAmount != i.ContractAmount || j.RetentionPct != i.RetentionPct
                           || j.SubcontractorId != i.SubcontractorId || j.ProjectId != i.ProjectId
                           || !oldStages.Select(s => (s.Name, s.Percent)).SequenceEqual(i.Stages.Select(s => (s.Name.Trim(), s.Percent)));
        if (moneyChanged && await IsLockedAsync(db, jobId))
            return "Billing has started on this job, so the subcontractor, project, amount, retention and stages can't change. Only the scope and dates can.";

        var before = Snapshot(j, oldStages);
        Apply(j, i);
        if (moneyChanged)
        {
            db.SubJobStages.RemoveRange(stages);
            AddStages(db, jobId, i.Stages);
        }
        AuditService.Add(db, Dept, "sub_jobs", j.JobId, await JobNameAsync(db, j), AuditActions.Edited, before, Snapshot(j, i.Stages));
        await db.SaveChangesAsync();
        return null;
    }

    /// <summary>Only a job with no progress, papers or billing yet (e.g. entered by mistake).</summary>
    public static async Task<string?> DeleteAsync(int jobId)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return denied;
        using var db = Db.Open();
        var j = await db.SubJobs.FirstAsync(x => x.JobId == jobId);
        if (await IsLockedAsync(db, jobId) || await db.SubProgress.AnyAsync(p => p.JobId == jobId) || await db.SubJobDocuments.AnyAsync(d => d.JobId == jobId))
            return "This job already has progress, papers or billing, so it can't be deleted.";
        AuditService.Add(db, Dept, "sub_jobs", j.JobId, await JobNameAsync(db, j), AuditActions.Deleted, Snapshot(j, []), null);
        db.SubVouchers.RemoveRange(db.SubVouchers.Where(v => v.JobId == jobId));   // only rejected ones can be left
        db.SubJobStages.RemoveRange(db.SubJobStages.Where(s => s.JobId == jobId));
        db.SubJobs.Remove(j);
        await db.SaveChangesAsync();
        return null;
    }

    static void Apply(SubJob j, JobInput i)
    {
        j.SubcontractorId = i.SubcontractorId; j.ProjectId = i.ProjectId; j.Scope = i.Scope.Trim();
        j.ContractAmount = i.ContractAmount; j.StartDate = i.StartDate; j.EndDate = i.EndDate; j.RetentionPct = i.RetentionPct;
        j.UpdatedAt = DateTime.Now;
    }

    static void AddStages(AppDbContext db, int jobId, IReadOnlyList<StageInput> stages)
    {
        for (int n = 0; n < stages.Count; n++)
            db.SubJobStages.Add(new SubJobStage { JobId = jobId, StageNo = n + 1, Name = stages[n].Name.Trim(), Percent = stages[n].Percent });
    }

    internal static async Task<string> JobNameAsync(AppDbContext db, SubJob j)
    {
        var sub = await db.Subcontractors.Where(s => s.SubcontractorId == j.SubcontractorId).Select(s => s.BusinessName).FirstAsync();
        var project = await db.Projects.Where(p => p.ProjectId == j.ProjectId).Select(p => p.ProjectName).FirstAsync();
        return $"{sub} - {project}";
    }

    static object Snapshot(SubJob j, IReadOnlyList<StageInput> stages) => new
    {
        scope = j.Scope, contract_amount = j.ContractAmount, start_date = j.StartDate, end_date = j.EndDate,
        retention_pct = j.RetentionPct, stages = string.Join(", ", stages.Select(s => $"{s.Name.Trim()} {s.Percent:0.##}%")),
    };

    // ----- supporting papers -----

    public static async Task<List<JobDocRow>> ListDocumentsAsync(int jobId)
    {
        using var db = Db.Open();
        return await db.SubJobDocuments.AsNoTracking().Where(d => d.JobId == jobId).OrderBy(d => d.UploadedAt)
            .Select(d => new JobDocRow(d.DocumentId, d.OriginalName, d.UploadedAt, d.FilePath)).ToListAsync();
    }

    public static async Task<string?> UploadDocumentAsync(int jobId, string sourcePath)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return denied;
        if (FileStore.Check(sourcePath) is { } problem) return problem;
        var relative = await FileStore.SaveAsync(sourcePath, "subcontractors");
        using var db = Db.Open();
        var j = await db.SubJobs.FirstAsync(x => x.JobId == jobId);
        var d = new SubJobDocument { JobId = jobId, FilePath = relative, OriginalName = Path.GetFileName(sourcePath), UploadedBy = Session.CurrentUser!.UserId };
        db.SubJobDocuments.Add(d);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "sub_job_documents", d.DocumentId, await JobNameAsync(db, j), AuditActions.Added, null, new { file = d.OriginalName });
        await db.SaveChangesAsync();
        return null;
    }

    // ----- progress -----

    public static async Task<List<ProgressRow>> ListProgressAsync(int jobId)
    {
        using var db = Db.Open();
        return await db.SubProgress.AsNoTracking().Where(p => p.JobId == jobId)
            .OrderByDescending(p => p.ProgressDate).ThenByDescending(p => p.ProgressId)
            .Select(p => new ProgressRow(p.ProgressId, p.ProgressDate, p.Percent, p.Notes, p.OriginalName, p.FilePath)).ToListAsync();
    }

    public static async Task<decimal> LatestPercentAsync(int jobId)
    {
        using var db = Db.Open();
        return (await LatestPercentAsync(db, [jobId])).GetValueOrDefault(jobId);
    }

    /// <summary>Saves a progress update (% completed on a date), with an optional photo or document.</summary>
    public static async Task<string?> AddProgressAsync(int jobId, decimal percent, DateOnly date, string? notes, string? sourcePath)
    {
        if (Access.RequirePage(AppPage.Subcontractors) is { } denied) return denied;
        if (percent < 0 || percent > 100) return "The % completed must be from 0 to 100.";
        if (date > DateOnly.FromDateTime(DateTime.Today)) return "The date can't be in the future.";
        if (sourcePath != null && FileStore.Check(sourcePath) is { } problem) return problem;
        var relative = sourcePath == null ? null : await FileStore.SaveAsync(sourcePath, "subcontractors");
        using var db = Db.Open();
        var j = await db.SubJobs.FirstAsync(x => x.JobId == jobId);
        var p = new SubProgress
        {
            JobId = jobId, Percent = percent, ProgressDate = date, Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim(),
            FilePath = relative, OriginalName = sourcePath == null ? null : Path.GetFileName(sourcePath), CreatedBy = Session.CurrentUser!.UserId,
        };
        db.SubProgress.Add(p);
        await db.SaveChangesAsync();
        AuditService.Add(db, Dept, "sub_progress", p.ProgressId, await JobNameAsync(db, j), AuditActions.Added, null,
            new { percent, date, notes = p.Notes, file = p.OriginalName });
        await db.SaveChangesAsync();
        return null;
    }
}
