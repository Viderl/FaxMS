using Microsoft.EntityFrameworkCore;
using FaxMS.Models;

namespace FaxMS.Data
{
    public class FaxMSDbContext : DbContext
    {
        public FaxMSDbContext(DbContextOptions<FaxMSDbContext> options) : base(options) { }

        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<SourceSystem> SourceSystems => Set<SourceSystem>();
        public DbSet<SourceSystemIP> SourceSystemIPs => Set<SourceSystemIP>();
        public DbSet<FaxRecord> FaxRecords => Set<FaxRecord>();
        public DbSet<FaxRecordAccess> FaxRecordAccesses => Set<FaxRecordAccess>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SourceSystem>()
                .HasMany(s => s.IPs)
                .WithOne(ip => ip.SourceSystem)
                .HasForeignKey(ip => ip.SourceSystemId);
        }
    }
}
