using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.JobOffer;

namespace JobBoard.Database.Repository
{
    public class JobOfferRepository : RepositoryBase<JobOffer, JobOfferModel, GetJobOfferRequest>, IJobOfferRepository
    {
        public JobOfferRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = JobOfferExpressions.Model;
        }
    }
}
