using FluentValidation;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Application.Validation
{
    public class UpdateJobOfferRequirementRequestValidator : AbstractValidator<UpdateJobOfferRequirementRequest>
    {
        public UpdateJobOfferRequirementRequestValidator()
        {
            RuleFor(r => r.Content).Length(1, 300);
        }
    }
}
