using JobBoard.Domain.Entity;
using JobBoard.Model.JobOffer;
using System.Linq.Expressions;

namespace JobBoard.Expressions
{
    public static class JobOfferExpressions
    {
        public static Expression<Func<JobOffer, JobOfferModel>> Model { get; } = offer => new JobOfferModel
        {
            Id = offer.Id,
            Company = CompanyAccountExpressions.Model.Compile().Invoke(offer.Company),
            CreationDate = offer.CreationDate,
            ExpirationDate = offer.ExpirationDate,
            Description = offer.Description,
            Title = offer.Title,
            Location = offer.Location,
            Requirements = offer.Requirements.AsQueryable().Select(JobOfferRequirementExpressions.Model).ToList(),
            BusinessName = offer.Business == null ? null : offer.Business.Name,
        };
    }
}
