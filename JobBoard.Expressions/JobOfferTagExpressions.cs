using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferTag;
using System.Linq.Expressions;

namespace JobBoard.Expressions
{
    public static class JobOfferTagExpressions
    {
        public static Expression<Func<JobOfferTag, JobOfferTagModel>> Model { get; } = tag => new JobOfferTagModel
        {
            Id = tag.Id,
            OfferId = tag.OfferId,
            Tag = tag.Tag,
        };
    }
}
