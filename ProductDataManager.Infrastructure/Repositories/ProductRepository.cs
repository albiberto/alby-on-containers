using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.ProductAggregate;

namespace ProductDataManager.Infrastructure.Repositories;

public class ProductRepository(ProductContext context) : RepositoryBase(context), IProductRepository
{
    public Task<List<Product>> GetAllAsync()
    {
        return context.Products
            .Include(product => product.Category)
            .ThenInclude(join => join!.DescriptionTypesCategories)
            .ThenInclude(join => join.DescriptionType)
            .ThenInclude(type => type!.DescriptionValues)
            .Include(product => product.ProductsAttributes)
            .ThenInclude(join => join.Attribute)
            .ToListAsync();
    }
    
    public async Task<Product> AddAsync(string? name = default, string? description = default, Guid? categoryId = default)
    {
        var descriptionType = new Product(name ?? string.Empty, description ?? string.Empty, categoryId ?? Guid.Empty);
        var entity = await context.Products.AddAsync(descriptionType);

        return entity.Entity;
    }
    
    public async Task UpdateAsync(Guid id, string name, string description, Guid? categoryId)
    {
        var current = await context.Products.FindAsync(id);

        if (current is null) throw new ArgumentException("Product not found!");
            
        current.Update(name, description, categoryId);
    }

    public async Task DeleteAsync(Guid id)
    {
        var current = await context.Products.FindAsync(id);
        
        if (current is null) throw new ArgumentException("Product not found!");
        
        context.Products.Remove(current);
    }
}