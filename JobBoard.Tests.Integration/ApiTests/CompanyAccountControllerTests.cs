using JobBoard.Application.Command;
using JobBoard.Model.CompanyAccount;
using JobBoard.Model.Response;
using System.Net;
using System.Net.Http.Json;

namespace JobBoard.Tests.Integration.ApiTests
{
    public class CompanyAccountControllerTests : ApiTest
    {
        const string Endpoint = "api/company-account";

        [Fact]
        public async Task Get_ReturnsListOfCompanies()
        {
            _dbContext.AddRange(FakeDataFactory.CreateCompanyAccounts(10));
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<CompanyModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(_dbContext.CompanyAccounts.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetById_ReturnsCorrectCompanyAccount()
        {
            _dbContext.AddRange(FakeDataFactory.CreateCompanyAccounts(10));
            _dbContext.SaveChanges();

            var expected = _dbContext.CompanyAccounts.OrderBy(x => x.Id).Last();

            var response = await _httpClient.GetAsync($"{Endpoint}/{expected.Id}");
            var content = await response.Content.ReadFromJsonAsync<CompanyModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected.Country, content.Country);
            Assert.Equal(expected.Name, content.Name);
            Assert.Equal(expected.PostalCode, content.PostalCode);
            Assert.Equal(expected.City, content.City);
            Assert.Equal(expected.Address, content.Address);
            Assert.Equal(expected.ContactEmail, content.ContactEmail);
        }

        [Fact]
        public async Task GetById_CompanyNotFound_ReturnsNotFound()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_ValidRequest_AddsCompanyAccount()
        {
            var request = new AddCompanyAccountRequest
            {
                Address = "addr",
                Email = "email@email.com",
                City = "city",
                ContactEmail = "email2@email.com",
                Country = "ctn",
                Name = "naame",
                Password = "zaq1@WSX",
                PostalCode = "postal"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.CompanyAccounts, x => x.Address == request.Address);
        }

        [Fact]
        public async Task Post_EmailAlreadyTaken_ReturnsBadRequest()
        {
            var existingAccount = FakeDataFactory.CreateCompanyAccounts(1).First();
            _dbContext.CompanyAccounts.Add(existingAccount);
            _dbContext.SaveChanges();

            var request = new AddCompanyAccountRequest
            {
                Address = "addr",
                Email = existingAccount.Email,
                City = "city",
                ContactEmail = "email2@email.com",
                Country = "ctn",
                Name = "naame",
                Password = "zaq1@WSX",
                PostalCode = "postal"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }

        [Fact]
        public async Task Post_InvalidRequest_ReturnsBadRequest()
        {
            var request = new AddCompanyAccountRequest
            {
                Address = "addr",
                Email = "email",
                City = "city",
                ContactEmail = "email2@email.com",
                Country = "ctn",
                Name = "naame",
                Password = "zaq1@WSX",
                PostalCode = "postal"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.ValidationErrors["Email"]);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsTokenAndUserId()
        {
            var registerRequest = new AddCompanyAccountCommand
            {
                Address = "addr",
                City = "city",
                ContactEmail = "company@company.com",
                Country = "pl",
                Email = "login@email.com",
                Name = "company",
                Password = "zaq1@WSX",
                PostalCode = "12345"
            };

             await _httpClient.PostAsJsonAsync(Endpoint, registerRequest);

            var loginRequest = new CompanyLoginCommand
            {
                Login = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            var account = _dbContext.CompanyAccounts.First();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(account.Id, content.UserId);
            Assert.NotEmpty(content.AuthToken);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            var loginRequest = new CompanyLoginCommand
            {
                Login = "em",
                Password = "pass",
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
