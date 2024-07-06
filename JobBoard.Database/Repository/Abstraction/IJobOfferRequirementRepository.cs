using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IJobOfferRequirementRepository : IRepositoryBase<JobOfferRequirement, JobOfferRequirementModel, GetJobOfferRequirementRequest>
    {
    }
}
