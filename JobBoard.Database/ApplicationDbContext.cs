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
        public DbSet<JobOfferRequirement> JobOfferRequirements { get; set; }
        public DbSet<Business> Businesses { get; set; }

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

            modelBuilder.Entity<JobOffer>()
                .HasMany(o => o.Requirements)
                .WithOne(r => r.Offer)
                .HasForeignKey(r => r.OfferId);

            modelBuilder.Entity<JobOfferRequirement>()
                .Property(e => e.Content).HasMaxLength(300);

            modelBuilder.Entity<JobOffer>()
                .Property(e => e.Description).HasMaxLength(1000);

            modelBuilder.Entity<JobOffer>()
                .Property(e => e.Title).HasMaxLength(100);

            modelBuilder.Entity<JobOffer>()
                .Property(e => e.Location).HasMaxLength(100);

            modelBuilder.Entity<Business>()
                .HasMany(e => e.JobOffers)
                .WithOne(e => e.Business)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
