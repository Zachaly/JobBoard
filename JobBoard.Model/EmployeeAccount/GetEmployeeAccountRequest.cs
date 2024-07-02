namespace JobBoard.Model.EmployeeAccount
{
    public class GetEmployeeAccountRequest : PagedRequest
    {
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
