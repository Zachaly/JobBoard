using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface IJobOfferRequirementFactory
    {
        JobOfferRequirement Create(AddJobOfferRequirementRequest request);
        void Update(JobOfferRequirement entity, UpdateJobOfferRequirementRequest request);
    }
}
