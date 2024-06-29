using JobBoard.Application.Exception;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command.Abstraction
{
    public abstract class RefreshTokenCommand : IRequest<LoginResponse>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public abstract class RefreshTokenHandler<TToken, TCommand> : IRequestHandler<TCommand, LoginResponse>
        where TToken : class, IRefreshToken
        where TCommand : RefreshTokenCommand
    {
        private readonly IRefreshTokenRepository<TToken> _tokenRepository;
        private readonly IAccessTokenService _tokenService;

        protected RefreshTokenHandler(IRefreshTokenRepository<TToken> tokenRepository, IAccessTokenService tokenService)
        {
            _tokenRepository = tokenRepository;
            _tokenService = tokenService;
        }

        protected abstract Task<string> CreateNewRefreshToken(long accountId);

        public async Task<LoginResponse> Handle(TCommand request, CancellationToken cancellationToken)
        {
            (long Id, string Role) userData;

            try
            {
                userData = await _tokenService.GetUserIdAndRoleFromToken(request.AccessToken);
            }
            catch (InvalidTokenException ex)
            {
                return new LoginResponse(ex.Message);
            }

            var usedToken = await _tokenRepository.GetValidTokenAsync(request.RefreshToken, userData.Id);

            if (usedToken is null)
            {
                return new LoginResponse("Invalid token");
            }

            usedToken.IsValid = false;
            await _tokenRepository.UpdateTokenAsync(usedToken);

            var newRefreshToken = await CreateNewRefreshToken(userData.Id);
            var newAccessToken = await _tokenService.GenerateTokenAsync(userData.Id, userData.Role);

            return new LoginResponse(userData.Id, newAccessToken, newRefreshToken);
        }
    }
}
