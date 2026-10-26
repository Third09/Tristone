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

    /// <summary>Creates a context for a plain Npgsql connection string.</summary>
    public static AppDbContext Create(string connectionString) =>
        new(new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString).Options);

    // The tables use "timestamp" (plain local time, no time zone): the office is in one time zone.
    // Without this, Npgsql treats DateTime as "timestamp with time zone" and only accepts UTC.
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
        configurationBuilder.Properties<DateTime?>().HaveColumnType("timestamp without time zone");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<LoginLog>().HasKey(l => l.LogId);
        modelBuilder.Entity<PasswordRequest>().HasKey(r => r.RequestId);
        modelBuilder.Entity<AuditLog>().HasKey(a => a.AuditId);
        modelBuilder.Entity<Project>().HasKey(p => p.ProjectId);
        modelBuilder.Entity<Project>().Property(p => p.ContractPrice).HasColumnType("numeric(14,2)");
    }
}
