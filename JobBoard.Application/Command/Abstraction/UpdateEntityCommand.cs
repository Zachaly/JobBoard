using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain;
using JobBoard.Model;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command.Abstraction
{
    public interface IUpdateEntityCommand : IRequest<ResponseModel>
    {
    }

    public abstract class UpdateEntityHandler<TEntity, TUpdateRequest, TCommand> : IRequestHandler<TCommand, ResponseModel>
        where TEntity : IEntity
        where TCommand : IRequest<ResponseModel>, TUpdateRequest
        where TUpdateRequest : IUpdateRequest
    {
        private readonly IUpdateRepositoryBase<TEntity> _repository;
        private readonly IUpdateFactory<TEntity, TUpdateRequest> _factory;
        private readonly IValidator<TUpdateRequest> _validator;

        protected UpdateEntityHandler(IUpdateRepositoryBase<TEntity> repository, IUpdateFactory<TEntity, TUpdateRequest> factory,
            IValidator<TUpdateRequest> validator)
        {
            _repository = repository;
            _factory = factory;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var entity = await _repository.GetEntityByIdAsync(request.Id);

            if (entity is null)
            {
                return new ResponseModel("Entity not found");
            }

            _factory.Update(entity, request);

            await _repository.UpdateAsync(entity);

            return new ResponseModel();
        }
    }
}
