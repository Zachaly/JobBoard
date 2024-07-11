using FluentValidation;
using JobBoard.Model.Business;

namespace JobBoard.Application.Validation
{
    public class AddBusinessRequestValidator : AbstractValidator<AddBusinessRequest>
    {
        public AddBusinessRequestValidator()
        {
            RuleFor(r => r.Name).Length(1, 100);
        }
    }
}
