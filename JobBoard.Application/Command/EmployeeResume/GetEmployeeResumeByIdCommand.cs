using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.EmployeeResume;

namespace JobBoard.Application.Command
{
    public record GetEmployeeResumeByIdCommand(long Id) : GetByIdCommand<EmployeeResumeModel>(Id);

    public class GetEmployeeResumeByIdHandler : GetByIdHandler<EmployeeResumeModel, GetEmployeeResumeRequest, GetEmployeeResumeByIdCommand>
    {
        public GetEmployeeResumeByIdHandler(IEmployeeResumeRepository repository) : base(repository)
        {
        }
    }
}
