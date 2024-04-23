namespace ProductDataManager.Infrastructure;

using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class ProductContext(DbContextOptions<ProductContext> options, IEnumerable<IInterceptor> interceptors) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<AttrType> AttrTypes { get; set; } = null!;
    public DbSet<CategoryAttrType> CategoryAttrTypes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(interceptors);
        
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<CategoryAttrType>()
            .HasKey(c => new { c.CategoryId, c.TypeId });
    }
}