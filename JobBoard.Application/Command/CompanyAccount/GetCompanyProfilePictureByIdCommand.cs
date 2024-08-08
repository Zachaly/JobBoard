using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using MediatR;

namespace JobBoard.Application.Command
{
    public record GetCompanyProfilePictureByIdCommand(long Id) : IRequest<FileStream>;

    public class GetCompanyProfilePictureByIdHandler : IRequestHandler<GetCompanyProfilePictureByIdCommand, FileStream>
    {
        private readonly IFileService _fileService;
        private readonly ICompanyAccountRepository _repository;

        public GetCompanyProfilePictureByIdHandler(IFileService fileService, ICompanyAccountRepository repository)
        {
            _fileService = fileService;
            _repository = repository;
        }

        public async Task<FileStream> Handle(GetCompanyProfilePictureByIdCommand request, CancellationToken cancellationToken)
        {
            var fileName = (await _repository.GetEntityByIdAsync(request.Id))?.ProfilePicture;

            if(fileName is null)
            {
                return await _fileService.GetCompanyDefaultPictureAsync();
            }

            return await _fileService.GetCompanyProfilePictureAsync(fileName);
        }
    }
}
