using JobBoard.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CompanyAccount> CompanyAccounts { get; set; }
        public DbSet<EmployeeAccount> EmployeeAccounts { get; set; }
        public DbSet<AdminAccount> AdminAccounts { get; set; }
        public DbSet<AdminAccountRefreshToken> AdminAccountRefreshTokens { get; set; }
        public DbSet<CompanyAccountRefreshToken> CompanyAccountRefreshTokens { get; set; }
        public DbSet<EmployeeAccountRefreshToken> EmployeeAccountRefreshTokens { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }

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

            modelBuilder.Entity<AdminAccount>()
                .Property(e => e.Login).HasMaxLength(100);

            modelBuilder.Entity<AdminAccountRefreshToken>()
                .HasKey(e => e.Token);

            modelBuilder.Entity<AdminAccount>()
                .HasMany(e => e.RefreshTokens)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);

            modelBuilder.Entity<CompanyAccountRefreshToken>()
                .HasKey(e => e.Token);

            modelBuilder.Entity<CompanyAccount>()
                .HasMany(e => e.RefreshTokens)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);

            modelBuilder.Entity<EmployeeAccountRefreshToken>()
                .HasKey(e => e.Token);

            modelBuilder.Entity<EmployeeAccount>()
                .HasMany(e => e.RefreshTokens)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);

            modelBuilder.Entity<CompanyAccount>()
                .HasMany(e => e.JobOffers)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.CompanyId);
        }
    }
}
