using JobBoard.Application.Service.Abstraction;
using JobBoard.Database;
using JobBoard.Domain.Entity;

namespace JobBoard.Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void CreateDefaultAdmin(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (dbContext.AdminAccounts.Any())
                {
                    return;
                }

                var hashService = scope.ServiceProvider.GetRequiredService<IHashService>();

                var account = new AdminAccount
                {
                    Login = app.Configuration["DefaultAdminData:Login"]!,
                    PasswordHash = hashService.HashPassword(app.Configuration["DefaultAdminData:Password"]!)
                };

                dbContext.AdminAccounts.Add(account);
                dbContext.SaveChanges();
            }
        }
    }
}
