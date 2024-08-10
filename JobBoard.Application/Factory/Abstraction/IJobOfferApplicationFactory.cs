using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface IJobOfferApplicationFactory : IUpdateFactory<JobOfferApplication, UpdateJobOfferApplicationRequest>
    {
        JobOfferApplication Create(AddJobOfferApplicationRequest request, string resumeFile);
    }
}
