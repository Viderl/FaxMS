using FaxMS.Models;
using Microsoft.EntityFrameworkCore;

namespace FaxMS.Data
{
    public class FaxMSDbContext : DbContext
    {
        public FaxMSDbContext(DbContextOptions<FaxMSDbContext> options) : base(options) { }

        public DbSet<FaxRecord> FaxRecords { get; set; }
        public DbSet<FaxViewLog> FaxViewLogs { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<SourceSystem> SourceSystems { get; set; }
        public DbSet<SourceSystemIP> SourceSystemIPs { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 設定一對多關聯
            modelBuilder.Entity<SourceSystem>()
                .HasMany(s => s.IPs)
                .WithOne(ip => ip.SourceSystem)
                .HasForeignKey(ip => ip.SourceSystemId);

            modelBuilder.Entity<FaxRecord>()
                .HasMany(f => f.ViewLogs)
                .WithOne(v => v.FaxRecord)
                .HasForeignKey(v => v.FaxRecordId);
        }
    }
}
