using JobBoard.Database.Repository;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Tests.Integration.RepositoryTests
{
    public class EmployeeAccountRepositoryTests : DatabaseTest
    {
        private readonly EmployeeAccountRepository _repository;

        public EmployeeAccountRepositoryTests()
        {
            _repository = new EmployeeAccountRepository(_dbContext);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(1, 5)]
        [InlineData(null, 5)]
        [InlineData(1, null)]
        public async Task GetAsync_ReturnsCorrectEntities(int? pageSize, int? index)
        {
            _dbContext.EmployeeAccounts.AddRange(FakeDataFactory.CreateEmployeeAccounts(20));
            _dbContext.SaveChanges();

            var request = new GetEmployeeAccountRequest
            {
                PageIndex = index,
                PageSize = pageSize
            };

            var res = await _repository.GetAsync(request);

            var expectedIds = _dbContext.CompanyAccounts.Skip((index ?? 0) * (pageSize ?? 10)).Take(pageSize ?? 10).Select(x => x.Id);

            Assert.Equivalent(expectedIds, res.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsCorrectEntity()
        {
            _dbContext.EmployeeAccounts.AddRange(FakeDataFactory.CreateEmployeeAccounts(10));
            _dbContext.SaveChanges();

            var expectedEntity = _dbContext.EmployeeAccounts.OrderBy(x => x.Id).Last();

            var res = await _repository.GetByEmailAsync(expectedEntity.Email);

            Assert.Equal(expectedEntity, res);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectEntity()
        {
            _dbContext.EmployeeAccounts.AddRange(FakeDataFactory.CreateEmployeeAccounts(10));
            _dbContext.SaveChanges();

            var expectedEntity = _dbContext.EmployeeAccounts.OrderBy(x => x.Id).Last();

            var res = await _repository.GetByIdAsync(expectedEntity.Id);

            Assert.Equal(expectedEntity.FirstName, res.FirstName);
            Assert.Equal(expectedEntity.LastName, res.LastName);
            Assert.Equal(expectedEntity.Email, res.Email);
            Assert.Equal(expectedEntity.PhoneNumber, res.PhoneNumber);
        }

        [Fact]
        public async Task AddAsync_AddsEntity()
        {
            var entity = FakeDataFactory.CreateEmployeeAccounts(1).First();

            await _repository.AddAsync(entity);

            Assert.Contains(_dbContext.EmployeeAccounts, x => x == entity);
        }
    }
}
