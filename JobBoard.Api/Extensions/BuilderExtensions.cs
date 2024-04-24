using JobBoard.Database;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Api.Extensions
{
    public static class BuilderExtensions
    {
        public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            return builder;
        }
    }
}
