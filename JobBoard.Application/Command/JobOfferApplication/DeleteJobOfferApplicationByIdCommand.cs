using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public record DeleteJobOfferApplicationByIdCommand(long Id) : IRequest<ResponseModel>
    {
    }

    public class DeleteJobOfferApplicationByIdHandler : IRequestHandler<DeleteJobOfferApplicationByIdCommand, ResponseModel>
    {
        private readonly IJobOfferApplicationRepository _repository;
        private readonly IFileService _fileService;

        public DeleteJobOfferApplicationByIdHandler(IJobOfferApplicationRepository repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<ResponseModel> Handle(DeleteJobOfferApplicationByIdCommand request, CancellationToken cancellationToken)
        {
            var application = await _repository.GetEntityByIdAsync(request.Id);

            if(application is null)
            {
                return new ResponseModel();
            }

            await _fileService.DeleteResumeFileAsync(application.ResumeFileName);

            await _repository.DeleteByIdAsync(request.Id);

            return new ResponseModel();
        }
    }
}
