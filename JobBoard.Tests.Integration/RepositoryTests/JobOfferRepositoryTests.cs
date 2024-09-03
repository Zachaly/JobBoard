using JobBoard.Database.Repository;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOffer;

namespace JobBoard.Tests.Integration.RepositoryTests
{
    public class JobOfferRepositoryTests : DatabaseTest
    {
        private readonly JobOfferRepository _repository;

        public JobOfferRepositoryTests(DatabaseContainerFixture fixture) : base(fixture)
        {
            _repository = new JobOfferRepository(_dbContext);
        }

        [Fact]
        public async Task GetAsync_SearchCompanyName_ReturnsProperEntities()
        {
            const string NameStart = "company";

            var companies = FakeDataFactory.CreateCompanyAccounts(3);

            companies[0].Name = NameStart;
            companies[1].Name = $"{NameStart}2";

            _dbContext.CompanyAccounts.AddRange(companies);
            _dbContext.SaveChanges();

            var offers = companies.SelectMany(c => FakeDataFactory.CreateJobOffers(c.Id, 4)).ToList();

            _dbContext.AddRange(offers);
            _dbContext.SaveChanges();

            var request = new GetJobOfferRequest
            {
                SearchCompanyName = NameStart,
            };

            var res = await _repository.GetAsync(request);
            
            var expectedIds = _dbContext.JobOffers.Where(o => o.Company.Name.StartsWith(NameStart)).Select(x => x.Id).ToList();

            Assert.Equivalent(expectedIds, res.Select(x => x.Id));
        }

        [Fact]
        public async Task GetAsync_TagsSpecified_ReturnsProperEntities()
        {
            var company = FakeDataFactory.CreateCompanyAccounts(1)[0];

            _dbContext.Add(company);
            _dbContext.SaveChanges();

            var offers = FakeDataFactory.CreateJobOffers(company.Id, 5);

            string[] specifiedTags = ["tag1", "tag2", "tag3"];

            offers[0].Tags = [
                new JobOfferTag { Tag = specifiedTags[0] },
                new JobOfferTag { Tag = specifiedTags[1] },
                ];
            offers[1].Tags = [
                new JobOfferTag { Tag = specifiedTags[1] },
                new JobOfferTag { Tag = specifiedTags[2] },
                ];
            offers[3].Tags = [
                new JobOfferTag { Tag = "unspecifiedtag"}
                ];

            _dbContext.AddRange(offers);
            _dbContext.SaveChanges();

            var request = new GetJobOfferRequest
            {
                Tags = specifiedTags
            };

            var res = await _repository.GetAsync(request);

            Assert.Equivalent(offers.Take(2).Select(x => x.Id), res.Select(x => x.Id));
        }
    }
}
