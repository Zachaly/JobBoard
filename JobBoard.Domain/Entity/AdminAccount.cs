namespace JobBoard.Domain.Entity
{
    public class AdminAccount : IEntity
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
