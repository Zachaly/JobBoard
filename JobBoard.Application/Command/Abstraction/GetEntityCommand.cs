using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model;
using MediatR;

namespace JobBoard.Application.Command.Abstraction
{
    public interface IGetEntityCommand<TModel> : IRequest<IEnumerable<TModel>>
    {
    }

    public abstract class GetEntityHandler<TModel, TGetRequest, TCommand> : IRequestHandler<TCommand, IEnumerable<TModel>>
        where TCommand : IRequest<IEnumerable<TModel>>, TGetRequest
        where TGetRequest : PagedRequest
        where TModel : class
    {
        private readonly IGetRepositoryBase<TModel, TGetRequest> _repository;

        protected GetEntityHandler(IGetRepositoryBase<TModel, TGetRequest> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<TModel>> Handle(TCommand request, CancellationToken cancellationToken)
            => _repository.GetAsync(request);
    }
}
