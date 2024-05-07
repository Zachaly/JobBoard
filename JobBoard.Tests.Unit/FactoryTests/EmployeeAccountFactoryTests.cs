using JobBoard.Application.Factory;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Tests.Unit.FactoryTests
{
    public class EmployeeAccountFactoryTests
    {
        private readonly EmployeeAccountFactory _factory;

        public EmployeeAccountFactoryTests()
        {
            _factory = new EmployeeAccountFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddEmployeeAccountRequest
            {
                AboutMe = "about",
                City = "city",
                Country = "cntr",
                Email = "email",
                FirstName = "fname",
                LastName = "lname",
                Password = "passw",
                PhoneNumber = "1234567890",
            };

            const string Hash = "hash";

            var account = _factory.Create(request, Hash);

            Assert.Equal(request.AboutMe, account.AboutMe);
            Assert.Equal(request.City, account.City);
            Assert.Equal(request.Country, account.Country);
            Assert.Equal(request.Email, account.Email);
            Assert.Equal(request.FirstName, account.FirstName);
            Assert.Equal(request.LastName, account.LastName);
            Assert.Equal(request.PhoneNumber, account.PhoneNumber);
            Assert.Equal(Hash, account.PasswordHash);
        }
    }
}
