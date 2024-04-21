using JobBoard.Application.Validation;
using JobBoard.Model.CompanyAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Tests.Unit.Validator
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
            var request = new AddCompanyAccountRequest(
                "email@email.com",
                "zaq1@WSX",
                "company name",
                "city",
                "12-345",
                "address",
                "country",
                "email2@email.com");

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidEmail_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest(
                "email",
                "zaq1@WSX",
                "company name",
                "city",
                "12-345",
                "address",
                "country",
                "email@email.com");

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(101)]
        public void InvalidPasswordLength_DoesNotPassValidation(int len)
        {
            var request = new AddCompanyAccountRequest(
                "email@email.com",
                new string('a', len),
                "company name",
                "city",
                "12-345",
                "address",
                "country",
                "email@email.com");

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidCompanyName_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest(
                "email@email.com",
                "zaq1@WSX",
                "",
                "city",
                "12-345",
                "address",
                "country",
                "email@email.com");

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidCity_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest(
                "email@email.com",
                "zaq1@WSX",
                "company name",
                "",
                "12-345",
                "address",
                "country",
                "email@email.com");

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidPostalCode_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest(
                "email@email.com",
                "zaq1@WSX",
                "company name",
                "city",
                "",
                "address",
                "country",
                "email@email.com");

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidAddress_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest(
                "email@email.com",
                "zaq1@WSX",
                "company name",
                "city",
                "12-345",
                "",
                "country",
                "email@email.com");

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidCountry_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest(
                "email@email.com",
                "zaq1@WSX",
                "company name",
                "city",
                "12-345",
                "address",
                "",
                "email@email.com");

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidContactEmail_DoesNotPassValidation()
        {
            var request = new AddCompanyAccountRequest(
                "email@email.com",
                "zaq1@WSX",
                "company name",
                "city",
                "12-345",
                "address",
                "country",
                "email");

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
