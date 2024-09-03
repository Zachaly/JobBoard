using JobBoard.Model.JobOffer;
using JobBoard.Model.Response;
using System.Net;
using System.Net.Http.Json;

namespace JobBoard.Tests.Integration.ApiTests
{
    [Collection(Collections.ApiCollection4)]
    public class JobOfferControllerTests : ApiTest
    {
        const string Endpoint = "api/job-offer";

        public JobOfferControllerTests(DatabaseContainerFixture fixture) : base(fixture)
        {
            
        }

        [Fact]
        public async Task Get_ReturnsOffers()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offers = FakeDataFactory.CreateJobOffers(companyData.UserId, 5);

            _dbContext.JobOffers.AddRange(offers);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<JobOfferModel>>();
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(offers.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetById_ReturnsCorrectJobOffer()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offers = FakeDataFactory.CreateJobOffers(companyData.UserId, 5);

            _dbContext.JobOffers.AddRange(offers);
            _dbContext.SaveChanges();

            var expected = offers.Last();

            var response = await _httpClient.GetAsync($"{Endpoint}/{expected.Id}");
            var content = await response.Content.ReadFromJsonAsync<JobOfferModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected.Title, content.Title);
            Assert.Equal(expected.Location, content.Location);
            Assert.Equal(expected.Description, content.Description);
            Assert.Equal(expected.ExpirationDate, content.ExpirationDate);
            Assert.Equal(expected.CreationDate, content.CreationDate);
            Assert.Equal(expected.CompanyId, content.Company.Id);
            Assert.Equal(expected.WorkType, content.WorkType);
            Assert.Equal(expected.SalaryType, content.SalaryType);
            Assert.Equal(expected.MinSalary, content.MinSalary);
            Assert.Equal(expected.MaxSalary, content.MaxSalary);
        }

        [Fact]
        public async Task GetById_NonexistentId_ReturnsNotFound()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_AddsNewJobOffer()
        {
            var companyData = await AuthorizeCompanyAsync();

            var request = new AddJobOfferRequest
            {
                CompanyId = companyData.UserId,
                Description = "desc",
                ExpirationTimestamp = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeMilliseconds(),
                Location = "loc",
                Title = "title",
                Requirements = ["req1", "req2"],
                Tags = [],
                WorkType = Domain.Enum.JobOfferWorkType.Remote,
                SalaryType = Domain.Enum.SalaryType.Monthly,
                MinSalary = 5000,
                MaxSalary = 6000,
                ExperienceLevel = Domain.Enum.WorkExperienceLevel.Mid
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.JobOffers, offer => offer.CompanyId == request.CompanyId
                 && offer.Description == request.Description
                 && offer.Title == request.Title
                 && offer.ExpirationDate == DateTimeOffset.FromUnixTimeMilliseconds(request.ExpirationTimestamp)
                 && offer.Location == request.Location
                 && offer.WorkType == request.WorkType
                 && offer.SalaryType == request.SalaryType
                 && offer.MaxSalary == request.MaxSalary
                 && offer.MinSalary == request.MinSalary
                 && offer.ExperienceLevel == request.ExperienceLevel);
            Assert.Contains(_dbContext.JobOfferRequirements, x => x.Content == request.Requirements.ElementAt(0));
            Assert.Contains(_dbContext.JobOfferRequirements, x => x.Content == request.Requirements.ElementAt(1));
        }

        [Fact]
        public async Task Post_InvalidRequest_ReturnsBadRequest()
        {
            var companyData = await AuthorizeCompanyAsync();

            var request = new AddJobOfferRequest
            {
                Title = "",
                CompanyId = companyData.UserId,
                Description = "desc",
                ExpirationTimestamp = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeMilliseconds(),
                Location = "loc",
                Requirements = [],
                Tags = [],
                WorkType = Domain.Enum.JobOfferWorkType.Remote,
                SalaryType = Domain.Enum.SalaryType.Hourly,
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Title");
        }

        [Fact]
        public async Task Update_UpdatesJobOffer()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.JobOffers.Add(offer);
            _dbContext.SaveChanges();

            var request = new UpdateJobOfferRequest
            {
                Id = offer.Id,
                Description = "new_desc",
                ExpirationTimestamp = offer.ExpirationDate.ToUnixTimeMilliseconds(),
                Location = "new_loc",
                Title = "title",
                WorkType = Domain.Enum.JobOfferWorkType.Remote,
                SalaryType = Domain.Enum.SalaryType.Yearly,
                MinSalary = 1000,
                MaxSalary = 2000,
                ExperienceLevel = Domain.Enum.WorkExperienceLevel.Mid,
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            _dbContext.Entry(offer).Reload();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.Description, offer.Description);
            Assert.Equal(request.ExpirationTimestamp, offer.ExpirationDate.ToUnixTimeMilliseconds());
            Assert.Equal(request.Location, offer.Location);
            Assert.Equal(request.Title, offer.Title);
            Assert.Equal(request.WorkType, offer.WorkType);
            Assert.Equal(request.SalaryType, offer.SalaryType);
            Assert.Equal(request.MinSalary, offer.MinSalary);
            Assert.Equal(request.MaxSalary, offer.MaxSalary);
            Assert.Equal(request.SalaryType, offer.SalaryType);
            Assert.Equal(request.ExperienceLevel, offer.ExperienceLevel);
        }

        [Fact]
        public async Task Update_InvalidRequest_ReturnsBadRequest()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.JobOffers.Add(offer);
            _dbContext.SaveChanges();

            var request = new UpdateJobOfferRequest
            {
                Id = offer.Id,
                Description = "new_desc",
                ExpirationTimestamp = offer.ExpirationDate.ToUnixTimeMilliseconds(),
                Location = "new_loc",
                Title = "",
                WorkType = Domain.Enum.JobOfferWorkType.Hybrid,
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Title");
            Assert.NotEqual(request.Description, offer.Description);
            Assert.NotEqual(request.Location, offer.Location);
            Assert.NotEqual(request.Title, offer.Title);
            Assert.NotEqual(request.WorkType, offer.WorkType);
        }

        [Fact]
        public async Task DeleteById_DeletesSpecifiedEntityAndChildren()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offers = FakeDataFactory.CreateJobOffers(companyData.UserId, 10);

            _dbContext.JobOffers.AddRange(offers);
            _dbContext.SaveChanges();

            var deletedEntity = offers.Last();

            _dbContext.AddRange(FakeDataFactory.CreateJobOfferTags(deletedEntity.Id, 5));
            _dbContext.AddRange(FakeDataFactory.CreateJobOfferRequirements(deletedEntity.Id, 5));

            _dbContext.SaveChanges();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{deletedEntity.Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.JobOffers, x => x.Id == deletedEntity.Id);
            Assert.DoesNotContain(_dbContext.JobOfferRequirements, req => req.OfferId == deletedEntity.Id);
            Assert.DoesNotContain(_dbContext.JobOfferTags, tag => tag.OfferId == deletedEntity.Id);
        }

        [Fact]
        public async Task GetCount_ReturnsProperCount()
        {
            var company = FakeDataFactory.CreateCompanyAccounts(1)[0];

            _dbContext.CompanyAccounts.Add(company);
            _dbContext.SaveChanges();

            _dbContext.AddRange(FakeDataFactory.CreateJobOffers(company.Id, 20));
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_dbContext.JobOffers.Count(), content);
        }
    }
}
