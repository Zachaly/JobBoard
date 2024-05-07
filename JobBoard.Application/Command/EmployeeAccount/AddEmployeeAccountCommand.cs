using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.EmployeeAccount;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public record AddEmployeeAccountCommand : AddEmployeeAccountRequest, IRequest<ResponseModel>
    {
    }

    public class AddEmployeeAccountHandler : IRequestHandler<AddEmployeeAccountCommand, ResponseModel>
    {
        private readonly IEmployeeAccountFactory _factory;
        private readonly IEmployeeAccountRepository _repository;
        private readonly IValidator<AddEmployeeAccountRequest> _validator;
        private readonly IHashService _hashService;

        public AddEmployeeAccountHandler(IEmployeeAccountFactory factory, IEmployeeAccountRepository repository,
            IValidator<AddEmployeeAccountRequest> validator, IHashService hashService)
        {
            _factory = factory;
            _repository = repository;
            _validator = validator;
            _hashService = hashService;
        }

        public async Task<ResponseModel> Handle(AddEmployeeAccountCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var existingAccount = await _repository.GetByEmailAsync(request.Email);

            if(existingAccount is not null)
            {
                return new ResponseModel("Email already taken!");
            }

            var account = _factory.Create(request, _hashService.HashPassword(request.Password));

            await _repository.AddAsync(account);

            return new ResponseModel();
        }
    }
}
