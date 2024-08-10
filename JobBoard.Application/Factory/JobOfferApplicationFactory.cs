using JobBoard.Application.Factory.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Domain.Enum;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Application.Factory
{
    public class JobOfferApplicationFactory : IJobOfferApplicationFactory
    {
        public JobOfferApplication Create(AddJobOfferApplicationRequest request, string resumeFile)
            => new JobOfferApplication
            {
                ApplicationDate = DateTimeOffset.UtcNow,
                EmployeeId = request.EmployeeId,
                OfferId = request.OfferId,
                ResumeFileName = resumeFile,
                State = JobOfferApplicationState.Sent,
            };

        public void Update(JobOfferApplication entity, UpdateJobOfferApplicationRequest request)
        {
            entity.State = request.State;
        }
    }
}
