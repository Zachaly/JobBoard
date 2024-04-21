using JobBoard.Model.CompanyAccount;
using JobBoard.Model.Response;

namespace JobBoard.Application.Service.Abstraction
{
    public interface ICompanyAccountService
    {
        Task<ResponseModel> AddAsync(AddCompanyAccountRequest request);
        Task<IEnumerable<CompanyModel>> GetAsync(GetCompanyRequest request);
        Task<CompanyModel?> GetById(long id);
    }
}
