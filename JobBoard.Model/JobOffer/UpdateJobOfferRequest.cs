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
    }
}
