using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public record DeleteEmployeeResumeByIdCommand(long Id) : IRequest<ResponseModel>;

    public class DeleteEmployeeResumeByIdHandler : IRequestHandler<DeleteEmployeeResumeByIdCommand, ResponseModel>
    {
        private readonly IFileService _fileService;
        private readonly IEmployeeResumeRepository _repository;

        public DeleteEmployeeResumeByIdHandler(IEmployeeResumeRepository repository, IFileService fileService)
        {
            _fileService = fileService;
            _repository = repository;
        }

        public async Task<ResponseModel> Handle(DeleteEmployeeResumeByIdCommand request, CancellationToken cancellationToken)
        {
            var resume = await _repository.GetEntityByIdAsync(request.Id);

            if(resume is null)
            {
                return new ResponseModel();
            }

            await _fileService.DeleteEmployeeResumeFileAsync(resume.FileName);

            await _repository.DeleteByIdAsync(request.Id);

            return new ResponseModel();
        }
    }
}
