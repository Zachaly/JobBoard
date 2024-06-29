using JobBoard.Application.Exception;
using JobBoard.Application.Service.Abstraction;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JobBoard.Application.Service
{
    public class TokenConfiguration 
    {
        public string SecretKey { get; init; }
        public string AuthIssuer { get; init; }
        public string AuthAudience { get; init; }
        public int TokenLifetime { get; init; }
    }

    public class AccessTokenService : IAccessTokenService
    {
        private readonly TokenConfiguration _configuration;
        private readonly TokenValidationParameters _validationParameters;

        public AccessTokenService(IOptions<TokenConfiguration> config, TokenValidationParameters validationParameters)
        {
            _configuration = config.Value;
            _validationParameters = validationParameters;
        }

        public Task<string> GenerateTokenAsync(long userId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim("sub", userId.ToString()),
                new Claim("jti", Guid.NewGuid().ToString()),
                new Claim("Role", role)
            };

            var handler = new JsonWebTokenHandler
            {
                MapInboundClaims = false
            };

            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Audience = _configuration.AuthAudience,
                Issuer = _configuration.AuthIssuer,
                Expires = DateTime.Now.AddSeconds(_configuration.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
            });

            return Task.FromResult(token);
        }

        public Task<(long Id, string Role)> GetUserIdAndRoleFromToken(string token)
        {
            var parameters = _validationParameters.Clone();

            parameters.ValidateLifetime = false;

            var tokenHandler = new JwtSecurityTokenHandler
            {
                MapInboundClaims = false
            };

            ClaimsPrincipal claimsPrincipal;
            SecurityToken securityToken;

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, parameters, out securityToken);
            }
            catch (System.Exception)
            {
                throw new InvalidTokenException("Token is not valid jwt");
            }

            if (securityToken is not JwtSecurityToken)
            {
                throw new InvalidTokenException("Token is not valid jwt");
            }

            var idClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "sub");
            var roleClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "Role");

            if (idClaim is null || roleClaim is null)
            {
                throw new InvalidTokenException("Token is not valid jwt");
            }

            var id = long.Parse(idClaim.Value);

            return Task.FromResult((id, roleClaim.Value));
        }
    }
}
