using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.CompanyAccount;
using JobBoard.Model.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Command
{
    public record AddCompanyAccountCommand : AddCompanyAccountRequest, IRequest<ResponseModel>
    {
    }

    public class AddCompanyAccountHandler : IRequestHandler<AddCompanyAccountCommand, ResponseModel>
    {
        private readonly ICompanyAccountRepository _repository;
        private readonly ICompanyAccountFactory _factory;
        private readonly IValidator<AddCompanyAccountRequest> _validator;
        private readonly IHashService _hashService;

        public AddCompanyAccountHandler(ICompanyAccountRepository repository, ICompanyAccountFactory factory,
            IValidator<AddCompanyAccountRequest> validator, IHashService hashService)
        {
            _repository = repository;
            _factory = factory;
            _validator = validator;
            _hashService = hashService;
        }

        public async Task<ResponseModel> Handle(AddCompanyAccountCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if (!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var existingAccount = await _repository.GetByEmailAsync(request.Email);

            if (existingAccount is not null)
            {
                return new ResponseModel("Email already taken");
            }

            var passwordHash = _hashService.HashPassword(request.Password);

            var account = _factory.Create(request, passwordHash);

            await _repository.AddAsync(account);

            return new ResponseModel();
        }
    }
}
