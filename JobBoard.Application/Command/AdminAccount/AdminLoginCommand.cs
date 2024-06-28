using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class AdminLoginCommand : LoginRequest, IRequest<LoginResponse>
    {
    }

    public class AdminLoginHandler : IRequestHandler<AdminLoginCommand, LoginResponse>
    {
        private readonly IAdminAccountRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IHashService _hashService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AdminLoginHandler(IAdminAccountRepository repository, ITokenService tokenService, IHashService hashService,
            IRefreshTokenService refreshTokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
            _hashService = hashService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LoginResponse> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByLoginAsync(request.Login);

            if(account is null)
            {
                return new LoginResponse("Login or password not correct");
            }

            if(!_hashService.VerifyPassword(request.Password, account.PasswordHash))
            {
                return new LoginResponse("Login or password not correct");
            }

            var refreshToken = await _refreshTokenService.GenerateAdminAccountTokenAsync(account.Id);
            var token = await _tokenService.GenerateTokenAsync(account.Id, "Admin");
            
            return new LoginResponse(account.Id, token, refreshToken);
        }
    }
}
