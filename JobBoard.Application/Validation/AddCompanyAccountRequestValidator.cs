using FluentValidation;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Application.Validation
{
    public class AddCompanyAccountRequestValidator : AbstractValidator<AddCompanyAccountRequest>
    {
        public AddCompanyAccountRequestValidator()
        {
            RuleFor(x => x.ContactEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().Length(8, 100);
            RuleFor(x => x.About).MaximumLength(1000);
        }
    }
}
