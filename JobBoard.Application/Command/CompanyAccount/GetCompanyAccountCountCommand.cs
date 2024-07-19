using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Application.Command
{
    public class GetCompanyAccountCountCommand : GetCompanyRequest, IGetCountCommand
    {
    }

    public class GetCompanyAccountCountHandler : GetCountHandler<CompanyModel, GetCompanyRequest, GetCompanyAccountCountCommand>
    {
        public GetCompanyAccountCountHandler(ICompanyAccountRepository repository) : base(repository)
        {
            
        }
    }
}
