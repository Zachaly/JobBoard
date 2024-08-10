using JobBoard.Domain.Enum;

namespace JobBoard.Model.JobOfferApplication
{
    public class UpdateJobOfferApplicationRequest : IUpdateRequest
    {
        public long Id { get; set; }
        public JobOfferApplicationState State { get; set; }
    }
}
