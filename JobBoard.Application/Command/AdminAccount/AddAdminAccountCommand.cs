using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.AdminAccount;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public record AddAdminAccountCommand : AddAdminAccountRequest, IRequest<ResponseModel>
    {
    }

    public class AddAdminAccountHandler : IRequestHandler<AddAdminAccountCommand, ResponseModel>
    {
        private readonly IAdminAccountRepository _repository;
        private readonly IAdminAccountFactory _factory;
        private readonly IHashService _hashService;
        private readonly IValidator<AddAdminAccountRequest> _validator;

        public AddAdminAccountHandler(IAdminAccountRepository repository, IAdminAccountFactory factory, IHashService hashService,
            IValidator<AddAdminAccountRequest> validator)
        {
            _repository = repository;
            _factory = factory;
            _hashService = hashService;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(AddAdminAccountCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var existingAccount = await _repository.GetByLoginAsync(request.Login);

            if(existingAccount is not null)
            {
                return new ResponseModel("Login taken");
            }

            var passwordHash = _hashService.HashPassword(request.Password);

            var account = _factory.Create(request, passwordHash);

            await _repository.AddAsync(account);

            return new ResponseModel();
        }
    }
}
