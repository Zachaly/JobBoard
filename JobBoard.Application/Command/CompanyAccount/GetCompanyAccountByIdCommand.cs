using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Application.Command
{
    public record GetCompanyAccountByIdCommand(long Id) : GetByIdCommand<CompanyModel>(Id);

    public class GetCompanyAccountByIdHandler : GetByIdHandler<CompanyModel, GetCompanyRequest, GetCompanyAccountByIdCommand>
    {
        public GetCompanyAccountByIdHandler(ICompanyAccountRepository repository) : base(repository)
        {
        }
    }
}
