namespace ProductDataManager.Domain.SeedWork;

public interface IRepository<T> where T:IAggregateRoot
{
    public IUnitOfWork UnitOfWork { get; }
    
    Task Clear<TEntity>(Guid id) where TEntity : Entity;
    void Clear();
    public bool HasChanges { get; }
}