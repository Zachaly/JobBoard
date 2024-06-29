using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public class CompanyLoginCommand : LoginCommand
    {
    }

    public class CompanyLoginHandler : LoginHandler<CompanyAccount, CompanyLoginCommand>
    {
        private readonly ICompanyAccountRepository _accountRepository;

        public CompanyLoginHandler(ICompanyAccountRepository repository, IHashService hashService, IAccessTokenService tokenService,
            IRefreshTokenService refreshTokenService) : base(tokenService, refreshTokenService, hashService)
        {
            _accountRepository = repository;
            Role = "Company";
        }

        protected override Task<string> GenerateRefreshTokenAsync(long accountId)
            => _refreshTokenService.GenerateCompanyAccountTokenAsync(accountId);

        protected override Task<CompanyAccount?> GetAccountByLoginAsync(string login)
            => _accountRepository.GetByEmailAsync(login);
    }
}
