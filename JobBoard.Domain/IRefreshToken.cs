namespace JobBoard.Domain
{
    public interface IRefreshToken
    {
        string Token { get; set; }
        DateTime CreationDate { get; set; }
        DateTime ExpirationDate { get; set; }
        bool IsValid { get; set; }
        long AccountId { get; set; }
    }
}
