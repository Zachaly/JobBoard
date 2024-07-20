using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferTag;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface IJobOfferTagFactory : ICreateFactory<JobOfferTag, AddJobOfferTagRequest>
    {
    }
}
