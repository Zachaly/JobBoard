using JobBoard.Domain.Entity;
using JobBoard.Model.AdminAccount;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IAdminAccountRepository
    {
        Task AddAsync(AdminAccount entity);
        Task<IEnumerable<AdminAccountModel>> GetAsync(GetAdminAccountRequest request);
        Task<AdminAccountModel?> GetByIdAsync(long id);
        Task<AdminAccount?> GetByLoginAsync(string login);
    }
}
