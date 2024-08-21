using JobBoard.Application.Factory;
using JobBoard.Domain.Entity;
using JobBoard.Domain.Enum;
using JobBoard.Model.JobOffer;

namespace JobBoard.Tests.Unit.FactoryTests
{
    public class JobOfferFactoryTests
    {
        private readonly JobOfferFactory _factory;

        public JobOfferFactoryTests()
        {
            _factory = new JobOfferFactory();
        }

        [Fact]
        public void Create_CreatesProperEntity()
        {
            var request = new AddJobOfferRequest
            {
                CompanyId = 1,
                Description = "description",
                ExpirationTimestamp = 1234,
                Location = "krk",
                Title = "title",
                Requirements = ["req1", "req2"],
                BusinessId = 2,
                Tags = ["tag1", "tag2"],
                WorkType = JobOfferWorkType.Hybrid,
                SalaryType = SalaryType.Daily,
                MaxSalary = 100,
                MinSalary = 10
            };

            var offer = _factory.Create(request);

            Assert.Equal(request.CompanyId, offer.CompanyId);
            Assert.Equal(request.Description, offer.Description);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(request.ExpirationTimestamp), offer.ExpirationDate);
            Assert.Equal(request.Title, offer.Title);
            Assert.Equal(request.Location, offer.Location);
            Assert.Equivalent(request.Requirements, offer.Requirements.Select(x => x.Content));
            Assert.Equal(request.BusinessId, offer.BusinessId);
            Assert.Equivalent(request.Tags, offer.Tags.Select(x => x.Tag));
            Assert.Equal(request.WorkType, offer.WorkType);
            Assert.Equal(request.MaxSalary, offer.MaxSalary);
            Assert.Equal(request.MinSalary, offer.MinSalary);
            Assert.Equal(request.SalaryType.GetValueOrDefault(), offer.SalaryType);
        }

        [Fact]
        public void Update_EntityUpdatedProperly()
        {
            var offer = new JobOffer
            {
                Id = 1,
                CompanyId = 1,
                Description = "description",
                CreationDate = DateTimeOffset.Now,
                ExpirationDate = DateTimeOffset.Now.AddDays(1),
                Location = "loc",
                Title = "ttile",
                BusinessId = 2,
                WorkType = JobOfferWorkType.Onsite,
                MinSalary = 1,
                MaxSalary = 2,
                SalaryType = SalaryType.Hourly,
            };

            var request = new UpdateJobOfferRequest
            {
                Description = "new_desc",
                ExpirationTimestamp = DateTimeOffset.UtcNow.AddDays(2).ToUnixTimeMilliseconds(),
                Location = "loc_new",
                Title = "title_new",
                BusinessId = 3,
                WorkType = JobOfferWorkType.Hybrid,
                SalaryType = SalaryType.Daily,
                MaxSalary = 5,
                MinSalary = 4
            };

            _factory.Update(offer, request);

            Assert.Equal(request.Description, offer.Description);
            Assert.Equal(request.Location, offer.Location);
            Assert.Equal(request.ExpirationTimestamp, offer.ExpirationDate.ToUnixTimeMilliseconds());
            Assert.Equal(request.Title, offer.Title);
            Assert.Equal(request.BusinessId, offer.BusinessId);
            Assert.Equal(request.WorkType, offer.WorkType);
            Assert.Equal(request.MaxSalary, offer.MaxSalary);
            Assert.Equal(request.MinSalary, offer.MinSalary);
            Assert.Equal(request.SalaryType.GetValueOrDefault(), offer.SalaryType);
        }
    }
}
