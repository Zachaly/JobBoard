namespace JobBoard.Model.JobOfferRequirement
{
    public class UpdateJobOfferRequirementRequest : IUpdateRequest
    {
        public long Id { get; set; }
        public string Content { get; set; }
    }
}
