using JobBoard.Domain.Enum;
using JobBoard.Model.Attributes;
using JobBoard.Model.Enum;

namespace JobBoard.Model.JobOfferApplication
{
    public class GetJobOfferApplicationRequest : PagedRequest
    {
        public JobOfferApplicationState? State { get; set; }
        public long? OfferId { get; set; }
        public long? EmployeeId { get; set; }
        [CustomFilter(Property = "ApplicationDate", ComparisonType = ComparisonType.GreaterOrEqual)]
        public DateTimeOffset? MinimalApplicationDate { get; set; }
    }
}
