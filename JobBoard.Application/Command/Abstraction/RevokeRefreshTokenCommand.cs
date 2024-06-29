using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command.Abstraction
{
    public record RevokeRefreshTokenCommand(string Token) : IRequest<ResponseModel>;

    public class RevokeRefreshTokenHandler<TToken, TCommand> : IRequestHandler<TCommand, ResponseModel>
        where TToken : class, IRefreshToken
        where TCommand : RevokeRefreshTokenCommand
    {
        private readonly IRefreshTokenRepository<TToken> _tokenRepository;

        public RevokeRefreshTokenHandler(IRefreshTokenRepository<TToken> tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<ResponseModel> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var token = await _tokenRepository.GetByTokenAsync(request.Token);

            if (token is null)
            {
                return new ResponseModel();
            }

            token.IsValid = false;
            await _tokenRepository.UpdateTokenAsync(token);

            return new ResponseModel();
        }
    }
}
