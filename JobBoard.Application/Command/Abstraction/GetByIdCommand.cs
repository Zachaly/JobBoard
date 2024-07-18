using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model;
using MediatR;

namespace JobBoard.Application.Command.Abstraction
{
    public record GetByIdCommand<TModel>(long Id) : IRequest<TModel?>;

    public abstract class GetByIdHandler<TModel, TGetRequest, TCommand> : IRequestHandler<TCommand, TModel?>
        where TCommand : GetByIdCommand<TModel>
        where TGetRequest : PagedRequest
        where TModel : class
    {
        private readonly IGetRepositoryBase<TModel, TGetRequest> _repository;

        protected GetByIdHandler(IGetRepositoryBase<TModel, TGetRequest> repository)
        {
            _repository = repository;
        }

        public Task<TModel?> Handle(TCommand request, CancellationToken cancellationToken)
            => _repository.GetByIdAsync(request.Id);
    }
}
