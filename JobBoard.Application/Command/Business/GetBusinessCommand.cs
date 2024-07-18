using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Business;

namespace JobBoard.Application.Command
{
    public class GetBusinessCommand : GetBusinessRequest, IGetEntityCommand<BusinessModel>
    {
    }

    public class GetBusinessHandler : GetEntityHandler<BusinessModel, GetBusinessRequest, GetBusinessCommand>
    {
        public GetBusinessHandler(IBusinessRepository repository) : base(repository)
        {
        }
    }
}
