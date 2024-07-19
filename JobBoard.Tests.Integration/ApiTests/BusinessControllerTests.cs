using JobBoard.Domain.Entity;
using JobBoard.Model.Business;
using JobBoard.Model.Response;
using System.Net;
using System.Net.Http.Json;

namespace JobBoard.Tests.Integration.ApiTests
{
    public class BusinessControllerTests : ApiTest
    {
        const string Endpoint = "api/business";

        [Fact]
        public async Task Get_ReturnsBusinesses()
        {
            var entities = FakeDataFactory.CreateBusinesses(5);

            _dbContext.Businesses.AddRange(entities);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<BusinessModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(entities.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task Post_ValidReqest_AddsBusiness()
        {
            await AuthorizeAdminAsync();

            var request = new AddBusinessRequest
            {
                Name = "newname"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.Businesses, x => x.Name == request.Name);
        }

        [Fact]
        public async Task Post_NameTaken_ReturnsBadRequest()
        {
            await AuthorizeAdminAsync();

            var business = new Business
            {
                Name = "name"
            };

            _dbContext.Businesses.Add(business);
            _dbContext.SaveChanges();

            var request = new AddBusinessRequest
            {
                Name = business.Name
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_InvalidRequest_ReturnsBadRequest()
        {
            await AuthorizeAdminAsync();

            var request = new AddBusinessRequest
            {
                Name = new string('a', 101)
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequestAsync<ResponseModel>(response); 

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Name");
        }

        [Fact]
        public async Task Update_UpdatesBusiness()
        {
            await AuthorizeAdminAsync();

            var business = FakeDataFactory.CreateBusinesses(1)[0];

            _dbContext.Businesses.Add(business);
            _dbContext.SaveChanges();

            var request = new UpdateBusinessRequest
            {
                Id = business.Id,
                Name = "new name"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            _dbContext.Entry(business).Reload();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.Name, business.Name);
        }

        [Fact]
        public async Task Update_EntityNotFound_ReturnsBadRequest()
        {
            await AuthorizeAdminAsync();

            var request = new UpdateBusinessRequest
            {
                Id = 2137,
                Name = "name"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_NameTaken_ReturnsBadRequest()
        {
            await AuthorizeAdminAsync();

            var businesses = FakeDataFactory.CreateBusinesses(2);

            _dbContext.Businesses.AddRange(businesses);
            _dbContext.SaveChanges();

            var request = new UpdateBusinessRequest
            {
                Id = businesses[0].Id,
                Name = businesses[1].Name
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteById_DeletesCorrectEntity_SetsNullForJobOffers()
        {
            await AuthorizeAdminAsync();

            var businesses = FakeDataFactory.CreateBusinesses(5);

            _dbContext.Businesses.AddRange(businesses);
            _dbContext.SaveChanges();

            var deletedEntity = businesses.Last();

            var company = FakeDataFactory.CreateCompanyAccounts(1)[0];

            _dbContext.CompanyAccounts.Add(company);
            _dbContext.SaveChanges();

            var jobOffers = FakeDataFactory.CreateJobOffers(company.Id, 4);

            jobOffers.ForEach(o => o.BusinessId = deletedEntity.Id);

            _dbContext.JobOffers.AddRange(jobOffers);
            _dbContext.SaveChanges();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{deletedEntity.Id}");

            jobOffers.ForEach(o => _dbContext.Entry(o).Reload());

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.Businesses, e => e.Id == deletedEntity.Id);
            Assert.Equal(_dbContext.JobOffers.Count(), jobOffers.Count);
            Assert.All(jobOffers, o => Assert.Null(o.BusinessId));
        }

        [Fact]
        public async Task GetCount_ReturnsCorrectCount()
        {
            _dbContext.Businesses.AddRange(FakeDataFactory.CreateBusinesses(20));
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_dbContext.Businesses.Count(), content);
        }
    }
}
