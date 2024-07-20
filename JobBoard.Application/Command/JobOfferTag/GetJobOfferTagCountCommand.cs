using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferTag;

namespace JobBoard.Application.Command
{
    public class GetJobOfferTagCountCommand : GetJobOfferTagRequest, IGetCountCommand
    {
    }

    public class GetJobOfferTagCountHandler : GetCountHandler<JobOfferTagModel, GetJobOfferTagRequest, GetJobOfferTagCountCommand>
    {
        public GetJobOfferTagCountHandler(IJobOfferTagRepository repository) : base(repository)
        {
        }
    }
}
