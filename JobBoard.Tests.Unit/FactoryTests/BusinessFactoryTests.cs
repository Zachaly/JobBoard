using JobBoard.Application.Factory;
using JobBoard.Domain.Entity;
using JobBoard.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Tests.Unit.FactoryTests
{
    public class BusinessFactoryTests
    {
        private readonly BusinessFactory _factory;

        public BusinessFactoryTests()
        {
            _factory = new BusinessFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddBusinessRequest
            {
                Name = "name",
            };

            var business = _factory.Create(request);

            Assert.Equal(request.Name, business.Name);
        }

        [Fact]
        public void Update_UpdatesEntity()
        {
            var business = new Business
            {
                Id = 1,
                Name = "name",
            };

            var request = new UpdateBusinessRequest
            {
                Name = "new_name",
                Id = business.Id
            };

            _factory.Update(business, request);

            Assert.Equal(request.Name, business.Name);
        }
    }
}
