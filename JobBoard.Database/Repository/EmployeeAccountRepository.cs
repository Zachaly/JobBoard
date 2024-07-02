using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.EmployeeAccount;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Database.Repository
{
    public class EmployeeAccountRepository : RepositoryBase<EmployeeAccount, EmployeeAccountModel, GetEmployeeAccountRequest>,
        IEmployeeAccountRepository
    {
        public EmployeeAccountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = EmployeeAccountExpressions.Model;
        }

        public Task<EmployeeAccount?> GetByEmailAsync(string email)
            => _dbContext.EmployeeAccounts.Where(a => a.Email == email).FirstOrDefaultAsync();       
    }
}
