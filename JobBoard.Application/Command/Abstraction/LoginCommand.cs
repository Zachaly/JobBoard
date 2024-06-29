using JobBoard.Application.Service.Abstraction;
using JobBoard.Domain;
using JobBoard.Model;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command.Abstraction
{
    public class LoginCommand : LoginRequest, IRequest<LoginResponse>
    {

    }

    public abstract class LoginHandler<TAccount, TCommand> : IRequestHandler<TCommand, LoginResponse>
        where TAccount : class, IAccountEntity
        where TCommand : LoginCommand
    {
        private readonly IAccessTokenService _accessTokenService;
        protected readonly IRefreshTokenService _refreshTokenService;
        private readonly IHashService _hashService;
        protected string Role { get; set; }

        protected LoginHandler(IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService,
            IHashService hashService)
        {
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _hashService = hashService;
        }

        public async Task<LoginResponse> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var account = await GetAccountByLoginAsync(request.Login);

            if (account is null)
            {
                return new LoginResponse("Login or password not correct");
            }

            if (!_hashService.VerifyPassword(request.Password, account.PasswordHash))
            {
                return new LoginResponse("Login or password not correct");
            }

            var refreshToken = await GenerateRefreshTokenAsync(account.Id);
            var token = await _accessTokenService.GenerateTokenAsync(account.Id, Role);

            return new LoginResponse(account.Id, token, refreshToken);
        }

        protected abstract Task<TAccount?> GetAccountByLoginAsync(string login);
        protected abstract Task<string> GenerateRefreshTokenAsync(long accountId);
    }
}
