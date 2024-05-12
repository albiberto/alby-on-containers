using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.ProductAggregate;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllAsync();
}