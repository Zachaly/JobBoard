using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferApplication;
using System.Linq.Expressions;

namespace JobBoard.Expressions
{
    public static class JobOfferApplicationExpressions
    {
        public static Expression<Func<JobOfferApplication, JobOfferApplicationModel>> Model { get; } = application => new JobOfferApplicationModel
        {
            ApplicationDate = application.ApplicationDate,
            Id = application.Id,
            State = application.State,
            Employee = EmployeeAccountExpressions.Model.Compile().Invoke(application.Employee),
            OfferId = application.OfferId,
            OfferTitle = application.Offer.Title
        };
    }
}
