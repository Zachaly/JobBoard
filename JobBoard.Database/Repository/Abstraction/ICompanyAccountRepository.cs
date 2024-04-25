using JobBoard.Domain.Entity;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface ICompanyAccountRepository
    {
        Task AddAsync(CompanyAccount account);
        Task<IEnumerable<CompanyModel>> GetAsync(GetCompanyRequest request);
        Task<CompanyModel?> GetByIdAsync(long id);
        Task<CompanyModel?> GetByEmailAsync(string email);
    }
}
