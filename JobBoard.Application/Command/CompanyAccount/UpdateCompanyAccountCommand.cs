using FluentValidation;
using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Application.Command
{
    public class UpdateCompanyAccountCommand : UpdateCompanyAccountRequest, IUpdateEntityCommand
    {
    }

    public class UpdateCompanyAccountHandler : UpdateEntityHandler<CompanyAccount, UpdateCompanyAccountRequest, UpdateCompanyAccountCommand>
    {
        public UpdateCompanyAccountHandler(ICompanyAccountRepository repository, ICompanyAccountFactory factory,
            IValidator<UpdateCompanyAccountRequest> validator) : base(repository, factory, validator)
        {
        }
    }
}
