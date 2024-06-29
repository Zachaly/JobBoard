namespace JobBoard.Application.Service.Abstraction
{
    public interface IAccessTokenService
    {
        Task<string> GenerateTokenAsync(long userId, string role);
        Task<(long Id, string Role)> GetUserIdAndRoleFromTokenAsync(string token);
    }
}
