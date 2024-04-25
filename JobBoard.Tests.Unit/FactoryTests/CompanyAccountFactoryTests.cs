using JobBoard.Application.Factory;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Tests.Unit.FactoryTests
{
    public class CompanyAccountFactoryTests 
    {
        private readonly CompanyAccountFactory _factory;

        public CompanyAccountFactoryTests()
        {
            _factory = new CompanyAccountFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddCompanyAccountRequest
            {
                Email = "email",
                Password = "pass",
                Name = "name",
                City = "city",
                PostalCode = "postal-code",
                Address = "addr",
                Country = "ctn",
                ContactEmail = "contactemail"
            };

            var passwordHash = "hash";

            var account = _factory.Create(request, passwordHash);

            Assert.Equal(passwordHash, account.Password);
            Assert.Equal(request.Country, account.Country);
            Assert.Equal(request.Name, account.Name);
            Assert.Equal(request.City, account.City);
            Assert.Equal(request.ContactEmail, account.ContactEmail);
            Assert.Equal(request.Address, account.Address);
            Assert.Equal(request.Email, account.Email);
        }
    }
}
