using JobBoard.Model.AdminAccount;
using System.Net.Http.Json;
using System.Net;
using JobBoard.Model.Response;

namespace JobBoard.Tests.Integration.ApiTests
{
    public class AdminAccountControllerTests : ApiTest
    {
        const string Endpoint = "api/admin-account";

        [Fact]
        public async Task Get_ReturnAccounts()
        {
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
            var expected = _dbContext.AdminAccounts.First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{expected.Id}");

            var content = await response.Content.ReadFromJsonAsync<AdminAccountModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected.Login, content.Login);
        }

        [Fact]
        public async Task GetById_AccountNotFount_ReturnsNotFound()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_ValidRequest_AddsNewAccount()
        {
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
            var request = new AddAdminAccountRequest
            {
                Login = "admin_main",
                Password = "zaq1@WSX"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }

        [Fact]
        public async Task Post_InvalidRequest_ReturnsBadRequest()
        {
            var request = new AddAdminAccountRequest
            {
                Login = "",
                Password = "zaq1@WSX"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.ValidationErrors);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Login");
        }
    }
}
