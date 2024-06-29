using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public class RefreshEmployeeTokenCommand : RefreshTokenCommand
    {
    }

    public class RefreshEmployeeTokenHandler : RefreshTokenHandler<EmployeeAccountRefreshToken, RefreshEmployeeTokenCommand>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshEmployeeTokenHandler(IEmployeeAccountRefreshTokenRepository tokenRepository, IAccessTokenService tokenService,
            IRefreshTokenService refreshTokenService)
            : base(tokenRepository, tokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        protected override Task<string> CreateNewRefreshToken(long accountId)
            => _refreshTokenService.GenerateEmployeeAccountTokenAsync(accountId);
    }
}
