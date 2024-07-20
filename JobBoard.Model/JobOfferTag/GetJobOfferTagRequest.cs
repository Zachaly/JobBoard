namespace JobBoard.Model.JobOfferTag
{
    public class GetJobOfferTagRequest : PagedRequest
    {
        public string? Tag { get; set; }
        public long? OfferId { get; set; }
    }
}
