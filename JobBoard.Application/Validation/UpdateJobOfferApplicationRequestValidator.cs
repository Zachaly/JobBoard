using FluentValidation;
using JobBoard.Model.JobOffer;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Application.Validation
{
    public class UpdateJobOfferApplicationRequestValidator : AbstractValidator<UpdateJobOfferApplicationRequest>
    {
        public UpdateJobOfferApplicationRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
