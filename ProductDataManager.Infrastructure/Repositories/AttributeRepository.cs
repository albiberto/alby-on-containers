using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Infrastructure.Repositories;

public class AttributeRepository(ProductContext context) : IAttributeRepository
{
    public IUnitOfWork UnitOfWork { get; } = context;

    public Task<List<AttributeCluster>> GetAllAsync() => context.AttributeCluster.Include(cluster => cluster).ToListAsync();

    public async Task<AttributeCluster> AddAsync(string? name = default, string? description = default)
    {
        var cluster = new AttributeCluster(name ?? string.Empty, description ?? string.Empty);
        var entity = await context.AttributeCluster.AddAsync(cluster);

        return entity.Entity;
    }
    
    public async Task UpdateAsync(Guid id, string name, string description)
    {
        var current = await context.AttributeCluster.FindAsync(id);

        if (current is null) throw new ArgumentException("Cluster not found!");
            
        current.Update(name, description);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var current = await context.AttributeCluster.FindAsync(id);
        
        if (current is null) throw new ArgumentException("Cluster not found!");
        
        context.AttributeCluster.Remove(current);
    }
    
    public async Task<AttributeType> AddTypeAsync(Guid clusterId, string? name = default, string? description = default)
    {
        var type = new AttributeType(name ?? string.Empty, description ?? string.Empty, clusterId);
        var entity = await context.AttributeType.AddAsync(type);

        return entity.Entity;
    }
    
    public async Task UpdateTypeAsync(Guid id, string name, string description)
    {
        var current = await context.AttributeType.FindAsync(id);

        if (current is null) throw new ArgumentException("Type not found!");
            
        current.Update(name, description);
    }
    
    public async Task DeleteTypeAsync(Guid id)
    {
        var current = await context.AttributeType.FindAsync(id);
        
        if (current is null) throw new ArgumentException("Type not found!");
        
        context.AttributeType.Remove(current);
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