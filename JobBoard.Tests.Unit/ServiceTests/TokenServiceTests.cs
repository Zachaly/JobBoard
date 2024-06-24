using JobBoard.Application.Service;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JobBoard.Tests.Unit.ServiceTests
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly IOptions<TokenConfiguration> _config;
        private readonly TokenValidationParameters _validationParameters;

        public TokenServiceTests()
        {
            var config = new TokenConfiguration
            {
                AuthAudience = "audience",
                AuthIssuer = "issuer",
                SecretKey = "xDIppqCRe5x6m96aCL0WThU5fpg8uAOG",
                TokenLifetime = 120
            };

            _config = Substitute.For<IOptions<TokenConfiguration>>();
            _config.Value.Returns(config);

            var bytes = Encoding.UTF8.GetBytes(config.SecretKey);
            var key = new SymmetricSecurityKey(bytes);
            _validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidIssuer = config.AuthIssuer,
                ValidAudience = config.AuthAudience,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAlgorithms = [SecurityAlgorithms.HmacSha256Signature],
                ValidateIssuerSigningKey = true,
            };

            _tokenService = new TokenService(_config, _validationParameters);
        }

        [Fact]
        public async Task CreateTokenAsync_CreatesValidToken()
        {
            const long UserId = 2;
            const string Role = "Test";

            var token = await _tokenService.GenerateTokenAsync(UserId, Role);

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.MapInboundClaims = false;

            var claimsPrincipal = tokenHandler.ValidateToken(token, _validationParameters, out var validatedToken);

            Assert.Contains(claimsPrincipal.Claims, c => c.Type == "sub" && c.Value == UserId.ToString());
            Assert.Contains(claimsPrincipal.Claims, c => c.Type == "Role" && c.Value == Role);
        }

        [Fact]
        public async Task GetIdFromTokenAsync_ReturnsProperId()
        {
            const long UserId = 2;
            const string Role = "Test";

            var token = await _tokenService.GenerateTokenAsync(UserId, Role);

            var res = await _tokenService.GetUserIdFromToken(token);

            Assert.Equal(UserId, res);
        }
    }
}
