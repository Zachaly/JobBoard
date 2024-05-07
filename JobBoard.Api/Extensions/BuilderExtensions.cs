using FluentValidation;
using JobBoard.Application.Command;
using JobBoard.Application.Factory;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Application.Validation;
using JobBoard.Database;
using JobBoard.Database.Repository;
using JobBoard.Database.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace JobBoard.Api.Extensions
{
    public static class BuilderExtensions
    {
        public static void AddDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            builder.Services.AddScoped<ICompanyAccountRepository, CompanyAccountRepository>();
            builder.Services.AddScoped<IEmployeeAccountRepository, EmployeeAccountRepository>();
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining<AddCompanyAccountCommand>());
            builder.Services.AddValidatorsFromAssemblyContaining<AddCompanyAccountRequestValidator>();

            builder.Services.AddScoped<IHashService, HashService>();
            builder.Services.AddScoped<ICompanyAccountFactory, CompanyAccountFactory>();
            builder.Services.AddScoped<IEmployeeAccountFactory, EmployeeAccountFactory>();
        }

        public static void ConfigureSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "JobBoard",
                    Description = ""
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }
    }
}
