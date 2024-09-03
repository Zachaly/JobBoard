using JobBoard.Model.EmployeeResume;
using System.Net;
using System.Net.Http.Json;

namespace JobBoard.Tests.Integration.ApiTests
{
    public class EmployeeResumeControllerTests : ApiTest
    {
        const string Endpoint = "api/employee-resume";

        public EmployeeResumeControllerTests(DatabaseContainerFixture fixture) : base(fixture)
        {
            
        }

        [Fact]
        public async Task Get_ReturnsListOfResumes()
        {
            var employeeData = await AuthorizeEmployeeAsync();

            var resumes = FakeDataFactory.CreateEmployeeResumes(employeeData.UserId, 5);

            _dbContext.AddRange(resumes);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeResumeModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(resumes.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetById_ReturnsCorrectResume()
        {
            var employeeData = await AuthorizeEmployeeAsync();

            var resumes = FakeDataFactory.CreateEmployeeResumes(employeeData.UserId, 5);

            _dbContext.AddRange(resumes);
            _dbContext.SaveChanges();

            var targetEntity = resumes.Last();

            var response = await _httpClient.GetAsync($"{Endpoint}/{targetEntity.Id}");

            var content = await response.Content.ReadFromJsonAsync<EmployeeResumeModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(targetEntity.Name, content.Name);
        }

        [Fact]
        public async Task GetById_EntityNotFound_ReturnsNotFound()
        {
            await AuthorizeEmployeeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
