using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Infrastructure;

public class ProductContext(DbContextOptions<ProductContext> options, IEnumerable<IInterceptor> interceptors) : DbContext(options), IUnitOfWork
{
    public DbSet<Category> Categories { get; private set; } = null!;
    public DbSet<DescriptionType> DescriptionTypes { get; private set; } = null!;
    public DbSet<DescriptionValue> DescriptionValues { get; private set; } = null!;
    public DbSet<DescriptionTypeCategory> DescriptionTypesCategories { get; private set; } = null!;
    public DbSet<AttributeCluster> AttributeCluster { get; private set; } = null!;
    public DbSet<AttributeType> AttributeType { get; private set; } = null!;

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
        
        modelBuilder
            .Entity<DescriptionType>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder
            .Entity<DescriptionType>()
            .Property(type => type.Name)
            .HasMaxLength(30);
        
        modelBuilder
            .Entity<DescriptionType>()
            .Property(type => type.Description)
            .HasMaxLength(100);
    }
}