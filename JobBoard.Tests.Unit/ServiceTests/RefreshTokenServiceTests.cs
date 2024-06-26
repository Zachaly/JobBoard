using JobBoard.Application.Service;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using NSubstitute;

namespace JobBoard.Tests.Unit.ServiceTests
{
    public class RefreshTokenServiceTests
    {
        private readonly IAdminAccountRefreshTokenRepository _adminTokenRepository;
        private readonly ICompanyAccountRefreshTokenRepository _companyTokenRepository;
        private readonly IEmployeeAccountRefreshTokenRepository _employeeTokenRepository;
        private readonly RefreshTokenService _service;

        public RefreshTokenServiceTests()
        {
            _adminTokenRepository = Substitute.For<IAdminAccountRefreshTokenRepository>();
            _companyTokenRepository = Substitute.For<ICompanyAccountRefreshTokenRepository>();
            _employeeTokenRepository = Substitute.For<IEmployeeAccountRefreshTokenRepository>();
            _service = new RefreshTokenService(_adminTokenRepository, _companyTokenRepository, _employeeTokenRepository);
        }

        [Fact]
        public async Task GenerateAdminAccountTokenAsync_ReturnsToken()
        {
            const long AccountId = 1;

            AdminAccountRefreshToken token = null;

            _adminTokenRepository.AddAsync(Arg.Any<AdminAccountRefreshToken>()).Returns(Task.CompletedTask)
                .AndDoes(info => token = info.Arg<AdminAccountRefreshToken>());

            _adminTokenRepository.CheckIfTokenIsTaken(Arg.Any<string>()).Returns(false);

            var res = await _service.GenerateAdminAccountTokenAsync(AccountId);

            Assert.Equal(token!.Token, res);
            Assert.Equal(AccountId, token.AccountId);
        }

        [Fact]
        public async Task GenerateCompanyAccountTokenAsync_ReturnsToken()
        {
            const long AccountId = 1;

            CompanyAccountRefreshToken token = null;

            _companyTokenRepository.AddAsync(Arg.Any<CompanyAccountRefreshToken>()).Returns(Task.CompletedTask)
                .AndDoes(info => token = info.Arg<CompanyAccountRefreshToken>());

            _companyTokenRepository.CheckIfTokenIsTaken(Arg.Any<string>()).Returns(false);

            var res = await _service.GenerateCompanyAccountTokenAsync(AccountId);

            Assert.Equal(token!.Token, res);
            Assert.Equal(AccountId, token.AccountId);
        }

        [Fact]
        public async Task GenerateEmployeeAccountTokenAsync_ReturnsToken()
        {
            const long AccountId = 1;

            EmployeeAccountRefreshToken token = null;

            _employeeTokenRepository.AddAsync(Arg.Any<EmployeeAccountRefreshToken>()).Returns(Task.CompletedTask)
                .AndDoes(info => token = info.Arg<EmployeeAccountRefreshToken>());

            _employeeTokenRepository.CheckIfTokenIsTaken(Arg.Any<string>()).Returns(false);

            var res = await _service.GenerateEmployeeAccountTokenAsync(AccountId);

            Assert.Equal(token!.Token, res);
            Assert.Equal(AccountId, token.AccountId);
        }
    }
}
