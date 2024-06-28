using JobBoard.Application.Command;
using JobBoard.Model.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace JobBoard.Tests.Integration.ApiTests
{
    public class ApiTest : DatabaseTest
    {
        protected readonly HttpClient _httpClient;

        public ApiTest()
        {
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

        protected async Task<T?> GetContentFromBadRequestAsync<T>(HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringContent);
        }

        protected async Task<LoginResponse> AuthorizeAdminAsync()
        {
            var request = new AdminLoginCommand
            {
                Login = "admin_main",
                Password = "zaq1@WSX"
            };

            var response = await _httpClient.PostAsJsonAsync("api/admin-account/login", request);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content.AuthToken);

            return content;
        }

        protected async Task<LoginResponse> AuthorizeEmployeeAsync()
        {
            var registerRequest = new AddEmployeeAccountCommand
            {
                Email = "test@email.com",
                Password = "zaq1@WSX",
                FirstName = "fname",
                LastName = "lname",
                PhoneNumber = "123456789"
            };

            await _httpClient.PostAsJsonAsync("api/employee-account", registerRequest);

            var loginRequest = new EmployeeLoginCommand
            {
                Login = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var response = await _httpClient.PostAsJsonAsync("api/employee-account/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content.AuthToken);

            return content;
        }

        protected async Task<LoginResponse> AuthorizeCompanyAsync()
        {
            var registerRequest = new AddCompanyAccountCommand
            {
                Address = "addr",
                City = "city",
                ContactEmail = "company@company.com",
                Country = "pl",
                Email = "login@email.com",
                Name = "company",
                Password = "zaq1@WSX",
                PostalCode = "12345"
            };

            await _httpClient.PostAsJsonAsync("api/company-account", registerRequest);

            var loginRequest = new CompanyLoginCommand
            {
                Login = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var response = await _httpClient.PostAsJsonAsync("api/company-account/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content.AuthToken);

            return content;
        }
    }
}
