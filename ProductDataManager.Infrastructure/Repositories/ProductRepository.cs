using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.Aggregates.ProductAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Infrastructure.Repositories;

public class ProductRepository(ProductContext productContext) : IProductRepository
{
    public IUnitOfWork UnitOfWork { get; } = productContext;
    
    public Task<List<Product>> GetAllAsync()
    {
        return productContext.Products
            .Include(product => product.Category)
            .ThenInclude(join => join.DescriptionTypesCategories)
            .ThenInclude(join => join.DescriptionType)
            .ThenInclude(type => type.DescriptionValues)
            .Include(product => product.ProductsAttributes)
            .ThenInclude(join => join.Attribute)
            .ToListAsync();
    }
}