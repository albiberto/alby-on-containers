using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.AttributeAggregate;

public interface IAttributeRepository : IRepository<AttributeCluster>
{
    Task<List<AttributeCluster>> GetAllAsync();
    Task<AttributeCluster> AddAsync(string? name = default, string? description = default);
    Task UpdateAsync(Guid id, string name, string description);
    Task DeleteAsync(Guid id);
    Task<AttributeType> AddTypeAsync(Guid clusterId, string? name = default, string? description = default );
    Task UpdateTypeAsync(Guid id, string name, string description);
    Task DeleteTypeAsync(Guid id);
    Task Clear<T>(Guid id) where T : Entity;
    void Clear();
    public bool HasChanges { get; }
}