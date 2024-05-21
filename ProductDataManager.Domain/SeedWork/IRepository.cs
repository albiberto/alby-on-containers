using Ardalis.Specification;

namespace ProductDataManager.Domain.SeedWork;

public interface IRepository<T> : IRepositoryBase<T> where T: class, IAggregateRoot
{
    Task Clear<TEntity>(Guid id) where TEntity : Entity;
    void Clear();
    public bool HasChanges { get; }
}