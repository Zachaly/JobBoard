using JobBoard.Domain.Enum;

namespace JobBoard.Model.JobOffer
{
    public class AddJobOfferRequest
    {
        public long CompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long ExpirationTimestamp { get; set; }
        public string Location { get; set; }
        public IEnumerable<string> Requirements { get; set; }
        public long? BusinessId { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public JobOfferWorkType WorkType { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public SalaryType? SalaryType { get; set; }
        public WorkExperienceLevel? ExperienceLevel { get; set; }
    }
}
