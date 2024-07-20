namespace JobBoard.Domain.Entity
{
    public class JobOfferTag : IEntity
    {
        public long Id { get; set; }
        public long OfferId { get; set; }
        public JobOffer Offer { get; set; }
        public string Tag { get; set; }
    }
}
