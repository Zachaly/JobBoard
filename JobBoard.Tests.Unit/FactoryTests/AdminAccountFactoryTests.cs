using JobBoard.Application.Factory;
using JobBoard.Model.AdminAccount;

namespace JobBoard.Tests.Unit.FactoryTests
{
    public class AdminAccountFactoryTests
    {
        private readonly AdminAccountFactory _factory;

        public AdminAccountFactoryTests()
        {
            _factory = new AdminAccountFactory();
        }

        [Fact]
        public void Create_CreatesProperEntity()
        {
            var request = new AddAdminAccountRequest
            {
                Login = "login",
                Password = "password"
            };

            const string Hash = "hash";

            var account = _factory.Create(request, Hash);

            Assert.Equal(request.Login, request.Login);
            Assert.Equal(Hash, account.PasswordHash);
        }
    }
}
