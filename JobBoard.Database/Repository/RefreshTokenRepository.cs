using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Database.Repository
{
    public abstract class RefreshTokenRepository<TToken> : IRefreshTokenRepository<TToken>
        where TToken : class, IRefreshToken
    {
        private readonly ApplicationDbContext _dbContext;

        protected RefreshTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TToken token)
        {
            await _dbContext.Set<TToken>().AddAsync(token);

            await _dbContext.SaveChangesAsync();
        }

        public Task<bool> CheckIfTokenIsTaken(string token)
            => _dbContext.Set<TToken>().AnyAsync(t => t.Token == token);

        public Task<TToken?> GetValidTokenAsync(string token, long accountId)
            => _dbContext.Set<TToken>().FirstOrDefaultAsync(t => t.IsValid && t.AccountId == accountId 
                && t.Token == token && t.ExpirationDate <= DateTime.Now);

        public Task UpdateTokenAsync(TToken token)
        {
            _dbContext.Set<TToken>().Update(token);

            return _dbContext.SaveChangesAsync();
        }
    }

    public class AdminAccountRefreshTokenRepository : RefreshTokenRepository<AdminAccountRefreshToken>,
        IAdminAccountRefreshTokenRepository
    {
        public AdminAccountRefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class CompanyAccountRefreshTokenRepository : RefreshTokenRepository<CompanyAccountRefreshToken>,
        ICompanyAccountRefreshTokenRepository
    {
        public CompanyAccountRefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class EmployeeAccountRefreshTokenRepository : RefreshTokenRepository<EmployeeAccountRefreshToken>,
        IEmployeeAccountRefreshTokenRepository
    {
        public EmployeeAccountRefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
