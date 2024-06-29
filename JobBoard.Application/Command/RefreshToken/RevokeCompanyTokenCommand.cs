using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public record RevokeCompanyTokenCommand(string Token) : RevokeRefreshTokenCommand(Token);

    public class RevokeCompanyTokenHandler : RevokeRefreshTokenHandler<CompanyAccountRefreshToken, RevokeCompanyTokenCommand>
    {
        public RevokeCompanyTokenHandler(ICompanyAccountRefreshTokenRepository tokenRepository) : base(tokenRepository)
        {
        }
    }
}
