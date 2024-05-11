using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.AttributeAggregate;

public interface IAttributeRepository : IRepository<AttributeType>
{
    Task<List<AttributeType>> GetAllAsync();
    Task<AttributeType> AddAttributeTypeAsync(string? name = default, string? description = default);
    Task UpdateAttributeTypeAsync(Guid id, string name, string description);
    Task DeleteAttributeTypeAsync(Guid id);
    Task<Attribute> AddAttributeAsync(Guid typeId, string? name = default, string? description = default );
    Task UpdateAttributeAsync(Guid id, string name, string description);
    Task DeleteAttributeAsync(Guid id);
    Task Clear<T>(Guid id) where T : Entity;
    void Clear();
    public bool HasChanges { get; }
}