using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public interface IDescriptionRepository : IRepository<DescriptionType>
{
    Task<List<DescriptionType>> GetAllAsync();
    Task<DescriptionType> AddAsync(string? name = default, string? description = default);
    Task UpdateAsync(Guid id, string name, string description);
    Task DeleteAsync(Guid id);
    Task<DescriptionValue> AddValueAsync(string name, string description, Guid descriptionTypeId);
    Task Clear(Guid id);
    void Clear();
    bool HasChanges { get; }
    Task<EntityState> GetStateAsync(Guid id);
}