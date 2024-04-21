using Microsoft.Extensions.Configuration;
using JobBoard.Application.Service;

namespace JobBoard.Tests.Unit.Service
{
    public class HashServiceTests
    {
        private readonly HashService _hashService;

        public HashServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                    {"PasswordSalt", "TestingSalt"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            _hashService = new HashService(configuration);
        }

        [Fact]
        public void HashPassword_PasswordHashed()
        {
            var password = "password";

            var hash = _hashService.HashPassword(password);

            Assert.NotEqual(hash, password);
        }

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            var password = "password";

            var hash = _hashService.HashPassword(password);

            Assert.True(_hashService.VerifyPassword(password, hash));
        }

        [Fact]
        public void VerifyPassword_IncorrectPassword_ReturnsFalse()
        {
            var password = "password";

            var hash = _hashService.HashPassword(password);

            Assert.False(_hashService.VerifyPassword("anotherpassword", hash));
        }
    }
}
