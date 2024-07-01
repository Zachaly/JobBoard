using FluentValidation;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Application.Validation
{
    public class UpdateCompanyAccountRequestValidator : AbstractValidator<UpdateCompanyAccountRequest>
    {
        public UpdateCompanyAccountRequestValidator()
        {
            RuleFor(x => x.ContactEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Country).NotEmpty();
        }
    }
}
