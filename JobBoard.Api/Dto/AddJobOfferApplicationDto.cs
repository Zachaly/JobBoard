using JobBoard.Application.Command;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Api.Dto
{
    public class AddJobOfferApplicationDto : AddJobOfferApplicationRequest
    {
        public IFormFile Resume { get; set; }

        public AddJobOfferApplicationCommand ToCommand()
        {
            var mime = Resume.FileName.Split(".").LastOrDefault();

            return new AddJobOfferApplicationCommand
            {
                EmployeeId = EmployeeId,
                OfferId = OfferId,
                Resume = Resume.OpenReadStream(),
                ResumeMimeType = mime ?? ""
            };
        }
    }
}
