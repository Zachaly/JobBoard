namespace JobBoard.Domain.Entity
{
    public class CompanyAccount : IAccountEntity
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ContactEmail { get; set; }
        public string? About { get; set; }

        public ICollection<CompanyAccountRefreshToken> RefreshTokens { get; set; }
        public ICollection<JobOffer> JobOffers { get; set; }
    }
}
