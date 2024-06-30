namespace JobBoard.Model.EmployeeAccount
{
    public class UpdateEmployeeAccountRequest
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? AboutMe { get; set; }
    }
}
