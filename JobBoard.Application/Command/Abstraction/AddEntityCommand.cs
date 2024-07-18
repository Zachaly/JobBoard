using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command.Abstraction
{
    public interface IAddEntityCommand : IRequest<ResponseModel>
    {
    }

    public abstract class AddEntityHandler<TEntity, TAddRequest, TCommand> : IRequestHandler<TCommand, ResponseModel>
        where TCommand : IAddEntityCommand, TAddRequest
        where TEntity : IEntity
    {
        private readonly IUpdateRepositoryBase<TEntity> _repository;
        private readonly ICreateFactory<TEntity, TAddRequest> _factory;
        private readonly IValidator<TAddRequest> _validator;

        protected AddEntityHandler(IUpdateRepositoryBase<TEntity> repository, ICreateFactory<TEntity, TAddRequest> factory,
            IValidator<TAddRequest> validator)
        {
            _repository = repository;
            _factory = factory;
            _validator = validator;
        }

        public virtual async Task<ResponseModel> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if (!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var entity = _factory.Create(request);

            await _repository.AddAsync(entity);

            return new ResponseModel();
        }
    }
}
