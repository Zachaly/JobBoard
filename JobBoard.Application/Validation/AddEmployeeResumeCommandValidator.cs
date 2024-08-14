using FluentValidation;
using JobBoard.Application.Command;

namespace JobBoard.Application.Validation
{
    public class AddEmployeeResumeCommandValidator : AbstractValidator<AddEmployeeResumeCommand>
    {
        public AddEmployeeResumeCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.ResumeName).MaximumLength(200).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(200).NotEmpty();
            RuleFor(x => x.About).MaximumLength(1000);
            RuleFor(x => x.City).MaximumLength(200);
            RuleForEach(x => x.Skills).NotEmpty().MaximumLength(100);
            RuleForEach(x => x.WorkExperience).ChildRules(exp =>
            {
                exp.RuleFor(x => x.Position).MaximumLength(100);
                exp.RuleFor(x => x.Company).MaximumLength(200);
                exp.RuleFor(x => x.Description).MaximumLength(500);
            });
            RuleForEach(x => x.Education).ChildRules(education =>
            {
                education.RuleFor(x => x.Level).MaximumLength(100);
                education.RuleFor(x => x.School).MaximumLength(200);
                education.RuleFor(x => x.Subject).MaximumLength(100);
            });
            RuleForEach(x => x.Languages).ChildRules(lang =>
            {
                lang.RuleFor(x => x.ProficiencyLevel).MaximumLength(50);
                lang.RuleFor(x => x.Name).MaximumLength(100);
            });
        }
    }
}
