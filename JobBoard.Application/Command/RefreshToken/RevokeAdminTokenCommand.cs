using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public record RevokeAdminTokenCommand(string Token) : RevokeRefreshTokenCommand(Token);

    public class RevokeAdminTokenHandler : RevokeRefreshTokenHandler<AdminAccountRefreshToken, RevokeAdminTokenCommand>
    {
        public RevokeAdminTokenHandler(IAdminAccountRefreshTokenRepository tokenRepository) : base(tokenRepository)
        {
        }
    }
}
