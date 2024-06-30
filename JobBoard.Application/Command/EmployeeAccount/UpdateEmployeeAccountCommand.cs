using FluentValidation;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.EmployeeAccount;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class UpdateEmployeeAccountCommand : UpdateEmployeeAccountRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateEmployeeAccountHandler : IRequestHandler<UpdateEmployeeAccountCommand, ResponseModel>
    {
        private readonly IEmployeeAccountRepository _repository;
        private readonly IValidator<UpdateEmployeeAccountRequest> _validator;

        public UpdateEmployeeAccountHandler(IEmployeeAccountRepository repository, IValidator<UpdateEmployeeAccountRequest> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(UpdateEmployeeAccountCommand request, CancellationToken cancellationToken)
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

            account.AboutMe = request.AboutMe;
            account.PhoneNumber = request.PhoneNumber;
            account.FirstName = request.FirstName;
            account.LastName = request.LastName;
            account.City = request.City;
            account.Country = request.Country;

            await _repository.UpdateAsync(account);

            return new ResponseModel();
        }
    }
}
