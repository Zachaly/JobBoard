namespace JobBoard.Domain.Entity
{
    public class JobOfferRequirement : IEntity
    {
        public long Id { get; set; }
        public long OfferId { get; set; }
        public JobOffer Offer { get; set; }
        public string Content { get; set; }
    }
}
