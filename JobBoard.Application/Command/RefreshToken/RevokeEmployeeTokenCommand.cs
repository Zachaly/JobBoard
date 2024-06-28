using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public record RevokeEmployeeTokenCommand(string Token) : RevokeRefreshTokenCommand(Token);

    public class RevokeEmployeeTokenHandler : RevokeRefreshTokenHandler<EmployeeAccountRefreshToken, RevokeEmployeeTokenCommand>
    {
        public RevokeEmployeeTokenHandler(IEmployeeAccountRefreshTokenRepository tokenRepository) : base(tokenRepository)
        {
        }
    }
}
