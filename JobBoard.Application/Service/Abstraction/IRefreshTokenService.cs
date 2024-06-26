namespace JobBoard.Application.Service.Abstraction
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateAdminAccountTokenAsync(long accountId);
        Task<string> GenerateCompanyAccountTokenAsync(long accountId);
        Task<string> GenerateEmployeeAccountTokenAsync(long accountId);
    }
}
