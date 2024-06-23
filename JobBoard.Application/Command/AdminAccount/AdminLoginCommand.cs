using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class AdminLoginCommand : LoginRequest, IRequest<LoginResponse>
    {
    }

    public class AdminLoginHandler : IRequestHandler<AdminLoginCommand, LoginResponse>
    {
        private readonly IAdminAccountRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IHashService _hashService;

        public AdminLoginHandler(IAdminAccountRepository repository, ITokenService tokenService, IHashService hashService)
        {
            _repository = repository;
            _tokenService = tokenService;
            _hashService = hashService;
        }

        public async Task<LoginResponse> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByLoginAsync(request.Login);

            if(account is null)
            {
                return new LoginResponse("Login or password not correct");
            }

            if(!_hashService.VerifyPassword(request.Password, account.PasswordHash))
            {
                return new LoginResponse("Login or password not correct");
            }

            var token = await _tokenService.GenerateTokenAsync(account.Id, "Admin");


            return new LoginResponse(account.Id, token);
        }
    }
}
