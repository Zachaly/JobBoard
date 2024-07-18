using JobBoard.Application.Factory;
using JobBoard.Domain.Entity;
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

        [Fact]
        public void Update_ProperlyUpdatesEntity()
        {
            var entity = new EmployeeAccount
            {
                Id = 1,
                AboutMe = "about",
                City = "city",
                Country = "cnt",
                FirstName = "fname",
                LastName = "lname",
                PhoneNumber = "1234567890",
            };

            var request = new UpdateEmployeeAccountRequest
            {
                AboutMe = "me",
                City = "ytic",
                Country = "tnc",
                FirstName = "namef",
                LastName = "namel",
                PhoneNumber = "0987654321"
            };

            _factory.Update(entity, request);

            Assert.Equal(request.AboutMe, entity.AboutMe);
            Assert.Equal(request.City, entity.City);
            Assert.Equal(request.Country, entity.Country);
            Assert.Equal(request.FirstName, entity.FirstName);
            Assert.Equal(request.LastName, entity.LastName);
            Assert.Equal(request.PhoneNumber, entity.PhoneNumber);
        }
    }
}
