using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferTag;

namespace JobBoard.Application.Command
{
    public class GetJobOfferTagCommand : GetJobOfferTagRequest, IGetEntityCommand<JobOfferTagModel>
    {
    }

    public class GetJobOfferTagHandler : GetEntityHandler<JobOfferTagModel, GetJobOfferTagRequest, GetJobOfferTagCommand>
    {
        public GetJobOfferTagHandler(IJobOfferTagRepository repository) : base(repository)
        {
        }
    }
}
