namespace JobBoard.Tests.Integration.ApiTests
{
    public class StartupTests : ApiTest
    {
        public StartupTests(DatabaseContainerFixture fixture) : base(fixture)
        {
            
        }

        [Fact]
        public void Startup_AdminAccountCreated()
        {
            Assert.NotEmpty(_dbContext.AdminAccounts);
        }
    }
}