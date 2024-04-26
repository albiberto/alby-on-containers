using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Infrastructure.Repositories;

public class DescriptionsRepository(ProductContext context) : IDescriptionRepository
{
    public IUnitOfWork UnitOfWork { get; } = context;
    
    public Task<List<DescriptionType>> GetAllAsync() => context.DescriptionTypes
        .Include(type => type.Categories)
        .Include(type => type.DescriptionValues)
        .OrderByDescending(type => type.Name).ToListAsync();

    public async Task<DescriptionType> AddAsync(string? name = default, string? description = default)
    {
        var descriptionType = new DescriptionType(name ?? string.Empty, description ?? string.Empty);
        var entity = await context.DescriptionTypes.AddAsync(descriptionType);

        return entity.Entity;
    }
    
    public async Task UpdateAsync(Guid id, string name, string description)
    {
        var current = await context.DescriptionTypes.FindAsync(id);

        if (current is null) throw new ArgumentException("DescriptionType not found!");
            
        current.Update(name, description);
    }

    public async Task DeleteAsync(Guid id)
    {
        var current = await context.DescriptionTypes.FindAsync(id);
        
        if (current is null) throw new ArgumentException("DescriptionType not found!");
        
        context.DescriptionTypes.Remove(current);
    }
    
    public async Task<DescriptionValue> AddValueAsync(string name, string description, Guid descriptionTypeId)
    {
        var value = new DescriptionValue(name, description, descriptionTypeId);
        await context.DescriptionValues.AddAsync(value);
        
        return value;
    }
    
    public async Task Clear(Guid id)
    {
        var entry = await GetEntityAsync(id);

        if (entry.State == EntityState.Deleted)
            entry.State = EntityState.Unchanged;
        else
            entry.CurrentValues.SetValues(entry.OriginalValues);
    }

    public void Clear() => context.ChangeTracker.Clear();
    
    public bool HasChanges => context.ChangeTracker.Entries().Any(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
    public async Task<EntityState> GetStateAsync(Guid id)
    {
        var entity = await GetEntityAsync(id);

        return entity.State;
    }
    
    
    async Task<EntityEntry<DescriptionType>> GetEntityAsync(Guid id)
    {
        var current = await context.DescriptionTypes.FindAsync(id);

        if(current is null) throw new ArgumentException("DescriptionType not found!"); 
        
        return context.DescriptionTypes.Entry(current);
    }
}