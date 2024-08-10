using JobBoard.Domain.Enum;
using JobBoard.Model.JobOfferApplication;
using JobBoard.Model.Response;
using System.Net;
using System.Net.Http.Json;

namespace JobBoard.Tests.Integration.ApiTests
{
    public class JobOfferApplicationControllerTests : ApiTest
    {
        const string Endpoint = "api/job-offer-application";

        [Fact]
        public async Task Get_ReturnsListOfApplications()
        {
            await AuthorizeEmployeeAsync();

            var employees = FakeDataFactory.CreateEmployeeAccounts(5);

            _dbContext.AddRange(employees);
            _dbContext.SaveChanges();

            var company = FakeDataFactory.CreateCompanyAccounts(1)[0];
            _dbContext.Add(company);
            _dbContext.SaveChanges();

            var offer = FakeDataFactory.CreateJobOffers(company.Id, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var applications = FakeDataFactory.CreateJobOfferApplications(offer.Id, employees.Select(x => x.Id));

            _dbContext.AddRange(applications);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<JobOfferApplicationModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(applications.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetById_ReturnsCorrectEntity()
        {
            await AuthorizeEmployeeAsync();

            var employees = FakeDataFactory.CreateEmployeeAccounts(5);

            _dbContext.AddRange(employees);
            _dbContext.SaveChanges();

            var company = FakeDataFactory.CreateCompanyAccounts(1)[0];
            _dbContext.Add(company);
            _dbContext.SaveChanges();

            var offer = FakeDataFactory.CreateJobOffers(company.Id, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var applications = FakeDataFactory.CreateJobOfferApplications(offer.Id, employees.Select(x => x.Id));

            _dbContext.AddRange(applications);
            _dbContext.SaveChanges();

            var requestedEntity = applications.Last();

            var response = await _httpClient.GetAsync($"{Endpoint}/{requestedEntity.Id}");

            var content = await response.Content.ReadFromJsonAsync<JobOfferApplicationModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(requestedEntity.Id, content.Id);
            Assert.Equal(requestedEntity.State, content.State);
            Assert.Equal(requestedEntity.ApplicationDate, content.ApplicationDate);
            Assert.Equal(requestedEntity.EmployeeId, content.Employee.Id);
            Assert.Equal(requestedEntity.OfferId, content.OfferId);
        }

        [Fact]
        public async Task GetById_EntityNotFound_ReturnsNotFound()
        {
            await AuthorizeEmployeeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCount_ReturnsProperCount()
        {
            await AuthorizeEmployeeAsync();

            var employees = FakeDataFactory.CreateEmployeeAccounts(5);

            _dbContext.AddRange(employees);
            _dbContext.SaveChanges();

            var company = FakeDataFactory.CreateCompanyAccounts(1)[0];
            _dbContext.Add(company);
            _dbContext.SaveChanges();

            var offer = FakeDataFactory.CreateJobOffers(company.Id, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var applications = FakeDataFactory.CreateJobOfferApplications(offer.Id, employees.Select(x => x.Id));

            _dbContext.AddRange(applications);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync($"{Endpoint}/count");

            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_dbContext.Applications.Count(), content);
        }

        [Fact]
        public async Task Update_UpdatesCorrectEntity()
        {
            await AuthorizeEmployeeAsync();

            var employees = FakeDataFactory.CreateEmployeeAccounts(5);

            _dbContext.AddRange(employees);
            _dbContext.SaveChanges();

            var company = FakeDataFactory.CreateCompanyAccounts(1)[0];
            _dbContext.Add(company);
            _dbContext.SaveChanges();

            var offer = FakeDataFactory.CreateJobOffers(company.Id, 1)[0];

            _dbContext.Add(offer);
            _dbContext.SaveChanges();

            var applications = FakeDataFactory.CreateJobOfferApplications(offer.Id, employees.Select(x => x.Id));

            _dbContext.AddRange(applications);
            _dbContext.SaveChanges();

            var updatedEntity = applications.Last();

            var request = new UpdateJobOfferApplicationRequest
            {
                Id = updatedEntity.Id,
                State = JobOfferApplicationState.Accepted
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            _dbContext.Entry(updatedEntity).Reload();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.State, updatedEntity.State);
        }

        [Fact]
        public async Task Update_EntityNotFound_ReturnsBadRequest()
        {
            await AuthorizeCompanyAsync();

            var request = new UpdateJobOfferApplicationRequest
            {
                Id = 2137,
                State = JobOfferApplicationState.Sent
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }
    }
}
