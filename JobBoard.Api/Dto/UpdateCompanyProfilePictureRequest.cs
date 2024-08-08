using JobBoard.Application.Command;

namespace JobBoard.Api.Dto
{
    public class UpdateCompanyProfilePictureRequest
    {
        public long CompanyId { get; set; }
        public IFormFile? Picture { get; set; }

        public UpdateCompanyAccountProfilePictureCommand ToCommand()
        {
            var mime = Picture?.FileName.Split('.').LastOrDefault();

            return new UpdateCompanyAccountProfilePictureCommand(CompanyId, Picture?.OpenReadStream(), mime);
        }
    }
}
