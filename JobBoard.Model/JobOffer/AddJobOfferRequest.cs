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
    }
}
