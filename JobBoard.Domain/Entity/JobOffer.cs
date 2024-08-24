using JobBoard.Domain.Enum;

namespace JobBoard.Domain.Entity
{
    public class JobOffer : IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public long CompanyId { get; set; }
        public CompanyAccount Company { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public long? BusinessId { get; set; }
        public Business? Business { get; set; }
        public JobOfferWorkType WorkType { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public SalaryType SalaryType { get; set; }
        public WorkExperienceLevel ExperienceLevel { get; set; }

        public ICollection<JobOfferRequirement> Requirements { get; set; }
        public ICollection<JobOfferTag> Tags { get; set; }
        public ICollection<JobOfferApplication> Applications { get; set; }
    }
}
