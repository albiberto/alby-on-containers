using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Infrastructure.Repositories;

public abstract class Repository<T>(DbContext context) : RepositoryBase<T>(context) where T : class, IAggregateRoot
{
    public override async Task<T> AddAsync(T entity, CancellationToken cancellationToken = new())
    {
        var result = await context.Set<T>().AddAsync(entity, cancellationToken);
        return result.Entity;
    }

    public override Task UpdateAsync(T entity, CancellationToken _ = new())
    {
        context.Set<T>().Update(entity);
        
        return Task.CompletedTask;
    }

    public override Task DeleteAsync(T entity, CancellationToken _ = new())
    {
        context.Set<T>().Remove(entity);

        return Task.CompletedTask;
    }
    
    public async Task Clear<TCurrent>(Guid id) where TCurrent : Entity
    {
        var current = await context.FindAsync<TCurrent>(id);

        if(current is null) throw new ArgumentException("Entity not found!"); 
        
        var entry = context.Entry(current);

        if (entry.State != EntityState.Deleted) entry.CurrentValues.SetValues(entry.OriginalValues);
        entry.State = EntityState.Unchanged;            
    }

    public void Clear() => context.ChangeTracker.Clear();
    
    public bool HasChanges => context.ChangeTracker.Entries().Any(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
}