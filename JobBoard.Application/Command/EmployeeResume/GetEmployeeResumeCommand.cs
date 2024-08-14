using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.EmployeeResume;

namespace JobBoard.Application.Command
{
    public class GetEmployeeResumeCommand : GetEmployeeResumeRequest, IGetEntityCommand<EmployeeResumeModel>
    {
    }

    public class GetEmployeeResumeHandler : GetEntityHandler<EmployeeResumeModel, GetEmployeeResumeRequest, GetEmployeeResumeCommand>
    {
        public GetEmployeeResumeHandler(IEmployeeResumeRepository repository) : base(repository)
        {
        }
    }
}
