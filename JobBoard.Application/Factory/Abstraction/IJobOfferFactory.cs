using JobBoard.Domain.Entity;
using JobBoard.Model.JobOffer;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface IJobOfferFactory : IEntityFactory<JobOffer, AddJobOfferRequest, UpdateJobOfferRequest>
    {
    }
}
