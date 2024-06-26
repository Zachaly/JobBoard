namespace JobBoard.Domain.Entity
{
    public class CompanyAccountRefreshToken : IRefreshToken
    {
        public string Token { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsValid { get; set; }
        public long AccountId { get; set; }
        public CompanyAccount Account { get; set; }
    }
}
