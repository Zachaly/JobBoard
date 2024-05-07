namespace JobBoard.Model.EmployeeAccount
{
    public record AddEmployeeAccountRequest
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
        public string? City { get; init; }
        public string? Country { get; init; }
        public string? AboutMe { get; init; }
    }
}
