using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferRequirement;
using System.Linq.Expressions;

namespace JobBoard.Expressions
{
    public static class JobOfferRequirementExpressions
    {
        public static Expression<Func<JobOfferRequirement, JobOfferRequirementModel>> Model { get; } = requirement => new JobOfferRequirementModel
        {
            Content = requirement.Content,
            Id = requirement.Id,
        };
    }
}
