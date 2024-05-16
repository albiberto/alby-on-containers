using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.ProductAggregate;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllAsync();

    Task<Product> AddAsync(string? name = default, string? description = default, Guid? categoryId = default);
    Task UpdateAsync(Guid id, string name, string description, Guid? categoryId);
    Task DeleteAsync(Guid id);
}