using JobBoard.Domain.Enum;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Model.JobOfferApplication
{
    public class JobOfferApplicationModel
    {
        public long Id { get; set; }
        public DateTimeOffset ApplicationDate { get; set; }
        public JobOfferApplicationState State { get; set; }
        public EmployeeAccountModel Employee { get; set; }
        public long OfferId { get; set; }
        public string OfferTitle { get; set; }
    }
}
