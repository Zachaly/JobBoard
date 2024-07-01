using FluentValidation;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.CompanyAccount;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class UpdateCompanyAccountCommand : UpdateCompanyAccountRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateCompanyAccountHandler : IRequestHandler<UpdateCompanyAccountCommand, ResponseModel>
    {
        private readonly ICompanyAccountRepository _repository;
        private readonly IValidator<UpdateCompanyAccountRequest> _validator;

        public UpdateCompanyAccountHandler(ICompanyAccountRepository repository, IValidator<UpdateCompanyAccountRequest> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(UpdateCompanyAccountCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var account = await _repository.GetEntityByIdAsync(request.Id);

            if(account is null)
            {
                return new ResponseModel("Entity not found");
            }

            account.Address = request.Address;
            account.City = request.City;
            account.PostalCode = request.PostalCode;
            account.Name = request.Name;
            account.ContactEmail = request.ContactEmail;
            account.Country = request.Country;

            await _repository.UpdateAsync(account);

            return new ResponseModel();
        }
    }
}
