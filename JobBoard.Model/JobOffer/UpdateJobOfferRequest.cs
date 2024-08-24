using JobBoard.Domain.Enum;

namespace JobBoard.Model.JobOffer
{
    public class UpdateJobOfferRequest : IUpdateRequest
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public long ExpirationTimestamp { get; set; }
        public long? BusinessId { get; set; }
        public JobOfferWorkType WorkType { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public SalaryType? SalaryType { get; set; }
        public WorkExperienceLevel? ExperienceLevel { get; set; }
    }
}
