using JobBoard.Model.JobOfferRequirement;
using JobBoard.Model.Response;
using System.Net;
using System.Net.Http.Json;

namespace JobBoard.Tests.Integration.ApiTests
{
    [Collection(Collections.ApiCollection4)]
    public class JobOfferRequirementControllerTests : ApiTest
    {
        const string Endpoint = "api/job-offer-requirement";

        public JobOfferRequirementControllerTests(DatabaseContainerFixture fixture) : base(fixture)
        {
            
        }

        [Fact]
        public async Task Get_ReturnsRequirements()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            offer.Requirements = FakeDataFactory.CreateJobOfferRequirements(0, 5);

            _dbContext.JobOffers.Add(offer);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<JobOfferRequirementModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(offer.Requirements.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task Post_AddsNewEntity()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.JobOffers.Add(offer);
            _dbContext.SaveChanges();

            var request = new AddJobOfferRequirementRequest
            {
                Content = "new_req",
                OfferId = offer.Id,
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.JobOfferRequirements, req => req.OfferId == request.OfferId
                && req.Content == request.Content);
        }

        [Fact]
        public async Task Post_InvalidRequest_ReturnsBadRequest() 
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.JobOffers.Add(offer);
            _dbContext.SaveChanges();

            var request = new AddJobOfferRequirementRequest
            {
                OfferId = offer.Id,
                Content = ""
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Content");
        }

        [Fact]
        public async Task Update_UpdatesEntity()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var requirements = FakeDataFactory.CreateJobOfferRequirements(offer.Id, 5);

            _dbContext.AddRange(requirements);
            _dbContext.SaveChanges();

            var updatedEntity = requirements.Last();

            var request = new UpdateJobOfferRequirementRequest
            {
                Content = "new content",
                Id = updatedEntity.Id,
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            _dbContext.Entry(updatedEntity).Reload();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.Content, updatedEntity.Content);
        }

        [Fact]
        public async Task Update_InvalidRequest_ReturnsBadRequest()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var requirements = FakeDataFactory.CreateJobOfferRequirements(offer.Id, 5);

            _dbContext.AddRange(requirements);
            _dbContext.SaveChanges();

            var updatedEntity = requirements.Last();

            var request = new UpdateJobOfferRequirementRequest
            {
                Content = new string('a', 400),
                Id = updatedEntity.Id,
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            _dbContext.Entry(updatedEntity).Reload();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Content");
            Assert.NotEqual(request.Content, updatedEntity.Content);
        }

        [Fact]
        public async Task Update_EntityNotFound_ReturnsBadRequest()
        {
            await AuthorizeCompanyAsync();

            var request = new UpdateJobOfferRequirementRequest
            {
                Content = "new content",
                Id = 2137,
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteById_DeletesProperEntity()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            var requirements = FakeDataFactory.CreateJobOfferRequirements(0, 5);

            offer.Requirements = requirements;

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var deletedEntity = requirements.Last();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{deletedEntity.Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.JobOfferRequirements, req => req.Id == deletedEntity.Id);
        }
    }
}
