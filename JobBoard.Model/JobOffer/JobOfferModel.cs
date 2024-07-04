using JobBoard.Model.CompanyAccount;

namespace JobBoard.Model.JobOffer
{
    public class JobOfferModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public CompanyModel Company { get; set; }
        public DateTimeOffset CreationDate{ get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}
