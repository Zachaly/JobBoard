using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.AdminAccount;

namespace JobBoard.Application.Command
{
    public class GetAdminAccountCommand : GetAdminAccountRequest, IGetEntityCommand<AdminAccountModel>
    {
    }

    public class GetAdminAccountHandler : GetEntityHandler<AdminAccountModel, GetAdminAccountRequest, GetAdminAccountCommand>
    {
        public GetAdminAccountHandler(IAdminAccountRepository repository) : base(repository)
        {
        }
    }
}
