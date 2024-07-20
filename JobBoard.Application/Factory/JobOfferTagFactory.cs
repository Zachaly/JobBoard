using JobBoard.Application.Factory.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferTag;

namespace JobBoard.Application.Factory
{
    public class JobOfferTagFactory : IJobOfferTagFactory
    {
        public JobOfferTag Create(AddJobOfferTagRequest request)
            => new JobOfferTag
            {
                Tag = request.Tag,
                OfferId = request.OfferId,
            };
    }
}
