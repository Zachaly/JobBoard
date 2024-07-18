using JobBoard.Domain.Entity;
using JobBoard.Model.Business;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface IBusinessFactory : IEntityFactory<Business, AddBusinessRequest, UpdateBusinessRequest>
    {
    }
}
