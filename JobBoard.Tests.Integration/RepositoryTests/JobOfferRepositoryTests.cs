using JobBoard.Database.Repository;
using JobBoard.Model.JobOffer;

namespace JobBoard.Tests.Integration.RepositoryTests
{
    public class JobOfferRepositoryTests : DatabaseTest
    {
        private readonly JobOfferRepository _repository;

        public JobOfferRepositoryTests() : base()
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
    }
}
