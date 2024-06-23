using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.AdminAccount;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoard.Database.Repository
{
    public class AdminAccountRepository : IAdminAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private Expression<Func<AdminAccount, AdminAccountModel>> ModelExpression { get; }

        public AdminAccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            ModelExpression = AdminAccountExpressions.Model;
        }

        public async Task AddAsync(AdminAccount entity)
        {
            await _dbContext.AdminAccounts.AddAsync(entity);

            await _dbContext.SaveChangesAsync();
        }

        public Task<IEnumerable<AdminAccountModel>> GetAsync(GetAdminAccountRequest request)
        {
            var query = _dbContext.AdminAccounts
                .AddPagination(request)
                .Select(ModelExpression);

            return Task.FromResult(query.AsEnumerable());
        }

        public Task<AdminAccountModel?> GetByIdAsync(long id)
            => _dbContext.AdminAccounts.Where(e => e.Id == id).Select(ModelExpression).FirstOrDefaultAsync();

        public Task<AdminAccount?> GetByLoginAsync(string login)
            => _dbContext.AdminAccounts.FirstOrDefaultAsync(e => e.Login == login);
    }
}
