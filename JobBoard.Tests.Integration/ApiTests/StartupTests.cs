namespace JobBoard.Tests.Integration.ApiTests
{
    public class StartupTests : ApiTest
    {
        [Fact]
        public void Startup_AdminAccountCreated()
        {
            Assert.NotEmpty(_dbContext.AdminAccounts);
        }
    }
}
