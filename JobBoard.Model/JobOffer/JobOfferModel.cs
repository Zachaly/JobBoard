using JobBoard.Model.CompanyAccount;
using JobBoard.Model.JobOfferRequirement;
using JobBoard.Model.JobOfferTag;

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
        public IEnumerable<JobOfferRequirementModel>? Requirements { get; set; }
        public long? BusinessId { get; set; }
        public string? BusinessName { get; set; }
        public IEnumerable<JobOfferTagModel> Tags { get; set; }
    }
}
