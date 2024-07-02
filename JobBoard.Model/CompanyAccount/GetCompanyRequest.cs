namespace JobBoard.Model.CompanyAccount
{
    public class GetCompanyRequest : PagedRequest
    {
        public string? Name { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
