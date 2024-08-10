using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using MediatR;

namespace JobBoard.Application.Command
{
    public record GetJobOfferApplicationResumeByIdCommand(long Id) : IRequest<Stream?>;

    public class GetJobOfferApplicationResumeByIdHandler : IRequestHandler<GetJobOfferApplicationResumeByIdCommand, Stream?>
    {
        private readonly IJobOfferApplicationRepository _repository;
        private readonly IFileService _fileService;

        public GetJobOfferApplicationResumeByIdHandler(IJobOfferApplicationRepository repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<Stream?> Handle(GetJobOfferApplicationResumeByIdCommand request, CancellationToken cancellationToken)
        {
            var application = await _repository.GetEntityByIdAsync(request.Id);

            if(application is null)
            {
                return null;
            }

            return await _fileService.GetResumeFile(application.ResumeFileName);
        }
    }
}
