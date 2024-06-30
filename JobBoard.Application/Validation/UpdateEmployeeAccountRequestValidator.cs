using FluentValidation;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Application.Validation
{
    public class UpdateEmployeeAccountRequestValidator : AbstractValidator<UpdateEmployeeAccountRequest>
    {
        public UpdateEmployeeAccountRequestValidator()
        {
            RuleFor(r => r.AboutMe).MaximumLength(5000);
            RuleFor(r => r.PhoneNumber).NotEmpty().MaximumLength(20);
            RuleFor(r => r.FirstName).NotEmpty().MaximumLength(200);
            RuleFor(r => r.LastName).NotEmpty().MaximumLength(200);
            RuleFor(r => r.Country).MaximumLength(100);
            RuleFor(r => r.City).MaximumLength(100);
        }
    }
}
