using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Application.Command
{
    public class GetCompanyAccountCommand : GetCompanyRequest, IGetEntityCommand<CompanyModel>
    {
    }

    public class GetCompanyAccountHandler : GetEntityHandler<CompanyModel, GetCompanyRequest, GetCompanyAccountCommand>
    {
        public GetCompanyAccountHandler(ICompanyAccountRepository repository) : base(repository)
        {
        }
    }
}
