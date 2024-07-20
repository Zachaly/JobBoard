using FluentValidation;
using JobBoard.Model.JobOfferTag;

namespace JobBoard.Application.Validation
{
    public class AddJobOfferTagRequestValidator : AbstractValidator<AddJobOfferTagRequest>
    {
        public AddJobOfferTagRequestValidator()
        {
            RuleFor(t => t.Tag).Length(1, 50);
        }
    }
}
