using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.CategoryAggregate;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetAllAsync();
    Task<Category> AddAsync(string name, string description, Guid? parentId = default);
    Task UpdateAsync(Guid id, string name, string description, Guid? parentId = default);
    Task DeleteAsync(Guid id);
}