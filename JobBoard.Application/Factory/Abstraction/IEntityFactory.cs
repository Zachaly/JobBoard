namespace JobBoard.Application.Factory.Abstraction
{
    public interface ICreateFactory<out TEntity, in TAddRequest>
    {
        TEntity Create(TAddRequest request);
    }

    public interface IUpdateFactory<in TEntity, in TUpdateRequest> 
    {
        void Update(TEntity entity, TUpdateRequest request);
    }


    public interface IEntityFactory<TEntity, TAddRequest, TUpdateRequest> : ICreateFactory<TEntity, TAddRequest>,
        IUpdateFactory<TEntity, TUpdateRequest>
    {
    }
}
