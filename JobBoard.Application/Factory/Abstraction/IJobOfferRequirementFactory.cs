using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface IJobOfferRequirementFactory : IEntityFactory<JobOfferRequirement, AddJobOfferRequirementRequest, UpdateJobOfferRequirementRequest> 
    {
    }
}
