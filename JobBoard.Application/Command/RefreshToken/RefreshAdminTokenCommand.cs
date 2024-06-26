using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public class RefreshAdminTokenCommand : RefreshTokenCommand
    {
    }

    public class RefreshAdminTokenHandler : RefreshTokenHandler<AdminAccountRefreshToken, RefreshAdminTokenCommand>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshAdminTokenHandler(IAdminAccountRefreshTokenRepository tokenRepository, ITokenService tokenService,
            IRefreshTokenService refreshTokenService) 
            : base(tokenRepository, tokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        protected override Task<string> CreateNewRefreshToken(long accountId)
            => _refreshTokenService.GenerateAdminAccountTokenAsync(accountId);
    }
}
