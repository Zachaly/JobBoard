using JobBoard.Application.Factory;
using JobBoard.Model.JobOfferTag;

namespace JobBoard.Tests.Unit.FactoryTests
{
    public class JobOfferTagFactoryTests
    {
        private readonly JobOfferTagFactory _factory;

        public JobOfferTagFactoryTests()
        {
            _factory = new JobOfferTagFactory();
        }

        [Fact]
        public void Create_CreatesProperEntity()
        {
            var request = new AddJobOfferTagRequest
            {
                Tag = "tag",
                OfferId = 1
            };

            var tag = _factory.Create(request);

            Assert.Equal(request.Tag, tag.Tag);
            Assert.Equal(request.OfferId, tag.OfferId);
        }
    }
}
