using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferApplication;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class AddJobOfferApplicationCommand : AddJobOfferApplicationRequest, IRequest<ResponseModel>
    {
        public Stream Resume { get; set; }
        public string ResumeMimeType { get; set; }
    }

    public class AddJobOfferApplicationHandler : IRequestHandler<AddJobOfferApplicationCommand, ResponseModel>
    {
        private readonly IJobOfferApplicationRepository _repository;
        private readonly IJobOfferApplicationFactory _factory;
        private readonly IFileService _fileService;

        public AddJobOfferApplicationHandler(IJobOfferApplicationRepository repository, IJobOfferApplicationFactory factory,
            IFileService fileService)
        {
            _repository = repository;
            _factory = factory;
            _fileService = fileService;
        }

        public async Task<ResponseModel> Handle(AddJobOfferApplicationCommand request, CancellationToken cancellationToken)
        {
            if(request.ResumeMimeType != "pdf")
            {
                return new ResponseModel("File must be pdf");
            }

            var file = await _fileService.SaveResumeFileAsync(request.Resume);

            var application = _factory.Create(request, file);

            await _repository.AddAsync(application);

            request.Resume.Close();

            return new ResponseModel();
        }
    }
}
