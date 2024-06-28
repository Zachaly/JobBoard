using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using System.Security.Cryptography;

namespace JobBoard.Application.Service
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IAdminAccountRefreshTokenRepository _adminTokenRepository;
        private readonly ICompanyAccountRefreshTokenRepository _companyTokenRepository;
        private readonly IEmployeeAccountRefreshTokenRepository _employeeTokenRepository;

        public RefreshTokenService(IAdminAccountRefreshTokenRepository adminTokenRepository,
            ICompanyAccountRefreshTokenRepository companyTokenRepository,
            IEmployeeAccountRefreshTokenRepository employeeTokenRepository)
        {
            _adminTokenRepository = adminTokenRepository;
            _companyTokenRepository = companyTokenRepository;
            _employeeTokenRepository = employeeTokenRepository;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<string> GenerateAdminAccountTokenAsync(long accountId)
        {
            var token = new AdminAccountRefreshToken
            {
                AccountId = accountId,
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMonths(1),
                IsValid = true,
                Token = GenerateRefreshToken()
            };

            while(await _adminTokenRepository.CheckIfTokenIsTaken(token.Token))
            {
                token.Token = GenerateRefreshToken();
            }

            await _adminTokenRepository.AddAsync(token);

            return token.Token;
        }

        public async Task<string> GenerateCompanyAccountTokenAsync(long accountId)
        {
            var token = new CompanyAccountRefreshToken
            {
                AccountId = accountId,
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMonths(1),
                IsValid = true,
                Token = GenerateRefreshToken()
            };

            while (await _companyTokenRepository.CheckIfTokenIsTaken(token.Token))
            {
                token.Token = GenerateRefreshToken();
            }

            await _companyTokenRepository.AddAsync(token);

            return token.Token;
        }

        public async Task<string> GenerateEmployeeAccountTokenAsync(long accountId)
        {
            var token = new EmployeeAccountRefreshToken
            {
                AccountId = accountId,
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMonths(1),
                IsValid = true,
                Token = GenerateRefreshToken()
            };

            while (await _employeeTokenRepository.CheckIfTokenIsTaken(token.Token))
            {
                token.Token = GenerateRefreshToken();
            }

            await _employeeTokenRepository.AddAsync(token);

            return token.Token;
        }
    }
}
