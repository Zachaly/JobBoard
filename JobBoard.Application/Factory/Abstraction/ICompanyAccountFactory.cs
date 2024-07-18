using JobBoard.Domain.Entity;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface ICompanyAccountFactory : IUpdateFactory<CompanyAccount, UpdateCompanyAccountRequest>
    {
        public CompanyAccount Create(AddCompanyAccountRequest request, string passwordHash);
    }
}
