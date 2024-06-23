namespace JobBoard.Model.Response
{
    public class LoginResponse : ResponseModel
    {
        public long UserId { get; set; }
        public string AuthToken { get; set; }

        public LoginResponse(long userId, string token) : base()
        {
            UserId = userId;
            AuthToken = token;
        }

        public LoginResponse(string error) : base(error)
        {
            UserId = 0;
            AuthToken = "";
        }
    }
}
