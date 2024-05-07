using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model;
using JobBoard.Model.EmployeeAccount;
namespace JobBoard.Database.Repository
{
    public class EmployeeAccountRepository : IEmployeeAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeAccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(EmployeeAccount account)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeAccountModel>> GetAsync(PagedRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeAccountModel>> GetAsync(GetEmployeeAccountRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeAccount?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeAccountModel?> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
