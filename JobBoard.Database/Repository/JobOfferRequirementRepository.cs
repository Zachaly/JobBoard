using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Database.Repository
{
    public class JobOfferRequirementRepository : RepositoryBase<JobOfferRequirement, JobOfferRequirementModel, GetJobOfferRequirementRequest>,
        IJobOfferRequirementRepository
    {
        public JobOfferRequirementRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = JobOfferRequirementExpressions.Model;
        }
    }
}
