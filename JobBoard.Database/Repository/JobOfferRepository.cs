using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.JobOffer;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Database.Repository
{
    public class JobOfferRepository : RepositoryBase<JobOffer, JobOfferModel, GetJobOfferRequest>, IJobOfferRepository
    {
        public JobOfferRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = JobOfferExpressions.Model;
        }

        public override async Task DeleteByIdAsync(long id)
        {
            var entity = await _dbContext.Set<JobOffer>()
                .Where(e => e.Id == id)
                .Include(e => e.Requirements)
                .FirstOrDefaultAsync();

            if(entity is null)
            {
                return;
            }

            _dbContext.Remove(entity);

            await _dbContext.SaveChangesAsync();
        }
    }
}
