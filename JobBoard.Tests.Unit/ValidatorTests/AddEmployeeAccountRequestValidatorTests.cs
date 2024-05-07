using JobBoard.Application.Validation;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Tests.Unit.ValidatorTests
{
    public class AddEmployeeAccountRequestValidatorTests
    {
        private readonly AddEmployeeAccountRequestValidator _validator;

        public AddEmployeeAccountRequestValidatorTests()
        {
            _validator = new AddEmployeeAccountRequestValidator();
        }

        [Fact]
        public void ValidRequest_OnlyRequiredFields_PassesValidation()
        {
            var request = new AddEmployeeAccountRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = "lname",
                Password = "password",
                PhoneNumber = "1234567890",
            };

            var validation = _validator.Validate(request);

            Assert.True(validation.IsValid);
        }

        [Fact]
        public void ValidRequest_AllFields_PassesValidation()
        {
            var request = new AddEmployeeAccountRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = "lname",
                Password = "password",
                PhoneNumber = "1234567890",
                AboutMe = "about",
                Country = "ctn",
                City = "city"
            };

            var validation = _validator.Validate(request);

            Assert.True(validation.IsValid);
        }

        [Fact]
        public void InvalidEmail_DoesNotPassValidation()
        {
            var request = new AddEmployeeAccountRequest
            {
                Email = "email",
                FirstName = "fname",
                LastName = "lname",
                Password = "password",
                PhoneNumber = "1234567890",
                AboutMe = "about",
                Country = "ctn",
                City = "city"
            };

            var validation = _validator.Validate(request);

            Assert.False(validation.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(201)]
        public void InvalidFirstName_DoesNotPassValidation(int len)
        {
            var request = new AddEmployeeAccountRequest
            {
                Email = "email@email.com",
                FirstName = new string('a', len),
                LastName = "lname",
                Password = "password",
                PhoneNumber = "1234567890",
                AboutMe = "about",
                Country = "ctn",
                City = "city"
            };

            var validation = _validator.Validate(request);

            Assert.False(validation.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(201)]
        public void InvalidLastName_DoesNotPassValidation(int len)
        {
            var request = new AddEmployeeAccountRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = new string('a', len),
                Password = "password",
                PhoneNumber = "1234567890",
                AboutMe = "about",
                Country = "ctn",
                City = "city"
            };

            var validation = _validator.Validate(request);

            Assert.False(validation.IsValid);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(101)]
        public void InvalidPasswordLength_DoesNotPassValidation(int len)
        {
            var request = new AddEmployeeAccountRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = "lname",
                Password = new string('a', len),
                PhoneNumber = "1234567890",
                AboutMe = "about",
                Country = "ctn",
                City = "city"
            };

            var validation = _validator.Validate(request);

            Assert.False(validation.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(21)]
        public void InvalidPhoneNumber_DoesNotPassValidation(int len)
        {
            var request = new AddEmployeeAccountRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = "lname",
                Password = "password",
                PhoneNumber = new string('1', len),
                AboutMe = "about",
                Country = "ctn",
                City = "city"
            };

            var validation = _validator.Validate(request);

            Assert.False(validation.IsValid);
        }

        [Fact]
        public void InvalidAboutMeLength_DoesNotPassValidation()
        {
            var request = new AddEmployeeAccountRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = "lname",
                Password = "password",
                PhoneNumber = "1234567890",
                AboutMe = new string('a', 5001),
                Country = "ctn",
                City = "city"
            };

            var validation = _validator.Validate(request);

            Assert.False(validation.IsValid);
        }

        [Fact]
        public void InvalidCountryLength_DoesNotPassValidation()
        {
            var request = new AddEmployeeAccountRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = "lname",
                Password = "password",
                PhoneNumber = "1234567890",
                AboutMe = "about",
                Country = new string('a', 101),
                City = "city"
            };

            var validation = _validator.Validate(request);

            Assert.False(validation.IsValid);
        }

        [Fact]
        public void InvalidCityLength_DoesNotPassValidation()
        {
            var request = new AddEmployeeAccountRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = "lname",
                Password = "password",
                PhoneNumber = "1234567890",
                AboutMe = "about",
                Country = "ctn",
                City = new string('a', 101)
            };

            var validation = _validator.Validate(request);

            Assert.False(validation.IsValid);
        }
    }
}
