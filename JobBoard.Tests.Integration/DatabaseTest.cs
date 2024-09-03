using JobBoard.Database;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Tests.Integration
{
    public class DatabaseTest : IDisposable, IClassFixture<DatabaseContainerFixture>
    {
        protected readonly ApplicationDbContext _dbContext;

        public DatabaseTest(DatabaseContainerFixture fixture)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(fixture.ConnectionString).Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.Migrate();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
