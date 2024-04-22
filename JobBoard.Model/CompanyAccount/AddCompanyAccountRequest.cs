namespace JobBoard.Model.CompanyAccount
{
    public record AddCompanyAccountRequest
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string Name { get; init; }
        public string City { get; init; }
        public string PostalCode { get; init; }
        public string Address { get; init; }
        public string Country { get; init; }
        public string ContactEmail { get; init; }
    }
}
