using JobBoard.Domain.Entity;
using JobBoard.Model.JobOffer;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface IJobOfferFactory
    {
        JobOffer Create(AddJobOfferRequest request);
        void Update(JobOffer offer, UpdateJobOfferRequest request);
    }
}
