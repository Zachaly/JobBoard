using JobBoard.Application.Validation;
using JobBoard.Model.AdminAccount;

namespace JobBoard.Tests.Unit.ValidatorTests
{
    public class AddAdminAccountRequestValidatorTests
    {
        private readonly AddAdminAccountRequestValidator _validator;

        public AddAdminAccountRequestValidatorTests()
        {
            _validator = new AddAdminAccountRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddAdminAccountRequest
            {
                Login = "login",
                Password = "password123"
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidLoginLength_DoesNotPassValidation(int len)
        {
            var request = new AddAdminAccountRequest
            {
                Login = new string('a', len),
                Password = "password123"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(101)]
        public void InvalidPasswordLength_DoesNotPassValidation(int len)
        {
            var request = new AddAdminAccountRequest
            {
                Login = "login",
                Password = new string('a', len)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
