namespace JobBoard.Domain.Entity
{
    public class CompanyAccount : IEntity
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ContactEmail { get; set; }

        public ICollection<CompanyAccountRefreshToken> RefreshTokens { get; set; }
    }
}
