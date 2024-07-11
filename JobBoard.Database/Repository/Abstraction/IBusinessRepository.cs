using JobBoard.Domain.Entity;
using JobBoard.Model.Business;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IBusinessRepository : IRepositoryBase<Business, BusinessModel, GetBusinessRequest>
    {
        Task<Business?> GetByNameAsync(string name);
    }
}
