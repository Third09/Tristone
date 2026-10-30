using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Shared.Data;

/// <summary>
/// The connection to construction_db. Tables are created by the numbered scripts in /database,
/// never by Entity Framework, so this class only maps classes to existing tables.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<LoginLog> LoginLogs => Set<LoginLog>();
    public DbSet<PasswordRequest> PasswordRequests => Set<PasswordRequest>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeeRate> EmployeeRates => Set<EmployeeRate>();
    public DbSet<AppSetting> AppSettings => Set<AppSetting>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<MaterialUsage> MaterialUsages => Set<MaterialUsage>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<VendorDocument> VendorDocuments => Set<VendorDocument>();
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<MaintenanceLog> MaintenanceLogs => Set<MaintenanceLog>();
    public DbSet<Subcontractor> Subcontractors => Set<Subcontractor>();
    public DbSet<SubcontractorTrade> SubcontractorTrades => Set<SubcontractorTrade>();
    public DbSet<SubJob> SubJobs => Set<SubJob>();
    public DbSet<SubJobStage> SubJobStages => Set<SubJobStage>();
    public DbSet<SubJobDocument> SubJobDocuments => Set<SubJobDocument>();
    public DbSet<SubProgress> SubProgress => Set<SubProgress>();
    public DbSet<SubVoucher> SubVouchers => Set<SubVoucher>();
    public DbSet<SubPayout> SubPayouts => Set<SubPayout>();
    public DbSet<Receipt> Receipts => Set<Receipt>();
    public DbSet<ChangeOrder> ChangeOrders => Set<ChangeOrder>();
    public DbSet<AttendanceImport> AttendanceImports => Set<AttendanceImport>();
    public DbSet<AttendanceScan> AttendanceScans => Set<AttendanceScan>();
    public DbSet<AttendanceDay> AttendanceDays => Set<AttendanceDay>();
    public DbSet<Inquiry> Inquiries => Set<Inquiry>();

    /// <summary>Creates a context for a plain Npgsql connection string.</summary>
    public static AppDbContext Create(string connectionString) =>
        new(new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString).Options);

    // The tables use "timestamp" (plain local time, no time zone): the office is in one time zone.
    // Without this, Npgsql treats DateTime as "timestamp with time zone" and only accepts UTC.
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
        configurationBuilder.Properties<DateTime?>().HaveColumnType("timestamp without time zone");
        // Money and quantities are NUMERIC in the database (exact, never floating point).
        configurationBuilder.Properties<decimal>().HaveColumnType("numeric");
        configurationBuilder.Properties<decimal?>().HaveColumnType("numeric");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<Inquiry>().HasKey(i => i.InquiryId);
        modelBuilder.Entity<LoginLog>().HasKey(l => l.LogId);
        modelBuilder.Entity<PasswordRequest>().HasKey(r => r.RequestId);
        modelBuilder.Entity<AuditLog>().HasKey(a => a.AuditId);
        modelBuilder.Entity<Project>().HasKey(p => p.ProjectId);
        modelBuilder.Entity<Project>().Property(p => p.ContractPrice).HasColumnType("numeric(14,2)");
        modelBuilder.Entity<Employee>().HasKey(e => e.EmployeeId);
        modelBuilder.Entity<EmployeeRate>().HasKey(r => r.RateId);
        modelBuilder.Entity<EmployeeRate>().Property(r => r.DailyRate).HasColumnType("numeric(10,2)");
        modelBuilder.Entity<AppSetting>().HasKey(s => s.Key);
        modelBuilder.Entity<Material>().HasKey(m => m.MaterialId);
        modelBuilder.Entity<MaterialUsage>().HasKey(u => u.UsageId);
        modelBuilder.Entity<Vendor>().HasKey(v => v.VendorId);
        modelBuilder.Entity<VendorDocument>().HasKey(d => d.DocumentId);
        modelBuilder.Entity<Quote>().HasKey(q => q.QuoteId);
        modelBuilder.Entity<Purchase>().HasKey(p => p.PurchaseId);
        modelBuilder.Entity<Delivery>().HasKey(d => d.DeliveryId);
        modelBuilder.Entity<Equipment>().HasKey(e => e.EquipmentId);
        modelBuilder.Entity<MaintenanceLog>().HasKey(l => l.LogId);

        modelBuilder.Entity<Subcontractor>().HasKey(s => s.SubcontractorId);
        modelBuilder.Entity<SubcontractorTrade>().HasKey(t => new { t.SubcontractorId, t.Trade });
        modelBuilder.Entity<SubJob>().HasKey(j => j.JobId);
        modelBuilder.Entity<SubJobStage>().HasKey(s => s.StageId);
        modelBuilder.Entity<SubJobDocument>().HasKey(d => d.DocumentId);
        modelBuilder.Entity<SubProgress>().HasKey(p => p.ProgressId);
        modelBuilder.Entity<SubVoucher>().HasKey(v => v.VoucherId);
        modelBuilder.Entity<SubPayout>().HasKey(p => p.PayoutId);

        modelBuilder.Entity<Receipt>().HasKey(r => r.ReceiptId);
        modelBuilder.Entity<ChangeOrder>().HasKey(c => c.ChangeOrderId);

        modelBuilder.Entity<AttendanceImport>().HasKey(i => i.ImportId);
        modelBuilder.Entity<AttendanceScan>().HasKey(s => s.ScanId);
        modelBuilder.Entity<AttendanceDay>().HasKey(d => d.DayId);
    }
}
