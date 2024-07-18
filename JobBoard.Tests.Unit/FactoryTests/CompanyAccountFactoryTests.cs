using JobBoard.Application.Factory;
using JobBoard.Domain.Entity;
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

            Assert.Equal(passwordHash, account.PasswordHash);
            Assert.Equal(request.Country, account.Country);
            Assert.Equal(request.Name, account.Name);
            Assert.Equal(request.City, account.City);
            Assert.Equal(request.ContactEmail, account.ContactEmail);
            Assert.Equal(request.Address, account.Address);
            Assert.Equal(request.Email, account.Email);
        }

        [Fact]
        public void Updates_ProperlyUpdatesEntity()
        {
            var account = new CompanyAccount();

            var request = new UpdateCompanyAccountRequest
            {
                Address = "addr",
                City = "city",
                ContactEmail = "email@email.com",
                Country = "ctn",
                Name = "naam",
                PostalCode = "postal"
            };

            _factory.Update(account, request);

            Assert.Equal(request.Name, account.Name);
            Assert.Equal(request.Country, account.Country);
            Assert.Equal(request.City, account.City);
            Assert.Equal(request.ContactEmail, account.ContactEmail);
            Assert.Equal(request.PostalCode, account.PostalCode);
            Assert.Equal(request.Address, account.Address);
        }
    }
}
