using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Application.Command
{
    public class GetEmployeeAccountCountCommand : GetEmployeeAccountRequest, IGetCountCommand
    {
    }

    public class GetEmployeeAccountCountHandler : GetCountHandler<EmployeeAccountModel, GetEmployeeAccountRequest,
        GetEmployeeAccountCountCommand>
    {
        public GetEmployeeAccountCountHandler(IEmployeeAccountRepository repository) : base(repository)
        {
        }
    }
}
