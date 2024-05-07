using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.EmployeeAccount;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Database.Repository
{
    public class EmployeeAccountRepository : IEmployeeAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeAccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(EmployeeAccount account)
        {
            await _dbContext.EmployeeAccounts.AddAsync(account);

            await _dbContext.SaveChangesAsync();
        }

        public Task<IEnumerable<EmployeeAccountModel>> GetAsync(GetEmployeeAccountRequest request)
        {
            var result = _dbContext.EmployeeAccounts
                .AddPagination(request)
                .Select(EmployeeAccountExpressions.Model)
                .AsEnumerable();

            return Task.FromResult(result);
        }

        public Task<EmployeeAccount?> GetByEmailAsync(string email)
            => _dbContext.EmployeeAccounts.Where(a => a.Email == email).FirstOrDefaultAsync();

        public Task<EmployeeAccountModel?> GetByIdAsync(long id)
            => _dbContext.EmployeeAccounts.Where(a => a.Id == id)
                .Select(EmployeeAccountExpressions.Model)
                .FirstOrDefaultAsync();
    }
}
