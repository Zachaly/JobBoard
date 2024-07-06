using JobBoard.Application.Factory.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Application.Factory
{
    public class JobOfferRequirementFactory : IJobOfferRequirementFactory
    {
        public JobOfferRequirement Create(AddJobOfferRequirementRequest request)
            => new JobOfferRequirement
            {
                OfferId = request.OfferId,
                Content = request.Content,
            };

        public void Update(JobOfferRequirement entity, UpdateJobOfferRequirementRequest request)
        {
            entity.Content = request.Content;
        }
    }
}
