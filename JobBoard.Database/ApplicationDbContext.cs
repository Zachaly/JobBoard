using JobBoard.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CompanyAccount> CompanyAccounts { get; set; }
        public DbSet<EmployeeAccount> EmployeeAccounts { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompanyAccount>()
                .Property(e => e.Name).HasMaxLength(200);

            modelBuilder.Entity<EmployeeAccount>()
                .Property(e => e.AboutMe).HasMaxLength(5000);

            modelBuilder.Entity<EmployeeAccount>()
                .Property(e => e.City).HasMaxLength(100);

            modelBuilder.Entity<EmployeeAccount>()
                .Property(e => e.Country).HasMaxLength(100);

            modelBuilder.Entity<EmployeeAccount>()
                .Property(e => e.FirstName).HasMaxLength(200);

            modelBuilder.Entity<EmployeeAccount>()
                .Property(e => e.LastName).HasMaxLength(200);

            modelBuilder.Entity<EmployeeAccount>()
                .Property(e => e.PhoneNumber).HasMaxLength(20);
        }
    }
}
