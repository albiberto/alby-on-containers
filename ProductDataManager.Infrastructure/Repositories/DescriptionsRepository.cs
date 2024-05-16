using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Infrastructure.Repositories;

public class DescriptionsRepository(ProductContext context) : RepositoryBase(context), IDescriptionRepository
{

    public Task<List<DescriptionType>> GetAllAsync() => context.DescriptionTypes
        .Include(type => type.DescriptionTypesCategories)
        .ThenInclude(join => join.Category)
        .Include(type => type.DescriptionValues)
        .OrderByDescending(type => type.Name).ToListAsync();

    public async Task<DescriptionType> AddAsync(string? name = default, string? description = default, bool mandatory = false)
    {
        var descriptionType = new DescriptionType(name ?? string.Empty, description ?? string.Empty, mandatory);
        var entity = await context.DescriptionTypes.AddAsync(descriptionType);

        return entity.Entity;
    }
    
    public async Task UpdateAsync(Guid id, string name, string description, bool mandatory)
    {
        var current = await context.DescriptionTypes.FindAsync(id);

        if (current is null) throw new ArgumentException("Type not found!");
            
        current.Update(name, description, mandatory);
    }

    public async Task DeleteAsync(Guid id)
    {
        var current = await context.DescriptionTypes.FindAsync(id);
        
        if (current is null) throw new ArgumentException("Type not found!");
        
        context.DescriptionTypes.Remove(current);
    }
    
    public async Task<DescriptionValue> AddValueAsync(Guid typeId, string? value = default, string? description = default)
    {
        var descriptionType = new DescriptionValue(value ?? string.Empty, description ?? string.Empty, typeId);
        var entity = await context.DescriptionValues.AddAsync(descriptionType);

        return entity.Entity;
    }
    
    public async Task UpdateValueAsync(Guid id, string name, string description)
    {
        var current = await context.DescriptionValues.FindAsync(id);

        if (current is null) throw new ArgumentException("Value not found!");
            
        current.Update(name, description);
    }

    public async Task DeleteValueAsync(Guid id)
    {
        var current = await context.DescriptionValues.FindAsync(id);
        
        if (current is null) throw new ArgumentException("Value not found!");
        
        context.DescriptionValues.Remove(current);
    }

    public async Task<DescriptionTypeCategory> AddCategoryAsync(Guid typeId, Guid categoryId)
    {
        var join = await context.DescriptionTypesCategories.AddAsync(new(typeId, categoryId));
        return join.Entity;
    }

    public async Task RemoveCategoryAsync(Guid id)
    {
        var current = await context.DescriptionTypesCategories.FindAsync(id);
        
        if (current is null) throw new ArgumentException("Join not found!");
        
        context.DescriptionTypesCategories.Remove(current);
    }
    
    
    public async Task Clear<T>(Guid id) where T : Entity
    {
        var current = await context.FindAsync<T>(id);

        if(current is null) throw new ArgumentException("Entity not found!"); 
        
        var entry = context.Entry(current);

        if (entry.State != EntityState.Deleted) entry.CurrentValues.SetValues(entry.OriginalValues);
        entry.State = EntityState.Unchanged;            
    }

    public void Clear() => context.ChangeTracker.Clear();
    
    public bool HasChanges => context.ChangeTracker.Entries().Any(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
}