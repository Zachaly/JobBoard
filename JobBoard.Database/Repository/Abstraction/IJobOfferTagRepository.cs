using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferTag;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IJobOfferTagRepository : IRepositoryBase<JobOfferTag, JobOfferTagModel, GetJobOfferTagRequest>
    {
    }
}
