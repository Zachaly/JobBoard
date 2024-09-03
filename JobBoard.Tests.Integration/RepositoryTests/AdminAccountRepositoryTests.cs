using JobBoard.Database.Repository;
using JobBoard.Model.AdminAccount;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Tests.Integration.RepositoryTests
{
    public class AdminAccountRepositoryTests : DatabaseTest
    {
        private readonly AdminAccountRepository _repository;

        public AdminAccountRepositoryTests(DatabaseContainerFixture fixture) : base(fixture)
        {
            _repository = new AdminAccountRepository(_dbContext);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(1, 5)]
        [InlineData(null, 5)]
        [InlineData(1, null)]
        public async Task GetAsync_ReturnsCorrectEntities(int? pageSize, int? index)
        {
            _dbContext.AdminAccounts.AddRange(FakeDataFactory.CreateAdminAccounts(20));
            _dbContext.SaveChanges();

            var request = new GetAdminAccountRequest
            {
                PageIndex = index,
                PageSize = pageSize
            };

            var res = await _repository.GetAsync(request);

            var expectedIds = _dbContext.CompanyAccounts.Skip((index ?? 0) * (pageSize ?? 10)).Take(pageSize ?? 10).Select(x => x.Id);

            Assert.Equivalent(expectedIds, res.Select(x => x.Id));
        }

        [Fact]
        public async Task AddAsync_AddsEntity()
        {
            var entity = FakeDataFactory.CreateAdminAccounts(1).First();
            
            await _repository.AddAsync(entity);

            Assert.Contains(_dbContext.AdminAccounts, account => account == entity);
        }

        [Fact]
        public async Task GetByLoginAsync_ReturnsCorrectEntity()
        {
            var entities = FakeDataFactory.CreateAdminAccounts(10);

            _dbContext.AdminAccounts.AddRange(entities);
            _dbContext.SaveChanges();

            var expected = entities.Last();

            var res = await _repository.GetByLoginAsync(expected.Login);

            Assert.Equal(expected.Id, res.Id);
            Assert.Equal(expected.PasswordHash, res.PasswordHash);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectEntity()
        {
            var entities = FakeDataFactory.CreateAdminAccounts(10);

            _dbContext.AdminAccounts.AddRange(entities);
            _dbContext.SaveChanges();

            var expected = entities.Last();

            var res = await _repository.GetByIdAsync(expected.Id);

            Assert.Equal(expected.Login, res.Login);
        }
    }
}
