namespace JobBoard.Tests.Integration.ApiTests
{
    public class StartupTests : ApiTest
    {
        [Fact]
        public async Task Startup_AdminAccountCreated()
        {
            Assert.NotEmpty(_dbContext.AdminAccounts);
        }
    }
}
