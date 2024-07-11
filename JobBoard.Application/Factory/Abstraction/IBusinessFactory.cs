using JobBoard.Domain.Entity;
using JobBoard.Model.Business;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface IBusinessFactory
    {
        Business Create(AddBusinessRequest request);
        void Update(Business entity, UpdateBusinessRequest request);
    }
}
