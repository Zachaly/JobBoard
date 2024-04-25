using JobBoard.Application.Validation;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Tests.Unit.ValidatorTests
{
    public class AddCompanyAccountRequestValidatorTests
    {
        private readonly AddCompanyAccountRequestValidator _validator;

        public AddCompanyAccountRequestValidatorTests()
        {
            _validator = new AddCompanyAccountRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddCompanyAccountRequest()
            {
                Email = "email@email.com",
                Password = "zaq1@WSX",
                Name = "company name",
                City = "city",
                PostalCode = "12-345",
                Address = "address",
                Country = "country",
                ContactEmail = "email2@email.com"
            };
                   
            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidEmail_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest()
            {
                Email = "email",
                Password = "zaq1@WSX",
                Name = "company name",
                City = "city",
                PostalCode = "12-345",
                Address = "address",
                Country = "country",
                ContactEmail = "email2@email.com"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(101)]
        public void InvalidPasswordLength_DoesNotPassValidation(int len)
        {
            var request = new AddCompanyAccountRequest()
            {
                Email = "email@email.com",
                Password = new string('a', len),
                Name = "company name",
                City = "city",
                PostalCode = "12-345",
                Address = "address",
                Country = "country",
                ContactEmail = "email2@email.com"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(201)]
        public void InvalidCompanyName_DoesNotPassValidation(int len)
        {
            var request = new AddCompanyAccountRequest()
            {
                Email = "email@email.com",
                Password = "zaq1@WSX",
                Name = new string('a', len),
                City = "city",
                PostalCode = "12-345",
                Address = "address",
                Country = "country",
                ContactEmail = "email2@email.com"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidCity_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest()
            {
                Email = "email@email.com",
                Password = "zaq1@WSX",
                Name = "company name",
                City = "",
                PostalCode = "12-345",
                Address = "address",
                Country = "country",
                ContactEmail = "email2@email.com"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidPostalCode_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest()
            {
                Email = "email@email.com",
                Password = "zaq1@WSX",
                Name = "company name",
                City = "city",
                PostalCode = "",
                Address = "address",
                Country = "country",
                ContactEmail = "email2@email.com"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidAddress_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest()
            {
                Email = "email@email.com",
                Password = "zaq1@WSX",
                Name = "company name",
                City = "city",
                PostalCode = "12-345",
                Address = "",
                Country = "country",
                ContactEmail = "email2@email.com"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidCountry_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest()
            {
                Email = "email@email.com",
                Password = "zaq1@WSX",
                Name = "company name",
                City = "city",
                PostalCode = "12-345",
                Address = "address",
                Country = "",
                ContactEmail = "email2@email.com"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidContactEmail_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest()
            {
                Email = "email@email.com",
                Password = "zaq1@WSX",
                Name = "company name",
                City = "city",
                PostalCode = "12-345",
                Address = "address",
                Country = "country",
                ContactEmail = "email"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
