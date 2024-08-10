using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IJobOfferApplicationRepository : IRepositoryBase<JobOfferApplication, JobOfferApplicationModel, GetJobOfferApplicationRequest>
    {
    }
}
