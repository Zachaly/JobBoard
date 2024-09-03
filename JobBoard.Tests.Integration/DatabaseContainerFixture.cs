using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;

namespace JobBoard.Tests.Integration
{
    public class DatabaseContainerFixture : IAsyncLifetime
    {
        private const string Password = "P@ssw0rd";

        private readonly MsSqlContainer _container;

        public string ConnectionString =>
            $"Server={_container.Hostname},{_container.GetMappedPublicPort(1433)};Database=JobBoardTests;User Id=sa;Password={Password};TrustServerCertificate=true;";

        public DatabaseContainerFixture()
        {
            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithExposedPort(1433)
                .WithPortBinding(1433, true)
                .WithPassword(Password)
                .WithCleanUp(true)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("MSSQL_PID", "Express")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();
        }

        public Task DisposeAsync()
        {
            return _container.DisposeAsync().AsTask();
        }

        public Task InitializeAsync()
        {
            return _container.StartAsync();
        }
    }
}
