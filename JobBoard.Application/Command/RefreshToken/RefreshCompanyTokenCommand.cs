using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public class RefreshCompanyTokenCommand : RefreshTokenCommand
    {
    }

    public class RefreshCompanyTokenHandler : RefreshTokenHandler<CompanyAccountRefreshToken, RefreshCompanyTokenCommand>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshCompanyTokenHandler(ICompanyAccountRefreshTokenRepository tokenRepository, ITokenService tokenService,
            IRefreshTokenService refreshTokenService) 
            : base(tokenRepository, tokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        protected override Task<string> CreateNewRefreshToken(long accountId)
            => _refreshTokenService.GenerateCompanyAccountTokenAsync(accountId);
    }
}
