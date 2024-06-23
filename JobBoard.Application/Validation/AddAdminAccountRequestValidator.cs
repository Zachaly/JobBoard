using FluentValidation;
using JobBoard.Model.AdminAccount;

namespace JobBoard.Application.Validation
{
    public class AddAdminAccountRequestValidator : AbstractValidator<AddAdminAccountRequest>
    {
        public AddAdminAccountRequestValidator()
        {
            RuleFor(x => x.Login).Length(1, 100);
            RuleFor(x => x.Password).Length(8, 100);
        }
    }
}
