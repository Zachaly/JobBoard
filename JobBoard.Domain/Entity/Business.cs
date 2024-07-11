namespace JobBoard.Domain.Entity
{
    public class Business : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<JobOffer> JobOffers { get; set; }
    }
}
