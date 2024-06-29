namespace JobBoard.Domain
{
    public interface IAccountEntity : IEntity
    {
        public string PasswordHash { get; set; }
    }
}
