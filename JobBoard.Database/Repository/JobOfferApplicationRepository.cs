using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Database.Repository
{
    public class JobOfferApplicationRepository : RepositoryBase<JobOfferApplication, JobOfferApplicationModel, GetJobOfferApplicationRequest>,
        IJobOfferApplicationRepository
    {
        public JobOfferApplicationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = JobOfferApplicationExpressions.Model;
        }
    }
}
