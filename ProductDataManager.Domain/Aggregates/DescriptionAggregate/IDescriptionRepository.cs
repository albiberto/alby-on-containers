using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public interface IDescriptionRepository : IRepository<DescriptionType>
{
    Task<List<DescriptionType>> GetAllAsync();
    Task<DescriptionType> AddAsync(string? name = default, string? description = default);
    Task UpdateAsync(Guid id, string name, string description);
    Task DeleteAsync(Guid id);
    Task Clear<T>(Guid id) where T : Entity;
    void Clear();
    public bool HasChanges { get; }
    Task<EntityState> GetStateAsync<T>(Guid id) where T : Entity;
    Task<DescriptionValue> AddValueAsync(Guid typeId, string? value = default, string? description = default);
    Task UpdateValueAsync(Guid id, string name, string description);
    Task DeleteValueAsync(Guid id);
    Task<DescriptionTypeCategory> AddCategory(Guid typeId, Guid categoryId);
    Task RemoveCategory(Guid id);
}