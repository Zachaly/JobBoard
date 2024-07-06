using JobBoard.Application.Factory;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferRequirement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Tests.Unit.FactoryTests
{
    public class JobOfferRequirementFactoryTests
    {
        private readonly JobOfferRequirementFactory _factory;

        public JobOfferRequirementFactoryTests()
        {
            _factory = new JobOfferRequirementFactory();
        }

        [Fact]
        public void Create_CreatesProperEntity()
        {
            var request = new AddJobOfferRequirementRequest
            {
                OfferId = 1,
                Content = "con"
            };

            var requirement = _factory.Create(request);

            Assert.Equal(request.Content, requirement.Content);
            Assert.Equal(request.OfferId, requirement.OfferId);
        }

        [Fact]
        public void Updates_UpdatesEntity()
        {
            var entity = new JobOfferRequirement
            {
                Id = 1,
                Content = "content"
            };

            var request = new UpdateJobOfferRequirementRequest
            {
                Content = "new_content"
            };

            _factory.Update(entity, request);

            Assert.Equal(request.Content, entity.Content);
        }
    }
}
