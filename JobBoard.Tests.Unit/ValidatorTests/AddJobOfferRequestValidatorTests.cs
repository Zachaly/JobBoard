using JobBoard.Application.Validation;
using JobBoard.Model.JobOffer;

namespace JobBoard.Tests.Unit.ValidatorTests
{
    public class AddJobOfferRequestValidatorTests
    {
        private readonly AddJobOfferRequestValidator _validator;

        public AddJobOfferRequestValidatorTests()
        {
            _validator = new AddJobOfferRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddJobOfferRequest
            {
                CompanyId = 1,
                Description = "desc",
                ExpirationTimestamp = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeMilliseconds(),
                Location = "loc",
                Title = "title",
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1001)]
        public void InvalidDescriptionLength_DoesNotPassValidation(int len)
        {
            var request = new AddJobOfferRequest
            {
                CompanyId = 1,
                Description = new string('a', len),
                ExpirationTimestamp = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeMilliseconds(),
                Location = "loc",
                Title = "title",
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidTitleLength_DoesNotPassValidation(int len)
        {
            var request = new AddJobOfferRequest
            {
                CompanyId = 1,
                Description = "desc",
                ExpirationTimestamp = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeMilliseconds(),
                Location = "loc",
                Title = new string('a', len),
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidLocationLength_DoesNotPassValidation(int len)
        {
            var request = new AddJobOfferRequest
            {
                CompanyId = 1,
                Description = "desc",
                ExpirationTimestamp = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeMilliseconds(),
                Location = new string('a', len),
                Title = "title",
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidExpirationTimestamp_DoesNotPassValidation()
        {
            var request = new AddJobOfferRequest
            {
                CompanyId = 1,
                Description = "desc",
                ExpirationTimestamp = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds(),
                Location = "loc",
                Title = "title",
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
