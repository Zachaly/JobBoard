using FluentValidation;
using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Application.Command
{
    public class AddJobOfferRequirementCommand : AddJobOfferRequirementRequest, IAddEntityCommand
    {
    }

    public class AddJobOfferRequirementHandler : AddEntityHandler<JobOfferRequirement, AddJobOfferRequirementRequest,
        AddJobOfferRequirementCommand>
    {
        public AddJobOfferRequirementHandler(IJobOfferRequirementRepository repository, IJobOfferRequirementFactory factory,
            IValidator<AddJobOfferRequirementRequest> validator) : base(repository, factory, validator)
        {
        }
    }
}
