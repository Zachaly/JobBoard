using JobBoard.Application.Factory.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOffer;

namespace JobBoard.Application.Factory
{
    public class JobOfferFactory : IJobOfferFactory
    {
        public JobOffer Create(AddJobOfferRequest request)
            => new JobOffer
            {
                CompanyId = request.CompanyId,
                CreationDate = DateTimeOffset.UtcNow,
                Description = request.Description,
                ExpirationDate = DateTimeOffset.FromUnixTimeMilliseconds(request.ExpirationTimestamp),
                Location = request.Location,
                Title = request.Title,
                Requirements = request.Requirements.Select(req => new JobOfferRequirement
                {
                    Content = req,
                }).ToList()
            };

        public void Update(JobOffer offer, UpdateJobOfferRequest request)
        {
            offer.Description = request.Description;
            offer.Title = request.Title;
            offer.ExpirationDate = DateTimeOffset.FromUnixTimeMilliseconds(request.ExpirationTimestamp);
            offer.Location = request.Location;
        }
    }
}
