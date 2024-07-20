using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.JobOfferTag;

namespace JobBoard.Database.Repository
{
    public class JobOfferTagRepository : RepositoryBase<JobOfferTag, JobOfferTagModel, GetJobOfferTagRequest>, IJobOfferTagRepository
    {
        public JobOfferTagRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = JobOfferTagExpressions.Model;
        }
    }
}
