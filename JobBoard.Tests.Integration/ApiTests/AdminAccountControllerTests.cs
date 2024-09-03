using JobBoard.Model.AdminAccount;
using System.Net.Http.Json;
using System.Net;
using JobBoard.Model.Response;
using JobBoard.Application.Command;

namespace JobBoard.Tests.Integration.ApiTests
{
    [Collection(Collections.ApiCollection1)]
    public class AdminAccountControllerTests : ApiTest
    {
        const string Endpoint = "api/admin-account";

        public AdminAccountControllerTests(DatabaseContainerFixture fixture) : base(fixture)
        {
            
        }

        [Fact]
        public async Task Get_ReturnAccounts()
        {
            await AuthorizeAdminAsync();

            _dbContext.AdminAccounts.AddRange(FakeDataFactory.CreateAdminAccounts(9));
            _dbContext.SaveChanges();

            var admins = _dbContext.AdminAccounts.ToList();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<AdminAccountModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(admins.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetById_ReturnsAccount()
        {
            await AuthorizeAdminAsync();

            var expected = _dbContext.AdminAccounts.First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{expected.Id}");

            var content = await response.Content.ReadFromJsonAsync<AdminAccountModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected.Login, content.Login);
        }

        [Fact]
        public async Task GetById_AccountNotFount_ReturnsNotFound()
        {
            await AuthorizeAdminAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_ValidRequest_AddsNewAccount()
        {
            await AuthorizeAdminAsync();

            var request = new AddAdminAccountRequest
            {
                Login = "new_admin",
                Password = "zaq1@WSX"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.AdminAccounts, account => account.Login == request.Login);
        }

        [Fact]
        public async Task Post_LoginTaken_ReturnsBadRequest()
        {
            await AuthorizeAdminAsync();

            var request = new AddAdminAccountRequest
            {
                Login = "admin_main",
                Password = "zaq1@WSX"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }

        [Fact]
        public async Task Post_InvalidRequest_ReturnsBadRequest()
        {
            await AuthorizeAdminAsync();

            var request = new AddAdminAccountRequest
            {
                Login = "",
                Password = "zaq1@WSX"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequestAsync<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.ValidationErrors);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Login");
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsTokenAndUserId()
        {
            var request = new AdminLoginCommand
            {
                Login = "admin_main",
                Password = "zaq1@WSX"
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", request);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            var account = _dbContext.AdminAccounts.Where(x => x.Login == request.Login).First();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(content.AuthToken);
            Assert.NotEmpty(content.RefreshToken);
            Assert.Equal(account.Id, content.UserId);
            Assert.Contains(_dbContext.AdminAccountRefreshTokens, t => t.Token == content.RefreshToken);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            var request = new AdminLoginCommand
            {
                Login = "login",
                Password = "pass"
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetCount_ReturnsCorrectCount()
        {
            await AuthorizeAdminAsync();

            _dbContext.AddRange(FakeDataFactory.CreateAdminAccounts(20));
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_dbContext.AdminAccounts.Count(), content);
        }
    }
}
