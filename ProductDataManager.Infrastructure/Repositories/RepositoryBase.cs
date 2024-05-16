using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Infrastructure.Repositories;

public abstract class RepositoryBase(ProductContext context)
{
    public IUnitOfWork UnitOfWork { get; } = context;

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