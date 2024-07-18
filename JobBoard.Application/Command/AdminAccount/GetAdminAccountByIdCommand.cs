using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.AdminAccount;

namespace JobBoard.Application.Command
{
    public record GetAdminAccountByIdCommand(long Id) : GetByIdCommand<AdminAccountModel>(Id);

    public class GetAdminAccountByIdHandler : GetByIdHandler<AdminAccountModel, GetAdminAccountRequest, GetAdminAccountByIdCommand>
    {
        public GetAdminAccountByIdHandler(IAdminAccountRepository repository) : base(repository)
        {
        }
    }
}
