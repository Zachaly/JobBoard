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
        private readonly ICompanyAccountRepository _accountRepository;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public CompanyLoginHandler(ICompanyAccountRepository repository, IHashService hashService, ITokenService tokenService,
            IRefreshTokenService refreshTokenService)
        {
            _accountRepository = repository;
            _hashService = hashService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LoginResponse> Handle(CompanyLoginCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByEmailAsync(request.Login);

            if(account is null)
            {
                return new LoginResponse("Email or password not correct");
            }

            if (!_hashService.VerifyPassword(request.Password, account.Password))
            {
                return new LoginResponse("Email or password not correct");
            }


            var refreshToken = await _refreshTokenService.GenerateCompanyAccountTokenAsync(account.Id);
            var token = await _tokenService.GenerateTokenAsync(account.Id, "Company");

            return new LoginResponse(account.Id, token, refreshToken);
        }
    }
}
