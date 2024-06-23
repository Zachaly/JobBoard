namespace JobBoard.Model.AdminAccount
{
    public record AddAdminAccountRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
