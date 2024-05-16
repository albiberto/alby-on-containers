using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public interface IDescriptionRepository : IRepository<DescriptionType>
{
    Task<List<DescriptionType>> GetAllAsync();
    Task<DescriptionType> AddAsync(string? name = default, string? description = default, bool mandatory = false);
    Task UpdateAsync(Guid id, string name, string description, bool mandatory);
    Task DeleteAsync(Guid id);
    Task<DescriptionValue> AddValueAsync(Guid typeId, string? value = default, string? description = default);
    Task UpdateValueAsync(Guid id, string name, string description);
    Task DeleteValueAsync(Guid id);
    Task<DescriptionTypeCategory> AddCategoryAsync(Guid typeId, Guid categoryId);
    Task RemoveCategoryAsync(Guid id);
}