using JobBoard.Application.Command;
using JobBoard.Model.EmployeeAccount;
using JobBoard.Model.Response;
using System.Net;
using System.Net.Http.Json;

namespace JobBoard.Tests.Integration.ApiTests
{
    public class EmployeeAccountControllerTests : ApiTest
    {
        const string Endpoint = "/api/employee-account";

        [Fact]
        public async Task Post_ValidRequest_AddsAccount()
        {
            var command = new AddEmployeeAccountCommand
            {
                Email = "email@email.com",
                Password = "zaq1@WSX",
                FirstName = "fname",
                LastName = "lname",
                PhoneNumber = "123456789"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, command);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.EmployeeAccounts, x => x.Email == command.Email);
        }

        [Fact]
        public async Task Post_InvalidRequest_ReturnsBadRequest()
        {
            var command = new AddEmployeeAccountCommand
            {
                Email = "email",
                Password = "zaq1@WSX",
                FirstName = "fname",
                LastName = "lname",
                PhoneNumber = "123456789"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, command);

            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Email");
            Assert.DoesNotContain(_dbContext.EmployeeAccounts, x => x.Email == command.Email);
        }

        [Fact]
        public async Task Post_EmailTaken_ReturnsBadRequest()
        {
            var command = new AddEmployeeAccountCommand
            {
                Email = "email@email.com",
                Password = "zaq1@WSX",
                FirstName = "fname",
                LastName = "lname",
                PhoneNumber = "123456789"
            };

            await _httpClient.PostAsJsonAsync(Endpoint, command);

            var response = await _httpClient.PostAsJsonAsync(Endpoint, command);
            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }

        [Fact]
        public async Task Get_ReturnsEntities()
        {
            _dbContext.EmployeeAccounts.AddRange(FakeDataFactory.CreateEmployeeAccounts(10));
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeAccountModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(_dbContext.EmployeeAccounts.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetById_ReturnsCorrectEntity()
        {
            _dbContext.EmployeeAccounts.AddRange(FakeDataFactory.CreateEmployeeAccounts(10));
            _dbContext.SaveChanges();

            var entity = _dbContext.EmployeeAccounts.OrderBy(x => x.Id).Last();

            var response = await _httpClient.GetAsync($"{Endpoint}/{entity.Id}");

            var content = await response.Content.ReadFromJsonAsync<EmployeeAccountModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(entity.Email, content.Email);
        }

        [Fact]
        public async Task GetById_NoEntityWithSpecifiedId_ReturnsNotFound()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsTokenAndUserId()
        {
            var registerRequest = new AddEmployeeAccountCommand
            {
                Email = "test@email.com",
                Password = "zaq1@WSX",
                FirstName = "fname",
                LastName = "lname",
                PhoneNumber = "123456789"
            };

            await _httpClient.PostAsJsonAsync(Endpoint, registerRequest);

            var loginRequest = new EmployeeLoginCommand
            {
                Login = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var account = _dbContext.EmployeeAccounts.First();

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(account.Id, content.UserId);
            Assert.NotEmpty(content.AuthToken);
            Assert.NotEmpty(content.RefreshToken);
            Assert.Contains(_dbContext.EmployeeAccountRefreshTokens, t => t.Token == content.RefreshToken);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            var loginRequest = new EmployeeLoginCommand
            {
                Login = "em",
                Password = "pass",
            };

            var response = await _httpClient.PostAsJsonAsync("api/employee-account/login", loginRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
