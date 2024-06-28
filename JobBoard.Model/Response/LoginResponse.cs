namespace JobBoard.Model.Response
{
    public class LoginResponse : ResponseModel
    {
        public long UserId { get; set; }
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }

        public LoginResponse(long userId, string token, string refreshToken) : base()
        {
            UserId = userId;
            AuthToken = token;
            RefreshToken = refreshToken;
        }

        public LoginResponse(string error) : base(error)
        {
            UserId = 0;
            AuthToken = "";
            RefreshToken = "";
        }

        public LoginResponse() : base() { }
    }
}
