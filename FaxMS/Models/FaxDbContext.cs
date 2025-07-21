using Microsoft.EntityFrameworkCore;

namespace FaxMS.Models
{
    public class FaxDbContext : DbContext
    {
        public FaxDbContext(DbContextOptions<FaxDbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<SourceSystem> SourceSystems { get; set; }
        public DbSet<SourceSystemIP> SourceSystemIPs { get; set; }
        public DbSet<Fax> Faxes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<SourceSystem>().ToTable("SourceSystems");
            modelBuilder.Entity<SourceSystemIP>().ToTable("SourceSystemIPs");
            modelBuilder.Entity<Fax>().ToTable("Faxes");
        }
    }
}