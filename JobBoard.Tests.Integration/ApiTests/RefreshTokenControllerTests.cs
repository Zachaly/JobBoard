using System.Net;
using JobBoard.Application.Command;
using System.Net.Http.Json;
using JobBoard.Model.Response;

namespace JobBoard.Tests.Integration.ApiTests
{
    public class RefreshTokenControllerTests : ApiTest
    {
        const string Endpoint = "api/refresh-token";

        [Fact]
        public async Task RefreshAdminToken_Success_ReturnsNewTokens()
        {
            var loginData = await AuthorizeAdminAsync();

            var request = new RefreshAdminTokenCommand
            {
                AccessToken = loginData.AuthToken,
                RefreshToken = loginData.RefreshToken,
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/admin", request);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(loginData.UserId, content.UserId);
            Assert.NotEqual(loginData.AuthToken, content.AuthToken);
            Assert.NotEqual(loginData.RefreshToken, content.RefreshToken);
            Assert.Contains(_dbContext.AdminAccountRefreshTokens, t => t.IsValid && t.Token == content.RefreshToken);
            Assert.Contains(_dbContext.AdminAccountRefreshTokens, t => !t.IsValid && t.Token == loginData.RefreshToken);
        }

        [Fact]
        public async Task RefreshAdminToken_InvalidRequest_ReturnsBadRequest()
        {
            var loginData = await AuthorizeAdminAsync();

            var request = new RefreshAdminTokenCommand
            {
                AccessToken = loginData.AuthToken,
                RefreshToken = "reftokennotvalid"
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/admin", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshCompanyToken_Success_ReturnsNewTokens()
        {
            var loginData = await AuthorizeCompanyAsync();

            var request = new RefreshCompanyTokenCommand
            {
                AccessToken = loginData.AuthToken,
                RefreshToken = loginData.RefreshToken,
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/company", request);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(loginData.UserId, content.UserId);
            Assert.NotEqual(loginData.AuthToken, content.AuthToken);
            Assert.NotEqual(loginData.RefreshToken, content.RefreshToken);
            Assert.Contains(_dbContext.CompanyAccountRefreshTokens, t => t.IsValid && t.Token == content.RefreshToken);
            Assert.Contains(_dbContext.CompanyAccountRefreshTokens, t => !t.IsValid && t.Token == loginData.RefreshToken);
        }

        [Fact]
        public async Task RefreshCompanyToken_InvalidRequest_ReturnsBadRequest()
        {
            var loginData = await AuthorizeCompanyAsync();

            var request = new RefreshAdminTokenCommand
            {
                AccessToken = loginData.AuthToken,
                RefreshToken = "reftokennotvalid"
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/company", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshEmployeeToken_Success_ReturnsNewTokens()
        {
            var loginData = await AuthorizeEmployeeAsync();

            var request = new RefreshEmployeeTokenCommand
            {
                AccessToken = loginData.AuthToken,
                RefreshToken = loginData.RefreshToken,
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/employee", request);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(loginData.UserId, content.UserId);
            Assert.NotEqual(loginData.AuthToken, content.AuthToken);
            Assert.NotEqual(loginData.RefreshToken, content.RefreshToken);
            Assert.Contains(_dbContext.EmployeeAccountRefreshTokens, t => t.IsValid && t.Token == content.RefreshToken);
            Assert.Contains(_dbContext.EmployeeAccountRefreshTokens, t => !t.IsValid && t.Token == loginData.RefreshToken);
        }

        [Fact]
        public async Task RefreshEmployeeToken_InvalidRequest_ReturnsBadRequest()
        {
            var loginData = await AuthorizeEmployeeAsync();

            var request = new RefreshEmployeeTokenCommand
            {
                AccessToken = loginData.AuthToken,
                RefreshToken = "reftokennotvalid"
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/employee", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RevokeAdminToken_TokenSetInvalid()
        {
            var loginData = await AuthorizeAdminAsync();

            var request = new RevokeRefreshTokenCommand(loginData.RefreshToken);

            var response = await _httpClient.PatchAsJsonAsync($"{Endpoint}/admin/revoke", request);

            var token = _dbContext.AdminAccountRefreshTokens.First(x => x.Token == loginData.RefreshToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.False(token.IsValid);
        }

        [Fact]
        public async Task RevokeCompanyToken_TokenSetInvalid()
        {
            var loginData = await AuthorizeCompanyAsync();

            var request = new RevokeRefreshTokenCommand(loginData.RefreshToken);

            var response = await _httpClient.PatchAsJsonAsync($"{Endpoint}/company/revoke", request);

            var token = _dbContext.CompanyAccountRefreshTokens.First(x => x.Token == loginData.RefreshToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.False(token.IsValid);
        }

        [Fact]
        public async Task RevokeEmployeeToken_TokenSetInvalid()
        {
            var loginData = await AuthorizeEmployeeAsync();

            var request = new RevokeRefreshTokenCommand(loginData.RefreshToken);

            var response = await _httpClient.PatchAsJsonAsync($"{Endpoint}/employee/revoke", request);

            var token = _dbContext.EmployeeAccountRefreshTokens.First(x => x.Token == loginData.RefreshToken);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.False(token.IsValid);
        }
    }
}
