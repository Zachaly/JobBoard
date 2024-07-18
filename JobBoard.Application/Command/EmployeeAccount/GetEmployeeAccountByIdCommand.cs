using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Application.Command
{
    public record GetEmployeeAccountByIdCommand(long Id) : GetByIdCommand<EmployeeAccountModel>(Id)
    {
    }

    public class GetEmployeeAccountByIdHandler : GetByIdHandler<EmployeeAccountModel, GetEmployeeAccountRequest, GetEmployeeAccountByIdCommand>
    {
        public GetEmployeeAccountByIdHandler(IEmployeeAccountRepository repository) : base(repository)
        {
        }
    }
}
