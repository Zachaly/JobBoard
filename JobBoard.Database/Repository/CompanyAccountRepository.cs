using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.CompanyAccount;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Database.Repository
{
    public class CompanyAccountRepository : ICompanyAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompanyAccountRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(CompanyAccount account)
        {
            _dbContext.Add(account);

            return _dbContext.SaveChangesAsync();
        }

        public Task<IEnumerable<CompanyModel>> GetAsync(GetCompanyRequest request)
        {
            var query = _dbContext.CompanyAccounts
                .AddPagination(request)
                .Select(CompanyAccountExpressions.Model)
                .AsEnumerable();

            return Task.FromResult(query);
        }

        public Task<CompanyAccount?> GetByEmailAsync(string email)
            => _dbContext.CompanyAccounts
                .Where(account => account.Email == email)
                .FirstOrDefaultAsync();

        public Task<CompanyModel?> GetByIdAsync(long id)
            => _dbContext.CompanyAccounts
                .Where(account => account.Id == id)
                .Select(CompanyAccountExpressions.Model)
                .FirstOrDefaultAsync();
    }
}
