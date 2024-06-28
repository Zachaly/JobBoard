using JobBoard.Domain.Entity;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IRefreshTokenRepository<TToken> where TToken : IRefreshToken
    {
        Task<bool> CheckIfTokenIsTaken(string token);
        Task<TToken?> GetValidTokenAsync(string token, long accountId);
        Task AddAsync(TToken token);
        Task UpdateTokenAsync(TToken token);
        Task<TToken?> GetByTokenAsync(string token);
    }

    public interface IAdminAccountRefreshTokenRepository : IRefreshTokenRepository<AdminAccountRefreshToken>
    {

    }

    public interface ICompanyAccountRefreshTokenRepository : IRefreshTokenRepository<CompanyAccountRefreshToken> 
    {

    }

    public interface IEmployeeAccountRefreshTokenRepository : IRefreshTokenRepository<EmployeeAccountRefreshToken> 
    {

    }
}
