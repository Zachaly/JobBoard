using JobBoard.Api.Constants;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Api.Infrastructure
{
    public class LoggingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public LoggingPipeline(IHttpContextAccessor httpContextAccessor, ILogger<TRequest> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var accountId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var accountType = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == AuthClaimNames.RoleClaim)?.Value;

            var res = await next();

            var scopeDict = new Dictionary<string, object?>
            {
                { "AccountId", accountId },
                { "AccountType", accountType },
            };

            var action = typeof(TRequest).Name.Replace("Command", "");

            if(res is ResponseModel)
            {
                var responseModel = (res as ResponseModel)!;

                scopeDict.Add("Error", responseModel.Error);

                using var logCtx = _logger.BeginScope(scopeDict);

                if(responseModel.IsSuccess)
                {
                    _logger.LogInformation("{Action}: Success", action);
                }
                else
                {
                    _logger.LogWarning("{Action}: Failure", action);
                }

            }
            else
            {
                using var logCtx = _logger.BeginScope(scopeDict);
                
                _logger.LogInformation(action);
            }

            return res;
        }
    }
}
