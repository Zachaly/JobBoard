using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.CompanyAccount;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Database.Repository
{
    public class CompanyAccountRepository : RepositoryBase<CompanyAccount, CompanyModel, GetCompanyRequest>, ICompanyAccountRepository
    {
        public CompanyAccountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = CompanyAccountExpressions.Model;
        }

        public Task<CompanyAccount?> GetByEmailAsync(string email)
            => _dbContext.CompanyAccounts
                .Where(account => account.Email == email)
                .FirstOrDefaultAsync();
    }
}
