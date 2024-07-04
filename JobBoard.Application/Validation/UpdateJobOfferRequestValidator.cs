using FluentValidation;
using JobBoard.Model.JobOffer;

namespace JobBoard.Application.Validation
{
    internal class UpdateJobOfferRequestValidator : AbstractValidator<UpdateJobOfferRequest>
    {
        public UpdateJobOfferRequestValidator()
        {
            RuleFor(r => r.Title).Length(1, 100);
            RuleFor(r => r.Description).Length(1, 1000);
            RuleFor(r => r.ExpirationTimestamp).GreaterThan(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            RuleFor(r => r.Location).Length(1, 100);
        }
    }
}
