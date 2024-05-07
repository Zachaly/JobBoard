using JobBoard.Database;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Tests.Integration
{
    public class DatabaseTest : IDisposable
    {
        protected readonly ApplicationDbContext _dbContext;

        public DatabaseTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Constants.ConnectionString).Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
