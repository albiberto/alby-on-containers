namespace AlbyOnContainers.ProductDataManager.Data;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models.Abstract;

public class AuditableInterceptor : SaveChangesInterceptor 
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context!;
        
        foreach (var entry in context.ChangeTracker.Entries<Auditable>())
        {
            switch (entry)
            {
                case { State: EntityState.Added }:
                    entry.Entity.Created = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "Alberto";
                    entry.Entity.LastModified = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = "Alberto";
                    break;

                case { State: EntityState.Modified }:
                    entry.Entity.LastModified = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = "Alberto";
                    break;
            }
        }
        
        return ValueTask.FromResult(result);
    }
}