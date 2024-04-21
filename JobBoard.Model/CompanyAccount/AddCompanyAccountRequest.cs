namespace JobBoard.Model.CompanyAccount
{
    public record class AddCompanyAccountRequest(
        string Email,
        string Password,
        string Name,
        string City,
        string PostalCode,
        string Address,
        string Country,
        string ContactEmail
        );
}
