using JobBoard.Domain.Enum;

namespace JobBoard.Domain.Entity
{
    public class JobOfferApplication : IEntity
    {
        public long Id { get; set; }
        public string ResumeFileName { get; set; }
        public DateTimeOffset ApplicationDate { get; set; }
        public JobOfferApplicationState State { get; set; }

        public long OfferId { get; set; }
        public JobOffer Offer { get; set; }
        public long EmployeeId { get; set; }
        public EmployeeAccount Employee { get; set; }
    }
}
