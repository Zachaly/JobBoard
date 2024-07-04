using JobBoard.Domain.Entity;
using JobBoard.Model.JobOffer;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IJobOfferRepository : IRepositoryBase<JobOffer, JobOfferModel, GetJobOfferRequest>
    {
    }
}
