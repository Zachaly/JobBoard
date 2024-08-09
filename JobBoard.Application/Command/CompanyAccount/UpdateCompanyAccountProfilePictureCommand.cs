using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public record UpdateCompanyAccountProfilePictureCommand(long CompanyId, Stream? Picture, string? PictureMimeType) : IRequest<ResponseModel>;

    public class UpdateCompanyAccountProfilePictureHandler : IRequestHandler<UpdateCompanyAccountProfilePictureCommand, ResponseModel>
    {
        private readonly IFileService _fileService;
        private readonly ICompanyAccountRepository _repository;
        private static readonly string[] ValidTypes = ["png", "jpg"];

        public UpdateCompanyAccountProfilePictureHandler(ICompanyAccountRepository repository, IFileService fileService)
        {
            _fileService = fileService;
            _repository = repository;
        }

        public async Task<ResponseModel> Handle(UpdateCompanyAccountProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var company = await _repository.GetEntityByIdAsync(request.CompanyId);

            if (company is null)
            {
                request.Picture?.Close();
                return new ResponseModel("No company with specified id");
            }

            if(request.PictureMimeType is not null && !ValidTypes.Contains(request.PictureMimeType))
            {
                request.Picture?.Close();
                return new ResponseModel("Invalid file mime type");
            }

            await _fileService.DeleteCompanyProfilePictureAsync(company.ProfilePicture);

            string? fileName = null;

            if(request.Picture is not null)
            {
                fileName = await _fileService.SaveCompanyProfilePictureAsync(request.Picture);
            }

            company.ProfilePicture = fileName;

            await _repository.UpdateAsync(company);

            request.Picture?.Close();

            return new ResponseModel();
        }
    }
}
