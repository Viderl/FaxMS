using Microsoft.EntityFrameworkCore;
using FaxMS.Models;

namespace FaxMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<SourceSystem> SourceSystems { get; set; }
        public DbSet<SourceIp> SourceIps { get; set; }
        public DbSet<FaxRecord> FaxRecords { get; set; }
        public DbSet<DepartmentSnapshot> DepartmentSnapshots { get; set; }
        public DbSet<FaxViewLog> FaxViewLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SourceSystem>()
                .HasMany(s => s.SourceIps)
                .WithOne(ip => ip.SourceSystem)
                .HasForeignKey(ip => ip.SourceSystemId);

            modelBuilder.Entity<FaxRecord>()
                .HasOne(f => f.SourceSystem)
                .WithMany()
                .HasForeignKey(f => f.SourceSystemId);

            modelBuilder.Entity<FaxViewLog>()
                .HasOne(v => v.FaxRecord)
                .WithMany()
                .HasForeignKey(v => v.FaxRecordId);
        }
    }
}