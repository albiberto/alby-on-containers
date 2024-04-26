using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Infrastructure.Repositories;

public class CategoryRepository(ProductContext context) : ICategoryRepository
{
    public IUnitOfWork UnitOfWork { get; } = context;
    
    public Task<List<Category>> GetAllAsync() => context.Categories
        .OrderByDescending(category => category.Name)
        .ToListAsync();

    public async Task<Category> AddAsync(string name, string description, Guid? parentId = default)
    {
        var category = new Category(name, description, parentId);
        var entity = await context.Categories.AddAsync(category);

        return entity.Entity;
    }

    public async Task UpdateAsync(Guid id, string name, string description, Guid? parentId = default)
    {
        var current = await context.Categories.FindAsync(id);

        if (current is null) throw new ArgumentException("Category not found!");
            
        current.Update(name, description, parentId);
    }

    public async Task DeleteAsync(Guid id)
    {
        var current = await context.Categories.FindAsync(id);
        
        if (current is null) throw new ArgumentException("Category not found!");
        
        context.Categories.Remove(current);
    }
}