namespace JobBoard.Application.Service.Abstraction
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(long userId, string role);
        Task<(long Id, string Role)> GetUserIdAndRoleFromToken(string token);
    }
}
