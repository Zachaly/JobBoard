using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.AdminAccount;

namespace JobBoard.Application.Command
{
    public class GetAdminAccountCountCommand : GetAdminAccountRequest, IGetCountCommand
    {
    }

    public class GetAdminAccountCountHandler : GetCountHandler<AdminAccountModel, GetAdminAccountRequest, GetAdminAccountCountCommand>
    {
        public GetAdminAccountCountHandler(IAdminAccountRepository repository) : base(repository)
        {
        }
    }
}
