using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public class AdminLoginCommand : LoginCommand
    {
    }

    public class AdminLoginHandler : LoginHandler<AdminAccount, AdminLoginCommand>
    {
        private readonly IAdminAccountRepository _repository;

        public AdminLoginHandler(IAdminAccountRepository repository, IAccessTokenService tokenService, IHashService hashService,
            IRefreshTokenService refreshTokenService) : base(tokenService, refreshTokenService, hashService)
        {
            _repository = repository;
            Role = "Admin";
        }

        protected override Task<string> GenerateRefreshTokenAsync(long accountId)
            => _refreshTokenService.GenerateAdminAccountTokenAsync(accountId);

        protected override Task<AdminAccount?> GetAccountByLoginAsync(string login)
            => _repository.GetByLoginAsync(login);
    }
}
