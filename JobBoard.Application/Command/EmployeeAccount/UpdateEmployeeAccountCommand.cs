using FluentValidation;
using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Application.Command
{
    public class UpdateEmployeeAccountCommand : UpdateEmployeeAccountRequest, IUpdateEntityCommand
    {
    }

    public class UpdateEmployeeAccountHandler : UpdateEntityHandler<EmployeeAccount, UpdateEmployeeAccountRequest, UpdateEmployeeAccountCommand>
    {
        public UpdateEmployeeAccountHandler(IEmployeeAccountRepository repository, IEmployeeAccountFactory factory,
            IValidator<UpdateEmployeeAccountRequest> validator)
            : base(repository, factory, validator)
        {
        }
    }
}
