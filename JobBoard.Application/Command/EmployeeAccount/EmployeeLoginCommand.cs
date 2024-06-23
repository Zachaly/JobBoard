using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model;
using JobBoard.Model.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Command
{
    public class EmployeeLoginCommand : LoginRequest, IRequest<LoginResponse>
    {
    }

    public class EmployeeLoginHandler : IRequestHandler<EmployeeLoginCommand, LoginResponse>
    {
        private readonly IEmployeeAccountRepository _repository;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;

        public EmployeeLoginHandler(IEmployeeAccountRepository repository, ITokenService tokenService, IHashService hashService)
        {
            _repository = repository;
            _hashService = hashService;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(EmployeeLoginCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByEmailAsync(request.Login);

            if (account is null)
            {
                return new LoginResponse("Email or password not correct");
            }

            if(!_hashService.VerifyPassword(request.Password, account.PasswordHash))
            {
                return new LoginResponse("Email or password not correct");
            }

            var token = await _tokenService.GenerateTokenAsync(account.Id, "Employee");

            return new LoginResponse(account.Id, token);
        }
    }
}
