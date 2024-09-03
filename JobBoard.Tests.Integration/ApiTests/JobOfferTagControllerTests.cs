using JobBoard.Model.JobOfferTag;
using JobBoard.Model.Response;
using System.Net;
using System.Net.Http.Json;

namespace JobBoard.Tests.Integration.ApiTests
{
    [Collection(Collections.ApiCollection5)]
    public class JobOfferTagControllerTests : ApiTest
    {
        const string Endpoint = "api/job-offer-tag";

        public JobOfferTagControllerTests(DatabaseContainerFixture fixture) : base(fixture)
        {
            
        }

        [Fact]
        public async Task Get_ReturnsTags()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var tags = FakeDataFactory.CreateJobOfferTags(offer.Id, 5);

            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<JobOfferTagModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(tags.Select(x => x.Id), content.Select(x => x.Id));  
        }

        [Fact]
        public async Task GetCount_ReturnsProperCount()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var tags = FakeDataFactory.CreateJobOfferTags(offer.Id, 5);

            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(tags.Count, content);
        }

        [Fact]
        public async Task Post_AddsNewEntity()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var request = new AddJobOfferTagRequest
            {
                OfferId = offer.Id,
                Tag = "tag"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.JobOfferTags, t => t.Tag == request.Tag && t.OfferId == request.OfferId);
        }

        [Fact]
        public async Task Post_InvalidRequest_ReturnsBadRequest()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var request = new AddJobOfferTagRequest
            {
                OfferId = offer.Id,
                Tag = ""
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Tag");
        }

        [Fact]
        public async Task DeleteById_DeletesProperEntity()
        {
            var companyData = await AuthorizeCompanyAsync();

            var offer = FakeDataFactory.CreateJobOffers(companyData.UserId, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var tags = FakeDataFactory.CreateJobOfferTags(offer.Id, 5);

            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();

            var deletedEntity = tags.Last();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{deletedEntity.Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.JobOfferTags, t => t.Id == deletedEntity.Id);
        }
    }
}
