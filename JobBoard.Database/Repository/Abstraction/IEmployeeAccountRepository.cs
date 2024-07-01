using JobBoard.Domain.Entity;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IEmployeeAccountRepository
    {
        Task AddAsync(EmployeeAccount account);
        Task<IEnumerable<EmployeeAccountModel>> GetAsync(GetEmployeeAccountRequest request);
        Task<EmployeeAccountModel?> GetByIdAsync(long id);
        Task<EmployeeAccount?> GetByEmailAsync(string email);
        Task UpdateAsync(EmployeeAccount account);
        Task<EmployeeAccount?> GetEntityByIdAsync(long id);
    }
}
