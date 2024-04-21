namespace JobBoard.Application.Service.Abstraction
{
    public interface IHashService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hash);
    }
}
