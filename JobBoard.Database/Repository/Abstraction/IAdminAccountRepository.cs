using JobBoard.Domain.Entity;
using JobBoard.Model.AdminAccount;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IAdminAccountRepository : IRepositoryBase<AdminAccount, AdminAccountModel, GetAdminAccountRequest>
    {
        Task<AdminAccount?> GetByLoginAsync(string login);
    }
}
