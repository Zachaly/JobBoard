﻿namespace JobBoard.Domain.Entity
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
        public ICollection<JobOfferRequirement> Requirements { get; set; }
    }
}
