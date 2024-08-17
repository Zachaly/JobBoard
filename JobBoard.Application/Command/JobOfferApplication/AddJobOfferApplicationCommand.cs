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
        public Stream? Resume { get; set; }
        public string? ResumeMimeType { get; set; }
        public long? ResumeId { get; set; }
    }

    public class AddJobOfferApplicationHandler : IRequestHandler<AddJobOfferApplicationCommand, ResponseModel>
    {
        private readonly IJobOfferApplicationRepository _jobOfferApplicationRepository;
        private readonly IJobOfferApplicationFactory _factory;
        private readonly IFileService _fileService;
        private readonly IEmployeeResumeRepository _employeeResumeRepository;

        public AddJobOfferApplicationHandler(IJobOfferApplicationRepository jobOfferApplicationRepository, IJobOfferApplicationFactory factory,
            IFileService fileService, IEmployeeResumeRepository employeeResumeRepository)
        {
            _jobOfferApplicationRepository = jobOfferApplicationRepository;
            _factory = factory;
            _fileService = fileService;
            _employeeResumeRepository = employeeResumeRepository;
        }

        public async Task<ResponseModel> Handle(AddJobOfferApplicationCommand request, CancellationToken cancellationToken)
        {
            string file = null;

            if(request.Resume is not null)
            {
                if(request.ResumeMimeType != "pdf")
                {
                    return new ResponseModel("File must be pdf");
                }

                file = await _fileService.SaveResumeFileAsync(request.Resume);
                request.Resume.Close();
            } 
            else if(request.ResumeId is not null)
            {
                var resume = await _employeeResumeRepository.GetEntityByIdAsync(request.ResumeId.Value);

                if(resume is null)
                {
                    return new ResponseModel("Resume not found");
                }

                file = await _fileService.CopyEmployeeResumeToApplicationAsync(resume.FileName);
            }

            if(file is null)
            {
                return new ResponseModel("No file or resume attached");
            }

            var application = _factory.Create(request, file);

            await _jobOfferApplicationRepository.AddAsync(application);

            return new ResponseModel();
        }
    }
}
