using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Application.Command
{
    public class GetEmployeeAccountCommand : GetEmployeeAccountRequest, IGetEntityCommand<EmployeeAccountModel>
    {
    }

    public class GetEmployeeAccountHandler : GetEntityHandler<EmployeeAccountModel, GetEmployeeAccountRequest, GetEmployeeAccountCommand>
    {
        public GetEmployeeAccountHandler(IEmployeeAccountRepository repository) : base(repository)
        {
        }
    }
}
