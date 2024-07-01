namespace JobBoard.Model.CompanyAccount
{
    public class UpdateCompanyAccountRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ContactEmail { get; set; }
    }
}
