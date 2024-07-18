using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command.Abstraction
{
    public record DeleteEntityByIdCommand(long Id) : IRequest<ResponseModel>
    {
    }

    public abstract class DeleteEntityByIdHandler<TEntity, TCommand> : IRequestHandler<TCommand, ResponseModel>
        where TCommand : DeleteEntityByIdCommand
        where TEntity : IEntity
    {
        private readonly IUpdateRepositoryBase<TEntity> _repository;

        protected DeleteEntityByIdHandler(IUpdateRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<ResponseModel> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.DeleteByIdAsync(request.Id);

                return new ResponseModel();
            }
            catch(System.Exception ex)
            {
                return new ResponseModel(ex.Message);
            }
        }
    }
}
