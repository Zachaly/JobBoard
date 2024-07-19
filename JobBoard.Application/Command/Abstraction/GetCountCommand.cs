using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model;
using MediatR;

namespace JobBoard.Application.Command.Abstraction
{
    public interface IGetCountCommand : IRequest<int>
    {
    }

    public abstract class GetCountHandler<TModel, TGetRequest, TCommand> : IRequestHandler<TCommand, int>
        where TCommand : IGetCountCommand, TGetRequest
        where TModel : class
        where TGetRequest : PagedRequest
    {
        private readonly IGetRepositoryBase<TModel, TGetRequest> _repository;

        protected GetCountHandler(IGetRepositoryBase<TModel, TGetRequest> repository)
        {
            _repository = repository;
        }

        public Task<int> Handle(TCommand request, CancellationToken cancellationToken)
            => _repository.GetCountAsync(request);
    }
}
