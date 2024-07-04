namespace JobBoard.Model.JobOffer
{
    public class GetJobOfferRequest : PagedRequest
    {
        public long? CompanyId { get; set; }
        public string? Location { get; set; }
    }
}
