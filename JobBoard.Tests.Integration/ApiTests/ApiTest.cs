using JobBoard.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace JobBoard.Tests.Integration.ApiTests
{
    public class ApiTest : IDisposable
    {
        protected readonly HttpClient _httpClient;
        protected readonly ApplicationDbContext _dbContext;

        public ApiTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Constants.ConnectionString).Options;

            _dbContext = new ApplicationDbContext(options);

            var webFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            ["ConnectionStrings:SqlServer"] = Constants.ConnectionString,
                        });
                    });
                });

            _httpClient = webFactory.CreateClient();
        }

        protected async Task<T?> GetContentFromBadRequest<T>(HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringContent);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
