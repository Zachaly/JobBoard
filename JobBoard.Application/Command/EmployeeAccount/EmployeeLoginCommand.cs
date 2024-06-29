using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class EmployeeLoginCommand : LoginRequest, IRequest<LoginResponse>
    {
    }

    public class EmployeeLoginHandler : IRequestHandler<EmployeeLoginCommand, LoginResponse>
    {
        private readonly IEmployeeAccountRepository _repository;
        private readonly IHashService _hashService;
        private readonly IAccessTokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public EmployeeLoginHandler(IEmployeeAccountRepository repository, IAccessTokenService tokenService, IHashService hashService,
            IRefreshTokenService refreshTokenService)
        {
            _repository = repository;
            _hashService = hashService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
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

            var refreshToken = await _refreshTokenService.GenerateEmployeeAccountTokenAsync(account.Id);
            var token = await _tokenService.GenerateTokenAsync(account.Id, "Employee");

            return new LoginResponse(account.Id, token, refreshToken);
        }
    }
}
