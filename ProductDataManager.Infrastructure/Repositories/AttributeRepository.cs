using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;
using ProductDataManager.Domain.SeedWork;
using Attribute = ProductDataManager.Domain.Aggregates.AttributeAggregate.Attribute;

namespace ProductDataManager.Infrastructure.Repositories;

public class AttributeRepository(ProductContext context) : IAttributeRepository
{
    public IUnitOfWork UnitOfWork { get; } = context;

    public Task<List<AttributeType>> GetAllAsync() => context.AttributeType.Include(type => type.Attributes).ToListAsync();

    public async Task<AttributeType> AddAttributeTypeAsync(string? name = default, string? description = default)
    {
        var type = new AttributeType(name ?? string.Empty, description ?? string.Empty);
        var entity = await context.AttributeType.AddAsync(type);

        return entity.Entity;
    }
    
    public async Task UpdateAttributeTypeAsync(Guid id, string name, string description)
    {
        var current = await context.AttributeType.FindAsync(id);

        if (current is null) throw new ArgumentException("Attribute type not found!");
            
        current.Update(name, description);
    }
    
    public async Task DeleteAttributeTypeAsync(Guid id)
    {
        var current = await context.AttributeType.FindAsync(id);
        
        if (current is null) throw new ArgumentException("Attribute type not found!");
        
        context.AttributeType.Remove(current);
    }
    
    public async Task<Attribute> AddAttributeAsync(Guid typeId, string? name = default, string? description = default)
    {
        var type = new Attribute(name ?? string.Empty, description ?? string.Empty, typeId);
        var entity = await context.Attribute.AddAsync(type);

        return entity.Entity;
    }
    
    public async Task UpdateAttributeAsync(Guid id, string name, string description)
    {
        var current = await context.Attribute.FindAsync(id);

        if (current is null) throw new ArgumentException("Type not found!");
            
        current.Update(name, description);
    }
    
    public async Task DeleteAttributeAsync(Guid id)
    {
        var current = await context.Attribute.FindAsync(id);
        
        if (current is null) throw new ArgumentException("Type not found!");
        
        context.Attribute.Remove(current);
    }
        
    public async Task Clear<T>(Guid id) where T : Entity
    {
        var current = await context.FindAsync<T>(id);

        if(current is null) throw new ArgumentException("Entity not found!"); 
        
        var entry =  context.Entry(current);

        if (entry.State != EntityState.Deleted) entry.CurrentValues.SetValues(entry.OriginalValues);
        entry.State = EntityState.Unchanged;            
    }

    public void Clear() => context.ChangeTracker.Clear();
    
    public bool HasChanges => context.ChangeTracker.Entries().Any(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
}