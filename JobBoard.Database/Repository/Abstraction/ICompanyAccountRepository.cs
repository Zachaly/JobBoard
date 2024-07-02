using JobBoard.Domain.Entity;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface ICompanyAccountRepository : IRepositoryBase<CompanyAccount, CompanyModel, GetCompanyRequest>
    {
        Task<CompanyAccount?> GetByEmailAsync(string email);
    }
}
