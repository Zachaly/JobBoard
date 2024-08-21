using JobBoard.Domain.Enum;
using JobBoard.Model.Attributes;
using JobBoard.Model.Enum;

namespace JobBoard.Model.JobOffer
{
    public class GetJobOfferRequest : PagedRequest
    {
        public long? CompanyId { get; set; }
        public string? Location { get; set; }
        [CustomFilter(Property = "Company", SubProperty = "Name", ComparisonType = ComparisonType.StartsWith)]
        public string? SearchCompanyName { get; set; }
        [CustomFilter(Property = "ExpirationDate", ComparisonType = ComparisonType.GreaterOrEqual)]
        public DateTimeOffset? MinimalExpirationDate { get; set; }
        [CustomFilter(ComparisonType = ComparisonType.Contains, Property = "BusinessId")]
        public ICollection<long?>? BusinessIds { get; set; }
        [SkipFilter]
        public ICollection<string>? Tags { get; set; }
        public JobOfferWorkType? WorkType { get; set; }
    }
}
