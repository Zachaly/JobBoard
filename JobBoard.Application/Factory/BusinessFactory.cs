using JobBoard.Application.Factory.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.Business;

namespace JobBoard.Application.Factory
{
    public class BusinessFactory : IBusinessFactory
    {
        public Business Create(AddBusinessRequest request)
            => new Business
            {
                Name = request.Name,
            };

        public void Update(Business entity, UpdateBusinessRequest request)
        {
            entity.Name = request.Name;
        }
    }
}
