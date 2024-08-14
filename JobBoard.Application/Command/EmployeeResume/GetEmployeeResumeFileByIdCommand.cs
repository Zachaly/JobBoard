using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using MediatR;

namespace JobBoard.Application.Command
{
    public record GetEmployeeResumeFileByIdCommand(long Id) : IRequest<(Stream? File, string Name)>;

    public class GetEmployeeResumeFileByIdHandler : IRequestHandler<GetEmployeeResumeFileByIdCommand, (Stream? File, string Name)>
    {
        private readonly IEmployeeResumeRepository _repository;
        private readonly IFileService _fileService;

        public GetEmployeeResumeFileByIdHandler(IEmployeeResumeRepository repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<(Stream? File, string Name)> Handle(GetEmployeeResumeFileByIdCommand request, CancellationToken cancellationToken)
        {
            var resume = await _repository.GetEntityByIdAsync(request.Id);

            if(resume is null)
            {
                return new (null, string.Empty);
            }

            return new (await _fileService.GetEmployeeResumeFileAsync(resume.FileName), resume.Name);
        }
    }
}
