using FluentValidation;
using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Application.Command
{
    public class UpdateJobOfferRequirementCommand : UpdateJobOfferRequirementRequest, IUpdateEntityCommand
    {
    }

    public class UpdateJobOfferRequirementHandler : UpdateEntityHandler<JobOfferRequirement, UpdateJobOfferRequirementRequest,
        UpdateJobOfferRequirementCommand>
    {
        public UpdateJobOfferRequirementHandler(IJobOfferRequirementRepository repository, IJobOfferRequirementFactory factory,
            IValidator<UpdateJobOfferRequirementRequest> validator) : base(repository, factory, validator)
        {
        }
    }
}
