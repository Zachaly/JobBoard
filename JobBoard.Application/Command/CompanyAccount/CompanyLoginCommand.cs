using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class CompanyLoginCommand : LoginRequest, IRequest<LoginResponse>
    {
    }

    public class CompanyLoginHandler : IRequestHandler<CompanyLoginCommand, LoginResponse>
    {
        private readonly ICompanyAccountRepository _repository;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;

        public CompanyLoginHandler(ICompanyAccountRepository repository, IHashService hashService, ITokenService tokenService)
        {
            _repository = repository;
            _hashService = hashService;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(CompanyLoginCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByEmailAsync(request.Login);

            if(account is null)
            {
                return new LoginResponse("Email or password not correct");
            }

            if (!_hashService.VerifyPassword(request.Password, account.Password))
            {
                return new LoginResponse("Email or password not correct");
            }

            var token = await _tokenService.GenerateTokenAsync(account.Id, "Company");

            return new LoginResponse(account.Id, token);
        }
    }
}
