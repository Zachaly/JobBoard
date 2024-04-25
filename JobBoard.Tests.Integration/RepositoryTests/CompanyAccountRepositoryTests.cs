using JobBoard.Database;
using JobBoard.Database.Repository;
using JobBoard.Domain.Entity;
using JobBoard.Model.CompanyAccount;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Tests.Integration.RepositoryTests
{
    public class CompanyAccountRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CompanyAccountRepository _repository;

        public CompanyAccountRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Constants.ConnectionString).Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.Migrate();
            _repository = new CompanyAccountRepository(_dbContext);
        }

        [Fact]
        public async Task AddAsync_AddsNewEntity()
        {
            var entity = new CompanyAccount()
            {
                Name = "name",
                Address = "addr",
                City = "cit",
                ContactEmail = "email",
                Country = "ctn",
                Email = "email",
                Password = "pass",
                PostalCode = "code"
            };

            await _repository.AddAsync(entity);

            Assert.Contains(_dbContext.CompanyAccounts, x => x == entity);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(1, 5)]
        [InlineData(null, 5)]
        [InlineData(1, null)]
        public async Task GetAsync_ReturnsCorrectEntities(int? pageSize, int? index)
        {
            _dbContext.CompanyAccounts.AddRange(FakeDataFactory.CreateCompanyAccounts(20));
            _dbContext.SaveChanges();

            var request = new GetCompanyRequest
            {
                PageIndex = index,
                PageSize = pageSize
            };

            var res = await _repository.GetAsync(request);

            var expectedIds = _dbContext.CompanyAccounts.Skip((index ?? 0) * (pageSize ?? 10)).Take(pageSize ?? 10).Select(x => x.Id);

            Assert.Equivalent(expectedIds, res.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectEntity()
        {
            _dbContext.CompanyAccounts.AddRange(FakeDataFactory.CreateCompanyAccounts(10));
            _dbContext.SaveChanges();

            var expected = _dbContext.CompanyAccounts.OrderBy(x => x.Id).Last();

            var res = await _repository.GetByIdAsync(expected.Id);

            Assert.Equal(expected.Country, res.Country);
            Assert.Equal(expected.Name, res.Name);
            Assert.Equal(expected.PostalCode, res.PostalCode);
            Assert.Equal(expected.City, res.City);
            Assert.Equal(expected.Address, res.Address);
            Assert.Equal(expected.ContactEmail, res.ContactEmail);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsCorrectEntity()
        {
            _dbContext.CompanyAccounts.AddRange(FakeDataFactory.CreateCompanyAccounts(10));
            _dbContext.SaveChanges();

            var expected = _dbContext.CompanyAccounts.OrderBy(x => x.Id).Last();

            var res = await _repository.GetByEmailAsync(expected.Email);

            Assert.Equal(expected.Country, res.Country);
            Assert.Equal(expected.Name, res.Name);
            Assert.Equal(expected.PostalCode, res.PostalCode);
            Assert.Equal(expected.City, res.City);
            Assert.Equal(expected.Address, res.Address);
            Assert.Equal(expected.ContactEmail, res.ContactEmail);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
