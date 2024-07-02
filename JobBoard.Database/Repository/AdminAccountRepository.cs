using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.AdminAccount;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Database.Repository
{
    public class AdminAccountRepository : RepositoryBase<AdminAccount, AdminAccountModel, GetAdminAccountRequest>, IAdminAccountRepository
    {

        public AdminAccountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = AdminAccountExpressions.Model;
        }

        public Task<AdminAccount?> GetByLoginAsync(string login)
            => _dbContext.AdminAccounts.FirstOrDefaultAsync(e => e.Login == login);
    }
}
