using FluentValidation;
using JobBoard.Model.JobOffer;

namespace JobBoard.Application.Validation
{
    public class AddJobOfferRequestValidator : AbstractValidator<AddJobOfferRequest>
    {
        public AddJobOfferRequestValidator()
        {
            RuleFor(r => r.Title).Length(1, 100);
            RuleFor(r => r.Description).Length(1, 1000);
            RuleFor(r => r.ExpirationTimestamp).GreaterThan(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            RuleFor(r => r.Location).Length(1, 100);
            RuleForEach(r => r.Requirements).Length(1, 300);
        }
    }
}
