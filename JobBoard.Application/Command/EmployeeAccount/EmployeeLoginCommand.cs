using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public class EmployeeLoginCommand : LoginCommand
    {
    }

    public class EmployeeLoginHandler : LoginHandler<EmployeeAccount, EmployeeLoginCommand>
    {
        private readonly IEmployeeAccountRepository _repository;

        public EmployeeLoginHandler(IEmployeeAccountRepository repository, IAccessTokenService tokenService, IHashService hashService,
            IRefreshTokenService refreshTokenService) : base(tokenService, refreshTokenService, hashService)
        {
            _repository = repository;
            Role = "Employee";
        }

        protected override Task<string> GenerateRefreshTokenAsync(long accountId)
            => _refreshTokenService.GenerateEmployeeAccountTokenAsync(accountId);

        protected override Task<EmployeeAccount?> GetAccountByLoginAsync(string login)
            => _repository.GetByEmailAsync(login);
    }
}
