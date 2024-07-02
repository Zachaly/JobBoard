using JobBoard.Domain.Entity;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IEmployeeAccountRepository : IRepositoryBase<EmployeeAccount, EmployeeAccountModel, GetEmployeeAccountRequest>
    {
        Task<EmployeeAccount?> GetByEmailAsync(string email);
    }
}
