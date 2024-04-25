using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ProductDataManager.Infrastructure;

public class ProductContext(DbContextOptions<ProductContext> options, IEnumerable<IInterceptor> interceptors) : DbContext(options), IUnitOfWork
{
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(interceptors);
        
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();
    }
}