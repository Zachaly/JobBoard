namespace JobBoard.Domain.Entity
{
    public class AdminAccount : IAccountEntity
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<AdminAccountRefreshToken> RefreshTokens { get; set; }
    }
}
