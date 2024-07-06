using FluentValidation;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Application.Validation
{
    public class AddJobOfferRequirementRequestValidator : AbstractValidator<AddJobOfferRequirementRequest>
    {
        public AddJobOfferRequirementRequestValidator()
        {
            RuleFor(x => x.Content).Length(1, 300);
        }
    }
}
