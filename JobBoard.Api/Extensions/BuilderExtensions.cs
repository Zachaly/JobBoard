using FluentValidation;
using JobBoard.Api.Constants;
using JobBoard.Application.Command;
using JobBoard.Application.Factory;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Application.Validation;
using JobBoard.Database;
using JobBoard.Database.Repository;
using JobBoard.Database.Repository.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

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
            builder.Services.AddScoped<IAdminAccountRepository, AdminAccountRepository>();
            builder.Services.AddScoped<IAdminAccountRefreshTokenRepository, AdminAccountRefreshTokenRepository>();
            builder.Services.AddScoped<ICompanyAccountRefreshTokenRepository, CompanyAccountRefreshTokenRepository>();
            builder.Services.AddScoped<IEmployeeAccountRefreshTokenRepository, EmployeeAccountRefreshTokenRepository>();
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining<AddCompanyAccountCommand>());
            builder.Services.AddValidatorsFromAssemblyContaining<AddCompanyAccountRequestValidator>();

            builder.Services.AddScoped<IHashService, HashService>();
            builder.Services.AddScoped<ICompanyAccountFactory, CompanyAccountFactory>();
            builder.Services.AddScoped<IEmployeeAccountFactory, EmployeeAccountFactory>();
            builder.Services.AddScoped<IAdminAccountFactory, AdminAccountFactory>();
            builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();
            builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
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

        public static void ConfigureAuthorization(this WebApplicationBuilder builder)
        {
            var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Token:SecretKey"]!);
            var key = new SymmetricSecurityKey(bytes);
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidIssuer = builder.Configuration["Token:AuthIssuer"],
                ValidAudience = builder.Configuration["Token:AuthAudience"],
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAlgorithms = [SecurityAlgorithms.HmacSha256Signature],
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
            };

            builder.Services.AddSingleton(validationParameters);
            builder.Services.Configure<TokenConfiguration>(builder.Configuration.GetSection("Token"));

            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.SaveToken = true;
                config.RequireHttpsMetadata = false;
                config.TokenValidationParameters = validationParameters;
                config.MapInboundClaims = false;
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolicyNames.Admin,
                    pol => pol.RequireClaim(AuthClaimNames.RoleClaim, AuthClaimNames.Admin));
                options.AddPolicy(AuthPolicyNames.Company,
                    pol => pol.RequireClaim(AuthClaimNames.RoleClaim, AuthClaimNames.Company, AuthClaimNames.Admin));
                options.AddPolicy(AuthPolicyNames.Employee,
                    pol => pol.RequireClaim(AuthClaimNames.RoleClaim, AuthClaimNames.Employee, AuthClaimNames.Admin));
            });
        }
    }
}
